//
//  SurveyListing.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import "SurveyListing.h"

@implementation SurveyListing

#pragma mark -
#pragma mark Dealloc

-(void)dealloc {
	RELEASE(title);
	RELEASE(description);
	RELEASE(iconUrl);

	[super dealloc];
}


#pragma mark -
#pragma mark Properties

@synthesize surveyId;
@synthesize title;
@synthesize description;
@synthesize iconUrl;
@synthesize durationInMinutes;
@synthesize numberReadyToSubmit;
@synthesize percentComplete;
@synthesize isFavorite;

@end
