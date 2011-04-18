//
//  SyncService.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/13/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "SyncService.h"
#import "TailspinProxy.h"
#import "SurveyRepository.h"
#import "SurveyResponseRepository.h"
#import "UserSettings.h"
#import "UserSettingsService.h"

@interface SyncService(Private)

- (void)checkForCompletion;
- (void)sendFinishedSurveyResponses;

@end

@implementation SyncService

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	self.delegate = nil;
	RELEASE(proxy);
	
	[super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize delegate;

#pragma mark -
#pragma mark Methods

-(void)syncSurveys {
	if(!proxy) {
		proxy = [[TailspinProxy alloc] init];
	}
	
	sentSurveys = NO;
	fetchedSurveys = NO;
	
    proxy.delegate = self;
	UserSettings *settings = [UserSettingsService retrieve];
    [proxy asyncFetchSurveysForUser:settings];
	[self sendFinishedSurveyResponses];
}


#pragma mark -
#pragma mark Private Methods

- (void)checkForCompletion {
	if(sentSurveys && fetchedSurveys) {
		[self.delegate syncServiceDidComplete:YES];
	}
}

- (void)sendFinishedSurveyResponses {
	SurveyResponseRepository *responseRepo = [[SurveyResponseRepository alloc] init];
	NSArray *responses = [responseRepo responsesReadyForSubmission];
	if(responses != nil && responses.count > 0) {
		[proxy sendSurveyResponses:responses];
	} else {
		sentSurveys = YES;  //nothing to do
	}
	[responseRepo release];
}


#pragma mark -
#pragma mark TailspinProxyDelegate Methods

- (void)didFetchSurveys:(NSArray *)surveys {
    SurveyRepository *repo = [[SurveyRepository alloc] init];
    for(Survey *s in surveys) {
        [repo save:s];
    }
	[repo release];
	
	fetchedSurveys = YES;
	[self checkForCompletion];
}

- (void)didFailWithError:(NSString *)error {
    LOGLINE(@"FAILURE! %@", error);
	[self.delegate syncServiceDidFailWithError:error];
}

- (void)didSendSurveyResponses:(NSArray *)responses {
	SurveyResponseRepository *responseRepo = [[SurveyResponseRepository alloc] init];
	for(SurveyResponse *response in responses) {
		
		[responseRepo delete:response];
	}
	sentSurveys = YES;
	[responseRepo release];
	[self checkForCompletion];
}

@end
