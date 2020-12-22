//
//  MoEUnityInitializer.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import "MoEUnityInitializer.h"
#import <MoEngage/MoEngage.h>
#import <MoEPluginBase/MoEPluginBase.h>
#import <MOInApp/MOInApp.h>
#import "MoEngageConfiguration.h"
#import "MoEUnityConstants.h"

#define MOE_UNITY_PLUGIN_VERSION    @"1.2.0"

@interface MoEUnityInitializer() <MoEPluginBridgeDelegate>
@property(assign, nonatomic) BOOL isSDKIntialized;
@property(nonatomic, strong) NSString* moeGameObjectName;

@end

@implementation MoEUnityInitializer

#pragma mark- Initialization

+(instancetype)sharedInstance{
    static dispatch_once_t onceToken;
    static MoEUnityInitializer *instance;
    dispatch_once(&onceToken, ^{
        instance = [[MoEUnityInitializer alloc] init];
    });
    return instance;
}

- (instancetype)init
{
    self = [super init];
    if (self) {
        self.isSDKIntialized = NO;
    }
    return self;
}

- (void)intializeSDKWithLaunchOptions:(NSDictionary*)launchOptions {
    
    [self intializeSDKWithLaunchOptions:launchOptions andSDKState:[[MoEngageCore sharedInstance] isSDKEnabled]];
}

- (void)intializeSDKWithLaunchOptions:(NSDictionary*)launchOptions andSDKState:(BOOL)isSDKEnabled{
    self.isSDKIntialized = YES;
    [self setupSDKWithLaunchOptions:launchOptions andSDKState:isSDKEnabled];
}

- (void)setupSDKWithGameObject:(NSString*)gameObjectName {
    self.moeGameObjectName = gameObjectName;
    if (!self.isSDKIntialized) {
        //this will works as fallback method if AppDelegate Swizzling doesn't work
        [self setupSDKWithLaunchOptions: nil andSDKState:[[MoEngageCore sharedInstance] isSDKEnabled]];
    }
    [[MoEPluginBridge sharedInstance] pluginInitialized];
}

- (void)setUnitySDKVersion{
    [[MoEPluginBridge sharedInstance] trackPluginVersion:MOE_UNITY_PLUGIN_VERSION forIntegrationType:Unity];
}

-(void)setupSDKWithLaunchOptions:(NSDictionary * _Nullable)launchOptions andSDKState:(BOOL)isSDKEnabled{
    
    if (kMoEngageLogsEnabled) {
        [MoEngage debug:LOG_ALL];
    }
    
    if (kDefaultIDFATrackingOptedOut) {
        [[MOAnalytics sharedInstance] optOutOfIDFATracking:true];
    }
    
    if (kDefaultIDFVTrackingOptedOut) {
        [[MOAnalytics sharedInstance] optOutOfIDFVTracking:true];
    }
    
    
    [self setUnitySDKVersion];
    [MoEPluginBridge sharedInstance].bridgeDelegate = self;
    [MoEngage setAppGroupID:[self getAppGroupID]];
    
    /* MoEngage - Create a MoEngageConfiguration.h file in Project's Assets > Plugin folder and provide the APP ID and Region for MoEngage Integration as shown below:
     
     #define kMoEngageAppID @"Your App ID"
     #define kMoEngageRegion @"DEFAULT"  // DEFAULT/EU/SERV3
     */
    
    NSString* region = kMoEngageRegion;
    [self setMoEngageRegion:region];
    
    NSString* moeAppID = kMoEngageAppID;
    if (moeAppID.length > 0) {
        [[MoEPluginInitializer sharedInstance] intializeSDKWithAppID:moeAppID withSDKState:isSDKEnabled andLaunchOptions:launchOptions];
    }
    else{
        NSAssert(NO, @"MoEngage - Provide the APP ID for your MoEngage App in MoEngageConfiguration.h file. To get the AppID login to your MoEngage account, after that go to Settings -> App Settings. You will find the App ID in this screen.");
    }
}

-(void)setMoEngageRegion:(NSString*)region{
    region = region.uppercaseString;
    if ([region isEqualToString:@"EU"]){
        [MoEngage redirectDataToRegion:MOE_REGION_EU];
    }
    else if ([region isEqualToString:@"SERV3"]){
        [MoEngage redirectDataToRegion:MOE_REGION_SERV3];
    }
    else{
        [MoEngage redirectDataToRegion:MOE_REGION_DEFAULT];
    }
}

-(NSString*)getAppGroupID{
    NSString *parentBundleIdentifier = [MOCoreUtils bundleIdentifier];
    NSString *appGroupID = [NSString stringWithFormat:@"group.%@.moengage",parentBundleIdentifier];
    return appGroupID;
}

#pragma mark- MoEPluginBridgeDelegate Callbacks

-(void)sendMessageWithName:(NSString *)name andPayload:(NSDictionary *)payloadDict{
    // TODO: Remove mapper in the next release -- To use same internal name as used in MoEPluginBase
    NSString* unityMethodName = nil;
    if ([name isEqualToString:kEventNamePushTokenRegistered]){
        unityMethodName = kUnityMethodNamePushTokenRegistered;
    }
    else if ([name isEqualToString:kEventNamePushClicked]) {
        unityMethodName = kUnityMethodNamePushClicked;
    }
    else if ([name isEqualToString:kEventNameInAppCampaignShown]){
        unityMethodName = kUnityMethodNameInAppShown;
    }
    else if ([name isEqualToString:kEventNameInAppCampaignClicked]){
        unityMethodName = kUnityMethodNameInAppClicked;
    }
    else if ([name isEqualToString:kEventNameInAppCampaignDismissed]){
        unityMethodName = kUnityMethodNameInAppDismissed;
    }
    else if ([name isEqualToString:kEventNameInAppSelfHandledCampaign]){
        unityMethodName = kUnityMethodNameInAppSelfHandled;
    }
    else if ([name isEqualToString:kEventNameInAppCampaignCustomAction]){
        unityMethodName = kUnityMethodNameInAppCustomAction;
    }
    
    if(unityMethodName != nil){
        NSDictionary* unityPayload = [payloadDict validObjectForKey:@"payload"];
        [self sendCallbackToUnityForMethod:unityMethodName withMessage:unityPayload];
    }
}

#pragma mark- Native to Unity Callbacks

-(void)sendCallbackToUnityForMethod:(NSString *)method withMessage:(NSDictionary *)messageDict {
    if (self.moeGameObjectName != nil) {
        NSString* objectName = self.moeGameObjectName;
        NSString* message = [self dictToJson:messageDict];
        UnitySendMessage([objectName UTF8String], [method UTF8String], [message UTF8String]);
    }
}

-(NSString *)dictToJson:(NSDictionary *)dict {
    NSError *err;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:0 error:&err];
    if(err != nil) {
        return nil;
    }
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}


@end
