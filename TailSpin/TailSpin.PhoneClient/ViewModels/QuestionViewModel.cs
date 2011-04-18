//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.ViewModels
{
    using Microsoft.Practices.Prism.ViewModel;
    using TailSpin.PhoneClient.Models;

    public class QuestionViewModel : NotificationObject
    {
        private readonly QuestionAnswer answer;

        public QuestionViewModel(QuestionAnswer answer)
        {
            this.answer = answer;
            this.QuestionText = answer.QuestionText;
        }

        public string QuestionText { get; private set; }

        protected QuestionAnswer Answer
        {
            get { return this.answer; }
        }
    }
}
