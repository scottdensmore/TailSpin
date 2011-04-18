//
//  AnswerView.m
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/26/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "AnswerView.h"


@implementation AnswerView

#pragma mark -
#pragma mark Dealloc

-(void)dealloc {
	delegate = nil;
	RELEASE(answerLabel);
	RELEASE(answer);
    RELEASE(tenant);
	RELEASE(slug);
	
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize answerLabel, answer, delegate, questionIndex, tenant, slug;

@end
