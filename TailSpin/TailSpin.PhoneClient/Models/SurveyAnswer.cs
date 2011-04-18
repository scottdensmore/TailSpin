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
    using System.Collections.Generic;
    using System.Device.Location;
    using System.Linq;

    public class SurveyAnswer
    {
        public List<QuestionAnswer> Answers { get; set; }

        public string Tenant { get; set; }

        public string SlugName { get; set; }

        public bool IsComplete { get; set; }

        public string Title { get; set; }

        public GeoCoordinate StartLocation { get; set; }

        public GeoCoordinate CompleteLocation { get; set; }

        public int CompletenessPercentage
        {
            get { return (this.Answers.Where(a => !string.IsNullOrEmpty(a.Value)).Count() * 100) / this.Answers.Count; }
        }

        public void SetAnswersFrom(SurveyAnswer source)
        {
            this.StartLocation = source.StartLocation;
            for (var i = 0; i < this.Answers.Count; i++)
            {
                this.Answers[i].Value = source.Answers[i].Value;
            }
        }
    }
}
