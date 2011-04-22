namespace TailSpin.PhoneClient.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Microsoft.Practices.Prism.Commands;
    using TailSpin.PhoneClient.Infrastructure;
    using TailSpin.PhoneClient.Models;
    using TailSpin.PhoneClient.Services;
    using TailSpin.PhoneClient.Services.Stores;

    public class TakeSurveyViewModel : ViewModel
    {
        private static readonly Dictionary<QuestionType, Func<QuestionAnswer, QuestionViewModel>> Maps =
            new Dictionary<QuestionType, Func<QuestionAnswer, QuestionViewModel>>
                {
                    { QuestionType.SimpleText, a => new OpenQuestionViewModel(a) }, 
                    { QuestionType.MultipleChoice, a => new MultipleChoiceQuestionViewModel(a) }, 
                    { QuestionType.FiveStars, a => new FiveStarsQuestionViewModel(a) }, 
                    { QuestionType.Voice, a => new VoiceQuestionViewModel(a) }, 
                    { QuestionType.Picture, a => new PictureQuestionViewModel(a) }
                };

        private ILocationService locationService;

        private SurveyAnswer surveyAnswer;
        private ISurveyStoreLocator surveyStoreLocator;
        private SurveyTemplateViewModel templateViewModel;
        private SurveyAnswer tombstoned;

        public TakeSurveyViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.IsBeingActivated();
        }

        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand CompleteCommand { get; set; }

        public ILocationService LocationService
        {
            set { this.locationService = value; }
        }

        public IList<QuestionViewModel> Questions { get; set; }

        public DelegateCommand SaveCommand { get; set; }
        public int SelectedPanoramaIndex { get; set; }

        public SurveyAnswer SurveyAnswer
        {
            get { return this.surveyAnswer; }

            set
            {
                // Set only if there was not a value coming from tombstoning (IsBeingActivated)
                if (this.surveyAnswer == null)
                {
                    this.surveyAnswer = value;
                }

                this.SubscribeToAnswersChanged();
            }
        }

        public ISurveyStoreLocator SurveyStoreLocator
        {
            set { this.surveyStoreLocator = value; }
        }

        public SurveyTemplateViewModel TemplateViewModel
        {
            get { return this.templateViewModel; }

            set
            {
                this.templateViewModel = value;
                this.surveyAnswer = this.surveyStoreLocator.GetStore().GetCurrentAnswerForTemplate(value.Template) ??
                                    value.Template.CreateSurveyAnswers();
                if (this.tombstoned != null)
                {
                    // There was a tombstoned one
                    this.surveyAnswer.SetAnswersFrom(this.tombstoned);
                }
            }
        }

        public override sealed void IsBeingActivated()
        {
            this.tombstoned = Tombstoning.Load<SurveyAnswer>("TakeSurveyAnswer");
            this.SelectedPanoramaIndex = Tombstoning.Load<int>("TakeSurveyCurrentIndex");
        }

        public override void IsBeingDeactivated()
        {
            Tombstoning.Save("TakeSurveyAnswer", this.SurveyAnswer);
            Tombstoning.Save("TakeSurveyCurrentIndex", this.SelectedPanoramaIndex);
            this.DisposeControls();
            base.IsBeingDeactivated();
        }

        public void Initialize()
        {
            this.SaveCommand =
                new DelegateCommand(
                    () =>
                        {
                            this.surveyStoreLocator.GetStore().SaveSurveyAnswer(this.SurveyAnswer);
                            this.TemplateViewModel.Completeness = this.SurveyAnswer.CompletenessPercentage;
                            this.CleanUpAndGoBack();
                        }, 
                    () => this.surveyAnswer.CompletenessPercentage > 0);
            this.CompleteCommand =
                new DelegateCommand(
                    () =>
                        {
                            this.surveyAnswer.IsComplete = true;
                            this.surveyAnswer.CompleteLocation = this.locationService.TryToGetCurrentLocation();
                            this.surveyStoreLocator.GetStore().SaveSurveyAnswer(this.SurveyAnswer);
                            this.TemplateViewModel.AnswersQty++;
                            this.TemplateViewModel.Completeness = 0;
                            this.CleanUpAndGoBack();
                        }, 
                    () => this.surveyAnswer.CompletenessPercentage == 100);
            this.Questions = this.SurveyAnswer.Answers.Select(a => Maps[a.QuestionType].Invoke(a)).ToArray();
        }

        private void AnswerChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            this.TemplateViewModel.Completeness = this.SurveyAnswer.CompletenessPercentage;
            this.CompleteCommand.RaiseCanExecuteChanged();
            this.SaveCommand.RaiseCanExecuteChanged();
        }

        private void CleanUpAndGoBack()
        {
            this.UnsubscribeToAnswersChanged();
            this.DisposeControls();
            this.Dispose();
            this.NavigationService.GoBack();
        }

        private void DisposeControls()
        {
            foreach (var questionViewModel in this.Questions)
            {
                var disposable = questionViewModel as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private void SubscribeToAnswersChanged()
        {
            this.surveyAnswer.Answers.ForEach(a => a.PropertyChanged += this.AnswerChangedHandler);
        }

        private void UnsubscribeToAnswersChanged()
        {
            this.surveyAnswer.Answers.ForEach(a => a.PropertyChanged -= this.AnswerChangedHandler);
        }
    }
}