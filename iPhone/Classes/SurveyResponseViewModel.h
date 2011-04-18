//
//  SurveyResponseViewModel.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/19/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "AnswerView.h"

@protocol SurveyResponseDelegate;
@class Question;
@class QuestionView;

@interface SurveyResponseViewModel : NSObject <AnswerDelegate> {
    NSInteger surveyId;
    NSString *surveyTitle;
    NSArray *questions;
    NSMutableArray *answers;
	BOOL surveyCompleted;
    id<SurveyResponseDelegate> delegate;
}

@property (nonatomic, assign) NSInteger surveyId;
@property (nonatomic, copy) NSString *surveyTitle;
@property (nonatomic, retain) NSArray *questions;
@property (nonatomic, retain) NSMutableArray *answers;
@property (nonatomic, assign) id<SurveyResponseDelegate> delegate;
@property (nonatomic, assign) BOOL surveyCompleted;

-(QuestionView *)viewForQuestion:(Question *)question atIndex:(NSInteger)questionIndex withTotalQuestions:(NSInteger)totalQuestions;

@end


@protocol SurveyResponseDelegate <NSObject>

- (void)surveyResponseViewModelDidChange:(SurveyResponseViewModel *)viewModel;

@end