




 




namespace TailSpin.Web.Survey.Shared.Notifications
{
    using System;
    using System.Globalization;
    using System.Net;

    public static class WebRequestFactory
    {
        public const string ContentType = "text/xml; charset=utf-8";
        public const string DeviceConnectionStatusHeader = "X-DeviceConnectionStatus";
        public const string MessageIdHeader = "X-MessageID";
        public const string NotificationClassHeader = "X-NotificationClass";
        public const string NotificationStatusHeader = "X-NotificationStatus";
        public const string SubscriptionStatusHeader = "X-SubscriptionStatus";
        public const string WindowsphoneTargetHeader = "X-WindowsPhone-Target";
        private const int MaxPayloadLength = 1024;

        public static WebRequest CreatePhoneRequest(
            string channelUri, int payloadLength, NotificationType notificationType, string messageIdHeader = null)
        {
            if (payloadLength > MaxPayloadLength)
            {
                throw new ArgumentOutOfRangeException(
                    "Payload is too long. Maximum payload size shouldn't exceed " + MaxPayloadLength + " bytes");
            }

            var request = (HttpWebRequest)WebRequest.Create(channelUri);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = ContentType;
            request.ContentLength = payloadLength;
            request.Headers[MessageIdHeader] = messageIdHeader ?? Guid.NewGuid().ToString();
            
            if (notificationType == NotificationType.Toast)
            {
                request.Headers[WindowsphoneTargetHeader] = "toast";
                request.Headers[NotificationClassHeader] = 2.ToString(CultureInfo.InvariantCulture);
            }
            else if (notificationType == NotificationType.Tile)
            {
                request.Headers[WindowsphoneTargetHeader] = "token";
                request.Headers[NotificationClassHeader] = 1.ToString(CultureInfo.InvariantCulture);
            }

            return request;
        }
    }
}