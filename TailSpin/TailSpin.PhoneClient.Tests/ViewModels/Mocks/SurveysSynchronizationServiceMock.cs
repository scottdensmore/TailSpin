namespace TailSpin.PhoneClient.Tests.ViewModels.Mocks
{
    using System;
    using Microsoft.Phone.Reactive;
    using PhoneClient.Services;

    public class SurveysSynchronizationServiceMock : ISurveysSynchronizationService
    {
        public IObservable<TaskCompletedSummary[]> StartSynchronization()
        {
            return Observable.Empty<TaskCompletedSummary[]>();
        }
    }
}