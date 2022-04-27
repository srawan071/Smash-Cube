using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using Newtonsoft.Json;

namespace SimpleSDKNS
{
#if UNITY_EDITOR
    public class SimpleSDKBridgeEditor : SimpleSDKBridge
	{
        private StaticInfo info = null;
        private string gameId = "";
        private long gameAccountId = -1;
        private ShopItemResult result = new ShopItemResult();
        private System.Random r = new System.Random();
        private PurchaseItems purchaseItems = new PurchaseItems();
        public void initWithConfig(string fileContent)
        {
            string j = fileContent.Replace('\n', ' ');
            Debug.Log("simple sdk start with " + j);

            Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(j);

            Dictionary<string, string> infoDict = new Dictionary<string, string>();
            gameId = (string)dict["gameName"];
            infoDict.Add("gameName", (string)dict["gameName"]);
            infoDict.Add("pn", Application.identifier);
            infoDict.Add("appVersion", "editor");
            infoDict.Add("deviceid", SystemInfo.deviceUniqueIdentifier);
            infoDict.Add("platform", "editor");
            infoDict.Add("idfa", "idfa1");
            infoDict.Add("uid", "uid");
            infoDict.Add("sessionId", "sessionId");

            infoDict.Add("idfv", "");
            infoDict.Add("android_id", "");

            infoDict.Add("band", "band");
            infoDict.Add("model", "model");
            infoDict.Add("deviceName", "mywindows");
            infoDict.Add("systemVersion", "win");
            infoDict.Add("network", "none");

            info = StaticInfo.fromJson(Json.Serialize(infoDict));

            //set attr info
            SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(3, setAttrInfo));
            
            //set shop item
            SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(5, SetShopItem));
        }
        private void setAttrInfo()
        {
            AttributionInfo ainfo = new AttributionInfo();
            ainfo.network = SimpleSDK.instance.editorChannel.ToString();
            ainfo.campaign = "";
            ainfo.adgroup = "";
            ainfo.creative = "";
            SimpleSDKCallback.instance.setAttributionInfo(ainfo.ToJson());
        }
        private void SetShopItem()
        {
            result = new ShopItemResult();
            result.items = new List<ShopItem>();
            var index = 99;
            foreach (var one in SimpleSDK.instance.editorTestMockShopItemIdsList)
            {
                var s = new ShopItem();
                s.itemId = one;
                s.price = index;
                s.currency = "USD";
                s.formattedPrice = "$ " + (s.price / 100.0);
                result.items.Add(s);
                index += 100;
            }

            //
            SimpleSDKCallback.instance.getShopItemsAsyncSuccess(JsonUtility.ToJson(result));

        }
        public void onPause()
        {
            Debug.Log("simple sdk onPause ");
        }
        public void onResume()
        {
            Debug.Log("simple sdk onResume");
        }
        public StaticInfo GetStaticInfo()
        {
            return info;
        }
        public void Log(string eventName, Dictionary<string, string> paramMap)
        {
            Debug.Log("read to send log " + eventName + " " + Json.Serialize(paramMap));
        }
        public bool HasInterstitial()
        {
            return true;
        }

        public void ShowInterstitial(string adEntry)
        {
            Debug.Log("ShowInter");
            if (EditorAdMock.HasInit())
            {
                SimpleSDKCallback.instance.onInterstitialAdShow("{\"unitId\":\"test\"}");
                EditorAdMock.ShowInterAd(InterFinish);
                SimpleSDKDemo.onFront = false;
            }
            else
            {
                SimpleSDKCallback.instance.onInterstitialAdShow("{\"unitId\":\"test\"}");
                SimpleSDKCallback.instance.onInterstitialAdClose("{\"unitId\":\"test\"}");
            }
        }
        public void InterFinish(bool isReward)
        {
            SimpleSDKDemo.onFront = true;
            SimpleSDKCallback.instance.onInterstitialAdClose("{\"unitId\":\"test\"}");
        }

        public bool HasReward()
        {
            return true;
        }

        public void ShowReward(string adEntry)
        {
            Debug.Log("ShowReward");
            if (EditorAdMock.HasInit())
            {
                SimpleSDKCallback.instance.onRewardedVideoAdPlayStart("{\"unitId\":\"test\"}");
                EditorAdMock.ShowRewardAd(RewardFinish);
                SimpleSDKDemo.onFront = false;
            }
            else
            {
                SimpleSDKCallback.instance.onRewardedVideoAdPlayStart("{\"unitId\":\"test\"}");
                SimpleSDKCallback.instance.onRewardedVideoAdPlayClosed("{\"unitId\":\"test\"}");
            }
        }

        public void RewardFinish(bool isReward)
        {
            SimpleSDKDemo.onFront = true;
            SimpleSDKCallback.instance.onRewardedVideoAdPlayClosed("{\"unitId\":\"test\"}");
        }


        public void ShowOrReShowBanner(BannerPos pos)
        {
            Debug.Log("show banner");
            SimpleSDKCallback.instance.onAdImpress("{\"unitId\":\"test\"}");
            SimpleSDKCallback.instance.onAdClick("{\"unitId\":\"test\"}");
        }

        public void HideBanner()
        {
            Debug.Log("hide banner");
        }

        public void RemoveBanner()
        {
            Debug.Log("remove banner");
        }

        public Dictionary<string, string> GetLoadingStatusSummary()
        {
            var r = new Dictionary<string, string>();
            r.Add("reward", "succss");
            r.Add("inter", "succss");
            r.Add("banner", "succss");
            return r;
        }
        public bool isLogin()
        {
            return gameAccountId > 0;
        }

        public long getGameAccountId()
        {
            return gameAccountId;
        }

        public void Logout()
        {
            gameAccountId = -1;
        }
        public void autoLoginAsync(bool platformLogin)
        {
            if (SimpleSDK.instance.editorTestMockAutoLoginReturnSucc)
            {
                //测试有缓存的情况下登录
                gameAccountId = 22345678;
                var t = new AutoLoginResult(gameId, gameAccountId, "editorMock", true);
                SimpleSDKCallback.instance.autoLoginSuccess(JsonUtility.ToJson(t));
            }
            else
            {
                SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(1, delegate
                {
                    var s = new State(-1, "FAIL IN EDITOR MOCK NET FAIL");
                    SimpleSDKCallback.instance.autoLoginFail(JsonUtility.ToJson(s));

                }));
            }
        }

        public void checkLoginAsync()
        {
            if (SimpleSDK.instance.editorTestMockCheckLoginReturnSucc)
            {
                //测试有缓存的情况下登录
                gameAccountId = 12345678;
                var t = new CheckLoginResult(gameId, gameAccountId);
                SimpleSDKCallback.instance.checkLoginSuccess(JsonUtility.ToJson(t));
            }
            else
            {
                
                //return fail after network request
                SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(1, delegate
                {
                    var s = new State(-1, "FAIL IN EDITOR MOCK NET FAIL");
                    SimpleSDKCallback.instance.checkLoginFail(JsonUtility.ToJson(s));

                }));
            }

        }

        public void loginWithTypeAsync(LOGIN_TYPE loginType)
        {
            gameAccountId = 321654987;
            switch (loginType)
            {
                case LOGIN_TYPE.DEVICE:
                    {
                        if (SimpleSDK.instance.editorTestMockCheckLoginDeviceReturnSucc)
                        {
                            SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(1, delegate
                            {
                                var t = new LoginResult();
                                t.gameAccountId = gameAccountId;
                                t.isNew = true;
                                t.loginType = loginType.ToString();
                                SimpleSDKCallback.instance.loginWithTypeAsyncSuccess(JsonUtility.ToJson(t));
                            }));
                        }
                        else
                        {
                            SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(1, delegate
                            {
                                var s = new State(-1, "FAIL IN EDITOR");
                                SimpleSDKCallback.instance.loginWithTypeAsyncFail(JsonUtility.ToJson(s));
                            }));
                        }
                    };
                    break;
                default:
                    {
                        var s = new State(-1, "unsupport IN EDITOR");
                        SimpleSDKCallback.instance.loginWithTypeAsyncFail(JsonUtility.ToJson(s));
                    };
                    break;
            }
        }

        public void bindWithTypeAsync(LOGIN_TYPE loginType)
        {
            if (gameAccountId != -1)
            {
                SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(1, delegate
                {
                    var r = new UserInfoResult();
                    r.gameId = gameId;
                    r.gameAccountId = gameAccountId;
                    r.loginInfo = new List<PlatformAccountInfo>();
                    var info = new PlatformAccountInfo();
                    r.loginInfo.Add(info);
                    info.platform = "DEVICE";
                    info.hasLinked = true;
                    info.uniqeId = SystemInfo.deviceUniqueIdentifier;
                    info.iconUrl = "";
                    info.nickName = "";
                    SimpleSDKCallback.instance.bindWithTypeAsyncSuccess(JsonUtility.ToJson(r));
                }));
            }
            else
            {
                var s = new State(-1, "not login");
                SimpleSDKCallback.instance.bindWithTypeAsyncFail(JsonUtility.ToJson(s));
            }
        }

        public void unbindWithTypeAsync(LOGIN_TYPE loginType)
        {
            if (gameAccountId != -1)
            {
                SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(1, delegate
                {
                    var r = new UserInfoResult();
                    r.gameId = gameId;
                    r.gameAccountId = gameAccountId;
                    r.loginInfo = new List<PlatformAccountInfo>();
                    var info = new PlatformAccountInfo();
                    r.loginInfo.Add(info);
                    info.platform = "DEVICE";
                    info.hasLinked = true;
                    info.uniqeId = SystemInfo.deviceUniqueIdentifier;
                    info.iconUrl = "";
                    info.nickName = "";
                    SimpleSDKCallback.instance.unbindWithTypeAsyncSuccess(JsonUtility.ToJson(r));
                }));
            }
            else
            {
                var s = new State(-1, "not login");
                SimpleSDKCallback.instance.unbindWithTypeAsyncFail(JsonUtility.ToJson(s));
            }
        }

        public void getUserInfoAsync()
        {
            if(gameAccountId != -1)
            {
                SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(1, delegate
                {
                    var r = new UserInfoResult();
                    r.gameId = gameId;
                    r.gameAccountId = gameAccountId;
                    r.loginInfo = new List<PlatformAccountInfo>();
                    var info = new PlatformAccountInfo();
                    r.loginInfo.Add(info);
                    info.platform = "DEVICE";
                    info.hasLinked = true;
                    info.uniqeId = SystemInfo.deviceUniqueIdentifier;
                    info.iconUrl = "";
                    info.nickName = "";
                    SimpleSDKCallback.instance.getUserInfoAsyncSuccess(JsonUtility.ToJson(r));
                }));
            }
            else
            {
                var s  = new State(-1, "not login");
                SimpleSDKCallback.instance.getUserInfoAsyncFail(JsonUtility.ToJson(s));
            }
        }

        public ShopItemResult getShopItems()
        {
            return result;
        }

        public void getShopItemsAsync()
        {
            if(result != null)
            {
                SimpleSDKCallback.instance.getShopItemsAsyncSuccess(JsonUtility.ToJson(result));
            }
            else
            {

            }
        }

        public void startPayment(string itemId, string cpOrderId)
        {
            if (gameAccountId != -1)
            {

                if (SimpleSDK.instance.editorTestMockPaySucc)
                {
                    long ts = (long)SimpleSDKBase.NowTimestamp() * 1000;
                    long gameOrderId = ts + 987654321 ;
                    SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(3, delegate
                    {
                        var r = new StartPaymentResult();
                        r.gameOrderId = gameOrderId;
                        SimpleSDKCallback.instance.startPaymentSuccess(JsonUtility.ToJson(r));
                    }));

                    SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(3.5, delegate
                    {
                        //uncomsume item callback
                        var one = new UnconsumeItem();
                        one.gameOrderId = gameOrderId;
                        one.createTime = ts;
                        one.itemId = itemId;
                        one.purchaseTime = ts;
                        one.status = 1;
                        purchaseItems.unconsumeItems.Add(one);

                        SimpleSDKCallback.instance.getPurchaseItems(JsonUtility.ToJson(purchaseItems));
                    }));
                }
                else
                {
                    SimpleSDK.instance.StartCoroutine(SimpleSDKBase.ICoroutine(3, delegate
                    {
                        var s = new State(-1, "payment not success");
                        SimpleSDKCallback.instance.startPaymentFail(JsonUtility.ToJson(s));
                    }));
                }
            }
            else
            {
                var s = new State(-1, "user not login");
                SimpleSDKCallback.instance.startPaymentFail(JsonUtility.ToJson(s));
            }
        }

        public void startPaymentForSimpleGame(string itemId)
        {
            if (isLogin())
            {
                startPayment(itemId, "");
            }
            else
            {
                gameAccountId = 22345678;
                startPayment(itemId, "");
            }
        }

        public void consumeItem(long gameOrderId)
        {
            UnconsumeItem pickOne = null;
            foreach(var one in purchaseItems.unconsumeItems)
            {
                if(one.gameOrderId == gameOrderId)
                {
                    pickOne = one;
                }
            }
            if(pickOne!= null)
            {
                purchaseItems.unconsumeItems.Remove(pickOne);
            }
        }
    }
#endif
}
