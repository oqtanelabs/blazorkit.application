using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using BlazorKit.Application.Models;
using BlazorKit.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorKit.Application.Jobs
{
    public class WeatherJob : HostedServiceBase
    {
        // JobType = "BlazorKit.Application.Jobs.WeatherJob, BlazorKit.Application.Server.Oqtane"

        private ITenantManager _TenantManager;
        private ISiteRepository _SiteRepository;
		private IWeatherDataRepository _WeatherDataRepository;

        public WeatherJob(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            Name = "Weather Job";
            Frequency = "d"; // days
            Interval = 1;
            IsEnabled = false;
        }

        // job is executed for each tenant in installation
        public override string ExecuteJob(IServiceProvider provider)
        {
            string log = "";

            // get services
            _TenantManager = provider.GetRequiredService<ITenantManager>();
            _SiteRepository = provider.GetRequiredService<ISiteRepository>();
            _WeatherDataRepository = provider.GetRequiredService<IWeatherDataRepository>();

            // iterate through sites for current tenant
            List<Site> sites = _SiteRepository.GetSites().ToList();
            foreach (Site site in sites)
            {
                log += $"Processing Site {site.Name}<br />";

                var tenantId = _TenantManager.GetTenant().TenantId;
                _TenantManager.SetAlias(tenantId, site.SiteId);

                // get existing weather data
                var weatherData = _WeatherDataRepository.GetAllWeatherData(site.SiteId, DateTime.MinValue, DateTime.MaxValue, "");

                // ** note that a real implementation would use an external service to get latest weather information

                // create synthetic weather data for each city
                foreach (var city in _WeatherDataRepository.GetWeatherCities(site.SiteId))
                {
                    var cityWeather = weatherData.Where(item => item.City == city).ToList();
                    var minHighTemperature = cityWeather.Min(item => item.HighTemperature);
                    var maxHighTemperature = cityWeather.Max(item => item.HighTemperature);
                    var minLowTemperature = cityWeather.Min(item => item.LowTemperature);
                    var maxLowTemperature = cityWeather.Max(item => item.LowTemperature);
                    var minHumidity = cityWeather.Min(item => item.Humidity);
                    var maxHumidity = cityWeather.Max(item => item.Humidity);
                    var minWindSpeed = cityWeather.Min(item => item.WindSpeed);
                    var maxWindSpeed = cityWeather.Max(item => item.WindSpeed);
                    var minAirPressure = cityWeather.Min(item => item.AirPressure);
                    var maxAirPressure = cityWeather.Max(item => item.AirPressure);
                    var minPrecipitation = cityWeather.Min(item => item.Precipitation);
                    var maxPrecipitation = cityWeather.Max(item => item.Precipitation);

                    for (DateTime date = cityWeather.Max(item => item.Date).AddDays(1).Date; date <= DateTime.UtcNow.Date; date = date.AddDays(1))
                    {
                        var data = new WeatherData();
                        data.SiteId = site.SiteId;
                        data.City = city;
                        data.Date = date;
                        data.HighTemperature = new Random().Next((int)minHighTemperature, (int)maxHighTemperature);
                        data.LowTemperature = new Random().Next((int)minLowTemperature, (int)maxLowTemperature);
                        data.Humidity = new Random().Next((int)minHumidity, (int)maxHumidity);
                        data.WindSpeed = new Random().Next((int)minWindSpeed, (int)maxWindSpeed);
                        data.AirPressure = new Random().Next((int)(minAirPressure * 100), (int)(maxAirPressure * 100)) / 100.0m;
                        data.Precipitation = new Random().Next((int)(minPrecipitation * 100), (int)(maxPrecipitation * 100)) / 100.0m;
                        _WeatherDataRepository.AddWeatherData(data);

                        log += $"Created Weather Data For {city} And Date {date.ToShortDateString()}<br />";
                    }
                }
            }

            return log;
        }
    }
}
