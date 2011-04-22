namespace TailSpin.PhoneClient.Tests.ViewModels.Mocks
{
    using TailSpin.PhoneClient.Services.Stores;

    public class MockSettingsStore : ISettingsStore
    {
        public string Password { get; set; }
        public string PushNotificationUri { get; set; }
        public bool SubscribeToPushNotifications { get; set; }
        public string UserName { get; set; }
        public bool LocationServiceAllowed { get; set; }
    }
}