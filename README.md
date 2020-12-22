# hms-unity-implementation

1. Add `agconnect-services.json` to `Assets` > `Plugins` > `Android`.
2. Edit the `SampleApplication.java` file in `Assets/Plugins/Android` to add your MoEngage APP_ID.
   ```
    //Add your APP_ID
    MoEngage.Builder moEngage = new MoEngage.Builder(this, "APP_ID")
        .setNotificationSmallIcon(smallIconId)
        .setNotificationLargeIcon(largeIconId)
        .optOutTokenRegistration()
        .enablePushKitTokenRegistration();
   ```