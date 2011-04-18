//
//  Question.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/19/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "Question.h"

@implementation Question

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(text);
	RELEASE(possibleAnswers);
    
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize questionId, surveyId, text, possibleAnswers, questionType, tenant, slug;

- (NSString *)possibleAnswersString {
    if(self.possibleAnswers == nil) {
        return nil;
    }
    
    return [self.possibleAnswers componentsJoinedByString:@"\n"];
}

- (void)setPossibleAnswersString:(NSString *)answerString {
    if (answerString == nil) {
        self.possibleAnswers = [NSArray array];
	} else {
        self.possibleAnswers = [answerString componentsSeparatedByString:@"\n"];
	}
}

- (NSString *)questionTypeString {
    switch(questionType) {
        case SimpleText: return @"SimpleText";
        case MultipleChoice: return @"MultipleChoice";
        case FiveStar: return @"FiveStars";
        case Picture: return @"Picture";
        case Voice: return @"Voice";
    }
    
    return nil;
}

- (void)setQuestionTypeString:(NSString *)typeString {
    if([typeString isEqualToString:@"SimpleText"])          self.questionType = SimpleText;
    else if([typeString isEqualToString:@"MultipleChoice"]) self.questionType = MultipleChoice;
    else if([typeString isEqualToString:@"FiveStars"])       self.questionType = FiveStar;
    else if([typeString isEqualToString:@"Picture"])        self.questionType = Picture;
    else if([typeString isEqualToString:@"Voice"])          self.questionType = Voice;
}

#pragma mark -
#pragma mark Class Methods

+ (Question *)questionFromDictionary:(NSDictionary *)dictionary {
    Question *q = [[[Question alloc] init] autorelease];
   
    q.text = [dictionary objectForKey:@"Text"];
    id possibleAnswersValue = [dictionary objectForKey:@"PossibleAnswers"];
    LOGLINE(@"possibleAnswersValue: %@", possibleAnswersValue);
    if(possibleAnswersValue != [NSNull null]) {
        q.possibleAnswersString = possibleAnswersValue;
    }
    
    q.questionTypeString = [dictionary objectForKey:@"Type"];
	    
    return q;
}

@end
