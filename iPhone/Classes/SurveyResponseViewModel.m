//
//  SurveyResponseViewModel.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/19/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "SurveyResponseViewModel.h"
#import "AnswerViews.h"
#import "ViewHelpers.h"
#import "Question.h"
#import "QuestionView.h"
#import "Answer.h"

@interface SurveyResponseViewModel(Private)

- (Answer *)getAnswer:(NSInteger)questionIndex;
- (void)setAnswer:(Answer *)answer forIndex:(NSInteger)questionIndex;
- (MultipleChoiceAnswerView *)multipleChoiceAnswerViewForQuestion:(Question *)question atIndex:(NSInteger)questionIndex;
- (AnswerView *)answerViewForQuestion:(Question *)question atIndex:(NSInteger)questionIndex;

@end

@implementation SurveyResponseViewModel

#pragma mark -
#pragma mark Dealoc

- (void)dealloc {
	self.delegate = nil;
	
	RELEASE(surveyTitle);
	RELEASE(questions);
	RELEASE(answers);
	
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize surveyId, surveyTitle, questions, answers, delegate, surveyCompleted;

#pragma mark -
#pragma mark Methods
-(QuestionView *)viewForQuestion:(Question *)question atIndex:(NSInteger)questionIndex withTotalQuestions:(NSInteger)totalQuestions {
    
    QuestionView *qv = LoadViewFromNib([QuestionView class]);
    
	qv.questionLabel.text = question.text;
    qv.questionIndexLabel.text = [NSString stringWithFormat:@"%d out of %d", questionIndex + 1, totalQuestions];
	
	qv.answerView = [self answerViewForQuestion:question atIndex:questionIndex];
    qv.answerView.questionIndex = questionIndex;
	qv.answerView.tenant = question.tenant;
	qv.answerView.slug = question.slug;
	
    Answer *answer = [self getAnswer:questionIndex];
    if(question.questionType == Picture || question.questionType == Voice) {
        qv.answerView.answer = answer.localFileUrl;
    } else {
        qv.answerView.answer = answer.answerText;
    }	
	
    qv.answerView.delegate = self;
    
    return qv;
}

#pragma mark -
#pragma mark Private Methods
- (Answer *)getAnswer:(NSInteger)questionIndex {
    NSAssert(questionIndex <= answers.count, @"Don't have an answer for that index!  This shouldn't happen");
	Answer *a = [answers objectAtIndex:questionIndex];
	return a;
}

- (void)setAnswer:(Answer *)answer forIndex:(NSInteger)questionIndex {
    NSAssert(questionIndex <= answers.count, @"Don't have an answer for that index!  This shouldn't happen");
	
	if(answer.answerText == nil)
		answer.answerText = @"";
	
    [answers replaceObjectAtIndex:questionIndex withObject:answer];
}

- (MultipleChoiceAnswerView *)multipleChoiceAnswerViewForQuestion:(Question *)question atIndex:(NSInteger)questionIndex {	
	MultipleChoiceAnswerView *mav = LoadViewFromNib([MultipleChoiceAnswerView class]);
	
	mav.possibleAnswers = question.possibleAnswers;
	return mav;
}

- (AnswerView *)answerViewForQuestion:(Question *)question atIndex:(NSInteger)questionIndex {
    
	LOGLINE(@"The question type is %d", question.questionType);
	if (question.questionType == SimpleText) {
        return LoadViewFromNib([SimpleTextAnswerView class]);
    }
	
	if(question.questionType == MultipleChoice) {
		// not using loadViewFromNib b/c we are setting the datasource here.  
		return [self multipleChoiceAnswerViewForQuestion:question atIndex:questionIndex];
	}
	if(question.questionType == FiveStar){
		FiveStarAnswerView *fiveStarView = [[FiveStarAnswerView alloc] initWithFrame: CGRectMake(20.0f, 80.0f, 200.0f, 100.0f)];
		return [fiveStarView autorelease];
	}
	if(question.questionType == Picture){
		return LoadViewFromNib([PictureAnswerView class]);

	}
	if(question.questionType == Voice){
		return LoadViewFromNib([VoiceAnswerView class]);
	}
	return nil;
}

#pragma mark -
#pragma mark AnswerDelegate Methods

- (void)answerView:(AnswerView *)answerView didUpdateAnswer:(NSString *)answerText {
    LOGLINE(@"answerView didUpdateAnswer: %@, (question index: %d)", answerText, answerView.questionIndex);
    
    Question *question = [self.questions objectAtIndex:answerView.questionIndex];
	
    Answer *answer = [self getAnswer:answerView.questionIndex];
    if(question.questionType == Picture || question.questionType == Voice) {
        answer.localFileUrl = answerText;
		//Clear out the answer text b/c we need to upload the new file.
		answer.answerText = @"";

    } else {
        answer.answerText = answerText;
    }
    
    [self setAnswer:answer forIndex:answerView.questionIndex];
    [self.delegate surveyResponseViewModelDidChange:self];
}

@end
