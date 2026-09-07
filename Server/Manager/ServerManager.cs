using BlazorKit.Application.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Repository;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BlazorKit.Application.Manager
{
    public class ServerManager : MigratableModuleBase, IInstallable, ISearchable, ISitemap
    {
        private readonly IDBContextDependencies _DBContextDependencies;
        private readonly IWeatherDataRepository _WeatherDataRepository;

        public ServerManager(IDBContextDependencies DBContextDependencies, IWeatherDataRepository WeatherDataRepository)
        {
            _DBContextDependencies = DBContextDependencies;
            _WeatherDataRepository = WeatherDataRepository;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new DatabaseContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new DatabaseContext(_DBContextDependencies), tenant, MigrationType.Down);
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
                    Body = $"Weather Information Including Temperature, Humidity, Wind, and Precipitation",
                    Url = $"/?city={WebUtility.UrlEncode(city)}",
                    ContentModifiedBy = "host",
                    ContentModifiedOn = lastIndexedOn
                });
            }

            return Task.FromResult(searchContentList);
        }

		public List<Sitemap> GetUrls(string alias, string path, Module module)
        {
			var sitemap = new List<Sitemap>();

			foreach (var city in _WeatherDataRepository.GetWeatherCities(module.SiteId))
			{
				sitemap.Add(new Sitemap { Url = $"/?city={WebUtility.UrlEncode(city)}", ModifiedOn = DateTime.UtcNow });
			}

            return sitemap;
		}
	}
}
