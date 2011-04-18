




 




namespace TailSpin.Web.Survey.Shared.SurveysFiltering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;

    public class TenantFilter : ISurveyFilter
    {
        private readonly ISurveyStore surveyStore;
        private readonly ITenantFilterStore tenantFilterStore;
        private readonly ITenantStore tenantStore;

        public TenantFilter(
                ISurveyStore surveyStore, 
                ITenantFilterStore tenantFilterStore,
                ITenantStore tenantStore)
        {
            this.surveyStore = surveyStore;
            this.tenantFilterStore = tenantFilterStore;
            this.tenantStore = tenantStore;
        }

        public IEnumerable<Survey> GetSurveys(string username, DateTime fromDate)
        {
            var tenants = this.tenantFilterStore.GetTenants(username);
            if (tenants.Count() == 0)
            {
                tenants = this.tenantStore.GetTenants().Select(t => t.Name.ToLowerInvariant());
            }

            return
                (from tenant in tenants
                 from survey in this.surveyStore.GetSurveys(tenant, fromDate)
                 select survey).Distinct(new SurveysComparer());
        }

        public IEnumerable<string> GetUsers(Survey survey)
        {
            return this.tenantFilterStore.GetUsers(survey.Tenant);
        }
    }
}