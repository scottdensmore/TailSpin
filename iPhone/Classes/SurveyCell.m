//
//  SurveyCell.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/13/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import "SurveyCell.h"


@implementation SurveyCell

#pragma mark -
#pragma mark Init

- (id)initWithStyle:(UITableViewCellStyle)style reuseIdentifier:(NSString *)reuseIdentifier {
    if ((self = [super initWithStyle:style reuseIdentifier:reuseIdentifier])) {	
		
    }
    return self;
}

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(progressView);
	RELEASE(favoriteStarView);
	RELEASE(readyToSubmitLabel);
	RELEASE(titleLabel);
	RELEASE(imageIcon);
	
    [super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize progressView, favoriteStarView, readyToSubmitLabel, titleLabel, imageIcon;

- (BOOL) isFavorite {
	if (favoriteStarView.image == nil) {
		return YES;
	}
	return NO;
}

#pragma mark -
#pragma mark View Methods

- (void)setSelected:(BOOL)selected animated:(BOOL)animated {

    [super setSelected:selected animated:animated];

    // Configure the view for the selected state
	readyToSubmitLabel.textColor = self.textLabel.textColor;
	readyToSubmitLabel.backgroundColor = self.textLabel.backgroundColor;
}


@end
