//
//  tailspin_iphoneAppDelegate.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 8/11/10.
//  Copyright ChaiONE 2010. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface AppDelegate : NSObject <UIApplicationDelegate> {
    UIWindow *window;
	UIViewController *viewController;
}

@property (nonatomic, retain) IBOutlet UIWindow *window;
@property (nonatomic, retain) IBOutlet UIViewController *viewController;

+(UIWindow *)window;
+(NSString *)databasePath;

@end

