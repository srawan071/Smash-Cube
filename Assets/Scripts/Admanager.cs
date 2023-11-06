using System;
using UnityEngine;

public class Admanager : MonoBehaviour
{

    public static Admanager Instance;
    public string YOUR_APP_KEY;

   private IRewardable _rewardable;
   
   void Awake(){

        DontDestroyOnLoad(gameObject);
        if (Instance == null)
            Instance = this;


        else if (Instance != this)
            Destroy(gameObject);
       }
    private void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
     
       
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;

        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;

        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
    }
    private void Start()
    {

        IronSource.Agent.init(YOUR_APP_KEY, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);

    }
    private void SdkInitializationCompletedEvent()
    {
        IronSource.Agent.validateIntegration();

        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);

        IronSource.Agent.loadInterstitial();
        IronSource.Agent.loadRewardedVideo();

    }
    private void BannerOnAdLoadedEvent(IronSourceAdInfo info)
    {
        IronSource.Agent.displayBanner();
    }

    private void InterstitialOnAdClosedEvent(IronSourceAdInfo info)
    {
        IronSource.Agent.loadInterstitial();
    }

    

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }
   

    public void ShowFullScreenAd()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            IronSource.Agent.loadInterstitial();
        }
       
    }
    public void ShowRewardedAd(IRewardable rewardable)
    {

        _rewardable = rewardable;
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();

        }
        else
        {
            IronSource.Agent.loadRewardedVideo();
        }
    }
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        //TODO - here you can reward the user according to the reward name and amount
        IronSource.Agent.loadRewardedVideo();
        _rewardable.GetReward();
       
    }
    /*   public void HandleUserEarnedReward(object sender, Reward args)
       {
           string type = args.Type;
           double amount = args.Amount;
           MonoBehaviour.print(
               "HandleRewardedAdRewarded event received for "
                           + amount.ToString() + " " + type);

           switch (kwala)
           {
               case "CONTINUE":
                   GameManager.singleton.Continue();
                   break;
               case "2BOMB":
                   GameManager.singleton.AddBomb();
                   break;
               case  "2XBOMB":
                   GameManager.singleton.DoubleBomb();
                   break;
           }


       }*/

    private void OnDisable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent -= SdkInitializationCompletedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent -= InterstitialOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;

        IronSourceBannerEvents.onAdLoadedEvent -= BannerOnAdLoadedEvent;
    }
}