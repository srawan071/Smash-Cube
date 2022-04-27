using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleSDKNS
{
    public interface IPurchaseItemsListener
    {
        void getPurchaseItems(PurchaseItems purchaseItems);
    }
}