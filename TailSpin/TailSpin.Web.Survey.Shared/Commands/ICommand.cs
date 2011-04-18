




 




namespace TailSpin.Web.Survey.Shared.Commands
{
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public interface ICommand<in T> where T : AzureQueueMessage
    {
        void Run(T message);
    }
}