namespace TailSpin.PhoneClient.Tests.ViewModels.Mocks
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Phone.Reactive;
    using TailSpin.PhoneClient.Models;
    using TailSpin.PhoneClient.Services;
    using TailSpin.PhoneClient.Services.RegistrationService;

    public class MockRegistrationService : IRegistrationServiceClient
    {
        public IObservable<TaskSummaryResult> UpdateReceiveNotifications(bool receiveNotifications)
        {
            return Observable.Return(TaskSummaryResult.Success);
        }

        public IObservable<Unit> UpdateTenants(IEnumerable<TenantItem> tenants)
        {
            return Observable.Empty<Unit>();
        }

        public IObservable<SurveyFiltersInformation> GetSurveysFilterInformation()
        {
            return Observable.Empty<SurveyFiltersInformation>();
        }
    }
}