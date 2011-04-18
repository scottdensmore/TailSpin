




 




namespace TailSpin.Web.Survey.Shared.Notifications
{
    public delegate void DeviceNotFoundInMpns(string channelUri);

    public interface IPushNotification
    {
        void PushTileNotification(string channelUri, string message, string backgroundImage, int count, DeviceNotFoundInMpns callback);
        void PushToastNotification(string channelUri, string text1, string text2, DeviceNotFoundInMpns callback);
    }
}