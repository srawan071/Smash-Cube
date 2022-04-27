using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSDKNS;
using System.Globalization;

namespace RdmNS
{
    public class DemoRewardListener : SimpleSDKRewardedVideoListener
    {
        

        public void onRewardedVideoAdPlayClicked(string unitId, CallbackInfo callbackInfo)
        {
            
        }

        public void onReward(string unitId, CallbackInfo callbackInfo)
        {

        }

        public void onRewardedVideoAdPlayClosed(string unitId, CallbackInfo callbackInfo)
        {
            
        }

        public void onRewardedVideoAdPlayFail(string adEntry, string code, string message)
        {
            
        }

        public void onRewardedVideoAdPlayStart(string unitId, CallbackInfo callbackInfo)
        {
            
        }
    }
    public class RdmDemo : BaseDemo
    {
        string currency = "coin";
        int itemId = 0;
        protected override void InnerStart()
        {
            //float a = 1.01f;
            //var ci = new CultureInfo("de-DE");
            //Debug.Log(a.ToString("R", ci));
            //Debug.Log(a.ToString("R", CultureInfo.InvariantCulture));
            AddButtion("start rdm", StartSDK);
            AddButtion("has rdm status", HasRdmStatus);
            AddButtion("to coin", ToCoin);
            AddButtion("to diamond", ToDiamond);
            AddButtion("send freepp and watch", sendFreeppAndWatch);
            AddButtion("send freepp and ignore", sendFreeppAndIgnore);
            AddButtion("send showad and watch", sendShowadAndWatch);
            AddButtion("send showad and ignore", sendShowadAndIgnore);
            AddButtion("send doublead and watch", sendDoubleadAndWatch);
            AddButtion("send shodoublead and ignore", sendDoubleadAndIgnore);
            AddButtion("showAllItem", showAllItem);
            AddButtion("incItemIndex", incItemIndex);
            AddButtion("SendReviewByItem", SendReviewByItem);
            AddButtion("SendReviewWithAllAmount", SendReviewWithAllAmount);
            AddButtion("show item detail", showItemDetail);
            AddButtion("show cash", showCash);
            AddButtion("show all", showAll);
            AddButtion("clean", clean);
            AddButtion("clear", clear);
            AddButtion("test", test);
        }
        public void StartSDK()
        {
            SimpleSDKAd.instance.SetSimpleSDKRewardedVideoListener(new DemoRewardListener());
            SimpleSDK.instance.SetAttributionInfoListener(StartRdmInCallback);
        }

        public void StartRdmInCallback(AttributionInfo attributionInfo)
        {
            var initInfo = new InitInfo(SimpleSDK.instance.GetStaticInfo(), attributionInfo.network );
            RdmApi.Init(initInfo, RdmInitSuccess, RdmInitFail);
        }
        public void RdmInitSuccess(RdmConfig rdmConfig)
        {
            GuiLog("init the rmd success");
        }
        public void RdmInitFail(string msg)
        {
            GuiLog("init the rmd fail");
        }

        public void HasRdmStatus()
        {
            GuiLog("has rdm:" + RdmSdk.rdmStatus.ToString()+" returnType:"+RdmSdk.GetInstance().GetReturnType());
        }

        public void ToCoin()
        {
            this.currency = "coin";
            GuiLog("set to coin");
        }

        public void ToDiamond()
        {
            this.currency = "diamond";
            GuiLog("set to diamond");
        }

        public void sendFreeppAndWatch()
        {
            SendEvent("freepp", true);
        }
        public void sendFreeppAndIgnore()
        {
            SendEvent("freepp", false);
        }

        public void sendShowadAndWatch()
        {
            SendEvent("showad", true);
        }
        public void sendShowadAndIgnore()
        {
            SendEvent("showad", false);
        }

        public void sendDoubleadAndWatch()
        {
            SendEvent("doublead", true);
        }
        public void sendDoubleadAndIgnore()
        {
            SendEvent("doublead", false);
        }


        private void SendEvent(string eventName, bool watch)
        {
            var re = RdmApi.ShowCollectWithEvent(currency, eventName);
            if (re.canShow)
            {
                if(re.pickCondition == PickCondition.doubleRule)
                {
                    //SHOW COLLECT PANEL
                    if (watch)
                    {
                        //double need to change the event's amount
                        re.value *= 2;
                        RdmApi.CollectCash(re);
                        GuiLog("call add cash " + re.value);
                    }
                    else
                    {
                        RdmApi.CollectCash(re);
                        GuiLog("call add cash " + re.value);
                    }
                }
                else if(re.pickCondition == PickCondition.ad)
                {
                    //SHOW COLLECT PANEL
                    if (watch)
                    {
                        RdmApi.CollectCash(re);
                        GuiLog("call add cash " + re.value);
                    }
                    else
                    {
                        GuiLog("don't watch ad and miss the cash " + re.value);
                    }
                }
                else if(re.pickCondition == PickCondition.direct)
                {
                    //SHOW COLLECT PANEL
                    RdmApi.CollectCash(re);
                    GuiLog("call add cash " + re.value);
                }
                else if(re.pickCondition == PickCondition.nopop) {
                    //DON'T SHOW COLLECT PANEL, DIRECT TO CALL THE COLLECT CASH
                    RdmApi.CollectCash(re);
                    GuiLog("call add cash " + re.value);
                }
                else
                {
                    GuiLog("unknow pick condition " + re.pickCondition);
                }
            }
            else
            {
                GuiLog("fail to pop.reason is:" + re.reason);
            }
        }
        private void showAllItem()
        {
            var items = RdmApi.GetAllItems(currency);
            for(int i=0;i<items.Count;i++)
            {
                var one = items[i];
                GuiLog("item " + i + " " + one.name + " " + one.value + " " + one.currency);
            }

            //example
        }
        private void incItemIndex()
        {
            itemId++;
            if (itemId > 2) itemId = 0;
            GuiLog("set itemId " + itemId);
        }
        private void SendReviewByItem()
        {
            GuiLog("review item " + itemId + " " + currency);
            var item = RdmApi.GetRdmItemByIndex(currency, itemId);
            RdmApi.SendReviewByItem(
                "user@123.com",
                "user@456.com",
                item,
                ReviewSuccess,
                ReviewFail
                );
        }
        private void SendReviewWithAllAmount()
        {
            GuiLog("review " + currency);
            RdmApi.SendReviewWithAllAmount(
                "user@123.com",
                "user@456.com",
                currency,
                ReviewSuccess,
                ReviewFail
                );
        }

        
        private void ReviewSuccess(string result, int timestamp)
        {
            GuiLog("review success " + result + " " + timestamp);
        }

        private void ReviewFail(long code, string errorMsg)
        {
            GuiLog("review fail" + code + " " + errorMsg);
        }

        private void showItemDetail()
        {
            var item = RdmApi.GetRdmItemByIndex(currency, itemId);
            GuiLog(item.name + " " + item.value + " " + item.currency);
        }

        private void showCash()
        {
            var value = RdmApi.GetCash(this.currency);
            GuiLog(currency + " " + value);
        }

        private void showAll()
        {
            var log = "";
            var value = RdmSdk.GetInstance().achievementManager.GetAllAchievement();
            foreach(var v in value)
            {
                log += v.Key + " " + v.Value + "\n";
            }
            GuiLog(log);
        }
        private void clean()
        {
            RdmSdk.GetInstance().achievementManager.ClearCash(currency);
        }
        private void clear()
        {
            PlayerPrefs.DeleteAll();
        }
        private void test()
        {
            CanShowResult re = RdmApi.ShowCollectWithEvent("Coin", "TaskCo1");
            Debug.Log(re.canShow);
            Debug.Log(re.reason);
        }
    }
}