using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "appuser",
                columns: table => new
                {
                    appuserid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    displayname = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true, defaultValueSql: "'Unkown'"),
                    password = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    role = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    avatar = table.Column<byte[]>(type: "BLOB", nullable: true),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_appuserid", x => x.appuserid);
                });

            migrationBuilder.CreateTable(
                name: "coffeetable",
                columns: table => new
                {
                    coffeetableid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tablenumber = table.Column<int>(type: "INTEGER", nullable: false),
                    seatingcapacity = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 4),
                    statustable = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, defaultValueSql: "'Đang sử dụng'"),
                    notes = table.Column<string>(type: "TEXT", nullable: true),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coffeetable", x => x.coffeetableid);
                });

            migrationBuilder.CreateTable(
                name: "foodcategory",
                columns: table => new
                {
                    foodcategoryid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    foodcategoryname = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_foodcategory", x => x.foodcategoryid);
                });

            migrationBuilder.CreateTable(
                name: "material",
                columns: table => new
                {
                    materialid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    materialname = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    unit = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_material", x => x.materialid);
                });

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    staffid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    staffname = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    sex = table.Column<bool>(type: "INTEGER", nullable: true),
                    birthday = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    startworkingdate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    endworkingdate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_staff", x => x.staffid);
                });

            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    supplierid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    suppliername = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    representativesupplier = table.Column<string>(type: "character varying", nullable: false),
                    phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    notes = table.Column<string>(type: "TEXT", nullable: true),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supplier", x => x.supplierid);
                });

            migrationBuilder.CreateTable(
                name: "food",
                columns: table => new
                {
                    foodid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    foodname = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    foodcategoryid = table.Column<int>(type: "INTEGER", nullable: false),
                    price = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true, defaultValueSql: "0"),
                    imagefood = table.Column<byte[]>(type: "BLOB", nullable: true),
                    discountfood = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: true, defaultValueSql: "0"),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_food", x => x.foodid);
                    table.ForeignKey(
                        name: "fk_food_foodcategory",
                        column: x => x.foodcategoryid,
                        principalTable: "foodcategory",
                        principalColumn: "foodcategoryid");
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    invoiceid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    paymentstartdate = table.Column<DateTime>(type: "Datetime", nullable: false),
                    paymentenddate = table.Column<DateTime>(type: "Datetime", nullable: true),
                    paymentstatus = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, defaultValueSql: "'Chưa thanh toán'"),
                    paymentmethod = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, defaultValueSql: "'Thanh toán tiền mặt'"),
                    discountinvoice = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: true, defaultValueSql: "0"),
                    coffeetableid = table.Column<int>(type: "INTEGER", nullable: true),
                    staffid = table.Column<int>(type: "INTEGER", nullable: false),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoices", x => x.invoiceid);
                    table.ForeignKey(
                        name: "fk_invoices_coffeetable",
                        column: x => x.coffeetableid,
                        principalTable: "coffeetable",
                        principalColumn: "coffeetableid");
                    table.ForeignKey(
                        name: "fk_invoices_staff",
                        column: x => x.staffid,
                        principalTable: "staff",
                        principalColumn: "staffid");
                });

            migrationBuilder.CreateTable(
                name: "staffsalaryhistory",
                columns: table => new
                {
                    staffsalaryhistoryid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    staffid = table.Column<int>(type: "INTEGER", nullable: false),
                    salary = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    effectivedate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_staffsalaryhistory", x => x.staffsalaryhistoryid);
                    table.ForeignKey(
                        name: "fk_staffsalaryhistory_staff",
                        column: x => x.staffid,
                        principalTable: "staff",
                        principalColumn: "staffid");
                });

            migrationBuilder.CreateTable(
                name: "imports",
                columns: table => new
                {
                    importid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    deliveryperson = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    shippingcompany = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    receiveddate = table.Column<DateTime>(type: "Datetime", nullable: false),
                    staffid = table.Column<int>(type: "INTEGER", nullable: false),
                    supplierid = table.Column<int>(type: "INTEGER", nullable: false),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_imports", x => x.importid);
                    table.ForeignKey(
                        name: "fk_imports_staff",
                        column: x => x.staffid,
                        principalTable: "staff",
                        principalColumn: "staffid");
                    table.ForeignKey(
                        name: "fk_imports_supplier",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "supplierid");
                });

            migrationBuilder.CreateTable(
                name: "materialsupplier",
                columns: table => new
                {
                    materialsupplierid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    materialid = table.Column<int>(type: "INTEGER", nullable: false),
                    supplierid = table.Column<int>(type: "INTEGER", nullable: false),
                    manufacturedate = table.Column<DateTime>(type: "Datetime", nullable: false),
                    expirationdate = table.Column<DateTime>(type: "Datetime", nullable: false),
                    original = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    manufacturer = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true, defaultValueSql: "0"),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_materialsupplier", x => x.materialsupplierid);
                    table.ForeignKey(
                        name: "fk_material_supplier",
                        column: x => x.materialid,
                        principalTable: "material",
                        principalColumn: "materialid");
                    table.ForeignKey(
                        name: "fk_supplier_material",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "supplierid");
                });

            migrationBuilder.CreateTable(
                name: "invoicedetails",
                columns: table => new
                {
                    invoicedetailid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    invoiceid = table.Column<int>(type: "INTEGER", nullable: true),
                    foodid = table.Column<int>(type: "INTEGER", nullable: false),
                    quantity = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoicedetails", x => x.invoicedetailid);
                    table.ForeignKey(
                        name: "fk_invoicedetails_food",
                        column: x => x.foodid,
                        principalTable: "food",
                        principalColumn: "foodid");
                    table.ForeignKey(
                        name: "fk_invoicedetails_invoices",
                        column: x => x.invoiceid,
                        principalTable: "invoices",
                        principalColumn: "invoiceid");
                });

            migrationBuilder.CreateTable(
                name: "consumedmaterials",
                columns: table => new
                {
                    consumedmaterialid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    materialsupplierid = table.Column<int>(type: "INTEGER", nullable: true),
                    quantity = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true, defaultValueSql: "0"),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false),
                    usagedate = table.Column<DateOnly>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_consumedmaterials", x => x.consumedmaterialid);
                    table.ForeignKey(
                        name: "pk_consumedmaterials_materialsupplierid",
                        column: x => x.materialsupplierid,
                        principalTable: "materialsupplier",
                        principalColumn: "materialsupplierid");
                });

            migrationBuilder.CreateTable(
                name: "importdetails",
                columns: table => new
                {
                    importdetailid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    importid = table.Column<int>(type: "INTEGER", nullable: false),
                    quantity = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true, defaultValueSql: "0"),
                    isdeleted = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false),
                    materialsupplierid = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_importdetails", x => x.importdetailid);
                    table.ForeignKey(
                        name: "fk_importdetails_imports",
                        column: x => x.importid,
                        principalTable: "imports",
                        principalColumn: "importid");
                    table.ForeignKey(
                        name: "fk_importdetails_materialsupplier",
                        column: x => x.materialsupplierid,
                        principalTable: "materialsupplier",
                        principalColumn: "materialsupplierid");
                });

            migrationBuilder.CreateIndex(
                name: "coffeetable_tablenumber_key",
                table: "coffeetable",
                column: "tablenumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consumedmaterials_materialsupplierid",
                table: "consumedmaterials",
                column: "materialsupplierid");

            migrationBuilder.CreateIndex(
                name: "IX_food_foodcategoryid",
                table: "food",
                column: "foodcategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_importdetails_importid",
                table: "importdetails",
                column: "importid");

            migrationBuilder.CreateIndex(
                name: "IX_importdetails_materialsupplierid",
                table: "importdetails",
                column: "materialsupplierid");

            migrationBuilder.CreateIndex(
                name: "IX_imports_staffid",
                table: "imports",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "IX_imports_supplierid",
                table: "imports",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_invoicedetails_foodid",
                table: "invoicedetails",
                column: "foodid");

            migrationBuilder.CreateIndex(
                name: "IX_invoicedetails_invoiceid",
                table: "invoicedetails",
                column: "invoiceid");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_coffeetableid",
                table: "invoices",
                column: "coffeetableid");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_staffid",
                table: "invoices",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "material_materialname_key",
                table: "material",
                column: "materialname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_materialsupplier_materialid",
                table: "materialsupplier",
                column: "materialid");

            migrationBuilder.CreateIndex(
                name: "IX_materialsupplier_supplierid",
                table: "materialsupplier",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_staffsalaryhistory_staffid",
                table: "staffsalaryhistory",
                column: "staffid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appuser");

            migrationBuilder.DropTable(
                name: "consumedmaterials");

            migrationBuilder.DropTable(
                name: "importdetails");

            migrationBuilder.DropTable(
                name: "invoicedetails");

            migrationBuilder.DropTable(
                name: "staffsalaryhistory");

            migrationBuilder.DropTable(
                name: "imports");

            migrationBuilder.DropTable(
                name: "materialsupplier");

            migrationBuilder.DropTable(
                name: "food");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "material");

            migrationBuilder.DropTable(
                name: "supplier");

            migrationBuilder.DropTable(
                name: "foodcategory");

            migrationBuilder.DropTable(
                name: "coffeetable");

            migrationBuilder.DropTable(
                name: "staff");
        }
    }
}
