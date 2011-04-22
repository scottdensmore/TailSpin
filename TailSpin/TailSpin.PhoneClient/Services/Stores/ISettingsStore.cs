namespace TailSpin.PhoneClient.Services.Stores
{
    public interface ISettingsStore
    {
        string Password { get; set; }
        string UserName { get; set; }
        bool SubscribeToPushNotifications { get; set; }
        bool LocationServiceAllowed { get; set; }
    }
}