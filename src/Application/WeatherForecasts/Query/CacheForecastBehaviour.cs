namespace Exemplum.Application.WeatherForecasts.Query;

using Common.Caching;
using MediatR;
using Models;

public class CacheForecastBehaviour : IPipelineBehavior<GetWeatherForecastQuery, WeatherForecast>
{
    private readonly IApplicationCache<WeatherForecast> _cache;

    public CacheForecastBehaviour(IApplicationCache<WeatherForecast> cache)
    {
        _cache = cache;
    }

    public async Task<WeatherForecast> Handle(GetWeatherForecastQuery request, RequestHandlerDelegate<WeatherForecast> next, CancellationToken cancellationToken)
    {
        string key = request?.ToString() ?? string.Empty;

        var response = await _cache.GetOrAddAsync(key, () => next(), cancellationToken: cancellationToken);

        return response;
    }
}