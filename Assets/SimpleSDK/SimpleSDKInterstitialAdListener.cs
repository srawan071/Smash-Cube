using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSDKNS
{
    public interface SimpleSDKInterstitialAdListener
    {
        /***
        * 广告展示
        */
        void onInterstitialAdShow(string unitId, CallbackInfo callbackInfo);
        /***
         * 广告关闭
         */
        void onInterstitialAdClose(string unitId, CallbackInfo callbackInfo);
        /***
         * 广告点击
         */
        void onInterstitialAdClick(string unitId, CallbackInfo callbackInfo);
    }
}