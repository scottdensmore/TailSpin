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
    public enum QuestionType
    {
        SimpleText,
        MultipleChoice,
        FiveStars,
        Picture,
        Voice
    }

    public class QuestionRow : TableServiceEntity
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public string PossibleAnswers { get; set; }
    }
}