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
    using System.Linq;

    public class SurveyDto : SurveyRow
    {
        public IQueryable<QuestionRow> Questions
        {
            get
            {
                return new SurveysServiceEntitiesMock().Questions.Where(q => q.PartitionKey == this.RowKey);
            }
        }

        public string IconUrl
        {
            get
            {
                switch (this.PartitionKey.ToLowerInvariant())
                {
                    case "adatum":
                        return "http://userlogos.org/files/imagecache/thumbnail/logos/Cracka/Battlefield-Heroes.png";
                    case "fabrikam":
                        return "http://userlogos.org/files/imagecache/thumbnail/logos/Cracka/Voodoo-Extreme.png";
                    default:
                        return "http://userlogos.org/files/imagecache/thumbnail/logos/VSRae/rtk.png";
                }
            }
        }
    }
}