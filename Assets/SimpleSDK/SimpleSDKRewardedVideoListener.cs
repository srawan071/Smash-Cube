using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSDKNS
{
    public interface SimpleSDKRewardedVideoListener
    {
        /***
        * 视频播放开始
        */
        void onRewardedVideoAdPlayStart(string unitId, CallbackInfo callbackInfo);
        /***
         * 视频播放失败
         * @param code 错误码
         * @param message 错误信息
         */
        void onRewardedVideoAdPlayFail(string unitId, string code, string message);
        /**
         * 视频页面关闭
         * @param isReward 视频是否播放完成
         */
        void onRewardedVideoAdPlayClosed(string unitId, CallbackInfo callbackInfo);

        /***
         * 视频点击
         */
        void onRewardedVideoAdPlayClicked(string unitId, CallbackInfo callbackInfo);

        /***
         * 发放奖励
         */
        void onReward(string unitId, CallbackInfo callbackInfo);
    }
}