//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Services.Stores
{
    using System.Collections.Generic;
    using TailSpin.PhoneClient.Models;

    public class NullSurveyStore : ISurveyStore
    {
        public string LastSyncDate { get; set; }

        public void DeleteSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers)
        {
        }

        public IEnumerable<SurveyAnswer> GetCompleteSurveyAnswers()
        {
            return new List<SurveyAnswer>();
        }

        public SurveyAnswer GetCurrentAnswerForTemplate(SurveyTemplate template)
        {
            return new SurveyAnswer();
        }

        public IEnumerable<SurveyTemplate> GetSurveyTemplates()
        {
            return new List<SurveyTemplate>();
        }

        public void SaveStore()
        {
        }

        public void SaveSurveyAnswer(SurveyAnswer answer)
        {
        }

        public void SaveSurveyTemplates(IEnumerable<SurveyTemplate> surveys)
        {
        }
    }
}