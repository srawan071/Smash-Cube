using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleSDKNS
{
    [Serializable]
    public class UserInfoResult
    {
        public string gameId;
        public long gameAccountId;
        public List<PlatformAccountInfo> loginInfo = new List<PlatformAccountInfo>();
    }
}