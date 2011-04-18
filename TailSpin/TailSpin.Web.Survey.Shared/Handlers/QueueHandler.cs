




 




namespace TailSpin.Web.Survey.Shared.Handlers
{
    using System;
    using System.Threading.Tasks;
    using TailSpin.Web.Survey.Shared.Commands;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public class QueueHandler<T> : GenericQueueHandler<T> where T : AzureQueueMessage
    {
        private readonly IAzureQueue<T> queue;
        private TimeSpan interval;

        protected QueueHandler(IAzureQueue<T> queue)
        {
            this.queue = queue;
            this.interval = TimeSpan.FromMilliseconds(200);
        }

        public static QueueHandler<T> For(IAzureQueue<T> queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException("queue");
            }

            return new QueueHandler<T>(queue);
        }

        public QueueHandler<T> Every(TimeSpan intervalBetweenRuns)
        {
            this.interval = intervalBetweenRuns;

            return this;
        }

        public virtual void Do(ICommand<T> command)
        {
            Task.Factory.StartNew(
                () =>
                    {
                        while (true)
                        {
                            this.Cycle(command);
                        }
                    },
                TaskCreationOptions.LongRunning);
        }

        protected void Cycle(ICommand<T> command)
        {
            try
            {
                ProcessMessages(this.queue, this.queue.GetMessages(1), command.Run);

                this.Sleep(this.interval);
            }
            catch (TimeoutException)
            {
            }
        }
    }
}