




 




namespace TailSpin.Web.Survey.Shared.Tests.Stores
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Shared.Stores;
    using Shared.Stores.AzureStorage;
    using TailSpin.Web.Survey.Shared.Models;

    [TestClass]
    public class MediaAnswerStoreFixture
    {
        [TestMethod]
        public void AddMediaAnswerStoresVoiceStream()
        {
            var mockVoiceAnswerBlobContainer = new Mock<IAzureBlobContainer<byte[]>>();
            mockVoiceAnswerBlobContainer.Setup(s => s.GetUri(It.IsAny<string>())).Returns(new Uri("http://uri/"));
            var store = new MediaAnswerStore(mockVoiceAnswerBlobContainer.Object, new Mock<IAzureBlobContainer<byte[]>>().Object);
            var bytes = new byte[0];
            Stream stream = new MemoryStream(bytes);

            store.SaveMediaAnswer(stream, QuestionType.Voice);

            mockVoiceAnswerBlobContainer.Verify(s => s.Save(It.IsAny<string>(), bytes), Times.Once());
        }

        [TestMethod]
        public void AddMediaAnswerGetsBlobUriAndReturnsItForVoice()
        {
            var mockVoiceAnswerBlobContainer = new Mock<IAzureBlobContainer<byte[]>>();
            mockVoiceAnswerBlobContainer.Setup(s => s.GetUri(It.IsAny<string>())).Returns(new Uri("http://uri/"));
            var store = new MediaAnswerStore(mockVoiceAnswerBlobContainer.Object, new Mock<IAzureBlobContainer<byte[]>>().Object);
            var bytes = new byte[0];
            Stream stream = new MemoryStream(bytes);

            var actual = store.SaveMediaAnswer(stream, QuestionType.Voice);

            Assert.AreEqual("http://uri/", actual);
        }

        [TestMethod]
        public void AddMediaAnswerStoresPictureStream()
        {
            var mockPictureAnswerBlobContainer = new Mock<IAzureBlobContainer<byte[]>>();
            mockPictureAnswerBlobContainer.Setup(s => s.GetUri(It.IsAny<string>())).Returns(new Uri("http://uri/"));
            var store = new MediaAnswerStore(new Mock<IAzureBlobContainer<byte[]>>().Object, mockPictureAnswerBlobContainer.Object);
            var bytes = new byte[0];
            Stream stream = new MemoryStream(bytes);

            store.SaveMediaAnswer(stream, QuestionType.Picture);

            mockPictureAnswerBlobContainer.Verify(s => s.Save(It.IsAny<string>(), bytes), Times.Once());
        }

        [TestMethod]
        public void AddMediaAnswerGetsBlobUriAndReturnsItForPicture()
        {
            var mockPictureAnswerBlobContainer = new Mock<IAzureBlobContainer<byte[]>>();
            mockPictureAnswerBlobContainer.Setup(s => s.GetUri(It.IsAny<string>())).Returns(new Uri("http://uri/"));
            var store = new MediaAnswerStore(new Mock<IAzureBlobContainer<byte[]>>().Object, mockPictureAnswerBlobContainer.Object);
            var bytes = new byte[0];
            Stream stream = new MemoryStream(bytes);

            var actual = store.SaveMediaAnswer(stream, QuestionType.Picture);

            Assert.AreEqual("http://uri/", actual);
        }
    }
}