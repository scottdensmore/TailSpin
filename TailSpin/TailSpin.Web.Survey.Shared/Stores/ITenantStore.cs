




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using System.Collections.Generic;
    using TailSpin.Web.Survey.Shared.Models;

    public interface ITenantStore
    {
        void Initialize();
        Tenant GetTenant(string tenant);
        IEnumerable<Tenant> GetTenants();
        void SaveTenant(Tenant tenant);
        void UploadLogo(string tenant, byte[] logo);
    }
}