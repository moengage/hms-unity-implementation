/*
 * Copyright (c) 2014-2020 MoEngage Inc.
 *
 * All rights reserved.
 *
 *  Use of source code or binaries contained within MoEngage SDK is permitted only to enable use of the MoEngage platform by customers of MoEngage.
 *  Modification of source code and inclusion in mobile apps is explicitly allowed provided that all other conditions are met.
 *  Neither the name of MoEngage nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *  Redistribution of source code or binaries is disallowed except with specific prior written permission. Any such redistribution must retain the above copyright notice, this list of conditions and the following disclaimer.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoEMiniJSON;

namespace MoEngage 
{

#if UNITY_ANDROID
	public class MoEngageAndroid 
	{
		private const string TAG = "MoEngageAndroid";

		private static AndroidJavaClass moengageAndroidClass = new AndroidJavaClass("com.moengage.unity.wrapper.MoEAndroidWrapper");
		private static AndroidJavaObject moengageAndroid = moengageAndroidClass.CallStatic<AndroidJavaObject>("getInstance");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameObject"></param>
		public static void Initialize(string gameObjectName) 
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ": Initialize:: ");
			string gameObjPayload = MoEUtils.GetGameObjectPayload(gameObjectName);
			moengageAndroid.Call("initialize", gameObjPayload);
#endif
	    }

    public static void SetAppStatus(MoEAppStatus appStatus)
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ": SetAppStatus:: appStatus: " + appStatus);
			string appStatusPayload = MoEUtils.GetAppStatusPayload(appStatus);
			Debug.Log(TAG + ": SetAppStatus:: appStatus: " + appStatusPayload);
			moengageAndroid.Call("setAppStatus", appStatusPayload);
#endif
	    }

		public static void SetAlias(string alias) 
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ": SetAlias:: alias: " + alias);
			string aliasPayload = MoEUtils.GetAliasPayload(alias);
			Debug.Log(TAG + ": SetAlias:: aliasPayload: " + aliasPayload);
			moengageAndroid.Call("setAlias", aliasPayload);
#endif
	    }

		public static void SetUserAttribute(string attributeName, object attributeValue) 
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ": SetUserAttribute:: attributeName: " + attributeName + ", attributeValue: " + attributeValue);
			string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue);
			Debug.Log(TAG + ": SetUserAttribute:: userAttributesPayload: " + userAttributesPayload);
			moengageAndroid.Call("setUserAttribute", userAttributesPayload);
#endif
		}

		public static void SetUserAttributeISODate(string attributeName, string isoDate) 
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ": SetUserAttributeISODate:: attributeName: " + attributeName + ", attributeValue: " + isoDate);
			string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_TIMESTAMP, isoDate);
			Debug.Log(TAG + ": SetUserAttributeISODate:: userAttributesPayload: " + userAttributesPayload);
			moengageAndroid.Call("setUserAttribute", userAttributesPayload);
#endif
		}

		public static void SetUserAttributeLocation(string attributeName, GeoLocation location)
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + "SetUserAttributeLocation:: attributeName: " + attributeName + ", location: " + location.ToString());
			Dictionary<string, double> locationDict = location.ToDictionary();
			string locationPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_LOCATION, locationDict);
			Debug.Log(TAG + ": SetUserAttributeLocation:: attributeName: " + attributeName + " locationPayload: " + locationPayload);
			moengageAndroid.Call("setUserAttribute", locationPayload);
#endif
	    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="attributes"></param>
	    public static void TrackEvent(string eventName, Properties properties) 
	    {
#if !UNITY_EDITOR
	    Debug.Log(TAG + ": TrackEvent:: eventName: " + eventName + "\n properties: " + properties);
			string eventPayload = MoEUtils.GetEventPayload(eventName, properties);
			Debug.Log(TAG + ": TrackEvent:: eventPayload: " + eventPayload);
			moengageAndroid.Call("trackEvent", eventPayload);
#endif
	    }

	  public static void EnableSDKLogs()
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ": EnableSDKLogs::");
			moengageAndroid.Call("enableSDKLogs");
#endif
		}

		public static void Logout() 
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ":  Logout:: ");
			moengageAndroid.Call("logout");
#endif
		}

	    public static void GetSelfHandledInApp() 
		{	
#if !UNITY_EDITOR
			Debug.Log(TAG + ": GetSelfHandledInApp::");
			moengageAndroid.Call("getSelfHandledInApp");
#endif
		}

		public static void ShowInApp() 
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ": ShowInApp::");
			moengageAndroid.Call("showInApp");
#endif
		}

		public static void PassPushPayload(IDictionary<string, string> pushPayloadDict) 
	    {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": PassPushPayload::");
			
			string pushPayload = MoEUtils.GetPushPayload(pushPayloadDict);
			Debug.Log(TAG + ": PassPushPayload:: pushPayload: " + pushPayload);
			moengageAndroid.Call("passPushPayload", pushPayload);
#endif
	    }

	    public static void PassPushToken(string pushToken) 
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ": PassPushToken:: ");
			string pushTokenPayload = MoEUtils.GetPushTokenPayload(pushToken);
			Debug.Log(TAG + ": PassPushToken:: pushToken: " + pushTokenPayload);
			moengageAndroid.Call("passPushToken", pushTokenPayload);
#endif
	    }

		public static void SelfHandledShown(InAppCampaign campaign)
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + " SelfHandledShown:: " );
			string payload = MoEUtils.GetSelfHandledPayload(campaign, MoEConstants.ATTRIBUTE_TYPE_SELF_HANDLED_IMPRESSION);
			Debug.Log(TAG + " SelfHandledShown() Payload: " + payload);
			moengageAndroid.Call("selfHandledCallback", payload);
#endif
		}

		public static void SelfHandledClicked(InAppCampaign campaign)
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + " SelfHandledClicked:: ");
			string payload = MoEUtils.GetSelfHandledPayload(campaign, MoEConstants.ATTRIBUTE_TYPE_SELF_HANDLED_CLICK);
			Debug.Log(TAG + " SelfHandledClicked:: Payload: " + payload);
			moengageAndroid.Call("selfHandledCallback", payload);
#endif
		}

		public static void SelfHandledPrimaryClicked(InAppCampaign campaign)
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + " SelfHandledPrimaryClicked::");
			string payload = MoEUtils.GetSelfHandledPayload(campaign, MoEConstants.ATTRIBUTE_TYPE_SELF_HANDLED_PRIMARY_CLICKED);
			Debug.Log(TAG + " SelfHandledPrimaryClicked:: Payload: " + payload);
			moengageAndroid.Call("selfHandledCallback", payload);
#endif
		}

		public static void SelfHandledDismissed(InAppCampaign campaign)
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + " SelfHandledDismissed::");
			string payload = MoEUtils.GetSelfHandledPayload(campaign, MoEConstants.ATTRIBUTE_TYPE_SELF_HANDLED_DISMISSED);
			Debug.Log(TAG + " SelfHandledDismissed:: Payload: " + payload);
			moengageAndroid.Call("selfHandledCallback", payload);
#endif
		}

		public static void SetAppContext(string[] contexts)
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + " SetAppContext:: " );
			string contextPayload = MoEUtils.GetContextsPayload(contexts);
			Debug.Log(TAG + " SetAppContext: Payload: " + contextPayload);
			moengageAndroid.Call("setAppContext", contextPayload);
#endif
		}

		public static void InvalidateAppContext()
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + " InvalidateAppContext:: " );
			moengageAndroid.Call("resetContext");
#endif
		}

		public static void optOutDataTracking(bool shouldOptOut)
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + " optOutDataTracking::");
			string payload = MoEUtils.GetOptOutTrackingPayload(MoEConstants.PARAM_TYPE_DATA, shouldOptOut);
			Debug.Log(TAG + " optOutDataTracking:: payload: " + payload);
			moengageAndroid.Call("optOutTracking", payload);
#endif
		}

		public static void optOutPushTracking(bool shouldOptOut)
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + " optOutPushTracking::");
			string payload = MoEUtils.GetOptOutTrackingPayload(MoEConstants.PARAM_TYPE_PUSH, shouldOptOut);
			Debug.Log(TAG + " optOutPushTracking:: payload: " + payload);
			moengageAndroid.Call("optOutTracking", payload);
#endif
		}

		public static void optOutInAppTracking(bool shouldOptOut)
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + " optOutInAppTracking::");
			string payload = MoEUtils.GetOptOutTrackingPayload(MoEConstants.PARAM_TYPE_INAPP, shouldOptOut);
			Debug.Log(TAG + " optOutInAppTracking:: payload: " + payload);
			moengageAndroid.Call("optOutTracking", payload);
#endif
		}

		public static void UpdateSdkState(bool state)
    {
#if !UNITY_EDITOR
			Debug.Log(TAG + " EnabledSdk::");
			string payload = MoEUtils.GetSdkStatePayload(state);
			Debug.Log(TAG + " EnableSdk:: payload " + payload);
			moengageAndroid.Call("updateSdkState", payload);
#endif
		}

	}

#endif
}