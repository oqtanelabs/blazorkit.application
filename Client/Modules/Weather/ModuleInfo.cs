using Oqtane.Models;
using Oqtane.Modules;

namespace BlazorKit.Application.Weather
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Weather",
            Description = "Weather",
            Version = "1.0.0",
			ServerManagerType = "BlazorKit.Application.Manager.ServerManager, BlazorKit.Application.Server.Oqtane",
			Dependencies = "BlazorKit.Application.Shared.Oqtane",
            PackageName = "BlazorKit.Application" 
        };
    }
}
