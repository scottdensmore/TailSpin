//
//  SurveyResponseRepository.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/22/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "RepositoryBase.h"

@class SurveyResponse;

@interface SurveyResponseRepository : RepositoryBase {

}

- (SurveyResponse *)surveyResponseForSurveyId:(NSInteger)surveyId;
- (void)save:(SurveyResponse *)response;
- (NSArray *)allResponses;
- (NSArray *)responsesReadyForSubmission;

@end
