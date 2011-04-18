/*
 *  C1Macros.h
 *  tailspin-iphone
 *
 *  Created by Ben Scheirman on 9/23/10.
 *  Copyright 2010 Microsoft. All rights reserved.
 *
 */


/*
 LOG -- calls NSLog only if DEBUG is defined
 */
#ifdef DEBUG
	#define LOG(...) NSLog(__VA_ARGS__)
#else
	#define LOG(...) /* */
#endif

/*
 LOGLINE -- calls NSLog only if DEBUG is defined, also adds in file, line numbers
 */
#ifdef DEBUG
	#define LOGLINE(fmt, ...) NSLog((@"%s [Line %d] " fmt), __PRETTY_FUNCTION__, __LINE__, ##__VA_ARGS__);
	#define FTLOGCALL LOG(@"[%@ %@]", NSStringFromClass([self class]), NSStringFromSelector(_cmd))
#else
	#define LOGLINE(...) /* */
	#define FTLOGCALL /* */
#endif

/* 
 RELEASE -- releases a variable and sets it to nil.
 */
//#define RELEASE(_obj) if(_obj) { [_obj release]; } _obj = nil

#if DEBUG 
	#define RELEASE(_obj) [_obj release] 
#else 
	#define RELEASE(_obj) [_obj release], _obj = nil 
#endif


#define NSNullIfNil(_obj) _obj == nil ? (id)[NSNull null] : _obj