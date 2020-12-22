﻿/*
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoEngage
{
    [System.Serializable]
    public class PushCampaign
    {
        public string platform;
        public bool isDefaultAction;
        public IDictionary<string,object> clickedAction;
        public IDictionary<string,object> payload;

        public PushCampaign(Dictionary<string, object> pushPayload)
        {
            platform = pushPayload[MoEConstants.PARAM_PLATFORM] as string;

            if (pushPayload.ContainsKey(MoEConstants.PARAM_IS_DEFAULT_ACTION)) {
                isDefaultAction = (bool)pushPayload[MoEConstants.PARAM_IS_DEFAULT_ACTION];
            }

            if (pushPayload.ContainsKey(MoEConstants.PARAM_CLICKED_ACTION)) {
                clickedAction = pushPayload[MoEConstants.PARAM_CLICKED_ACTION] as  Dictionary<string, object>;
            }

            payload = pushPayload[MoEConstants.PARAM_PAYLOAD] as Dictionary<string, object>;
        }
    }
}