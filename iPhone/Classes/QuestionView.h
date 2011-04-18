//
//  QuestionView.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/20/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>

@class AnswerView;

@interface QuestionView : UIView {
	UITextView *questionLabel;
    UILabel *questionIndexLabel;    
	UIView *answerContainer;
    AnswerView *answerView;
}

@property (nonatomic, retain) IBOutlet UITextView *questionLabel;
@property (nonatomic, retain) IBOutlet UILabel *questionIndexLabel;
@property (nonatomic, retain) IBOutlet UIView *answerContainer;
@property (nonatomic, retain) AnswerView *answerView;
@property (nonatomic, assign) id answer;

@end
