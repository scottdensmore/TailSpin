namespace TailSpin.PhoneClient.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows.Data;
    using Microsoft.Phone.Reactive;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
    using TailSpin.PhoneClient.Infrastructure;
    using TailSpin.PhoneClient.Services;
    using TailSpin.PhoneClient.Services.Stores;

    public class SurveyListViewModel : ViewModel
    {
        private readonly InteractionRequest<Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification> submitErrorInteractionRequest;
        private readonly InteractionRequest<Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification> submitNotificationInteractionRequest;
        private readonly ISurveyStoreLocator surveyStoreLocator;
        private readonly ISurveysSynchronizationService synchronizationService;
        private CollectionViewSource bylengthViewSource;
        private CollectionViewSource favoritesViewSource;
        private ISurveyStore lastSurveyStore;
        private CollectionViewSource newSurveysViewSource;
        private ObservableCollection<SurveyTemplateViewModel> observableSurveyTemplates;
        private int selectedPivotIndex;
        private SurveyTemplateViewModel selectedSurveyTemplate;
        private bool isSyncing;

        public SurveyListViewModel(
            ISurveyStoreLocator surveyStoreLocator,
            ISurveysSynchronizationService synchronizationService,
            INavigationService navigationService)
            : base(navigationService)
        {
            this.surveyStoreLocator = surveyStoreLocator;
            this.synchronizationService = synchronizationService;
            this.submitErrorInteractionRequest = new InteractionRequest<Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification>();
            this.submitNotificationInteractionRequest = new InteractionRequest<Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification>();

            this.StartSyncCommand = new DelegateCommand(
                () => { this.StartSync(); },
                () => !this.IsSynchronizing && !this.SettingAreNotConfigured);

            this.FiltersCommand = new DelegateCommand(
                () => { this.NavigationService.Navigate(new Uri("/Views/FilterSettingsView.xaml?a=1", UriKind.Relative)); },
                () => !this.IsSynchronizing && !this.SettingAreNotConfigured);

            this.AppSettingsCommand = new DelegateCommand(
                () => { this.NavigationService.Navigate(new Uri("/Views/AppSettingsView.xaml", UriKind.Relative)); },
                () => !this.IsSynchronizing);

            this.IsBeingActivated();
        }

        public DelegateCommand AppSettingsCommand { get; set; }

        public ICollectionView ByLengthItems
        {
            get { return this.bylengthViewSource.View; }
        }

        public ICollectionView FavoriteItems
        {
            get { return this.favoritesViewSource.View; }
        }

        public DelegateCommand FiltersCommand { get; set; }

        public bool IsSynchronizing
        {
            get { return this.isSyncing; }
            set
            {
                this.isSyncing = value;
                this.RaisePropertyChanged(() => this.IsSynchronizing);
            }
        }

        public ICollectionView NewItems
        {
            get { return this.newSurveysViewSource.View; }
        }

        public int SelectedPivotIndex
        {
            get { return this.selectedPivotIndex; }

            set
            {
                this.selectedPivotIndex = value;
                this.HandleCurrentSectionChanged();
            }
        }

        public SurveyTemplateViewModel SelectedSurveyTemplate
        {
            get { return this.selectedSurveyTemplate; }

            set
            {
                if (value != null)
                {
                    this.selectedSurveyTemplate = value;
                    this.RaisePropertyChanged(() => this.SelectedSurveyTemplate);
                }
            }
        }

        public bool SettingAreConfigured
        {
            get { return !this.SettingAreNotConfigured; }
        }

        public bool SettingAreNotConfigured
        {
            get { return this.surveyStoreLocator.GetStore() is NullSurveyStore; }
        }

        public DelegateCommand StartSyncCommand { get; set; }

        public IInteractionRequest SubmitErrorInteractionRequest
        {
            get { return this.submitErrorInteractionRequest; }
        }

        public IInteractionRequest SubmitNotificationInteractionRequest
        {
            get { return this.submitNotificationInteractionRequest; }
        }

        public override void IsBeingDeactivated()
        {
            Tombstoning.Save("SelectedTemplate", this.SelectedSurveyTemplate);
            Tombstoning.Save("MainPivot", this.SelectedPivotIndex);

            base.IsBeingDeactivated();
        }

        public override sealed void IsBeingActivated()
        {
            if (this.selectedSurveyTemplate == null)
            {
                var tombstoned = Tombstoning.Load<SurveyTemplateViewModel>("SelectedTemplate");
                if (tombstoned != null)
                {
                    this.SelectedSurveyTemplate = new SurveyTemplateViewModel(tombstoned.Template, this.NavigationService);
                }

                this.selectedPivotIndex = Tombstoning.Load<int>("MainPivot");
            }
        }

        public void Refresh()
        {
            if (this.surveyStoreLocator.GetStore() != this.lastSurveyStore)
            {
                this.lastSurveyStore = this.surveyStoreLocator.GetStore();
                this.BuildPivotDimensions();
                this.RaisePropertyChanged(string.Empty);
                this.StartSyncCommand.RaiseCanExecuteChanged();
                this.FiltersCommand.RaiseCanExecuteChanged();
            }
        }

        public void StartSync()
        {
            if (this.IsSynchronizing)
            {
                return;
            }

            this.IsSynchronizing = true;
            this.synchronizationService
                .StartSynchronization()
                .ObserveOnDispatcher()
                .Subscribe(taskSummaries => this.SyncCompleted(taskSummaries));
        }

        private static string GetDescriptionForSummary(TaskCompletedSummary summary)
        {
            switch (summary.Result)
            {
                case TaskSummaryResult.Success:
                    switch (summary.Task)
                    {
                        case SurveysSynchronizationService.GetSurveysTask:
                            return string.Format(
                                CultureInfo.InvariantCulture,
                                "{0} new surveys",
                                summary.Context);
                        case SurveysSynchronizationService.SaveSurveyAnswersTask:
                            return string.Format(
                                CultureInfo.InvariantCulture,
                                "{0} answers sent",
                                summary.Context);
                        default:
                            return string.Format(
                                CultureInfo.InvariantCulture,
                                "{0}: {1}",
                                summary.Task,
                                TaskCompletedSummaryStrings.GetDescriptionForResult(summary.Result));
                    }
                default:
                    return TaskCompletedSummaryStrings.GetDescriptionForResult(summary.Result);
            }
        }

        private void BuildPivotDimensions()
        {
            this.observableSurveyTemplates = new ObservableCollection<SurveyTemplateViewModel>();
            List<SurveyTemplateViewModel> surveyTemplateViewModels = this.surveyStoreLocator.GetStore().GetSurveyTemplates().Select(t => new SurveyTemplateViewModel(t, this.NavigationService)
                                                                                                                    {
                                                                                                                        Completeness = this.surveyStoreLocator.GetStore().GetCurrentAnswerForTemplate(t) != null
                                                                                                                                           ? this.surveyStoreLocator.GetStore().GetCurrentAnswerForTemplate(t).
                                                                                                                                                 CompletenessPercentage
                                                                                                                                           : 0
                                                                                                                    }).ToList();
            surveyTemplateViewModels.ForEach(this.observableSurveyTemplates.Add);

            // Listen for survey changes
            // and calculate answersQty
            this.ListenSurveyChanges();

            // Create collection views
            this.newSurveysViewSource = new CollectionViewSource { Source = this.observableSurveyTemplates };
            this.bylengthViewSource = new CollectionViewSource { Source = this.observableSurveyTemplates };
            this.favoritesViewSource = new CollectionViewSource { Source = this.observableSurveyTemplates };

            this.newSurveysViewSource.Filter += (o, e) => e.Accepted = ((SurveyTemplateViewModel)e.Item).Template.IsNew;
            this.favoritesViewSource.Filter += (o, e) => e.Accepted = ((SurveyTemplateViewModel)e.Item).Template.IsFavorite;
            this.bylengthViewSource.SortDescriptions.Add(new SortDescription("Length", ListSortDirection.Ascending));

            this.newSurveysViewSource.View.CurrentChanged += (o, e) => this.SelectedSurveyTemplate = (SurveyTemplateViewModel)this.newSurveysViewSource.View.CurrentItem;
            this.favoritesViewSource.View.CurrentChanged += (o, e) => this.SelectedSurveyTemplate = (SurveyTemplateViewModel)this.favoritesViewSource.View.CurrentItem;
            this.bylengthViewSource.View.CurrentChanged += (o, e) => this.SelectedSurveyTemplate = (SurveyTemplateViewModel)this.bylengthViewSource.View.CurrentItem;

            // Initialize the selected survey template
            this.HandleCurrentSectionChanged();
        }

        private void HandleCurrentSectionChanged()
        {
            ICollectionView currentView = null;
            switch (this.SelectedPivotIndex)
            {
                case 0:
                    currentView = this.NewItems;
                    break;
                case 1:
                    currentView = this.FavoriteItems;
                    break;
                case 2:
                    currentView = this.ByLengthItems;
                    break;
            }

            if (currentView != null)
            {
                this.SelectedSurveyTemplate = (SurveyTemplateViewModel)currentView.CurrentItem;
            }
        }

        private void ListenSurveyChanges()
        {
            foreach (var template in this.observableSurveyTemplates)
            {
                template.PropertyChanged += (s, e) =>
                                                {
                                                    if (e.PropertyName == "IsFavorite")
                                                    {
                                                        this.favoritesViewSource.View.Refresh();
                                                    }
                                                };

                // Calculate Answers Qty
                SurveyTemplateViewModel templateCopy = template;
                template.AnswersQty = this.surveyStoreLocator.GetStore()
                    .GetCompleteSurveyAnswers()
                    .Count(a =>
                           a.Tenant == templateCopy.Tenant &&
                           a.SlugName == templateCopy.Template.SlugName);
            }
        }

        private void SyncCompleted(IEnumerable<TaskCompletedSummary> taskSummaries)
        {
            var stringBuilder = new StringBuilder();

            var errorSummary =
                taskSummaries.FirstOrDefault(
                    s =>
                        s.Result == TaskSummaryResult.UnreachableServer ||
                        s.Result == TaskSummaryResult.AccessDenied);

            if (errorSummary != null)
            {
                stringBuilder.AppendLine(GetDescriptionForSummary(errorSummary));
                this.submitErrorInteractionRequest.Raise(
                        new Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification { Title = "Synchronization error", Content = stringBuilder.ToString() },
                        n => { });
            }
            else
            {
                foreach (var task in taskSummaries)
                {
                    stringBuilder.AppendLine(GetDescriptionForSummary(task));
                }

                if (taskSummaries.Any(t => t.Result != TaskSummaryResult.Success))
                {
                    this.submitErrorInteractionRequest.Raise(
                        new Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification { Title = "Synchronization error", Content = stringBuilder.ToString() },
                        n => { });
                }
                else
                {
                    this.submitNotificationInteractionRequest.Raise(
                        new Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification { Title = stringBuilder.ToString(), Content = null },
                        n => { });
                }
            }

            // Update the View Model status
            this.BuildPivotDimensions();
            this.RaisePropertyChanged(string.Empty);
            this.IsSynchronizing = false;
            this.UpdateCommandsForSync();
        }

        private void UpdateCommandsForSync()
        {
            this.StartSyncCommand.RaiseCanExecuteChanged();
            this.FiltersCommand.RaiseCanExecuteChanged();
            this.AppSettingsCommand.RaiseCanExecuteChanged();
        }
    }
}