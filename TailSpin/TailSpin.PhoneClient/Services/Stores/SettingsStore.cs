namespace TailSpin.PhoneClient.Services.Stores
{
    using System;
    using System.Collections.Generic;
    using System.IO.IsolatedStorage;

    public class SettingsStore : ISettingsStore
    {
        private const bool LocationServiceSettingDefault = false;
        private const string LocationServiceSettingKeyName = "LocationService";
        private const string PasswordSettingDefault = "";
        private const string PasswordSettingKeyName = "PasswordSetting";
        private const bool PushNotificationSettingDefault = false;
        private const string PushNotificationSettingKeyName = "PushNotification";
        private const string UserNameSettingDefault = "";
        private const string UserNameSettingKeyName = "UserNameSetting";
        private readonly IsolatedStorageSettings isolatedStore;

        public SettingsStore()
        {
            this.isolatedStore = IsolatedStorageSettings.ApplicationSettings;
        }

        public string Password
        {
            get { return this.GetValueOrDefault(PasswordSettingKeyName, PasswordSettingDefault); }
            set { this.AddOrUpdateValue(PasswordSettingKeyName, value); }
        }

        public bool SubscribeToPushNotifications
        {
            get { return this.GetValueOrDefault(PushNotificationSettingKeyName, PushNotificationSettingDefault); }
            set { this.AddOrUpdateValue(PushNotificationSettingKeyName, value); }
        }

        public bool LocationServiceAllowed
        {
            get { return this.GetValueOrDefault(LocationServiceSettingKeyName, LocationServiceSettingDefault); }
            set { this.AddOrUpdateValue(LocationServiceSettingKeyName, value); }
        }

        public string UserName
        {
            get { return this.GetValueOrDefault(UserNameSettingKeyName, UserNameSettingDefault); }
            set { this.AddOrUpdateValue(UserNameSettingKeyName, value); }
        }

        private void AddOrUpdateValue(string key, object value)
        {
            bool valueChanged = false;

            try
            {
                // if new value is different, set the new value.
                if (this.isolatedStore[key] != value)
                {
                    this.isolatedStore[key] = value;
                    valueChanged = true;
                }
            }
            catch (KeyNotFoundException)
            {
                this.isolatedStore.Add(key, value);
                valueChanged = true;
            }
            catch (ArgumentException)
            {
                this.isolatedStore.Add(key, value);
                valueChanged = true;
            }

            if (valueChanged)
            {
                this.Save();
            }
        }

        private T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;

            try
            {
                value = (T)this.isolatedStore[key];
            }
            catch (KeyNotFoundException)
            {
                value = defaultValue;
            }
            catch (ArgumentException)
            {
                value = defaultValue;
            }

            return value;
        }

        private void Save()
        {
            this.isolatedStore.Save();
        }
    }
}