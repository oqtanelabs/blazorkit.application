using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

                // a real implementation would use an external service to get latest weather information
                foreach (var city in _WeatherDataRepository.GetWeatherCities(site.SiteId))
                {
                    var data = new WeatherData();
                    data.City = city;
                    data.Date = DateTime.UtcNow.Date;
                    data.HighTemperature = new Random().Next(70, 100);
                    data.LowTemperature = new Random().Next(50, 80);
                    data.Humidity = new Random().Next(30, 80);
                    data.WindSpeed = new Random().Next(0, 20);
                    data.AirPressure = new Random().Next(28, 32);
                    data.Precipitation = new Random().Next(0, 2);
                    _WeatherDataRepository.AddWeatherData(data);
                }

                log += $"Weather Data Retrieved For {DateTime.Now.ToShortDateString()}<br />";
            }

            return log;
        }
    }
}
