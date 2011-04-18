//
//  SurveyCell.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/13/10.
//  Copyright 2010 ChaiONE. All rights reserved.
//

#import <UIKit/UIKit.h>


@interface SurveyCell : UITableViewCell {
	UIProgressView *progressView;
	UIImageView *favoriteStarView;
	UILabel *readyToSubmitLabel;
	UILabel *titleLabel;
	UIImageView *imageIcon;
}

@property (nonatomic, readonly) IBOutlet UIProgressView *progressView;
@property (nonatomic, readonly) IBOutlet UIImageView *favoriteStarView;
@property (nonatomic, readonly) IBOutlet UILabel *readyToSubmitLabel;
@property (nonatomic, readonly) IBOutlet UILabel *titleLabel;
@property (nonatomic, readonly) IBOutlet UIImageView *imageIcon;

- (BOOL)isFavorite;

@end
