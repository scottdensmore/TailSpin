namespace TailSpin.PhoneClient.Services.RegistrationService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Phone.Notification;
    using Microsoft.Phone.Reactive;
    using TailSpin.PhoneClient.Models;
    using TailSpin.PhoneClient.Services.Clients;
    using TailSpin.PhoneClient.Services.Stores;
    using TailSpin.Services.Surveys.Registration;

    public class RegistrationServiceClient : IRegistrationServiceClient, IDisposable
    {
        private const string ChannelName = "tailspindemo.cloudapp.net";
        private const string ServiceName = "TailSpinRegistrationService";
        private readonly Uri serviceUri;
        private readonly ISettingsStore settingsStore;
        private HttpNotificationChannel httpChannel;
        private bool disposed;

        public RegistrationServiceClient(Uri serviceUri, ISettingsStore settingsStore)
        {
            this.serviceUri = serviceUri;
            this.settingsStore = settingsStore;
        }

        public IObservable<SurveyFiltersInformation> GetSurveysFilterInformation()
        {
            var uri = new Uri(this.serviceUri, "GetFilters");
            return
                HttpClient
                    .RequestTo(uri, this.settingsStore.UserName, this.settingsStore.Password)
                    .GetJson<SurveyFiltersInformationDto>()
                    .Select(
                        surveyFiltersInformationDto =>
                            new SurveyFiltersInformation
                                               {
                                                   AllTenants =
                                                       surveyFiltersInformationDto.AllTenants
                                                       .Select(t => new TenantItem { Key = t.Key, Name = t.Name }),
                                                   SelectedTenants =
                                                       surveyFiltersInformationDto.SurveyFilters.Tenants
                                                       .Select(t => new TenantItem { Key = t.Key, Name = t.Name })
                                               });
        }

        public IObservable<Unit> UpdateTenants(IEnumerable<TenantItem> tenants)
        {
            var surveyFiltersDto = new SurveyFiltersDto { Tenants = tenants.Select(t => new TenantDto { Key = t.Key }).ToArray() };

            var uri = new Uri(this.serviceUri, "SetFilters");
            return
                HttpClient
                    .RequestTo(uri, this.settingsStore.UserName, this.settingsStore.Password)
                    .PostJson(surveyFiltersDto);
        }

        public IObservable<TaskSummaryResult> UpdateReceiveNotifications(bool receiveNotifications)
        {
            this.httpChannel = HttpNotificationChannel.Find(ChannelName);

            if (receiveNotifications)
            {
                if (this.httpChannel != null)
                {
                    this.httpChannel.Close();
                    this.httpChannel.Dispose();
                    this.httpChannel = null;
                }

                this.httpChannel = new HttpNotificationChannel(ChannelName, ServiceName);

                var channelUriUpdated =
                    from o in Observable.FromEvent<NotificationChannelUriEventArgs>(
                        h => this.httpChannel.ChannelUriUpdated += h,
                        h => this.httpChannel.ChannelUriUpdated -= h)
                    from summary in this.BindChannelAndUpdateDeviceUriInService()
                    select summary;

                // If this line is not present, the above subcription is never executed
                this.httpChannel.ChannelUriUpdated += (s, a) => { };

                var channelUriUpdateFail =
                    from o in Observable.FromEvent<NotificationChannelErrorEventArgs>(
                            h => this.httpChannel.ErrorOccurred += h,
                            h => this.httpChannel.ErrorOccurred -= h)
                    select TaskSummaryResult.UnknownError;

                this.httpChannel.Open();

                // If the notification service does not respond in time, it is assumed that the server is unreachable
                var timeout =
                    Observable
                        .Interval(TimeSpan.FromSeconds(60))
                        .Select(i => TaskSummaryResult.UnreachableServer);

                // The first event that happens is returned
                return channelUriUpdated.Amb(channelUriUpdateFail).Amb(timeout).Take(1);
            }
            
            if (this.httpChannel != null && this.httpChannel.ChannelUri != null)
            {
                return
                    this.CallUpdateReceiveNotifications(false, this.httpChannel.ChannelUri)
                        .Select(
                            s =>
                                {
                                    this.httpChannel.Close();
                                    this.httpChannel.Dispose();
                                    this.httpChannel = null;

                                    return s;
                                });
            }

            return Observable.Return(TaskSummaryResult.Success);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private IObservable<TaskSummaryResult> BindChannelAndUpdateDeviceUriInService()
        {
            this.httpChannel = HttpNotificationChannel.Find(ChannelName);

            if (!this.httpChannel.IsShellToastBound)
            {
                this.httpChannel.BindToShellToast();
            }

            if (!this.httpChannel.IsShellTileBound)
            {
                this.httpChannel.BindToShellTile();
            }

            return this.CallUpdateReceiveNotifications(true, this.httpChannel.ChannelUri);
        }

        private IObservable<TaskSummaryResult> CallUpdateReceiveNotifications(bool receiveNotifications, Uri channelUri)
        {
            var device = new DeviceDto
            {
                Uri = channelUri.ToString(),
                RecieveNotifications = receiveNotifications
            };

            var uri = new Uri(this.serviceUri, "Notifications");
            return
                HttpClient
                    .RequestTo(uri, this.settingsStore.UserName, this.settingsStore.Password)
                    .PostJson(device)
                    .Select(u => TaskSummaryResult.Success);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.httpChannel != null)
                {
                    this.httpChannel.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}