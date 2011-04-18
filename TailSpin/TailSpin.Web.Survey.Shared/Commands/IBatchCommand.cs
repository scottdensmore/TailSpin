




 




namespace TailSpin.Web.Survey.Shared.Commands
{
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public interface IBatchCommand<in T> : ICommand<T> where T : AzureQueueMessage
    {
        void PreRun();
        void PostRun();
    }
}