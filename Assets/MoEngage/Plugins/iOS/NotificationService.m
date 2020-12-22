//
//  NotificationService.m
//  MoENotificationServiceExtension
//
//  Created by Chengappa C D on 04/04/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import "NotificationService.h"
#import <MORichNotification/MORichNotification.h>

@interface NotificationService ()
@property (nonatomic, strong) void (^contentHandler)(UNNotificationContent *contentToDeliver);
@property (nonatomic, strong) UNMutableNotificationContent *bestAttemptContent;
@end

@implementation NotificationService

- (void)didReceiveNotificationRequest:(UNNotificationRequest *)request withContentHandler:(void (^)(UNNotificationContent * _Nonnull))contentHandler {
    
    NSString* appGroupId = [self getAppGroupID];
    [MORichNotification setAppGroupID:appGroupId];
    
    self.contentHandler = contentHandler;
    self.bestAttemptContent = [request.content mutableCopy];
    
    // Handle Rich Notification
    [MORichNotification handleRichNotificationRequest:request withContentHandler:contentHandler];
    
}


/// Save the image to disk
- (void)serviceExtensionTimeWillExpire {
    // Called just before the extension will be terminated by the system.
    // Use this as an opportunity to deliver your "best attempt" at modified content, otherwise the original push payload will be used.
    self.contentHandler(self.bestAttemptContent);
}

-(NSString*)getAppGroupID{
    NSString *extbundleIdentifier = [[NSBundle bundleForClass:[self class]] bundleIdentifier];
    NSString *parentBundleIdentifier = [extbundleIdentifier stringByReplacingOccurrencesOfString:@".MoENotificationServiceExtension" withString:@""];
    NSString *appGroupID = [NSString stringWithFormat:@"group.%@.moengage",parentBundleIdentifier];
    return appGroupID;
}

@end


