using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RdmNS
{

    public class RdmApi
    {
        public delegate void OpenRdmSuccess(RdmConfig rdmConfig);

        public delegate void OpenRdmFail(string msg);

        public delegate void ReviewSuccessDelegate(string result, int timestamp);

        public delegate void ReviewFailDelegate(long code, string errorMsg);

        public delegate void GetTimestampSuccess(int timestamp);

        public delegate void GetTimestampFail(long code, string errorMsg);

        static public void Init(InitInfo initInfo, OpenRdmSuccess success, OpenRdmFail fail)
        {
            RdmSdk.Init(initInfo, success, fail);
        }

        static public RdmStatus GetRdmStatus()
        {
            return RdmSdk.rdmStatus;
        }

        //Send an http request to get the server timestamp
        //Please don't invoke this function in your update function or other high frequency sense
        static private GetTimestampSuccess gts;
        static private GetTimestampFail gtf;
        static public void GetTimestampFromNetwork(MonoBehaviour mono, GetTimestampSuccess gts, GetTimestampFail gtf)
        {
            RdmApi.gts = gts;
            RdmApi.gtf = gtf;
            RdmBase.HttpGET(mono, RdmSdk.TIME_UTL, SuccessCall, FailCall);
        }

        static private void SuccessCall(string result)
        {
            var d = Json.Deserialize(result) as Dictionary<string, object>;
            var ts = (long)d["timestamp"];
            if(gts != null)
            {
                gts((int)(ts / 1000));
            }
        }
        static private void FailCall(long code, string errorMsg)
        {
            if (gtf != null)
            {
                gtf(code, errorMsg);
            }
        }

        //接受事件,如果需要弹出则调用UI部分的弹出逻辑
        static public CanShowResult ShowCollectWithEvent(string currency, string name)
        {
            var rdmStatus = GetRdmStatus();
            if (rdmStatus == RdmStatus.RDM_OPEN)
            {
                var m = RdmSdk.GetInstance().rdmPopManager;
                if (m != null)
                {
                    return m.CanShow(currency, name);
                }
                else return CanShowResult.Fail("rdmPopManager is null");
            }
            else return CanShowResult.Fail("rdmsdk is not init");
        }

        //revice the cash

        static public void CollectCash(CanShowResult result)
        {
            var m = RdmSdk.GetInstance().rdmCallbackManager;
            if (m != null)
            {
                m.CollectCash(result);
            }
        }
        static public void AddCashWithType(CoinRuleType coinRuleType, string currency, double value)
        {
            var m = RdmSdk.GetInstance().achievementManager;
            if (m != null)
            {
                m.AddCashWithType(coinRuleType, currency, value).Save();
            }
        }

        static public double GetCash(string currency)
        {
            var m = RdmSdk.GetInstance().achievementManager;
            if (m != null)
            {
                return m.GetCash(currency);
            }
            else return 0;
        }
        //achieve day active

        static public int GetDayActive()
        {
            var m = RdmSdk.GetInstance().achievementManager;
            if (m != null)
            {
                return m.GetActiveDay();
            }
            else return 0;
        }

        static public void SetLevel(int level)
        {
            var m = RdmSdk.GetInstance().achievementManager;
            if (m != null)
            {
                m.SetLevel(level);
            }
        }
        static public int GetLevel()
        {
            var m = RdmSdk.GetInstance().achievementManager;
            if (m != null)
            {
                return m.GetLevel();
            }
            else return 0;
        }

        static public void SetTollgate(int level)
        {
            var m = RdmSdk.GetInstance().achievementManager;
            if (m != null)
            {
                m.SetTollgate(level);
            }
        }
        static public int GetTollgate()
        {
            var m = RdmSdk.GetInstance().achievementManager;
            if (m != null)
            {
                return m.GetTollgate();
            }
            else return 0;
        }


        static public void IncShowAdNumber()
        {
            var m = RdmSdk.GetInstance().achievementManager;
            if (m != null)
            {
                m.IncShowAdNumber();
            }
        }

        static public int GetShowAdNumber()
        {
            var m = RdmSdk.GetInstance().achievementManager;
            if (m != null)
            {
                return m.GetShowAdNumber();
            }
            else return 0;
        }

        //items
        static public List<string> GetAllCurrency()
        {
            var rdmStatus = GetRdmStatus();
            if (rdmStatus == RdmStatus.RDM_OPEN)
            {
                var m = RdmSdk.GetInstance().rdmConfig;
                return m.GetAllCurrency();
            }
            return null;
        }

        static public List<RdmItem> GetAllItems(string currency)
        {
            var rdmStatus = GetRdmStatus();
            if (rdmStatus == RdmStatus.RDM_OPEN)
            {
                var m = RdmSdk.GetInstance().rdmConfig;
                if (m != null)
                {
                    return m.GetAllItems(currency);
                }
            }
            return null;
        }

        static public RdmItem GetRdmItemByIndex(string currency, int index)
        {
            var rdmStatus = GetRdmStatus();
            if (rdmStatus == RdmStatus.RDM_OPEN)
            {
                var m = RdmSdk.GetInstance().rdmConfig;
                if (m != null)
                {
                    var list = m.GetAllItems(currency);
                    if(list != null && index < list.Count)
                    {
                        return list[index];
                    }
                }
            }
            return null;
        }

        //review
        static public void SendReviewByItem(string email, string account, RdmItem item, RdmApi.ReviewSuccessDelegate success, RdmApi.ReviewFailDelegate fail)
        {
            var rdmStatus = GetRdmStatus();
            if (rdmStatus == RdmStatus.RDM_OPEN)
            {
                var m = RdmSdk.GetInstance().rdmCallbackManager;
                if (m != null)
                {
                    m.SendReviewByItem(email, account, item, success, fail);
                }
            }
        }

        static public void SendReviewWithAllAmount(string email, string account, string currency, RdmApi.ReviewSuccessDelegate success, RdmApi.ReviewFailDelegate fail)
        {
            var rdmStatus = GetRdmStatus();
            if (rdmStatus == RdmStatus.RDM_OPEN)
            {
                var m = RdmSdk.GetInstance().rdmCallbackManager;
                if (m != null)
                {
                    m.SendReviewWithAllAmount(email, account, currency, success, fail);
                }
            }
        }

    }
}

