#import <Foundation/Foundation.h>
#import "SimpleNativeManager.h"
#import "SimpleNativeBridgeManager.h"


static SimpleNativeManager *_instance = nil;

@implementation SimpleNativeManager

+(instancetype)sharedInstance{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _instance = [self alloc];
    });
    return _instance;
}

-(void)bridge:(NSString *) method params:(NSString *) params{
    [SimpleNativeBridgeManager callback:method params:params];
}

+ (void)init:(NSString *) str;{
    SimpleNativeManager * one = [SimpleNativeManager sharedInstance];
    [[[SimpleNativeIOSSDK sharedInstance] getAttrInstance] setSimpleAttrCallback:(id)one];
    [[[SimpleNativeIOSSDK sharedInstance] getAdInstance] setSimpleSDKRewardedVideoListener:(id)one];
    [[[SimpleNativeIOSSDK sharedInstance] getAdInstance] setSimpleSDKInterstitialAdListener:(id)one];
    [[[SimpleNativeIOSSDK sharedInstance] getAdInstance] setSimpleSDKBannerAdListener:(id)one];
    
    [[[SimpleNativeIOSSDK sharedInstance] geUPInstance] setPurchaseItemsListener:(id)one];
    
    //add notification from OpenUrl
//    if([[SimpleNativeIOSSDK sharedInstance] geUPInstance]){
//        [[NSNotificationCenter defaultCenter] addObserver:one selector:@selector(openUrlFromUnity:) name:@"kUnityOnOpenURL" object:nil];
//    }
    [[SimpleNativeIOSSDK sharedInstance] sdkInitWithString:str callback:^(bool isSuccess,NSString* msg){
        NSLog(@"SimpleNativeManager sdk callback %i %@", isSuccess, msg);
    }];
}
//
//-(void) openUrlFromUnity:(NSNotification *)notification{
//    NSLog(@"Simple UnityBridge openUrlFromUnity");
//
//    NSDictionary * notifData =  notification.userInfo;
//    id url = [notifData objectForKey:@"url"];
//    id sourceApplication = [notifData objectForKey:@"sourceApplication"];
//    id annotation = [notifData objectForKey:@"annotation"];
//
//    [[SimpleNativeIOSSDK sharedInstance] application:[UIApplication sharedApplication] openURL:url sourceApplication:sourceApplication annotation:annotation];
//}

+(NSString *) getStaticInfo{
    NSDictionary * dict = [SimpleJsonMapper objectToDict:[[SimpleNativeIOSSDK sharedInstance] getStaticInfo]];
    return [SimpleSDKUtils dictToJson:dict];
}

+(void)log:(NSString *) paramMapJson{
    NSDictionary * dict = [SimpleSDKUtils dictFromJsonStr:paramMapJson];
    NSString * eventName = (NSString *)[dict objectForKey:@"eventName"];
    NSDictionary * params = (NSDictionary *)[dict objectForKey:@"params"];
    
    [[SimpleNativeIOSSDK sharedInstance]log:eventName withParams:params];
}

+(bool)hasInterstitial{
    return [[[SimpleNativeIOSSDK sharedInstance]getAdInstance]hasInterstitial];
}

+(void)showInterstitial:(NSString *) adEntry{
    [[[SimpleNativeIOSSDK sharedInstance]getAdInstance]showInterstitial:adEntry];
}

+(bool)hasReward{
    return [[[SimpleNativeIOSSDK sharedInstance]getAdInstance]hasReward];
}

+(void)showReward:(NSString *) adEntry{
    [[[SimpleNativeIOSSDK sharedInstance]getAdInstance]showReward:adEntry];
}

+(void)showOrReShowBanner:(int)pos{
    [[[SimpleNativeIOSSDK sharedInstance]getAdInstance]showOrReShowBanner:pos];
}

+(void)hideBanner{
    [[[SimpleNativeIOSSDK sharedInstance]getAdInstance]hideBanner];
}

+(void)removeBanner{
    [[[SimpleNativeIOSSDK sharedInstance]getAdInstance]removeBanner];
}

+(NSString *)getLoadingStatusSummary{
    NSDictionary * dict = [[[SimpleNativeIOSSDK sharedInstance]getAdInstance]getLoadingStatusSummary];
    return [SimpleSDKUtils dictToJson:dict];
}
+(bool) isLogin{
    return [[[SimpleNativeIOSSDK sharedInstance] geUPInstance] isLogin];
}
+(NSString *) getGameAccountId{
    long long v =  [[[SimpleNativeIOSSDK sharedInstance] geUPInstance]getGameAccountId];
    return [NSString stringWithFormat:@"%lld", v];
}
+(void) logout{
    [[[SimpleNativeIOSSDK sharedInstance] geUPInstance] logout];
}
+(void) autoLoginAsync:(bool)platformLogin{
    [[[SimpleNativeIOSSDK sharedInstance] geUPInstance] autoLoginAsync:platformLogin success:^(AutoLoginResult * _Nonnull result) {
            [[SimpleNativeManager sharedInstance] bridge:@"autoLoginSuccess" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:result]]];
        } fail:^(State * _Nonnull state) {
            [[SimpleNativeManager sharedInstance] bridge:@"autoLoginFail" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:state]]];
        }];
}
+(void) checkLoginAsync{
    [[[SimpleNativeIOSSDK sharedInstance] geUPInstance] checkLoginAsync:^(CheckLoginResult * _Nonnull result) {
            [[SimpleNativeManager sharedInstance] bridge:@"checkLoginSuccess" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:result]]];
        } fail:^(State * _Nonnull state) {
            [[SimpleNativeManager sharedInstance] bridge:@"checkLoginFail" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:state]]];
        }];
}
+(void) loginWithTypeAsync:(NSString *)loginType{
    [[[SimpleNativeIOSSDK sharedInstance]geUPInstance]loginWithTypeAsync:loginType success:^(LoginResult * _Nonnull result) {
        [[SimpleNativeManager sharedInstance] bridge:@"loginWithTypeAsyncSuccess" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:result]]];
    } fail:^(State * _Nonnull state) {
        [[SimpleNativeManager sharedInstance] bridge:@"loginWithTypeAsyncFail" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:state]]];
    }];
}
+(void) bindWithTypeAsync:(NSString *)loginType{
    [[[SimpleNativeIOSSDK sharedInstance]geUPInstance] bindWithTypeAsync:loginType success:^(UserInfoResult * _Nonnull result) {
            [[SimpleNativeManager sharedInstance] bridge:@"bindWithTypeAsyncSuccess" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:result]]];
        } fail:^(State * _Nonnull state) {
            [[SimpleNativeManager sharedInstance] bridge:@"bindWithTypeAsyncFail" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:state]]];
        }];
}
+(void) unbindWithTypeAsync:(NSString *) loginType{
    [[[SimpleNativeIOSSDK sharedInstance]geUPInstance] unbindWithTypeAsync:loginType success:^(UserInfoResult * _Nonnull result) {
        [[SimpleNativeManager sharedInstance] bridge:@"unbindWithTypeAsyncSuccess" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:result]]];
    } fail:^(State * _Nonnull state) {
        [[SimpleNativeManager sharedInstance] bridge:@"unbindWithTypeAsyncFail" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:state]]];
    }];
}
+(void) getUserInfoAsync{
    [[[SimpleNativeIOSSDK sharedInstance]geUPInstance] getUserInfoAsync:^(UserInfoResult * _Nonnull result) {
        [[SimpleNativeManager sharedInstance] bridge:@"getUserInfoAsyncSuccess" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:result]]];
    } fail:^(State * _Nonnull state) {
        [[SimpleNativeManager sharedInstance] bridge:@"getUserInfoAsyncFail" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:state]]];
    }];
}
+(NSString *) getShopItems{
    ShopItemResult * result = [[[SimpleNativeIOSSDK sharedInstance]geUPInstance]getShopItems ];
    if(result){
        return [SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:result]];
    }
    else return nil;
}
+(void) startPayment:(NSString *)itemId  cpOrderId:(NSString *)cpOrderId{
    [[[SimpleNativeIOSSDK sharedInstance]geUPInstance] startPayment:itemId cpOrderId:cpOrderId success:^(StartPaymentResult * _Nonnull result) {
        [[SimpleNativeManager sharedInstance] bridge:@"startPaymentSuccess" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:result]]];
    } fail:^(State * _Nonnull state) {
        [[SimpleNativeManager sharedInstance] bridge:@"startPaymentFail" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:state]]];
    }];
}

+(void) startPaymentForSimpleGame:(NSString *)itemId{
    [[[SimpleNativeIOSSDK sharedInstance]geUPInstance] startPaymentForSimpleGame:itemId success:^(StartPaymentResult * _Nonnull result) {
        [[SimpleNativeManager sharedInstance] bridge:@"startPaymentForSimpleGameSuccess" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:result]]];
    } fail:^(State * _Nonnull state) {
        [[SimpleNativeManager sharedInstance] bridge:@"startPaymentForSimpleGameFail" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:state]]];
    }];
}

+(void) consumeItem:(NSString *) gameOrderId{
    [[[SimpleNativeIOSSDK sharedInstance]geUPInstance] consumeItem:[gameOrderId longLongValue]];
}

#pragma protocol
- (void)getAttr:(SimpleAttrInfo*)info{
    NSString * jsonStr = [SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:info]];
    [[SimpleNativeManager sharedInstance] bridge:@"setAttributionInfo" params:jsonStr];
}

#pragma reward

- (void) onRewardedVideoAdPlayStart:(NSString *) unitId callback:(SimpleAdCallbackInfo *) callbackInfo{
    NSDictionary * params = @{@"unitId":unitId, @"callbackInfo":[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:callbackInfo]]};
    [[SimpleNativeManager sharedInstance] bridge:@"onRewardedVideoAdPlayStart" params:[SimpleSDKUtils dictToJson:params]];
}
- (void) onRewardedVideoAdPlayFail:(NSString *) unitId code:(NSString *) code  message:(NSString *) message{
    NSDictionary * params = @{@"unitId":unitId, @"code":code, @"message":message};
    [[SimpleNativeManager sharedInstance] bridge:@"onRewardedVideoAdPlayFail" params:[SimpleSDKUtils dictToJson:params]];
}
- (void) onRewardedVideoAdPlayClosed:(NSString *) unitId callback:(SimpleAdCallbackInfo *) callbackInfo{
    NSDictionary * params = @{@"unitId":unitId, @"callbackInfo":[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:callbackInfo]]};
    [[SimpleNativeManager sharedInstance] bridge:@"onRewardedVideoAdPlayClosed" params:[SimpleSDKUtils dictToJson:params]];
}
- (void) onRewardedVideoAdPlayClicked:(NSString *) unitId callback:(SimpleAdCallbackInfo *) callbackInfo{
    NSDictionary * params = @{@"unitId":unitId, @"callbackInfo":[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:callbackInfo]]};
    [[SimpleNativeManager sharedInstance] bridge:@"onRewardedVideoAdPlayClicked" params:[SimpleSDKUtils dictToJson:params]];
}
- (void) onReward:(NSString *) unitId callback:(SimpleAdCallbackInfo *) callbackInfo{
    NSDictionary * params = @{@"unitId":unitId, @"callbackInfo":[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:callbackInfo]]};
    [[SimpleNativeManager sharedInstance] bridge:@"onReward" params:[SimpleSDKUtils dictToJson:params]];
}

#pragma inter
- (void) onInterstitialAdShow:(NSString *) unitId callback:(SimpleAdCallbackInfo *) callbackInfo{
    NSDictionary * params = @{@"unitId":unitId, @"callbackInfo":[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:callbackInfo]]};
    [[SimpleNativeManager sharedInstance] bridge:@"onInterstitialAdShow" params:[SimpleSDKUtils dictToJson:params]];
}
- (void) onInterstitialAdClose:(NSString *) unitId callback:(SimpleAdCallbackInfo *) callbackInfo{
    NSDictionary * params = @{@"unitId":unitId, @"callbackInfo":[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:callbackInfo]]};
    [[SimpleNativeManager sharedInstance] bridge:@"onInterstitialAdClose" params:[SimpleSDKUtils dictToJson:params]];
}
- (void) onInterstitialAdClick:(NSString *) unitId callback:(SimpleAdCallbackInfo *) callbackInfo{
    NSDictionary * params = @{@"unitId":unitId, @"callbackInfo":[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:callbackInfo]]};
    [[SimpleNativeManager sharedInstance] bridge:@"onInterstitialAdClick" params:[SimpleSDKUtils dictToJson:params]];
}

#pragma bannder
- (void) onAdImpress:(NSString *) unitId callback:(SimpleAdCallbackInfo *) callbackInfo{
    NSDictionary * params = @{@"unitId":unitId, @"callbackInfo":[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:callbackInfo]]};
    [[SimpleNativeManager sharedInstance] bridge:@"onAdImpress" params:[SimpleSDKUtils dictToJson:params]];
}
- (void) onAdClick:(NSString *) unitId callback:(SimpleAdCallbackInfo *) callbackInfo{
    NSDictionary * params = @{@"unitId":unitId, @"callbackInfo":[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:callbackInfo]]};
    [[SimpleNativeManager sharedInstance] bridge:@"onAdClick" params:[SimpleSDKUtils dictToJson:params]];
}

#pragma mark IPurchaseItemsListener
- (void) getPurchaseItems:(PurchaseItems *) purchaseItems{
    [[SimpleNativeManager sharedInstance] bridge:@"getPurchaseItems" params:[SimpleSDKUtils dictToJson:[SimpleJsonMapper objectToDict:purchaseItems]]];
}
@end
