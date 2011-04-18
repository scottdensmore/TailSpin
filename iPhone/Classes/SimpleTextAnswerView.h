//
//  SimpleTextAnswerView.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/23/10.
//  Copyright (c) 2010 __MyCompanyName__. All rights reserved.
//



#import "AnswerView.h"

@interface SimpleTextAnswerView : AnswerView {
	UITextField *answerInput;
}

@property (nonatomic, retain) IBOutlet UITextField *answerInput;


-(IBAction) textFieldDoneEditing:(id)sender;

@end
