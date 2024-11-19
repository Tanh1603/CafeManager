using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn_StaffSalaryHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "staffid",
                table: "staffsalaryhistory",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "salary",
                table: "staffsalaryhistory",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AddColumn<bool>(
                name: "isdeleted",
                table: "staffsalaryhistory",
                type: "boolean",
                nullable: true,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isdeleted",
                table: "staffsalaryhistory");

            migrationBuilder.AlterColumn<int>(
                name: "staffid",
                table: "staffsalaryhistory",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "salary",
                table: "staffsalaryhistory",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2);
        }
    }
}
