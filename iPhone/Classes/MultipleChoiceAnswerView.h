//
//  MultipleChoiceAnswerView.h
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/25/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "AnswerView.h"

@interface MultipleChoiceAnswerView : AnswerView <UITableViewDelegate, UITableViewDataSource> {
	NSArray *possibleAnswers;
	UITableView *tableView;
}

@property (nonatomic, retain) NSArray *possibleAnswers;
@property (nonatomic, retain) IBOutlet UITableView *tableView;

@end
