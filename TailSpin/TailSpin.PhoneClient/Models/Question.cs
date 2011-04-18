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

    public class Question
    {
        public Question()
        {
           this.PossibleAnswers = new List<string>();
        }

        public QuestionType Type { get; set; }

        public string Text { get; set; }

        public List<string> PossibleAnswers { get; set; }
    }
}
