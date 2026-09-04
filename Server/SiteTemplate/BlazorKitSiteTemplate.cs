using BlazorKit.Application.Models;
using Microsoft.AspNetCore.Hosting;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using System;
using System.Collections.Generic;
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
			get { return "BlazorKit Template"; }
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
                        ModuleDefinitionName = typeof(Weather.Index).ToModuleDefinitionName(), Title = "Weather Dashboard", Pane = PaneNames.Default,
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
						Content = "<img src=\"blazorkit.png\" alt=\"BlazorKit\" style=\"float: right; margin: 15px; width: 300px;\">\r\n\r\n<p>Build, extend, and deploy modern web applications with a .NET-native platform designed for developers who want the power of Blazor without the complexity of assembling everything from scratch.</p>\r\n\r\n<p>Built on the mature&nbsp;<a href=\"https://www.oqtane.org\" target=\"_blank\">Oqtane</a>&nbsp;framework, this distribution provides a production-ready foundation for .NET teams looking to standardize on Blazor and build scalable, modular web applications.</p>\r\n\r\n<h3>Created for Modern .NET</h3>\r\n\r\n<p>Bring your existing .NET expertise to modern web development. Write application experiences using C# and Blazor, share code across your application stack, and work with familiar .NET tooling, libraries, patterns, and development workflows.</p>\r\n\r\n<p>Instead of stitching together a collection of frameworks, authentication models, UI components, deployment approaches, and infrastructure, BlazorKit provides an integrated foundation you can build upon.</p>\r\n\r\n<h3>Designed for Developers</h3>\r\n\r\n<p>BlazorKit is built with .NET developers in mind, providing a modular architecture that makes it easy to create, customize, and extend applications.</p>\r\n\r\n<p>Key capabilities include:</p><ul>\r\n<li>Blazor-first development - Build rich, interactive web experiences using Blazor and C#.</li>\r\n<li>Modular architecture - Extend the platform without modifying its core.</li>\r\n<li>Multi-tenant scalability - Manage multiple workloads from a single installation</li>\r\n<li>Modern .NET foundation - Leverage the performance, tooling, libraries, and ecosystem of .NET.</li>\r\n<li>Reusable components - Create functionality that can be shared across applications and projects.</li>\r\n<li>Developer-friendly extensibility - Customize the platform to match your application's architecture and business requirements.</li>\r\n<li>Enterprise-ready foundation - Give development teams a consistent platform for building and deploying .NET web solutions.</li>\r\n<li>Cloud and self-hosted flexibility - Deploy applications in the environments that make sense for your organization.</li>\r\n</ul>\r\n\r\n<h3>Optimized for the Enterprise</h3>\r\n\r\n<p>For organizations evaluating Blazor, adopting a platform can dramatically reduce the amount of infrastructure developers need to create and maintain themselves.</p>\r\n\r\n<p>Rather than asking every project team to solve the same problems repeatedly, organizations can establish a common foundation for:</p>\r\n\r\n<p>Architecture → Development → Deployment → Operations</p>\r\n\r\n<p>This enables teams to spend more time building differentiated business functionality and less time creating application plumbing.</p>\r\n\r\n<h3>Built on .NET. Built with Blazor.</h3>\r\n\r\n<p>Whether you're an experienced .NET developer exploring Blazor, a software company building a portfolio of applications, or an enterprise looking to establish a standardized Blazor development platform, BlazorKit gives you the solid foundation you need.</p>\r\n\r\n<p>Start with a proven modular .NET platform. Extend it with your own capabilities. Build the applications your business needs.</p>\r\n\r\n<p>Your .NET skills. Your code. Your platform. Your applications.</p>\r\n\r\n<p>Learn more at: <a href=\"https://blazorkit.net/products/blazorkit\" target=\"_new\">https://blazorkit.net</a></p>"
					}
				}
			});

			return pageTemplates;
		}
	}
}
