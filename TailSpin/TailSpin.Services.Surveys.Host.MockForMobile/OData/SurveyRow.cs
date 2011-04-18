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

    public class SurveyRow : TableServiceEntity
    {
        public string SlugName { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}