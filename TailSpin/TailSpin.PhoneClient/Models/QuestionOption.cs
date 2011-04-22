namespace TailSpin.PhoneClient.Models
{
    using Microsoft.Practices.Prism.ViewModel;

    public class QuestionOption : NotificationObject
    {
        private bool isSelected;
        private string text;

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                this.isSelected = value;
                this.RaisePropertyChanged(() => this.IsSelected);
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                this.RaisePropertyChanged(() => this.Text);
            }
        }
    }
}