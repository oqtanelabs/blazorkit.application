using System.Collections.Generic;
using Oqtane.Models;
using Oqtane.Themes;
using Oqtane.Shared;

namespace BlazorKit.Application.BlazorKitTheme
{
    public class ThemeInfo : ITheme
    {
        public Theme Theme => new Theme
        {
            Name = "BlazorKit Theme",
            Version = "1.0.0",
            PackageName = "BlazorKit.Application.BlazorKitTheme",
            ThemeSettingsType = "BlazorKit.Application.BlazorKitTheme.ThemeSettings, BlazorKit.Application.Client.Oqtane",
            ContainerSettingsType = "BlazorKit.Application.BlazorKitTheme.ContainerSettings, BlazorKit.Application.Client.Oqtane",
            Resources = new List<Resource>()
            {
                // Stylesheets
                new Stylesheet("~/assets/css/bootstrap.min.css"),
                new Stylesheet("~/assets/css/dashboard.css"),
                // JavaScript
                new Script("~/assets/js/bootstrap.bundle.min.js"),
                new Script("~/assets/js/dashboard.js", "", "", ResourceLocation.Body, ResourceLoadBehavior.Always, null, "", "", RenderModes.Static)
            }
        };
    }
}
