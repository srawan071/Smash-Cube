using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleSDKNS
{
    [Serializable]
    public class CallbackInfo
    {
        private string network;
        private string placementId;
        private string adsourceId;

        public CallbackInfo()
        {

        }

        public Dictionary<string, string> GetAdditionInfo()
        {
            return new Dictionary<string, string>();
        }

        public string GetAdSourceId()
        {
            return adsourceId;
        }

        public string GetNetworkFirmId()
        {
            return network;
        }

        public string GetNetworkPlacement()
        {
            return placementId;
        }
    }
}