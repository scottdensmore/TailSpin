




 




namespace TailSpin.Web.Survey.Shared.Handlers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TailSpin.Web.Survey.Shared.Commands;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public class BatchProcessingQueueHandler<T> : GenericQueueHandler<T> where T : AzureQueueMessage
    {
        private readonly IAzureQueue<T> queue;
        private TimeSpan interval;

        protected BatchProcessingQueueHandler(IAzureQueue<T> queue)
        {
            this.queue = queue;
            this.interval = TimeSpan.FromMilliseconds(200);
        }

        public static BatchProcessingQueueHandler<T> For(IAzureQueue<T> queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException("queue");
            }

            return new BatchProcessingQueueHandler<T>(queue);
        }

        public BatchProcessingQueueHandler<T> Every(TimeSpan intervalBetweenRuns)
        {
            this.interval = intervalBetweenRuns;

            return this;
        }

        public virtual void Do(IBatchCommand<T> batchCommand)
        {
            Task.Factory.StartNew(
                () =>
                    {
                        while (true)
                        {
                            this.Cycle(batchCommand);
                        }
                    },
                TaskCreationOptions.LongRunning);
        }

        protected void Cycle(IBatchCommand<T> batchCommand)
        {
            try
            {
                batchCommand.PreRun();

                bool continueProcessing;
                do
                {
                    var messages = this.queue.GetMessages(32);
                    ProcessMessages(this.queue, messages, batchCommand.Run);

                    continueProcessing = messages.Count() > 0;
                } 
                while (continueProcessing);
                
                batchCommand.PostRun();

                this.Sleep(this.interval);
            }
            catch (TimeoutException)
            {
            }
        }
    }
}