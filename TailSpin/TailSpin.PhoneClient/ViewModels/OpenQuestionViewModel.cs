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
