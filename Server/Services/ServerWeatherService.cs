using BlazorKit.Application.Models;
using BlazorKit.Application.Repository;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Extensions;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Security;
using Oqtane.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorKit.Application.Services
{
    public class ServerWeatherService : IWeatherService
    {
		private readonly IWeatherDataRepository _weatherDataRepository;
		private readonly ILogManager _logger;

		public ServerWeatherService(IWeatherDataRepository weatherDataRepository, ILogManager logger)
		{
			_weatherDataRepository = weatherDataRepository;
			_logger = logger;
		}

		public Task<List<WeatherData>> GetAllWeatherDataAsync(int SiteId, DateTime From, DateTime To, string City)
		{
			return Task.FromResult(_weatherDataRepository.GetAllWeatherData(SiteId, From, To, City).ToList());
		}

        public Task<List<string>> GetWeatherCitiesAsync(int SiteId)
        {
            return Task.FromResult(_weatherDataRepository.GetWeatherCities(SiteId).ToList());
        }
        
		public Task<WeatherData> GetWeatherDataAsync(int WeatherDataId)
		{
			return Task.FromResult(_weatherDataRepository.GetWeatherData(WeatherDataId));
		}

		public Task<WeatherData> AddWeatherDataAsync(WeatherData WeatherData)
		{
			WeatherData = _weatherDataRepository.AddWeatherData(WeatherData);
			_logger.Log(LogLevel.Information, this, LogFunction.Create, "Weather Data Added {WeatherData}", WeatherData);
			return Task.FromResult(WeatherData);
		}

		public Task<WeatherData> UpdateWeatherDataAsync(WeatherData WeatherData)
		{
			WeatherData = _weatherDataRepository.UpdateWeatherData(WeatherData);
			_logger.Log(LogLevel.Information, this, LogFunction.Update, "Weather Data Updated {WeatherData}", WeatherData);
			return Task.FromResult(WeatherData);
		}

		public Task DeleteWeatherDataAsync(int WeatherDataId)
		{
			_weatherDataRepository.DeleteWeatherData(WeatherDataId);
			_logger.Log(LogLevel.Information, this, LogFunction.Delete, "Weather Data Deleted {WeatherDataId}", WeatherDataId);
			return Task.CompletedTask;
		}
	}
}
