namespace Exemplum.WebApp.Location;

using Microsoft.JSInterop;

public class LocationService : ILocationService
{
    private readonly IJSRuntime _jsRuntime;

    public LocationService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<GeoLocation?> GetCurrentLocation()
    {
        var geoLocation = await _jsRuntime.InvokeAsync<GeoLocation>("getCurrentLocation");

        return geoLocation;
    }
}