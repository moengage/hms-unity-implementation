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
using System.Runtime.InteropServices;
using UnityEngine;
using MoEMiniJSON;

namespace MoEngage
{

#if UNITY_IOS
    public class MoEngageiOS
    {
        private const string TAG = "MoEngageiOS";

        #region DLL Imports

        [DllImport("__Internal")]
        private static extern void initialize(string gameObjPayload);

        [DllImport("__Internal")]
        private static extern void setAppStatus(string appStatusPayload);

        [DllImport("__Internal")]
        private static extern void setAlias(string aliasPayload);

        [DllImport("__Internal")]
        private static extern void setUserAttribute(string userAttrPayload);

        [DllImport("__Internal")]
        private static extern void trackEvent(string eventPayload);

        [DllImport("__Internal")]
        private static extern void resetUser();

        [DllImport("__Internal")]
        private static extern void enableLogs();

        [DllImport("__Internal")]
        private static extern void registerForPush();

        [DllImport("__Internal")]
        private static extern void showInApp();

        [DllImport("__Internal")]
        private static extern void setInAppContexts(string contextsPayload);

        [DllImport("__Internal")]
        private static extern void invalidateInAppContexts();

        [DllImport("__Internal")]
        private static extern void getSelfHandledInApp();

        [DllImport("__Internal")]
        private static extern void updateSelfHandledInAppStatusWithPayload(string selfHandledPayload);

        [DllImport("__Internal")]
        private static extern void startGeofenceMonitoring();

        [DllImport("__Internal")]
        private static extern void optOutOfIDFATracking(string optOutPayload);

        [DllImport("__Internal")]
        private static extern void optOutOfIDFVTracking(string optOutPayload);

        [DllImport("__Internal")]
        private static extern void optOutGDPRTracking(string optOutPayload);

        [DllImport("__Internal")]
        private static extern void updateSdkState(string payload);

        #endregion

        #region Initialize

        public static void Initialize(string gameObjectName)
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": Initialize:: ");
			string gameObjPayload = MoEUtils.GetGameObjectPayload(gameObjectName);
			initialize(gameObjPayload);
#endif
        }

        #endregion

        #region AppStatus 
        public static void SetAppStatus(MoEAppStatus appStatus)
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": SetAppStatus:: appStatus: " + appStatus);
			string appStatusPayload = MoEUtils.GetAppStatusPayload(appStatus);
			setAppStatus(appStatusPayload);
#endif
        }

        #endregion

        #region UserAttribute Tracking
        public static void SetAlias(string alias)
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": SetAlias:: alias: " + alias);
			string aliasPayload = MoEUtils.GetAliasPayload(alias);
			setAlias(aliasPayload);
#endif
        }


        public static void SetUserAttribute<T>(string attributeName, T attributeValue)
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": SetUserAttribute:: attributeName: " + attributeName + "attributeValue: " + attributeValue);
			string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue);
			setUserAttribute(userAttributesPayload);
#endif
        }

        public static void SetUserAttributeISODate(string attributeName, string isoDate)
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": SetUserAttributeISODate:: attributeName: " + attributeName + " isoDate: " + isoDate);
			string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_TIMESTAMP, isoDate);
			setUserAttribute(userAttributesPayload);
#endif
        }

        public static void SetUserAttributeLocation(string attributeName, GeoLocation location)
        {
#if !UNITY_EDITOR
			Dictionary<string,double> locationDict = location.ToDictionary();
			Debug.Log(TAG + ": SetUserAttributeLocation:: attributeName: " + attributeName + " locationDict: " + locationDict);
			string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_LOCATION, locationDict);
			setUserAttribute(userAttributesPayload);
#endif
        }

        #endregion

        #region Event Tracking

        public static void TrackEvent(string eventName, Properties properties)
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": TrackEvent:: eventName: " + eventName + "\n properties: " + properties);
			string eventPayload = MoEUtils.GetEventPayload(eventName, properties);
			Debug.Log(TAG + ": TrackEvent:: eventPayload: " + eventPayload);
			trackEvent(eventPayload);
#endif
        }

        #endregion

        #region Push Notifications

        public static void RegisterForPush()
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": RegisterForPush::");
			registerForPush();
#endif
        }

        #endregion

        #region InApp Methods

        public static void ShowInApp()
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": ShowInApp::");
			showInApp();
#endif
        }

        public static void SetInAppContexts(string[] contexts)
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": SetInAppContexts::");
			string contextPayload = MoEUtils.GetContextsPayload(contexts);
			Debug.Log(TAG + ": SetInAppContexts:: contextPayload: " + contextPayload);
			setInAppContexts(contextPayload);	
#endif
        }

        public static void InvalidateInAppContexts()
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": InvalidateInAppContexts::");
			invalidateInAppContexts();
#endif
        }

        public static void GetSelfHandledInApp()
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": GetSelfHandledInApp::");
			getSelfHandledInApp();
#endif
        }

        public static void SelfHandledShown(InAppCampaign campaign)
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + " SelfHandledShown:: " );
			string payload = MoEUtils.GetSelfHandledPayload(campaign, MoEConstants.ATTRIBUTE_TYPE_SELF_HANDLED_IMPRESSION);
			Debug.Log(TAG + " SelfHandledShown() Payload: " + payload);
			updateSelfHandledInAppStatusWithPayload(payload);	
#endif
        }

        public static void SelfHandledClicked(InAppCampaign campaign)
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + " SelfHandledClicked:: ");
			string payload = MoEUtils.GetSelfHandledPayload(campaign, MoEConstants.ATTRIBUTE_TYPE_SELF_HANDLED_CLICK);
			Debug.Log(TAG + " SelfHandledClicked:: Payload: " + payload);
			updateSelfHandledInAppStatusWithPayload(payload);	
#endif
        }

        public static void SelfHandledPrimaryClicked(InAppCampaign campaign)
	{
#if !UNITY_EDITOR
			Debug.Log(TAG + " SelfHandledPrimaryClicked::");
			string payload = MoEUtils.GetSelfHandledPayload(campaign, MoEConstants.ATTRIBUTE_TYPE_SELF_HANDLED_PRIMARY_CLICKED);
			Debug.Log(TAG + " SelfHandledPrimaryClicked:: Payload: " + payload);
			updateSelfHandledInAppStatusWithPayload(payload);
#endif
	}

        public static void SelfHandledDismissed(InAppCampaign campaign)
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + " SelfHandledDismissed::");
			string payload = MoEUtils.GetSelfHandledPayload(campaign, MoEConstants.ATTRIBUTE_TYPE_SELF_HANDLED_DISMISSED);
			Debug.Log(TAG + " SelfHandledDismissed:: Payload: " + payload);
			updateSelfHandledInAppStatusWithPayload(payload);	
#endif
        }

        #endregion

        #region Geofence Method

        public static void StartGeofenceMonitoring()
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": StartGeofenceMonitoring::");
			startGeofenceMonitoring();
#endif
        }

        #endregion

        #region Utils and OptOuts

        public static void EnableSDKLogs()
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": EnableSDKLogs::");
			enableLogs();
#endif
        }

        public static void OptOutOfIDFATracking(bool optOut)
        {
#if !UNITY_EDITOR
            Debug.Log(TAG + " OptOutOfIDFATracking::");
            var optOutDict = new Dictionary<string, bool>()
            {
                { MoEConstants.ARGUMENT_OPT_OUT_STATUS, optOut }
            };
            string payload = Json.Serialize(optOutDict);
            optOutOfIDFATracking(payload);
#endif
        }

        public static void OptOutOfIDFVTracking(bool optOut)
        {
#if !UNITY_EDITOR
            Debug.Log(TAG + " OptOutOfIDFVTracking::");
            var optOutDict = new Dictionary<string, bool>()
            {
                { MoEConstants.ARGUMENT_OPT_OUT_STATUS, optOut }
            };
            string payload = Json.Serialize(optOutDict);
            optOutOfIDFVTracking(payload);
#endif
        }

        public static void optOutDataTracking(bool shouldOptOut)
	{
#if !UNITY_EDITOR
	        Debug.Log(TAG + " optOutDataTracking::");
		string payload = MoEUtils.GetOptOutTrackingPayload(MoEConstants.PARAM_TYPE_DATA, shouldOptOut);
		Debug.Log(TAG + " optOutDataTracking:: payload: " + payload);
		optOutGDPRTracking(payload);
#endif
	}

	public static void optOutPushTracking(bool shouldOptOut)
	{
#if !UNITY_EDITOR
		Debug.Log(TAG + " optOutPushTracking::");
		string payload = MoEUtils.GetOptOutTrackingPayload(MoEConstants.PARAM_TYPE_PUSH, shouldOptOut);
		Debug.Log(TAG + " optOutPushTracking:: payload: " + payload);
		optOutGDPRTracking(payload);
#endif
	}

	public static void optOutInAppTracking(bool shouldOptOut)
	{
#if !UNITY_EDITOR
		Debug.Log(TAG + " optOutInAppTracking::");
		string payload = MoEUtils.GetOptOutTrackingPayload(MoEConstants.PARAM_TYPE_INAPP, shouldOptOut);
		Debug.Log(TAG + " optOutInAppTracking:: payload: " + payload);
		optOutGDPRTracking(payload);
#endif
	}

        #endregion

        #region Reset User

        public static void Logout()
        {
#if !UNITY_EDITOR
		Debug.Log(TAG + ":  ResetUser::");
		resetUser();
#endif
        }

        #endregion

        #region Reset User

        public static void UpdateSdkState(bool state)
        {
#if !UNITY_EDITOR
		Debug.Log(TAG + " UpdateSdkState::");
		string payload = MoEUtils.GetSdkStatePayload(state);
		Debug.Log(TAG + " UpdateSdkState:: payload " + payload);
		updateSdkState(payload);
#endif
	}
        #endregion

    }

#endif
}
