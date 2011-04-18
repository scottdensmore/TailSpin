




 




namespace TailSpin.Web.Survey.Shared.Notifications
{
    using System.IO;
    using System.Text;
    using System.Xml;

    public static class ToastNotificationPayloadBuilder
    {
        public static byte[] Create(string text1, string text2 = null)
        {
            using (var stream = new MemoryStream())
            {
                var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    if (writer != null)
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("wp", "Notification", "WPNotification");
                        writer.WriteStartElement("wp", "Toast", "WPNotification");
                        writer.WriteStartElement("wp", "Text1", "WPNotification");
                        writer.WriteValue(text1);
                        writer.WriteEndElement();
                        writer.WriteStartElement("wp", "Text2", "WPNotification");
                        writer.WriteValue(text2);
                        writer.WriteEndElement();
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