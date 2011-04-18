//
//  PictureAnswerView.m
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/30/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "PictureAnswerView.h"
#import "FileHelpers.h"


#define radians( degrees ) ( degrees * M_PI / 180 )

@interface PictureAnswerView(Private)

- (NSString *)filePath;
- (UIImage *)imageWithImage:(UIImage*)sourceImage scaledToSizeWithSameAspectRatio:(CGSize)targetSize;

@end

@implementation PictureAnswerView

#pragma mark -
#pragma mark Init

- (id)initWithFrame:(CGRect)frame {
    if ((self = [super initWithFrame:frame])) {
        // Initialization code
		
    }
    return self;
}

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(imageView);
	RELEASE(takePictureButton);
	
    [super dealloc];
}


#pragma mark -
#pragma mark Properties

@synthesize imageView, takePictureButton;

#pragma mark -
#pragma mark View Methods

- (void)layoutSubviews{
	[super layoutSubviews];
	
	LOGLINE(@"DELEGATE: %@", self.delegate);
	
	//Check for file
	if([[NSFileManager defaultManager] fileExistsAtPath:[self filePath]]){
		self.imageView.image = [UIImage imageWithContentsOfFile:[self filePath]];
	};
	
}

- (UIViewController*)viewController {
	for (UIView *next = [self superview]; next; next = next.superview) {
		UIResponder *nextResponder = [next nextResponder];
		if ([nextResponder isKindOfClass:[UIViewController class]]) {
			return (UIViewController *)nextResponder;
		}
	}
	return nil;
}

#pragma mark -
#pragma mark Actions
- (IBAction)getCameraPicture:(id)sender {
	LOGLINE(@"take picture was clicked.");
	if ([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeCamera]) {
		UIImagePickerController *picker = [[UIImagePickerController alloc] init];
		picker.delegate = self;
		picker.allowsEditing = NO;
		picker.sourceType =  UIImagePickerControllerSourceTypeCamera;
		
		UIViewController *myViewController = [self viewController];
		[myViewController presentModalViewController:picker animated:YES];
		
		[picker release];									   		
	}
	else {
		LOGLINE(@"No camera :(");
	}
}

#pragma mark -
#pragma mark Private Methdos

- (NSString *)filePath {
	return [NSString stringWithFormat:@"%@/%@_%@_%d.png", DocumentsDirectory(),self.tenant, self.slug, self.questionIndex];
}


- (UIImage *)imageWithImage:(UIImage*)sourceImage scaledToSizeWithSameAspectRatio:(CGSize)targetSize {  
    CGSize imageSize = sourceImage.size;
    CGFloat width = imageSize.width;
    CGFloat height = imageSize.height;
    CGFloat targetWidth = targetSize.width;
    CGFloat targetHeight = targetSize.height;
    CGFloat scaleFactor = 0.0;
    CGFloat scaledWidth = targetWidth;
    CGFloat scaledHeight = targetHeight;
    CGPoint thumbnailPoint = CGPointMake(0.0,0.0);
	
    if (CGSizeEqualToSize(imageSize, targetSize) == NO) {
        CGFloat widthFactor = targetWidth / width;
        CGFloat heightFactor = targetHeight / height;
		
        if (widthFactor > heightFactor) {
            scaleFactor = widthFactor; // scale to fit height
        }
        else {
            scaleFactor = heightFactor; // scale to fit width
        }
		
        scaledWidth  = width * scaleFactor;
        scaledHeight = height * scaleFactor;
		
        // center the image
        if (widthFactor > heightFactor) {
            thumbnailPoint.y = (targetHeight - scaledHeight) * 0.5; 
        }
        else if (widthFactor < heightFactor) {
            thumbnailPoint.x = (targetWidth - scaledWidth) * 0.5;
        }
    }     
	
    CGImageRef imageRef = [sourceImage CGImage];
    CGBitmapInfo bitmapInfo = CGImageGetBitmapInfo(imageRef);
    CGColorSpaceRef colorSpaceInfo = CGImageGetColorSpace(imageRef);
	
    if (bitmapInfo == kCGImageAlphaNone) {
        bitmapInfo = kCGImageAlphaNoneSkipLast;
    }
	
    CGContextRef bitmap;
	
    if (sourceImage.imageOrientation == UIImageOrientationUp || sourceImage.imageOrientation == UIImageOrientationDown) {
        bitmap = CGBitmapContextCreate(NULL, targetWidth, targetHeight, CGImageGetBitsPerComponent(imageRef), CGImageGetBytesPerRow(imageRef), colorSpaceInfo, bitmapInfo);
		
    } else {
        bitmap = CGBitmapContextCreate(NULL, targetHeight, targetWidth, CGImageGetBitsPerComponent(imageRef), CGImageGetBytesPerRow(imageRef), colorSpaceInfo, bitmapInfo);
		
    }   
	
    // In the right or left cases, we need to switch scaledWidth and scaledHeight,
    // and also the thumbnail point
    if (sourceImage.imageOrientation == UIImageOrientationLeft) {
        thumbnailPoint = CGPointMake(thumbnailPoint.y, thumbnailPoint.x);
        CGFloat oldScaledWidth = scaledWidth;
        scaledWidth = scaledHeight;
        scaledHeight = oldScaledWidth;
		
        CGContextRotateCTM (bitmap, radians(90));
        CGContextTranslateCTM (bitmap, 0, -targetHeight);
		
    } else if (sourceImage.imageOrientation == UIImageOrientationRight) {
        thumbnailPoint = CGPointMake(thumbnailPoint.y, thumbnailPoint.x);
        CGFloat oldScaledWidth = scaledWidth;
        scaledWidth = scaledHeight;
        scaledHeight = oldScaledWidth;
		
        CGContextRotateCTM (bitmap, radians(-90));
        CGContextTranslateCTM (bitmap, -targetWidth, 0);
		
    } else if (sourceImage.imageOrientation == UIImageOrientationUp) {
        // NOTHING
    } else if (sourceImage.imageOrientation == UIImageOrientationDown) {
        CGContextTranslateCTM (bitmap, targetWidth, targetHeight);
        CGContextRotateCTM (bitmap, radians(-180.));
    }
	
    CGContextDrawImage(bitmap, CGRectMake(thumbnailPoint.x, thumbnailPoint.y, scaledWidth, scaledHeight), imageRef);
    CGImageRef ref = CGBitmapContextCreateImage(bitmap);
    UIImage* newImage = [UIImage imageWithCGImage:ref];
	
    CGContextRelease(bitmap);
    CGImageRelease(ref);
	
    return newImage; 
}

#pragma mark -
#pragma mark UIImagePickerControllerDelegate Methods

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info{
	LOGLINE(@"did finish picking media with info");
	[picker dismissModalViewControllerAnimated:YES];
	
	LOGLINE(@"show animation now");

	UIImage *new = [self imageWithImage:[info objectForKey:UIImagePickerControllerOriginalImage] scaledToSizeWithSameAspectRatio:self.imageView.frame.size];
	
	imageView.image = new;
	
	//write the file to disk
	NSData* imageData = UIImagePNGRepresentation(new);
	NSString *fullPathToFile = [self filePath];
	LOGLINE(@"Writing file: %@", fullPathToFile);
	[imageData writeToFile:fullPathToFile atomically:NO];

	self.answer = fullPathToFile;
	[self.delegate answerView:self didUpdateAnswer:self.answer];
	
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker{
	[picker dismissModalViewControllerAnimated:YES];
}

@end
