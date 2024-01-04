using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class Admanager : MonoBehaviour
{

    public static Admanager Instance;
    private string YOUR_APP_KEY = "1c539ec7d";

    private IRewardable _rewardable;
    [SerializeField]
    private bool showAd=false;

   
    void Awake()
    {

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
        //  IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
    }
    private void Start()
    {
        IronSource.Agent.validateIntegration();
        IronSource.Agent.init(YOUR_APP_KEY, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);
        Invoke("RequestBanner",3f);
      //  RequestBanner();
        StartCoroutine(EnableAd());
      
    }

    void RequestBanner()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        IronSource.Agent.displayBanner();
    }
    private void SdkInitializationCompletedEvent()
    {
      //  IronSource.Agent.validateIntegration();

        

        IronSource.Agent.loadInterstitial();
        IronSource.Agent.loadRewardedVideo();

    }
    private void BannerOnAdLoadedEvent(IronSourceAdInfo info)
    {
      
    }

    private void InterstitialOnAdClosedEvent(IronSourceAdInfo info)
    {
        IronSource.Agent.loadInterstitial();
    }



    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);

        Debug.Log("Pause");
    }


    public void ShowFullScreenAd()
    {
        if (showAd)
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

    IEnumerator EnableAd()
    {
        yield return new WaitForSeconds(60);
        showAd = true;

    }
    private void OnDisable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent -= SdkInitializationCompletedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent -= InterstitialOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;

       // IronSourceBannerEvents.onAdLoadedEvent -= BannerOnAdLoadedEvent;
    }

  
}