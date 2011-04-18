//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Services.SurveysService
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Phone.Reactive;
    using TailSpin.PhoneClient.Models;

    public interface ISurveysServiceClient
    {
        IObservable<IEnumerable<SurveyTemplate>> GetNewSurveys(string lastSyncDate);
        IObservable<Unit> SaveSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswers);
    }
}