//
//  CellMenuItem.m
//  tailspin-iphone
//
//  Created by Scott Densmore on 9/29/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "CellMenuItem.h"


@implementation CellMenuItem

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
    RELEASE(indexPath);
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize indexPath;

@end
