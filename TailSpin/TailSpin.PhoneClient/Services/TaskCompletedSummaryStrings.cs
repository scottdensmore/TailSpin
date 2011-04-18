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
    using System;

    public static class TaskCompletedSummaryStrings
    {
        public static string GetDescriptionForResult(TaskSummaryResult result)
        {
            switch (result)
            {
                case TaskSummaryResult.Success:
                    return "Completed successfully";
                case TaskSummaryResult.AccessDenied:
                    return "The credentials you have configured were invalid. Please enter a valid username and password in the Settings page and try again.";
                case TaskSummaryResult.UnreachableServer:
                    return "The connection to our server couldn't be established. This would be caused by a temporary network issue. Please try again later.";
                case TaskSummaryResult.UnknownError:
                    return "There was an unknown error. Please try again later.";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}