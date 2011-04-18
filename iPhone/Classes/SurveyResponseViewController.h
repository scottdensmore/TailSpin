//
//  SurveyResponseViewController.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/19/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "SurveyResponseViewModel.h"

@interface SurveyResponseViewController : UIViewController <SurveyResponseDelegate, UIScrollViewDelegate> {
	SurveyResponseViewModel *viewModel;
	NSArray *questionViews;
	UIScrollView *scrollView; 
	UIToolbar *toolbar;
	UIBarButtonItem *nextButton;
    UIBarButtonItem *prevButton;
	UIBarButtonItem *submitButton;
	NSInteger surveyId;
	NSInteger currentIndex;
	CGFloat width;
}

@property (nonatomic, retain) SurveyResponseViewModel *viewModel;
@property (nonatomic, retain) NSArray *questionViews;
@property (nonatomic, retain) IBOutlet UIToolbar *toolbar;
@property (nonatomic, retain) IBOutlet UIBarButtonItem *nextButton;
@property (nonatomic, retain) IBOutlet UIBarButtonItem *prevButton;
@property (nonatomic, retain) IBOutlet UIBarButtonItem *submitButton;

- (IBAction)onNextTapped:(id)sender;
- (IBAction)onPrevTapped:(id)sender;
- (IBAction)onSubmitTapped:(id)sender;

-(id)initWithSurveyId:(NSInteger)surveyId;

@end
