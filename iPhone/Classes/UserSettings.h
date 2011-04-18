//
//  User.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/3/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface UserSettings : NSObject {
	NSString *username;
	NSString *password;
	BOOL captureLocation;
	BOOL pushNotifications;
	NSDate *lastSyncDate;
}

@property (nonatomic, copy) NSString *username;
@property (nonatomic, copy) NSString *password;
@property (nonatomic, assign) BOOL captureLocation;
@property (nonatomic, assign) BOOL pushNotifications;
@property (nonatomic, retain) NSDate *lastSyncDate;

- (id)initWithUsername:(NSString*)anUsername password:(NSString*)aPassword captureLocation:(BOOL)aCaptureLocation pushNotifications:(BOOL)aPushNotifications;

@end
