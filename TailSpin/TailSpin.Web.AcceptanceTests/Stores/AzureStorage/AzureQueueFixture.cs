




 




namespace TailSpin.Web.AcceptanceTests.Stores.AzureStorage
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Survey.Shared;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    [TestClass]
    public class AzureQueueFixture
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var newSurveyAnswerQueue = new AzureQueue<MessageForTests>(account);
            newSurveyAnswerQueue.EnsureExist();
        }

        [TestMethod]
        public void AddAndGetMessage()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var queue = new AzureQueue<MessageForTests>(account);
            var message = new MessageForTests { Content = "content" };

            queue.AddMessage(message);
            var actualMessage = queue.GetMessage();

            Assert.AreEqual(message.Content, actualMessage.Content);
        }

        [TestMethod]
        public void PurgeAndGetMessageReturnNull()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var queue = new AzureQueue<MessageForTests>(account);
            var message = new MessageForTests { Content = "content" };

            queue.AddMessage(message);
            queue.Clear();
            var actualMessage = queue.GetMessage();

            Assert.IsNull(actualMessage);
        }

        [TestMethod]
        public void AddAndGetMessages()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var newSurveyAnswerQueue = new AzureQueue<MessageForTests>(account);
            int maxMessagesToReturn = 2;

            newSurveyAnswerQueue.AddMessage(new MessageForTests());
            newSurveyAnswerQueue.AddMessage(new MessageForTests());
            var actualMessages = newSurveyAnswerQueue.GetMessages(maxMessagesToReturn);

            Assert.AreEqual(2, actualMessages.Count());
        }

        [TestMethod]
        public void AddAndGetAndDeleteMessage()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var queue = new AzureQueue<MessageForTests>(account);
            var message = new MessageForTests { Content = "content" };

            queue.Clear();
            queue.AddMessage(message);
            var addedMessage = queue.GetMessage();
            queue.DeleteMessage(addedMessage);
            var actualMessage = queue.GetMessage();

            Assert.IsNull(actualMessage);
        }

        private class MessageForTests : AzureQueueMessage
        {
            public string Content { get; set; }
        }
    }
}
