//
//  Survey.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface Survey : NSObject {
    NSInteger surveyId;
	NSString *title;
	NSString *imageUrl;
	NSString *description;
	NSInteger durationInMinutes;
	NSInteger numberReadyToSubmit;
    NSArray *questions;
    NSString *tenant;
    NSString *slug;
	BOOL isFavorite;
}

@property (nonatomic) NSInteger surveyId;
@property (nonatomic, copy) NSString *title;
@property (nonatomic, copy) NSString *imageUrl;
@property (nonatomic, copy) NSString *description;
@property (nonatomic) NSInteger durationInMinutes;
@property (nonatomic) NSInteger numberReadyToSubmit;
@property (nonatomic, retain) NSArray *questions;
@property (nonatomic, copy) NSString *tenant;
@property (nonatomic, readonly) NSString *key;
@property (nonatomic, copy) NSString *slug;
@property (nonatomic) BOOL isFavorite;

+ (Survey *)surveyFromDictionary:(NSDictionary *)dictionary;
@end
