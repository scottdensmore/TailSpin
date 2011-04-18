




 




namespace TailSpin.Workers.Notifications.Tests
{
    using System;
    using System.IO;
    using System.Xml;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TailSpin.Web.Survey.Shared.Notifications;

    [TestClass]
    public class TileNotificationPayloadBuilderFixture
    {
        [TestMethod]
        public void CreateSetsTheTitleForWindowsPhoneTileNotification()
        {
            string title = "title";
            byte[] payload = TileNotificationPayloadBuilder.Create(title);
            var doc = new XmlDocument();
            using (var memoryStream = new MemoryStream(payload))
            {
                doc.Load(memoryStream);
            }
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("wp", "WPNotification");
            XmlNode node = doc.SelectSingleNode("wp:Notification/wp:Tile/wp:Title", nsmgr);

            string text = node.InnerText;

            Assert.AreEqual(title, text);
        }

        [TestMethod]
        public void CreateSetsTheBackgroundImageForWindowsPhoneTileNotification()
        {
            string backgroundImage = "backgroundImage";
            byte[] payload = TileNotificationPayloadBuilder.Create(string.Empty, backgroundImage);
            var doc = new XmlDocument();
            using (var memoryStream = new MemoryStream(payload))
            {
                doc.Load(memoryStream);
            }
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("wp", "WPNotification");
            XmlNode node = doc.SelectSingleNode("wp:Notification/wp:Tile/wp:BackgroundImage", nsmgr);

            string text = node.InnerText;

            Assert.AreEqual(backgroundImage, text);
        }

        [TestMethod]
        public void CreateSetsTheCountForWindowsPhoneTileNotification()
        {
            int count = 2;
            byte[] payload = TileNotificationPayloadBuilder.Create(string.Empty, string.Empty, count);
            var doc = new XmlDocument();
            using (var memoryStream = new MemoryStream(payload))
            {
                doc.Load(memoryStream);
            }
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("wp", "WPNotification");
            XmlNode node = doc.SelectSingleNode("wp:Notification/wp:Tile/wp:Count", nsmgr);

            string text = node.InnerText;

            Assert.AreEqual(count, Convert.ToInt32(text));
        }
    }
}