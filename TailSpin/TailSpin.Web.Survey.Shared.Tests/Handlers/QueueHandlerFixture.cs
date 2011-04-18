




 




namespace TailSpin.Web.Survey.Shared.Tests.Handlers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Survey.Shared.Commands;
    using TailSpin.Web.Survey.Shared.Handlers;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    [TestClass]
    public class QueueHandlerFixture
    {
        [TestMethod]
        public void ForCreatesHandlerForGivenQueue()
        {
            var mockQueue = new Mock<IAzureQueue<StubMessage>>();

            var queueHandler = QueueHandler.For(mockQueue.Object);

            Assert.IsInstanceOfType(queueHandler, typeof(QueueHandler<StubMessage>));
        }

        [TestMethod]
        public void EveryReturnsSameHandlerForGivenQueue()
        {
            var mockQueue = new Mock<IAzureQueue<StubMessage>>();
            var queueHandler = new QueueHandlerStub(mockQueue.Object);

            var returnedQueueHandler = queueHandler.Every(TimeSpan.Zero);

            Assert.AreSame(queueHandler, returnedQueueHandler);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForThrowsWhenQueueIsNull()
        {
            QueueHandler.For(default(IAzureQueue<StubMessage>));
        }

        [TestMethod]
        public void DoRunsGivenCommandForEachMessage()
        {
            var message1 = new StubMessage();
            var message2 = new StubMessage();
            var mockQueue = new Mock<IAzureQueue<StubMessage>>();
            mockQueue.Setup(q => q.GetMessages(1)).Returns(() => new[] { message1, message2 });
            var command = new Mock<ICommand<StubMessage>>();
            var queueHandler = new QueueHandlerStub(mockQueue.Object);

            queueHandler.Do(command.Object);

            command.Verify(c => c.Run(It.IsAny<StubMessage>()), Times.Exactly(2));
            command.Verify(c => c.Run(message1));
            command.Verify(c => c.Run(message2));
        }

        [TestMethod]
        public void DoDeletesMessageWhenRunIsSuccessfull()
        {
            var message = new StubMessage();
            var mockQueue = new Mock<IAzureQueue<StubMessage>>();
            mockQueue.Setup(q => q.GetMessages(1)).Returns(() => new[] { message });
            var command = new Mock<ICommand<StubMessage>>();
            var queueHandler = new QueueHandlerStub(mockQueue.Object);

            queueHandler.Do(command.Object);

            mockQueue.Verify(q => q.DeleteMessage(message));
        }

        [TestMethod]
        public void DoDeletesMessageWhenRunIsNotSuccessfullAndMessageHasBeenDequeuedMoreThanFiveTimes()
        {
            var message = new StubMessage { DequeueCount = 6 };
            var mockQueue = new Mock<IAzureQueue<StubMessage>>();
            mockQueue.Setup(q => q.GetMessages(1)).Returns(() => new[] { message });
            var command = new Mock<ICommand<StubMessage>>();
            command.Setup(c => c.Run(It.IsAny<StubMessage>())).Throws(new Exception("This will cause the command to fail"));
            var queueHandler = new QueueHandlerStub(mockQueue.Object);

            queueHandler.Do(command.Object);

            mockQueue.Verify(q => q.DeleteMessage(message));
        }

        public class StubMessage : AzureQueueMessage
        {
        }

        private class QueueHandlerStub : QueueHandler<StubMessage>
        {
            public QueueHandlerStub(IAzureQueue<StubMessage> queue)
                : base(queue)
            {
            }

            public override void Do(ICommand<StubMessage> batchCommand)
            {
                this.Cycle(batchCommand);
            }
        }
    }
}