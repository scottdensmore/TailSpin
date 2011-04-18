//
//  SurveyResponseService.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/19/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "SurveyResponseService.h"
#import "SurveyRepository.h"
#import "SurveyResponseRepository.h"
#import "SurveyResponseViewModel.h"
#import "Survey.h"
#import "Answer.h"
#import "SurveyResponse.h"

@implementation SurveyResponseService

#pragma mark -
#pragma mark Class Methods

+ (SurveyResponseViewModel *)viewModelForSurveyId:(NSInteger)surveyId {
	SurveyRepository *repo = [[SurveyRepository alloc] init];
	Survey *survey = [repo surveyWithId:surveyId];
	[repo release];
	
	return [SurveyResponseService viewModelForSurvey:survey];
}

+ (SurveyResponseViewModel *)viewModelForSurvey:(Survey *)survey {
    SurveyResponseViewModel *vm = [[SurveyResponseViewModel alloc] init];
    vm.surveyId = survey.surveyId;
    vm.surveyTitle = survey.title;
    vm.questions = survey.questions;
    vm.answers = [NSMutableArray array];
    
    SurveyResponseRepository *responseRepository = [[SurveyResponseRepository alloc] init];
    SurveyResponse* response = [responseRepository surveyResponseForSurveyId:survey.surveyId];
	vm.surveyCompleted = response.progressPercentage >= 1.0f;
    
    //add all the question's answers in order
	for(int i=0; i<vm.questions.count; i++) {
        Answer *answer = [response answerForQuestionIndex:i];
        if(answer == nil) {
            //insert a "Null Object" in order to keep the order correct, as well as to make binding much easier
			answer = [Answer emptyAnswerAtQuestionIndex:i];
        }
		
        [vm.answers addObject:answer];
    }
    [responseRepository release];
    return [vm autorelease];
}

+ (void)persistResponse:(SurveyResponseViewModel *)viewModel {
    
    SurveyResponseRepository *repo = [[SurveyResponseRepository alloc] init];
    
    SurveyResponse *response = [repo surveyResponseForSurveyId:viewModel.surveyId];
    if(response == nil) {
        response = [[[SurveyResponse alloc] init] autorelease];
        response.surveyId = viewModel.surveyId;
    }
    
    //set answers
    for(Answer *answer in viewModel.answers) {
        [response updateAnswer:answer];
    }
    
    [repo save:response];
	
	viewModel.surveyCompleted = response.progressPercentage >= 1.0f;
        
    [repo release];
}

+ (void)submitSurvey:(SurveyResponseViewModel *)viewModel {
	SurveyResponseRepository *repo = [[SurveyResponseRepository alloc] init];
	SurveyResponse *response = [repo surveyResponseForSurveyId:viewModel.surveyId];
	response.readyForSubmission = YES;
	[repo save:response];
	[repo release];
}

@end
