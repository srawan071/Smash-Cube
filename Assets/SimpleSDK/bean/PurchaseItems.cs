using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SimpleSDKNS
{
    [Serializable]
    public class PurchaseItems
    {
        public List<UnconsumeItem> unconsumeItems = new List<UnconsumeItem>();
    }
}