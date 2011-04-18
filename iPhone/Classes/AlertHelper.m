//
//  AlertHelper.m
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/1/10.
//  Copyright (c) 2010 __MyCompanyName__. All rights reserved.
//

#import "AlertHelper.h"


void ShowAlert(NSString *title, NSString *msg) {
    UIAlertView *av = [[UIAlertView alloc] initWithTitle:title 
                                                 message:msg 
                                                delegate:nil 
                                       cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
    [av show];
    [av autorelease];
}