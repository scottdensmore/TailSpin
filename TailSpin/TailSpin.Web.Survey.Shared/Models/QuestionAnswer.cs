




 




namespace TailSpin.Web.Survey.Shared.Models
{
    [QuestionAnswerValidator(ErrorMessage = "* You must provide an answer.")]
    public class QuestionAnswer
    {
        public string QuestionText { get; set; }

        public QuestionType QuestionType { get; set; }
        
        public string Answer { get; set; }

        public string PossibleAnswers { get; set; }
    }
}