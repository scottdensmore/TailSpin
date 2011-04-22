namespace TailSpin.PhoneClient.Services
{
    using System;

    public interface ISurveysSynchronizationService
    {
        IObservable<TaskCompletedSummary[]> StartSynchronization();
    }
}
