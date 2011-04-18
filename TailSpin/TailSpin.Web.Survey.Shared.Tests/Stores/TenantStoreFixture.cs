




 




namespace TailSpin.Web.Survey.Shared.Tests.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Shared.Models;
    using Shared.Stores;
    using Shared.Stores.AzureStorage;

    [TestClass]
    public class TenantStoreFixture
    {
        [TestMethod]
        public void GetTenantCallsBlobStorageToRetrieveTenant()
        {
            var mockTenantBlobContainer = new Mock<IAzureBlobContainer<Tenant>>();
            var store = new TenantStore(mockTenantBlobContainer.Object, null);

            store.GetTenant("tenant");

            mockTenantBlobContainer.Verify(c => c.Get("tenant"), Times.Once());
        }

        [TestMethod]
        public void GetTenantReturnsTenantFromBlobStorage()
        {
            var mockTenantBlobContainer = new Mock<IAzureBlobContainer<Tenant>>();
            var store = new TenantStore(mockTenantBlobContainer.Object, null);
            var tenant = new Tenant();
            mockTenantBlobContainer.Setup(c => c.Get("tenant")).Returns(tenant);

            var actualTenant = store.GetTenant("tenant");

            Assert.AreSame(tenant, actualTenant);
        }

        [TestMethod]
        public void GetTenantsReturnsTenantsFromBlobStorage()
        {
            var mockTenantBlobContainer = new Mock<IAzureBlobContainer<Tenant>>();
            var store = new TenantStore(mockTenantBlobContainer.Object, null);
            var tenant = new Tenant();
            var rowsToReturn = new List<Tenant> { tenant };
            mockTenantBlobContainer.Setup(c => c.GetAll()).Returns(rowsToReturn);

            var actualTenant = store.GetTenants();

            Assert.AreSame(tenant, actualTenant.First());
        }

        [TestMethod]
        public void InitializeEnsuresContainerExists()
        {
            var mockTenantBlobContainer = new Mock<IAzureBlobContainer<Tenant>>();
            var mockLogosBlobContainer = new Mock<IAzureBlobContainer<byte[]>>();
            mockLogosBlobContainer.Setup(c => c.GetUri(It.IsAny<string>())).Returns(new Uri("http://uri"));
            var store = new TenantStore(mockTenantBlobContainer.Object, mockLogosBlobContainer.Object);

            store.Initialize();

            mockTenantBlobContainer.Verify(c => c.EnsureExist(), Times.Once());
        }

        [TestMethod]
        public void UploadLogoSavesLogoToContainer()
        {
            var mockLogosBlobContainer = new Mock<IAzureBlobContainer<byte[]>>();
            var mockTenantContainer = new Mock<IAzureBlobContainer<Tenant>>();
            var store = new TenantStore(mockTenantContainer.Object, mockLogosBlobContainer.Object);
            mockTenantContainer.Setup(c => c.Get("tenant")).Returns(new Tenant());
            mockLogosBlobContainer.Setup(c => c.GetUri(It.IsAny<string>())).Returns(new Uri("http://bloburi"));
            var logo = new byte[1];

            store.UploadLogo("tenant", logo);

            mockLogosBlobContainer.Verify(c => c.Save("tenant", logo), Times.Once());
        }

        [TestMethod]
        public void UploadLogoGetsTenatToUpdateFromContainer()
        {
            var mockLogosBlobContainer = new Mock<IAzureBlobContainer<byte[]>>();
            var mockTenantContainer = new Mock<IAzureBlobContainer<Tenant>>();
            var store = new TenantStore(mockTenantContainer.Object, mockLogosBlobContainer.Object);
            mockTenantContainer.Setup(c => c.Get("tenant")).Returns(new Tenant()).Verifiable();
            mockLogosBlobContainer.Setup(c => c.GetUri(It.IsAny<string>())).Returns(new Uri("http://bloburi"));

            store.UploadLogo("tenant", new byte[1]);

            mockTenantContainer.Verify();
        }

        [TestMethod]
        public void UploadLogoSaveTenatWithLogoUrl()
        {
            var mockLogosBlobContainer = new Mock<IAzureBlobContainer<byte[]>>();
            var mockTenantContainer = new Mock<IAzureBlobContainer<Tenant>>();
            var store = new TenantStore(mockTenantContainer.Object, mockLogosBlobContainer.Object);
            mockTenantContainer.Setup(c => c.Get("tenant")).Returns(new Tenant());
            mockLogosBlobContainer.Setup(c => c.GetUri(It.IsAny<string>())).Returns(new Uri("http://bloburi/"));

            store.UploadLogo("tenant", new byte[1]);

            mockTenantContainer.Verify(c => c.Save("tenant", It.Is<Tenant>(t => t.Logo == "http://bloburi/")));
        }
    }
}