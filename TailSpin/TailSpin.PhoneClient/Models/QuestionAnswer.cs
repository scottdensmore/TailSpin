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