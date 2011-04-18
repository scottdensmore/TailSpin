//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Models
{
    using System;
    using System.Collections.Generic;
    using TailSpin.PhoneClient.Services;

    public class SurveyTemplate
    {
        public SurveyTemplate()
        {
            this.Questions = new List<Question>();
        }

        public string IconUrl { get; set; }

        public string Tenant { get; set; }

        public string Title { get; set; }

        public string SlugName { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<Question> Questions { get; set; }

        public bool IsNew { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsLocal { get; set; }

        public int Completeness { get; set; }

        public int Length { get; set; }

        public SurveyAnswer CreateSurveyAnswers()
        {
            var survey = new SurveyAnswer
                             {
                                 Answers = new List<QuestionAnswer>(),
                                 SlugName = this.SlugName,
                                 Tenant = this.Tenant,
                                 Title = this.Title
                             };
            foreach (var question in this.Questions)
            {
                survey.Answers.Add(new QuestionAnswer 
                {
                    QuestionType = question.Type,
                    QuestionText = question.Text,
                    PossibleAnswers = new List<string>(question.PossibleAnswers)
                });
            }

            return survey;
        }

        public SurveyAnswer CreateSurveyAnswers(ILocationService locationService)
        {
            var result = this.CreateSurveyAnswers();
            result.StartLocation = locationService.TryToGetCurrentLocation();
            return result;
        }
    }
}
