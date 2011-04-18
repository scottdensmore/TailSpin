




 




namespace TailSpin.Web.Survey.Shared.Tests.SurveysFiltering
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;

    [TestClass]
    public class TenantFilterStoreFixture
    {
        [TestMethod]
        public void GetTenantsFiltersByUsernameInRowKey()
        {
            var mockTenantFilterTable = new Mock<IAzureTable<TenantFilterRow>>();
            var store = new TenantFilterStore(mockTenantFilterTable.Object);
            var row1 = new TenantFilterRow { RowKey = "username", PartitionKey = "tenant-to-return" };
            var row2 = new TenantFilterRow { RowKey = "other-username", PartitionKey = "other-tenant" };
            mockTenantFilterTable.SetupGet(t => t.Query).Returns(new[] { row1, row2 } .AsQueryable());

            var tenants = store.GetTenants("username");

            Assert.AreSame("tenant-to-return", tenants.Single());
        }

        [TestMethod]
        public void SetTenantsDeletesAllRowsFromUser()
        {
            var mockTenantFilterTable = new Mock<IAzureTable<TenantFilterRow>>();
            var store = new TenantFilterStore(mockTenantFilterTable.Object);
            var rowToDelete = new TenantFilterRow { RowKey = "username" };
            var otherRow = new TenantFilterRow { RowKey = "other-username" };
            mockTenantFilterTable.SetupGet(t => t.Query).Returns(new[] { rowToDelete, otherRow } .AsQueryable());
            IEnumerable<TenantFilterRow> rows = null;
            mockTenantFilterTable.Setup(t => t.Delete(It.IsAny<IEnumerable<TenantFilterRow>>()))
                                 .Callback<IEnumerable<TenantFilterRow>>(r => rows = r);

            store.SetTenants("username", new string[0]);

            Assert.AreSame(rowToDelete, rows.Single());
        }

        [TestMethod]
        public void SetTenantsAddOneRowPerTenant()
        {
            var mockTenantFilterTable = new Mock<IAzureTable<TenantFilterRow>>();
            var store = new TenantFilterStore(mockTenantFilterTable.Object);
            IEnumerable<TenantFilterRow> rows = null;
            mockTenantFilterTable.Setup(t => t.Add(It.IsAny<IEnumerable<TenantFilterRow>>()))
                                 .Callback<IEnumerable<TenantFilterRow>>(r => rows = r);

            store.SetTenants("username", new[] { "Tenant1", "Tenant2" });

            Assert.IsNotNull(rows.Single(r => r.PartitionKey == "tenant1" && r.RowKey == "username"));
            Assert.IsNotNull(rows.Single(r => r.PartitionKey == "tenant2" && r.RowKey == "username"));
        }

        [TestMethod]
        public void GetUsersFiltersByTenantInPartitionKey()
        {
            var mockTenantFilterTable = new Mock<IAzureTable<TenantFilterRow>>();
            var store = new TenantFilterStore(mockTenantFilterTable.Object);
            var row1 = new TenantFilterRow { RowKey = "username", PartitionKey = "tenant" };
            var row2 = new TenantFilterRow { RowKey = "username", PartitionKey = "other-tenant" };
            mockTenantFilterTable.SetupGet(t => t.Query).Returns(new[] { row1, row2 } .AsQueryable());

            var users = store.GetUsers("tenant");

            Assert.AreSame("username", users.Single());
        }
    }
}