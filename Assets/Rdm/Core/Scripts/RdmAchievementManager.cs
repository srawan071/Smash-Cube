using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleSDKNS;

namespace RdmNS { 
    public class RdmAchievementManager
    {
        private int startTimestamp;
        private int serverTimestamp;
        static public string ACTIVE_DAY = "active_day";
        static public string LAST_DATE = "last_date";

        static public string CASH_AMOUNT = "cash_amount";
        static public string RAND_CASH_AMOUNT = "rand_cash_amount";

        static public string SHOW_AD_NUMBER = "show_ad_number";
        static public string REVIEW_ID = "review_ids";
        static public string CP_LEVEL = "cp_level";
        static public string CP_TOLLGATE = "cp_tollgate";
        static public string OPEN_TIMES = "open_times";

        static private string GetCurrencyKey(string currency)
        {
            return CASH_AMOUNT + "_" + currency;
        }

        static private string GetRandCurrencyKey(string currency)
        {
            return RAND_CASH_AMOUNT + "_" + currency;
        }

        //private RdmStore.sint activeDay = new RdmStore.sint("active_day", 0);
        //private RdmStore.sstring lastDate = new RdmStore.sstring("last_date", "0000-00-00");
        public RdmAchievementManager(MonoBehaviour mono)
        {
            IncOpenTimes();
            RdmApi.GetTimestampFromNetwork(mono, GetTimeSuccess, GetTimeFail);
        }
        public void GetTimeSuccess(int ts)
        {
            var dto = RdmBase.FromTimestamp(ts);
            var date = dto.ToLocalTime().ToString("yyyy-MM-dd");
            RdmBase.Log("call timestamp succes and get " + date);
            ActiveDay(date);
        }
        private void GetTimeFail(long code, string errorMsg)
        {
            RdmBase.Log("cal timestamp error "+code+" "+errorMsg);
        }
        private void ActiveDay(string activeDate)
        {
            int activeDayNow = RdmStore.GetInt(ACTIVE_DAY, 0);
            if(activeDayNow == 0)
            {
                //first init
                RdmStore.SetInt(ACTIVE_DAY, 1);
                RdmStore.SetString(LAST_DATE, activeDate);
                RdmStore.Save();
            }
            else
            {
                var lastDateNow = RdmStore.GetString(LAST_DATE, "0000-00-00");
                if (activeDate.CompareTo(lastDateNow) != 0)
                {
                    RdmBase.Log("date has updated and reset some achieve");
                    RdmStore.AddInt(ACTIVE_DAY, 1);
                    RdmStore.SetString(LAST_DATE, activeDate);
                    RdmStore.Save();
                }
            }
        }

        public RdmAchievementManager AddCashWithType(CoinRuleType coinRuleType, string currency, double value)
        {
            RdmStore.AddDouble(GetCurrencyKey(currency), value);
            double randAddV = 0;
            if (coinRuleType == CoinRuleType.Rand)
            {
                randAddV = value;
                RdmStore.AddDouble(GetRandCurrencyKey(currency), value);
            }
            //
            if(SimpleSDKBase.debug)
            {
                var nowTotalV  = RdmStore.GetDouble(GetCurrencyKey(currency), 0);
                var nowRandV = RdmStore.GetDouble(GetRandCurrencyKey(currency), 0);
                RdmBase.Log("adding cash type:"+ coinRuleType.ToString()+ " total add:" + value + " now:" + nowTotalV + " rand add:" + randAddV + " now:" + nowRandV);
            }
            return this;
        }

        public RdmAchievementManager AddCash(CanShowResult result)
        {
            return AddCashWithType(result.coinRuleType, result.currency, result.value);
        }

        public RdmAchievementManager SubCash(string currency,double cash)
        {
            RdmStore.AddDouble(GetCurrencyKey(currency), -cash);
            var now = RdmStore.GetDouble(GetRandCurrencyKey(currency), 0);
            var want = now - cash;
            if (want < 0)
            {
                RdmStore.SetDouble(GetRandCurrencyKey(currency), 0);
            }
            else
            {
                RdmStore.SetDouble(GetRandCurrencyKey(currency), want);
            }

            if (SimpleSDKBase.debug)
            {
                var nowTotalV = RdmStore.GetDouble(GetCurrencyKey(currency), 0);
                var nowRandV = RdmStore.GetDouble(GetRandCurrencyKey(currency), 0);
                RdmBase.Log("subing cash  add:" + cash + " total  now:" + nowTotalV + " rand now:" + nowRandV);
            }
            return this;
        }

        public double GetCash(string currency)
        {
            return RdmStore.GetDouble(GetCurrencyKey(currency), 0);
        }

        public double GetRandCash(string currency)
        {
            return RdmStore.GetDouble(GetRandCurrencyKey(currency), 0);
        }

        public void ClearCash(string currency)
        {
            RdmStore.SetDouble(GetCurrencyKey(currency), 0);
            RdmStore.SetDouble(GetRandCurrencyKey(currency), 0);
        }

        public int GetOpenTimes()
        {
            return RdmStore.GetInt(OPEN_TIMES, 0);
        }
        public void IncOpenTimes()
        {
            RdmStore.AddInt(OPEN_TIMES, 1);
        }

        public void SetLevel(int level)
        {
            RdmStore.SetInt(CP_LEVEL, level);
        }
        public int GetLevel()
        {
            return RdmStore.GetInt(CP_LEVEL, 0);
        }

        public void SetTollgate(int tollgate)
        {
            RdmStore.SetInt(CP_TOLLGATE, tollgate);
        }
        public int GetTollgate()
        {
            return RdmStore.GetInt(CP_TOLLGATE, 0);
        }

        public int GetActiveDay()
        {
            return RdmStore.GetInt(ACTIVE_DAY, 0);
        }

        public void IncShowAdNumber()
        {
            RdmStore.AddInt(SHOW_AD_NUMBER, 1);
        }

        public int GetShowAdNumber()
        {
            return RdmStore.GetInt(SHOW_AD_NUMBER, 0);
        }

        public RdmAchievementManager SaveReviewID(string reviewID)
        {
            string now = RdmStore.GetString(REVIEW_ID, "");
            RdmStore.SetString(REVIEW_ID, now + "," + reviewID);
            return this;
        }

        public Dictionary<string, string> GetAllAchievement()
        {
            return RdmStore.GetAll();
        }

        public void Save()
        {
            RdmStore.Save();
        }
    }
}