//
//  SurveyListingService.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import "SurveyListingService.h"
#import "SurveyRepository.h"
#import "SurveyResponseRepository.h"
#import "SurveyListing.h"
#import "SurveyResponse.h"
#import "Survey.h"

@interface SurveyListingService(Private)

- (NSMutableArray *)surveyListingsForSurveys:(NSArray *)surveys;
- (NSArray *)surveys;

@end

@implementation SurveyListingService

#pragma mark -
#pragma mark Methods

- (NSArray *)surveyListings {
    NSArray *surveys = [self surveys];
    return [self surveyListingsForSurveys:surveys];       
}

- (NSArray *)surveyListingsFavorites {
	NSArray *listings = [self surveyListings];
	NSMutableArray *favorites = [NSMutableArray array];
	
	for(SurveyListing *s in listings){
		if (s.isFavorite){
			[favorites addObject:s];
		}
	}
	return favorites;
}

- (NSArray *)surveyListingsOrderedByDuration{
	NSArray *listings = [self surveyListings];
	NSArray *descriptors = [[[NSArray alloc] initWithObjects:[NSSortDescriptor sortDescriptorWithKey:@"durationInMinutes" ascending:YES], nil] autorelease];
	return [listings sortedArrayUsingDescriptors:descriptors];
}

#pragma mark -
#pragma mark Private Methods

- (NSMutableArray *)surveyListingsForSurveys:(NSArray *)surveys {
	NSMutableArray *listings = [NSMutableArray array];
	
	SurveyResponseRepository *responseRepository = [[SurveyResponseRepository alloc] init];
	
    for(Survey *s in surveys) {
		SurveyResponse* response = [responseRepository surveyResponseForSurveyId:s.surveyId];
		SurveyListing *listing = [[SurveyListing alloc] init];
		listing.surveyId = s.surveyId;
        listing.title = s.title;
        listing.description = s.description;
        listing.durationInMinutes = s.durationInMinutes;
		listing.numberReadyToSubmit = s.numberReadyToSubmit;
		listing.iconUrl = s.imageUrl;
        listing.isFavorite = s.isFavorite;
        listing.percentComplete = response.progressPercentage;
		
        [listings addObject:listing];
		[listing release];
	}
	
	[responseRepository release];
	
	return listings;
}

- (NSArray *)surveys {
	SurveyRepository *repo = [[SurveyRepository alloc] init];
    NSArray *surveys = [repo surveys];
	[repo release];
	return surveys;
	
}

@end
