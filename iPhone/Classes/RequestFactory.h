//
//  RequestFactory.h
//  tailspin-iphone
//
//  Created by Kevin Lee on 9/9/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>

@class ASIHTTPRequest;

@interface RequestFactory : NSObject {
	
}

+(NSURL *)apiURLWithEndpoint:(NSString *)endpoint;
+(ASIHTTPRequest *)requestWithEndpoint:(NSString *)endpoint;
+(ASIHTTPRequest *)mediaRequestWithEndpoint:(NSString *)endpoint;

@end