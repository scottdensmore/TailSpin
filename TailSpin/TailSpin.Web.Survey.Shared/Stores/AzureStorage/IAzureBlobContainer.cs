




 




namespace TailSpin.Web.Survey.Shared.Stores.AzureStorage
{
    using System;
    using System.Collections.Generic;

    public interface IAzureBlobContainer<T>
    {
        void EnsureExist();
        void Save(string objId, T obj);
        T Get(string objId);
        IEnumerable<T> GetAll();
        Uri GetUri(string objId);
        void Delete(string objId);
        void DeleteContainer();
    }
}