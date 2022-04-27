using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;


namespace RdmNS
{
    public enum CashRuleManagerType
    {
        Unknow, CostTotalCashRule, RandCostTotalCashRule
    }
    // -- CashRuleManager --
    public struct ValueLimit
    {
        public CashRuleManagerType cashRuleManagerType;
        public double minLimit;
        public double maxLimit;
    }
    public interface CashRuleManager
    {
        ValueLimit GetNextValueLimit();
    }
    public class CostTotalParam
    {
        public double minAmount;
        public double randMin;
        public double randMax;
    }
    public class ConfigParser
    {
        static public CashRuleManager ParseCashRule(string currency, CashRule cashRule)
        {
            switch (cashRule.name)
            {
                case "cost_total": return new CostTotalCashRule(currency, cashRule);
                case "rand_cost_total": return new RandCostTotalCashRule(currency, cashRule);
            }
            return null;
        }
        static public EventPopManager ParsePopRule(PopRule popRule)
        {
            switch (popRule.name)
            {
                case "event_with_interval": return new EventWithInterval(popRule);
                case "event_with_cooldown": return new EventWithCoolDown(popRule);
                case "event_with_rate": return new EventWithRate(popRule);
            }
            return null;
        }
        static public CoinRule ParseCoinRule(PopRule popRule)
        {
            switch (popRule.coinRuleName)
            {
                case "rand_coin": return new RandCoinRule();
                case "fixed_coin": return new FixedCoinRule(popRule.coinNum);
                default: return new RandCoinRule();
            }
        }
    }

    public abstract class BaseCostTotalCashRule : CashRuleManager
    {
        protected string currency;
        List<CostTotalParam> param = new List<CostTotalParam>();
        public BaseCostTotalCashRule(string currency, CashRule cashRule)
        {
            this.currency = currency;
            foreach (var one in cashRule.rules)
            {
                try
                {
                    string[] temp = one.Split(',');
                    CostTotalParam costTotalParam = new CostTotalParam();
                    costTotalParam.minAmount = RdmBase.ParseDouble(temp[0]);
                    costTotalParam.randMin = RdmBase.ParseDouble(temp[1]);
                    costTotalParam.randMax = RdmBase.ParseDouble(temp[2]);
                    param.Add(costTotalParam);
                }
                catch (Exception e)
                {
                    Debug.Log("parse the cash rule with exception:" + e+" "+one);
                    //ignore
                }
            }
        }
        protected abstract CashRuleManagerType GetCashRuleManagerType();
        protected abstract double GetJudgeValue();
        public ValueLimit GetNextValueLimit()
        {
            double nowValue = GetJudgeValue();
            var costTotalParam = PickParam(nowValue);
            RdmBase.Log("pick "+ GetCashRuleManagerType().ToString() + " params amount:" + costTotalParam.minAmount + "[" + costTotalParam.randMin + "," + costTotalParam.randMax + "]");

            var vl = new ValueLimit
            {
                cashRuleManagerType = GetCashRuleManagerType(),
                minLimit = costTotalParam.randMin,
                maxLimit = costTotalParam.randMax
            };
            //var r =  UnityEngine.Random.Range(costTotalParam.randMin, costTotalParam.randMax);
            return vl;
        }
        private CostTotalParam PickParam(double nowValue)
        {
            for (int i = 0; i < param.Count; i++)
            {
                double nextParamsValue = double.MaxValue;
                if (i + 1 < param.Count)
                {
                    nextParamsValue = param[i + 1].minAmount;
                }
                if (nowValue < nextParamsValue)
                {
                    return param[i];
                }
            }
            return param[param.Count - 1];
        }
        override public string ToString()
        {
            var s = "";
            foreach (var x in this.param)
            {
                s += x.minAmount + "," + x.randMin + "," + x.randMax + " ";
            }
            return GetCashRuleManagerType().ToString()+"[" + s + "]";
        }

    }
    public class CostTotalCashRule : BaseCostTotalCashRule
    {
        public CostTotalCashRule(string currency, CashRule cashRule):base(currency, cashRule)
        {
            
        }

        protected override CashRuleManagerType GetCashRuleManagerType()
        {
            return CashRuleManagerType.CostTotalCashRule;
        }

        override protected double GetJudgeValue()
        {
            double v =  RdmSdk.GetInstance().achievementManager.GetCash(currency);
            RdmBase.Log("getting judge value total:" + v);
            return v;
        }
    }
    public class RandCostTotalCashRule : BaseCostTotalCashRule
    {
        public RandCostTotalCashRule(string currency, CashRule cashRule) : base(currency, cashRule)
        {

        }
        protected override CashRuleManagerType GetCashRuleManagerType()
        {
            return CashRuleManagerType.RandCostTotalCashRule;
        }
        override protected double GetJudgeValue()
        {
            double v =  RdmSdk.GetInstance().achievementManager.GetRandCash(currency);
            RdmBase.Log("getting judge value only rand:" + v);
            return v;
        }
    }
    public interface PopManager
    {
        CanShowResult CanShowAndCount(int value);
    }
    public enum CoinRuleType
    {
        Rand,Fixed,Unknow
    }
    public interface CoinRule
    {
        double GetValue(ValueLimit vl);
        CoinRuleType GetCoinRuleType();
    }
    public class RandCoinRule:CoinRule
    {
        public double GetValue(ValueLimit vl)
        {
            var random = new System.Random();
            return random.NextDouble() * (vl.maxLimit - vl.minLimit) + vl.minLimit;
        }
        public CoinRuleType GetCoinRuleType()
        {
            return CoinRuleType.Rand;
        }
    }
    public class FixedCoinRule:CoinRule
    {
        readonly double fixedValue = 0;
        public FixedCoinRule(string coinNum)
        {
            // fixedValue = double.Parse(coinNum);
            fixedValue = RdmBase.ParseDouble(coinNum);
        }
        public double GetValue(ValueLimit vl)
        {
            if (vl.cashRuleManagerType == CashRuleManagerType.CostTotalCashRule)
            {
                if (fixedValue < vl.maxLimit)
                {
                    return fixedValue;
                }
                else return -1;
            }
            else if (vl.cashRuleManagerType == CashRuleManagerType.RandCostTotalCashRule)
            {
                //RandCostTotalCashRule 下的固定产出 不受控制
                return fixedValue;
            }
            else return -1;
        }
        public CoinRuleType GetCoinRuleType()
        {
            return CoinRuleType.Fixed;
        }
    }
    public interface EventPopManager
    {
        string GetEventName();
        CanShowResult CanShowAndCount(string currency,bool isFloat, ValueLimit valueLimit);
    }
    public abstract class BaseEventPopManager:EventPopManager
    {
        PopRule popRule;
        CoinRule coinRule;
        public BaseEventPopManager(PopRule rule)
        {
            popRule = rule;
            coinRule = ConfigParser.ParseCoinRule(rule);
        }
        public string GetEventName()
        {
            return popRule.eventName;
        }
        
        public abstract string Judge();
        public abstract void Count(CanShowResult result);
        public CanShowResult AddPickCondition(string currency,bool isFloat,CoinRuleType coinRuleType, double value)
        {
            var pc = popRule.pickCondition;
            if ( pc== PickCondition.doubleRule)
            {
                return CanShowResult.Success(currency, isFloat, pc, coinRuleType, value / 2);
            }
            else
            {
                return CanShowResult.Success(currency, isFloat, pc, coinRuleType, value);
            }
        }

        public CanShowResult CanShowAndCount(string currency, bool isFloat, ValueLimit valueLimit)
        {
            var reason = Judge();
            CanShowResult re;
            if (reason == null)
            {
                CoinRuleType coinRuleType = coinRule.GetCoinRuleType();
                double value = coinRule.GetValue(valueLimit);
                if(value > 0)
                {
                    re = AddPickCondition(currency, isFloat, coinRuleType, value);
                }
                else
                {
                   re = CanShowResult.Fail("block by value " + value);
                }
            }
            else
            {
                re = CanShowResult.Fail(reason);
            }
            Count(re);
            return re;
        }
    }
    public class EventWithRate:BaseEventPopManager
    {
        int rate;
        public EventWithRate(PopRule popRule):base(popRule)
        {
            rate = int.Parse(popRule.param);
        }

        override public string Judge()
        {
            
            int random = UnityEngine.Random.Range(0, 100);
            var result = random < rate;
            if (result)
            {
                return null;
            }
            else
            {
                return "block by rate " + random + " " + rate;
            }
        }
        public override void Count(CanShowResult result)
        {
            //do nothing
        }
        public override string ToString()
        {
            return "EventWithRate[event:" + GetEventName() + " rate:" + this.rate+"]";
        }
    }
    public class EventWithInterval : BaseEventPopManager
    {
        int interval = 0;
        int coolDown = 0;
        public EventWithInterval(PopRule popRule):base(popRule)
        {
            coolDown = 0;
            interval = int.Parse(popRule.param);
        }
        override public string Judge()
        {
           if(coolDown == 0)
            {
                return null;
            }
            else
            {
                return "block by interval " + coolDown;
            }
        }
        public override void Count(CanShowResult result)
        {
            //do nothing
            if (result.canShow)
            {
                coolDown = interval;
            }
            else
            {
                coolDown--;
            }
        }
        public override string ToString()
        {
            return "EventWithInterval[event:" + GetEventName() + " interval:" + this.interval + "]";
        }
    }
    public class EventWithCoolDown : BaseEventPopManager
    {
        readonly int interval = 0;
        int lastTs = 0;
        public EventWithCoolDown(PopRule popRule) : base(popRule)
        {
            lastTs = 0;
            interval = int.Parse(popRule.param);
        }
        override public string Judge()
        {
            int now = RdmBase.ToTimestamp(DateTime.UtcNow);
            if( lastTs + interval < now)
            {
                return null;
            }
            else
            {
                return "block by time " + lastTs + " " + interval + " " + now;
            }
        }
        public override void Count(CanShowResult result)
        {
            int now = RdmBase.ToTimestamp(DateTime.UtcNow);
            if (result.canShow)
            {
                lastTs = now;
            }
        }
        public override string ToString()
        {
            return "EventWithCoolDown[event:" + GetEventName() + " interval:" + this.interval + "]";
        }
    }
    public class CanShowResult
    {
        public string currency;
        public bool isFloat;
        public bool canShow;
        public string reason;
        public PickCondition pickCondition;
        public CoinRuleType coinRuleType;
        public double value;

        private CanShowResult(string reason)
        {   
            canShow = false;
            this.reason = reason;
            pickCondition = PickCondition.direct;
            coinRuleType = CoinRuleType.Unknow;
            value = 0;
        }
        private CanShowResult(string currency, bool isFloat, PickCondition pc, CoinRuleType coinRuleType, double value)
        {
            this.currency = currency;
            this.isFloat = isFloat;
            canShow = true;
            reason = "";
            pickCondition = pc;
            this.coinRuleType = coinRuleType;
            if (!isFloat)
            {
                this.value = (double)Math.Floor((double)value);
            }
            else
            {
                this.value = value;
            }
        }
        static public CanShowResult Success(string currency,bool isFloat, PickCondition pc, CoinRuleType coinRuleType, double value)
        {
            return new CanShowResult(currency, isFloat, pc, coinRuleType, value);
        }
        static public CanShowResult Fail(string reason)
        {
            return new CanShowResult(reason);
        }
    }
    public class CurrencyPopManager
    {
        private CurrencyConfig currencyConfig;
        private Dictionary<string, EventPopManager> eventPopManagers;
        private CashRuleManager cashRuleManager;
        public CurrencyPopManager(CurrencyConfig currencyConfig)
        {
            this.currencyConfig = currencyConfig;
            this.cashRuleManager = ConfigParser.ParseCashRule(currencyConfig.currency, currencyConfig.cashRule);
            this.eventPopManagers = new Dictionary<string, EventPopManager>();
            foreach (var one in currencyConfig.popRules)
            {
                var eventName = one.eventName;
                var manager = ConfigParser.ParsePopRule(one);
                if(manager != null)
                {
                    eventPopManagers[manager.GetEventName()] = manager;
                }
            }
        }

        public CanShowResult CanShow(string eventName)
        {
            var valueLimit = this.cashRuleManager.GetNextValueLimit();
            return EventCanShow(eventName, valueLimit);
        }
        private CanShowResult EventCanShow(string eventName, ValueLimit valueLimit)
        {
            if (eventPopManagers.ContainsKey(eventName))
            {
                return eventPopManagers[eventName].CanShowAndCount(this.currencyConfig.currency, this.currencyConfig.isFloat,
                    valueLimit);
            }
            else return CanShowResult.Fail("not found this event pop manager");
        }
    }


    public class RdmPopManager
    {
        private Dictionary<string, CurrencyPopManager> popManagers;
        
        public RdmPopManager(RdmConfig rdmConfig)
        {
            popManagers = new Dictionary<string, CurrencyPopManager>();
            foreach(var one in rdmConfig.currencyMap)
            {
                RdmBase.Log("add CurrencyPopManager " + one.Key);
                popManagers[one.Key] = new CurrencyPopManager(one.Value);
            }
        }

        public CanShowResult CanShow(string currency, string eventName)
        {
            if (popManagers.ContainsKey(currency))
            {
                return this.popManagers[currency].CanShow(eventName);
            }
            else return CanShowResult.Fail("not find this currency "+currency);
        }
    }
}