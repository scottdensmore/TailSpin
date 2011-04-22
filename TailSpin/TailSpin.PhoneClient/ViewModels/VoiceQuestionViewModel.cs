namespace TailSpin.PhoneClient.ViewModels
{
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Text;
    using System.Threading;
    using System.Windows.Input;
    using Microsoft.Phone.Reactive;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Xna.Framework.Audio;
    using TailSpin.PhoneClient.Infrastructure;
    using TailSpin.PhoneClient.Models;

    public class VoiceQuestionViewModel : QuestionViewModel, IDisposable
    {
        private readonly string wavFileName;
        private WaveFormatter formatter;
        private bool isPlaying;
        private IDisposable observableMic;
        private byte[] buffer;
        private MemoryStream stream;

        public VoiceQuestionViewModel(QuestionAnswer questionAnswer)
            : base(questionAnswer)
        {
            this.DefaultActionCommand = new DelegateCommand(this.DefaultAction);
            this.wavFileName = questionAnswer.Value ?? string.Format("{0}.wav", Guid.NewGuid());
            this.PlayCommand = new DelegateCommand(this.Play, () => !this.IsPlaying && !this.IsRecording && !string.IsNullOrEmpty(this.Answer.Value));
        }

        public ICommand DefaultActionCommand { get; set; }

        public string DefaultActionText
        {
            get { return this.IsRecording ? "Stop Recording" : "Start Recording"; }
        }

        public bool IsPlaying
        {
            get { return this.isPlaying; }

            set
            {
                this.isPlaying = value;
                this.PlayCommand.RaiseCanExecuteChanged();
                this.RaisePropertyChanged(() => this.IsPlaying);
                this.RaisePropertyChanged(() => this.DefaultActionText);
            }
        }

        public bool IsRecording { get; set; }

        public DelegateCommand PlayCommand { get; set; }

        public void Dispose()
        {
            if (this.IsRecording)
            {
                this.StopRecording();
            }
        }

        private void DefaultAction()
        {
            if (!this.IsRecording)
            {
                this.StartRecording();
            }
            else
            {
                this.StopRecording();
            }

            this.IsRecording = !this.IsRecording;
            this.PlayCommand.RaiseCanExecuteChanged();
            this.RaisePropertyChanged(string.Empty);
        }

        private void Play()
        {
            this.IsPlaying = true;
            using (var fileSystem = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var dat = fileSystem.OpenFile(this.wavFileName, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        using (var effect = SoundEffect.FromStream(dat))
                        {
                            effect.Play();
                            var instance = effect.CreateInstance();
                            instance.Play();


                            while (instance.State == SoundState.Playing)
                            {
                                Thread.Sleep(100);
                            }
                        }
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }

            this.IsPlaying = false;
        }

        private void StartRecording()
        {
            var mic = Microphone.Default;
            if (mic.State == MicrophoneState.Started)
            {
                mic.Stop();
            }

            this.stream = new MemoryStream();

            buffer = new byte[mic.GetSampleSizeInBytes(mic.BufferDuration)];

            this.observableMic = Observable.FromEvent<EventArgs>(h => mic.BufferReady += h, h => mic.BufferReady -= h)
                .Subscribe(p => this.CaptureMicrophoneBufferResults());
            mic.Start();
        }

        private void CaptureMicrophoneBufferResults()
        {
            var mic = Microphone.Default;
            int bufferCaptured = mic.GetData(this.buffer);
            this.stream.Write(this.buffer, 0, bufferCaptured);
        }

        private void StopRecording()
        {
            var mic = Microphone.Default;
            this.CaptureMicrophoneBufferResults(); // Capture last bit of buffer not capture by event.
            mic.Stop();

            this.formatter = new WaveFormatter(this.wavFileName, (ushort)mic.SampleRate, 16, 1);
            this.formatter.WriteDataChunk(stream.ToArray());
           
            this.stream.Dispose();
            this.observableMic.Dispose();
            this.formatter.Dispose();
            this.formatter = null;
            this.buffer = null;
            this.Answer.Value = this.wavFileName;
        }
    }
}