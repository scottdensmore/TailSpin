namespace TailSpin.PhoneClient.Services.SurveysService
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Phone.Reactive;
    using TailSpin.PhoneClient.Models;

    public interface ISurveysServiceClient
    {
        IObservable<IEnumerable<SurveyTemplate>> GetNewSurveys(string lastSyncDate);
        IObservable<Unit> SaveSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers);
    }
}