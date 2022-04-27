using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace RdmNS
{
    public class RdmCallbackManager
    {
        
        RdmSdk rdmSdk;
        public RdmCallbackManager(RdmSdk rdmSdk)
        {
            this.rdmSdk = rdmSdk;
        }

        public void CollectCash(CanShowResult result)
        {
            rdmSdk.achievementManager.AddCash(result).Save();
        }

        // UI 回调上报审核，如果调用成功则扣减金钱
        private RdmApi.ReviewSuccessDelegate reviewSuccess;
        private RdmApi.ReviewFailDelegate reviewFail;
        private RdmItem reviewItem;

        public void SendReviewWithAllAmount(string email, string account, string currency, RdmApi.ReviewSuccessDelegate success, RdmApi.ReviewFailDelegate fail)
        {
            RdmItem newItem = new RdmItem();
            newItem.currency = currency;
            newItem.name = "AllAmount";
            var achievementManager = RdmSdk.GetInstance().achievementManager;
            double now = achievementManager.GetCash(currency);
            newItem.value = now;
            newItem.conditions = new  List<RdmItemCondition>();
            SendReviewByItem(email, account, newItem, success, fail);
        }

        public void SendReviewByItem(string email, string account, RdmItem item, RdmApi.ReviewSuccessDelegate success, RdmApi.ReviewFailDelegate fail)
        {
            reviewSuccess = success;
            reviewFail = fail;
                
            reviewItem = item;

            var cashAmount = rdmSdk.achievementManager.GetCash(item.currency);
            var achieves = rdmSdk.achievementManager.GetAllAchievement();
            var initInfo = rdmSdk.initInfo;
            var body = new Dictionary<string, object>();
            body.Add("gameId", initInfo.gameId);
            body.Add("packageName", Application.identifier);
            body.Add("system", RdmBase.GetSystem());
            body.Add("deviceId", initInfo.deviceId);
            body.Add("idfa", initInfo.idfa);
            {
                var cashInfo = new Dictionary<string, object>();
                cashInfo.Add("amount", cashAmount);
                cashInfo.Add("paypalAccount", account);
                cashInfo.Add("email", email);
                cashInfo.Add("itemName", item.name);
                cashInfo.Add("itemValue", item.value);
                cashInfo.Add("itemCurrency", item.currency);

                body.Add("cashInfo", cashInfo);
            }
            {
                body.Add("achievement", achieves);
            }

            var bodyJson = Json.Serialize(body);
            RdmBase.Log(RdmSdk.CASH_URL + " : " + bodyJson);
            RdmBase.HttpPostJson(rdmSdk, RdmSdk.CASH_URL, bodyJson, RdmSdk.AES_SECRET, SendReviewSuccess, SendReviewFail);

        }
        public void SendReviewSuccess(string result)
        {
            Debug.Log(result);
            var response = Json.Deserialize(result) as Dictionary<string, object>;
            var isSuccess = (bool)response["isSuccess"];
            if (isSuccess)
            {
                var data = (Dictionary<string, object>)response["data"];
                var reviewId = (string)data["reviewId"];
                var timestamp = Convert.ToInt32((long)data["timestamp"]);
                //save  reviewId
                rdmSdk.achievementManager.SubCash(reviewItem.currency, reviewItem.value).SaveReviewID(reviewId).Save();
                reviewSuccess(reviewId, timestamp);
            }
            else
            {
                var msg = (string)response["msg"];
                RdmBase.Log("review is not pass right now. reason:" + msg);
                reviewFail(-1, "review is not pass right now." + msg);
            }
        }
        public void SendReviewFail(long code,string errorMsg)
        {
            reviewFail(code, errorMsg);
        }
    }


}