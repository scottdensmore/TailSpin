//
//  Class.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/21/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface SurveyFilterCategory : NSObject {
    NSString *name;
    NSMutableArray *selectedFilters;
	NSArray *allFilters;
}

@property (nonatomic, copy) NSString *name;
@property (nonatomic, retain) NSMutableArray *selectedFilters;
@property (nonatomic, retain) NSArray *allFilters;

+ (SurveyFilterCategory *)categoryNamed:(NSString *)name selectedFilters:(NSArray *)selectedFilters allFilters:(NSArray *)allFilters;

@end
