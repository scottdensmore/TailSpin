




 




namespace TailSpin.Web.Survey.Shared.Stores.AzureStorage
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure.StorageClient;

    public interface IAzureTable<T> where T : TableServiceEntity
    {
        IQueryable<T> Query { get; }
        void EnsureExist();
        void Add(T obj);
        void Add(IEnumerable<T> objs);
        void AddOrUpdate(T obj);
        void AddOrUpdate(IEnumerable<T> objs);
        void Delete(T obj);
        void Delete(IEnumerable<T> objs);
    }
}