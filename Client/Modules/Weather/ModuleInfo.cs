using Oqtane.Models;
using Oqtane.Modules;

namespace BlazorKit.Application.Client.Modules.Weather
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Weather",
            Description = "Weather",
            Version = "1.0.0",
			ServerManagerType = "BlazorKit.Application.Manager.BlazorKitManager, BlazorKit.Application.Server.Oqtane",
			Dependencies = "BlazorKit.Application.Shared.Oqtane",
            PackageName = "BlazorKit.Application" 
        };
    }
}
