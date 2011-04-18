//
//  SurveyListingService.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface SurveyListingService : NSObject  {
}

- (NSArray *)surveyListings;
- (NSArray *)surveyListingsFavorites;
- (NSArray *)surveyListingsOrderedByDuration;

@end

