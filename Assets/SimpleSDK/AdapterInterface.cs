using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSDKNS
{
    public interface AdapterInterface
    {
        void SetSimpleSDKRewardedVideoListener(SimpleSDKRewardedVideoListener simpleSDKRewardedVideoListener);

        void SetSimpleSDKInterstitialAdListener(SimpleSDKInterstitialAdListener simpleSDKInterstitialAdListener);

        void SetSimpleSDKBannerAdListener(SimpleSDKBannerAdListener simpleSDKBannerAdListener);

        bool HasReward();

        void ShowReward(string adEntry);

        bool HasInterstitial();

        void ShowInterstitial(string adEntry);

        void ShowOrReShowBanner(BannerPos pos);

        void HideBanner();

        void RemoveBanner();

        Dictionary<string, string> GetLoadingStatusSummary();
    }
}