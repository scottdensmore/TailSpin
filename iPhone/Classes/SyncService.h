//
//  SyncService.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/13/10.
//  Copyright (c) 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "TailspinProxy.h"

@protocol SyncServiceDelegate;

@interface SyncService : NSObject <TailspinProxyDelegate> {
    id<SyncServiceDelegate> delegate;
	TailspinProxy *proxy;
	BOOL sentSurveys;
	BOOL fetchedSurveys;
}

@property (nonatomic, assign) id<SyncServiceDelegate> delegate;

-(void)syncSurveys;

@end


@protocol SyncServiceDelegate <NSObject>

-(void)syncServiceDidComplete:(BOOL)wasUpdated;
-(void)syncServiceDidFailWithError:(NSString *)error;

@end