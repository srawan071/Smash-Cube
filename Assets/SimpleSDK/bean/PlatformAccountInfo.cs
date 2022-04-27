using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SimpleSDKNS
{
    [Serializable]
    public class PlatformAccountInfo
    {
        public string platform;
        public bool hasLinked;
        public string uniqeId;
        public string iconUrl;
        public string nickName;
    }
}