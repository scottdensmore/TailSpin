//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Services
{
    using System;
    using System.Linq;
    using System.Net;
    using Microsoft.Phone.Reactive;
    using TailSpin.PhoneClient.Services.Stores;
    using TailSpin.PhoneClient.Services.SurveysService;

    public class SurveysSynchronizationService : ISurveysSynchronizationService
    {
        public const string GetSurveysTask = "GetNewSurveys";
        public const string SaveSurveyAnswersTask = "SaveSurveyAnswers";
        private readonly Func<ISurveysServiceClient> surveysServiceClientFactory;
        private readonly ISurveyStoreLocator surveyStoreLocator;

        public SurveysSynchronizationService(
                Func<ISurveysServiceClient> surveysServiceClientFactory,
                ISurveyStoreLocator surveyStoreLocator)
        {
            this.surveysServiceClientFactory = surveysServiceClientFactory;
            this.surveyStoreLocator = surveyStoreLocator;
        }

        public IObservable<TaskCompletedSummary[]> StartSynchronization()
        {
            var surveyStore = this.surveyStoreLocator.GetStore();

            var getNewSurveys =
                this.surveysServiceClientFactory()
                    .GetNewSurveys(surveyStore.LastSyncDate)
                    .Select(
                        surveys =>
                            {
                                surveyStore.SaveSurveyTemplates(surveys);
                                if (surveys.Count() > 0)
                                {
                                    surveyStore.LastSyncDate = surveys.Max(s => s.CreatedOn).ToString("s");
                                }

                                return new TaskCompletedSummary
                                           {
                                               Task = GetSurveysTask, 
                                               Result = TaskSummaryResult.Success,
                                               Context = surveys.Count().ToString()
                                           };
                            })
                    .Catch(
                        (Exception exception) =>
                            {
                                if (exception is WebException)
                                {
                                    var webException = exception as WebException;
                                    var summary = ExceptionHandling.GetSummaryFromWebException(GetSurveysTask, webException);
                                    return Observable.Return(summary);
                                }

                                if (exception is UnauthorizedAccessException)
                                {
                                    return Observable.Return(new TaskCompletedSummary { Task = GetSurveysTask, Result = TaskSummaryResult.AccessDenied });
                                }
                                
                                throw exception;
                            });

            var surveyAnswers = surveyStore.GetCompleteSurveyAnswers();
            var saveSurveyAnswers = 
                Observable.Return(
                    new TaskCompletedSummary
                        {
                            Task = SaveSurveyAnswersTask, 
                            Result = TaskSummaryResult.Success,
                            Context = 0.ToString()
                        });
            if (surveyAnswers.Count() > 0)
            {
                saveSurveyAnswers =
                this.surveysServiceClientFactory()
                    .SaveSurveyAnswers(surveyAnswers)
                    .Select(
                        unit => 
                        {
                            var sentAnswersCount = surveyAnswers.Count();
                            surveyStore.DeleteSurveyAnswers(surveyAnswers);
                            return new TaskCompletedSummary
                                       {
                                           Task = SaveSurveyAnswersTask, 
                                           Result = TaskSummaryResult.Success,
                                           Context = sentAnswersCount.ToString()
                                       };
                        })
                    .Catch(
                        (Exception exception) =>
                        {
                            if (exception is WebException)
                            {
                                var webException = exception as WebException;
                                var summary = ExceptionHandling.GetSummaryFromWebException(SaveSurveyAnswersTask, webException);
                                return Observable.Return(summary);
                            }

                            if (exception is UnauthorizedAccessException)
                            {
                                return Observable.Return(new TaskCompletedSummary { Task = SaveSurveyAnswersTask, Result = TaskSummaryResult.AccessDenied });
                            }
                                
                            throw exception;
                        });
            }

            return Observable.ForkJoin(getNewSurveys, saveSurveyAnswers);
        }
    }
}
