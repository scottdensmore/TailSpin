




 




namespace TailSpin.Web.Survey.Shared.SurveysFiltering
{
    using System.Collections.Generic;
    using System.Linq;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public class TenantFilterStore : ITenantFilterStore
    {
        private readonly IAzureTable<TenantFilterRow> tenantFilterTable;

        public TenantFilterStore(IAzureTable<TenantFilterRow> tenantFilterTable)
        {
            this.tenantFilterTable = tenantFilterTable;
        }

        public void Initialize()
        {
            this.tenantFilterTable.EnsureExist();
        }

        public IEnumerable<string> GetTenants(string username)
        {
            return
                (from r in this.tenantFilterTable.Query
                 where r.RowKey == username
                 select r).ToList().Select(r => r.PartitionKey);
        }

        public void SetTenants(string username, IEnumerable<string> tenants)
        {
            var rowsToDelete = 
                (from r in this.tenantFilterTable.Query
                 where r.RowKey == username
                 select r).ToList();

            this.tenantFilterTable.Delete(rowsToDelete);

            var rowsToAdd = tenants.Select(t => new TenantFilterRow { PartitionKey = t.ToLowerInvariant(), RowKey = username });
            this.tenantFilterTable.Add(rowsToAdd);
        }

        public IEnumerable<string> GetUsers(string tenant)
        {
            return 
                (from r in this.tenantFilterTable.Query
                 where r.PartitionKey == tenant
                 select r).ToList().Select(r => r.RowKey);
        }
    }
}