using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using BlazorKit.Application.Services;
using BlazorKit.Application.Repository;

namespace BlazorKit.Application.Startup
{
    public class ServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // not implemented
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IWeatherService, ServerWeatherService>();

			services.AddDbContextFactory<BlazorKitContext>(opt => { }, ServiceLifetime.Transient);
		}
	}
}
