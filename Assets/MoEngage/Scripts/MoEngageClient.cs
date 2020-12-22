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
using System.Collections.Generic;
using UnityEngine;

namespace MoEngage
{
    public class MoEngageClient : MonoBehaviour
    {
        void Awake()
        {
#if (UNITY_IPHONE || UNITY_ANDROID)
           DontDestroyOnLoad(gameObject);
#endif
        }

        #region Initialize
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject">Instance of Game Object</param>
        public static void Initialize(GameObject gameObject)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.Initialize(gameObject ? gameObject.name : null);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.Initialize(gameObject ? gameObject.name : null);
#endif
        }
        #endregion

        #region AppStatus Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appStatus">Instance of MoEAppStatus</param>
        public static void SetAppStatus(MoEAppStatus appStatus)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetAppStatus(appStatus);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetAppStatus(appStatus);
#endif
        }

        #endregion

        #region UserAttribute Tracking Methods

        /// <summary>
        /// Updates the already set unique identifier, sets a unique identifier if not set already.
        /// </summary>
        /// <param name="alias"></param>
        public static void SetAlias(string alias)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetAlias(alias);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetAlias(alias);
#endif
        }


        /// <summary>
        /// Tracks a unique identifier for the user.
        /// </summary>
        /// <param name="uniqueId"></param>
        public static void SetUniqueId(string uniqueId)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_UNIQUE_ID, uniqueId);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_UNIQUE_ID, uniqueId);
#endif
        }

        /// <summary>
        /// Track user's first name.
        /// </summary>
        /// <param name="firstName"></param>
        public static void SetFirstName(string firstName)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_USER_FIRST_NAME, firstName);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_USER_FIRST_NAME, firstName);
#endif
        }

        /// <summary>
        /// Track user's last name.
        /// </summary>
        /// <param name="lastName"></param>
        public static void SetLastName(string lastName)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_USER_LAST_NAME, lastName);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_USER_LAST_NAME, lastName);
#endif
        }

        /// <summary>
        /// Track user's email-id.
        /// </summary>
        /// <param name="emailId"></param>
        public static void SetEmail(string emailId)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_USER_EMAIL, emailId);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_USER_EMAIL, emailId);
#endif
        }

        /// <summary>
        /// Track user's phone number.
        /// </summary>
        /// <param name="phoneNumber"></param>
        public static void SetPhoneNumber(string phoneNumber)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_USER_MOBILE, phoneNumber);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_USER_MOBILE, phoneNumber);
#endif
        }

        /// <summary>
        /// Track user's gender.
        /// </summary>
        /// <param name="gender"></param>
        public static void SetGender(MoEUserGender gender)
        {
            string genderVal = gender.ToString().ToLower();
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_USER_GENDER, genderVal);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(MoEConstants.USER_ATTRIBUTE_USER_GENDER, genderVal);
#endif
        }


        /// <summary>
        /// Tracks birthdate as user attribute.
        /// </summary>
        /// <param name="time"></param>
        public static void SetBirthdate(string time)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttributeISODate(MoEConstants.USER_ATTRIBUTE_USER_BDAY, time);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttributeISODate(MoEConstants.USER_ATTRIBUTE_USER_BDAY, time);
#endif
        }

        /// <summary>
        /// Tracks user's location as attribute.
        /// </summary>
        /// <param name="location"></param>
        public static void SetUserLocation(GeoLocation location)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        MoEngageAndroid.SetUserAttributeLocation(MoEConstants.USER_ATTRIBUTE_USER_LOCATION_ANDROID, location);
#elif UNITY_IOS && !UNITY_EDITOR
        MoEngageiOS.SetUserAttributeLocation(MoEConstants.USER_ATTRIBUTE_USER_LOCATION_IOS, location);
#endif
        }


        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, int attributeValue)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(attributeName, attributeValue);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(attributeName, attributeValue);
#endif
        }

        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, double attributeValue)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(attributeName, attributeValue);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(attributeName, attributeValue);
#endif
        }

        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, float attributeValue)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(attributeName, attributeValue);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(attributeName, attributeValue);
#endif
        }

        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, bool attributeValue)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(attributeName, attributeValue);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(attributeName, attributeValue);
#endif
        }

        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, long attributeValue)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(attributeName, attributeValue);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(attributeName, attributeValue);
#endif
        }

        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, string attributeValue)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttribute(attributeName, attributeValue);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttribute(attributeName, attributeValue);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="isoDate"></param>
        public static void SetUserAttributeISODate(string attributeName, string isoDate)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttributeISODate(attributeName, isoDate);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttributeISODate(attributeName, isoDate);
#endif
        }

        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="location"></param>
        public static void SetUserAttributeLocation(string attributeName, GeoLocation location)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetUserAttributeLocation(attributeName, location);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SetUserAttributeLocation(attributeName, location);
#endif
        }
        #endregion

        #region User Reset

        /// <summary>
        /// Invalidates the existing user and session. A new user and session is created.
        /// </summary>
        public static void Logout()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.Logout();
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.Logout();
#endif
        }

        #endregion

        #region Track Event

        /// <summary>
        /// Tracks an event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="properties">Event Attributes.</param>
        public static void TrackEvent(string eventName, Properties properties)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.TrackEvent(eventName, properties);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.TrackEvent(eventName, properties);
#endif
        }

        #endregion

        #region InApp Methods
        /// <summary>
        /// 
        /// </summary>
        public static void ShowInApp()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.ShowInApp();
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.ShowInApp();
#endif
        }

        public static void SetInAppContexts(string[] contexts)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SetAppContext(contexts);
#elif UNITY_IOS && !UNITY_EDITOR
             MoEngageiOS.SetInAppContexts(contexts);
#endif

        }

        public static void InvalidateInAppContexts()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.InvalidateAppContext();
#elif UNITY_IOS && !UNITY_EDITOR
             MoEngageiOS.InvalidateInAppContexts();
#endif

        }

        public static void GetSelfHandledInApp()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.GetSelfHandledInApp();
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.GetSelfHandledInApp();
#endif
        }
        public static void SelfHandledShown(InAppCampaign inAppCampaign)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.SelfHandledShown(inAppCampaign);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SelfHandledShown(inAppCampaign);
#endif
        }

        public static void SelfHandledClicked(InAppCampaign inAppCampaign)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			MoEngageAndroid.SelfHandledClicked(inAppCampaign);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SelfHandledClicked(inAppCampaign);
#endif
        }

        public static void SelfHandledPrimaryClicked(InAppCampaign inAppCampaign)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
	    MoEngageAndroid.SelfHandledPrimaryClicked(inAppCampaign);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SelfHandledPrimaryClicked(inAppCampaign);
#endif
        }

        public static void SelfHandledDismissed(InAppCampaign inAppCampaign)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			MoEngageAndroid.SelfHandledDismissed(inAppCampaign);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.SelfHandledDismissed(inAppCampaign);
#endif
        }

        #endregion

        #region GDPR OptOut Methods

        public static void optOutDataTracking(bool shouldOptOut)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.optOutDataTracking(shouldOptOut);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.optOutDataTracking(shouldOptOut);
#endif
        } 
        
        public static void optOutPushTracking(bool shouldOptOut)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.optOutPushTracking(shouldOptOut);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.optOutPushTracking(shouldOptOut);
#endif
        } 

         public static void optOutInAppTracking(bool shouldOptOut)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.optOutInAppTracking(shouldOptOut);
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.optOutInAppTracking(shouldOptOut);
#endif
        } 

        #endregion

        #region Enable/Disable SDK Methods

        public static void EnableSdk()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.UpdateSdkState(true);  
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.UpdateSdkState(true);
#endif
        }

        public static void DisableSdk()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.UpdateSdkState(false);   
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.UpdateSdkState(false);
#endif
        }

         #endregion

        #region iOS Specific Methods

        /// <summary>
        /// Enable verbose logs of the SDK.
        /// </summary>
        public static void EnableSDKLogs()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        	MoEngageAndroid.EnableSDKLogs();
#elif UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.EnableSDKLogs();
#endif
        }

        public static void OptOutOfIDFATracking(bool optOut)
        {
#if UNITY_IOS && !UNITY_EDITOR
           MoEngageiOS.OptOutOfIDFATracking(optOut);
#endif
        }

        public static void OptOutOfIDFVTracking(bool optOut)
        {
#if UNITY_IOS && !UNITY_EDITOR
           MoEngageiOS.OptOutOfIDFVTracking(optOut);
#endif
        }

        public static void RegisterForPush()
        {
#if UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.RegisterForPush();
#endif
        }

        public static void StartGeofenceMonitoring()
        {
#if UNITY_IOS && !UNITY_EDITOR
            MoEngageiOS.StartGeofenceMonitoring();
#endif
        }

        #endregion

        #region Android Specific Methods

        public static void PassPushPayload(IDictionary<string, string> pushPayload)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.PassPushPayload(pushPayload);
#endif
        }

        public static void PassPushToken(string pushToken)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.PassPushToken(pushToken);
#endif
        }

        #endregion

    }
}