//
//  SurveyService.m
//  tailspin-iphone
//
//  Created by Scott Densmore on 9/29/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "SurveyService.h"
#import "Survey.h"
#import "SurveyRepository.h"

@implementation SurveyService

#pragma mark -
#pragma mark Class Methods

+ (void)surveyIsFavorite:(BOOL)isFavorite forSurveyId:(NSInteger)surveyId {
	SurveyRepository *repo = [[SurveyRepository alloc] init];
	Survey *s = [repo surveyWithId:surveyId];
	if (s != nil) {
		s.isFavorite = isFavorite;
		[repo save:s];
		LOGLINE(@"We just set the favorite to : %d for survey id %d", isFavorite, s.surveyId);
	}
	[repo release];
}

@end
