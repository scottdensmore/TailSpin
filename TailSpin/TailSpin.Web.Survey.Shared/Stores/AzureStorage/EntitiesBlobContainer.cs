




 




namespace TailSpin.Web.Survey.Shared.Stores.AzureStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Script.Serialization;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    public class EntitiesBlobContainer<T> : IAzureBlobContainer<T>
    {
        private readonly CloudStorageAccount account;
        private readonly CloudBlobContainer container;

        public EntitiesBlobContainer(CloudStorageAccount account)
            : this(account, typeof(T).Name.ToLowerInvariant())
        {
        }

        public EntitiesBlobContainer(CloudStorageAccount account, string containerName)
        {
            this.account = account;

            var client = this.account.CreateCloudBlobClient();
            client.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));

            this.container = client.GetContainerReference(containerName);
        }

        public void EnsureExist()
        {
            this.container.CreateIfNotExist();
        }

        public void Save(string objId, T obj)
        {
            CloudBlob blob = this.container.GetBlobReference(objId);
            blob.Properties.ContentType = "application/json";
            var serializer = new JavaScriptSerializer();
            blob.UploadText(serializer.Serialize(obj));
        }

        public T Get(string objId)
        {
            CloudBlob blob = this.container.GetBlobReference(objId);
            try
            {
                var serializer = new JavaScriptSerializer();
                return serializer.Deserialize<T>(blob.DownloadText());
            }
            catch (StorageClientException)
            {
                return default(T);
            }
        }

        public IEnumerable<T> GetAll()
        {
            IEnumerable<IListBlobItem> blobItems = this.container.ListBlobs();

            return blobItems.Select(blobItem => this.Get(blobItem.Uri.ToString())).ToList();
        }

        public Uri GetUri(string objId)
        {
            return new Uri(objId);
        }

        public void Delete(string objId)
        {
            CloudBlob blob = this.container.GetBlobReference(objId);
            blob.DeleteIfExists();
        }

        public void DeleteContainer()
        {
            try
            {
                this.container.Delete();
            }
            catch (StorageClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return;
                }

                throw;
            }
        }
    }
}