//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Services
{
    using System;
    using System.Device.Location;
    using TailSpin.PhoneClient.Services.Stores;

    public class LocationService : ILocationService
    {
        private readonly TimeSpan maximumAge = TimeSpan.FromMinutes(15);
        private readonly ISettingsStore settingsStore;
        private GeoCoordinate lastCoordinate = GeoCoordinate.Unknown;
        private DateTime lastCoordinateTime;
        private GeoCoordinateWatcher watcher;

        public LocationService(ISettingsStore settingsStore)
        {
            this.settingsStore = settingsStore;
        }

        public GeoCoordinate TryToGetCurrentLocation()
        {
            if (!this.settingsStore.LocationServiceAllowed)
            {
                return GeoCoordinate.Unknown;
            }

            if (this.watcher == null)
            {
                if (this.maximumAge < (DateTime.Now - this.lastCoordinateTime) || this.lastCoordinate == GeoCoordinate.Unknown)
                {
                    this.InitializeWatcher();
                }
            }

            return this.lastCoordinate;
        }

        private void InitializeWatcher()
        {
            this.watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            this.watcher.Start(true);
            if (this.watcher.Status == GeoPositionStatus.Initializing || this.watcher.Status == GeoPositionStatus.NoData)
            {
                this.watcher.StatusChanged += this.WatcherStatusChanged;
            }
            else
            {
                this.GetNewLocation();
            }
        }

        private void WatcherStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            this.GetNewLocation();
        }

        private void GetNewLocation()
        {
            if (this.watcher != null)
            {
                var newCoordinate = this.watcher.Position.Location;
                if (newCoordinate != GeoCoordinate.Unknown)
                {
                    this.lastCoordinate = this.watcher.Position.Location;
                    this.lastCoordinateTime = DateTime.Now;
                }

                this.DisposeWatcher();
            }
        }

        private void DisposeWatcher()
        {
            if (this.watcher != null)
            {
                var oldWatcher = this.watcher;
                this.watcher = null;
                oldWatcher.Stop();
                oldWatcher.Dispose();
            }
        }
    }
}