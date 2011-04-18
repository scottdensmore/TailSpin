//
//  PictureAnswerView.h
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/30/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "AnswerView.h"


@interface PictureAnswerView : AnswerView <UIImagePickerControllerDelegate, UINavigationControllerDelegate>{
	UIImageView *imageView;
	UIButton *takePictureButton;

}

@property(nonatomic, retain) IBOutlet UIImageView *imageView;
@property (nonatomic, retain) IBOutlet UIButton *takePictureButton;

-(IBAction)getCameraPicture:(id)sender;

@end
