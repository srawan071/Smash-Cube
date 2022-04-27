using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleSDKNS
{
    public class SimpleSDKUserPayment
    {
        static public SimpleSDKUserPayment instance = new SimpleSDKUserPayment(SimpleSDKBridgeFactory.instance);

        private SimpleSDKBridge bridge;
        public Action<AutoLoginResult> autoLoginAsyncSuccess = null;
        public Action<State> autoLoginAsyncFail = null;

        public Action<CheckLoginResult> checkLoginAsyncSuccess = null;
        public Action<State> checkLoginAsyncFail = null;

        public Action<LoginResult> loginWithTypeAsyncSuccess = null;
        public Action<State> loginWithTypeAsyncFail = null;

        public Action<UserInfoResult> bindWithTypeAsyncSuccess = null;
        public Action<State> bindWithTypeAsyncFail = null;

        public Action<UserInfoResult> unbindWithTypeAsyncSuccess = null;
        public Action<State> unbindWithTypeAsyncFail = null;

        public Action<UserInfoResult> getUserInfoAsyncSuccess = null;
        public Action<State> getUserInfoAsyncFail = null;

        public Action<ShopItemResult> getShopItemsAsyncSuccess = null;
        public Action<State> getShopItemsAsyncFail = null;

        public Action<StartPaymentResult> startPaymentSuccess = null;
        public Action<State> startPaymentFail = null;

        public Action<StartPaymentResult> startPaymentForSimpleGameSuccess = null;
        public Action<State> startPaymentForSimpleGameFail = null;

        public IPurchaseItemsListener listener = null;

        public SimpleSDKUserPayment(SimpleSDKBridge b) {
            this.bridge = b;
        }
        public bool isLogin()
        {
            return bridge.isLogin();
        }

        public long getGameAccountId()
        {
            return bridge.getGameAccountId();
        }

        public void Logout()
        {
            bridge.Logout();
        }

        public void autoLoginAsync(bool needGooglePlay, Action<AutoLoginResult> success, Action<State> fail)
        {
            autoLoginAsyncSuccess = success;
            autoLoginAsyncFail = fail;
            bridge.autoLoginAsync(needGooglePlay);
        }

        public void checkLoginAsync(Action<CheckLoginResult> success, Action<State> fail)
        {
            checkLoginAsyncSuccess = success;
            checkLoginAsyncFail = fail;
            bridge.checkLoginAsync();
        }

        public void loginWithTypeAsync(LOGIN_TYPE loginType, Action<LoginResult> success, Action<State> fail)
        {
            loginWithTypeAsyncSuccess = success;
            loginWithTypeAsyncFail = fail;
            bridge.loginWithTypeAsync(loginType);
        }

        public void bindWithTypeAsync(LOGIN_TYPE loginType, Action<UserInfoResult> success, Action<State> fail)
        {
            bindWithTypeAsyncSuccess = success;
            bindWithTypeAsyncFail = fail;
            bridge.bindWithTypeAsync(loginType);
        }

        public void unbindWithTypeAsync(LOGIN_TYPE loginType, Action<UserInfoResult> success, Action<State> fail)
        {
            unbindWithTypeAsyncSuccess = success;
            unbindWithTypeAsyncFail = fail;
            bridge.unbindWithTypeAsync(loginType);
        }

        public void getUserInfoAsync(Action<UserInfoResult> success, Action<State>fail)
        {
            getUserInfoAsyncSuccess = success;
            getUserInfoAsyncFail = fail;
            bridge.getUserInfoAsync();
        }

        public ShopItemResult getShopItems()
        {
            return bridge.getShopItems();
        }

        public void getShopItemsAsync(Action<ShopItemResult> success, Action<State> fail)
        {
            getShopItemsAsyncSuccess = success;
            getShopItemsAsyncFail = fail;
            bridge.getShopItemsAsync();
        }

        //
        public void startPayment(string itemId, string cpOrderId,
                          Action<StartPaymentResult> success, Action<State>fail)
        {
            startPaymentSuccess = success;
            startPaymentFail = fail;
            bridge.startPayment(itemId, cpOrderId);
        }
        public void startPaymentForSimpleGame(string itemId, Action<StartPaymentResult> success, Action<State> fail)
        {
            startPaymentForSimpleGameSuccess = success;
            startPaymentForSimpleGameFail = fail;
            bridge.startPaymentForSimpleGame(itemId);
        }

        public void setIPurchaseItemsListener(IPurchaseItemsListener listener)
        {
            this.listener = listener;
        }

        public void consumeItem(long gameOrderId)
        {
            bridge.consumeItem(gameOrderId);
        }
    }
}