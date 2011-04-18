//
//  Class.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/21/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "SurveyFilterCategory.h"
#import "SurveyFilter.h"

@implementation SurveyFilterCategory

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
    RELEASE(name);
	RELEASE(selectedFilters);
	RELEASE(allFilters);
    
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize name;
@synthesize selectedFilters;
@synthesize allFilters;

#pragma mark -
#pragma mark Class Methods

+ (SurveyFilterCategory *)categoryNamed:(NSString *)name selectedFilters:(NSArray *)selectedFilters allFilters:(NSArray *)allFilters {
    SurveyFilterCategory *category = [[SurveyFilterCategory alloc] init];
    category.name = name;
	
	category.selectedFilters = [NSMutableArray array];
    for (NSDictionary *selectedFilterDict in selectedFilters) {
        SurveyFilter *filter = [[SurveyFilter alloc] initWithKey:[selectedFilterDict objectForKey:@"Key"] value:[selectedFilterDict objectForKey:@"Name"]];
        [category.selectedFilters addObject:filter];
        [filter release];
    }
	
	NSMutableArray *localAllFilters = [NSMutableArray array];
	for (NSDictionary *filterDict in allFilters) {
		SurveyFilter *filter = [[SurveyFilter alloc] initWithKey:[filterDict objectForKey:@"Key"] value:[filterDict objectForKey:@"Name"]];
		[localAllFilters addObject:filter];
		[filter release];
	}
	category.allFilters = localAllFilters;
	
    return [category autorelease];
}

@end
