//
//  SurveyResponse.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import <Foundation/Foundation.h>

@class Answer;

@interface SurveyResponse : NSObject {
    NSInteger		surveyResponseId;
    NSInteger		surveyId;
    float			progressPercentage;
    BOOL			readyForSubmission;
    //NSDate			*lastSyncDate;
    NSMutableArray	*answers;
}

@property (nonatomic, assign) NSInteger surveyResponseId;
@property (nonatomic, assign) NSInteger surveyId;
@property (nonatomic, assign) float     progressPercentage;
@property (nonatomic, assign) BOOL      readyForSubmission;
//@property (nonatomic, retain) NSDate    *lastSyncDate;
@property (nonatomic, retain) NSArray   *answers;

- (Answer *)answerForQuestionIndex:(NSInteger)questionIndex;
- (void)updateAnswer:(Answer *)answer;
- (void)removeAnswers;
@end
