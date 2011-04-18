




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using System;
    using System.Collections.Generic;

    public interface IUserDeviceStore
    {
        void Initialize();
        void SetUserDevice(string username, Uri deviceUri);
        void RemoveUserDevice(Uri deviceUri);
        IEnumerable<Uri> GetDevices(string username);
    }
}