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