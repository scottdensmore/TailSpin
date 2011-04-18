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

    public class SurveyFiltersInformation
    {
        public SurveyFiltersInformation()
        {
            this.AllTenants = new TenantItem[0];
            this.SelectedTenants = new TenantItem[0];
        }

        public IEnumerable<TenantItem> AllTenants { get; set; }
        public IEnumerable<TenantItem> SelectedTenants { get; set; }
    }
}