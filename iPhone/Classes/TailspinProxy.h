//
//  TailspinProxy.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/1/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ASIHTTPRequestDelegate.h"

@class ASINetworkQueue;
@protocol TailspinProxyDelegate;
@class UserSettings;

@interface TailspinProxy : NSObject <ASIHTTPRequestDelegate> {
    id<TailspinProxyDelegate> delegate;
	ASINetworkQueue *surveyQueue;
	ASINetworkQueue *mediaQueue;
	BOOL didFindFailuresInQueue;
	NSArray *localResponses;
}

@property (nonatomic, assign) id<TailspinProxyDelegate> delegate;

-(void)asyncFetchSurveysForUser:(UserSettings *)userSettings;
-(void)asyncFetchFilters;
-(void)sendSurveyResponses:(NSArray *)responses;
-(void)sendFilters:(NSArray *)categories;

@end

@protocol TailspinProxyDelegate <NSObject>

@optional

- (void)didFetchSurveys:(NSArray *)surveys;
- (void)didFetchFilters:(NSArray *)filters;
- (void)didSendSurveyResponses:(NSArray *)responses;
- (void)didFailWithError:(NSString *)error;
- (void)didSendFilters;

@end