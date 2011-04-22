namespace TailSpin.PhoneClient.Tests.Services.Mocks
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Phone.Reactive;
    using PhoneClient.Models;
    using PhoneClient.Services.SurveysService;

    public class SurveysServiceClientMock : ISurveysServiceClient
    {
        public SurveysServiceClientMock()
        {
            this.SurveysToReturn = new List<SurveyTemplate>();
            this.SentSurveyAnswers = new List<SurveyAnswer>();
        }

        public bool GetNewSurveysWasCalled { get; private set; }
        public string GetNewSurveysLastSyncDate { get; private set; }
        public IEnumerable<SurveyTemplate> SurveysToReturn { get; set; }
        public IEnumerable<SurveyAnswer> SentSurveyAnswers { get; set; }

        public IObservable<IEnumerable<SurveyTemplate>> GetNewSurveys(string lastSyncDate)
        {
            this.GetNewSurveysWasCalled = true;
            this.GetNewSurveysLastSyncDate = lastSyncDate;
            return new[] { this.SurveysToReturn } .ToObservable();
        }

        public IObservable<Unit> SaveSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers)
        {
            this.SentSurveyAnswers = surveyAnswers;
            return Observable.Return(new Unit());
        }
    }
}