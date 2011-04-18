




 




namespace TailSpin.Web.Survey.Shared.Handlers
{
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public static class BatchProcessingQueueHandler
    {
        public static BatchProcessingQueueHandler<T> For<T>(IAzureQueue<T> queue) where T : AzureQueueMessage
        {
            return BatchProcessingQueueHandler<T>.For(queue);
        }
    }
}