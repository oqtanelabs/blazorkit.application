using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorKit.Application.Repository
{
    public interface IWeatherDataRepository
    {
        IEnumerable<Models.WeatherData> GetAllWeatherData(int SiteId, DateTime From, DateTime To, string City);
        IEnumerable<string> GetWeatherCities(int SiteId);
        Models.WeatherData GetWeatherData(int WeatherDataId);
        Models.WeatherData AddWeatherData(Models.WeatherData WeatherData);
        Models.WeatherData UpdateWeatherData(Models.WeatherData WeatherData);
        void DeleteWeatherData(int WeatherDataId);
    }

    public class WeatherDataRepository : IWeatherDataRepository, ITransientService
    {
        private readonly IDbContextFactory<BlazorKitContext> _factory;

        public WeatherDataRepository(IDbContextFactory<BlazorKitContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.WeatherData> GetAllWeatherData(int SiteId, DateTime From, DateTime To, string City)
        {
            using var db = _factory.CreateDbContext();
            return db.WeatherData
                .Where(item => item.SiteId == SiteId && item.Date >= From && item.Date <= To && (item.City == City || City == ""))
                .OrderBy(item => item.Date)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<string> GetWeatherCities(int SiteId)
        {
            using var db = _factory.CreateDbContext();
            return db.WeatherData
                .Where(item => item.SiteId == SiteId)
                .Select(item => item.City)
                .Distinct()
                .OrderBy(item => item)
                .AsNoTracking()
                .ToList();
        }

        public Models.WeatherData GetWeatherData(int WeatherDataId)
        {
            using var db = _factory.CreateDbContext();
            return db.WeatherData
                .AsNoTracking()
                .FirstOrDefault(item => item.WeatherDataId == WeatherDataId);
        }

        public Models.WeatherData AddWeatherData(Models.WeatherData WeatherData)
        {
            using var db = _factory.CreateDbContext();
            db.WeatherData.Add(WeatherData);
            db.SaveChanges();

            return WeatherData;
        }

        public Models.WeatherData UpdateWeatherData(Models.WeatherData WeatherData)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(WeatherData).State = EntityState.Modified;
            db.SaveChanges();
            return WeatherData;
        }

        public void DeleteWeatherData(int WeatherDataId)
        {
            using var db = _factory.CreateDbContext();
            Models.WeatherData WeatherData = db.WeatherData.Find(WeatherDataId);
            db.WeatherData.Remove(WeatherData);
            db.SaveChanges();
        }
    }
}
