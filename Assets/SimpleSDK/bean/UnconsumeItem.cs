using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SimpleSDKNS
{
    [Serializable]
    public class UnconsumeItem
    {
        public long gameOrderId;
        public string itemId;
        public long createTime;
        public long purchaseTime;
        public int status;
    }
}