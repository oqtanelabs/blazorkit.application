using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using BlazorKit.Application.Repository;
using BlazorKit.Application.Migrations.EntityBuilders;

namespace BlazorKit.Application.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    // make sure to update the version number in the ModuleInfo.cs file when adding a new migration or else it will not be executed
    [Migration("BlazorKit.Application.01.00.00.00")]
    public class Initialize : MultiDatabaseMigration
    {
        public Initialize(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var weatherDataEntityBuilder = new WeatherDataEntityBuilder(migrationBuilder, ActiveDatabase);
            weatherDataEntityBuilder.Create();
            weatherDataEntityBuilder.AddIndex("IX_WeatherData", new[] { "City", "Date" }, true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // not implemented
        }
    }
}
