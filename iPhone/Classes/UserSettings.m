//
//  User.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/3/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "UserSettings.h"

@implementation UserSettings

#pragma mark -
#pragma mark Init

- (id)init {
    return [self initWithUsername:@"" password:@"" captureLocation:NO pushNotifications:NO];
}


- (id)initWithUsername:(NSString*)anUsername password:(NSString*)aPassword captureLocation:(BOOL)aCaptureLocation pushNotifications:(BOOL)aPushNotifications {
    self = [super init];
    if (self) {
        self.username = anUsername;
        self.password = aPassword;
		self.captureLocation = aCaptureLocation;
		self.pushNotifications = aPushNotifications;
    }
    return self;
}

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(username);
	RELEASE(password);
	RELEASE(lastSyncDate);
	
	[super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize username, password, captureLocation, pushNotifications, lastSyncDate;

@end
