#import <Foundation/Foundation.h>
#import<SimpleNativeIOSSDK/SimpleNativeIOSSDK.h>

@interface SimpleNativeManager<SimpleAttrCallback, SimpleSDKRewardedVideoListener, SimpleSDKInterstitialAdListener, SimpleSDKBannerAdListener, IPurchaseItemsListener>:NSObject

+ (instancetype)sharedInstance;

+ (void)init:(NSString *) str;

+ (NSString *) getStaticInfo;

+ (void)log:(NSString *) paramMapJson;

+ (bool)hasInterstitial;

+ (void)showInterstitial:(NSString *) adEntry;

+ (bool)hasReward;

+ (void)showReward:(NSString *) adEntry;

+ (void)showOrReShowBanner:(int)pos;

+ (void)hideBanner;

+ (void)removeBanner;

+ (NSString *)getLoadingStatusSummary;

+ (bool) isLogin;
+ (NSString *) getGameAccountId;
+ (void) logout;
+ (void) autoLoginAsync:(bool)platformLogin;
+ (void) checkLoginAsync;
+ (void) loginWithTypeAsync:(NSString *)loginType;
+ (void) bindWithTypeAsync:(NSString *)loginType;
+ (void) unbindWithTypeAsync:(NSString *) loginType;
+ (void) getUserInfoAsync;
+ (NSString *) getShopItems;
+ (void) startPayment:(NSString *)itemId  cpOrderId:(NSString *)cpOrderId;
+ (void) startPaymentForSimpleGame:(NSString *)itemId;
+ (void) consumeItem:(NSString *) gameOrderId;

@end
