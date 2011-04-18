//
//  SelectFiltersViewController.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/22/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "SelectFiltersViewController.h"
#import "SurveyFilterCategory.h"
#import "SurveyFilter.h"
#import "AlertHelper.h"

@interface SelectFiltersViewController(Private)

- (void)setupSaveButton;
- (void)setupIndicatorButton;
- (void)setupCancelButton;

@end

@implementation SelectFiltersViewController

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(proxy);
	RELEASE(filters);
	RELEASE(indicatorView);
	self.delegate = nil;
	
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize filters, indicatorView, delegate;

#pragma mark -
#pragma mark View Methods

- (void)viewDidLoad {
    [super viewDidLoad];
	
    self.tableView.backgroundView = [[[UIImageView alloc] initWithImage:[UIImage imageNamed:@"Default.png"]] autorelease];
    
    self.title = @"Select Filters";
	[self setupIndicatorButton];
	[self.indicatorView startAnimating];
	
	self.filters = [NSArray array];
    
    proxy = [[TailspinProxy alloc] init];
    proxy.delegate = self;
    [proxy asyncFetchFilters];
}

- (void)didReceiveMemoryWarning {
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
    
    // Relinquish ownership any cached data, images, etc that aren't in use.
}

- (void)viewDidUnload {
    [super viewDidUnload];
}

#pragma mark -
#pragma mark Actions
- (IBAction)saveButtonTapped:(id)sender {
	self.indicatorView = [[UIActivityIndicatorView alloc] initWithFrame:CGRectMake(0, 0, 20, 20)];
    UIBarButtonItem *barButton = [[UIBarButtonItem alloc] initWithCustomView:self.indicatorView];
	[self navigationItem].rightBarButtonItem = barButton;
	[self navigationItem].leftBarButtonItem.enabled = NO;
	[self.indicatorView startAnimating];
	[proxy sendFilters:self.filters];
}

- (IBAction)cancelButtonTapped:(id)sender {
	[self.delegate filtersDismissed];
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

- (void)setupCancelButton {
	self.navigationItem.rightBarButtonItem =  [[UIBarButtonItem alloc] initWithBarButtonSystemItem: UIBarButtonSystemItemCancel 
																						   target:self 
																						   action:@selector(cancelButtonTapped:)];
}

#pragma mark -
#pragma mark TailspinProxyDelegate callbacks

- (void)didFetchFilters:(NSArray *)filters_ {
    self.filters = filters_;
	[self setupSaveButton];
	[self setupCancelButton];
    [self.tableView reloadData];
}

- (void)didFailWithError:(NSString *)error {
	ShowAlert(@"Error", error);
	[self setupCancelButton];
	[self setupSaveButton];
	self.navigationItem.leftBarButtonItem.enabled = NO;
}

- (void)didSendFilters {
	[self.delegate filtersDismissed];
}

#pragma mark -
#pragma mark UITableViewDataSource Methods

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
	LOGLINE(@"# sections");
    return [filters count];
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
	LOGLINE(@"# rows");
    return [[[filters objectAtIndex:section] allFilters] count];
}

- (NSString *)tableView:(UITableView *)tableView titleForHeaderInSection:(NSInteger)section {
	LOGLINE(@"section title");
    return [[filters objectAtIndex:section] name];
}

- (UIView *)tableView:(UITableView *)tableView_ viewForHeaderInSection:(NSInteger)section {
	UIView *container = [[[UIView alloc] initWithFrame:CGRectMake(0, 0, 320, 40)] autorelease];
	UILabel *label = [[[UILabel alloc] initWithFrame:CGRectInset(container.bounds, 10, 0)] autorelease];
	label.font = [UIFont boldSystemFontOfSize:16];
	label.shadowColor = [UIColor blackColor];
	label.shadowOffset = CGSizeMake(1, 1);
	label.textColor = [UIColor whiteColor];
	label.text = [self tableView:self.tableView titleForHeaderInSection:section];
	label.backgroundColor = [UIColor clearColor];
	
	[container addSubview:label];
	return container;
}

- (CGFloat)tableView:(UITableView *)tableView heightForHeaderInSection:(NSInteger)section {
	return 40;
}

// Customize the appearance of table view cells.
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    
    static NSString *CellIdentifier = @"Cell";
    
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier];
    if (cell == nil) {
        cell = [[[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier] autorelease];
    }
    
    SurveyFilterCategory *category = [self.filters objectAtIndex:indexPath.section];
    SurveyFilter *filter = [category.allFilters objectAtIndex:indexPath.row];
    
    if([category.selectedFilters containsObject:filter]) {
        cell.accessoryType = UITableViewCellAccessoryCheckmark;
    } else {
        cell.accessoryType = UITableViewCellAccessoryNone;
    }
    
    cell.textLabel.text = filter.value;
    
    return cell;
}

#pragma mark -
#pragma mark UITableViewDelegate Methods

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    [tableView deselectRowAtIndexPath:indexPath animated:YES];
    
    SurveyFilterCategory *cat = [self.filters objectAtIndex:indexPath.section];
    SurveyFilter *filter = [cat.allFilters objectAtIndex:indexPath.row];
    
    if([cat.selectedFilters containsObject:filter]) {
        [cat.selectedFilters removeObject:filter];
    } else {
        [cat.selectedFilters addObject:filter];
    }
	
    [tableView reloadRowsAtIndexPaths:[NSArray arrayWithObject:indexPath] withRowAnimation:UITableViewRowAnimationNone];
}


@end

