//
//  RepositoryBase.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/10/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "RepositoryBase.h"
#import "AppDelegate.h"
#import "FMDatabase.h"
#import "FMDatabaseAdditions.h"

@implementation RepositoryBase

#pragma mark -
#pragma mark Methods

-(FMDatabase *)db {
    NSString *databasePath = [AppDelegate databasePath];
	FMDatabase *db = [[[FMDatabase alloc] initWithPath:databasePath] autorelease];
	[db setBusyRetryTimeout:50];
	
#ifdef DEBUG
	//[db setTraceExecution:YES];
	[db setLogsErrors:YES];
#endif
	return db;
}

@end
