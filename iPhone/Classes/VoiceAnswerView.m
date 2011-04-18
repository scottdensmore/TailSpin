//
//  VoiceAnswerView.m
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/31/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import "VoiceAnswerView.h"

#define DOCUMENTS_FOLDER [NSHomeDirectory() stringByAppendingPathComponent:@"Documents"]

@interface VoiceAnswerView(Private)

- (void)updateViewForPlayerState;
- (void)updateViewForRecorderState;
- (void)pausePlayback;
- (void)startPlayback;
- (void)startRecording;

@end

@implementation VoiceAnswerView

#pragma mark -
#pragma mark Dealloc

- (void)dealloc {
	RELEASE(audioService);
	RELEASE(playButton);
	RELEASE(recordButton);
	
	[super dealloc];
}

#pragma mark -
#pragma mark Properties

@synthesize playButton, recordButton;
@synthesize audioService;

-(void)initializeRecorder{
	AudioService *as = [[AudioService alloc] initForTenant:self.tenant withSlug:self.slug atQuestionIndex:self.questionIndex];
	as.recorderDelegate = self;
	as.playerDelegate = self;
	
	self.audioService = as;
	
	[as release];
}

#pragma mark -
#pragma mark Actions

- (IBAction)recordButtonPressed:(UIButton*)sender {
	
	LOGLINE(@"The record button was pressed");
	if (self.audioService == nil){
		[self initializeRecorder];		
	}
	
	if ([self.audioService isRecording]) {
		LOGLINE(@"stop recording.");
		
		[self.audioService stopRecording]; 		
	} 
	else {
		LOGLINE(@"starting recording.");	
		[self.audioService record];	
	}
	
	[self updateViewForRecorderState];
}

- (IBAction)playButtonPressed:(UIButton *)sender {
	
	LOGLINE(@"The play button was pressed");
	if (self.audioService == nil){
		[self initializeRecorder];		
	}
	
	if ([self.audioService.player isPlaying] == YES) {
		[self pausePlayback];
	}
	else {
		[self startPlayback];
	}
}


#pragma mark -
#pragma mark Private Methods

- (void)updateViewForPlayerState {
	
	if ([self.audioService isPlaying]) {
		[playButton setTitle:@"Pause" forState:UIControlStateNormal];
	}
	else {
		[playButton setTitle:@"Play" forState:UIControlStateNormal];
	}
}
- (void)updateViewForRecorderState {
	if ([self.audioService isRecording]) {
		[recordButton setTitle: @"Stop" forState: UIControlStateNormal];
	}
	else {
		[recordButton setTitle: @"Record" forState: UIControlStateNormal];
	}
}

- (void)pausePlayback {
	[self.audioService pausePlaying];	
	[self updateViewForPlayerState];
}

- (void)startPlayback {
	[self.audioService play];
	[self updateViewForPlayerState];
}

- (void)startRecording {
	[self.audioService record];
}

#pragma mark -
#pragma mark AVAudioPlayerDelegate Methods

- (void)audioPlayerDidFinishPlaying:(AVAudioPlayer *)p successfully:(BOOL)flag {
	[self updateViewForPlayerState];
	
}

// we will only get these notifications if playback was interrupted
- (void)audioPlayerBeginInterruption:(AVAudioPlayer *)p {
	LOGLINE(@"Interruption begin. Updating UI for new state");
	[self updateViewForPlayerState];
}

- (void)audioPlayerEndInterruption:(AVAudioPlayer *)p {
	LOGLINE(@"Interruption ended. Resuming playback");
	[self startPlayback];
}

#pragma mark -
#pragma mark AVAudioRecorderDelegate Methods

- (void)audioRecorderDidFinishRecording:(AVAudioRecorder *) aRecorder successfully:(BOOL)flag {	
	[self updateViewForRecorderState];
	self.answer = self.audioService.filePath;
	[self.delegate answerView:self didUpdateAnswer:self.answer];
}

@end
