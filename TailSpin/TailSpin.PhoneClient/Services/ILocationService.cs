namespace TailSpin.PhoneClient.Services
{
    using System.Device.Location;

    public interface ILocationService
    {
        GeoCoordinate TryToGetCurrentLocation();
    }
}
