using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorKit.Application.Models
{
    public class WeatherData : ModelBase
    {
		public int WeatherDataId { get; set; }
		public int SiteId { get; set; }
		public DateTime Date { get; set; }
		public string City { get; set; }
		public decimal HighTemperature { get; set; }
		public decimal LowTemperature { get; set; }
		public decimal Humidity { get; set; }
		public decimal WindSpeed { get; set; }
		public decimal AirPressure { get; set; }
		public decimal Precipitation { get; set; }
	}
}
