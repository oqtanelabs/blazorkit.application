using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using BlazorKit.Application.Services;

namespace BlazorKit.Application.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IWeatherService)))
            {
                services.AddScoped<IWeatherService, ClientWeatherService>();
            }
        }
    }
}
