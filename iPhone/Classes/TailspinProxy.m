//
//  TailspinProxy.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/1/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "TailspinProxy.h"
#import "RequestFactory.h"
#import "JSON.h"
#import "SurveyResponseRepository.h"
#import "SurveyRepository.h"
#import "UserSettings.h"
#import "ASIHTTPRequest.h"
#import "ASINetworkQueue.h"
#import "SurveyFilterCategory.h"
#import "SurveyFilter.h"
#import "Question.h"
#import "Answer.h"
#import "SurveyResponse.h"
#import "Survey.h"
#import "UserSettings.h"
#import "UserSettingsService.h"

NSString * const OPERATION_KEY = @"operation";
NSString * const MODEL_OBJECT_KEY = @"modelObject";
NSString * const FETCH_SURVEYS_OPERATION = @"FetchSurveys";
NSString * const FETCH_FILTERS_OPERATION = @"GetFilters";
NSString * const SUBMIT_FILTERS_OPERATION = @"SetFilters";
NSString * const SUBMIT_SURVEY_OPERATION = @"SubmitSurvey";
NSString * const MEDIA_UPLOAD_OPERATION = @"MediaUpload";

@interface TailspinProxy(Private)

- (NSDictionary *)userInfoForOperation:(NSString *)operation;
- (NSString *)jsonForSurveyResponse:(SurveyResponse *)response;
- (void)uploadMediaFilesForResponses;
- (void)uploadSurveyResponses;
- (NSString *)jsonForFilters:(NSArray *)filters;

@end

@implementation TailspinProxy

#pragma mark -
#pragma mark Init

- (id) init {
	self = [super init];
	if (self != nil) {
		surveyQueue = [[ASINetworkQueue alloc] init];
		surveyQueue.delegate = self;
		surveyQueue.requestDidStartSelector = @selector(queueRequestDidStart:);
		surveyQueue.queueDidFinishSelector = @selector(queueDidFinish);
		surveyQueue.requestDidFinishSelector = @selector(queueRequestDidFinish:);
		surveyQueue.requestDidFailSelector = @selector(queueRequestDidFail:);
		
		mediaQueue = [[ASINetworkQueue alloc] init];
		mediaQueue.delegate = self;
		mediaQueue.requestDidStartSelector = @selector(mediaRequestDidStart:);
		mediaQueue.queueDidFinishSelector = @selector(mediaQueueDidFinish);
		mediaQueue.requestDidFinishSelector = @selector(mediaRequestDidFinish:);
		mediaQueue.requestDidFailSelector = @selector(mediaRequestDidFail:);
	}
	return self;
}


#pragma mark -
#pragma mark  Dealloc

- (void)dealloc {
	delegate = nil;
	RELEASE(surveyQueue);
	RELEASE(mediaQueue);
	RELEASE(localResponses);
	[super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize delegate;

#pragma mark -
#pragma mark Methods

- (void)asyncFetchSurveysForUser:(UserSettings *)userSettings{
    NSAssert(delegate, @"delegate must be set!");
	
	ASIHTTPRequest *request = nil;
	
	if (userSettings.lastSyncDate != nil) {
		NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
		formatter.timeZone = [NSTimeZone timeZoneWithName:@"GMT"];
		formatter.dateFormat = @"MM'/'dd'/'yyyy HH':'mm':'ss";
		NSString *utcDate = [formatter stringFromDate:userSettings.lastSyncDate];
		[formatter release];
		NSString *endPoint = [NSString stringWithFormat:@"/Survey/Surveys?lastSyncUtcDate=%@", utcDate];
		request = [RequestFactory requestWithEndpoint:endPoint];
	} else {
		request = [RequestFactory requestWithEndpoint:@"/Survey/Surveys"];
	}
	
	[request setUserInfo:[self userInfoForOperation:FETCH_SURVEYS_OPERATION]];
	request.delegate = self;
	
	[request startAsynchronous];
}

- (void)asyncFetchFilters {
	NSAssert(delegate, @"delegate must be set!");
	ASIHTTPRequest *request = [RequestFactory requestWithEndpoint:@"/Registration/GetFilters"];
	request.userInfo = [self userInfoForOperation:FETCH_FILTERS_OPERATION];
	request.delegate = self;
    request.cachePolicy = ASIOnlyLoadIfNotCachedCachePolicy;
    request.cacheStoragePolicy = ASICachePermanentlyCacheStoragePolicy; 
	
	[request startAsynchronous];
}

- (void)sendFilters:(NSArray *)categories {
	NSAssert(delegate, @"delegate must be set!");
	ASIHTTPRequest *request = [RequestFactory requestWithEndpoint:@"/Registration/SetFilters"];
	request.requestMethod = @"POST";
	request.userInfo = [self userInfoForOperation:SUBMIT_FILTERS_OPERATION];
	request.delegate = self;	
	NSData *postData = [[self jsonForFilters:categories] dataUsingEncoding:NSUTF8StringEncoding];
	request.postBody = [postData mutableCopy];
	[request startAsynchronous];
}

- (void)sendSurveyResponses:(NSArray *)responses {	
	NSAssert(delegate, @"delegate must be set!");
	didFindFailuresInQueue = NO;
	localResponses = [responses retain];
	[self uploadMediaFilesForResponses];
}

#pragma mark -
#pragma mark Private Methods

- (NSDictionary *)userInfoForOperation:(NSString *)operation {
    return [NSDictionary dictionaryWithObject:operation forKey:OPERATION_KEY];
}

- (NSString *)jsonForFilters:(NSArray *)categories {
	NSMutableDictionary *container = [NSMutableDictionary dictionaryWithCapacity:[categories count]];
	for (SurveyFilterCategory *category in categories) {
		NSMutableArray *filtersToSend = [NSMutableArray array];
		for (SurveyFilter *filter in category.selectedFilters) {
			[filtersToSend addObject:[NSDictionary dictionaryWithObjectsAndKeys:
									  filter.key,	@"Key",
									  filter.value,	@"Name", 
									  nil]];
			
		}
		[container setObject:filtersToSend forKey:category.name]; 
	}
		
	NSString *json = [container JSONRepresentation];
	LOGLINE(@"Generated JSON for SetFilters: %@", json);
	return json;
}

- (NSString *)jsonForSurveyResponse:(SurveyResponse *)response {
	Survey *survey = [[[[SurveyRepository alloc] init] autorelease] surveyWithId:response.surveyId];
	
	NSMutableArray *questions = [NSMutableArray array];
	for(int i=0; i < survey.questions.count; i++) {
		Question *q = [survey.questions objectAtIndex:i];
		Answer *a = [response answerForQuestionIndex:i];
		if (q.questionType != Voice || q.questionType != Picture) {
			[questions addObject:[NSDictionary dictionaryWithObjectsAndKeys:
								  a.answerText,			@"Answer",
								  q.possibleAnswers,	@"PossibleAnswers",
								  q.text,				@"QuestionText",
								  q.questionTypeString, @"QuestionType", nil]];
		}
	}
	
	NSArray *container = [NSArray arrayWithObject:
						  [NSDictionary dictionaryWithObjectsAndKeys:
						   @"",					@"CompletionLocation",
						   questions,			@"QuestionAnswers",
						   survey.slug,			@"SlugName",
						   @"",					@"StartLocation",
						   survey.tenant,		@"Tenant",
						   survey.title,		@"Title", nil]];
	
	NSString *json = [container JSONRepresentation];
	LOGLINE(@"Generated JSON: %@", json);
	return json;
}

- (void)uploadSurveyResponses {
	 for(SurveyResponse *response in localResponses) {		
		 ASIHTTPRequest *req = [RequestFactory requestWithEndpoint:@"/Survey/SurveyAnswers"];
		 req.requestMethod = @"POST";
		 req.userInfo = [NSDictionary dictionaryWithObjectsAndKeys:
						 SUBMIT_SURVEY_OPERATION, OPERATION_KEY,
						 response, MODEL_OBJECT_KEY, nil];
	 
		 NSData *postData = [[self jsonForSurveyResponse:response] dataUsingEncoding:NSUTF8StringEncoding];
		 req.postBody = [postData mutableCopy];
	 
	 
		 LOGLINE(@"Adding survey submission request to queue...");
		 [surveyQueue addOperation:req];
	 }
	 [surveyQueue go];
}

- (void)uploadMediaFilesForResponses {
	LOGLINE(@"Adding media upload requests to queue");
	SurveyRepository *surveyRepo = [[[SurveyRepository alloc] init] autorelease];
	for(SurveyResponse *response in localResponses) {
		Survey *survey = [surveyRepo surveyWithId:response.surveyId];
		for(int i=0; i < survey.questions.count; i++) {
			Question *q = [survey.questions objectAtIndex:i];
			if (q.questionType != Voice && q.questionType != Picture) {
				continue;
			}
			Answer *answer = [response answerForQuestionIndex:i];
			if(answer.localFileUrl == nil || answer.localFileUrl.length == 0) {
				LOGLINE(@"Nothing to upload for this answer...");
				continue;
			}
						
			if(answer.answerText != nil && answer.answerText.length > 0) {
				LOGLINE(@"This answer already has text");
				continue;
			}
			
			NSString *request = [NSString stringWithFormat:@"/Survey/MediaAnswer?type=%@", q.questionType == Voice ? @"Voice" : @"Picture"];
			LOGLINE(@"%@", request);
			ASIHTTPRequest *uploadRequest = [RequestFactory mediaRequestWithEndpoint:request];
			uploadRequest.requestMethod = @"POST";
			[uploadRequest setShouldStreamPostDataFromDisk:YES];
			uploadRequest.postBodyFilePath = answer.localFileUrl;
			LOGLINE(@"%@", answer.localFileUrl);
			uploadRequest.userInfo = [NSDictionary dictionaryWithObjectsAndKeys:
									  answer, MODEL_OBJECT_KEY,
									  MEDIA_UPLOAD_OPERATION, OPERATION_KEY, nil];
			
			[mediaQueue addOperation:uploadRequest];
		}
	}
	
	if (mediaQueue.requestsCount > 0) {
		[mediaQueue go];
	} else {
		[self uploadSurveyResponses];
	}

}

#pragma mark -
#pragma mark ASINetworkQueue Surveys Selector Callbacks

- (void)queueRequestDidStart:(ASIHTTPRequest *)request {
	LOGLINE(@"Survey queue request did start");
}

- (void)queueRequestDidFinish:(ASIHTTPRequest *)request {
	LOGLINE(@"Survey queue request did finish");
}

- (void)queueRequestDidFail:(ASIHTTPRequest *)request {
	LOGLINE(@"Survey queue request FAILED!");
	didFindFailuresInQueue = YES;
}

- (void)queueDidFinish {
	LOGLINE(@"Survey queue did finish");
	
	if(didFindFailuresInQueue) {
		if ([self.delegate respondsToSelector:@selector(didFailWithError:)]) {
			NSString *error = @"One or more of the requests failed";
			[self.delegate didFailWithError:error];
		}
	} else {
		[self.delegate didSendSurveyResponses:localResponses];
	}
}

#pragma mark -
#pragma mark ASINetworkQueue Media Selector Callbacks

- (void)mediaRequestDidStart:(ASIHTTPRequest *)request {
	LOGLINE(@"Media queue request did start");
}

- (void)mediaRequestDidFinish:(ASIHTTPRequest *)request {
	LOGLINE(@"Media queue request did finish");
    int statusCode = request.responseStatusCode;
    LOGLINE(@"Status Code: %d", statusCode);
	
	if (statusCode != 200) {
		didFindFailuresInQueue = YES;
		return;
	}
	
    NSString *responseString = [request responseString];
    LOGLINE(@"Response String: %@",responseString);
	
    NSString *operation = [request.userInfo objectForKey:OPERATION_KEY];
	if ([operation isEqualToString:MEDIA_UPLOAD_OPERATION]) {
		LOGLINE(@"Processing uploaded media...");
		Answer *answer = [request.userInfo objectForKey:MODEL_OBJECT_KEY];
		NSString *url = [responseString stringByReplacingOccurrencesOfString:@"\\" withString:@""];
		LOGLINE(@"%@", url);
		answer.answerText = url;
	}
}

- (void)meidaRequestDidFail:(ASIHTTPRequest *)request {
	LOGLINE(@"Media queue request FAILED!");
	didFindFailuresInQueue = YES;
}

- (void)mediaQueueDidFinish {
	LOGLINE(@"Media queue did finish");
	if(didFindFailuresInQueue) {
		if ([self.delegate respondsToSelector:@selector(didFailWithError:)]) {
			NSString *error = @"One or more of the requests failed";
			[self.delegate didFailWithError:error];
		}
	} else {
		[self uploadSurveyResponses];
	}
}

#pragma mark -
#pragma mark ASIHTTPRequestDelegate Methods

- (void)requestFailed:(ASIHTTPRequest *)request {
	NSMutableString *error = [NSMutableString stringWithString:@""];
	LOGLINE(@"Request Finished (%@)", request.url);
    int statusCode = request.responseStatusCode;
    LOGLINE(@"Status Code: %d", statusCode);
	LOGLINE(@"Response String: %@",[request responseString]);
	if (statusCode == 401) {
		[error appendFormat:@"Invalid user name and / or password. Check your credentials."];
	}
	[self.delegate didFailWithError:error];
}

-(void)requestFinished:(ASIHTTPRequest *)request {
    LOGLINE(@"Request Finished (%@)", request.url);
    int statusCode = request.responseStatusCode;
    LOGLINE(@"Status Code: %d", statusCode);
	
    NSString *responseString = [request responseString];
    LOGLINE(@"Response String: %@",responseString);
	
    NSString *operation = [request.userInfo objectForKey:OPERATION_KEY];
    
    if(statusCode != 200) {
        [self.delegate didFailWithError:[NSString stringWithFormat:@"ERROR %d during %@ operation.", statusCode, operation]];
        return;
    }
    
    if([operation isEqualToString:FETCH_SURVEYS_OPERATION]) {                           
        LOGLINE(@"Processing surveys...");
		NSArray *surveys = [responseString JSONValue];
        NSMutableArray *processedSurveys = [NSMutableArray array];
        for (NSDictionary *surveyProperties in surveys){
            [processedSurveys addObject:[Survey surveyFromDictionary:surveyProperties]];
        }
		UserSettings *settings = [UserSettingsService retrieve];
		settings.lastSyncDate = [NSDate date];
		[UserSettingsService save:settings];
        [self.delegate didFetchSurveys:processedSurveys];
    } else if([operation isEqualToString:FETCH_FILTERS_OPERATION]) {
        
		LOGLINE(@"Processing filters...");
        LOGLINE(@"Response JSON: %@", responseString);
		NSDictionary *responseDictionary = [responseString JSONValue];
        NSDictionary *filterData = [responseDictionary objectForKey:@"SurveyFilters"];
		
		
        NSMutableArray *filterCategories = [NSMutableArray array];
        for (NSString *key in [filterData allKeys]) {
			// this should be based on the key name of the survey filters / need to update the name
			NSArray *allFilters = [responseDictionary objectForKey:@"AllTenants"];
            SurveyFilterCategory *category = [SurveyFilterCategory categoryNamed:key 
																 selectedFilters:[filterData objectForKey:key] 
																	  allFilters:allFilters];
            [filterCategories addObject:category];
        }
		
		
        
        [self.delegate didFetchFilters:filterCategories];
	} else if ([operation isEqualToString:SUBMIT_FILTERS_OPERATION]) {
		[self.delegate didSendFilters];
	} 
}

@end
