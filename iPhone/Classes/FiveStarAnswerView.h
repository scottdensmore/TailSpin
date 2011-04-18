//
//  FiveStarAnswerView.h
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/27/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "AnswerView.h"

@interface FiveStarAnswerView : AnswerView	{
	NSMutableArray *stars;
}

@property(nonatomic, retain) NSMutableArray *stars;

@end
