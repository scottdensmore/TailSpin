




 




namespace TailSpin.Web.AcceptanceTests.Stores.AzureStorage
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.StorageClient;
    using TailSpin.Web.Survey.Shared;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    [TestClass]
    public class AzureTableFixture
    {
        private const string TableName = "tableForTest";

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var azureTable = new AzureTable<TestRow>(account, TableName);
            azureTable.EnsureExist();
        }

        [TestMethod]
        public void DeleteAndAddAndGet()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var azureTable = new AzureTable<TestRow>(account, TableName);
            var row = new TestRow
                                {
                                    PartitionKey = "partition_key_DeleteAndAddAndGet",
                                    RowKey = "row_key_DeleteAndAddAndGet",
                                    Content = "content"
                                };
            
            azureTable.Delete(row);
            TestRow deletedRow = (from o in azureTable.Query
                                  where o.RowKey == row.RowKey
                                  select o).SingleOrDefault();
            Assert.IsNull(deletedRow);

            azureTable.Add(row);
            TestRow savedRow = (from o in azureTable.Query
                                where o.RowKey == row.RowKey &&
                                      o.PartitionKey == "partition_key_DeleteAndAddAndGet" &&
                                              o.Content == "content"
                                select o).SingleOrDefault();
            Assert.IsNotNull(savedRow);

            azureTable.Delete(row);
            TestRow actualRow = (from o in azureTable.Query
                                 where o.RowKey == row.RowKey
                                 select o).SingleOrDefault();
            Assert.IsNull(actualRow);
        }

        [TestMethod]
        public void SaveManyAndDeleteMany()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var azureTable = new AzureTable<TestRow>(account, TableName);
            var row1 = new TestRow { PartitionKey = "partition_key_SaveManyAndDeleteMany", RowKey = "row_key_1_SaveManyAndDeleteMany" };
            var row2 = new TestRow { PartitionKey = "partition_key_SaveManyAndDeleteMany", RowKey = "row_key_2_SaveManyAndDeleteMany" };

            azureTable.Delete(new[] { row1, row2 });
            var rowsToDelete = (from o in azureTable.Query
                                where o.PartitionKey == "partition_key_SaveManyAndDeleteMany"
                                select o).ToList();
            Assert.AreEqual(0, rowsToDelete.Count);

            azureTable.Add(new[] { row1, row2 });
            var insertedRows = (from o in azureTable.Query
                                where o.PartitionKey == "partition_key_SaveManyAndDeleteMany"
                                select o).ToList();
            Assert.AreEqual(2, insertedRows.Count);

            azureTable.Delete(new[] { row1, row2 });
            var actualRows = (from o in azureTable.Query
                              where o.PartitionKey == "partition_key_SaveManyAndDeleteMany"
                              select o).ToList();
            Assert.AreEqual(0, actualRows.Count);
        }

        [TestMethod]
        public void AddOrUpdateAddsWhenNotExists()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var azureTable = new AzureTable<TestRow>(account, TableName);
            var row = new TestRow
            {
                PartitionKey = "partition_key_AddOrUpdateAddsWhenNotExists",
                RowKey = "row_key_AddOrUpdateAddsWhenNotExists",
                Content = "content"
            };

            azureTable.Delete(row);
            TestRow deletedRow = (from o in azureTable.Query
                                  where o.RowKey == row.RowKey
                                  select o).SingleOrDefault();
            Assert.IsNull(deletedRow);

            azureTable.AddOrUpdate(row);
            TestRow savedRow = (from o in azureTable.Query
                                where o.RowKey == row.RowKey &&
                                      o.PartitionKey == "partition_key_AddOrUpdateAddsWhenNotExists" &&
                                      o.Content == "content"
                                select o).SingleOrDefault();
            Assert.IsNotNull(savedRow);
        }

        [TestMethod]
        public void AddOrUpdateUpdatesWhenExists()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var azureTable = new AzureTable<TestRow>(account, TableName);
            var row = new TestRow
            {
                PartitionKey = "partition_key_AddOrUpdateUpdatesWhenExists",
                RowKey = "row_key_AddOrUpdateUpdatesWhenExists",
                Content = "content"
            };

            azureTable.Delete(row);
            TestRow deletedRow = (from o in azureTable.Query
                                  where o.RowKey == row.RowKey
                                  select o).SingleOrDefault();
            Assert.IsNull(deletedRow);

            azureTable.Add(row);
            TestRow savedRow = (from o in azureTable.Query
                                where o.RowKey == row.RowKey &&
                                      o.PartitionKey == "partition_key_AddOrUpdateUpdatesWhenExists" &&
                                      o.Content == "content"
                                select o).SingleOrDefault();
            Assert.IsNotNull(savedRow);

            row.Content = "content modified";
            azureTable.AddOrUpdate(row);
            TestRow updatedRow = (from o in azureTable.Query
                                  where o.RowKey == row.RowKey &&
                                        o.PartitionKey == "partition_key_AddOrUpdateUpdatesWhenExists" &&
                                        o.Content == "content modified"
                                  select o).SingleOrDefault();
            Assert.IsNotNull(updatedRow);
        }

        [TestMethod]
        public void AddOrUpdateMany()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
            var azureTable = new AzureTable<TestRow>(account, TableName);
            var row1 = new TestRow { PartitionKey = "partition_key_AddOrUpdateMany", RowKey = "row_key_1_AddOrUpdateMany", Content = "content 1" };
            var row2 = new TestRow { PartitionKey = "partition_key_AddOrUpdateMany", RowKey = "row_key_2_AddOrUpdateMany", Content = "content 2" };

            azureTable.Delete(new[] { row1, row2 });
            var rowsToDelete = (from o in azureTable.Query
                                where o.PartitionKey == "partition_key_AddOrUpdateMany"
                                select o).ToList();
            Assert.AreEqual(0, rowsToDelete.Count);

            azureTable.Add(row1);
            var actualRows = (from o in azureTable.Query
                              where o.PartitionKey == "partition_key_AddOrUpdateMany"
                              select o).ToList();
            Assert.AreEqual(1, actualRows.Count);

            row1.Content = "content modified";
            azureTable.AddOrUpdate(new[] { row1, row2 });
            var insertedRows = (from o in azureTable.Query
                                where o.PartitionKey == "partition_key_AddOrUpdateMany"
                                select o).ToList();
            Assert.AreEqual(2, insertedRows.Count);

            TestRow updatedRow = (from o in azureTable.Query
                                  where o.RowKey == row1.RowKey &&
                                        o.PartitionKey == "partition_key_AddOrUpdateMany" &&
                                        o.Content == "content modified"
                                  select o).SingleOrDefault();
            Assert.IsNotNull(updatedRow);
        }

        private class TestRow : TableServiceEntity
        {
            public string Content { get; set; }
        }
    }
}
