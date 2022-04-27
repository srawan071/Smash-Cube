
#import<SimpleNativeIOSSDK/SimpleNativeIOSSDK.h>
#import "SimpleNativeManager.h"


// Converts an NSString into a const char* ready to be sent to Unity
static const char* nsstringToChar(NSString* input){
    const char* string = [input UTF8String];
    return string ? strdup(string) : NULL;
}

static NSString* charToNSString(char* x){
    return ((x) != NULL ? [NSString stringWithUTF8String:x] : [NSString stringWithUTF8String:""]);
}

void bridgeInitWithConfig(char * fileContent){
    [SimpleNativeManager init:charToNSString(fileContent)];
}

const char* bridgeGetStaticInfo(){
    return nsstringToChar([SimpleNativeManager getStaticInfo]);
}

void bridgeLog(char * paramMapJson){
    [SimpleNativeManager log:charToNSString(paramMapJson)];
}

bool bridgeHasInterstitial() {
    return [SimpleNativeManager hasInterstitial];
}

void bridgeShowInterstitial(char *  adEntry) {
    [SimpleNativeManager showInterstitial:charToNSString(adEntry)];
}

bool bridgeHasReward() {
    return [SimpleNativeManager hasReward];
}

void bridgeShowReward(char * adEntry) {
    [SimpleNativeManager showReward:charToNSString(adEntry)];
}

void bridgeShowOrReShowBanner(int pos) {
    [SimpleNativeManager showOrReShowBanner:pos];
}

void bridgeHideBanner() {
    [SimpleNativeManager hideBanner];
}

void bridgeRemoveBanner() {
    [SimpleNativeManager removeBanner];
}
const char*  bridgeGetLoadingStatusSummary() {
    return nsstringToChar([SimpleNativeManager getLoadingStatusSummary]);
}

//userpayment
bool bridgeIsLogin(){
    return [SimpleNativeManager isLogin];
}

const char* bridgeGetGameAccountId(){
    NSString * re = [SimpleNativeManager  getGameAccountId];
    return nsstringToChar(re);
}
void bridgeLogout(){
    [SimpleNativeManager  logout];
}
void bridgeAutoLoginAsync(bool platformLogin){
    [SimpleNativeManager  autoLoginAsync:platformLogin];
}
void bridgeCheckLoginAsync(){
    [SimpleNativeManager  checkLoginAsync];
}
void bridgeLoginWithTypeAsync(char * loginType){
    [SimpleNativeManager  loginWithTypeAsync:charToNSString(loginType)];
}
void bridgeBindWithTypeAsync(char * loginType){
    [SimpleNativeManager  bindWithTypeAsync:charToNSString(loginType)];
}
void bridgeUnbindWithTypeAsync(char * loginType){
    [SimpleNativeManager  unbindWithTypeAsync:charToNSString(loginType)];
}
void bridgeGetUserInfoAsync(){
    [SimpleNativeManager  getUserInfoAsync];
}
const char * bridgeGetShopItems(){
    NSString * re = [SimpleNativeManager  getShopItems];
    return nsstringToChar(re);
}
void  bridgeStartPayment(char * itemId, char * cpOrderId){
    [SimpleNativeManager  startPayment:charToNSString(itemId) cpOrderId:charToNSString(cpOrderId)];
}
void  bridgeStartPaymentForSimpleGame(char * itemId){
    [SimpleNativeManager  startPaymentForSimpleGame:charToNSString(itemId)];
}
void bridgeConsumeItem(char * gameOrderId){
    [SimpleNativeManager  consumeItem:charToNSString(gameOrderId)];
}
