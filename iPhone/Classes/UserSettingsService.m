//
//  AuthenticationService.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/31/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "UserSettingsService.h"
#import "FileHelpers.h"
#import "UserSettings.h"

#define PASSWORD_KEY		@"password"
#define USERNAME_KEY		@"username"
#define LOCATION_KEY		@"location"
#define NOTIFICATION_KEY	@"notification"
#define LASTSYNC_KEY		@"lastsync"

@interface UserSettingsService(Private)

- (BOOL)isLoggedIn;
- (void)signInFinished;
- (void)saveInfo:(UserSettings *)settings;
- (UserSettings *)readInfo;

@end

@implementation UserSettingsService

#pragma mark -
#pragma mark Dealloc
- (void) dealloc {
	delegate = nil;
	
	[super dealloc];
}


#pragma mark -
#pragma mark Properties

@synthesize delegate;

#pragma mark -
#pragma mark Methods

- (void)registerWithUsername:(NSString *)username password:(NSString *)password notifications:(BOOL)notifications sendLocation:(BOOL)sendLocation {
	UserSettings *user = [[UserSettings alloc] initWithUsername:username password:password captureLocation:notifications pushNotifications:sendLocation];
	[self saveInfo:user];
	[user release];
	[self signInFinished];
}

#pragma mark -
#pragma mark Class Methods

+ (BOOL)isLoggedIn {
    return [[[[UserSettingsService alloc] init] autorelease] isLoggedIn];
}

+ (UserSettings *)retrieve {
	return [[[[UserSettingsService alloc] init] autorelease] readInfo];
}

+ (void)save:(UserSettings *)user {
	[[[[UserSettingsService alloc] init] autorelease] saveInfo:user];
}

#pragma mark -
#pragma mark Private Methods

- (BOOL)isLoggedIn {
    return [self readInfo] != nil;
}

- (void)saveInfo:(UserSettings *)settings {
	NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
	[defaults setObject:settings.username forKey:USERNAME_KEY];
	[defaults setObject:settings.password forKey:PASSWORD_KEY];
	[defaults setObject:[NSNumber numberWithBool:settings.pushNotifications] forKey:NOTIFICATION_KEY];
	[defaults setObject:[NSNumber numberWithBool:settings.captureLocation] forKey:LOCATION_KEY];
	if (settings.lastSyncDate) {
		[defaults setObject:settings.lastSyncDate forKey:LASTSYNC_KEY];
	}
}

- (UserSettings *)readInfo {
	NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
	NSNumber *locationNumber = [defaults objectForKey:NOTIFICATION_KEY];
	NSNumber *notificationNumber = [defaults objectForKey:LOCATION_KEY];
	UserSettings *user = [[UserSettings alloc] initWithUsername:[defaults objectForKey:USERNAME_KEY] 
									   password:[defaults objectForKey:PASSWORD_KEY] 
								captureLocation:[locationNumber boolValue]
							  pushNotifications:[notificationNumber boolValue]];
	NSDate *lastSync = [defaults objectForKey:LASTSYNC_KEY];
	if (lastSync) {
		user.lastSyncDate = lastSync;
	}
	return [user autorelease];
}

- (void)signInFinished {
    [self.delegate signInCompletedWithSuccess];
}

@end
