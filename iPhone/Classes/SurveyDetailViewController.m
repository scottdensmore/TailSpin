//
//  SurveyDetailViewController.m
//  tailspin-iphone
//
//  Created by Kevin Lee on 9/22/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "SurveyDetailViewController.h"
#import "SurveyDetailView.h"
#import "SurveyService.h"

@implementation SurveyDetailViewController

#pragma mark -
#pragma mark Init

-(id)initWithSurveyListing:(SurveyListing *)surveyListing{
	if((self = [super initWithNibName:@"SurveyDetailViewController" bundle:nil])) {
		self.viewModel = surveyListing;
	}
	
	return self;
}

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(viewModel);
	
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize viewModel;

#pragma mark -
#pragma mark View Methods

- (void)viewDidLoad {
    [super viewDidLoad];
	
	SurveyDetailView *aView = (SurveyDetailView *)self.view;
	aView.titleLabel.text = self.viewModel.title;
	aView.durationLabel.text = [NSString stringWithFormat:@"Estimated Length: %d minutes", self.viewModel.durationInMinutes];
	aView.progressView.progress = self.viewModel.percentComplete;
	aView.favoriteSwitch.on = self.viewModel.isFavorite;
}

- (void)didReceiveMemoryWarning {
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
}

- (void)viewDidUnload {
    [super viewDidUnload];
}

#pragma mark -
#pragma mark Actions

-(IBAction)toggleFavorite:(id)sender {	
	[SurveyService surveyIsFavorite:[sender isOn] forSurveyId:self.viewModel.surveyId];
}

@end
