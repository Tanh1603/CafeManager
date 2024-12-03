using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Dbimport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "pk_consumedmaterials_material",
                table: "consumedmaterials");

            migrationBuilder.RenameColumn(
                name: "materialid",
                table: "consumedmaterials",
                newName: "materialsupplierid");

            migrationBuilder.RenameIndex(
                name: "IX_consumedmaterials_materialid",
                table: "consumedmaterials",
                newName: "IX_consumedmaterials_materialsupplierid");

            migrationBuilder.AddForeignKey(
                name: "pk_consumedmaterials_materialsupplierid",
                table: "consumedmaterials",
                column: "materialsupplierid",
                principalTable: "materialsupplier",
                principalColumn: "materialsupplierid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "pk_consumedmaterials_materialsupplierid",
                table: "consumedmaterials");

            migrationBuilder.RenameColumn(
                name: "materialsupplierid",
                table: "consumedmaterials",
                newName: "materialid");

            migrationBuilder.RenameIndex(
                name: "IX_consumedmaterials_materialsupplierid",
                table: "consumedmaterials",
                newName: "IX_consumedmaterials_materialid");

            migrationBuilder.AddForeignKey(
                name: "pk_consumedmaterials_material",
                table: "consumedmaterials",
                column: "materialid",
                principalTable: "material",
                principalColumn: "materialid");
        }
    }
}
