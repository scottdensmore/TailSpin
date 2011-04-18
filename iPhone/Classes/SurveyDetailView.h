//
//  SurveyDetailView.h
//  tailspin-iphone
//
//  Created by Kevin Lee on 9/22/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>


@interface SurveyDetailView : UIView {
	UILabel *durationLabel;
	UILabel *titleLabel;
	UIProgressView *progressView;
	UISwitch *favoriteSwitch;
}

@property (nonatomic, readonly) IBOutlet UILabel *durationLabel;
@property (nonatomic, readonly) IBOutlet UILabel *titleLabel;
@property (nonatomic, retain) IBOutlet UIProgressView *progressView;
@property (nonatomic, retain) IBOutlet UISwitch *favoriteSwitch;

@end
