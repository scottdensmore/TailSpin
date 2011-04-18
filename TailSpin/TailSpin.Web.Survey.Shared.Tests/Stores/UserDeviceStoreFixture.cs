




 




namespace TailSpin.Web.Survey.Shared.Tests.Stores
{
    using System;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    [TestClass]
    public class UserDeviceStoreFixture
    {
        [TestMethod]
        public void SetUserDeviceSavesUsernameInPartitionKey()
        {
            var mockUserDeviceTable = new Mock<IAzureTable<UserDeviceRow>>();
            var store = new UserDeviceStore(mockUserDeviceTable.Object);
            UserDeviceRow row = null;
            mockUserDeviceTable.Setup(t => t.Add(It.IsAny<UserDeviceRow>())).Callback<UserDeviceRow>(r => row = r);

            store.SetUserDevice("username", new Uri("http://device-uri/"));

            Assert.AreEqual("username", row.PartitionKey);
        }

        [TestMethod]
        public void SetUserDeviceSavesDeviceUriInRowKey()
        {
            var mockUserDeviceTable = new Mock<IAzureTable<UserDeviceRow>>();
            var store = new UserDeviceStore(mockUserDeviceTable.Object);
            UserDeviceRow row = null;
            mockUserDeviceTable.Setup(t => t.Add(It.IsAny<UserDeviceRow>())).Callback<UserDeviceRow>(r => row = r);

            store.SetUserDevice(null, new Uri("http://device-uri/"));

            var encodedUri = Convert.ToBase64String(Encoding.UTF8.GetBytes("http://device-uri/"));
            Assert.AreEqual(encodedUri, row.RowKey);
        }

        [TestMethod]
        public void SetUserDeviceDeltesRowWithDeviceUri()
        {
            var mockUserDeviceTable = new Mock<IAzureTable<UserDeviceRow>>();
            var store = new UserDeviceStore(mockUserDeviceTable.Object);
            var rowToDelete = new UserDeviceRow { RowKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("http://device-uri-to-delete/")) };
            var rowNotToDelete = new UserDeviceRow { RowKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("http://other-device-uri/")) };
            mockUserDeviceTable.SetupGet(s => s.Query).Returns(new[] { rowToDelete, rowNotToDelete } .AsQueryable());

            store.SetUserDevice(null, new Uri("http://device-uri-to-delete/"));

            mockUserDeviceTable.Verify(t => t.Delete(rowToDelete), Times.Once());
        }

        [TestMethod]
        public void SetUserDeviceDoesNotDelteWhenDeviceUriNotInTable()
        {
            var mockUserDeviceTable = new Mock<IAzureTable<UserDeviceRow>>();
            var store = new UserDeviceStore(mockUserDeviceTable.Object);
            var rowToDelete = new UserDeviceRow { RowKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("http://device-uri/")) };
            var rowNotToDelete = new UserDeviceRow { RowKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("http://other-device-uri/")) };
            mockUserDeviceTable.SetupGet(s => s.Query).Returns(new[] { rowToDelete, rowNotToDelete } .AsQueryable());

            store.SetUserDevice(null, new Uri("http://device-uri-to-delete/"));

            mockUserDeviceTable.Verify(t => t.Delete(It.IsAny<UserDeviceRow>()), Times.Never());
        }

        [TestMethod]
        public void RemoveUserDeviceDeltesRowWithDeviceUri()
        {
            var mockUserDeviceTable = new Mock<IAzureTable<UserDeviceRow>>();
            var store = new UserDeviceStore(mockUserDeviceTable.Object);
            var rowToDelete = new UserDeviceRow { RowKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("http://device-uri-to-delete/")) };
            var rowNotToDelete = new UserDeviceRow { RowKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("http://other-device-uri/")) };
            mockUserDeviceTable.SetupGet(s => s.Query).Returns(new[] { rowToDelete, rowNotToDelete } .AsQueryable());

            store.RemoveUserDevice(new Uri("http://device-uri-to-delete/"));

            mockUserDeviceTable.Verify(t => t.Delete(rowToDelete), Times.Once());
        }

        [TestMethod]
        public void RemoveUserDeviceDoesNotDelteWhenDeviceUriNotInTable()
        {
            var mockUserDeviceTable = new Mock<IAzureTable<UserDeviceRow>>();
            var store = new UserDeviceStore(mockUserDeviceTable.Object);
            var rowToDelete = new UserDeviceRow { RowKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("http://device-uri/")) };
            var rowNotToDelete = new UserDeviceRow { RowKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("http://other-device-uri/")) };
            mockUserDeviceTable.SetupGet(s => s.Query).Returns(new[] { rowToDelete, rowNotToDelete } .AsQueryable());

            store.RemoveUserDevice(new Uri("http://device-uri-to-delete/"));

            mockUserDeviceTable.Verify(t => t.Delete(It.IsAny<UserDeviceRow>()), Times.Never());
        }
        
        [TestMethod]
        public void GetDevicesFiltersByPartitionKeyWithUsername()
        {
            var mockUserDeviceTable = new Mock<IAzureTable<UserDeviceRow>>();
            var store = new UserDeviceStore(mockUserDeviceTable.Object);
            var rowToReturn = new UserDeviceRow { PartitionKey = "username", RowKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("http://device-to-return/")) };
            var rowNotToReturn = new UserDeviceRow { PartitionKey = "other-username", RowKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("http://other-device/")) };
            mockUserDeviceTable.SetupGet(s => s.Query).Returns(new[] { rowToReturn, rowNotToReturn } .AsQueryable());

            var devices = store.GetDevices("username");

            Assert.AreEqual(new Uri("http://device-to-return/"), devices.Single());
        }
    }
}
