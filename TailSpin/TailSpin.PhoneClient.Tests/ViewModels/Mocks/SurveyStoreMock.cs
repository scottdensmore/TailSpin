namespace TailSpin.PhoneClient.Tests.ViewModels.Mocks
{
    using System;
    using System.Collections.Generic;
    using PhoneClient.Models;
    using TailSpin.PhoneClient.Services.Stores;

    public class SurveyStoreMock : ISurveyStore
    {
        private List<SurveyTemplate> surveys;

        public SurveyStoreMock()
        {
            this.surveys = new List<SurveyTemplate>();
            this.SurveyAnswers = new List<SurveyAnswer>();
        }

        public SurveyStoreMock(List<SurveyTemplate> surveys)
        {
            this.surveys = surveys;
        }

        public IEnumerable<SurveyTemplate> SavedSurveys { get; private set; }

        public IEnumerable<SurveyAnswer> SurveyAnswers { get; set; }

        public string LastSyncDate { get; set; }

        public SurveyAnswer GetCurrentAnswerForTemplate(SurveyTemplate template)
        {
            return null;
        }

        public void DeleteSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers)
        {
        }

        public IEnumerable<SurveyAnswer> GetCompleteSurveyAnswers()
        {
            return this.SurveyAnswers;
        }

        public IEnumerable<SurveyTemplate> GetSurveyTemplates()
        {
            return this.surveys;
        }

        public void Initialize()
        {
            if (this.surveys == null)
            {
                this.surveys = new List<SurveyTemplate>(
                    new[]
                        {
                            new SurveyTemplate
                                {
                                    Title = "My survey1",
                                    Tenant = "Tenant One",
                                    IconUrl = "http://icon",
                                    Completeness = 100,
                                    IsFavorite = true,
                                    IsNew = true,
                                    Length = 40
                                },
                            new SurveyTemplate
                                {
                                    Title = "My survey2",
                                    Tenant = "Tenant One",
                                    IconUrl = "http://icon",
                                    Completeness = 50,
                                    IsFavorite = true,
                                    IsNew = false,
                                    Length = 40
                                },
                            new SurveyTemplate
                                {
                                    Title = "My survey3",
                                    Tenant = "Tenant One",
                                    IconUrl = "http://icon",
                                    Completeness = 100,
                                    IsFavorite = false,
                                    IsNew = false,
                                    Length = 40
                                },
                            new SurveyTemplate
                                {
                                    Title = "My survey4",
                                    Tenant = "Tenant Two",
                                    IconUrl = "http://icon",
                                    Completeness = 50,
                                    IsFavorite = false,
                                    IsNew = true,
                                    Length = 40
                                },
                            new SurveyTemplate
                                {
                                    Title = "My survey5",
                                    Tenant = "Tenant Two",
                                    IconUrl = "http://icon",
                                    Completeness = 100,
                                    IsFavorite = false,
                                    IsNew = false,
                                    Length = 40
                                },
                            new SurveyTemplate
                                {
                                    Title = "My survey6",
                                    Tenant = "Tenant Two",
                                    IconUrl = "http://icon",
                                    Completeness = 100,
                                    IsFavorite = true,
                                    IsNew = false,
                                    Length = 40
                                },
                        });
            }
        }

        public void SaveSurveyAnswer(SurveyAnswer answer)
        {
            throw new NotImplementedException();
        }

        public void SaveSurveyTemplates(IEnumerable<SurveyTemplate> surveys)
        {
            this.SavedSurveys = surveys;
        }

        public void SaveStore()
        {
        }
    }
}
