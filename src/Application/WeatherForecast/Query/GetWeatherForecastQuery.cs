namespace Exemplum.Application.WeatherForecast.Query
{
    using Common.Policies;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Options;
    using Model;
    using Polly;
    using Polly.Caching;
    using Polly.Registry;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetWeatherForecastQuery : IRequest<WeatherForecast>
    {
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }

        public override string ToString()
        {
            return $"{nameof(GetWeatherForecastQuery)}_{Lat}_{Lon}";
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
        private readonly WeatherForecastOptions _options;
        private readonly AsyncCachePolicy _cachePolicy;

        public GetWeatherForecastQueryHandler(IWeatherForecastClient client,
            IOptions<WeatherForecastOptions> options,
            IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _client = client;
            _cachePolicy = policyRegistry.Get<AsyncCachePolicy>(ExecutionPolicy.CachingPolicy); 
            _options = options.Value;
        }
        
        public async Task<WeatherForecast> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
        {
            var weatherForecast = await _cachePolicy.ExecuteAsync(context => _client.GetForecast(request.Lat, request.Lon, _options.AppId), 
                new Context(request.ToString()));

            return weatherForecast;
        }
    }
}