//
//  RequestFactory.m
//  tailspin-iphone
//
//  Created by Kevin Lee on 9/9/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "RequestFactory.h"
#import "ASIHTTPRequest.h"
#import "UserSettings.h"
#import "UserSettingsService.h"

@implementation RequestFactory

#pragma mark -
#pragma mark Class Methods

+ (NSURL *)apiURLWithEndpoint:(NSString *)endpoint {
	NSString *urlString = [NSString stringWithFormat:@"http://tailspindemo.cloudapp.net:8080%@",endpoint];
	NSString *esapedString = [urlString stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	LOGLINE(@"Escaped URL: %@", esapedString);
	NSURL *url = [NSURL URLWithString:esapedString];
	return url;
}

+ (ASIHTTPRequest *)requestWithEndpoint:(NSString *)endpoint {	
	ASIHTTPRequest *request = [ASIHTTPRequest requestWithURL: [self apiURLWithEndpoint:endpoint] ];
	[request addRequestHeader:@"Accept" value:@"application/json"];
	[request addRequestHeader:@"Content-Type" value:@"application/json"];
	UserSettings *settings = [UserSettingsService retrieve];
	NSString *authString = [NSString stringWithFormat:@"user %@:pass %@", settings.username, settings.password];
	[request addRequestHeader:@"Authorization" value:authString];
	return request;	
}

+ (ASIHTTPRequest *)mediaRequestWithEndpoint:(NSString *)endpoint {
	ASIHTTPRequest *request = [ASIHTTPRequest requestWithURL: [self apiURLWithEndpoint:endpoint] ];
	[request addRequestHeader:@"Accept" value:@"application/json"];
	//[request addRequestHeader:@"Content-Type" value:@"application/json"];
	UserSettings *settings = [UserSettingsService retrieve];
	NSString *authString = [NSString stringWithFormat:@"user %@:pass %@", settings.username, settings.password];
	[request addRequestHeader:@"Authorization" value:authString];
	return request;	
}

@end
