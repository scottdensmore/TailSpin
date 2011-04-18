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
    using System.Collections.ObjectModel;
    using TailSpin.PhoneClient.Models;

    public class MultipleChoiceQuestionViewModel : QuestionViewModel
    {
        public MultipleChoiceQuestionViewModel(QuestionAnswer answer)
            : base(answer)
        {
            this.Options = new ObservableCollection<QuestionOption>();
            foreach (var option in answer.PossibleAnswers)
            {
                var questionOption = new QuestionOption { Text = option, IsSelected = option == answer.Value };
                this.Options.Add(questionOption);
                questionOption.PropertyChanged += delegate
                {
                    if (questionOption.IsSelected)
                    {
                        this.Answer.Value = questionOption.Text;
                    }
                };
            }
        }

        public ObservableCollection<QuestionOption> Options { get; private set; }
    }
}
