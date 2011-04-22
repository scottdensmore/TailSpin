namespace TailSpin.PhoneClient.Services
{
    using System;

    public interface INavigationService
    {
        bool CanGoBack { get; }
        Uri CurrentSource { get; }
        bool Navigate(Uri source);
        void GoBack();
    }
}
