




 




namespace TailSpin.Services.Surveys.Registration
{
    using System;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.Threading;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class RegistrationService : IRegistrationService
    {
        private readonly ITenantFilterStore tenantFilterStore;
        private readonly IUserDeviceStore userDeviceStore;
        private readonly ITenantStore tenantStore;

        public RegistrationService(
                ITenantFilterStore tenantFilterStore, 
                IUserDeviceStore userDeviceStore, 
                ITenantStore tenantStore)
        {
            this.tenantFilterStore = tenantFilterStore;
            this.userDeviceStore = userDeviceStore;
            this.tenantStore = tenantStore;
        }
        
        public void SetFilters(SurveyFiltersDto surveyFiltersDto)
        {
            var username = Thread.CurrentPrincipal.Identity.Name;

            this.tenantFilterStore.SetTenants(username, surveyFiltersDto.Tenants.Select(t => t.Key));
        }

        public SurveyFiltersInformationDto GetFilters()
        {
            var username = Thread.CurrentPrincipal.Identity.Name;

            return new SurveyFiltersInformationDto
                       {
                           AllTenants = this.tenantStore.GetTenants().Select(t => GetTenantDto(t.Name)).ToArray(),
                           SurveyFilters = new SurveyFiltersDto
                                               {
                                                   Tenants = this.tenantFilterStore.GetTenants(username).Select(GetTenantDto).ToArray()
                                               }
                       };
        }

        public void Notifications(DeviceDto device)
        {
            var username = Thread.CurrentPrincipal.Identity.Name;

            bool isWellFormedUriString = Uri.IsWellFormedUriString(device.Uri, UriKind.Absolute);
            if (isWellFormedUriString)
            {
                if (device.RecieveNotifications)
                {
                    this.userDeviceStore.SetUserDevice(username, new Uri(device.Uri));
                }
                else
                {
                    this.userDeviceStore.RemoveUserDevice(new Uri(device.Uri));
                }
            }
        }

        private static TenantDto GetTenantDto(string tenantName)
        {
            return new TenantDto { Key = tenantName.ToLowerInvariant(), Name = tenantName };
        }
    }
}