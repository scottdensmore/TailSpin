//
//  SurveyListingController.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "SurveyListingService.h"
#import "ASIHTTPRequest.h"
#import "SyncService.h"
#import "LoginViewController.h"
#import "SelectFiltersViewController.h"

@interface SurveyListingController : UIViewController <SelectFiltersViewControllerDelegate, 
														LoginViewControllerDelegate, 
														SyncServiceDelegate, 
														ASIHTTPRequestDelegate, 
														UITableViewDelegate, 
														UITableViewDataSource> {
	NSArray *surveyListings;
    SyncService *syncService;
    NSMutableDictionary *icons;
    NSMutableDictionary *iconRequests;
	UITableView *tableView;
	UISegmentedControl *filterSegments;
}

@property (nonatomic, retain) NSArray *surveyListings;
@property(nonatomic, retain) IBOutlet UITableView *tableView;
@property (nonatomic, retain) IBOutlet UISegmentedControl *filterSegments;

- (IBAction)sync:(id)sender;
- (IBAction)openSettings:(id)sender;
- (IBAction)openFilters:(id)sender;
- (IBAction)onFilterChanged:(id)sender;
@end
