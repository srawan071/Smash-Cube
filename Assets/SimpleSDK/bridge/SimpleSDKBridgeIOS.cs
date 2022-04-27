using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace SimpleSDKNS
{
#if UNITY_IOS
    public class SimpleSDKBridgeIOS : SimpleSDKBridge
    {
        string className = "com.simplesdk.simplenativeunitybridge.SimpleNativeUnityBridge";

        private string CallStaticWithString(string methodName, params object[] args)
        {
            AndroidJavaClass jc = new AndroidJavaClass(className);
            return jc.CallStatic<string>(methodName, args);
        }
        private bool CallStaticWithBool(string methodName, params object[] args)
        {
            AndroidJavaClass jc = new AndroidJavaClass(className);
            return jc.CallStatic<bool>(methodName, args);
        }
        private void CallStatic(string methodName, params object[] args)
        {
            AndroidJavaClass jc = new AndroidJavaClass(className);
            jc.CallStatic(methodName, args);
        }

        [DllImport("__Internal")]
        private static extern void bridgeInitWithConfig(string fileContent);
        [DllImport("__Internal")]
        private static extern void bridgeLog(string paramJson);
        [DllImport("__Internal")]
        private static extern string bridgeGetStaticInfo();
        [DllImport("__Internal")]
        private static extern bool bridgeHasInterstitial();
        [DllImport("__Internal")]
        private static extern void bridgeShowInterstitial(string adEntry);
        [DllImport("__Internal")]
        private static extern bool bridgeHasReward();
        [DllImport("__Internal")]
        private static extern void bridgeShowReward(string adEntry);
        [DllImport("__Internal")]
        private static extern void bridgeShowOrReShowBanner(int pos);
        [DllImport("__Internal")]
        private static extern void bridgeHideBanner();
        [DllImport("__Internal")]
        private static extern void bridgeRemoveBanner();
        [DllImport("__Internal")]
        private static extern string bridgeGetLoadingStatusSummary();

        //userpayment
        [DllImport("__Internal")]
        private static extern bool bridgeIsLogin();
        [DllImport("__Internal")]
        private static extern string bridgeGetGameAccountId();
        [DllImport("__Internal")]
        private static extern void bridgeLogout();
        [DllImport("__Internal")]
        private static extern void bridgeAutoLoginAsync(bool platformLogin);
        [DllImport("__Internal")]
        private static extern void bridgeCheckLoginAsync();
        [DllImport("__Internal")]
        private static extern void bridgeLoginWithTypeAsync(string loginType);
        [DllImport("__Internal")]
        private static extern void bridgeBindWithTypeAsync(string loginType);
        [DllImport("__Internal")]
        private static extern void bridgeUnbindWithTypeAsync(string loginType);
        [DllImport("__Internal")]
        private static extern void bridgeGetUserInfoAsync();
        [DllImport("__Internal")]
        private static extern string bridgeGetShopItems();
        [DllImport("__Internal")]
        private static extern void bridgeStartPayment(string itemId, string cpOrderId);
        [DllImport("__Internal")]
        private static extern void bridgeStartPaymentForSimpleGame(string itemId);
        [DllImport("__Internal")]
        private static extern void bridgeConsumeItem(string gameOrderId);

        public void initWithConfig(string fileContent)
        {
            bridgeInitWithConfig(fileContent);
        }
        public void onResume()
        {
            
        }
        public void onPause()
        {
            
        }
        public void Log(string eventName, Dictionary<string, string> paramMap)
        {
            Dictionary<string, object> sendParams = new Dictionary<string, object>();
            sendParams.Add("eventName", eventName);
            sendParams.Add("params", paramMap);
            string paramJson = Json.Serialize(sendParams);
            bridgeLog(paramJson);
        }
        public StaticInfo GetStaticInfo()
        {
            string s = bridgeGetStaticInfo();
            return StaticInfo.fromJson(s);
        }

        public bool HasInterstitial()
        {
            return bridgeHasInterstitial();
        }

        public void ShowInterstitial(string adEntry)
        {
            bridgeShowInterstitial(adEntry);
        }

        public bool HasReward()
        {
            return bridgeHasReward();
        }

        public void ShowReward(string adEntry)
        {
            bridgeShowReward(adEntry);
        }
        public void ShowOrReShowBanner(BannerPos pos)
        {
            bridgeShowOrReShowBanner((int)pos);
        }
        public void HideBanner()
        {
            bridgeHideBanner();
        }

        public void RemoveBanner()
        {
            bridgeRemoveBanner();
        }

        public Dictionary<string, string> GetLoadingStatusSummary()
        {
            string s = bridgeGetLoadingStatusSummary();
            Dictionary<string, object> m = (Dictionary<string, object>)Json.Deserialize(s);
            Dictionary<string, string> re = new Dictionary<string, string>();
            foreach (var entry in m)
            {
                re.Add(entry.Key, (string)entry.Value);
            }
            return re;
        }
        public bool isLogin()
        {
            return bridgeIsLogin();
        }
        public long getGameAccountId()
        {
            return long.Parse(bridgeGetGameAccountId());
        }

        public void Logout()
        {
            bridgeLogout();
        }

        public void autoLoginAsync(bool platformLogin)
        {
            bridgeAutoLoginAsync(platformLogin);
        }

        public void checkLoginAsync()
        {
            bridgeCheckLoginAsync();
        }

        public void loginWithTypeAsync(LOGIN_TYPE loginType)
        {
            bridgeLoginWithTypeAsync(loginType.ToString());
        }

        public void bindWithTypeAsync(LOGIN_TYPE loginType)
        {
            bridgeBindWithTypeAsync(loginType.ToString());
        }

        public void unbindWithTypeAsync(LOGIN_TYPE loginType)
        {
            bridgeUnbindWithTypeAsync(loginType.ToString());
        }

        public void getUserInfoAsync()
        {
            bridgeGetUserInfoAsync();
        }

        public ShopItemResult getShopItems()
        {
            string s = bridgeGetShopItems();
            if (s != null && s.Length > 0)
            {
                return JsonUtility.FromJson<ShopItemResult>(s);
            }
            else return null;
        }

        public void getShopItemsAsync()
        {
            bridgeGetShopItems();
        }

        public void startPayment(string itemId, string cpOrderId)
        {
            bridgeStartPayment(itemId, cpOrderId);
        }
        public void startPaymentForSimpleGame(string itemId)
        {
            bridgeStartPaymentForSimpleGame(itemId);
        }

        public void consumeItem(long gameOrderId)
        {
            bridgeConsumeItem(gameOrderId.ToString());
        }
    }
#endif
}