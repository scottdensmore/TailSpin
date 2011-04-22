namespace TailSpin.PhoneClient.ViewModels
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Windows.Media.Imaging;
    using Microsoft.Phone;
    using Microsoft.Phone.Reactive;
    using Microsoft.Phone.Tasks;
    using Microsoft.Practices.Prism.Commands;
    using Models;

    public class PictureQuestionViewModel : QuestionViewModel
    {
        private readonly CameraCaptureTask task;
        private bool capturing;

        public PictureQuestionViewModel(QuestionAnswer questionAnswer)
            : base(questionAnswer)
        {
            this.CameraCaptureCommand = new DelegateCommand(this.CapturePicture, () => !this.Capturing);
            if (questionAnswer.Value != null)
            {
                this.CreatePictureBitmap(questionAnswer.Value);
            }
            this.task = new CameraCaptureTask();
            
            // Subscribe to handle new photo stream
            Observable.FromEvent<PhotoResult>(h => this.task.Completed += h, h => this.task.Completed -= h)
                .Where(e => e.EventArgs.ChosenPhoto != null)
                .ObserveOn(Scheduler.ThreadPool)
                .Select(a => CreatePictureFile(a.EventArgs.ChosenPhoto))
                .ObserveOn(Scheduler.Dispatcher)
                .Subscribe(p =>
                               {
                                   this.Answer.Value = p;
                                   this.CreatePictureBitmap(p);
                                   this.Capturing = false;
                                   this.RaisePropertyChanged(string.Empty);
                                   this.CameraCaptureCommand.RaiseCanExecuteChanged();
                               });

            // Subscribe to enable Capture command
            Observable.FromEvent<PhotoResult>(h => this.task.Completed += h, h => this.task.Completed -= h)
                .Where(e => e.EventArgs.ChosenPhoto == null)
                .ObserveOn(Scheduler.Dispatcher)
                .Subscribe(p =>
                                {
                                    this.Capturing = false;
                                    this.CameraCaptureCommand.RaiseCanExecuteChanged();
                                });
        }

        public DelegateCommand CameraCaptureCommand { get; set; }

        public WriteableBitmap Picture { get; set; }

        public bool Capturing
        {
            get { return this.capturing; }
            set
            {
                if (this.capturing != value)
                {
                    this.capturing = value;
                    this.RaisePropertyChanged(() => this.Capturing);
                }
            }
        }

        private static string CreatePictureFile(Stream chosenPhoto)
        {
            // Save the stream and return the file path
            var filename = string.Format(CultureInfo.InvariantCulture, "{0}.jpeg", Guid.NewGuid());

            using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fs = new IsolatedStorageFileStream(filename, FileMode.Create, filesystem))
                {
                    using (var sr = new BinaryReader(chosenPhoto))
                    {
                        for (int i = 0; i < chosenPhoto.Length; i++)
                        {
                            fs.WriteByte(sr.ReadByte());
                        }
                    }
                    fs.Flush();
                }
            }

            return filename;
        }

        private void CreatePictureBitmap(string path)
        {
            using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (filesystem.FileExists(path))
                {
                    using (var fs = new IsolatedStorageFileStream(path, FileMode.Open, filesystem))
                    {
                        this.Picture = PictureDecoder.DecodeJpeg(fs);
                    }
                }
            }
        }

        private void CapturePicture()
        {
            if (!this.Capturing)
            {
                this.task.Show();
                this.Capturing = true;
                this.CameraCaptureCommand.RaiseCanExecuteChanged();
            }
        }
    }
}