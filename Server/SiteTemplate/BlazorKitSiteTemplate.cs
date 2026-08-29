using BlazorKit.Application.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using System;
using System.Collections.Generic;
using System.Text.Json;
using BlazorKit.Application.Repository;

namespace BlazorKit.Application.SiteTemplate
{
	public class BlazorKitSiteTemplate : ISiteTemplate
	{
		private readonly ISiteRepository _siteRepository;
		private readonly IWebHostEnvironment _environment;
		private readonly IWeatherDataRepository _weatherDataRepository;

		public BlazorKitSiteTemplate(ISiteRepository siteRepository, IWebHostEnvironment environment, IWeatherDataRepository weatherDataRepository)
		{
			_siteRepository = siteRepository;
			_environment = environment;
			_weatherDataRepository = weatherDataRepository;
		}

		public string Name
		{
			get { return "BlazorKit Site Template"; }
		}

		public List<PageTemplate> CreateSite(Site site)
		{
			// set default theme
			site.Name = "BlazorKit";
			site.DefaultThemeType = "BlazorKit.Application.BlazorKitTheme.Default, BlazorKit.Application.Client.Oqtane";
			site.DefaultContainerType = "BlazorKit.Application.BlazorKitTheme.Container, BlazorKit.Application.Client.Oqtane";
			_siteRepository.UpdateSite(site);

			// populate seed weather data
			var lines = System.IO.File.ReadLines($"{_environment.WebRootPath}\\weather.txt");
			foreach (var line in lines)
			{
                var cols = line.Split('\t');
                if (cols.Length == 8)
                {
                    var data = new WeatherData();
                    data.SiteId = site.SiteId;
                    data.City = cols[0];
                    data.Date = DateTime.Parse(cols[1]);
                    data.HighTemperature = decimal.Parse(cols[2]);
                    data.LowTemperature = decimal.Parse(cols[3]);
                    data.Humidity = decimal.Parse(cols[4]);
                    data.WindSpeed = decimal.Parse(cols[5]);
                    data.AirPressure = decimal.Parse(cols[6]);
                    data.Precipitation = decimal.Parse(cols[7]);
                    _weatherDataRepository.AddWeatherData(data);
                }
            }
	
			// default pages/modules
			var pageTemplates = new List<PageTemplate>();
            pageTemplates.Add(new PageTemplate
            {
                Name = "Dashboard",
                Parent = "",
                Path = "/",
                Order = 1,
                Icon = "oi oi-home",
                IsNavigation = true,
                IsPersonalizable = false,
                PermissionList = new List<Permission>
                {
                    new Permission(PermissionNames.View, RoleNames.Admin, true),
                    new Permission(PermissionNames.View, RoleNames.Everyone, true),
                    new Permission(PermissionNames.Edit, RoleNames.Admin, true)
                },
                PageTemplateModules = new List<PageTemplateModule>
                {
                    new PageTemplateModule
                    {
                        ModuleDefinitionName = typeof(BlazorKit.Application.Weather.Index).ToModuleDefinitionName(), Title = "Weather", Pane = PaneNames.Default,
                        PermissionList = new List<Permission>
                        {
                            new Permission(PermissionNames.View, RoleNames.Admin, true),
                            new Permission(PermissionNames.View, RoleNames.Everyone, true),
                            new Permission(PermissionNames.Edit, RoleNames.Admin, true)
                        }
                    }
                }
            }); 
			pageTemplates.Add(new PageTemplate
			{
				Name = "About",
				Parent = "",
				Path = "about",
				Order = 3,
				Icon = "oi oi-info",
				IsNavigation = true,
				IsPersonalizable = false,
				PermissionList = new List<Permission>
				{
					new Permission(PermissionNames.View, RoleNames.Admin, true),
					new Permission(PermissionNames.View, RoleNames.Everyone, true),
					new Permission(PermissionNames.Edit, RoleNames.Admin, true)
				},
				PageTemplateModules = new List<PageTemplateModule>
				{
					new PageTemplateModule
					{
						ModuleDefinitionName = typeof(Oqtane.Modules.HtmlText.Index).ToModuleDefinitionName(), Title = "BlazorKit", Pane = PaneNames.Default,
						PermissionList = new List<Permission>
						{
							new Permission(PermissionNames.View, RoleNames.Admin, true),
							new Permission(PermissionNames.View, RoleNames.Everyone, true),
							new Permission(PermissionNames.Edit, RoleNames.Admin, true)
						},
						Content = "<h1>Welcome to BlazorKit</h1><p>This is a sample BlazorKit site template.</p>"
					}
				}
			});

			return pageTemplates;
		}
	}
}
