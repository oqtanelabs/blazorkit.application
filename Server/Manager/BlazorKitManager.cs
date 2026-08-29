using BlazorKit.Application.Repository;
using Microsoft.IdentityModel.Tokens;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace BlazorKit.Application.Manager
{
    public class BlazorKitManager : MigratableModuleBase, IInstallable, ISearchable
    {
        private readonly IDBContextDependencies _DBContextDependencies;
        private readonly IWeatherDataRepository _WeatherDataRepository;

        public BlazorKitManager(IDBContextDependencies DBContextDependencies, IWeatherDataRepository WeatherDataRepository)
        {
            _DBContextDependencies = DBContextDependencies;
            _WeatherDataRepository = WeatherDataRepository;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new BlazorKitContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new BlazorKitContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
            var searchContentList = new List<SearchContent>();

            foreach (var city in _WeatherDataRepository.GetWeatherCities(pageModule.Module.SiteId))
            {
                searchContentList.Add(new SearchContent
                {
                    EntityName = "City",
                    EntityId = city,
                    Title = city,
                    Description = "",
                    Body = $"Weather Information Include Temperature, Humidity, Wind, and Precipitation",
                    Url = $"/?city={WebUtility.UrlEncode(city)}",
                    ContentModifiedBy = "host",
                    ContentModifiedOn = lastIndexedOn
                });
            }

            return Task.FromResult(searchContentList);
        }

    }
}
