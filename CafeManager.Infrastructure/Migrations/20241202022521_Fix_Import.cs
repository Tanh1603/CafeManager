using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Import : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_importdetails_material",
                table: "importdetails");

            migrationBuilder.RenameColumn(
                name: "materialid",
                table: "importdetails",
                newName: "materialsupplierid");

            migrationBuilder.RenameIndex(
                name: "IX_importdetails_materialid",
                table: "importdetails",
                newName: "IX_importdetails_materialsupplierid");

            migrationBuilder.AddForeignKey(
                name: "fk_importdetails_materialsupplier",
                table: "importdetails",
                column: "materialsupplierid",
                principalTable: "materialsupplier",
                principalColumn: "materialsupplierid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_importdetails_materialsupplier",
                table: "importdetails");

            migrationBuilder.RenameColumn(
                name: "materialsupplierid",
                table: "importdetails",
                newName: "materialid");

            migrationBuilder.RenameIndex(
                name: "IX_importdetails_materialsupplierid",
                table: "importdetails",
                newName: "IX_importdetails_materialid");

            migrationBuilder.AddForeignKey(
                name: "fk_importdetails_material",
                table: "importdetails",
                column: "materialid",
                principalTable: "material",
                principalColumn: "materialid");
        }
    }
}
