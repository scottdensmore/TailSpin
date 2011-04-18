//
//  SurveyResponse.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import "SurveyResponse.h"
#import "Answer.h"

@implementation SurveyResponse

#pragma mark -
#pragma mark Init

- (id)init {
    if((self = [super init])) {
        answers = [[NSMutableArray alloc] init];
    }
    
    return self;
}

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	//RELEASE(lastSyncDate);
	RELEASE(answers);

    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize surveyResponseId, surveyId, progressPercentage, readyForSubmission, /*lastSyncDate,*/ answers;

#pragma mark -
#pragma mark Methods

- (Answer *)answerForQuestionIndex:(NSInteger)questionIndex {
    for(Answer *a in answers) {
		if(a.questionIndex == questionIndex) {
			return a;
		}
	}
    
    return nil;
}


- (void)updateAnswer:(Answer *)answer {
    Answer *existing = [self answerForQuestionIndex:answer.questionIndex];
	if (answer.answerText == nil) {
		answer.answerText = @"";
	}
	
    if (existing == nil) {
        [answers addObject:answer];
    } else {
        NSInteger i = [answers indexOfObject:existing];
        [answers replaceObjectAtIndex:i withObject:answer];
    }
	
	NSInteger numAnswered = 0;
	for(Answer *a in answers) {
		if (![a isEmptyAnswer]) numAnswered++;
	}
	
	self.progressPercentage = numAnswered / (float)answers.count;
}

- (void)removeAnswers {
	[answers removeAllObjects];
}

@end
