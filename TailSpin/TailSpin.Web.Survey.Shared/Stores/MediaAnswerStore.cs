




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using System;
    using System.Globalization;
    using System.IO;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public class MediaAnswerStore : IMediaAnswerStore
    {
        private readonly IAzureBlobContainer<byte[]> voiceAnswerBlobContainer;
        private readonly IAzureBlobContainer<byte[]> pictureAnswerBlobContainer;

        public MediaAnswerStore(IAzureBlobContainer<byte[]> voiceAnswerBlobContainer, IAzureBlobContainer<byte[]> pictureAnswerBlobContainer)
        {
            this.voiceAnswerBlobContainer = voiceAnswerBlobContainer;
            this.pictureAnswerBlobContainer = pictureAnswerBlobContainer;
        }

        public void Initialize()
        {
            this.voiceAnswerBlobContainer.EnsureExist();
            this.pictureAnswerBlobContainer.EnsureExist();
        }

        public string SaveMediaAnswer(Stream media, QuestionType questionType)
        {
            var mediaBytes = ReadAllBytes(media);

            var id = Guid.NewGuid().ToString();
            switch (questionType)
            {
                case QuestionType.Picture:
                    var pictureId = string.Format(CultureInfo.InvariantCulture, "{0}.jpg", id);
                    this.pictureAnswerBlobContainer.Save(pictureId, mediaBytes);
                    return this.pictureAnswerBlobContainer.GetUri(pictureId).ToString();
                case QuestionType.Voice:
                    var voiceId = string.Format(CultureInfo.InvariantCulture, "{0}.wav", id);
                    this.voiceAnswerBlobContainer.Save(voiceId, mediaBytes);
                    return this.voiceAnswerBlobContainer.GetUri(voiceId).ToString();
                default:
                    throw new ArgumentOutOfRangeException("questionType", @"Can only save picture and voice answers.");
            }
        }

        public void DeleteMediaAnswer(string id, QuestionType questionType)
        {
            switch (questionType)
            {
                case QuestionType.Picture:
                    this.pictureAnswerBlobContainer.Delete(id);
                    break;
                case QuestionType.Voice:
                    this.voiceAnswerBlobContainer.Delete(id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("questionType", @"Can only save picture and voice answers.");
            }
        }

        private static byte[] ReadAllBytes(Stream input)
        {
            var readBuffer = new byte[4096];

            int totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = input.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = input.ReadByte();
                    if (nextByte != -1)
                    {
                        var temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }

            return buffer;
        }
    }
}