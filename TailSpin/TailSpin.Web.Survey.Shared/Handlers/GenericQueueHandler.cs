




 




namespace TailSpin.Web.Survey.Shared.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public abstract class GenericQueueHandler<T> where T : AzureQueueMessage
    {
        protected static void ProcessMessages(IAzureQueue<T> queue, IEnumerable<T> messages, Action<T> action)
        {
            if (queue == null)
            {
                throw new ArgumentNullException("queue");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (messages == null)
            {
                throw new ArgumentNullException("messages");
            }

            foreach (var message in messages)
            {
                var success = false;

                try
                {
                    action(message);
                    success = true;
                }
                catch (Exception)
                {
                    success = false;
                }
                finally
                {
                    if (success || message.DequeueCount > 5)
                    {
                        queue.DeleteMessage(message);
                    }
                }
            }
        }

        protected virtual void Sleep(TimeSpan interval)
        {
            Thread.Sleep(interval);
        }
    }
}