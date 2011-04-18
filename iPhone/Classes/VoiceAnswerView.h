//
//  VoiceAnswerView.h
//  tailspin-iphone
//
//  Created by Kevin Lee on 8/31/10.
//  Copyright 2010 Microsoft. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "AnswerView.h"
#import "AudioService.h"

@interface VoiceAnswerView : AnswerView <AVAudioSessionDelegate, AVAudioRecorderDelegate, AVAudioPlayerDelegate>{
	UIButton *playButton;
	UIButton *recordButton;
	AudioService *audioService;
}
@property (nonatomic, retain) IBOutlet UIButton *playButton;
@property (nonatomic, retain) IBOutlet UIButton *recordButton;
@property (nonatomic, retain) AudioService *audioService;

- (IBAction)playButtonPressed:(UIButton*)sender;
- (IBAction)recordButtonPressed:(UIButton*)sender;


@end
