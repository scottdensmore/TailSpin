//
//  SurveyResponseRepository.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/22/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "SurveyResponseRepository.h"
#import "SurveyResponse.h"
#import "Answer.h"
#import "FMDatabase.h"
#import "FMDatabaseAdditions.h"

@interface SurveyResponseRepository(Private)

- (SurveyResponse *)responseFromResultSet:(FMResultSet *)rs;
- (void)insertSurveyResponse:(SurveyResponse *)response withDb:(FMDatabase *)db;
- (void)updateSurveyResponse:(SurveyResponse *)response withDb:(FMDatabase *)db;
- (void)delete:(SurveyResponse *)surveyResponse;
- (Answer *)answerFromResultSet:(FMResultSet *)rs;
- (NSArray *)answersForSurveyResponse:(SurveyResponse *)response withDb:(FMDatabase *)db;
- (void)insertOrUpdateAnswer:(Answer *)answer withDb:(FMDatabase *)db;

@end

@implementation SurveyResponseRepository

#pragma mark -
#pragma mark Methods

- (SurveyResponse *)responseFromResultSet:(FMResultSet *)rs {
    SurveyResponse *response = [[SurveyResponse alloc] init];
    response.surveyResponseId = [rs intForColumn:@"id"];
    response.surveyId = [rs intForColumn:@"surveyId"];
    response.progressPercentage = [rs doubleForColumn:@"progressPercentage"];
    response.readyForSubmission = [rs boolForColumn:@"readyForSubmission"];
    
    return [response autorelease];
}

- (SurveyResponse *)surveyResponseForSurveyId:(NSInteger)surveyId {    
    FMDatabase *db = [self db];
    if(![db open]) {
        LOGLINE(@"Couldn't open sqlite database.");
        exit(1);
    }
	
    FMResultSet *rs = [db executeQuery:@"SELECT * FROM survey_responses WHERE surveyId = ? AND readyForSubmission = ? LIMIT 1", 
                       [NSNumber numberWithInt:surveyId],
					   [NSNumber numberWithBool:NO]];
    
    SurveyResponse *response = nil;
    if([rs next]) {
		LOGLINE(@"Processing row");
        response = [self responseFromResultSet:rs];
		response.answers = [self answersForSurveyResponse:response withDb:db];
    }
	
	[rs close];
    [db close];
    
    return response;
}

- (void)save:(SurveyResponse *)response {
	
    FMDatabase *db = [self db];
	LOGLINE(@"Opening database");
    if(![db open]) {
		LOGLINE(@"Error opening sqlite database!");
		exit(1);
	}
	
	LOGLINE(@"Beginning transaction...");
    [db beginTransaction];
    
    @try 
	{
        if(response.surveyResponseId == 0) {
			LOGLINE(@"Inserting survey response");
            [self insertSurveyResponse:response withDb:db];
        } else {
			LOGLINE(@"Updating response");
            [self updateSurveyResponse:response withDb:db];
        }
        
		LOGLINE(@"committing transaction");
        [db commit];
		[[NSNotificationCenter defaultCenter] postNotificationName:SURVEY_RESPONSE_SAVED object:response];
    }
    @catch (id exception) {
		LOGLINE(@"Rolling back, due to error!");
        [db rollback];
        @throw;
    }
    @finally {
		LOGLINE(@"Closing database.");
        [db close];
    }
}

- (NSArray *)allResponses {
	FMDatabase *db = [self db];
    if(![db open]) {
		LOGLINE(@"Error opening sqlite database!");
		exit(1);
	}
	
	@try {
		NSMutableArray *responses = [NSMutableArray array];
		FMResultSet *rs = [db executeQuery:@"SELECT * FROM survey_responses"];
		while([rs next]) {
			SurveyResponse *response = [self responseFromResultSet:rs];
			response.answers = [self answersForSurveyResponse:response withDb:db];
			[responses addObject:response];
		}
		
		[rs close];
		return responses;
	}
	@catch (NSException * e) {
		LOGLINE(@"ERROR fetching responses! %@", e);
		exit(1);
	}
	@finally {
		[db close];
	}
}

- (NSArray *)responsesReadyForSubmission {
	NSMutableArray *readyResponses = [NSMutableArray array];
	for(SurveyResponse *response in [self allResponses]) {
		if(response.readyForSubmission) {
			[readyResponses addObject:response];
		}
	}
	
	return readyResponses;
}

#pragma mark -
#pragma mark Private Methos

- (void)insertSurveyResponse:(SurveyResponse *)response withDb:(FMDatabase *)db {
    [db executeUpdate:@"INSERT INTO survey_responses(surveyId, progressPercentage, readyForSubmission) VALUES(?, ?, ?)", 
     [NSNumber numberWithInt:response.surveyId],
     [NSNumber numberWithFloat:response.progressPercentage],
     [NSNumber numberWithBool:response.readyForSubmission]];
    
    response.surveyResponseId = [db lastInsertRowId];
    
    for(Answer *answer in response.answers) {
        answer.surveyResponseId = response.surveyResponseId;
        [self insertOrUpdateAnswer:answer withDb:db];
    }
}

- (void)updateSurveyResponse:(SurveyResponse *)response withDb:(FMDatabase *)db {
    [db executeUpdate:@"UPDATE survey_responses SET progressPercentage=?, readyForSubmission=? WHERE id=?", 
     [NSNumber numberWithFloat:response.progressPercentage],
     [NSNumber numberWithBool:response.readyForSubmission],
	 [NSNumber numberWithInt:response.surveyResponseId]];
	
    for(Answer *answer in response.answers) {
		answer.surveyResponseId = response.surveyResponseId;
        [self insertOrUpdateAnswer:answer withDb:db];
    }
}

- (void)delete:(SurveyResponse *)surveyResponse {
	FMDatabase *db = [self db];
	LOGLINE(@"Opening database");
    if(![db open]) {
		LOGLINE(@"Error opening sqlite database!");
		exit(1);
	}
	LOGLINE(@"Beginning transaction...");
    [db beginTransaction];
    @try 
	{
		[db executeUpdate:@"DELETE FROM survey_response_answers WHERE surveyResponse_id = ?", [NSNumber numberWithInt:surveyResponse.surveyResponseId]];
		[db executeUpdate:@"DELETE FROM survey_responses WHERE id = ?", [NSNumber numberWithInt:surveyResponse.surveyResponseId]];
		LOGLINE(@"committing transaction");
        [db commit];
	}
    @catch (id exception) {
		LOGLINE(@"Rolling back, due to error!");
        [db rollback];
        @throw;
    }
    @finally {
		LOGLINE(@"Closing database.");
        [db close];
    }
}

- (Answer *)answerFromResultSet:(FMResultSet *)rs {
    Answer *a = [[Answer alloc] init];
    a.answerId =            [rs intForColumn:@"id"];
    a.surveyResponseId =    [rs intForColumn:@"surveyResponse_Id"];
    a.localFileUrl =        [rs stringForColumn:@"localFileUrl"];
    a.answerText =          [rs stringForColumn:@"answer"];
    a.questionIndex =       [rs intForColumn:@"questionIndex"];
    
    return [a autorelease];
}

- (NSArray *)answersForSurveyResponse:(SurveyResponse *)response withDb:(FMDatabase *)db {
    FMResultSet *rs = [db executeQuery:@"SELECT * FROM survey_response_answers WHERE surveyResponse_id = ? ORDER BY questionIndex",
					   [NSNumber numberWithInt:response.surveyResponseId]];
	
    NSMutableArray *answers = [NSMutableArray array];
    while([rs next]) {
        Answer *answer = [self answerFromResultSet:rs];
        [answers addObject:answer];
    }
    [rs close];
    return answers;
}


- (void)insertOrUpdateAnswer:(Answer *)answer withDb:(FMDatabase *)db {
	NSAssert(answer.answerText != nil, @"AnswerText (%d) shouldn't be nil!", answer.questionIndex);
	
    int count = [db intForQuery:@"SELECT COUNT(*) from survey_response_answers WHERE surveyResponse_id=? and questionIndex=?", 
                 [NSNumber numberWithInt:answer.surveyResponseId],
                 [NSNumber numberWithInt:answer.questionIndex]];
    
	
	id localFileValue = answer.localFileUrl == nil ? (id)[NSNull null] : answer.localFileUrl;
	
    if(count == 0) {
        //insert
		LOGLINE(@"Inserting answer");
        [db executeUpdate:@"INSERT INTO survey_response_answers(surveyResponse_id, localFileUrl, answer, questionIndex) VALUES(?, ?, ?, ?)",
         [NSNumber numberWithInt:answer.surveyResponseId],
         localFileValue,
         answer.answerText,
         [NSNumber numberWithInt:answer.questionIndex]];
        answer.answerId = [db lastInsertRowId];
    } else {
        //update
		LOGLINE(@"Updating answer");
        [db executeUpdate:@"UPDATE survey_response_answers SET localFileUrl=?, answer=? WHERE surveyResponse_id=? AND questionIndex=?",
         localFileValue,
         answer.answerText,
         [NSNumber numberWithInt:answer.surveyResponseId],
         [NSNumber numberWithInt:answer.questionIndex]];
    }
}

@end
