




 




namespace TailSpin.Web.Survey.Shared.Notifications
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net;

    public class PushNotification : IPushNotification
    {
        public void PushTileNotification(string channelUri, string message, string backgroundImage, int count, DeviceNotFoundInMpns callback)
        {
            byte[] payload = TileNotificationPayloadBuilder.Create(message, backgroundImage, count);
            string messageId = Guid.NewGuid().ToString();
            this.SendMessage(NotificationType.Tile, channelUri, messageId, payload, callback);
        }

        public void PushToastNotification(string channelUri, string text1, string text2, DeviceNotFoundInMpns callback)
        {
            byte[] payload = ToastNotificationPayloadBuilder.Create(text1, text2);
            string messageId = Guid.NewGuid().ToString();
            this.SendMessage(NotificationType.Toast, channelUri, messageId, payload, callback);
        }

        protected void OnNotified(NotificationType notificationType, HttpWebResponse response)
        {
            var args = new NotificationArgs(notificationType, response);
            Debug.WriteLine(args.ToString());
            Trace.TraceInformation(args.ToString());
        }

        private void SendMessage(NotificationType notificationType, string channelUri, string messageId, byte[] payload, DeviceNotFoundInMpns callback)
        {
            try
            {
                WebRequest request = WebRequestFactory.CreatePhoneRequest(channelUri, payload.Length, notificationType, messageId);
                request.BeginGetRequestStream(
                    ar =>
                        {
                            // Once async call returns get the Stream object
                            Stream requestStream = request.EndGetRequestStream(ar);

                            // and start to write the payload to the stream asynchronously
                            requestStream.BeginWrite(
                                payload,
                                0,
                                payload.Length,
                                iar =>
                                    {
                                        // When the writing is done, close the stream
                                        requestStream.EndWrite(iar);
                                        requestStream.Close();

                                        // and switch to receiving the response from MPNS
                                        request.BeginGetResponse(
                                            iarr =>
                                                {
                                                    try
                                                    {
                                                        using (WebResponse response = request.EndGetResponse(iarr))
                                                        {
                                                            // Notify the caller with the MPNS results
                                                            this.OnNotified(notificationType, (HttpWebResponse)response);
                                                        }
                                                    }
                                                    catch (WebException ex)
                                                    {
                                                        if (ex.Status == WebExceptionStatus.ProtocolError)
                                                        {
                                                            this.OnNotified(notificationType, (HttpWebResponse)ex.Response);
                                                        }
                                                        if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
                                                        {
                                                            callback(channelUri);
                                                        }
                                                        Trace.TraceError(ex.TraceInformation());
                                                    }
                                                },
                                            null);
                                    },
                                null);
                        },
                    null);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    this.OnNotified(notificationType, (HttpWebResponse)ex.Response);
                }
                Trace.TraceError(ex.TraceInformation());
            }
        }

        private class NotificationArgs
        {
            public NotificationArgs(NotificationType notificationType, HttpWebResponse response)
            {
                this.Timestamp = DateTimeOffset.Now;
                this.MessageId = response.Headers[WebRequestFactory.MessageIdHeader];
                this.ChannelUri = response.ResponseUri.ToString();
                this.NotificationType = notificationType;
                this.StatusCode = response.StatusCode;
                this.NotificationStatus = response.Headers[WebRequestFactory.NotificationStatusHeader];
                this.DeviceConnectionStatus = response.Headers[WebRequestFactory.DeviceConnectionStatusHeader];
                this.SubscriptionStatus = response.Headers[WebRequestFactory.SubscriptionStatusHeader];
            }

            private string ChannelUri { get; set; }
            private string DeviceConnectionStatus { get; set; }
            private string MessageId { get; set; }
            private string NotificationStatus { get; set; }
            private NotificationType NotificationType { get; set; }
            private HttpStatusCode StatusCode { get; set; }
            private string SubscriptionStatus { get; set; }
            private DateTimeOffset Timestamp { get; set; }

            public override string ToString()
            {
                return
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "ChannelUri = {0}{8}DeviceConnectionStatus = {1}{8}MessageId = {2}{8}NotificationStatus = {3}{8}NotificationType = {4}{8}HttpStatusCode = {5}{8}SubscriptionStatus = {6}{8}Timestamp = {7}",
                        this.ChannelUri,
                        this.DeviceConnectionStatus,
                        this.MessageId,
                        this.NotificationStatus,
                        this.NotificationType,
                        this.StatusCode,
                        this.SubscriptionStatus,
                        this.Timestamp,
                        Environment.NewLine);
            }
        }
    }
}