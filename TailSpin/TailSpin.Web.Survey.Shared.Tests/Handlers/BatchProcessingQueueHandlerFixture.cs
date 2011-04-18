




 




namespace TailSpin.Web.Survey.Shared.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Survey.Shared.Commands;
    using TailSpin.Web.Survey.Shared.Handlers;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    [TestClass]
    public class BatchProcessingQueueHandlerFixture
    {
        [TestMethod]
        public void ForCreatesHandlerForGivenQueue()
        {
            var mockQueue = new Mock<IAzureQueue<StubMessage>>();

            var queueHandler = BatchProcessingQueueHandler.For(mockQueue.Object);

            Assert.IsInstanceOfType(queueHandler, typeof(BatchProcessingQueueHandler<StubMessage>));
        }

        [TestMethod]
        public void EveryReturnsSameHandlerForGivenQueue()
        {
            var mockQueue = new Mock<IAzureQueue<StubMessage>>();
            var queueHandler = new BatchProcessingQueueHandlerStub(mockQueue.Object);

            var returnedQueueHandler = queueHandler.Every(TimeSpan.Zero);

            Assert.AreSame(queueHandler, returnedQueueHandler);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForThrowsWhenQueueIsNull()
        {
            BatchProcessingQueueHandler.For(default(IAzureQueue<StubMessage>));
        }

        [TestMethod]
        public void DoCallsPreRunForBatch()
        {
            var message1 = new StubMessage();
            var message2 = new StubMessage();
            var mockQueue = new Mock<IAzureQueue<StubMessage>>();
            var queue = new Queue<IEnumerable<StubMessage>>();
            queue.Enqueue(new[] { message1, message2 });
            mockQueue.Setup(q => q.GetMessages(32)).Returns(() => queue.Count > 0 ? queue.Dequeue() : new StubMessage[] { });
            var command = new Mock<IBatchCommand<StubMessage>>();
            var queueHandler = new BatchProcessingQueueHandlerStub(mockQueue.Object);

            queueHandler.Do(command.Object);

            command.Verify(c => c.PreRun(), Times.Once());
        }

        [TestMethod]
        public void DoCallsPostRunForBatch()
        {
            var message1 = new StubMessage();
            var message2 = new StubMessage();
            var mockQueue = new Mock<IAzureQueue<StubMessage>>();
            var queue = new Queue<IEnumerable<StubMessage>>();
            queue.Enqueue(new[] { message1, message2 });
            mockQueue.Setup(q => q.GetMessages(32)).Returns(() => queue.Count > 0 ? queue.Dequeue() : new StubMessage[] { });
            var command = new Mock<IBatchCommand<StubMessage>>();
            var queueHandler = new BatchProcessingQueueHandlerStub(mockQueue.Object);

            queueHandler.Do(command.Object);

            command.Verify(c => c.PostRun(), Times.Once());
        }

        [TestMethod]
        public void DoRunsGivenCommandForEachMessage()
        {
            var message1 = new StubMessage();
            var message2 = new StubMessage();
            var mockQueue = new Mock<IAzureQueue<StubMessage>>();
            var queue = new Queue<IEnumerable<StubMessage>>();
            queue.Enqueue(new[] { message1, message2 });
            mockQueue.Setup(q => q.GetMessages(32)).Returns(() => queue.Count > 0 ? queue.Dequeue() : new StubMessage[] { });
            var command = new Mock<IBatchCommand<StubMessage>>();
            var queueHandler = new BatchProcessingQueueHandlerStub(mockQueue.Object);

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
            var queue = new Queue<IEnumerable<StubMessage>>();
            queue.Enqueue(new[] { message });
            mockQueue.Setup(q => q.GetMessages(32)).Returns(() => queue.Count > 0 ? queue.Dequeue() : new StubMessage[] { });
            var command = new Mock<IBatchCommand<StubMessage>>();
            var queueHandler = new BatchProcessingQueueHandlerStub(mockQueue.Object);
            
            queueHandler.Do(command.Object);

            mockQueue.Verify(q => q.DeleteMessage(message));
        }

        [TestMethod]
        public void DoDeletesMessageWhenRunIsNotSuccessfullAndMessageHasBeenDequeuedMoreThanFiveTimes()
        {
            var message = new StubMessage { DequeueCount = 6 };
            var mockQueue = new Mock<IAzureQueue<StubMessage>>();
            var queue = new Queue<IEnumerable<StubMessage>>();
            queue.Enqueue(new[] { message });
            mockQueue.Setup(q => q.GetMessages(32)).Returns(() => queue.Count > 0 ? queue.Dequeue() : new StubMessage[] { });
            var command = new Mock<IBatchCommand<StubMessage>>();
            command.Setup(c => c.Run(It.IsAny<StubMessage>())).Throws(new Exception("This will cause the command to fail"));
            var queueHandler = new BatchProcessingQueueHandlerStub(mockQueue.Object);

            queueHandler.Do(command.Object);

            mockQueue.Verify(q => q.DeleteMessage(message));
        }

        public class StubMessage : AzureQueueMessage
        {
        }

        private class BatchProcessingQueueHandlerStub : BatchProcessingQueueHandler<StubMessage>
        {
            public BatchProcessingQueueHandlerStub(IAzureQueue<StubMessage> queue)
                : base(queue)
            {
            }

            public override void Do(IBatchCommand<StubMessage> batchCommand)
            {
                this.Cycle(batchCommand);
            }
        }
    }
}
