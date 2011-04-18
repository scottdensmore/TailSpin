//
//  SurveyResponseViewController.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/19/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import "SurveyResponseViewController.h"
#import <QuartzCore/QuartzCore.h>
#import "SurveyResponseService.h"
#import "QuestionView.h"

@interface SurveyResponseViewController(Private)

- (void)updateButtonStatus;
- (void)animateScrollOffset:(CGFloat)deltaX deltaY:(CGFloat)deltaY;

@end

@implementation SurveyResponseViewController

#pragma mark -
#pragma mark Init

-(id)initWithSurveyId:(NSInteger)theSurveyId {
	if((self = [super initWithNibName:@"SurveyResponseViewController" bundle:nil])) {
		surveyId = theSurveyId;
	}	
	return self;
}

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
    viewModel.delegate = nil;
    RELEASE(viewModel);
	RELEASE(questionViews);
	RELEASE(scrollView);
	RELEASE(toolbar);
	RELEASE(prevButton);
	RELEASE(nextButton);
	RELEASE(submitButton);
    
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize viewModel, toolbar, nextButton, submitButton, prevButton, questionViews;


#pragma mark -
#pragma mark View Methods

- (void)viewDidLoad {
    [super viewDidLoad];
	
	self.viewModel = [SurveyResponseService viewModelForSurveyId:surveyId];
	self.viewModel.delegate = self;
  
    self.title = viewModel.surveyTitle;
    scrollView = [[UIScrollView alloc] initWithFrame:self.view.bounds];
    scrollView.contentSize = CGSizeMake(self.view.frame.size.width * viewModel.questions.count, self.view.frame.size.height - self.navigationController.navigationBar.frame.size.height);
    scrollView.pagingEnabled = YES;
	scrollView.delegate = self;
    scrollView.backgroundColor = [UIColor darkGrayColor];
    
    CGFloat x = 0;
    NSInteger index = 0;
	NSMutableArray *qvs = [NSMutableArray array];
	width = self.view.frame.size.width;
    for(Question *q in viewModel.questions) {
        NSInteger questionIndex = [viewModel.questions indexOfObject:q];
        QuestionView *questionView = [self.viewModel viewForQuestion:q atIndex:questionIndex withTotalQuestions:viewModel.questions.count];
		//self.view.autoresizesSubviews = YES;
		questionView.layer.borderWidth = 2;
        questionView.layer.borderColor = [[UIColor darkGrayColor] CGColor];
        questionView.frame = CGRectMake(x, 0, self.view.frame.size.width, scrollView.contentSize.height - toolbar.frame.size.height);
        questionView.backgroundColor = [UIColor whiteColor];
		[qvs addObject:questionView];
		[scrollView addSubview:questionView];
        
		x += self.view.frame.size.width;
        index++;
    }
	
	self.questionViews = [qvs retain];
	currentIndex = 0;
	[self updateButtonStatus];
	[self.view insertSubview:scrollView belowSubview:toolbar];
	//[scrollView release];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
}

- (void)viewDidUnload {
    [super viewDidUnload];
	[scrollView release];
	self.toolbar = nil;
	self.nextButton = nil;
	self.prevButton = nil;
	self.submitButton = nil;
}

#pragma mark -
#pragma mark Toolbar Button Actions

- (IBAction)onNextTapped:(id)sender {
	currentIndex ++;
	[self updateButtonStatus];
    [self animateScrollOffset:self.view.frame.size.width deltaY:0];
}

- (IBAction)onPrevTapped:(id)sender {
	currentIndex--;
	[self updateButtonStatus];
    [self animateScrollOffset:self.view.frame.size.width * -1 deltaY:0];
}

- (IBAction)onSubmitTapped:(id)sender {
	[SurveyResponseService submitSurvey:self.viewModel];
	//pop back to listing page
	[self.navigationController popViewControllerAnimated:YES];
}

#pragma mark -
#pragma Private Methods

- (void)updateButtonStatus {
	prevButton.enabled = (currentIndex > 0) ? YES : NO;
	nextButton.enabled = (currentIndex < ([questionViews count] - 1)) ? YES : NO;
	submitButton.enabled = viewModel.surveyCompleted;
}

- (void)animateScrollOffset:(CGFloat)deltaX deltaY:(CGFloat)deltaY {
    [UIView beginAnimations:@"page" context:nil];
    scrollView.contentOffset = CGPointMake(scrollView.contentOffset.x + deltaX, deltaY);
    [UIView commitAnimations];
}

#pragma mark -
#pragma mark SurveyResponseDelegate Methods

- (void)surveyResponseViewModelDidChange:(SurveyResponseViewModel *)vm {
    [SurveyResponseService persistResponse:vm];
	//submitButton.enabled = vm.surveyCompleted;
	[self updateButtonStatus];
}

#pragma mark -
#pragma mark UIScrollViewDelegate Methods
- (void)scrollViewDidEndDecelerating:(UIScrollView *)sv {
	FTLOGCALL;
	CGPoint point = [sv contentOffset];
	if (point.x == 0) {
		currentIndex = 0;
	} else {
		currentIndex = point.x / width;
	}
	[self updateButtonStatus];
}

@end
