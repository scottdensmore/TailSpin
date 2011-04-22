namespace TailSpin.PhoneClient.Services.RegistrationService
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.Phone.Reactive;
    using TailSpin.PhoneClient.Models;
    using TailSpin.PhoneClient.Services.Stores;

    public class RegistrationServiceClientMock : IRegistrationServiceClient
    {
        private readonly ISettingsStore settingsStore;

        public RegistrationServiceClientMock(ISettingsStore settingsStore)
        {
            this.settingsStore = settingsStore;
        }

        public IObservable<TaskSummaryResult> UpdateReceiveNotifications(bool receiveNotifications)
        {
            return Observable.Return(TaskSummaryResult.Success);
        }

        public IObservable<Unit> UpdateTenants(IEnumerable<TenantItem> tenants)
        {
            return Observable.Throw<Unit>(new InvalidOperationException("You can’t change the filter settings in PhoneOnly solution"));
        }

        public IObservable<SurveyFiltersInformation> GetSurveysFilterInformation()
        {
            if (this.CredentialsAreInvalid())
            {
                return Observable.Throw<SurveyFiltersInformation>(new UnauthorizedAccessException());
            }

            return Observable.Return(
                new SurveyFiltersInformation
                    {
                        AllTenants = new[]
                                         {
                                             new TenantItem { Key = "adatum", Name = "Adatum" },
                                             new TenantItem { Key = "fabrikam", Name = "Fabrikam" }
                                         },
                        SelectedTenants = new TenantItem[0]
                    });
        }

        private bool CredentialsAreInvalid()
        {
            var currentUser = this.settingsStore.UserName.ToLower(CultureInfo.InvariantCulture);
            return currentUser != "fred"
                && currentUser != "joe"
                && currentUser != "scott";
        }
    }
}