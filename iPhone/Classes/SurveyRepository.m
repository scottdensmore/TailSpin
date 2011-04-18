//
//  SurveyRepository.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import "SurveyRepository.h"
#import "Survey.h"
#import "FMDatabase.h"
#import "FMDatabaseAdditions.h"
#import "Question.h"

@interface SurveyRepository(Private)

- (BOOL)surveyExists:(Survey *)survey withDb:(FMDatabase *)db;
- (Survey *)surveyFromResultSet:(FMResultSet *)rs;
- (void)insertSurvey:(Survey *)survey withDb:(FMDatabase *)db;
- (void)updateSurvey:(Survey *)survey withDb:(FMDatabase *)db;
- (Question *)questionFromResultSet:(FMResultSet *)rs;
- (NSArray *)questionsForSurvey:(Survey *)survey withDb:(FMDatabase *)db;
- (void)insertQuestion:(Question *)question withDb:(FMDatabase *)db;
- (void)updateQuestion:(Question *)question withDb:(FMDatabase *)db;

@end

@implementation SurveyRepository

#pragma mark -
#pragma mark Methods

- (NSArray *)surveys {
    NSString *sql = @"SELECT * FROM surveys";
    FMDatabase *db = [self db];
    [db open];
    FMResultSet *resultSet = [db executeQuery:sql];
    
    NSMutableArray *surveys = [NSMutableArray array];
    while([resultSet next]) {
        Survey *survey = [self surveyFromResultSet:resultSet];
        int count = [db intForQuery:@"SELECT COUNT(*) FROM survey_responses WHERE surveyId = ? AND readyForSubmission = ?", [NSNumber numberWithInt:survey.surveyId], [NSNumber numberWithBool:YES]];
		survey.numberReadyToSubmit = count;
        FMResultSet *questionResultSet = [db executeQuery:@"SELECT * FROM questions WHERE survey_id = ?", [NSNumber numberWithInt:survey.surveyId]];
        NSMutableArray *questions = [NSMutableArray array];
        while([questionResultSet next]){
            Question *q = [self questionFromResultSet:questionResultSet];
            [questions addObject:q];
        }
        
        survey.questions = questions;
        
        [surveys addObject:survey];
    }
    
    [db close];
    
    return surveys;
}

- (Survey *)surveyWithId:(NSInteger)surveyId {
	NSString *sql = @"SELECT * FROM surveys WHERE id = ? LIMIT 1";
    FMDatabase *db = [self db];
    [db open];
    FMResultSet *rs = [db executeQuery:sql, [NSNumber numberWithInt:surveyId]];
    
	Survey *s = nil;
	if([rs next]) {
		s = [self surveyFromResultSet:rs];
	}
    [rs close];
    
    if(s == nil) {
        [db close];
        return nil;
    }
    
    s.questions = [self questionsForSurvey:s withDb:db];
	
	[db close];
	return s;
}

- (void)save:(Survey *)survey {
    FMDatabase *db = [self db];
    [db open];
    //[db beginTransaction];
    {   
        if(![self surveyExists:survey withDb:db]) {
            [self insertSurvey:survey withDb:db];
            for(Question *question in survey.questions) {
                question.surveyId = survey.surveyId;                
                [self insertQuestion:question withDb:db];
            }
        } else {
            [self updateSurvey:survey withDb:db];
        }
    }
    
    if([db hadError]) {
        //[db rollback];
        [db close];
		[NSException raise:@"Database Error" format:@"Error: %@", [db lastErrorMessage]];
    } else {
        //[db commit];        
    }
    
    [db close];
}

#pragma mark -
#pragma mark Private Methods

- (BOOL)surveyExists:(Survey *)survey withDb:(FMDatabase *)db {
    return [db intForQuery:@"SELECT COUNT(*) FROM surveys WHERE slug=? AND tenant=?", survey.slug, survey.tenant] > 0;
}

- (Survey *)surveyFromResultSet:(FMResultSet *)rs {
    Survey *s = [[Survey alloc] init];
    s.surveyId = [rs intForColumn:@"id"];
    s.title = [rs stringForColumn:@"title"];
    s.description = [rs stringForColumn:@"description"];
    s.durationInMinutes = [rs intForColumn:@"durationInMinutes"];
    s.tenant = [rs stringForColumn:@"tenant"];
	s.imageUrl = [rs stringForColumn:@"imageUrl"];
    s.slug = [rs stringForColumn:@"slug"];
	s.isFavorite = [rs boolForColumn:@"isFavorite"];

    
    return [s autorelease];
}

- (void)insertSurvey:(Survey *)survey withDb:(FMDatabase *)db {
    NSString *sql = @"INSERT INTO surveys(title, tenant, imageUrl, durationInMinutes, slug, isFavorite) VALUES(?, ?, ?, ?, ?, ?);";
    [db executeUpdate:sql, 
	 survey.title,
	 survey.tenant,
	 survey.imageUrl,
	 survey.durationInMinutes,
	 survey.slug,
	 [NSNumber numberWithBool:survey.isFavorite]];
    
    survey.surveyId = [db lastInsertRowId];
}

- (void)updateSurvey:(Survey *)survey withDb:(FMDatabase *)db {
    NSString *sql = @"UPDATE surveys SET title=?, tenant=?, imageUrl=?, durationInMinutes=?, slug=?, isFavorite=? WHERE id=?";
    [db executeUpdate:sql,
	 survey.title,
	 survey.tenant,
	 survey.imageUrl,
	 [NSNumber numberWithInt:survey.durationInMinutes],
	 survey.slug,
	 [NSNumber numberWithBool:survey.isFavorite],
	 [NSNumber numberWithInt:survey.surveyId], nil];
}

- (Question *)questionFromResultSet:(FMResultSet *)rs {
    Question *q = [[Question alloc] init];
    q.questionId = [rs intForColumn:@"id"];
    q.surveyId = [rs intForColumn:@"survey_id"];
    q.text = [rs stringForColumn:@"text"];
    q.questionTypeString = [rs stringForColumn:@"questionType"];
    q.possibleAnswersString = [rs stringForColumn:@"possibleAnswers"];
    
    return [q autorelease];
}


- (NSArray *)questionsForSurvey:(Survey *)survey withDb:(FMDatabase *)db {
    FMResultSet *rs = [db executeQuery:@"SELECT * FROM questions WHERE survey_id = ? ORDER BY id", 
                       [NSNumber numberWithInt:survey.surveyId]];
    
	NSMutableArray *questions = [NSMutableArray array];
	while([rs next]) {
		Question * q = [self questionFromResultSet:rs];
		q.tenant = survey.tenant;
		q.slug = survey.slug;
		[questions addObject:q];
	}
	[rs close];
    return questions;
}

- (void)insertQuestion:(Question *)question withDb:(FMDatabase *)db {
    NSString *sql = @"INSERT INTO questions(survey_id, questionType, text, possibleAnswers) VALUES(?, ?, ?, ?);";
    [db executeUpdate:sql,
        [NSNumber numberWithInt:question.surveyId],
        question.questionTypeString,
        question.text,
		question.possibleAnswersString, nil];

		question.questionId = [db lastInsertRowId];
}

- (void)updateQuestion:(Question *)question withDb:(FMDatabase *)db {
    NSString *sql = @"UPDATE questions SET questionType=?, text=?, possibleAnswers=? WHERE id=?";
    [db executeUpdate:sql,
        question.questionType,
        question.text,
        question.possibleAnswersString, 
        [NSNumber numberWithInt:question.questionId],nil];
}

@end
