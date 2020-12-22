
//
//  MoEUnityAppController.mm
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import "UnityAppController.h"
#import "AppDelegateListener.h"
#import "MoEUnityInitializer.h"

@interface MoEUnityAppController : UnityAppController <AppDelegateListener>

@end

@implementation MoEUnityAppController

- (instancetype)init
{
    self = [super init];
    if (self) {
        UnityRegisterAppDelegateListener(self);
    }
    return self;
}

# pragma mark - UIApplicationDelegate methods

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    NSLog(@"MoEAppDelegate called from application:didFinishLaunchingWithOptions:");
    [[MoEUnityInitializer sharedInstance] intializeSDKWithLaunchOptions:launchOptions];
    return YES;
}

@end

IMPL_APP_CONTROLLER_SUBCLASS(MoEUnityAppController)

