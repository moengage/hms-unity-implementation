#define ADD_APP_GROUP
#define ADD_PUSH_TEMPLATES

#if UNITY_5_4_OR_NEWER && UNITY_IPHONE && UNITY_EDITOR

using System.IO;
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Text;
using System.Collections.Generic;

#if UNITY_2017_2_OR_NEWER
using UnityEditor.iOS.Xcode.Extensions;
#endif

public static class BuildPostProcessor
{
    public static readonly string DEFAULT_PROJECT_TARGET_NAME = "Unity-iPhone";
    public static readonly string NOTIFICATION_SERVICE_EXTENSION_TARGET_NAME = "MoENotificationServiceExtension";
    public static readonly string NOTIFICATION_SERVICE_EXTENSION_OBJECTIVEC_FILENAME = "NotificationService";

    public static readonly string PUSH_TEMPLATES_EXTENSION_TARGET_NAME = "MoEPushTemplateExtension";
    public static readonly string PUSH_TEMPLATES_EXTENSION_OBJECTIVEC_FILENAME = "NotificationViewController";

    private static readonly char DIR_CHAR = Path.DirectorySeparatorChar;
    public static readonly string MOE_IOS_LOCATION = "Assets" + DIR_CHAR + "MoEngage" + DIR_CHAR + "Plugins" + DIR_CHAR + "iOS";
    public static readonly string MOE_IOS_PUSH_TEMP_LOCATION = MOE_IOS_LOCATION + DIR_CHAR + "PushTemplates";

    private static readonly string[] FRAMEWORKS_TO_ADD = {
         "Foundation.framework",
         "UIKit.framework",
         "SystemConfiguration.framework",
         "CoreGraphics.framework",
         "Security.framework",
         "ImageIO.framework",
         "NotificationCenter.framework",
         "UserNotifications.framework",
         "UserNotificationsUI.framework",
         "AppTrackingTransparency.framework",
      };

    private enum EntitlementOptions
    {
        ApsEnv,
        AppGroups
    }

    // Unity 2019.3 made large changes to the Xcode build system / API.
    // There is now two targets;
    //  * Unity-Iphone (Main)
    //  * UnityFramework
    //     - Plugins are now added to this instead of the main target
#if UNITY_2019_3_OR_NEWER
    private static string GetPBXProjectTargetName(PBXProject project)
    {
        // var projectUUID = project.GetUnityMainTargetGuid();
        // return project.GetBuildPhaseName(projectUUID);
        // The above always returns null, using a static value for now.
        return DEFAULT_PROJECT_TARGET_NAME;
    }

    private static string GetPBXProjectTargetGUID(PBXProject project)
    {
        return project.GetUnityMainTargetGuid();
    }

    private static string GetPBXProjectUnityFrameworkGUID(PBXProject project)
    {
        return project.GetUnityFrameworkTargetGuid();
    }
#else
         private static string GetPBXProjectTargetName(PBXProject project)
         {
            return PBXProject.GetUnityTargetName();
         }

         private static string GetPBXProjectTargetGUID(PBXProject project)
         { 
            return project.TargetGuidByName(PBXProject.GetUnityTargetName());
         }

         private static string GetPBXProjectUnityFrameworkGUID(PBXProject project)
         {
            return GetPBXProjectTargetGUID(project);
         }
#endif

    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        var projectPath = PBXProject.GetPBXProjectPath(path);
        var project = new PBXProject();

        project.ReadFromString(File.ReadAllText(projectPath));

        var mainTargetName = GetPBXProjectTargetName(project);
        var mainTargetGUID = GetPBXProjectTargetGUID(project);
        var unityFrameworkGUID = GetPBXProjectUnityFrameworkGUID(project);

        foreach (var framework in FRAMEWORKS_TO_ADD)
        {
            project.AddFrameworkToProject(unityFrameworkGUID, framework, false);
        }
        project.SetBuildProperty(mainTargetGUID, "GCC_ENABLE_OBJC_EXCEPTIONS", "Yes");

        AddOrUpdateEntitlements(
           path,
           project,
           mainTargetGUID,
           mainTargetName,
           new HashSet<EntitlementOptions> {
               EntitlementOptions.ApsEnv, EntitlementOptions.AppGroups
           }
        );

        // Add the NSE target to the Xcode project
        AddNotificationServiceExtension(project, path);

#if ADD_PUSH_TEMPLATES
        AddPushTemplateExtension(project, path);
#endif

        // Reload file after changes from AddNotificationServiceExtension
        project.WriteToFile(projectPath);
        var contents = File.ReadAllText(projectPath);
        project.ReadFromString(contents);

        // Add push notifications as a capability on the main app target
        AddPushCapability(project, path, mainTargetGUID, mainTargetName);

        File.WriteAllText(projectPath, project.WriteToString());

    }
    /*
     * TO DO: Try to embed frameworks with PostProcessBuildAttribute
    [PostProcessBuildAttribute(47)]//must be between 40 and 50 to ensure that it's not overriden by Podfile generation (40) and that it's added before "pod install" (50)
    public static void PostProcessBuild_iOS(BuildTarget target, string buildPath)
    {
        using (StreamWriter sw = File.AppendText(buildPath + "/Podfile"))
        {
            sw.WriteLine("\ntarget '" + NOTIFICATION_SERVICE_EXTENSION_TARGET_NAME + "' do\n  pod 'MORichNotification' \nend");
        }
    }
    */

    // Returns exisiting file if found, otherwises provides a default name to use
    private static string GetEntitlementsPath(string path, PBXProject project, string targetGUI, string targetName)
    {
        // Check if there is already an eltitlements file configured in the Xcode project
#if UNITY_2018_2_OR_NEWER
        var relativeEntitlementPath = project.GetBuildPropertyForConfig(targetGUI, "CODE_SIGN_ENTITLEMENTS");
        if (relativeEntitlementPath != null)
        {
            var entitlementPath = path + DIR_CHAR + relativeEntitlementPath;
            if (File.Exists(entitlementPath))
            {
                return entitlementPath;
            }
        }
#endif

        // No existing file, use a new name
        return path + DIR_CHAR + targetName + DIR_CHAR + targetName + ".entitlements";
    }

    private static void AddOrUpdateEntitlements(string path, PBXProject project, string targetGUI, string targetName, HashSet<EntitlementOptions> options)
    {
        string entitlementPath = GetEntitlementsPath(path, project, targetGUI, targetName);
        var entitlements = new PlistDocument();

        // Check if the file already exisits and read it
        if (File.Exists(entitlementPath))
        {
            entitlements.ReadFromFile(entitlementPath);
        }

        if (options.Contains(EntitlementOptions.ApsEnv))
        {
            if (entitlements.root["aps-environment"] == null)
                entitlements.root.SetString("aps-environment", "development");
        }

        // TOOD: This can be updated to use project.AddCapability() in the future
#if ADD_APP_GROUP
        if (options.Contains(EntitlementOptions.AppGroups) && entitlements.root["com.apple.security.application-groups"] == null)
        {
            var groups = entitlements.root.CreateArray("com.apple.security.application-groups");
            groups.AddString("group." + PlayerSettings.applicationIdentifier + ".moengage");
        }
#endif

        entitlements.WriteToFile(entitlementPath);

        // Copy the entitlement file to the xcode project
        var entitlementFileName = Path.GetFileName(entitlementPath);
        var relativeDestination = targetName + "/" + entitlementFileName;

        // Add the pbx configs to include the entitlements files on the project
        project.AddFile(relativeDestination, entitlementFileName);
        project.AddBuildProperty(targetGUI, "CODE_SIGN_ENTITLEMENTS", relativeDestination);
    }

    private static void AddPushCapability(PBXProject project, string path, string targetGUID, string targetName)
    {
        var projectPath = PBXProject.GetPBXProjectPath(path);
        project.AddCapability(targetGUID, PBXCapabilityType.PushNotifications);
        project.AddCapability(targetGUID, PBXCapabilityType.BackgroundModes);

        var entitlementsPath = GetEntitlementsPath(path, project, targetGUID, targetName);
        // NOTE: ProjectCapabilityManager's 4th constructor param requires Unity 2019.3+
        var projCapability = new ProjectCapabilityManager(projectPath, entitlementsPath, targetName);
        projCapability.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);
        projCapability.WriteToFile();
    }

    private static void AddNotificationServiceExtension(PBXProject project, string path)
    {
#if UNITY_2017_2_OR_NEWER && !UNITY_CLOUD_BUILD
        var projectPath = PBXProject.GetPBXProjectPath(path);
        var mainTargetGUID = GetPBXProjectTargetGUID(project);
        var extensionTargetName = NOTIFICATION_SERVICE_EXTENSION_TARGET_NAME;

        var exisitingPlistFile = CreateExtensionPlistFile(path, true);
        // If file exisits then the below has been completed before from another build
        // The below will not be updated on Append builds
        // Changes would most likely need to be made to support Append builds
        if (exisitingPlistFile)
            return;

        var extensionGUID = PBXProjectExtensions.AddAppExtension(
           project,
           mainTargetGUID,
           extensionTargetName,
           PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS) + "." + extensionTargetName,
           extensionTargetName + "/" + "Info.plist" // Unix path as it's used by Xcode
        );

        AddExtensionSourceFilesToTarget(project, extensionGUID, path, true);

        foreach (var framework in FRAMEWORKS_TO_ADD)
        {
            project.AddFrameworkToProject(extensionGUID, framework, true);
        }

        // Makes it so that the extension target is Universal (not just iPhone) and has an iOS 10 deployment target
        project.SetBuildProperty(extensionGUID, "TARGETED_DEVICE_FAMILY", "1,2");
        project.SetBuildProperty(extensionGUID, "IPHONEOS_DEPLOYMENT_TARGET", "10.0");

        project.SetBuildProperty(extensionGUID, "ARCHS", "$(ARCHS_STANDARD)");
        project.SetBuildProperty(extensionGUID, "DEVELOPMENT_TEAM", PlayerSettings.iOS.appleDeveloperTeamID);
        project.SetBuildProperty(extensionGUID, "GCC_ENABLE_OBJC_EXCEPTIONS", "Yes");

        project.WriteToFile(projectPath);

        //var contents = File.ReadAllText(projectPath);
        // This method only modifies the PBXProject string passed in (contents).
        // After this method finishes, we must write the contents string to disk
        //File.WriteAllText(projectPath, contents);

        AddOrUpdateEntitlements(
           path,
           project,
           extensionGUID,
           extensionTargetName,
           new HashSet<EntitlementOptions> {
               EntitlementOptions.ApsEnv, EntitlementOptions.AppGroups
           }
        );
#endif
    }

    // Copies NotificationService.m and .h files into the NotificationServiceExtension folder adds them to the Xcode target
    private static void AddExtensionSourceFilesToTarget(PBXProject project, string extensionGUID, string path, bool forServiceExtension)
    {
        var buildPhaseID = project.AddSourcesBuildPhase(extensionGUID);
        foreach (var type in new string[] { "m", "h" })
        {
            var nativeFileName = "";
            var sourcePath = "";
            var nativeFileRelativeDestination = "";

            if(forServiceExtension){
                nativeFileName = NOTIFICATION_SERVICE_EXTENSION_OBJECTIVEC_FILENAME + "." + type;
                sourcePath = MOE_IOS_LOCATION + DIR_CHAR + nativeFileName;
                nativeFileRelativeDestination = NOTIFICATION_SERVICE_EXTENSION_TARGET_NAME + "/" + nativeFileName;
            }
            else{
                nativeFileName = PUSH_TEMPLATES_EXTENSION_OBJECTIVEC_FILENAME + "." + type;
                sourcePath = MOE_IOS_PUSH_TEMP_LOCATION + DIR_CHAR + nativeFileName;
                nativeFileRelativeDestination = PUSH_TEMPLATES_EXTENSION_TARGET_NAME + "/" + nativeFileName;
            }
            

            var destPath = path + DIR_CHAR + nativeFileRelativeDestination;
            if (!File.Exists(destPath))
                FileUtil.CopyFileOrDirectory(sourcePath, destPath);

            var sourceFileGUID = project.AddFile(nativeFileRelativeDestination, nativeFileRelativeDestination, PBXSourceTree.Source);
            project.AddFileToBuildSection(extensionGUID, buildPhaseID, sourceFileGUID);
        }
    }

    // Create a .plist file for the NSE
    // NOTE: File in Xcode project is replaced everytime, never appends
    private static bool CreateExtensionPlistFile(string path, bool forServiceExtension)
    {
#if UNITY_2017_2_OR_NEWER
        var pathToExtension = path + DIR_CHAR;
        if(forServiceExtension){
            pathToExtension = pathToExtension + NOTIFICATION_SERVICE_EXTENSION_TARGET_NAME;
        }
        else{
            pathToExtension = pathToExtension + PUSH_TEMPLATES_EXTENSION_TARGET_NAME;
        }
        Directory.CreateDirectory(pathToExtension);

        var extensionPlistPath = pathToExtension + DIR_CHAR + "Info.plist";
        bool exisiting = File.Exists(extensionPlistPath);

        // Read plist from MoEngage iOS folder.
        var plistFile = new PlistDocument();
        if(forServiceExtension){
            plistFile.ReadFromFile(MOE_IOS_LOCATION + DIR_CHAR + "Info.plist");
        }
        else{
            plistFile.ReadFromFile(MOE_IOS_PUSH_TEMP_LOCATION + DIR_CHAR + "Info.plist");
        }
        
        plistFile.root.SetString("CFBundleShortVersionString", PlayerSettings.bundleVersion);
        plistFile.root.SetString("CFBundleVersion", PlayerSettings.iOS.buildNumber.ToString());
        plistFile.WriteToFile(extensionPlistPath);
        return exisiting;
#else
         return true;
#endif
    }


        private static void AddPushTemplateExtension(PBXProject project, string path)
    {
#if UNITY_2017_2_OR_NEWER && !UNITY_CLOUD_BUILD
        var projectPath = PBXProject.GetPBXProjectPath(path);
        var mainTargetGUID = GetPBXProjectTargetGUID(project);
        var extensionTargetName = PUSH_TEMPLATES_EXTENSION_TARGET_NAME;

        var exisitingPlistFile = CreateExtensionPlistFile(path,false);
        // If file exisits then the below has been completed before from another build
        // The below will not be updated on Append builds
        // Changes would most likely need to be made to support Append builds
        if (exisitingPlistFile)
            return;

        var extensionGUID = PBXProjectExtensions.AddAppExtension(
           project,
           mainTargetGUID,
           extensionTargetName,
           PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS) + "." + extensionTargetName,
           extensionTargetName + "/" + "Info.plist" // Unix path as it's used by Xcode
        );

        AddExtensionSourceFilesToTarget(project, extensionGUID, path, false);
        AddExtensionStoryBoardToTarget(project, extensionGUID, path);

        foreach (var framework in FRAMEWORKS_TO_ADD)
        {
            project.AddFrameworkToProject(extensionGUID, framework, true);
        }

        // Makes it so that the extension target is Universal (not just iPhone) and has an iOS 10 deployment target
        project.SetBuildProperty(extensionGUID, "TARGETED_DEVICE_FAMILY", "1,2");
        project.SetBuildProperty(extensionGUID, "IPHONEOS_DEPLOYMENT_TARGET", "12.0");

        project.SetBuildProperty(extensionGUID, "ARCHS", "$(ARCHS_STANDARD)");
        project.SetBuildProperty(extensionGUID, "DEVELOPMENT_TEAM", PlayerSettings.iOS.appleDeveloperTeamID);
        project.SetBuildProperty(extensionGUID, "GCC_ENABLE_OBJC_EXCEPTIONS", "Yes");

        project.WriteToFile(projectPath);

        //var contents = File.ReadAllText(projectPath);
        // This method only modifies the PBXProject string passed in (contents).
        // After this method finishes, we must write the contents string to disk
        //File.WriteAllText(projectPath, contents);

        AddOrUpdateEntitlements(
           path,
           project,
           extensionGUID,
           extensionTargetName,
           new HashSet<EntitlementOptions> {
               EntitlementOptions.ApsEnv, EntitlementOptions.AppGroups
           }
        );
#endif
    }

    private static void AddExtensionStoryBoardToTarget(PBXProject project, string extensionGUID, string path)
    {
        var buildPhaseID = project.AddSourcesBuildPhase(extensionGUID);
        var storyBoardFileName = "MainInterface.storyboard";
        var sourcePath = MOE_IOS_PUSH_TEMP_LOCATION + DIR_CHAR + storyBoardFileName;
        var nativeFileRelativeDestination = PUSH_TEMPLATES_EXTENSION_TARGET_NAME + DIR_CHAR + storyBoardFileName;

        var destPath = path + DIR_CHAR + nativeFileRelativeDestination;
        if (!File.Exists(destPath))
            FileUtil.CopyFileOrDirectory(sourcePath, destPath);

        string resourcesBuildPhase = project.GetResourcesBuildPhaseByTarget(extensionGUID);
        string resourcesFilesGuid = project.AddFile(destPath, nativeFileRelativeDestination, PBXSourceTree.Source);
        project.AddFileToBuildSection(extensionGUID, resourcesBuildPhase, resourcesFilesGuid);
    }

}
#endif