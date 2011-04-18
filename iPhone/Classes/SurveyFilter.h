//
//  SurveyFilter.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/21/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface SurveyFilter : NSObject {
    NSString *key;
    NSString *value;
}

@property (nonatomic, copy) NSString *key;
@property (nonatomic, copy) NSString *value;

- (id)initWithKey:(NSString*)aKey value:(NSString*)aValue;
@end
