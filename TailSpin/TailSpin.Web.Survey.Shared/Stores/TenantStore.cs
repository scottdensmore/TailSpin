




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Properties;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public class TenantStore : ITenantStore
    {
        private const string AdatumPhoneLogo = "adatum-phone";
        private const string FabrikamPhoneLogo = "fabrikam-phone";
        private readonly IAzureBlobContainer<Tenant> tenantBlobContainer;
        private readonly IAzureBlobContainer<byte[]> logosBlobContainer;

        public TenantStore(IAzureBlobContainer<Tenant> tenantBlobContainer, IAzureBlobContainer<byte[]> logosBlobContainer)
        {
            this.tenantBlobContainer = tenantBlobContainer;
            this.logosBlobContainer = logosBlobContainer;
        }

        public IEnumerable<Tenant> GetTenants()
        {
            return this.tenantBlobContainer.GetAll();
        }

        public void Initialize()
        {
            this.logosBlobContainer.EnsureExist();
            this.tenantBlobContainer.EnsureExist();

            if (this.GetTenant("adatum") == null)
            {
                byte[] logo;
                using (var stream = new MemoryStream())
                {
                    PhoneLogos.adatum.Save(stream, ImageFormat.Png);
                    logo = stream.ToArray();
                }
                this.logosBlobContainer.Save(AdatumPhoneLogo, logo);
                var phoneLogoUri = this.logosBlobContainer.GetUri(AdatumPhoneLogo);
                this.SaveTenant(new Tenant
                                    {
                                        Name = "Adatum",
                                        HostGeoLocation = "Anywhere US",
                                        IssuerUrl = "https://localhost/Adatum.SimulatedIssuer.wp7/",
                                        IssuerThumbPrint = "f260042d59e14817984c6183fbc6bfc71baf5462",
                                        ClaimType = "http://schemas.xmlsoap.org/claims/group",
                                        ClaimValue = "Marketing Managers",
                                        SqlAzureConnectionString =
                                            @"Data Source=.\SQLExpress;Initial Catalog=adatum-surveywp7;Integrated Security=True",
                                        DatabaseName = "adatum-survey.database.windows.net",
                                        DatabaseUserName = "adatumuser",
                                        DatabasePassword = "SecretPassword",
                                        PhoneLogo = phoneLogoUri.ToString()
                                    });
            }

            if (this.GetTenant("fabrikam") == null)
            {
                byte[] logo;
                using (var stream = new MemoryStream())
                {
                    PhoneLogos.fabrikam.Save(stream, ImageFormat.Png);
                    logo = stream.ToArray();
                }
                this.logosBlobContainer.Save(FabrikamPhoneLogo, logo);
                var phoneLogoUri = this.logosBlobContainer.GetUri(FabrikamPhoneLogo);
                this.SaveTenant(new Tenant
                                    {
                                        Name = "Fabrikam",
                                        HostGeoLocation = "Anywhere US",
                                        IssuerUrl = "https://localhost/Fabrikam.SimulatedIssuer.wp7/",
                                        IssuerThumbPrint = "d2316a731b59683e744109278c80e2614503b17e",
                                        ClaimType = "http://schemas.xmlsoap.org/claims/group",
                                        ClaimValue = "Marketing Managers",
                                        SqlAzureConnectionString = string.Empty,
                                        PhoneLogo = phoneLogoUri.ToString()
                                    });
            }
        }

        public Tenant GetTenant(string tenant)
        {
            return this.tenantBlobContainer.Get(tenant.ToLowerInvariant());
        }

        public void SaveTenant(Tenant tenant)
        {
            this.tenantBlobContainer.Save(tenant.Name.ToLowerInvariant(), tenant);
        }

        public void UploadLogo(string tenant, byte[] logo)
        {
            this.logosBlobContainer.Save(tenant, logo);

            var tenantToUpdate = this.tenantBlobContainer.Get(tenant);
            tenantToUpdate.Logo = this.logosBlobContainer.GetUri(tenant).ToString();
            this.tenantBlobContainer.Save(tenant, tenantToUpdate);
        }
    }
}