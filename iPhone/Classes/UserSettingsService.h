//
//  AuthenticationService.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/31/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol UserSettingsServiceDelegate;

@class UserSettings;

@interface UserSettingsService : NSObject {
    id<UserSettingsServiceDelegate> delegate;
}

@property (nonatomic, assign) id<UserSettingsServiceDelegate> delegate;

- (void)registerWithUsername:(NSString *)username password:(NSString *)password notifications:(BOOL)notifications sendLocation:(BOOL)sendLocation; 

+ (BOOL)isLoggedIn;
+ (UserSettings *)retrieve;
+ (void)save:(UserSettings *)user;

@end


@protocol UserSettingsServiceDelegate <NSObject>

- (void)signInCompletedWithSuccess;

- (void)signInFailedWithError:(NSString *)errorMessage;

@end
