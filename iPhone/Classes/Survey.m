//
//  Survey.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import "Survey.h"
#import "Question.h"

@implementation Survey

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(title);
	RELEASE(imageUrl);
	RELEASE(description);
	RELEASE(tenant);
	RELEASE(slug);
	RELEASE(questions);
	
	[super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize surveyId, title, imageUrl, description, durationInMinutes, questions, tenant, slug, isFavorite, numberReadyToSubmit;

-(NSString *)key {
    return [NSString stringWithFormat:@"%@_%@", self.tenant, self.slug];
}

#pragma mark -
#pragma mark Class Methods

+ (Survey *)surveyFromDictionary:(NSDictionary *)dictionary{
	Survey *s1 = [[[Survey alloc] init] autorelease];
	s1.title = [dictionary objectForKey:@"Title"];
	s1.imageUrl =[dictionary objectForKey:@"IconUrl"];
	s1.description = [dictionary objectForKey:@"SlugName"];
	s1.tenant = [dictionary objectForKey:@"Tenant"];
	s1.durationInMinutes =(NSInteger)[dictionary objectForKey:@"Length"];
    s1.slug	= [dictionary objectForKey:@"SlugName"];
    NSMutableArray *questions = [NSMutableArray array];
    for(NSDictionary *questionProperties in [dictionary objectForKey:@"Questions"]) {
        Question *q = [Question questionFromDictionary:questionProperties];
        [questions addObject:q];
    }
    
    s1.questions = questions;
    
	return s1;
}
@end
