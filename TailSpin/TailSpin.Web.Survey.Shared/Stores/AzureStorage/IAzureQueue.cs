




 




namespace TailSpin.Web.Survey.Shared.Stores.AzureStorage
{
    using System.Collections.Generic;

    public interface IAzureQueue<T> where T : AzureQueueMessage
    {
        void EnsureExist();
        void Clear();
        void AddMessage(T message);
        T GetMessage();
        IEnumerable<T> GetMessages(int maxMessagesToReturn);
        void DeleteMessage(T message);
    }
}