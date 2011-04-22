namespace TailSpin.PhoneClient.Models
{
    using System.Collections.Generic;

    public class SurveyFiltersInformation
    {
        public SurveyFiltersInformation()
        {
            this.AllTenants = new TenantItem[0];
            this.SelectedTenants = new TenantItem[0];
        }

        public IEnumerable<TenantItem> AllTenants { get; set; }
        public IEnumerable<TenantItem> SelectedTenants { get; set; }
    }
}