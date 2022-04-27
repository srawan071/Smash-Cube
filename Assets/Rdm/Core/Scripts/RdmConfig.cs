using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace RdmNS
{   
    public class RdmItemCondition
    {
        public string name;
        public int value;
        public string textId;
        public string op; //lessThan moreThan less more equal
        public bool IsReach()
        {
            var nowValue = GetNowValue();
            return nowValue >= value;
        }
        private int GetNowValue()
        {
            var achievementManager = RdmSdk.GetInstance().achievementManager;
            if (name == RdmAchievementManager.ACTIVE_DAY) return achievementManager.GetActiveDay();
            else if (name == RdmAchievementManager.CP_LEVEL) return achievementManager.GetLevel();
            else if (name == RdmAchievementManager.SHOW_AD_NUMBER) return achievementManager.GetShowAdNumber();
            else return 0;
        }
    }
    public class RdmItem
    {
        public string currency;
        public string name;
        public double value;
        public List<RdmItemCondition> conditions;
        public bool CanBuy()
        {
            var achievementManager = RdmSdk.GetInstance().achievementManager;
            double now = achievementManager.GetCash(currency);
            if (now >= value)
            {
                foreach(var condition in conditions)
                {
                    if (!condition.IsReach()) return false;
                }
                return true;
            }
            else return false;
        }
    }
    public enum PickCondition
    {
        ad,
        direct,
        doubleRule,
        nopop
    }
    public class PopRule
    {
        public string name;
        public PickCondition pickCondition;
        public string param;
        public string eventName;
        public string coinRuleName;
        public string coinNum;
    }
    public class CashRule
    {
        public string name;
        public List<string> rules;
    }

    public class CurrencyConfig
    {
        public string currency;
        public bool isFloat;
        public List<RdmItem> items;
        public List<PopRule> popRules;
        public CashRule cashRule;
    }
    public class RdmConfig
    {
        public bool isSuccess;
        public string returnType;
        public Dictionary<string, CurrencyConfig> currencyMap = new Dictionary<string, CurrencyConfig>();

        public List<string> GetAllCurrency()
        {
            var re = new List<string>();
            foreach(var pair in currencyMap)
            {
                re.Add(pair.Key);
            }
            return re;
        }

        public List<RdmItem> GetAllItems(string currency)
        {
            if (currencyMap.ContainsKey(currency))
            {
                return currencyMap[currency].items;
            }
            else return null;
        }

        static public RdmConfig Parse(string jsonStr)
        {
            RdmBase.Log("ready to parse " + jsonStr);
            var dict = RdmNS.Json.Deserialize(jsonStr) as Dictionary<string, object>;
            try
            {
                if (dict != null)
                {
                    var isSuccess = (bool)dict["isSuccess"];
                    var rdmConfig = new RdmConfig();
                    rdmConfig.isSuccess = isSuccess;
                    if (isSuccess)
                    {
                        var data = (Dictionary<string, object>)dict["data"];
                        if(data.ContainsKey("returnType"))
                        {
                            rdmConfig.returnType = (string)data["returnType"];
                        }
                        else
                        {
                            rdmConfig.returnType = "pass";
                        }
                        
                        var currencyConfigs = (List<object>)data["mulit_currency"];
                        foreach(var one in currencyConfigs)
                        {
                            var c = new CurrencyConfig();
                            var jsonOne = (Dictionary<string, object>)one;
                            c.currency = (string)jsonOne["currency"];
                            c.isFloat = (((string)jsonOne["is_float"]) == "1");
                            c.items = ParseItems(c.currency, (List<object>)jsonOne["item_configs"]);

                            c.popRules = ParsePopRules((List<object>)jsonOne["pop_rules"]);
                            c.cashRule = ParseCashRule((Dictionary<string, object>)jsonOne["cash_rule"]);

                            rdmConfig.currencyMap[c.currency] = c;
                        }

                        // do something
                        RdmBase.Log("rdm:get api correct and parse finish "+rdmConfig);
                        return rdmConfig;
                    }
                    else
                    {
                        RdmBase.Log("rdm:get config but isSuccess is false");
                        return rdmConfig;
                    }
                }
                else
                {
                    RdmBase.Log("rdm:parse json error " + jsonStr);
                    return null;
                }
            }
            catch (Exception e)
            {
                RdmBase.LogException("rdm:parse json error " + jsonStr , e);
                return null;
            }
        }
        static private List<RdmItem> ParseItems(string currency, List<object> items)
        {
            var tempList = new List<RdmItem>();
            foreach(object i in items)
            {
                var oneItem = (Dictionary<string, object>)i;
                RdmItem temp = new RdmItem();
                temp.currency = currency;
                temp.name = (string)oneItem["item_name"];
                temp.value = RdmBase.ParseDouble((string)oneItem["value"]);
                {
                    temp.conditions = new List<RdmItemCondition>();
                    var conditions = (List<object>)oneItem["item_conditions"];
                    foreach (object j in conditions)
                    {
                        var oneCon = (Dictionary<string, object>)j;
                        RdmItemCondition oneCondition = new RdmItemCondition();
                        oneCondition.name = (string)oneCon["name"];
                        oneCondition.value = int.Parse((string)oneCon["param"]);
                        oneCondition.op = (string)oneCon["op"];
                        temp.conditions.Add(oneCondition);
                    }
                }
                tempList.Add(temp);
            }
            return tempList;
        }
        static private List<PopRule> ParsePopRules(List<object> popRules)
        {
            var tempList = new List<PopRule>();
            foreach (object i in popRules)
            {
                var oneItem = (Dictionary<string, object>)i;
                PopRule temp = new PopRule();
                temp.name = (string)oneItem["name"];
                temp.pickCondition = ParsePickCondition((string)oneItem["pick_condition"]);
                temp.param = (string)oneItem["param"];
                temp.eventName = GetValue(oneItem, "event");
                temp.coinRuleName = GetValue(oneItem, "coin_rule_name");
                temp.coinNum = GetValue(oneItem, "coin_num");
                tempList.Add(temp);
            }
            return tempList;
        }

        static private PickCondition ParsePickCondition(string name)
        {
            if (name.Equals(PickCondition.ad.ToString())) return PickCondition.ad;
            if (name.Equals(PickCondition.direct.ToString())) return PickCondition.direct;
            if (name.Equals("double")) return PickCondition.doubleRule;
            if (name.Equals(PickCondition.nopop.ToString())) return PickCondition.nopop;
            return PickCondition.ad;
        }
        
        static private CashRule ParseCashRule(Dictionary<string,object> cashRules)
        {
            CashRule temp = new CashRule();
            temp.name = (string)cashRules["name"];
            temp.rules = new List<string>();
            var rules = (List<object>)cashRules["params"];
            foreach(object o in rules)
            {
                var one = (Dictionary<string, object>)o;
                var param = (string)one["param"];
                temp.rules.Add(param);
            }
            return temp;
        }

        static private string GetValue(Dictionary<string, object> temp, string key)
        {
            if (temp.ContainsKey(key))
            {
                return (string)temp[key];
            }
            else return "";
        }  
    }
}