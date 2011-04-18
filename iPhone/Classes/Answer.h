//
//  SimpleTextAnswer.h
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/25/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface Answer : NSObject {
    NSInteger answerId;
    NSInteger surveyResponseId;
    NSString *localFileUrl;  //used if we are saving a file as an answer
	NSString *answerText;
    NSInteger questionIndex;
}

@property (nonatomic, assign) NSInteger answerId;
@property (nonatomic, assign) NSInteger surveyResponseId;
@property (nonatomic, copy) NSString *localFileUrl;
@property (nonatomic, copy) NSString *answerText;
@property (nonatomic, assign) NSInteger questionIndex;

- (BOOL)isEmptyAnswer;

+ (Answer *)emptyAnswerAtQuestionIndex:(NSInteger)questionIndex;

@end
