
//
//  MoEUnityBinding.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import <MoEPluginBase/MoEPluginBase.h>
#import <MoEngage/MoEngage.h>
#import "MoEUnityInitializer.h"

extern "C"{

#pragma mark- Utils Methods
void enableLogs() {
    [[MoEPluginBridge sharedInstance] enableLogs];
}

NSString* getNSStringFromChar(const char* str) {
    return str != NULL ? [NSString stringWithUTF8String:str] : [NSString stringWithUTF8String:""];
}

NSString* getJSONString(id val) {
    NSString *jsonString;
    
    if (val == nil) {
        return nil;
    }
    
    if ([val isKindOfClass:[NSArray class]] || [val isKindOfClass:[NSDictionary class]]) {
        NSError *error;
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:val options:NSJSONWritingPrettyPrinted error:&error];
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        
        if (error != nil) {
            jsonString = nil;
        }
    } else {
        jsonString = [NSString stringWithFormat:@"%@", val];
    }
    
    return jsonString;
}

NSMutableArray* getNSArrayFromArray(const char* array[], int size) {
    
    NSMutableArray *values = [NSMutableArray arrayWithCapacity:size];
    for (int i = 0; i < size; i ++) {
        NSString *value = getNSStringFromChar(array[i]);
        [values addObject:value];
    }
    
    return values;
}

NSMutableDictionary* getDictionaryFromJSON(const char* jsonString) {
    
    NSMutableDictionary *dict = [NSMutableDictionary dictionaryWithCapacity:1];
    
    if (jsonString != NULL && jsonString != nil) {
        NSError *jsonError;
        NSData *objectData = [getNSStringFromChar(jsonString) dataUsingEncoding:NSUTF8StringEncoding];
        dict = [NSJSONSerialization JSONObjectWithData:objectData
                                               options:NSJSONReadingMutableContainers
                                                 error:&jsonError];
    }
    
    return dict;
}

NSMutableArray* moe_NSArrayFromJsonString(const char* jsonString) {
    NSMutableArray *arr = [NSMutableArray arrayWithCapacity:1];
    
    if (jsonString != NULL && jsonString != nil) {
        NSError *jsonError;
        NSData *objectData = [getNSStringFromChar(jsonString) dataUsingEncoding:NSUTF8StringEncoding];
        arr = [NSJSONSerialization JSONObjectWithData:objectData
                                              options:NSJSONReadingMutableContainers
                                                error:&jsonError];
    }
    
    return arr;
}


char* moe_cStringCopy(const char* string) {
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    
    return res;
}


#pragma mark- Unity Init

void initialize(const char* gameObjPayload){
    NSMutableDictionary *gameObjDict = getDictionaryFromJSON(gameObjPayload);
    NSString* gameObjectName = [gameObjDict validObjectForKey:@"gameObjectName"];
    [[MoEUnityInitializer sharedInstance] setupSDKWithGameObject:gameObjectName];
}

#pragma mark- INSTALL/UPDATE Tracking

void setAppStatus(const char* appStatusPayload){
    NSMutableDictionary *appStatusDict = getDictionaryFromJSON(appStatusPayload);
    [[MoEPluginBridge sharedInstance] setAppStatus:appStatusDict];
}

#pragma mark- User Attributes

void setUserAttribute(const char* userAttrPayload){
    NSMutableDictionary *userAttrDict = getDictionaryFromJSON(userAttrPayload);
    [[MoEPluginBridge sharedInstance] setUserAttributeWithPayload:userAttrDict];
}

void setAlias(const char* aliasPayload){
    NSMutableDictionary *aliasDict = getDictionaryFromJSON(aliasPayload);
    [[MoEPluginBridge sharedInstance] setAlias:aliasDict];
}

#pragma mark- Track Event

void trackEvent(const char* eventPayload) {
    NSMutableDictionary *eventPayloadDict = getDictionaryFromJSON(eventPayload);
    [[MoEPluginBridge sharedInstance] trackEventWithPayload:eventPayloadDict];
}

#pragma mark- Push Notification

void registerForPush() {
    [[MoEPluginBridge sharedInstance] registerForPush];
}

#pragma mark- InApp Nativ
void showInApp() {
    [[MoEPluginBridge sharedInstance] showInApp];
}

void setInAppContexts(const char* contextsPayload){
    NSMutableDictionary *contextsPayloadDict = getDictionaryFromJSON(contextsPayload);
    [[MoEPluginBridge sharedInstance] setInAppContexts:contextsPayloadDict];
}

void invalidateInAppContexts(){
    [[MoEPluginBridge sharedInstance] invalidateInAppContexts];
}

void getSelfHandledInApp() {
    [[MoEPluginBridge sharedInstance] getSelfHandledInApp];
}

void updateSelfHandledInAppStatusWithPayload(const char* selfHandledPayload){
    NSMutableDictionary *selfHandledCampaignDict = getDictionaryFromJSON(selfHandledPayload);
    [[MoEPluginBridge sharedInstance] updateSelfHandledInAppStatusWithPayload:selfHandledCampaignDict];
}

#pragma mark- Geofence
void startGeofenceMonitoring() {
    [[MoEPluginBridge sharedInstance] startGeofenceMonitoring];
}

#pragma mark- OptOuts

void optOutOfIDFATracking(const char* optOutPayload) {
    NSMutableDictionary *optOutDict = getDictionaryFromJSON(optOutPayload);
    id optOutVal = [optOutDict validObjectForKey:@"isOptedOut"];
    if (optOutVal) {
        [[MOAnalytics sharedInstance] optOutOfIDFATracking:[optOutVal boolValue]];
    }
}

void optOutOfIDFVTracking(const char* optOutPayload) {
    NSMutableDictionary *optOutDict = getDictionaryFromJSON(optOutPayload);
    id optOutVal = [optOutDict validObjectForKey:@"isOptedOut"];
    if (optOutVal) {
        [[MOAnalytics sharedInstance] optOutOfIDFVTracking:[optOutVal boolValue]];
    }
}

void optOutGDPRTracking(const char* optOutPayload) {
    NSMutableDictionary *optOutDict = getDictionaryFromJSON(optOutPayload);
    [[MoEPluginBridge sharedInstance] optOutTracking:optOutDict];
}

#pragma mark- Reset User
void resetUser(){
    [[MoEPluginBridge sharedInstance] resetUser];
}

#pragma mark- Update SDK State
void updateSdkState(const char* sdkStatePayload){
    NSMutableDictionary *sdkStateDict = getDictionaryFromJSON(sdkStatePayload);
    if (sdkStateDict) {
        [[MoEPluginBridge sharedInstance] updateSDKState:sdkStateDict];
    }
}

}

