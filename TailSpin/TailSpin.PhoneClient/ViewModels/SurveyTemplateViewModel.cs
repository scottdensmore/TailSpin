namespace TailSpin.PhoneClient.ViewModels
{
    using System;
    using System.Runtime.Serialization;
    using Microsoft.Practices.Prism.Commands;
    using TailSpin.PhoneClient.Models;
    using TailSpin.PhoneClient.Services;

    [DataContract]
    public class SurveyTemplateViewModel : ViewModel
    {
        private int completeness;
        private int answersQty;

        public SurveyTemplateViewModel(SurveyTemplate surveyTemplate, INavigationService navigationService)
            : base(navigationService)
        {
            this.Template = surveyTemplate;
            this.MarkFavoriteCommand = new DelegateCommand(() =>
                            {
                                this.Template.IsFavorite = true;
                                this.RaisePropertyChanged(() => this.IsFavorite);
                            });
            this.RemoveFavoriteCommand = new DelegateCommand(() =>
                            {
                                this.Template.IsFavorite = false;
                                this.RaisePropertyChanged(() => this.IsFavorite);
                            });
            this.ShowDetailsCommand = new DelegateCommand(() => this.NavigationService.Navigate(BuildUri(@"/Views/SurveyDetailsView.xaml")));
            this.TakeSurveyCommand = new DelegateCommand(() => this.NavigationService.Navigate(BuildUri(@"/Views/TakeSurvey/TakeSurveyView.xaml")));
            this.IsBeingActivated();
        }

        public DelegateCommand ShowDetailsCommand { get; set; }

        public DelegateCommand TakeSurveyCommand { get; set; }

        public DelegateCommand MarkFavoriteCommand { get; set; }

        public DelegateCommand RemoveFavoriteCommand { get; set; }

        [DataMember]
        public SurveyTemplate Template { get; set; }

        public string IconUrl { get { return this.Template.IconUrl; } }

        public string Tenant { get { return this.Template.Tenant; } }

        public string Title { get { return this.Template.Title; } }

        public int Length { get { return this.Template.Length; } }

        public int QuestionsQty { get { return this.Template.Questions.Count; } }

        public int AnswersQty
        {
            get
            {
                return this.answersQty;
            }

            set
            {
                if (this.answersQty != value)
                {
                    this.answersQty = value;
                    this.RaisePropertyChanged(() => this.AnswersQty);
                }
            }
        }

        public bool IsFavorite { get { return this.Template.IsFavorite; } }

        public int Completeness
        {
            get
            {
                return this.completeness;
            }

            set
            {
                this.completeness = value;
                this.RaisePropertyChanged(() => this.Completeness);
            }
        }

        public override sealed void IsBeingActivated()
        {
        }

        private static Uri BuildUri(string baseUriString)
        {
            return new Uri(baseUriString, UriKind.Relative);
        }
    }
}
