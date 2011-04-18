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
    using Models;

    public class FiveStarsQuestionViewModel : QuestionViewModel
    {
        public FiveStarsQuestionViewModel(QuestionAnswer questionAnswer)
            : base(questionAnswer)
        {
        }

        public int Rating
        {
            get { return this.Answer.Value != null ? int.Parse(this.Answer.Value) : 0; }

            set
            {
                this.Answer.Value = value.ToString();
                this.RaisePropertyChanged(string.Empty);
            }
        }
    }
}