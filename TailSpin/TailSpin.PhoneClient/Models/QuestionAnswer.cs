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
    using Microsoft.Practices.Prism.ViewModel;

    public class QuestionAnswer : NotificationObject
    {
        private string backingValue;

        public QuestionType QuestionType { get; set; }

        public string QuestionText { get; set; }

        public string Value
        {
            get
            {
                return this.backingValue;
            }

            set
            {
                this.backingValue = value;
                this.RaisePropertyChanged(() => this.Value);
            }
        }

        public List<string> PossibleAnswers { get; set; }
    }
}