//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Services.SurveysService
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Phone.Reactive;
    using TailSpin.PhoneClient.Models;
    using TailSpin.PhoneClient.Services.Stores;

    public class SurveysServiceClientMock : ISurveysServiceClient
    {
        private readonly ISettingsStore settingsStore;
        private static string lastUser;
        private List<SurveyTemplate> surveys;

        public SurveysServiceClientMock(ISettingsStore settingsStore)
        {
            this.settingsStore = settingsStore;
        }

        public IObservable<IEnumerable<SurveyTemplate>> GetNewSurveys(string lastSyncDate)
        {
            if (this.CredentialsAreInvalid())
            {
                return Observable.Throw<IEnumerable<SurveyTemplate>>(new UnauthorizedAccessException());
            }

            if (this.surveys == null || this.settingsStore.UserName != lastUser)
            {
                this.InitializeSurveys();
                lastUser = this.settingsStore.UserName;
            }

            Func<SurveyTemplate, bool> filter = null;
            switch (this.settingsStore.UserName.ToLower(CultureInfo.InvariantCulture))
            {
                case "joe":
                    filter = survey => survey.Tenant == "adatum";
                    break;
                case "scott":
                    filter = survey => survey.Tenant == "fabrikam";
                    break;
                default:
                    break;
            }

            var surveysToReturn = new List<SurveyTemplate>();
            surveysToReturn.AddRange(filter != null ? this.surveys.Where(filter) : this.surveys);

            surveysToReturn.ToList().ForEach(s => this.surveys.Remove(s));
            return new[] { surveysToReturn.AsEnumerable() } .ToObservable();
        }

        public IObservable<Unit> SaveSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers)
        {
            if (this.CredentialsAreInvalid())
            {
                return Observable.Throw<Unit>(new UnauthorizedAccessException());
            }

            return Observable.Return(new Unit());
        }

        private bool CredentialsAreInvalid()
        {
            var currentUser = this.settingsStore.UserName.ToLower(CultureInfo.InvariantCulture);
            return currentUser != "fred"
                && currentUser != "joe"
                && currentUser != "scott";
        }

        private void InitializeSurveys()
        {
            this.surveys = new List<SurveyTemplate>
                               {
                                   new SurveyTemplate
                                       {
                                           Title = "Satisfaction",
                                           SlugName = "satisfaction",
                                           Tenant = "adatum",
                                           Completeness = 0,
                                           CreatedOn = new DateTime(2010, 01, 01),
                                           IconUrl = "/Resources/Images/adatum-logo.png",
                                           Length = 30,
                                           Questions = new List<Question>
                                                           {
                                                               new Question
                                                                   {
                                                                       Type = QuestionType.MultipleChoice,
                                                                       Text = "How did you hear about us?",
                                                                       PossibleAnswers =
                                                                           new List<string>
                                                                               {
                                                                                   "Television", 
                                                                                   "Radio", 
                                                                                   "Internet", 
                                                                                   "Other"
                                                                               }
                                                                   },
                                                               new Question
                                                                   {
                                                                       Type = QuestionType.MultipleChoice,
                                                                       Text = "How would you rate our service?",
                                                                       PossibleAnswers = new List<string> { "Great", "Good", "Not bad", "Horrible" }
                                                                   },
                                                               new Question
                                                                   {
                                                                       Type = QuestionType.FiveStars,
                                                                       Text = "How do you rate our service up to five stars?",
                                                                   },
                                                               new Question
                                                                   {
                                                                       Type = QuestionType.Picture,
                                                                       Text = "Please take a picture",
                                                                   },
                                                               new Question
                                                                   {
                                                                       Type = QuestionType.Voice,
                                                                       Text = "Please record a comment.",
                                                                   },
                                                           }
                                       },
                                       new SurveyTemplate
                                       {
                                           Title = "Customer loyalty",
                                           SlugName = "customer-loyalty",
                                           Tenant = "fabrikam",
                                           Completeness = 0,
                                           CreatedOn = new DateTime(2010, 01, 01),
                                           IconUrl = "/Resources/Images/fabrikam-logo.png",
                                           Length = 30,
                                           Questions = new List<Question>
                                                           {
                                                               new Question
                                                                   {
                                                                       Type = QuestionType.MultipleChoice,
                                                                       Text = "Are you planning to visit us this month?",
                                                                       PossibleAnswers = new List<string> { "Yes", "No" }
                                                                   },
                                                               new Question
                                                                   {
                                                                       Type = QuestionType.SimpleText,
                                                                       Text = "Enter your suggestions:"
                                                                   }
                                                           }
                                       }
                               };
        }
    }
}