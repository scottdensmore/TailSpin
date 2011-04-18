




 




namespace TailSpin.Workers.Notifications.Tests
{
    using System.IO;
    using System.Xml;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TailSpin.Web.Survey.Shared.Notifications;

    [TestClass]
    public class ToastNotificationPayloadBuilderFixture
    {
        [TestMethod]
        public void CreateSetsTheText1ForWindowsPhoneToastNotification()
        {
            string text1 = "text1";
            byte[] payload = ToastNotificationPayloadBuilder.Create(text1);
            var doc = new XmlDocument();
            using (var memoryStream = new MemoryStream(payload))
            {
                doc.Load(memoryStream);
            }
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("wp", "WPNotification");
            XmlNode node = doc.SelectSingleNode("wp:Notification/wp:Toast/wp:Text1", nsmgr);

            string text = node.InnerText;

            Assert.AreEqual(text1, text);
        }

        [TestMethod]
        public void CreateSetsTheText2ForWindowsPhoneToastNotification()
        {
            string text2 = "text2";
            byte[] payload = ToastNotificationPayloadBuilder.Create(string.Empty, text2);
            var doc = new XmlDocument();
            using (var memoryStream = new MemoryStream(payload))
            {
                doc.Load(memoryStream);
            }
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("wp", "WPNotification");
            XmlNode node = doc.SelectSingleNode("wp:Notification/wp:Toast/wp:Text2", nsmgr);

            string text = node.InnerText;

            Assert.AreEqual(text2, text);
        }
    }
}