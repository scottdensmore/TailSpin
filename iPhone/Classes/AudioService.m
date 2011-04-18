//
//  AudioService.m
//  tailspin-iphone
//
//  Created by Kevin Lee on 9/2/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "AudioService.h"
#import "FileHelpers.h"

@interface AudioService(Private)

- (void)initializeAudioSession;
- (NSMutableDictionary *)recorderSettings;
- (void)initializePlayer;
- (void)initializeRecorder;

@end

@implementation AudioService

#pragma mark -
#pragma mark Init

-(id) initForTenant:(NSString*)tenant withSlug:(NSString *)slug atQuestionIndex:(NSInteger)questionIndex {
	//set up the audio session and check hardware;
	if ((self = [super init])){
		
		[self initializeAudioSession];
		
		BOOL audioHWAvailable = session.inputIsAvailable;
		if (! audioHWAvailable) {
			UIAlertView *cantRecordAlert =
			[[UIAlertView alloc] initWithTitle: @"Warning"
									   message: @"Audio input hardware not available"
									  delegate: nil
							 cancelButtonTitle:@"OK"
							 otherButtonTitles:nil, nil];
			[cantRecordAlert show];
			[cantRecordAlert release]; 
			
		}
		
		filePath = [NSString stringWithFormat:@"%@/%@_%@_%d.caf", DocumentsDirectory(),tenant,slug, questionIndex];
		self.audioFile = [NSURL fileURLWithPath:filePath];
	}
	
	return self;
}

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(player);
	playerDelegate = nil;
	RELEASE(recorder);
	recorderDelegate = nil;
	RELEASE(audioFile);
	RELEASE(session);
	RELEASE(filePath);
	
	[super dealloc];
}


#pragma mark -
#pragma mark Properties

@synthesize player, playerDelegate, recorder, recorderDelegate, audioFile, session, filePath;

#pragma mark -
#pragma mark Methods

- (void)stopPlaying {
	if (!self.player)
		[self.player stop];
}

- (void)pausePlaying {
	if (!self.player)
		[self.player pause];
}

- (BOOL)play {
	[self initializeAudioSession];
	NSError *err = nil;
	if (!self.player){
		[self initializePlayer];
	}
	
	[self.session setActive:YES error:&err];
	if([self.player play])
		return YES;
	else{
		LOGLINE(@"play: %@ %d %@", [err domain], [err code], [[err userInfo] description]);
		LOGLINE(@"Could not play %@\n", self.player.url);
		LOGLINE(@"recorder path %@\n", self.recorder.url);
		return NO;
	}
	
}

- (BOOL)isPlaying {
	if (!self.player)
		return [self.player isPlaying];
	else {
		return NO;
	}
	
}

- (void)stopRecording {
	[self.recorder stop];
	[self.session setActive:NO error:nil];
	LOGLINE(@"Recording has stoped");
}

- (void)pauseRecording {
	[self.recorder pause];
}

- (BOOL)record {
	NSError *err= nil;
	if ( self.recorder == nil){
		[self initializeRecorder];
	}
	
	if([[NSFileManager defaultManager] fileExistsAtPath:filePath]){
		//[[NSFileManager defaultManager] removeItemAtPath:filePath error:nil];
		LOGLINE(@"File existed");
	}
	
	[self.session setActive:YES error:&err];
	
	if(err){
        LOGLINE(@"record: %@ %d %@", [err domain], [err code], [[err userInfo] description]);
	}
	
	return [self.recorder recordForDuration:(NSTimeInterval) 99];
}

- (BOOL)isRecording {
	return [self.recorder isRecording];
}

#pragma mark -
#pragma mark Private Methods

- (void)initializeAudioSession {
	if (self.session == nil){
		AVAudioSession *audioSession = [AVAudioSession sharedInstance];
		NSError *err= nil;
		[audioSession setCategory :AVAudioSessionCategoryPlayAndRecord error:&err];
		
		if(err){
			LOGLINE(@"audioSession: %@ %d %@", [err domain], [err code], [[err userInfo] description]);
		}
		
		self.session =audioSession;
	}
}

- (NSMutableDictionary *)recorderSettings {
	NSMutableDictionary *recorderSetting = [[NSMutableDictionary alloc] init];	
	[recorderSetting setValue:[NSNumber numberWithInt:kAudioFormatLinearPCM] forKey:AVFormatIDKey];
	[recorderSetting setValue:[NSNumber numberWithFloat:44100.0] forKey:AVSampleRateKey]; 
	[recorderSetting setValue:[NSNumber numberWithInt: 2] forKey:AVNumberOfChannelsKey];
	[recorderSetting setValue:[NSNumber numberWithInt:16] forKey:AVLinearPCMBitDepthKey];
	[recorderSetting setValue:[NSNumber numberWithBool:NO] forKey:AVLinearPCMIsBigEndianKey];
	[recorderSetting setValue:[NSNumber numberWithBool:NO] forKey:AVLinearPCMIsFloatKey];
	return [recorderSetting autorelease];
}

- (void)initializePlayer {
	AVAudioPlayer *aPlayer = [[AVAudioPlayer alloc] initWithContentsOfURL:self.audioFile error:nil];	
	aPlayer.numberOfLoops = 0;
	aPlayer.delegate = self;
	self.player = aPlayer;
	[aPlayer release];
}

- (void)initializeRecorder {
	NSError *err = nil;
	
	AVAudioRecorder *aRecorder = [[ AVAudioRecorder alloc] initWithURL:self.audioFile settings:[self recorderSettings] error:&err];
	[aRecorder setDelegate:self];
	[aRecorder prepareToRecord];
	aRecorder.meteringEnabled = YES;
	
	self.recorder = aRecorder;
	[aRecorder release];
}


#pragma mark -
#pragma mark AVAudioPlayerDelegate Methods

- (void)audioPlayerDidFinishPlaying:(AVAudioPlayer *)p successfully:(BOOL)flag {
	if (flag == NO) {
		LOGLINE(@"Playback finished unsuccessfully");
	}
	else {
		LOGLINE(@"Playback finished successfully");
	}

	[self.session setActive:NO error:nil];
	
	[p setCurrentTime:0.];
	
	[self.playerDelegate audioPlayerDidFinishPlaying:p successfully:flag];
}

- (void)audioPlayerBeginInterruption:(AVAudioPlayer *)p {
	LOGLINE(@"Interruption begin");
	[self.playerDelegate audioPlayerBeginInterruption:p];
}

- (void)audioPlayerEndInterruption:(AVAudioPlayer *)p {
	LOGLINE(@"Interruption ended. Resuming playback");
	[self.playerDelegate audioPlayerEndInterruption:p];
}

- (void)playerDecodeErrorDidOccur:(AVAudioPlayer *)p error:(NSError *)error {
	LOGLINE(@"ERROR IN DECODE: %@\n", error); 
}


#pragma mark -
#pragma mark AVAudioRecorderDelegate Methods

- (void)audioRecorderDidFinishRecording:(AVAudioRecorder *) aRecorder successfully:(BOOL)flag {	
	LOGLINE (@"audioRecorderDidFinishRecording:successfully:");
	[self.session setActive:NO error:nil];
	[self.recorderDelegate audioRecorderDidFinishRecording:aRecorder successfully:flag];
}

@end
