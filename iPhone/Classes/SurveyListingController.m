//
//  SurveyListingController.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import "SurveyListingController.h"
#import "CellMenuItem.h"
#import "SelectFiltersViewController.h"
#import "SurveyCell.h"
#import "SurveyDetailViewController.h"
#import "SurveyResponseService.h"
#import "SurveyResponseViewController.h"
#import "SurveyService.h"
#import "UserSettings.h"
#import "AlertHelper.h"

const CGFloat ROW_HEIGHT = 80;

@interface SurveyListingController(Private)

- (void)showSurveyListings;
- (void)promptForLogin;
- (void)handleLongPress:(UILongPressGestureRecognizer*)longPressRecognizer;
- (void)favoriteMenuButtonPressed:(UIMenuController*)menuController;
- (void)detailsMenuButtonPressed:(UIMenuController*)menuController;

@end

@implementation SurveyListingController

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(surveyListings);
	RELEASE(syncService);
	RELEASE(icons);
	RELEASE(iconRequests);
	RELEASE(tableView);
	RELEASE(filterSegments);
	
	[super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize surveyListings, tableView, filterSegments;

#pragma mark -
#pragma mark View Methods

- (void)viewDidLoad {
    [super viewDidLoad];
    
    if(!icons) {
        icons = [[NSMutableDictionary alloc] init];
        iconRequests = [[NSMutableDictionary alloc] init];
    }
    
    if([UserSettingsService isLoggedIn]) {
        [self showSurveyListings];
    } else {
        [self promptForLogin];
    }
	
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(showSurveyListings)
												 name:SURVEY_RESPONSE_SAVED
											   object:nil];
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation {
	return NO;
}

- (void)didReceiveMemoryWarning {
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
}

- (void)viewDidUnload {
	[super viewDidUnload];
	self.tableView = nil;
	self.filterSegments = nil;
	[[NSNotificationCenter defaultCenter] removeObserver:self
													name:SURVEY_RESPONSE_SAVED 
												  object:nil];
}

- (BOOL)canBecomeFirstResponder {
    return YES;
}

#pragma mark -
#pragma mark Actions

- (IBAction)sync:(id)sender {
    if(!syncService) {
        syncService = [[SyncService alloc] init];
        syncService.delegate = self;
    }
    
    [syncService syncSurveys];
}

- (IBAction)openFilters:(id)sender {
	SelectFiltersViewController *viewController = [[SelectFiltersViewController alloc] initWithNibName:@"SelectFiltersViewController" bundle:nil];
	viewController.delegate = self;
	UINavigationController *modalNavController = [[UINavigationController alloc] initWithRootViewController:viewController];
	[self presentModalViewController:modalNavController animated:YES];
	[modalNavController release];
	[viewController release];
}

- (IBAction)openSettings:(id)sender {
	[self promptForLogin];
}

- (IBAction)onFilterChanged:(id)sender {
	LOGLINE(@"the filter was changed");
	[self showSurveyListings];
}

- (void)unFavoriteMenuButtonPressed:(UIMenuController*)menuController {
	CellMenuItem *menuItem = [[[UIMenuController sharedMenuController] menuItems] objectAtIndex:0];
	if (menuItem.indexPath) {
		[self resignFirstResponder];
		SurveyListing *selectedSurvey = [self.surveyListings objectAtIndex:menuItem.indexPath.row];
		[SurveyService surveyIsFavorite:YES forSurveyId:selectedSurvey.surveyId];
		UITableViewCell *cell = [self.tableView cellForRowAtIndexPath:menuItem.indexPath];
		if ([cell isKindOfClass:[SurveyCell class]]){
			((SurveyCell *)cell).favoriteStarView.image= nil;
		}
		
	}
}

- (void)favoriteMenuButtonPressed:(UIMenuController*)menuController {
	CellMenuItem *menuItem = [[[UIMenuController sharedMenuController] menuItems] objectAtIndex:0];
	if (menuItem.indexPath) {
		[self resignFirstResponder];
		SurveyListing *selectedSurvey = [self.surveyListings objectAtIndex:menuItem.indexPath.row];
		[SurveyService surveyIsFavorite:YES forSurveyId:selectedSurvey.surveyId];
		UITableViewCell *cell = [self.tableView cellForRowAtIndexPath:menuItem.indexPath];
		if ([cell isKindOfClass:[SurveyCell class]]){
			((SurveyCell *)cell).favoriteStarView.image= [UIImage imageNamed:@"selected.png"];
		}
		
	}
}

- (void)detailsMenuButtonPressed:(UIMenuController*)menuController {
	CellMenuItem *menuItem = [[[UIMenuController sharedMenuController] menuItems] objectAtIndex:0];
	if (menuItem.indexPath) {
		[self resignFirstResponder];
		SurveyListing *selectedSurvey = [self.surveyListings objectAtIndex:menuItem.indexPath.row];
		SurveyDetailViewController *vc = [[SurveyDetailViewController alloc]  initWithSurveyListing:selectedSurvey];
		[self.navigationController pushViewController:vc animated:YES];
		[vc release];
	}
}

- (void)handleLongPress:(UILongPressGestureRecognizer*)longPressRecognizer {
    
    /*
     For the long press, the only state of interest is Began.
     When the long press is detected, find the index path of the row (if there is one) at press location.
     If there is a row at the location, create a suitable menu controller and display it.
     */
    if (longPressRecognizer.state == UIGestureRecognizerStateBegan) {
        
        NSIndexPath *pressedIndexPath = [self.tableView indexPathForRowAtPoint:[longPressRecognizer locationInView:self.tableView]];
        UITableViewCell *cell = [self.tableView cellForRowAtIndexPath:pressedIndexPath];
        if (pressedIndexPath && (pressedIndexPath.row != NSNotFound) && (pressedIndexPath.section != NSNotFound)) {
            [self becomeFirstResponder];
            UIMenuController *menuController = [UIMenuController sharedMenuController];
			NSMutableArray *menuItems = [NSMutableArray arrayWithCapacity:3];
			
			if (((SurveyCell *)cell).isFavorite == NO) {
				CellMenuItem *unFavoriteMenuItem = [[CellMenuItem alloc] initWithTitle:@"Unmark as Favorite"
																				action:@selector(unFavoriteMenuButtonPressed:)];
				unFavoriteMenuItem.indexPath = pressedIndexPath;
				[menuItems addObject:unFavoriteMenuItem];
				[unFavoriteMenuItem release];
			} else {
				CellMenuItem *favoriteMenuItem = [[CellMenuItem alloc] initWithTitle:@"Mark as Favorite"
																			  action:@selector(favoriteMenuButtonPressed:)];
				favoriteMenuItem.indexPath = pressedIndexPath;
				[menuItems addObject:favoriteMenuItem];
				[favoriteMenuItem release];
			}
            
			CellMenuItem *detailsMenuItem = [[CellMenuItem alloc] initWithTitle:@"Survey Details" action:@selector(detailsMenuButtonPressed:)];
			detailsMenuItem.indexPath = pressedIndexPath;
			[menuItems addObject:detailsMenuItem];
			[detailsMenuItem release];
			
			menuController.menuItems = [NSArray arrayWithArray:menuItems];
            [menuController setTargetRect:[self.tableView rectForRowAtIndexPath:pressedIndexPath] inView:self.tableView];
            [menuController setMenuVisible:YES animated:YES];
        }
    }
}

#pragma mark -
#pragma mark Methods

- (void)promptForLogin {
	
    LoginViewController *lvc = [[LoginViewController alloc] initWithNibName:@"LoginViewController" bundle:nil];
	lvc.delegate = self;
	UINavigationController *modalNavController = [[UINavigationController alloc] initWithRootViewController:lvc];
    [self presentModalViewController:modalNavController animated:YES];
    [lvc release];
	[modalNavController release];
    return;	
}

- (void)showSurveyListings {
	SurveyListingService *service = [[SurveyListingService alloc] init];
	
	switch(filterSegments.selectedSegmentIndex) {
		case 0:
			LOGLINE(@"case ALL");
			self.surveyListings = [service surveyListings];
			break;
		case 1:
			LOGLINE(@"case Favs");
			self.surveyListings = [service surveyListingsFavorites];
			break;
		case 2:
			LOGLINE(@"case Duration");
			self.surveyListings = [service surveyListingsOrderedByDuration];
			break;
		default:
			LOGLINE(@"case oops nothing to do");
			self.surveyListings = [service surveyListings];
			break;
	}
	
	[service release];	
	[self.tableView reloadData];
}
#pragma mark -
#pragma mark Class Methods

+(UIWindow *)window {
	AppDelegate *appDelegate = [[UIApplication sharedApplication] delegate];
	return appDelegate.window;
}

#pragma mark - 
#pragma mark SyncDelegate Methods

-(void)syncServiceDidComplete:(BOOL)wasUpdated {
    [self showSurveyListings];
}

-(void)syncServiceDidFailWithError:(NSString *)error {
    LOGLINE(@"syncServiceDidFAIL! %@", error);
	ShowAlert(@"Sync Error", error);
} 

#pragma mark -
#pragma mark SelectFiltersViewControllerDelegate Methods

- (void)filtersDismissed {
	[self dismissModalViewControllerAnimated:YES];
}

#pragma mark -
#pragma mark LoginViewControllerDelegate Methods

- (void) didAuthenticate {
	[self dismissModalViewControllerAnimated:YES];
	[self showSurveyListings];
}

- (UIImage *)iconForListing:(SurveyListing *)listing {
    
	if(listing == nil)
        return nil;
    
    if([[icons allKeys] containsObject:listing.iconUrl]) {
        return [icons objectForKey:listing.iconUrl];
    }
    
    //Are we already requesting this icon?
    if([[iconRequests allKeys] containsObject:listing.iconUrl]) {
        return nil; 
    }
    
    if(!listing.iconUrl)
        return nil; //no icon to fetch
    
    //request the image
    ASIHTTPRequest *iconRequest = [ASIHTTPRequest requestWithURL:[NSURL URLWithString:listing.iconUrl]];
    iconRequest.delegate = self;
    iconRequest.userInfo = [NSDictionary dictionaryWithObject:listing forKey:@"Listing"];
    iconRequest.cachePolicy = ASIOnlyLoadIfNotCachedCachePolicy;
    iconRequest.cacheStoragePolicy = ASICachePermanentlyCacheStoragePolicy;

    [iconRequests setObject:iconRequest forKey:listing.iconUrl];
    [iconRequest startAsynchronous];
    return nil;
}

#pragma mark -
#pragma mark ASIHTTPRequestDelegate MEthods

-(void)requestFinished:(ASIHTTPRequest *)request {
    NSData *imageData = [request responseData];
    UIImage *image = [UIImage imageWithData:imageData];
    [iconRequests removeObjectForKey:request.url];
    
    SurveyListing *listing = [request.userInfo objectForKey:@"Listing"];
    NSAssert(listing != nil, @"Listing shouldn't be nil!");
    [icons setObject:image forKey:listing.iconUrl];
    [self.tableView reloadData];
}

-(void)requestFailed:(ASIHTTPRequest *)request {
    LOGLINE(@"Icon request failed for url: %@", request.url);
}

#pragma mark -
#pragma mark UITableViewDataSource Methods

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    if (self.surveyListings)
        return 1;
    
    return 0;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    return [surveyListings count];
}


- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath {
	return ROW_HEIGHT;
}


- (UITableViewCell *)tableView:(UITableView *)tv cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    
    static NSString *CellIdentifier = @"Cell";
    SurveyCell *cell = (SurveyCell *)[tv dequeueReusableCellWithIdentifier:CellIdentifier];
    if (cell == nil) {		
		NSArray *nibs = [[NSBundle mainBundle] loadNibNamed:@"SurveyCell" owner:self options:nil];
		for (id nib in nibs) {
			if ([nib isKindOfClass:[SurveyCell class]]) {
				cell = (SurveyCell *)nib;
				UILongPressGestureRecognizer *longPressRecognizer = [[UILongPressGestureRecognizer alloc] initWithTarget:self action:@selector(handleLongPress:)];
				[cell addGestureRecognizer:longPressRecognizer];        
				[longPressRecognizer release];
			}
		}		
    }

	SurveyListing *listing = [self.surveyListings objectAtIndex:indexPath.row];
	
	if (listing.percentComplete == 0) {
		cell.progressView.hidden = YES;
	} 
	else {
		cell.progressView.hidden = NO;
		cell.progressView.progress = listing.percentComplete;
	}
    cell.imageIcon.image = [self iconForListing:listing];
	cell.titleLabel.text = listing.title;
	cell.readyToSubmitLabel.text = [NSString stringWithFormat:@"%d", listing.numberReadyToSubmit];
	if (listing.isFavorite){
		cell.favoriteStarView.image= [UIImage imageNamed:@"selected.png"];
	} else {
		cell.favoriteStarView.image= nil;		
	}
	return cell;
}


#pragma mark -
#pragma mark UITableViewDelegate Methods

- (void)tableView:(UITableView *)tv didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    if (!self.surveyListings)
        return;

    SurveyListing *selectedSurvey = [self.surveyListings objectAtIndex:indexPath.row];
	SurveyResponseViewController *vc = [[SurveyResponseViewController alloc] initWithSurveyId:selectedSurvey.surveyId];	
    [self.navigationController pushViewController:vc animated:YES];
    [vc release];
	
    [tv deselectRowAtIndexPath:indexPath animated:YES];
}


@end

