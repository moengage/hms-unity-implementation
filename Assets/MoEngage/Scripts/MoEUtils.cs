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
using MoEMiniJSON;

namespace MoEngage
{
	public class MoEUtils
	{
        public static string GetGameObjectPayload(string gameObjectName)
		{
			Dictionary<string, string> payloadDict = new Dictionary<string, string> {
				{ MoEConstants.ARGUMENT_GAME_OBJECT, gameObjectName }
			};
			string payload = Json.Serialize(payloadDict);
			return payload;
		}

		public static string GetAppStatusPayload(MoEAppStatus appStatus)
		{
			Dictionary<string, string> appStatusDict = new Dictionary<string, string> {
				{ MoEConstants.ARGUMENT_APP_STATUS, appStatus.ToString() }
			};

			string appStatusPayload = Json.Serialize(appStatusDict);
			return appStatusPayload;

		}

		public static string GetAliasPayload(string alias)
		{
			Dictionary<string, string> aliasDict = new Dictionary<string, string> {
				{ MoEConstants.ARGUMENT_ALIAS, alias }
			};

			string aliasPayload = Json.Serialize(aliasDict);
			return aliasPayload;
		}

		public static string GetUserAttributePayload<T>(string attrName, string attrType, T attrValue)
		{
			Dictionary<string, object> userAttributesDict = new Dictionary<string, object> {
				{ MoEConstants.ARGUMENT_USER_ATTRIBUTE_NAME, attrName },
				{ MoEConstants.ARGUMENT_TYPE, attrType },
				{ attrType.Equals(MoEConstants.ATTRIBUTE_TYPE_LOCATION) ? 
					MoEConstants.ARGUMENT_USER_ATTRIBUTE_LOCATION_VALUE : MoEConstants.ARGUMENT_USER_ATTRIBUTE_VALUE, attrValue}
			};

			string userAttributesPayload = Json.Serialize(userAttributesDict);
			return userAttributesPayload;
		}

		public static string GetEventPayload(string eventName, Properties properties)
		{
			Dictionary<string, object> eventDict = new Dictionary<string, object> {
				{ MoEConstants.ARGUMENT_EVENT_NAME, eventName },
				{ MoEConstants.ARGUMENT_EVENT_ATTRIBUTES, properties.ToDictionary() },
				{MoEConstants.ARGUMENT_IS_NON_INTERACTIVE_EVENT, properties.GetIsNonInteractive() }
			};

			string eventPayload = Json.Serialize(eventDict);
			return eventPayload;
		}

		public static string GetPushPayload(IDictionary<string, string> payload) {
			Dictionary<string, object> pushPayloadDict = new Dictionary<string, object> {
		        { MoEConstants.ARGUMENT_FCM_PAYLOAD, payload }
		      };

		    string pushPayload =Json.Serialize(pushPayloadDict);
		    return pushPayload;
		}

		public static string GetPushTokenPayload(string pushToken) 
		{
			Dictionary<string, string> tokenDict = new Dictionary<string, string>
			{
				{ MoEConstants.ARGUMENT_FCM_TOKEN, pushToken }
			};

			string pushTokenPayload = Json.Serialize(tokenDict);
			return pushTokenPayload;
		}


		public static string GetContextsPayload(string[] contexts)
		{
			Dictionary<string, string[]> contextDict = new Dictionary<string, string[]> {
				{ MoEConstants.ARGUMENT_CONTEXTS, contexts }
			};

			string contextPayload = Json.Serialize(contextDict);
			return contextPayload;
		}

		public static InAppCampaign GetInAppCampaignFromPayload(string payload)
		{

			Dictionary<string, object> payloadDictionary = MoEMiniJSON.Json.Deserialize(payload) as Dictionary<string, object>;
			InAppCampaign campaign = new InAppCampaign
			{
				platform = payloadDictionary[MoEConstants.PARAM_PLATFORM] as string,
				campaignId = payloadDictionary[MoEConstants.PARAM_CAMPAIGN_ID] as string,
				campaignName = payloadDictionary[MoEConstants.PARAM_CAMPAIGN_NAME] as string
			};

			// Navigation Action Info
			if (payloadDictionary.ContainsKey(MoEConstants.PARAM_NAVIGATION))
			{
				var navigationDictionary = payloadDictionary[MoEConstants.PARAM_NAVIGATION] as Dictionary<string, object>;
				NavigationAction navigationAction = new NavigationAction()
				{
					navigationType = navigationDictionary[MoEConstants.PARAM_NAVIGATION_TYPE] as string,
					url = navigationDictionary[MoEConstants.PARAM_NAVIGATION_URL] as string
				};

				if (navigationDictionary.ContainsKey(MoEConstants.PARAM_KEY_VALUE_PAIR))
				{
					navigationAction.keyValuePairs = navigationDictionary[MoEConstants.PARAM_KEY_VALUE_PAIR] as Dictionary<string, object>;
				}
				campaign.navigation = navigationAction;
			}

			// Custom Action Info
			if (payloadDictionary.ContainsKey(MoEConstants.PARAM_CUSTOM_ACTION))
			{
				CustomAction custom = new CustomAction()
				{
					keyValuePairs = payloadDictionary[MoEConstants.PARAM_CUSTOM_ACTION] as Dictionary<string, object>
				};
				campaign.customAction = custom;
			}

			// Self Handled InApp Campaign
			if (payloadDictionary.ContainsKey(MoEConstants.PARAM_SELF_HANDLED))
			{
				var selfHandledDictionary = payloadDictionary[MoEConstants.PARAM_SELF_HANDLED] as Dictionary<string, object>;
				SelfHandled selfHandled = new SelfHandled()
				{
					payload = selfHandledDictionary[MoEConstants.PARAM_PAYLOAD] as string,
					dismissInterval = (long)selfHandledDictionary[MoEConstants.PARAM_DISMISS_INTERVAL]
				};

				if (selfHandledDictionary.ContainsKey(MoEConstants.PARAM_IS_CANCELLABLE))
                {
					selfHandled.isCancellable = (bool)selfHandledDictionary[MoEConstants.PARAM_IS_CANCELLABLE];
                }
				campaign.selfHandled = selfHandled;
			}

			return campaign;
		}

		public static string GetSelfHandledPayload(InAppCampaign inAppCampaign, string type)
		{
			var selfHandledDictionary = new Dictionary<string, object>()
			{
				{ MoEConstants.ARGUMENT_PAYLOAD,  inAppCampaign.selfHandled.payload},
				{ MoEConstants.ARGUMENT_DISMISS_INTERVAL, inAppCampaign.selfHandled.dismissInterval },
				{ MoEConstants.ARGUMENT_IS_CANCELLABLE, inAppCampaign.selfHandled.isCancellable}
			};
			var inAppCampaignDictionary = new Dictionary<string, object>()
			{
				{ MoEConstants.ARGUMENT_CAMPAIGN_ID, inAppCampaign.campaignId },
				{ MoEConstants.ARGUMENT_CAMPAIGN_NAME, inAppCampaign.campaignName},
				{ MoEConstants.ARGUMENT_SELF_HANDLED, selfHandledDictionary},
				{ MoEConstants.ARGUMENT_TYPE, type}
			};

			return Json.Serialize(inAppCampaignDictionary);
		}

		public static string GetOptOutTrackingPayload( string type, bool shouldOptOut) {
			var optOutTrackingDictionary = new Dictionary<string, object>()
			{
				{ MoEConstants.ARGUMENT_TYPE, type },
				{ MoEConstants.PARAM_STATE, shouldOptOut}
			};

			return Json.Serialize(optOutTrackingDictionary);
		}

			public static string GetSdkStatePayload(bool isSdkEnabled)
      {
			var sdkStatusDictionary = new Dictionary<string, object>()
			{
				{ MoEConstants.FEATURE_STATUS_IS_SDK_ENABLED, isSdkEnabled},
			};

			return Json.Serialize(sdkStatusDictionary);
		}

		public static PushToken GetPushTokenFromPayload(string payload)
    {
			Dictionary<string, object> payloadDictionary = MoEMiniJSON.Json.Deserialize(payload) as Dictionary<string, object>;
			
			return new PushToken(
				payloadDictionary[MoEConstants.PARAM_PLATFORM] as string,
				payloadDictionary[MoEConstants.PARAM_PUSH_TOKEN] as string,
				(PushService)Enum.Parse(typeof(PushService), payloadDictionary[MoEConstants.PARAM_PUSH_SERVICE] as string)
				);
    }

	}
}

