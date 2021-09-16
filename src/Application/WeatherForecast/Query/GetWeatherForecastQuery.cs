﻿namespace Exemplum.Application.WeatherForecast.Query
{
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

        public GetWeatherForecastQueryHandler(IWeatherForecastClient client,
            IOptions<WeatherForecastOptions> options)
        {
            _client = client;
            _options = options.Value;
        }
        
        public Task<WeatherForecast> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
        {
            return _client.GetForecast(request.Lat, request.Lon, _options.AppId);
        }
    }
}