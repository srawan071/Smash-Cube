using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SimpleSDKNS
{
    public class SimpleSDKCallback : MonoBehaviour
    {
        static public SimpleSDKCallback instance = null;
        public void Awake()
        {
            instance = this;
        }

        public void setAttributionInfo(string str)
        {
            Debug.Log("SimpleSDKCallback setAttributionInfo " + str);
            AttributionInfo info = AttributionInfo.FromJson(str);
            AttributionHelper.GetInstance().SetAttributionInfo(info);
        }

        //private CallbackInfo GetCallbackInfoFromDict(Dictionary<string, object> dict)
        //{
        //    if (dict.ContainsKey("callbackInfo"))
        //    {
        //        return JsonCallbackInfo.fromDict((Dictionary<string, object>)dict["callbackInfo"]);
        //    }
        //    else return null;
           
        //}
        //********************* ad **********************
        public void onAdImpress(string str)
        {
            Debug.Log("SimpleSDKCallback onAdImpress " + str);
            SimpleAdCallbackInfo info = JsonUtility.FromJson<SimpleAdCallbackInfo>(str);
            //Dictionary<string,object> dict =  (Dictionary<string, object>)Json.Deserialize(str);
            //string unitId = (string)dict["unitId"];
            //CallbackInfo info = GetCallbackInfoFromDict(dict);
            SimpleSDKBannerAdListener listener = SimpleSDKAd.instance.GetSimpleSDKBannerAdListener();
            if(listener!= null)
            {
                listener.onAdImpress(info.unitId, info.callbackInfo);
            }
        }

        public void onAdClick(string str)
        {
            Debug.Log("SimpleSDKCallback onAdClick " + str);
            //Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(str);
            //string unitId = (string)dict["unitId"];
            //CallbackInfo info = GetCallbackInfoFromDict(dict);
            SimpleAdCallbackInfo info = JsonUtility.FromJson<SimpleAdCallbackInfo>(str);
            SimpleSDKBannerAdListener listener = SimpleSDKAd.instance.GetSimpleSDKBannerAdListener();
            if (listener != null)
            {
                listener.onAdClick(info.unitId, info.callbackInfo);
            }
        }

        public void onInterstitialAdShow(string str)
        {
            Debug.Log("SimpleSDKCallback onInterstitialAdShow " + str);
            //Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(str);
            //string unitId = (string)dict["unitId"];
            //CallbackInfo info = GetCallbackInfoFromDict(dict);
            SimpleAdCallbackInfo info = JsonUtility.FromJson<SimpleAdCallbackInfo>(str);
            SimpleSDKInterstitialAdListener listener = SimpleSDKAd.instance.GetSimpleSDKInterstitialAdListener();
            if (listener != null)
            {
                listener.onInterstitialAdShow(info.unitId, info.callbackInfo);
            }
        }
         
        public void onInterstitialAdClose(string str)
        {
            Debug.Log("SimpleSDKCallback onInterstitialAdClose " + str);
            //Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(str);
            //string unitId = (string)dict["unitId"];
            //CallbackInfo info = GetCallbackInfoFromDict(dict);
            SimpleAdCallbackInfo info = JsonUtility.FromJson<SimpleAdCallbackInfo>(str);
            SimpleSDKInterstitialAdListener listener = SimpleSDKAd.instance.GetSimpleSDKInterstitialAdListener();
            if (listener != null)
            {
                listener.onInterstitialAdClose(info.unitId, info.callbackInfo);
            }
        }
        public void onInterstitialAdClick(string str)
        {
            Debug.Log("SimpleSDKCallback onInterstitialAdClick " + str);
            //Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(str);
            //string unitId = (string)dict["unitId"];
            //CallbackInfo info = GetCallbackInfoFromDict(dict);
            SimpleAdCallbackInfo info = JsonUtility.FromJson<SimpleAdCallbackInfo>(str);
            SimpleSDKInterstitialAdListener listener = SimpleSDKAd.instance.GetSimpleSDKInterstitialAdListener();
            if (listener != null)
            {
                listener.onInterstitialAdClick(info.unitId,info.callbackInfo);
            }
        }

        public void onRewardedVideoAdPlayStart(string str)
        {
            Debug.Log("SimpleSDKCallback onRewardedVideoAdPlayStart " + str);
            //Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(str);
            //string unitId = (string)dict["unitId"];
            //CallbackInfo info = GetCallbackInfoFromDict(dict);
            SimpleAdCallbackInfo info = JsonUtility.FromJson<SimpleAdCallbackInfo>(str);
            SimpleSDKRewardedVideoListener listener = SimpleSDKAd.instance.GetSimpleSDKRewardedVideoListener();
            if (listener != null)
            {
                listener.onRewardedVideoAdPlayStart(info.unitId, info.callbackInfo);
            }
        }

        public void onRewardedVideoAdPlayFail(string str)
        {
            Debug.Log("SimpleSDKCallback onRewardedVideoAdPlayFail " + str);

            //Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(str);
            //string unitId = (string)dict["unitId"];
            //string code = (string)dict["code"];
            //string message = (string)dict["message"];
            SimpleAdCallbackFailInfo info = JsonUtility.FromJson<SimpleAdCallbackFailInfo>(str);
            SimpleSDKRewardedVideoListener listener = SimpleSDKAd.instance.GetSimpleSDKRewardedVideoListener();
            if (listener != null)
            {
                listener.onRewardedVideoAdPlayFail(info.unitId,info.code,info.message);
            }
        }
        public void onRewardedVideoAdPlayClosed(string str)
        {
            Debug.Log("SimpleSDKCallback onRewardedVideoAdPlayClosed " + str);
            //Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(str);
            //string unitId = (string)dict["unitId"];
            //CallbackInfo info = GetCallbackInfoFromDict(dict);
            SimpleAdCallbackInfo info = JsonUtility.FromJson<SimpleAdCallbackInfo>(str);
            //SimpleAdCallbackInfo info = JsonUtility.FromJson<SimpleAdCallbackInfo>(str);
            SimpleSDKRewardedVideoListener listener = SimpleSDKAd.instance.GetSimpleSDKRewardedVideoListener();
            if (listener != null)
            {
                listener.onRewardedVideoAdPlayClosed(info.unitId, info.callbackInfo);
            }
        }
        public void onRewardedVideoAdPlayClicked(string str)
        {
            Debug.Log("SimpleSDKCallback onRewardedVideoAdPlayClicked "+str);
            //Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(str);
            //string unitId = (string)dict["unitId"];
            //CallbackInfo info = GetCallbackInfoFromDict(dict);
            SimpleAdCallbackInfo info = JsonUtility.FromJson<SimpleAdCallbackInfo>(str);
            SimpleSDKRewardedVideoListener listener = SimpleSDKAd.instance.GetSimpleSDKRewardedVideoListener();
            if (listener != null)
            {
                listener.onRewardedVideoAdPlayClicked(info.unitId, info.callbackInfo);
            }
        }

        /***
         * 发放奖励
         */
        public void onReward(string str)
        {
            Debug.Log("SimpleSDKCallback onReward " + str);
            SimpleAdCallbackInfo info = JsonUtility.FromJson<SimpleAdCallbackInfo>(str);
            //Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(str);
            //string unitId = (string)dict["unitId"];
            //CallbackInfo info = GetCallbackInfoFromDict(dict);
            SimpleSDKRewardedVideoListener listener = SimpleSDKAd.instance.GetSimpleSDKRewardedVideoListener();
            if (listener != null)
            {
                listener.onReward(info.unitId, info.callbackInfo);
            }
        }

        //********************* pay **********************
        public void autoLoginSuccess(string s)
        {
            var r = JsonUtility.FromJson<AutoLoginResult>(s);
            SimpleSDKUserPayment.instance.autoLoginAsyncSuccess(r);
        }

        public void autoLoginFail(string s)
        {
            var r = JsonUtility.FromJson<State>(s);
            SimpleSDKUserPayment.instance.autoLoginAsyncFail(r);
        }

        public void checkLoginSuccess(string s)
        {
            var r = JsonUtility.FromJson<CheckLoginResult>(s);
            SimpleSDKUserPayment.instance.checkLoginAsyncSuccess(r);
        }

        public void checkLoginFail(string s)
        {
            var r = JsonUtility.FromJson<State>(s);
            SimpleSDKUserPayment.instance.checkLoginAsyncFail(r);
        }

        public void loginWithTypeAsyncSuccess(string s)
        {
            var r = JsonUtility.FromJson<LoginResult>(s);
            SimpleSDKUserPayment.instance.loginWithTypeAsyncSuccess(r);
        }

        public void loginWithTypeAsyncFail(string s)
        {
            var r = JsonUtility.FromJson<State>(s);
            SimpleSDKUserPayment.instance.loginWithTypeAsyncFail(r);
        }

        public void bindWithTypeAsyncSuccess(string s)
        {
            var r = JsonUtility.FromJson<UserInfoResult>(s);
            SimpleSDKUserPayment.instance.bindWithTypeAsyncSuccess(r);
        }

        public void bindWithTypeAsyncFail(string s)
        {
            var r = JsonUtility.FromJson<State>(s);
            SimpleSDKUserPayment.instance.bindWithTypeAsyncFail(r);
        }

        public void unbindWithTypeAsyncSuccess(string s)
        {
            var r = JsonUtility.FromJson<UserInfoResult>(s);
            SimpleSDKUserPayment.instance.unbindWithTypeAsyncSuccess(r);
        }

        public void unbindWithTypeAsyncFail(string s)
        {
            var r = JsonUtility.FromJson<State>(s);
            SimpleSDKUserPayment.instance.unbindWithTypeAsyncFail(r);
        }

        public void getUserInfoAsyncSuccess(string s)
        {
            var r = JsonUtility.FromJson<UserInfoResult>(s);
            SimpleSDKUserPayment.instance.getUserInfoAsyncSuccess(r);
        }

        public void getUserInfoAsyncFail(string s)
        {
            var r = JsonUtility.FromJson<State>(s);
            SimpleSDKUserPayment.instance.getUserInfoAsyncFail(r);
        }

        public void getShopItemsAsyncSuccess(string s)
        {
            var r = JsonUtility.FromJson<ShopItemResult>(s);
            var c = SimpleSDKUserPayment.instance.getShopItemsAsyncSuccess;
            if (c != null)
            {
                c(r);
            }
        }

        public void getShopItemsAsyncFail(string s)
        {
            var r = JsonUtility.FromJson<State>(s);
            SimpleSDKUserPayment.instance.getShopItemsAsyncFail(r);
        }

        public void startPaymentSuccess(string s)
        {
            var r = JsonUtility.FromJson<StartPaymentResult>(s);
            SimpleSDKUserPayment.instance.startPaymentSuccess(r);
        }

        public void startPaymentFail(string s)
        {
            var r = JsonUtility.FromJson<State>(s);
            SimpleSDKUserPayment.instance.startPaymentFail(r);
        }

        public void getPurchaseItems(string s)
        {
            var r = JsonUtility.FromJson<PurchaseItems>(s);
            var c = SimpleSDKUserPayment.instance.listener;
            if (c != null)
            {
                c.getPurchaseItems(r);
            }
        }
    }
}