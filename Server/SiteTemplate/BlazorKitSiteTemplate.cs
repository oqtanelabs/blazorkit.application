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
						Content = "<img src=\"blazorkit.png\" alt=\"BlazorKit\" style=\"float: right; margin: 15px; width: 300px;\">\r\n\r\n<p>The power of open source. Built for business.</p>\r\n\r\n<p>BlazorKit brings the capabilities of Blazor and the Oqtane framework to organizations that need more... more scale, stronger security, enterprise-grade reliability, and expert support. Built on the same trusted open-source foundation, BlazorKit adds the features and services businesses need to deploy confidently in production and operate at scale.</p>\r\n\r\n<p>Why choose BlazorKit?</p>\r\n\r\n<ul>\r\n<li>Expert Support - Get direct access to experienced experts when you need help.</li>\r\n<li>Service Level Agreement (SLA) - Prioritized assistance when your business needs it the most.</li>\r\n<li>Confidential Communication - Dedicated, private channels ensure sensitive information is never shared in public forums.</li>\r\n<li>Enterprise Capabilities - Advanced features designed for larger, more complex deployments.</li>\r\n<li>Security and Governance - Additional controls to help organizations meet security, compliance, and governance requirements.</li>\r\n<li>Scalability - Optimized for high-volume, mission-critical environments.</li><li>Seamless Operations - Reduce the effort required to deploy, manage, monitor, and maintain the product.</li><li>Indemnification - Commercial protections designed to help reduce organizational risk and provide greater assurance during enterprise procurement and deployment.</li>\r\n<li>Seamless Migration Path - Start with the open-source edition and adopt commercial capabilities as your needs grow.</li>\r\n</ul>\r\n\r\n<p>Open source at the core. Enterprise value on top.</p>\r\n\r\n<p>Open-source is the foundation... BlazorKit builds on that foundation with advanced capabilities and services designed specifically for organizations running in business-critical environments.</p>\r\n\r\n<p>Start with open source. Scale with confidence. Upgrade when your organization needs more.</p>\r\n\r\n<p>Learn more: <a href=\"https://blazorkit.net/products/blazorkit\" target=\"_new\">https://blazorkit.net</a></p>"
                    }
				}
			});

			return pageTemplates;
		}
	}
}
