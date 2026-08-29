using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace BlazorKit.Application.Migrations.EntityBuilders
{
    public class WeatherDataEntityBuilder : AuditableBaseEntityBuilder<WeatherDataEntityBuilder>
    {
        private const string _entityTableName = "WeatherData";
        private readonly PrimaryKey<WeatherDataEntityBuilder> _primaryKey = new("PK_WeatherData", x => x.WeatherDataId);
		private readonly ForeignKey<WeatherDataEntityBuilder> _foreignKey = new("FK_WeatherData_Site", x => x.SiteId, "Site", "SiteId", ReferentialAction.Cascade);

		public WeatherDataEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
			ForeignKeys.Add(_foreignKey);
        }

        protected override WeatherDataEntityBuilder BuildTable(ColumnsBuilder table)
        {
            WeatherDataId = AddAutoIncrementColumn(table, "WeatherDataId");
			SiteId = AddIntegerColumn(table, "SiteId");
			City = AddStringColumn(table, "City", 100);
            Date = AddDateTimeColumn(table, "Date");
            HighTemperature = AddDecimalColumn(table, "HighTemperature", 8, 2);
            LowTemperature = AddDecimalColumn(table, "LowTemperature", 8, 2);
            Humidity = AddDecimalColumn(table, "Humidity", 8, 2);
            WindSpeed = AddDecimalColumn(table, "WindSpeed", 8, 2);
            AirPressure = AddDecimalColumn(table, "AirPressure", 8, 2);
            Precipitation = AddDecimalColumn(table, "Precipitation", 8, 2);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> WeatherDataId { get; set; }
		public OperationBuilder<AddColumnOperation> SiteId { get; set; }
		public OperationBuilder<AddColumnOperation> Date { get; set; }
		public OperationBuilder<AddColumnOperation> City { get; set; }
        public OperationBuilder<AddColumnOperation> HighTemperature { get; set; }
        public OperationBuilder<AddColumnOperation> LowTemperature { get; set; }
        public OperationBuilder<AddColumnOperation> Humidity { get; set; }
        public OperationBuilder<AddColumnOperation> WindSpeed { get; set; }
        public OperationBuilder<AddColumnOperation> AirPressure { get; set; }
        public OperationBuilder<AddColumnOperation> Precipitation { get; set; }
    }
}
