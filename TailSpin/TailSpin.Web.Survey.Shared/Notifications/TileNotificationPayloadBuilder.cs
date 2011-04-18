




 




namespace TailSpin.Web.Survey.Shared.Notifications
{
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml;

    public static class TileNotificationPayloadBuilder
    {
        public static byte[] Create(string title, string backgroundImage = null, int count = 0)
        {
            using (var stream = new MemoryStream())
            {
                var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    if (writer != null)
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("wp", "Notification", "WPNotification");
                        writer.WriteStartElement("wp", "Tile", "WPNotification");
                        writer.WriteStartElement("wp", "BackgroundImage", "WPNotification");
                        writer.WriteValue(backgroundImage ?? string.Empty);
                        writer.WriteEndElement();
                        writer.WriteStartElement("wp", "Count", "WPNotification");
                        writer.WriteValue(count == 0 ? string.Empty : count.ToString(CultureInfo.InvariantCulture));
                        writer.WriteEndElement();
                        writer.WriteStartElement("wp", "Title", "WPNotification");
                        writer.WriteValue(title);
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Close();
                    }

                    byte[] payload = stream.ToArray();
                    return payload;
                }
            }
        }
    }
}