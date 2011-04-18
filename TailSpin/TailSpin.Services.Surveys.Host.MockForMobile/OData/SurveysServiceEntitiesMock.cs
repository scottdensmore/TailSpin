//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.Services.Surveys.OData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;

    public class SurveysServiceEntitiesMock
    {
        private IQueryable<SurveyDto> surveys;
        private IQueryable<QuestionRow> questions;

        public IQueryable<SurveyDto> Surveys
        {
            get
            {
                if (this.surveys == null)
                {
                    this.InitializeSurveys();
                }

                var username = Thread.CurrentPrincipal.Identity.Name;
                Expression<Func<SurveyDto, bool>> filterExpression = null;
                switch (username.ToLowerInvariant())
                {
                    case "joe":
                        filterExpression = survey => survey.PartitionKey == "adatum";
                        break;
                    case "scott":
                        filterExpression = survey => survey.PartitionKey == "fabrikam";
                        break;
                    default:
                        break;
                }

                if (filterExpression != null)
                {
                    return this.surveys.Where(filterExpression).AsQueryable();
                }

                return this.surveys.AsQueryable();
            }
        }

        public IQueryable<QuestionRow> Questions
        {
            get
            {
                if (this.questions == null)
                {
                    this.InitializeQuestion();
                }

                return this.questions;
            }
        }

        private void InitializeSurveys()
        {
            var surveyList = new List<SurveyDto>
                                 {
                                     new SurveyDto
                                         {
                                             PartitionKey = "adatum",
                                             RowKey = "adatum_satisfaction",
                                             Title = "Satisfaction",
                                             SlugName = "satisfaction",
                                             Timestamp = new DateTime(2010, 01, 01)
                                         }
                                 };

            this.surveys = surveyList.AsQueryable();
        }

        private void InitializeQuestion()
        {
            var questionList = new List<QuestionRow>
                                   {
                                       new QuestionRow
                                           {
                                               PartitionKey = "adatum_satisfaction",
                                               RowKey = "0123456789012345678_000",
                                               Type = Enum.GetName(typeof(QuestionType), QuestionType.MultipleChoice),
                                               Text = "How did you hear about us?",
                                               PossibleAnswers = "TV\nRadio\nInternet\nOther",
                                               Timestamp = new DateTime(2010, 01, 01)
                                           },
                                       new QuestionRow
                                           {
                                               PartitionKey = "adatum_satisfaction",
                                               RowKey = "0123456789012345678_001",
                                               Type = Enum.GetName(typeof(QuestionType), QuestionType.FiveStars),
                                               Text = "How would you rate our service?",
                                               Timestamp = new DateTime(2010, 01, 01)
                                           },
                                       new QuestionRow
                                           {
                                               PartitionKey = "adatum_satisfaction",
                                               RowKey = "0123456789012345678_002",
                                               Type = Enum.GetName(typeof(QuestionType), QuestionType.SimpleText),
                                               Text = "Enter your suggestions:",
                                               Timestamp = new DateTime(2010, 01, 01)
                                           }
                                   };

            this.questions = questionList.AsQueryable();
        }
    }
}