using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSDKNS
{
    public interface SimpleSDKBannerAdListener
    {
        /**
        * 广告展示
        */
        void onAdImpress(string unitId, CallbackInfo callbackInfo);
        /**
         * 广告点击
         */
        void onAdClick(string unitId, CallbackInfo callbackInfo);
    }
}