




 




namespace TailSpin.Workers.Notifications.Tests
{
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TailSpin.Web.Survey.Shared.Notifications;

    [TestClass]
    public class WebRequestFactoryFixture
    {
        private const string Uri = "http://www.test.com";
        private const int PayloadLength = 100;

        [TestMethod]
        public void FactoryCreatesRequestWithUri()
        {
            WebRequest request = WebRequestFactory.CreatePhoneRequest(Uri, PayloadLength, NotificationType.Tile);

            Assert.AreEqual(Uri, request.RequestUri.OriginalString);
        }

        [TestMethod]
        public void FactorySetsRequestMethodToPost()
        {
            WebRequest request = WebRequestFactory.CreatePhoneRequest(Uri, PayloadLength, NotificationType.Tile);

            Assert.AreEqual(WebRequestMethods.Http.Post, request.Method);
        }

        [TestMethod]
        public void FactorySetsContentTypeToTextXml()
        {
            WebRequest request = WebRequestFactory.CreatePhoneRequest(Uri, PayloadLength, NotificationType.Tile);

            Assert.AreEqual(WebRequestFactory.ContentType, request.ContentType);
        }

        [TestMethod]
        public void FactorySetsContentLengthToPayloadLength()
        {
            WebRequest request = WebRequestFactory.CreatePhoneRequest(Uri, PayloadLength, NotificationType.Tile);

            Assert.AreEqual(PayloadLength, request.ContentLength);
        }

        [TestMethod]
        public void FactorySetsNotificationClassHeaderToOneForTile()
        {
            WebRequest request = WebRequestFactory.CreatePhoneRequest(Uri, PayloadLength, NotificationType.Tile);

            Assert.AreEqual(((int)NotificationType.Tile).ToString(), request.Headers[WebRequestFactory.NotificationClassHeader]);
        }

        [TestMethod]
        public void FactorySetsNotificationClassHeaderToOneForToast()
        {
            WebRequest request = WebRequestFactory.CreatePhoneRequest(Uri, PayloadLength, NotificationType.Toast);

            Assert.AreEqual(((int)NotificationType.Toast).ToString(), request.Headers[WebRequestFactory.NotificationClassHeader]);
        }

        [TestMethod]
        public void FactorySetsWindowsphoneTargetHeaderToToastForToast()
        {
            WebRequest request = WebRequestFactory.CreatePhoneRequest(Uri, PayloadLength, NotificationType.Toast);

            Assert.AreEqual("toast", request.Headers[WebRequestFactory.WindowsphoneTargetHeader]);
        }

        [TestMethod]
        public void FactorySetsWindowsphoneTargetHeaderToTokenForTile()
        {
            WebRequest request = WebRequestFactory.CreatePhoneRequest(Uri, PayloadLength, NotificationType.Tile);

            Assert.AreEqual("token", request.Headers[WebRequestFactory.WindowsphoneTargetHeader]);
        }

        [TestMethod]
        public void FactorySetsMessageIdHeader()
        {
            WebRequest request = WebRequestFactory.CreatePhoneRequest(Uri, PayloadLength, NotificationType.Tile, "messageId");

            Assert.AreEqual("messageId", request.Headers[WebRequestFactory.MessageIdHeader]);
        }
    }
}