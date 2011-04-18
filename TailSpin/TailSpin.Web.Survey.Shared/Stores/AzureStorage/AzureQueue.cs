




 




namespace TailSpin.Web.Survey.Shared.Stores.AzureStorage
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    public class AzureQueue<T> : IAzureQueue<T> where T : AzureQueueMessage
    {
        private readonly CloudStorageAccount account;
        private readonly TimeSpan visibilityTimeout;
        private readonly CloudQueue queue;

        public AzureQueue(CloudStorageAccount account)
            : this(account, typeof(T).Name.ToLowerInvariant())
        {
        }

        public AzureQueue(CloudStorageAccount account, string queueName)
            : this(account, queueName, TimeSpan.FromSeconds(30))
        {
        }

        public AzureQueue(CloudStorageAccount account, string queueName, TimeSpan visibilityTimeout)
        {
            this.account = account;
            this.visibilityTimeout = visibilityTimeout;

            var client = this.account.CreateCloudQueueClient();

            this.queue = client.GetQueueReference(queueName);
        }

        public void AddMessage(T message)
        {
            string serializedMessage = new JavaScriptSerializer().Serialize(message);
            this.queue.AddMessage(new CloudQueueMessage(serializedMessage));
        }
        
        public T GetMessage()
        {
            var message = this.queue.GetMessage(this.visibilityTimeout);

            if (message == null)
            {
                return default(T);
            }

            return GetDeserializedMessage(message);
        }

        public IEnumerable<T> GetMessages(int maxMessagesToReturn)
        {
            var messages = this.queue.GetMessages(maxMessagesToReturn, this.visibilityTimeout);

            foreach (var message in messages)
            {
                yield return GetDeserializedMessage(message);
            }
        }

        public void EnsureExist()
        {
            this.queue.CreateIfNotExist();
        }

        public void Clear()
        {
            this.queue.Clear();
        }

        public void DeleteMessage(T message)
        {
            this.queue.DeleteMessage(message.Id, message.PopReceipt);
        }

        private static T GetDeserializedMessage(CloudQueueMessage message)
        {
            var deserializedMessage = new JavaScriptSerializer().Deserialize<T>(message.AsString);
            deserializedMessage.Id = message.Id;
            deserializedMessage.PopReceipt = message.PopReceipt;
            deserializedMessage.DequeueCount = message.DequeueCount;

            return deserializedMessage;
        }
    }
}