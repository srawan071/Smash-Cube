using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSDKNS
{
    public enum BannerPos
    {
        TOP,
        BOTTOM
    }
    public class SimpleSDKAd
    {
        static public BridgeAdapterInterface instance = new BridgeAdapterInterface(SimpleSDKBridgeFactory.instance);
    }
    public class BridgeAdapterInterface: AdapterInterface
    {
        SimpleSDKBridge bridge = null;
        SimpleSDKBannerAdListener simpleSDKBannerAdListener = null;
        SimpleSDKInterstitialAdListener simpleSDKInterstitialAdListener = null;
        SimpleSDKRewardedVideoListener simpleSDKRewardedVideoListener = null;
        public BridgeAdapterInterface(SimpleSDKBridge inputBridge)
        {
            this.bridge = inputBridge;
        }

        public bool HasInterstitial()
        {
            return bridge.HasInterstitial();
        }

        public void ShowInterstitial(string adEntry)
        {
            bridge.ShowInterstitial(adEntry);
        }

        public bool HasReward()
        {
            return bridge.HasReward();
        }

        public void ShowReward(string adEntry)
        {
            bridge.ShowReward(adEntry);
        }
        public void ShowOrReShowBanner(BannerPos pos)
        {
            bridge.ShowOrReShowBanner(pos);
        }
        public void HideBanner()
        {
            bridge.HideBanner();
        }

        public void RemoveBanner()
        {
            bridge.RemoveBanner();
        }

        public Dictionary<string, string> GetLoadingStatusSummary()
        {
            return bridge.GetLoadingStatusSummary();
        }

        public void SetSimpleSDKBannerAdListener(SimpleSDKBannerAdListener simpleSDKBannerAdListener)
        {
            this.simpleSDKBannerAdListener = simpleSDKBannerAdListener;
        }
        public SimpleSDKBannerAdListener GetSimpleSDKBannerAdListener()
        {
            return this.simpleSDKBannerAdListener;
        }

        public void SetSimpleSDKInterstitialAdListener(SimpleSDKInterstitialAdListener simpleSDKInterstitialAdListener)
        {
            this.simpleSDKInterstitialAdListener = simpleSDKInterstitialAdListener;
        }
        public SimpleSDKInterstitialAdListener GetSimpleSDKInterstitialAdListener()
        {
            return this.simpleSDKInterstitialAdListener;
        }

        public void SetSimpleSDKRewardedVideoListener(SimpleSDKRewardedVideoListener simpleSDKRewardedVideoListener)
        {
            this.simpleSDKRewardedVideoListener = simpleSDKRewardedVideoListener;
        }
        public SimpleSDKRewardedVideoListener GetSimpleSDKRewardedVideoListener()
        {
            return this.simpleSDKRewardedVideoListener;
        }


    }
}