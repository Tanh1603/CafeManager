using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Col_UsageDate_ConsumedMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "usagedate",
                table: "consumedmaterials",
                type: "date",
                nullable: false,
                defaultValueSql: "now()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usagedate",
                table: "consumedmaterials");
        }
    }
}
