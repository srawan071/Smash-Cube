#import <Foundation/Foundation.h>

@interface SimpleNativeBridgeManager:NSObject

+ (void)callback:(NSString *)method params:(NSString *)params;

@end
