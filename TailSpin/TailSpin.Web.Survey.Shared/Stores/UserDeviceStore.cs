




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TailSpin.Web.Survey.Shared.Stores.AzureStorage;

    public class UserDeviceStore : IUserDeviceStore
    {
        private readonly IAzureTable<UserDeviceRow> userDeviceTable;

        public UserDeviceStore(IAzureTable<UserDeviceRow> userDeviceTable)
        {
            this.userDeviceTable = userDeviceTable;
        }

        public void Initialize()
        {
            this.userDeviceTable.EnsureExist();
        }

        public void SetUserDevice(string username, Uri deviceUri)
        {
            this.RemoveUserDevice(deviceUri);

            var encodedUri = Convert.ToBase64String(Encoding.UTF8.GetBytes(deviceUri.ToString()));
            this.userDeviceTable.Add(
                new UserDeviceRow
                    {
                        PartitionKey = username,
                        RowKey = encodedUri
                    });
        }

        public void RemoveUserDevice(Uri deviceUri)
        {
            var encodedUri = Convert.ToBase64String(Encoding.UTF8.GetBytes(deviceUri.ToString()));
            var row =
                (from r in this.userDeviceTable.Query
                 where r.RowKey == encodedUri
                 select r).SingleOrDefault();

            if (row != null)
            {
                this.userDeviceTable.Delete(row);
            }
        }

        public IEnumerable<Uri> GetDevices(string username)
        {
            return
                (from deviceRow in this.userDeviceTable.Query
                 where deviceRow.PartitionKey == username
                 select deviceRow)
                 .ToList()
                 .Select(deviceRow => new Uri(Encoding.UTF8.GetString(Convert.FromBase64String(deviceRow.RowKey))));
        }
    }
}