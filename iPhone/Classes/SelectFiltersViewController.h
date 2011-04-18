//
//  SelectFiltersViewController.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/22/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "TailspinProxy.h"

@protocol SelectFiltersViewControllerDelegate;

@interface SelectFiltersViewController : UITableViewController <TailspinProxyDelegate> {
	TailspinProxy *proxy;
	NSArray *filters;
	UIActivityIndicatorView *indicatorView;
	id<SelectFiltersViewControllerDelegate> delegate;
}

@property (nonatomic, retain) NSArray *filters;
@property (nonatomic, retain) UIActivityIndicatorView *indicatorView;
@property (nonatomic, assign) id<SelectFiltersViewControllerDelegate> delegate;

- (IBAction)saveButtonTapped:(id)sender;
@end

@protocol SelectFiltersViewControllerDelegate <NSObject>

- (void)filtersDismissed;

@end