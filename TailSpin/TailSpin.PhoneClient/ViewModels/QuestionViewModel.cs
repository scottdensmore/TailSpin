namespace TailSpin.PhoneClient.ViewModels
{
    using Microsoft.Practices.Prism.ViewModel;
    using TailSpin.PhoneClient.Models;

    public class QuestionViewModel : NotificationObject
    {
        private readonly QuestionAnswer answer;

        public QuestionViewModel(QuestionAnswer answer)
        {
            this.answer = answer;
            this.QuestionText = answer.QuestionText;
        }

        public string QuestionText { get; private set; }

        protected QuestionAnswer Answer
        {
            get { return this.answer; }
        }
    }
}
