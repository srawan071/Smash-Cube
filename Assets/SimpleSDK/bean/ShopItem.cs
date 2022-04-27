using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleSDKNS
{
    [Serializable]
    public class ShopItem
    {
        public string itemId;
        public string itemType;
        public long price;
        public string currency;
        public string formattedPrice;
    }
}