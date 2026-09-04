using BlazorKit.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Controllers;
using Oqtane.Enums;
using Oqtane.Extensions;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Net;
using BlazorKit.Application.Models;
using System;

namespace BlazorKit.Application.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class WeatherDataController : ModuleControllerBase
    {
        private readonly IWeatherService _WeatherService;

        public WeatherDataController(IWeatherService WeatherService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _WeatherService = WeatherService;
        }

        // GET: api/<controller>?siteid=x&from=yyyy-mm-dd&to=yyyy-mm-dd&city=cityname
        [HttpGet]
		[Authorize(Roles = RoleNames.Registered)]
		public IEnumerable<WeatherData> Get(string siteid, DateTime from, DateTime to, string city)
		{
			int SiteId;
			if (int.TryParse(siteid, out SiteId) && SiteId == HttpContext.GetAlias().SiteId)
			{
				return _WeatherService.GetAllWeatherDataAsync(SiteId, from, to, city).Result;
			}
			else
			{
				_logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Weather Data Get Attempt {SiteId}", siteid);
				HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
				return null;
			}
		}

        [HttpGet("cities")]
        public IEnumerable<string> GetWeatherCities(string siteid)
        {
            int SiteId;
            if (int.TryParse(siteid, out SiteId) && SiteId == HttpContext.GetAlias().SiteId)
            {
                return _WeatherService.GetWeatherCitiesAsync(SiteId).Result;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Weather Cities Get Attempt {SiteId}", siteid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
		public WeatherData Get(int id)
		{
			var weatherData = _WeatherService.GetWeatherDataAsync(id).Result;
			if (weatherData != null && weatherData.SiteId == HttpContext.GetAlias().SiteId)
			{
				return weatherData;
			}
			else
			{
				if (weatherData != null)
				{
					_logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Weather Data Get Attempt {WeatherDataId}", id);
					HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
				}
				else
				{
					HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
				}
				return null;
			}
		}

		// POST api/<controller>
		[HttpPost]
		[Authorize(Roles = RoleNames.Admin)]
		public WeatherData Post([FromBody] WeatherData weatherData)
		{
			if (ModelState.IsValid && weatherData.SiteId == HttpContext.GetAlias().SiteId)
			{
				weatherData = _WeatherService.AddWeatherDataAsync(weatherData).Result;
				_logger.Log(LogLevel.Information, this, LogFunction.Create, "Weather Data Added {WeatherData}", weatherData);
			}
			else
			{
				_logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Weather Data Post Attempt {WeatherData}", weatherData);
				HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
				weatherData = null;
			}
			return weatherData;
		}

		// PUT api/<controller>/5
		[HttpPut("{id}")]
		[Authorize(Roles = RoleNames.Admin)]
		public WeatherData Put(int id, [FromBody] WeatherData weatherData)
		{
			if (ModelState.IsValid && weatherData.SiteId == HttpContext.GetAlias().SiteId && weatherData.WeatherDataId == id && _WeatherService.GetWeatherDataAsync(weatherData.WeatherDataId) != null)
			{
				weatherData = _WeatherService.UpdateWeatherDataAsync(weatherData).Result;
				_logger.Log(LogLevel.Information, this, LogFunction.Update, "Weather Data Updated {WeatherData}", weatherData);
			}
			else
			{
				_logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Weather Data Put Attempt {WeatherData}", weatherData);
				HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
				weatherData = null;
			}
			return weatherData;
		}

		// DELETE api/<controller>/5
		[HttpDelete("{id}")]
		[Authorize(Roles = RoleNames.Admin)]
		public void Delete(int id)
		{
			var weatherData = _WeatherService.GetWeatherDataAsync(id).Result;
			if (weatherData != null && weatherData.SiteId == HttpContext.GetAlias().SiteId)
			{
				_WeatherService.DeleteWeatherDataAsync(id).Wait();
				_logger.Log(LogLevel.Information, this, LogFunction.Delete, "Weather Data Deleted {WeatherDataId}", id);
			}
			else
			{
				_logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Weather Data Delete Attempt {WeatherDataId}", id);
				HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
			}
		}
	}
}
