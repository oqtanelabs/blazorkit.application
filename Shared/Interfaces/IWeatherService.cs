using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorKit.Application.Models;

namespace BlazorKit.Application.Services
{
    public interface IWeatherService 
    {
		Task<List<WeatherData>> GetAllWeatherDataAsync(int SiteId, DateTime From, DateTime To, string City);

        Task<List<string>> GetWeatherCitiesAsync(int SiteId);
        
		Task<WeatherData> GetWeatherDataAsync(int WeatherDataId);

		Task<WeatherData> AddWeatherDataAsync(WeatherData WeatherData);

		Task<WeatherData> UpdateWeatherDataAsync(WeatherData WeatherData);

		Task DeleteWeatherDataAsync(int WeatherDataId);
	}
}
