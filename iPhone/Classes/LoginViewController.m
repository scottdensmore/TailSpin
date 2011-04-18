//
//  LoginViewController.m
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/31/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "LoginViewController.h"
#import "SurveyListingController.h"
#import "AlertHelper.h"
#import "UserSettings.h"

const int UserNameTag = 0;
const int PasswordTag = 1;

@interface LoginViewController(Private)

- (void)setupSaveButton;
- (void)setupIndicatorButton;

@end

@implementation LoginViewController

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(userName);
	RELEASE(password);
	RELEASE(indicatorView);
	RELEASE(notificationSwitch);
	RELEASE(locationSwitch);
	delegate = nil;
	
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize userName, password, indicatorView, delegate, notificationSwitch, locationSwitch;

#pragma mark -
#pragma mark View Methods

- (void)viewDidLoad {
    [super viewDidLoad];
	self.navigationItem.title = @"Settings";
	self.navigationItem.leftBarButtonItem =  [[UIBarButtonItem alloc] initWithBarButtonSystemItem: UIBarButtonSystemItemSave 
																						   target:self 
																						   action:@selector(saveButtonTapped:)];
	UserSettings *userSettings = [UserSettingsService retrieve];
	self.password.text = userSettings.password;
	self.userName.text	= userSettings.username;
	self.locationSwitch.on = userSettings.captureLocation;
	self.notificationSwitch.on = userSettings.pushNotifications;
    self.indicatorView.hidden = YES;
}

- (void)didReceiveMemoryWarning {
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
    
    // Release any cached data, images, etc that aren't in use.
}

- (void)viewDidUnload {
    [super viewDidUnload];
    //self.indicatorView = nil;
	self.userName = nil;
	self.password = nil;
	self.locationSwitch = nil;
	self.notificationSwitch = nil;
}

#pragma mark -
#pragma mark Private Methods
- (void)setupSaveButton {
	self.navigationItem.leftBarButtonItem =  [[UIBarButtonItem alloc] initWithBarButtonSystemItem: UIBarButtonSystemItemSave 
																						   target:self 
																						   action:@selector(saveButtonTapped:)];
}

- (void)setupIndicatorButton {
	if (!self.indicatorView)
		self.indicatorView = [[UIActivityIndicatorView alloc] initWithFrame:CGRectMake(0, 0, 20, 20)];
    UIBarButtonItem *barButton = [[UIBarButtonItem alloc] initWithCustomView:self.indicatorView];
	[self navigationItem].leftBarButtonItem = barButton;
}

#pragma mark -
#pragma mark Actions

- (IBAction)saveButtonTapped:(id)sender {
	[self setupIndicatorButton];
	[self navigationItem].leftBarButtonItem.enabled = NO;
	[self.indicatorView startAnimating];
    UserSettingsService *svc = [[UserSettingsService alloc] init];
    svc.delegate = self;
    [svc registerWithUsername:userName.text password:password.text
				notifications:notificationSwitch.on sendLocation:locationSwitch.on];
    [svc release];
}

#pragma mark -
#pragma mark AuthenticationServiceDelegate Methods

- (void)signInCompletedWithSuccess {    
    [self.indicatorView stopAnimating];
	[self.delegate didAuthenticate];		
}

-(void)signInFailedWithError:(NSString *)errorMessage {
	[self.indicatorView stopAnimating];
	[self setupSaveButton];
    ShowAlert(@"Registration Error", errorMessage);
}

#pragma mark -
#pragma mark UITextFieldDelegate Methods

- (BOOL)textFieldShouldReturn:(UITextField *)textField{
	if (textField.tag == UserNameTag) {
		[password becomeFirstResponder];
	} else {
		[textField resignFirstResponder];
	}
	return NO;
}

@end
