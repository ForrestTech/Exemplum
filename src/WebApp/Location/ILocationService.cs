namespace Exemplum.WebApp.Location
{
    using System.Threading.Tasks;

    public interface ILocationService
    {
        Task<GeoLocation?> GetCurrentLocation();
    }
}