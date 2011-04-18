//
//  SurveyResponseService.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/19/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>

@class SurveyResponseViewModel;
@class Survey;

@interface SurveyResponseService : NSObject {

}

+ (SurveyResponseViewModel *)viewModelForSurveyId:(NSInteger)surveyId;
+ (SurveyResponseViewModel *)viewModelForSurvey:(Survey *)survey;

+ (void)persistResponse:(SurveyResponseViewModel *)viewModel;
+ (void)submitSurvey:(SurveyResponseViewModel *)viewModel;

@end