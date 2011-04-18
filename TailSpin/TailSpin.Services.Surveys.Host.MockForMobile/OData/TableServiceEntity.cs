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
    using System.Data.Services.Common;

    [DataServiceKey(new[] { "PartitionKey", "RowKey" })]
    public abstract class TableServiceEntity
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }
        
        public DateTime Timestamp { get; set; }
    }
}