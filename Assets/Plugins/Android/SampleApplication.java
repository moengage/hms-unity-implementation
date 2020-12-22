package com.moengage.sampleapp;

import com.moengage.core.MoEngage;
import android.app.Application;
import android.content.Context;
import com.moengage.unity.wrapper.MoEInitializer;
import com.moengage.core.Logger;

/**
 * @author Umang Chamaria
 * Date: 25/06/20
 */
public class SampleApplication extends Application {
  @Override public void onCreate() {
    super.onCreate();

    //Initialise MoEngage SDK

    int smallIconId = getApplicationContext().getResources().getIdentifier("small_icon", "drawable",
        getApplicationContext().getPackageName());

    int largeIconId = getApplicationContext().getResources().getIdentifier("large_icon","drawable",
        getApplicationContext().getPackageName());

    //Add your APP_ID
    MoEngage.Builder moEngage = new MoEngage.Builder(this, "APP_ID")
        .setNotificationSmallIcon(smallIconId)
        .setNotificationLargeIcon(largeIconId)
        .optOutTokenRegistration()
        .enablePushKitTokenRegistration();

    MoEInitializer.initialize(getApplicationContext(), moEngage);
  }

  @Override
  protected void attachBaseContext(Context base) {
     super.attachBaseContext(base);
  }
}
