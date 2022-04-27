using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json;
namespace SimpleSDKNS
{
#if UNITY_ANDROID
    public class SimpleSDKBridgeAndroid : SimpleSDKBridge
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

        public void initWithConfig(string fileContent)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            CallStatic("setActivity", jo);

            CallStatic("init", fileContent);
        }
        public void onResume()
        {
            CallStatic("onResume");
        }
        public void onPause()
        {
            CallStatic("onPause");
        }
        public void Log(string eventName, Dictionary<string, string> paramMap)
        {
            Dictionary<string, object> sendParams = new Dictionary<string, object>();
            sendParams.Add("eventName", eventName);
            sendParams.Add("params", paramMap);
            string paramJson = Json.Serialize(sendParams);
            CallStatic("log", paramJson);
        }
        public StaticInfo GetStaticInfo()
        {
            string s = CallStaticWithString("getStaticInfo");
            return StaticInfo.fromJson(s);
        }

        public bool HasInterstitial()
        {
            return CallStaticWithBool("hasInterstitial");
        }

        public void ShowInterstitial(string adEntry)
        {
            CallStatic("showInterstitial", adEntry);
        }

        public bool HasReward()
        {
            return CallStaticWithBool("hasReward");
        }

        public void ShowReward(string adEntry)
        {
            CallStatic("showReward", adEntry);
        }
        public void ShowOrReShowBanner(BannerPos pos)
        {
            CallStatic("showOrReShowBanner", (int)pos);
        }
        public void HideBanner()
        {
            CallStatic("hideBanner");
        }

        public void RemoveBanner()
        {
            CallStatic("removeBanner");
        }

        public Dictionary<string, string> GetLoadingStatusSummary()
        {
            string s = CallStaticWithString("getLoadingStatusSummary");
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
            return CallStaticWithBool("isLogin");
        }
        public long getGameAccountId()
        {
            string s = CallStaticWithString("getGameAccountId");
            return long.Parse(s);
        }

        public void Logout()
        {
            CallStatic("logout");
        }
        public void autoLoginAsync(bool platformLogin)
        {
            CallStatic("autoLoginAsync", platformLogin);
        }
        public void checkLoginAsync()
        {
            CallStatic("checkLoginAsync");
        }

        public void loginWithTypeAsync(LOGIN_TYPE loginType)
        {
            CallStatic("loginWithTypeAsync", loginType.ToString());
        }

        public void bindWithTypeAsync(LOGIN_TYPE loginType)
        {
            CallStatic("bindWithTypeAsync", loginType.ToString());
        }

        public void unbindWithTypeAsync(LOGIN_TYPE loginType)
        {
            CallStatic("unbindWithTypeAsync", loginType.ToString());
        }

        public void getUserInfoAsync()
        {
            CallStatic("getUserInfoAsync");
        }

        public ShopItemResult getShopItems()
        {
            string s = CallStaticWithString("getShopItems");
            if (s != null && s.Length > 0)
            {
                return JsonUtility.FromJson<ShopItemResult>(s);
            }
            else return null;
        }

        public void getShopItemsAsync()
        {
            CallStatic("getShopItemsAsync");
        }

        public void startPayment(string itemId, string cpOrderId)
        {
            CallStatic("startPayment", itemId, cpOrderId);
        }

        public void startPaymentForSimpleGame(string itemId)
        {
            CallStatic("startPaymentForSimpleGame", itemId);
        }

        public void consumeItem(long gameOrderId)
        {
            CallStatic("consumeItem", gameOrderId.ToString());
        }
    }
#endif
}
