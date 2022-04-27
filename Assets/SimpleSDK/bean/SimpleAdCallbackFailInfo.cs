using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleSDKNS
{
    [Serializable]
    public class SimpleAdCallbackFailInfo
    {
        public string unitId;
        public string code;
        public string message;
        public SimpleAdCallbackFailInfo()
        {

        }
    }
}