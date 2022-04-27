#import <Foundation/Foundation.h>
#import "SimpleNativeBridgeManager.h"
#import "SimpleNativeManager.h"
#import <Unity/UnityInterface.h>

static const char* nsstringToChar(NSString* input){
    const char* string = [input UTF8String];
    return string ? strdup(string) : NULL;
}
static SimpleNativeBridgeManager *_instance = nil;

@implementation SimpleNativeBridgeManager

+(instancetype)sharedInstance{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _instance = [self alloc];
    });
    return _instance;
}
+ (void)load {
    if([[SimpleNativeIOSSDK sharedInstance] geUPInstance]){
        NSLog(@"SimpleNativeBridgeManager addObserver kUnityOnOpenURL");
        [[NSNotificationCenter defaultCenter] addObserver:[SimpleNativeBridgeManager sharedInstance] selector:@selector(openUrlFromUnity:) name:@"kUnityOnOpenURL" object:nil];
    }
}

-(void) openUrlFromUnity:(NSNotification *)notification{
    NSLog(@"SimpleNativeBridgeManager UnityBridge openUrlFromUnity");

    NSDictionary * notifData =  notification.userInfo;
    id url = [notifData objectForKey:@"url"];
    id sourceApplication = [notifData objectForKey:@"sourceApplication"];
    id annotation = [notifData objectForKey:@"annotation"];

    [[SimpleNativeIOSSDK sharedInstance] application:[UIApplication sharedApplication] openURL:url sourceApplication:sourceApplication annotation:annotation];
}

+ (void)callback:(NSString *)method params:(NSString *)params{
    NSLog(@"SimpleNativeBridgeManager callback %@ %@", method, params);
    UnitySendMessage("NativeInternalObject", nsstringToChar(method), nsstringToChar(params));
}


@end
