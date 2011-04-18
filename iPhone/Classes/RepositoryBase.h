//
//  RepositoryBase.h
//  tailspin-iphone
//
//  Created by Ben Scheirman on 9/10/10.
//  Copyright (c) 2010 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@class FMDatabase;

@interface RepositoryBase : NSObject {
	
}

-(FMDatabase *)db;

@end
