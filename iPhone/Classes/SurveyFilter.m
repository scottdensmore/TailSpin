//
//  SurveyFilter.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/21/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "SurveyFilter.h"


@implementation SurveyFilter

#pragma mark -
#pragma mark Init

- (id)init 
{
	return [self initWithKey:@"" value:@""];
}

- (id)initWithKey:(NSString*)aKey value:(NSString*)aValue 
{
    self = [super init];
    if (self) {
        self.key = aKey;
        self.value = aValue;
    }
    return self;
}

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
    RELEASE(key);
	RELEASE(value);
    
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize key, value;

#pragma mark -
#pragma mark Equality Overrides
- (BOOL)isEqual: (id)other {
	return ([other isKindOfClass: [SurveyFilter class]] && 
			[[other key] isEqual:key]);
}

- (NSUInteger)hash
{
	return [key hash];
}
@end
