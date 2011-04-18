//
//  Question.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/19/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef enum QuestionType {
    SimpleText,
    MultipleChoice,
    FiveStar,
    Picture,
    Voice
} QuestionType;

@interface Question : NSObject {
    NSInteger questionId;
    NSInteger surveyId;
    NSString *text;
    NSArray *possibleAnswers;
    QuestionType questionType;
	NSString *tenant;
	NSString *slug;
}

@property (nonatomic) NSInteger questionId;
@property (nonatomic) NSInteger surveyId;
@property (nonatomic, copy) NSString *text;
@property (nonatomic, retain) NSArray *possibleAnswers;
@property (nonatomic, assign) NSString *possibleAnswersString;
@property (nonatomic) QuestionType questionType;
@property (nonatomic, assign) NSString *questionTypeString;
@property (nonatomic, assign) NSString *tenant;
@property (nonatomic, assign) NSString *slug;

+ (Question *)questionFromDictionary:(NSDictionary *)dictionary;

@end