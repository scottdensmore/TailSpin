//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using Microsoft.Phone.Net.NetworkInformation;
    using Microsoft.Phone.Reactive;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
    using TailSpin.PhoneClient.Infrastructure;
    using TailSpin.PhoneClient.Models;
    using TailSpin.PhoneClient.Services;
    using TailSpin.PhoneClient.Services.RegistrationService;
    using TailSpin.PhoneClient.Services.Stores;
    using Notification = Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification;

    public class FilterSettingsViewModel : ViewModel
    {
        private readonly IRegistrationServiceClient registrationServiceClient;
        private readonly InteractionRequest<Notification> submitErrorInteractionRequest;
        private readonly ISurveyStoreLocator surveyStoreLocator;
        private readonly ObservableCollection<TenantItem> tenants;
        private List<TenantItem> tombstonedTenants;
        private bool canSubmit;
        private bool isSyncing;
        private IEnumerable<TenantItem> selectedTenantsInService;

        public FilterSettingsViewModel(
            IRegistrationServiceClient registrationServiceClient, 
            INavigationService navigationService,
            ISurveyStoreLocator surveyStoreLocator) : base(navigationService)
        {
            this.registrationServiceClient = registrationServiceClient;
            this.surveyStoreLocator = surveyStoreLocator;
            this.submitErrorInteractionRequest = new InteractionRequest<Notification>();
            this.tenants = new ObservableCollection<TenantItem>();

            this.IsBeingActivated();

            if (this.tombstonedTenants == null)
            {
                this.CanSubmit = false;
                this.IsSyncing = true;

                this.registrationServiceClient
                    .GetSurveysFilterInformation()
                    .ObserveOnDispatcher()
                    .Subscribe(
                        surveyFiltersInformation =>
                            {
                                this.selectedTenantsInService = surveyFiltersInformation.SelectedTenants;
                                surveyFiltersInformation.AllTenants.ToList().ForEach(t => this.tenants.Add(t));

                                if (surveyFiltersInformation.SelectedTenants.Count() > 0)
                                {
                                    this.tenants
                                        .Intersect(surveyFiltersInformation.SelectedTenants, new TenantItemComparer())
                                        .ToList().ForEach(t => t.IncludeInFilter = true);
                                }
                                else
                                {
                                    this.tenants.ToList().ForEach(t => t.IncludeInFilter = true);
                                }

                                this.CanSubmit = true;
                                this.IsSyncing = false;
                            },
                        exception =>
                            {
                                if (exception is WebException)
                                {
                                    this.HandleWebException(exception as WebException, () => this.NavigationService.GoBack());
                                }
                                else if (exception is UnauthorizedAccessException)
                                {
                                    this.HandleUnauthorizedException(() => this.NavigationService.GoBack());
                                }
                                else
                                {
                                    throw exception;
                                }
                            });
            }
            else
            {
                this.tombstonedTenants.ForEach(t => this.tenants.Add(t));
            }

            this.SaveCommand = new DelegateCommand(this.Submit, () => !this.CanSubmit);
        }

        public bool CanSubmit
        {
            get { return this.canSubmit; }
            set
            {
                if (!value.Equals(this.canSubmit))
                {
                    this.canSubmit = value;
                    this.RaisePropertyChanged(() => this.CanSubmit);
                }
            }
        }

        public IInteractionRequest SubmitErrorInteractionRequest { get { return this.submitErrorInteractionRequest; } }

        public bool IsSyncing
        {
            get { return this.isSyncing; }
            set
            {
                this.isSyncing = value;
                this.RaisePropertyChanged(() => this.IsSyncing);
            }
        }

        public bool NetworkAvailable
        {
            get { return NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None; }
        }

        public DelegateCommand SaveCommand { get; set; }

        public IEnumerable<TenantItem> Tenants
        {
            get { return this.tenants; }
        }

        public void Submit()
        {
            this.IsSyncing = true;

            var selectedTenants = (from tenant in this.tenants where tenant.IncludeInFilter select tenant).ToList();
            if (selectedTenants.Count < 1)
            {
                selectedTenants = (from tenant in this.tenants select tenant).ToList();
            }

            if (!this.selectedTenantsInService.SequenceEqual(selectedTenants, new TenantItemComparer()))
            {
                this.registrationServiceClient
                    .UpdateTenants(selectedTenants)
                    .ObserveOnDispatcher()
                    .Subscribe(
                        unused =>
                            {
                                this.selectedTenantsInService = selectedTenants;
                                this.surveyStoreLocator.GetStore().LastSyncDate = string.Empty;
                                this.IsSyncing = false;
                                this.NavigationService.GoBack();
                            },
                        exception =>
                            {
                                if (exception is WebException)
                                {
                                    HandleWebException(exception as WebException, () => { });
                                }
                                else if (exception is InvalidOperationException)
                                {
                                    this.IsSyncing = false;
                                    this.submitErrorInteractionRequest.Raise(
                                        new Notification { Title = "Warning", Content = exception.Message },
                                        n => { });
                                }
                                else
                                {
                                    throw exception;
                                }
                            });
            }
            else
            {
                this.IsSyncing = false;
                this.NavigationService.GoBack();
            }
        }

        public override void IsBeingDeactivated()
        {
            if (this.Tenants != null)
            {
                Tombstoning.Save("SettingsTenants", this.Tenants.ToList());
            }

            if (this.selectedTenantsInService != null)
            {
                Tombstoning.Save("SettingsSelectedTenantsInService", this.selectedTenantsInService.ToList());
            }

            base.IsBeingDeactivated();
        }

        public override sealed void IsBeingActivated()
        {
            this.tombstonedTenants = Tombstoning.Load<List<TenantItem>>("SettingsTenants");
            this.selectedTenantsInService = Tombstoning.Load<List<TenantItem>>("SettingsSelectedTenantsInService");
        }

        private void HandleWebException(WebException webException, Action afterNotification)
        {
            var summary = ExceptionHandling.GetSummaryFromWebException(string.Empty, webException);
            var errorText = TaskCompletedSummaryStrings.GetDescriptionForResult(summary.Result);
            this.IsSyncing = false;
            this.submitErrorInteractionRequest.Raise(
                new Notification { Title = "Server error", Content = errorText },
                n => afterNotification());
        }

        private void HandleUnauthorizedException(Action afterNotification)
        {
            this.IsSyncing = false;
            this.submitErrorInteractionRequest.Raise(
                new Notification { Title = "Server error", Content = TaskCompletedSummaryStrings.GetDescriptionForResult(TaskSummaryResult.AccessDenied) },
                n => afterNotification());
        }

        private class TenantItemComparer : IEqualityComparer<TenantItem>
        {
            public bool Equals(TenantItem x, TenantItem y)
            {
                return x.Key == y.Key;
            }

            public int GetHashCode(TenantItem obj)
            {
                return obj.Key.GetHashCode();
            }
        }
    }
}