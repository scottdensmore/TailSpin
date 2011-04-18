//
//  CellMenuItem.h
//  tailspin-iphone
//
//  Created by Scott Densmore on 9/29/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>


@interface CellMenuItem : UIMenuItem {
	NSIndexPath *indexPath;
}

@property (nonatomic, retain) NSIndexPath* indexPath;

@end
