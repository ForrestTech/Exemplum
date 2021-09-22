namespace Exemplum.Application.WeatherForecasts.Query
{
    using Common.Caching;
    using MediatR;
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    public class CacheForecastBehaviour : IPipelineBehavior<GetWeatherForecastQuery, WeatherForecast> 
    {
        private readonly IApplicationCache<WeatherForecast> _cache;

        public CacheForecastBehaviour(IApplicationCache<WeatherForecast> cache)
        {
            _cache = cache;
        }

        public async Task<WeatherForecast> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken,
            RequestHandlerDelegate<WeatherForecast> next)
        {
            string key = request?.ToString() ?? string.Empty;

            var response = await _cache.GetOrAddAsync(key, () => next(), cancellationToken: cancellationToken);

            return response;
        }
    }
}