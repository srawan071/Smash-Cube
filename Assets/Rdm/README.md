
# 安装

双击unity package导入资源

# 配置

将`RdmSDK`挂载在启动场景的常驻节点，跟`SimpleSDK`一样。

* 这个版本暂时不提供UI，请自行实现UI


# 使用

`RdmSDK`在`Awake`阶段会做一定的初始化工作。请在`Start`阶段再启动`RdmSDK`

所有建议调用的API都统一封装在`RdmApi`

## 初始化

启动返现逻辑需要传入必要的参数, 其中channel参数值最重要的参数。可以在SDK的买量函数回调的方法中，再初始化变现SDK。

```
SimpleSDK.instance.SetAttributionInfoListener(this.GetAttribution);

public void GetAttribution(AttributionInfo info)
{
   var initInfo = new InitInfo(SimpleSDK.instance.GetStaticInfo(), attributionInfo.network );
   RdmApi.Init(initInfo, RdmInitSuccess, RdmInitFail);
}
```

## 判断返现状态
```
RdmApi.GetRdmStatus()
NOT_INIT为还没初始化 INITING 为正在初始化 FX_OPEN 为返现开启 FX_CLOSE为被风控了
或者在初始化的时候，通过回调OpenRdmSuccess来知道开启返现成功

```

## UI实现指导


* 弹窗逻辑：
  * 通过调用`RdmApi.ShowCollectWithEvent`获得返回值`CanShowResult`
    * 根据`CanShowResult`的值实现如下内容：
      * 如果`CanShowResult.canShow`为`true`,可以弹窗。否则直接忽略跳过
      * 根据`CanShowResult.pickCondition`的值，控制弹窗形式
        * direct:玩家直接点击领取即可
        * ad:玩家观看广告可以领取，不观看再不能获得
        * double:玩家观看广告可以双倍领取
        * nopop:获取的窗口都不弹出，直接累加金额
      * 如果玩家成功领取调用，则`RdmApi.CollectCash()`，进行金额累加
        * 注意如果是双倍领取，请修改参数的value值为两倍
* 成就逻辑
  * 等级更新获取: `RdmApi.SetLevel()`和`RdmApi.GetLevel()`
  * 广告播放统计:在激励视频播放结束的时候调用`RdmApi.IncShowAdNumber()`,需要的时候调用`RdmApi.GetShowAdNumber()`
  * 金额获取: `RdmApi.GetCash()`
  * 活跃天数获取: `RdmApi.GetDayActive()`
* 兑换品相获取
  * 通过`RdmApi.GetAllItems()`获取这个币种下全部可兑换的品相
  * 每个品相`RdmItem`都有目标金额`value`，以及兑换条件列表`RdmItemCondition`
    * `RdmItemCondition`的name可以为以下值
      * "active_day" 对应 `RdmApi.GetDayActive()`
      * "cp_level" 对应 `RdmApi.GetLevel()`
      * "show_ad_number" 对应 `RdmApi.GetShowAdNumber()`
    * `RdmItemCondition`的op一般为`moreThan`
    * `RdmItemCondition`的value为条件对应需要达到的值
  * 某个品相达到了金额和全部条件后，可以申请兑换，可以调用`RdmItem.CanBuy()`进行判断
    * 调用`RdmApi.SendReview()`可以条件兑换申请，需要填入Email和PaypalAccount。
    * 申请成功条件后，会扣减金额和返回申请ID

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


# 修改日志


* rdmv2-v0.1.7: 增加了appVersion风控