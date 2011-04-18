//
//  SurveyRepository.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "RepositoryBase.h"

@class Survey;

@interface SurveyRepository : RepositoryBase {
}

-(NSArray *)surveys;
-(void)save:(Survey *)survey;
-(Survey *)surveyWithId:(NSInteger)surveyId;

@end