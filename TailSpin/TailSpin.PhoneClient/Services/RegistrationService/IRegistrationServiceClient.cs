namespace TailSpin.PhoneClient.Services.RegistrationService
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Phone.Reactive;
    using TailSpin.PhoneClient.Models;

    public interface IRegistrationServiceClient
    {
        IObservable<TaskSummaryResult> UpdateReceiveNotifications(bool receiveNotifications);
        IObservable<Unit> UpdateTenants(IEnumerable<TenantItem> tenants);
        IObservable<SurveyFiltersInformation> GetSurveysFilterInformation();
    }
}