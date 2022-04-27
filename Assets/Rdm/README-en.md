
# INSTALL

double click unity package to import

# Config

add `RdmSDK` to the first sense 's node, it could add to the node with `SimpleSDK`

* This version don't provide UI

# API

`RdmSDK` would init at the `Awake` phase, please start the SDK in `Start` phase.
All the api is in `RdmApi`

## Init SDK

The channel params need to init the SDK.Please do it in the `SetAttributionInfoListener` callback.

```
SimpleSDK.instance.SetAttributionInfoListener(this.GetAttribution);

public void GetAttribution(AttributionInfo info)
{
   var initInfo = new InitInfo(SimpleSDK.instance.GetStaticInfo(), attributionInfo.network );
   RdmApi.Init(initInfo, RdmInitSuccess, RdmInitFail);
}
```

## Judge The Redeem Status

There is two way to know whether to open the redeem
* use `RdmApi.GetRdmStatus()` to get the status
  * NOT_INIT:the SDK is not init
  * INITING:the SDK is initing
  * RDM_OPEN: the redeem is opening
  * RDM_CLOSE: the redeem is close
* or use the `RdmInitSuccess` Callback in the `Init` function to know the redeem is opening.
```

```

## How to implement the UI

* Pop windows Logic：
  * invoke `RdmApi.ShowCollectWithEvent` to get the `CanShowResult`
    * if `CanShowResult.canShow` is `true`, we need to pop the windows
      *  according `CanShowResult.pickCondition` to display different windows
        * direct: the player can direct get the cash
        * ad: the player has to watch the Ad to get the cash
        * double: the player can watch the Ad to get the double cash or get the origin cash without watching Ad
        * nopop: Don't show the collect panel and direct to collect the cash
      * if the player can get the cash ，use `RdmApi.CollectCash()` to collect
        * Attention: if the pickCondition is double and the player has watched the ad, please change the Double the `CanShowResult.value` before invoking the `CollectCash`
* Achieve Logic
  * Level: use `RdmApi.SetLevel()` to update the player's level and use `RdmApi.GetLevel()` to get the level
  * Show Ad Number :After the player watch a Reward Ad, invoke `RdmApi.IncShowAdNumber()` to increment the value.And use `RdmApi.GetShowAdNumber()` to know the amount
  * Cash with one Currency: use `RdmApi.GetCash()` to get this currency's amount
  * Active Day: use `RdmApi.GetDayActive()` to get the Active Day Number
* Currency Item
  * use `RdmApi.GetAllItems()` to know all the item can buy in this currency
  * every `RdmItem` has a target amount `value` and a list of the condition of buying it `RdmItemCondition`
    * `RdmItemCondition`'s field `name` can be one of the below value
      * "active_day": point to the achieve `RdmApi.GetDayActive()`
      * "cp_level" point to the achieve `RdmApi.GetLevel()`
      * "show_ad_number" point to the achieve `RdmApi.GetShowAdNumber()`
    * `RdmItemCondition`'s field `op` must be `moreThan`, we can ignore right now
    * `RdmItemCondition`'s field `value` is the value the achieve need to reach
  * if an item 's number and all condition is reach ,player can buy it and submit an application.We can use `RdmItem.CanBuy()` to help to judge it.
    * let player to enter the Email and the PaypalAccount ,use `RdmApi.SendReview()`to send an application
    * the backend would double check the condition, if the application is submitted succuss, an application Id would be return.

```Example Code
The Logic For ShowCollectWithEvent

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
```
