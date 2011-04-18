//
//  AudioService.h
//  tailspin-iphone
//
//  Created by Kevin Lee on 9/2/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <AVFoundation/AVFoundation.h>
#import <AudioToolbox/AudioToolbox.h>


@interface AudioService : NSObject <AVAudioSessionDelegate, AVAudioRecorderDelegate, AVAudioPlayerDelegate>{
	//player stuff
	AVAudioPlayer *player;
	id<AVAudioPlayerDelegate> playerDelegate;
	
	//recorder stuff
	AVAudioRecorder *recorder;
	id<AVAudioRecorderDelegate> recorderDelegate;

	//shared stuff
	NSURL *audioFile;
	AVAudioSession *session;
	
	NSString *filePath;
}
//player stuff
@property(nonatomic, retain) AVAudioPlayer *player;
@property(nonatomic, assign) id<AVAudioPlayerDelegate> playerDelegate;

//recorder stuff
@property(nonatomic, retain) AVAudioRecorder *recorder;
@property(nonatomic, assign) id<AVAudioRecorderDelegate> recorderDelegate;

//shared stuff
@property(nonatomic, retain) NSURL *audioFile;
@property(nonatomic, retain) AVAudioSession *session;
@property(nonatomic, retain) NSString *filePath;

- (void)stopPlaying;
- (void)pausePlaying;
- (BOOL)play;
- (BOOL)isPlaying;

- (void)stopRecording;
- (void)pauseRecording;
- (BOOL)record;
- (BOOL)isRecording;

- (id)initForTenant:(NSString*)tenant withSlug:(NSString *)slug atQuestionIndex:(NSInteger)questionIndex;

@end
