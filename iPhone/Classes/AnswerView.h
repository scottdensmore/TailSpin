//
//  AnswerView.h
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/26/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>

@protocol AnswerDelegate;

@interface AnswerView : UIView {
	UILabel *answerLabel;
    NSString *answer;
    id<AnswerDelegate> delegate;
    NSInteger questionIndex;
	NSString *tenant;
	NSString *slug;
}

@property (nonatomic, retain) IBOutlet UILabel *answerLabel;
@property (nonatomic, retain) NSString *answer;
@property (nonatomic, assign) id<AnswerDelegate> delegate;
@property (nonatomic, assign) NSInteger questionIndex;
@property (nonatomic, retain) NSString *tenant;
@property (nonatomic, retain) NSString *slug;

@end


@protocol AnswerDelegate <NSObject>

- (void)answerView:(AnswerView *)answerView didUpdateAnswer:(NSString *)answerText;

@end
    