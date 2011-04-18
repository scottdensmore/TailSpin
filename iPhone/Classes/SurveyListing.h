//
//  SurveyListing.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface SurveyListing : NSObject {
	NSInteger surveyId;
	NSString *title;
	NSString *description;
	NSString *iconUrl;
	NSInteger durationInMinutes;
	NSInteger numberReadyToSubmit;
	CGFloat percentComplete;
	BOOL isFavorite;	
}

@property (nonatomic) NSInteger surveyId;
@property (nonatomic, copy) NSString *title;
@property (nonatomic, copy) NSString *description;
@property (nonatomic, copy) NSString *iconUrl;
@property (nonatomic) NSInteger durationInMinutes;
@property (nonatomic) NSInteger numberReadyToSubmit;
@property (nonatomic) CGFloat percentComplete;
@property (nonatomic, getter=isFavorite) BOOL isFavorite;

@end
