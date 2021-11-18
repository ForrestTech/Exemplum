namespace Exemplum.WebApp.Location;

public interface ILocationService
{
    Task<GeoLocation?> GetCurrentLocation();
}