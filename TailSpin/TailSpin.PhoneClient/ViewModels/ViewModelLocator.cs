namespace TailSpin.PhoneClient.ViewModels
{
    using System;
    using TailSpin.PhoneClient.Services;
    using TailSpin.PhoneClient.Services.Stores;

    public class ViewModelLocator : IDisposable
    {
        private readonly ContainerLocator containerLocator;
        private bool disposed;

        public ViewModelLocator()
        {
            this.containerLocator = new ContainerLocator();
        }

        public AppSettingsViewModel AppSettingsViewModel
        {
            get { return this.containerLocator.Container.Resolve<AppSettingsViewModel>(); }
        }

        public FilterSettingsViewModel FilterSettingsViewModel
        {
            get { return this.containerLocator.Container.Resolve<FilterSettingsViewModel>(); }
        }

        public SurveyListViewModel SurveyListViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SurveyListViewModel>();
            }
        }

        public TakeSurveyViewModel TakeSurveyViewModel
        {
            get
            {
                var templateViewModel = this.SurveyListViewModel.SelectedSurveyTemplate;
                var vm = new TakeSurveyViewModel(this.containerLocator.Container.Resolve<INavigationService>())
                             {
                                 SurveyStoreLocator = this.containerLocator.Container.Resolve<ISurveyStoreLocator>(),
                                 LocationService = this.containerLocator.Container.Resolve<ILocationService>(),
                                 TemplateViewModel = templateViewModel,
                                 SurveyAnswer = this.containerLocator.Container.Resolve<ISurveyStoreLocator>().GetStore().GetCurrentAnswerForTemplate(templateViewModel.Template) ??
                                                templateViewModel.Template.CreateSurveyAnswers(this.containerLocator.Container.Resolve<ILocationService>())
                             };
                vm.Initialize();
                return vm;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.containerLocator.Dispose();
            }

            this.disposed = true;
        }
    }
}
