//
//  SurveyDetailView.m
//  tailspin-iphone
//
//  Created by Kevin Lee on 9/22/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "SurveyDetailView.h"


@implementation SurveyDetailView

#pragma mark -
#pragma mark Init

- (id)initWithFrame:(CGRect)frame {
    if ((self = [super initWithFrame:frame])) {
        // Initialization code
    }
    return self;
}

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(durationLabel);
	RELEASE(titleLabel);
	RELEASE(progressView);
	RELEASE(favoriteSwitch);
    [super dealloc];
}


#pragma mark -
#pragma mark Properties

@synthesize  favoriteSwitch, durationLabel, titleLabel, progressView;

@end
