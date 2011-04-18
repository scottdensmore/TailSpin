//
//  MultipleChoiceAnswerView.m
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/25/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "MultipleChoiceAnswerView.h"
#import "AppDelegate.h"

@implementation MultipleChoiceAnswerView

#pragma mark -
#pragma mark Dealoc

- (void)dealloc {
	RELEASE(possibleAnswers);
	RELEASE(tableView);
	
    [super dealloc];
}


#pragma mark - 
#pragma mark Properties

@synthesize possibleAnswers;
@synthesize tableView;


#pragma mark -
#pragma mark View Methods

- (void)layoutSubviews {
	[super layoutSubviews];
}

#pragma mark -
#pragma mark UITableViewDataSource Methods

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tv {
    return 1;
}

- (NSInteger)tableView:(UITableView *)tv numberOfRowsInSection:(NSInteger)section {
    return [possibleAnswers count];
}

- (UITableViewCell *)tableView:(UITableView *)tv cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    
    static NSString *CellIdentifier = @"Cell";
    
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier];
    if (cell == nil) {
        cell = [[[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier] autorelease];
    }
     
	NSString *possibleAnswer = [possibleAnswers objectAtIndex:indexPath.row];
    if([self.answer isEqual:possibleAnswer]) {
        cell.accessoryType = UITableViewCellAccessoryCheckmark;
    } else {
        cell.accessoryType = UITableViewCellAccessoryNone;
    }
    
    cell.textLabel.text = possibleAnswer;
    
    return cell;
}

#pragma mark -
#pragma mark UITableViewDelegate Methods

- (void)tableView:(UITableView *)tv didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    [tableView deselectRowAtIndexPath:indexPath animated:NO];
	
	NSInteger index = [possibleAnswers indexOfObject:self.answer];
    if (index == indexPath.row) {
        return;
    }
	
    NSIndexPath *oldIndexPath = [NSIndexPath indexPathForRow:index inSection:0];
	
	UITableViewCell *newCell = [tableView cellForRowAtIndexPath:indexPath];
    if (newCell.accessoryType == UITableViewCellAccessoryNone) {
        newCell.accessoryType = UITableViewCellAccessoryCheckmark;
        self.answer = [possibleAnswers objectAtIndex:indexPath.row];
		[self.delegate answerView:self didUpdateAnswer:self.answer];
    }
	
    UITableViewCell *oldCell = [tableView cellForRowAtIndexPath:oldIndexPath];
    if (oldCell.accessoryType == UITableViewCellAccessoryCheckmark) {
        oldCell.accessoryType = UITableViewCellAccessoryNone;
    }
}


@end
