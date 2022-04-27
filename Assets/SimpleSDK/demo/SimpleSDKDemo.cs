using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json;
namespace SimpleSDKNS
{

    public class SimpleSDKDemo : MonoBehaviour
    {
        private int count;
        private List<string> logs = new List<string>() { };
        int maxLogsCount = 30;

        //int startPaymentIndex = 0;
        int buttonWith = 300;
        int buttonHeight = 80;
        string myGameContentVersion = "1.0";

        static public bool onFront = true;

        int screenWidth;
        int screenHeight;
        private Vector2 btnScrollPosition;
        private Vector2 logScrollPosition;
        private int totalScrollHeight = 1000;

        private ShopItemResult shopItemResult = null;

        public class DemoRewardListener : SimpleSDKRewardedVideoListener
        {
            private SimpleSDKDemo demo;
            public DemoRewardListener(SimpleSDKDemo demo)
            {
                this.demo = demo;
            }
            public void onRewardedVideoAdPlayStart(string adEntry, CallbackInfo callbackInfo)
            {
                demo.log("reward video start");
            }
            public void onRewardedVideoAdPlayFail(string adEntry, string code, string message)
            {
                demo.log("reward video play fail");
            }
            public void onRewardedVideoAdPlayClicked(string adEntry, CallbackInfo callbackInfo)
            {
                demo.log("reward video click");
            }

            public void onRewardedVideoAdPlayClosed(string unitId, CallbackInfo callbackInfo)
            {
                demo.log("reward video close");
            }

            public void onReward(string unitId, CallbackInfo callbackInfo)
            {
                demo.log("give reward in onReward");
            }
        }

        public class DemoInterListener : SimpleSDKInterstitialAdListener
        {
            private SimpleSDKDemo demo;
            public DemoInterListener(SimpleSDKDemo demo)
            {
                this.demo = demo;
            }
            public void onInterstitialAdShow(string adEntry, CallbackInfo callbackInfo)
            {
                demo.log("Interstitial ad show");
            }
            /***
             * 广告关闭
             * @param unitId 广告位id
             */
            public void onInterstitialAdClose(string adEntry, CallbackInfo callbackInfo)
            {
                demo.log("Interstitial close");
            }
            /***
             * 广告点击
             * @param unitId 广告位id
             */
            public void onInterstitialAdClick(string adEntry, CallbackInfo callbackInfo)
            {
                demo.log("Interstitial ad click");
            }
        }

        public class DemoBannerListener : SimpleSDKBannerAdListener
        {
            private SimpleSDKDemo demo;
            public DemoBannerListener(SimpleSDKDemo demo)
            {
                this.demo = demo;
            }
            public void onAdClick(string unitId, CallbackInfo callbackInfo)
            {
                demo.log("banner ad click");
            }

            public void onAdImpress(string unitId, CallbackInfo callbackInfo)
            {
                demo.log("banner ad click");
            }
        }


        public class DemoPurchaseListener : IPurchaseItemsListener
        {
            private SimpleSDKDemo demo;
            public DemoPurchaseListener(SimpleSDKDemo demo)
            {
                this.demo = demo;
            }
            public void getPurchaseItems(PurchaseItems purchaseItems)
            {
                foreach(var one in purchaseItems.unconsumeItems)
                {
                    demo.log("find unconsume item" + one.itemId + " " + one.gameOrderId+" and ready to consume");
                    SimpleSDKUserPayment.instance.consumeItem(one.gameOrderId);
                    demo.log("success to unconsume item" + one.itemId + " " + one.gameOrderId);
                }
            }
        }

        void Start()
        {
            screenWidth = UnityEngine.Screen.width;
            screenHeight = UnityEngine.Screen.height;
#if !UNITY_EDITOR
    		buttonWith = screenWidth / 2 ;
    		buttonHeight = screenHeight / 13;
#endif
            btnScrollPosition = Vector2.zero;
            logScrollPosition = Vector2.zero;

            //the sdkOrderIds should save in the local or cloud
            HashSet<string> hasSendedSdkOrders = new HashSet<string>();

            SimpleSDK.instance.SetAttributionInfoListener(AttationInfoCallback);


            //ad listener
            SimpleSDKAd.instance.SetSimpleSDKRewardedVideoListener(new DemoRewardListener(this));
            SimpleSDKAd.instance.SetSimpleSDKInterstitialAdListener(new DemoInterListener(this));
            SimpleSDKAd.instance.SetSimpleSDKBannerAdListener(new DemoBannerListener(this));

            //item listener
            SimpleSDKUserPayment.instance.setIPurchaseItemsListener(new DemoPurchaseListener(this));

        }

        void OnGUI()
        {
            if (!onFront)
            {
                //if it is not on front stop draw the gui
                //Debug.Log("no on front return");
                return;
            }
            while (logs.Count > maxLogsCount)
            {
                logs.RemoveAt(0);
            }

            GUIStyle fontStyle = new GUIStyle();
            fontStyle.normal.background = null;
            fontStyle.normal.textColor = new Color(1, 1, 1);
            fontStyle.fontSize = 30;
            fontStyle.wordWrap = true;

            GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
            btnStyle.alignment = TextAnchor.MiddleCenter;
            btnStyle.fontSize = 28;
            btnStyle.normal.textColor = Color.white;
            btnStyle.wordWrap = true;


            string l = "";
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                l += logs[i] + "\n*************\n";
            }

            logScrollPosition = GUI.BeginScrollView(new Rect(5, 0, Screen.width - 5, Screen.height / 4), logScrollPosition, new Rect(5, 0, Screen.width - 10, 2000), false, false);
            GUI.Label(new Rect(5, 0, Screen.width - 15, Screen.height / 4), l, fontStyle);
            GUI.EndScrollView();

            //string playerItemStr = "player packages:\n";
            //foreach (var pair in playerPackages)
            //{
            //    playerItemStr += pair.Key + " : " + pair.Value + " \n";
            //}

            int y = Screen.height / 4 + 5;

            //GUI.Label(new Rect(buttonWith + 10, y + 10, Screen.width - buttonWith - 20, 150), playerItemStr, fontStyle);

            btnScrollPosition = GUI.BeginScrollView(new Rect(0, y, Screen.width, Screen.height - Screen.height / 4), btnScrollPosition, new Rect(0, y, Screen.width, totalScrollHeight), false, false);


            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "hasReward", btnStyle))
            {
                var result = SimpleSDKAd.instance.HasReward();
                log("call hasRewardedVideo " + result);
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "showRewardVideoAd", btnStyle))
            {
                log("call showRewardVideoAd ");
                SimpleSDKAd.instance.ShowReward("wheel");
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "hasInterstitial", btnStyle))
            {
                var result = SimpleSDKAd.instance.HasInterstitial();
                log("call hasInterstitial " + result + " ");
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "showInterstitialAd", btnStyle))
            {
                log("call showInterstitialAd ");
                SimpleSDKAd.instance.ShowInterstitial("wheel");
            }


            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "showBannerAd top", btnStyle))
            {
                log("call showBannerAd top");
                SimpleSDKAd.instance.ShowOrReShowBanner(BannerPos.TOP);
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "showBannerAd bottom", btnStyle))
            {
                log("call showBannerAd bottom");
                SimpleSDKAd.instance.ShowOrReShowBanner(BannerPos.BOTTOM);
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "hideBannerAd", btnStyle))
            {
                log("call dismissBannerAd ");
                SimpleSDKAd.instance.HideBanner();
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "removeBannerAd", btnStyle))
            {
                log("call dismissBannerAd ");
                SimpleSDKAd.instance.RemoveBanner();
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "testLog", btnStyle))
            {
                log("call testLog ");
                Dictionary<string, string> paramsDic = new Dictionary<string, string>();
                paramsDic.Add("paramA", "valueA");
                paramsDic.Add("paramB", "valueB");
                SimpleSDK.instance.Log("unityLogTest", paramsDic);
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "pay success", btnStyle))
            {
                log("call testLog ");
                SimpleSDK.instance.LogPaySuccess("google play", "transactionID", "productID", DateTime.Now,
                    (decimal)0.99, "$ 0.99", "USD");
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "print attribution info", btnStyle))
            {
                var info = SimpleSDK.instance.GetAttributionInfo();
                if (info != null)
                {
                    log("info " + info.ToJson());
                }
                else
                {
                    log("info is null");
                }
            }
            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "pring loadding status", btnStyle))
            {
                var info = SimpleSDKAd.instance.GetLoadingStatusSummary();
                if (info != null)
                {
                    log("info " + Json.Serialize(info) );
                }
                else
                {
                    log("info is null");
                }
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "pring statics info", btnStyle))
            {
                var info = SimpleSDK.instance.GetStaticInfo();
                if (info != null)
                {
                    log("info " + info.toJson());
                }
                else
                {
                    log("info is null");
                }
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "autologin", btnStyle))
            {
                SimpleSDKUserPayment.instance.autoLoginAsync(true, delegate (AutoLoginResult r) {
                    log("autologin success " + JsonUtility.ToJson(r));

                }, delegate (State s) {
                    log("autologin fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "check login", btnStyle))
            {
                SimpleSDKUserPayment.instance.checkLoginAsync(delegate(CheckLoginResult r) {
                    log("check login success " + JsonUtility.ToJson(r));

	            }, delegate(State s ) {
                    log("check login fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "login with device", btnStyle))
            {
                SimpleSDKUserPayment.instance.loginWithTypeAsync(LOGIN_TYPE.DEVICE, delegate (LoginResult r) {
                    log("login success " + JsonUtility.ToJson(r));

                }, delegate (State s) {
                    log("login fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "login with facebook", btnStyle))
            {
                SimpleSDKUserPayment.instance.loginWithTypeAsync(LOGIN_TYPE.FACEBOOK, delegate (LoginResult r) {
                    log("login success " + JsonUtility.ToJson(r));

                }, delegate (State s) {
                    log("login fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "login with google play or game center", btnStyle))
            {
                LOGIN_TYPE t = LOGIN_TYPE_HELPER.GetLoginTypeWithSyste();
                SimpleSDKUserPayment.instance.loginWithTypeAsync(t, delegate (LoginResult r) {
                    log("login success " + JsonUtility.ToJson(r));

                }, delegate (State s) {
                    log("login fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "bind with device", btnStyle))
            {
                SimpleSDKUserPayment.instance.bindWithTypeAsync(LOGIN_TYPE.DEVICE, delegate (UserInfoResult r) {
                    log("bind success " + JsonUtility.ToJson(r));

                }, delegate (State s) {
                    log("bind fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "bind with facebook", btnStyle))
            {
                SimpleSDKUserPayment.instance.bindWithTypeAsync(LOGIN_TYPE.FACEBOOK, delegate (UserInfoResult r) {
                    log("bind success " + JsonUtility.ToJson(r));

                }, delegate (State s) {
                    log("bind fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "bind with google play or game center", btnStyle))
            {
                LOGIN_TYPE t = LOGIN_TYPE_HELPER.GetLoginTypeWithSyste();
                SimpleSDKUserPayment.instance.bindWithTypeAsync(t, delegate (UserInfoResult r) {
                    log("bind success " + JsonUtility.ToJson(r));

                }, delegate (State s) {
                    log("bind fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "unbind with device", btnStyle))
            {
                SimpleSDKUserPayment.instance.unbindWithTypeAsync(LOGIN_TYPE.DEVICE, delegate (UserInfoResult r) {
                    log("unbind success " + JsonUtility.ToJson(r));

                }, delegate (State s) {
                    log("unbind fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "unbind with facebook", btnStyle))
            {
                SimpleSDKUserPayment.instance.unbindWithTypeAsync(LOGIN_TYPE.FACEBOOK, delegate (UserInfoResult r) {
                    log("unbind success " + JsonUtility.ToJson(r));

                }, delegate (State s) {
                    log("unbind fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "unbind with google play or game center", btnStyle))
            {
                LOGIN_TYPE t = LOGIN_TYPE_HELPER.GetLoginTypeWithSyste();
                SimpleSDKUserPayment.instance.unbindWithTypeAsync(t, delegate (UserInfoResult r) {
                    log("unbind success " + JsonUtility.ToJson(r));

                }, delegate (State s) {
                    log("unbind fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "get user info", btnStyle))
            {
                log("gameAccountId is " + SimpleSDKUserPayment.instance.getGameAccountId());
                SimpleSDKUserPayment.instance.getUserInfoAsync(delegate (UserInfoResult r) {
                    log("get user info success " + JsonUtility.ToJson(r));

                }, delegate (State s) {
                    log("get user info fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "logout", btnStyle))
            {
                SimpleSDKUserPayment.instance.Logout();
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "getItemSync", btnStyle))
            {
                SimpleSDKUserPayment.instance.getShopItemsAsync(delegate (ShopItemResult r) {
                    log("get item success " + JsonUtility.ToJson(r));
                    shopItemResult = r;

                }, delegate (State s) {
                    log("get item fail " + JsonUtility.ToJson(s));
                });
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "getItem", btnStyle))
            {
                var result = SimpleSDKUserPayment.instance.getShopItems();
                if (result != null) { 
                    log("get item success " + JsonUtility.ToJson(result));
                    shopItemResult = result;
                }
                else
                {
                    log("item is null try later ");
                }
            }

            if(shopItemResult!= null && shopItemResult.items !=null)
            {
                foreach(var one in shopItemResult.items)
                {
                    y += buttonHeight + 10;
                    if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "buy item "+one.itemId+" "+one.formattedPrice, btnStyle))
                    {
                        SimpleSDKUserPayment.instance.startPayment(one.itemId, "cporderId", delegate (StartPaymentResult r) {
                            log("payment is success " + r.gameOrderId);

                        }, delegate (State s) {
                            log("payment is fail " + JsonUtility.ToJson(s));
                        });
                    }
                }
            }

            y += buttonHeight + 10;
            if (GUI.Button(new Rect(0, y, buttonWith, buttonHeight), "buy item item_id1", btnStyle))
            {
                SimpleSDKUserPayment.instance.startPaymentForSimpleGame("item_id1", delegate (StartPaymentResult r) {
                    log("payment is success " + r.gameOrderId);

                }, delegate (State s) {
                    log("payment is fail " + JsonUtility.ToJson(s));
                });
            }
            GUI.EndScrollView();

            totalScrollHeight = y + 100;

            //		//supp
            //		if (showRward) {
            //			if (currentTs > rewardTs) {
            //				showRward = false;
            //			} else {
            //				GUI.Box (new Rect (0, 0, Screen.width, Screen.height, "showing reward"));
            //				if (GUI.Button (new Rect (100, 100, 100, 100), "clickAd")) );
            //					log ("click Ad");
            //					Mopub
            //				}
            //			}
            //		}
            //		//show all buy items

        }

        public void AttationInfoCallback(AttributionInfo info)
        {
            log("get Attribution  " + info.ToJson() );
        }
        public void log(string s)
        {
            Debug.Log(s);
            logs.Add(s);
        }
        void Update()
        {
            if (Input.touchCount > 0)
            {

                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Moved)
                {
                    if (touch.position.y < Screen.height / 4 * 3)
                    {
                        btnScrollPosition.y += touch.deltaPosition.y;
                    }
                    else
                    {
                        logScrollPosition.x -= touch.deltaPosition.x;
                        logScrollPosition.y += touch.deltaPosition.y;
                    }
                }
            }
        }

    }

}