//
//  MoEUnityInitializer.h
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface MoEUnityInitializer : NSObject
@property(assign, nonatomic, readonly) BOOL isSDKIntialized;

+(instancetype)sharedInstance;
- (void)intializeSDKWithLaunchOptions:(NSDictionary*)launchOptions;
- (void)intializeSDKWithLaunchOptions:(NSDictionary*)launchOptions andSDKState:(BOOL)isSDKEnabled;
- (void)setupSDKWithGameObject:(NSString*)gameObjectName;
@end
