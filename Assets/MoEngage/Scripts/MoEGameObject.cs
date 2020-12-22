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
    public class MoEGameObject : MonoBehaviour
    {
        private const string TAG = "MoEGameObject";

        public static event EventHandler<PushToken> PushTokenCallback;
        public static event EventHandler<PushCampaign> PushNotifCallback;
        public static event EventHandler<InAppCampaign> InAppShown;
        public static event EventHandler<InAppCampaign> InAppClicked;
        public static event EventHandler<InAppCampaign> InAppDismissed;
        public static event EventHandler<InAppCampaign> InAppCustomAction;
        public static event EventHandler<InAppCampaign> InAppSelfHandled;

        // Start is called before the first frame update
        void Start()
        {
            MoEngageClient.Initialize(gameObject);
        }

        public void PushToken(string payload)
        {
            Debug.Log(TAG + " PushToken() Callback from native: " + payload);
            PushToken token = MoEUtils.GetPushTokenFromPayload(payload);
            OnPushTokenGenerated(token);
        }

        public void PushClicked(string payload)
        {
            Debug.Log(TAG + "PushClicked() Callback from Native: " + payload);
            Dictionary<string, object> dict = Json.Deserialize(payload) as Dictionary<string, object>;
            PushCampaign campaign = new PushCampaign(dict);
            OnPushClicked(campaign);
        }

        public void InAppCampaignShown(string payload)
        {
            Debug.Log(TAG + " InAppCampaignShown() Callback From Native" + payload);
            InAppCampaign campaign = MoEUtils.GetInAppCampaignFromPayload(payload);
            OnInAppShown(campaign);
        }

        public void InAppCampaignClicked(string payload)
        {
            Debug.Log(TAG + " InAppCampaignClicked() Callback From Native" + payload);
            InAppCampaign campaign = MoEUtils.GetInAppCampaignFromPayload(payload);
            OnInAppClicked(campaign);
        }

        public void InAppCampaignDismissed(string payload)
        {
            Debug.Log(TAG + " InAppCampaignDismissed() Callback from Native: " + payload);
            InAppCampaign campaign = MoEUtils.GetInAppCampaignFromPayload(payload);
            OnInAppDismissed(campaign);
        }

        public void InAppCampaignCustomAction(string payload)
        {
            Debug.Log(TAG + " InAppCampaignCustomAction() Callback from Native: " + payload);
            InAppCampaign campaign = MoEUtils.GetInAppCampaignFromPayload(payload);
            OnInAppCustomAction(campaign);

        }

        public void InAppCampaignSelfHandled(string payload)
        {
            Debug.Log(TAG + " InAppCampaignSelfHandled() Callback from Native: " + payload);
            InAppCampaign campaign = MoEUtils.GetInAppCampaignFromPayload(payload);
            OnInAppSelfHandled(campaign);
        }


        protected virtual void OnPushClicked(PushCampaign payload)
        {
            PushNotifCallback?.Invoke(this, payload);
        }

        protected virtual void OnInAppShown(InAppCampaign campaign)
        {
            InAppShown?.Invoke(this, campaign);
        }

        protected virtual void OnInAppClicked(InAppCampaign campaign)
        {
            InAppClicked?.Invoke(this, campaign);
        }

        protected virtual void OnInAppDismissed(InAppCampaign campaign)
        {
            InAppDismissed?.Invoke(this, campaign);
        }

        protected virtual void OnInAppCustomAction(InAppCampaign campaign)
        {
            InAppCustomAction?.Invoke(this, campaign);
        }

        protected virtual void OnInAppSelfHandled(InAppCampaign campaign)
        {
            InAppSelfHandled?.Invoke(this, campaign);
        }

        protected virtual void OnPushTokenGenerated(PushToken pushToken)
        {
            PushTokenCallback?.Invoke(this, pushToken);
        }
    }
}
