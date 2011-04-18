//
//  FiveStarAnswerView.m
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/27/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "FiveStarAnswerView.h"

@interface FiveStarAnswerView(Private)

- (void)highlightStarsToTag:(NSInteger)tag;
- (void)starChosen:(UIButton *)star;

@end

@implementation FiveStarAnswerView

#pragma mark -
#pragma mark Init

- (id)initWithFrame:(CGRect)frame {
    if ((self = [super initWithFrame:frame])) {
		self.autoresizesSubviews = YES;
		self.backgroundColor = [UIColor whiteColor];
		
		self.stars = [NSMutableArray array];
		
		for (int i=0; i <5; i++) {

			UIButton *star = [UIButton buttonWithType:UIButtonTypeCustom];
			star.tag = i+1;
			
			NSInteger margin = 10 *i;
			star.frame = CGRectMake(20 +40 *i + margin, 40, 40, 40);
			
			[star setImage:[UIImage imageNamed:@"not_selected_40.png"] forState:UIControlStateNormal];
			[star setImage:[UIImage imageNamed:@"selected_40.png"] forState:UIControlStateHighlighted];
			[star addTarget:self action:@selector(starChosen:) forControlEvents:UIControlEventTouchUpInside];
			
			[stars addObject:star];
			[self addSubview:star];
		}
		
    }
	
    return self;
}

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(stars);
	
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize stars;

#pragma mark -
#pragma mark View Methods

- (void)layoutSubviews {
	[super layoutSubviews];
	
	NSInteger numberOfStars = [self.answer intValue];
	[self highlightStarsToTag:numberOfStars];
}

#pragma mark -
#pragma mark Private Methods

- (void)highlightStarsToTag:(NSInteger)tag {
	if(tag == 0)
		return;
		
	for (int i=0; i <=tag-1; i++ ) {
		UIButton *theButton = [stars objectAtIndex:i];
		[theButton setImage:[UIImage imageNamed:@"selected_40.png"] forState:UIControlStateNormal];
	}
	
	//unselect the stars if they were already selected.
	if (tag < [stars count]){
		for (int i = tag; i <= 4; i++) {
			UIButton *theButton = [stars objectAtIndex:i];
			[theButton setImage:[UIImage imageNamed:@"not_selected_40.png"] forState:UIControlStateNormal];	
		}
	}
}


- (void)starChosen:(UIButton *)star {
	self.answer = [NSString stringWithFormat:@"%d", star.tag];

	LOGLINE(@"chose star: %d, our answer is :%@",star.tag, answer);

	[self highlightStarsToTag:star.tag];
	
    [self.delegate answerView:self didUpdateAnswer:self.answer];
}

@end
