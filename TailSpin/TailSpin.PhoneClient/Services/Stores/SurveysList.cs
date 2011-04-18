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

    public class SurveysList
    {
        public SurveysList()
        {
            this.LastSyncDate = string.Empty;
        }

        public List<SurveyTemplate> Templates { get; set; }

        public List<SurveyAnswer> Answers { get; set; }

        public string LastSyncDate { get; set; }
    }
}
