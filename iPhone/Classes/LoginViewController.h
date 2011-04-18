//
//  LoginViewController.h
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/31/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "UserSettingsService.h"


@protocol LoginViewControllerDelegate;

@interface LoginViewController : UIViewController <UITextFieldDelegate, UserSettingsServiceDelegate> {
	UITextField *userName;
	UITextField *password;
	UISwitch *locationSwitch;
	UISwitch *notificationSwitch;
    UIActivityIndicatorView *indicatorView;
	id<LoginViewControllerDelegate> delegate;
}

@property(nonatomic, retain) IBOutlet UITextField *userName;
@property(nonatomic, retain) IBOutlet UITextField *password;
@property(nonatomic, retain) IBOutlet UISwitch *locationSwitch;
@property(nonatomic, retain) IBOutlet UISwitch *notificationSwitch;
@property(nonatomic, retain) IBOutlet UIActivityIndicatorView *indicatorView;
@property(nonatomic, assign) id<LoginViewControllerDelegate> delegate;

- (IBAction)saveButtonTapped:(id)sender;
@end


@protocol LoginViewControllerDelegate <NSObject>

- (void)didAuthenticate;

@end