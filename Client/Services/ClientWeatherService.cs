using System;
using Oqtane.Models;
using Oqtane.Services;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorKit.Application.Models;

namespace BlazorKit.Application.Services
{

    public class ClientWeatherService : ServiceBase, IWeatherService
    {
        public ClientWeatherService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("WeatherData");

        public async Task<List<WeatherData>> GetAllWeatherDataAsync(int SiteId, DateTime From, DateTime To, string City)
        {
            return await GetJsonAsync<List<WeatherData>>($"{Apiurl}?siteid={SiteId}&from={From:yyyy-MM-dd}&to={To:yyyy-MM-dd}&city={City}", Enumerable.Empty<WeatherData>().ToList());
        }

        public async Task<List<string>> GetWeatherCitiesAsync(int SiteId)
        {
            return await GetJsonAsync<List<string>>($"{Apiurl}/cities?siteid={SiteId}", Enumerable.Empty<string>().ToList());
        }
        
		public async Task<WeatherData> GetWeatherDataAsync(int WeatherDataId)
		{
			return await GetJsonAsync<WeatherData>($"{Apiurl}/{WeatherDataId}");
		}

		public async Task<WeatherData> AddWeatherDataAsync(WeatherData WeatherData)
		{
			return await PostJsonAsync<WeatherData>(Apiurl, WeatherData);
		}

		public async Task<WeatherData> UpdateWeatherDataAsync(WeatherData WeatherData)
		{
			return await PutJsonAsync<WeatherData>($"{Apiurl}/{WeatherData.WeatherDataId}", WeatherData);
		}

		public async Task DeleteWeatherDataAsync(int WeatherDataId)
		{
			await DeleteAsync($"{Apiurl}/{WeatherDataId}");
		}
	}
}
