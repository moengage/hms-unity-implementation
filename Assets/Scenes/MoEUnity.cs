using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuaweiService;
using MoEngage;

public class MoETestScript : MonoBehaviour
{
    private static string TAG = "MoETestScript";
    // Start is called before the first frame update
    void Start() {
        Debug.Log(TAG + " Start() : ");
        MoEngageClient.Initialize(gameObject);
        MoEngageClient.SetUniqueId("moengageunitysample");
        MoEngageClient.EnableSDKLogs();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
         {
             if (Application.platform == RuntimePlatform.Android)
             {
                 AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                 activity.Call<bool>("moveTaskToBack", true);
             }
         }
        
    }
}
