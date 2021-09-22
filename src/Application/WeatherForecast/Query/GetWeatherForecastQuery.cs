namespace Exemplum.Application.WeatherForecast.Query
{
    using Common.Caching;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Options;
    using Model;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetWeatherForecastQuery : IRequest<WeatherForecast>
    {
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }

        public override string ToString()
        {
            return $"{Lat}-{Lon}";
        }
    }

    public class GetWeatherForecastQueryValidator : AbstractValidator<GetWeatherForecastQuery>
    {
        public GetWeatherForecastQueryValidator()
        {
            RuleFor(x => x.Lat).GreaterThanOrEqualTo(1);
            RuleFor(x => x.Lon).GreaterThanOrEqualTo(1);
        }
    }

    public class GetWeatherForecastQueryHandler : IRequestHandler<GetWeatherForecastQuery, WeatherForecast>
    {
        private readonly IWeatherForecastClient _client;
        private readonly IApplicationCache<WeatherForecast> _cache;
        private readonly WeatherForecastOptions _options;

        public GetWeatherForecastQueryHandler(IWeatherForecastClient client,
            IOptions<WeatherForecastOptions> options,
            IApplicationCache<WeatherForecast> cache)
        {
            _client = client;
            _cache = cache;
            _options = options.Value;
        }

        public async Task<WeatherForecast> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
        {
            var weatherForecast = await _cache.GetOrAddAsync(request.ToString(),
                () => _client.GetForecast(request.Lat, request.Lon, _options.AppId, cancellationToken),
                cancellationToken: cancellationToken);

            return weatherForecast;
        }
    }
}