//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Services
{
    public class TaskCompletedSummary
    {
        public string Task { get; set; }

        public TaskSummaryResult Result { get; set; }

        public string Context { get; set; }
    }
}