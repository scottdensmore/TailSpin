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
    using TailSpin.PhoneClient.Models;

    public class OpenQuestionViewModel : QuestionViewModel
    {
        public OpenQuestionViewModel(QuestionAnswer answer)
            : base(answer)
        {
        }

        public string AnswerText
        {
            get { return this.Answer.Value; } 
            set { this.Answer.Value = value; }
        }
    }
}
