




 




namespace TailSpin.Web.Survey.Shared.SurveysFiltering
{
    using System.Collections.Generic;

    public interface ITenantFilterStore
    {
        void Initialize();
        IEnumerable<string> GetTenants(string username);
        void SetTenants(string username, IEnumerable<string> tenants);
        IEnumerable<string> GetUsers(string tenant);
    }
}