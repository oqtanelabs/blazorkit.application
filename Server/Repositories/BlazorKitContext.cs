using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Repository.Databases.Interfaces;

namespace BlazorKit.Application.Repository
{
    public class BlazorKitContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.WeatherData> WeatherData { get; set; }

        public BlazorKitContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
