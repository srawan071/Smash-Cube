using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleSDKNS
{
    [Serializable]
    public class LoginResult
    {
        public long gameAccountId;
        public string loginType;
        public bool isNew;
    }
}