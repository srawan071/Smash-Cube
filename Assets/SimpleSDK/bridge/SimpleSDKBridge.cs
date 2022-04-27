using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSDKNS
{
    public interface SimpleSDKBridge
    {
        void initWithConfig(string fileContent);
        void onResume();
        void onPause();
        void Log(string eventName, Dictionary<string, string> paramMap);
        StaticInfo GetStaticInfo();

        bool HasInterstitial();

        void ShowInterstitial(string adEntry);

        bool HasReward();

        void ShowReward(string adEntry);
        void ShowOrReShowBanner(BannerPos pos);
        void HideBanner();

        void RemoveBanner();

        Dictionary<string, string> GetLoadingStatusSummary();

        //USERPAYMENT
        bool isLogin();
        
        long getGameAccountId();

        void Logout();

        void autoLoginAsync(bool platformLogin);

        void checkLoginAsync();

        void loginWithTypeAsync(LOGIN_TYPE loginType);

        void bindWithTypeAsync(LOGIN_TYPE loginType);

        void unbindWithTypeAsync(LOGIN_TYPE loginType);

        void getUserInfoAsync();

        ShopItemResult getShopItems();

        void getShopItemsAsync();

        //
        void startPayment(string itemId, string cpOrderId);

        void startPaymentForSimpleGame(string itemId);

        void consumeItem(long gameOrderId);

        //void printDatabase();
    }
}
