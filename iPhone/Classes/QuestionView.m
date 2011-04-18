//
//  QuestionView.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/20/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "QuestionView.h"
#import "AnswerView.h"

@implementation QuestionView

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(questionLabel);
	RELEASE(questionIndexLabel);
	RELEASE(answerContainer);
	RELEASE(answerView);
	
    [super dealloc];
}


#pragma mark -
#pragma mark Properties

@synthesize questionLabel, questionIndexLabel, answerContainer, answerView;

- (id)answer {
    if (self.answerView)
        return self.answerView.answer;
    return nil;
}

- (void)setAnswer:(id)answer {
    if (self.answerView)
        [self.answerView setAnswer:answer];
}

#pragma mark -
#pragma mark View Methods

- (void)layoutSubviews {
    //set up the answer view frame (if necessary)
    if (self.answerView) {
		if (![[self.answerContainer subviews] containsObject:self.answerView]) {
			[self.answerContainer addSubview:self.answerView];	
		}
		
		self.answerView.frame = self.answerContainer.bounds;
    }
}

@end
