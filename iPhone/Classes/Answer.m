//
//  SimpleTextAnswer.m
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/25/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "Answer.h"

@interface Answer(Private)

- (BOOL)isEmptyAnswerText;
- (BOOL)isEmptyLocalFileUrl;

@end

@implementation Answer

#pragma mark -
#pragma mark Deallc

- (void)dealloc {
	RELEASE(localFileUrl);
	RELEASE(answerText);
	
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize answerId, surveyResponseId, localFileUrl, answerText, questionIndex;

#pragma mark -
#pragma mark Methods

- (NSString *)description {
	if ([self isEmptyAnswer]) {
		return @"[Empty Answer]";
	} else {
		return [NSString stringWithFormat:@"[Answer: %@", self.answerText];
	}
}

-(BOOL)isEmptyAnswer {
	return [self isEmptyAnswerText] && [self isEmptyLocalFileUrl];
}

#pragma mark -
#pragma mark Private Methods

- (BOOL)isEmptyAnswerText {
	return self.answerText == nil || [self.answerText isEqualToString:@""];
}

- (BOOL)isEmptyLocalFileUrl {
	return self.localFileUrl == nil || [self.localFileUrl isEqualToString:@""];
}


#pragma mark -
#pragma mark Class Methods

+ (Answer *)emptyAnswerAtQuestionIndex:(NSInteger)questionIndex {
	Answer *answer = [[[Answer alloc] init] autorelease];
	answer.questionIndex = questionIndex;
	return answer;
}


@end
