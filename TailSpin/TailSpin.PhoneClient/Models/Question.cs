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
