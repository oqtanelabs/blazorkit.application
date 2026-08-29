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

                // create random weather data for each city for each day since the last date
                foreach (var city in _WeatherDataRepository.GetWeatherCities(site.SiteId))
                {
                    for (DateTime date = weatherData.Max(item => item.Date).AddDays(1).Date; date <= DateTime.UtcNow.Date; date = date.AddDays(1))
                    {
                        var data = new WeatherData();
                        data.SiteId = site.SiteId;
                        data.City = city;
                        data.Date = date;
                        data.HighTemperature = new Random().Next(70, 100);
                        data.LowTemperature = new Random().Next(50, 80);
                        data.Humidity = new Random().Next(30, 80);
                        data.WindSpeed = new Random().Next(0, 20);
                        data.AirPressure = new Random().Next(28, 32);
                        data.Precipitation = new Random().Next(0, 2);
                        _WeatherDataRepository.AddWeatherData(data);

                        log += $"Created Weather Data For {city} And Date {date.ToShortDateString()}<br />";
                    }
                }
            }

            return log;
        }
    }
}
