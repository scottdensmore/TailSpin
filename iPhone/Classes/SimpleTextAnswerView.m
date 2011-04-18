//
//  SimpleTextAnswerView.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/23/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "SimpleTextAnswerView.h"

@implementation SimpleTextAnswerView

#pragma mark -
#pragma mark Init

- (id)initWithFrame:(CGRect)frame {
    if ((self = [super initWithFrame:frame])) {
    }
    return self;
}

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	delegate = nil;
	answerInput.delegate = nil;
	RELEASE(answerInput);
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize answerInput;

#pragma mark -
#pragma mark View Methods

-(void)layoutSubviews {
	[super layoutSubviews];
	self.answerInput.text = self.answer;
}

#pragma mark -
#pragma mark Actions

-(IBAction) textFieldDoneEditing:(id)sender{    
	[sender resignFirstResponder];
    self.answer = answerInput.text;
    [self.delegate answerView:self didUpdateAnswer:self.answer];
}

@end