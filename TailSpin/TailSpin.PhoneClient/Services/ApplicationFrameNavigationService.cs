namespace TailSpin.PhoneClient.Services
{
    using System;
    using Microsoft.Phone.Controls;

    public class ApplicationFrameNavigationService : INavigationService
    {
        private readonly PhoneApplicationFrame frame;

        public ApplicationFrameNavigationService(PhoneApplicationFrame frame)
        {
            this.frame = frame;
        }

        public bool CanGoBack
        {
            get { return this.frame.CanGoBack; }
        }

        public Uri CurrentSource
        {
            get { return this.frame.CurrentSource; }
        }

        public bool Navigate(Uri source)
        {
            return this.frame.Navigate(source);
        }

        public void GoBack()
        {
            this.frame.GoBack();
        }
    }
}
