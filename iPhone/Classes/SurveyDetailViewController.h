//
//  SurveyDetailViewController.h
//  tailspin-iphone
//
//  Created by Kevin Lee on 9/22/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "SurveyListing.h"

@interface SurveyDetailViewController : UIViewController {
	SurveyListing *viewModel;
}

@property (nonatomic, retain) SurveyListing *viewModel;

-(id)initWithSurveyListing:(SurveyListing *)surveyListing;
-(IBAction)toggleFavorite:(id)sender;
@end
