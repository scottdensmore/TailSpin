/*
 *  ViewHelpers.m
 *  tailspin-iphone
 *
 *  Created by Ben Scheirman on 9/24/10.
 *  Copyright 2010 Microsoft. All rights reserved.
 *
 */

#import "ViewHelpers.h"

id LoadViewFromNib(Class class) {
	NSString *name = [class description];
	NSArray *nibViews = [[NSBundle mainBundle] loadNibNamed:name owner:nil options:nil];
	
	if(nibViews.count == 0) {
		LOG(@"The nib didn't contain any views");
		exit(1);
	}
	return [nibViews objectAtIndex:0];
}