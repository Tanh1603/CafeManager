using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CafeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "appuser",
                columns: table => new
                {
                    appuserid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    role = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    displayname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_appuserid", x => x.appuserid);
                });

            migrationBuilder.CreateTable(
                name: "coffeetable",
                columns: table => new
                {
                    coffeetableid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    statustable = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coffeetable", x => x.coffeetableid);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    customerid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    displayname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    buydate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    totalspent = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer", x => x.customerid);
                });

            migrationBuilder.CreateTable(
                name: "foodcategory",
                columns: table => new
                {
                    foodcategoryid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    displayname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_foodcategory", x => x.foodcategoryid);
                });

            migrationBuilder.CreateTable(
                name: "imports",
                columns: table => new
                {
                    importid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    importdate = table.Column<DateOnly>(type: "date", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_imports", x => x.importid);
                });

            migrationBuilder.CreateTable(
                name: "material",
                columns: table => new
                {
                    materialid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    displayname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    expirydate = table.Column<DateOnly>(type: "date", nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_material", x => x.materialid);
                });

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    staffid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    displayname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    sex = table.Column<char>(type: "character(1)", maxLength: 1, nullable: true),
                    birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    startworking = table.Column<DateOnly>(type: "date", nullable: true),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    workingstatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    basicsalary = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    workinghours = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    salary = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_staff", x => x.staffid);
                });

            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    supplierid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    displayname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    contractdate = table.Column<DateOnly>(type: "date", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supplier", x => x.supplierid);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    invoiceid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    paymentdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    paymentstatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    discountinvoice = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    coffeetableid = table.Column<int>(type: "integer", nullable: true),
                    customerid = table.Column<int>(type: "integer", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
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
                        name: "fk_invoices_customer",
                        column: x => x.customerid,
                        principalTable: "customer",
                        principalColumn: "customerid");
                });

            migrationBuilder.CreateTable(
                name: "food",
                columns: table => new
                {
                    foodid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    displayname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    foodcategoryid = table.Column<int>(type: "integer", nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    imagefood = table.Column<byte[]>(type: "bytea", nullable: true),
                    discountfood = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
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
                name: "consumedmaterials",
                columns: table => new
                {
                    consumedmaterialid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    materialid = table.Column<int>(type: "integer", nullable: true),
                    quantity = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true, defaultValueSql: "0"),
                    unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_consumedmaterials", x => x.consumedmaterialid);
                    table.ForeignKey(
                        name: "fk_consumedmaterials_material",
                        column: x => x.materialid,
                        principalTable: "material",
                        principalColumn: "materialid");
                });

            migrationBuilder.CreateTable(
                name: "importdetails",
                columns: table => new
                {
                    importdetailid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    importid = table.Column<int>(type: "integer", nullable: false),
                    materialid = table.Column<int>(type: "integer", nullable: false),
                    supplierid = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    unitprice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true, defaultValueSql: "0"),
                    totalprice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true, defaultValueSql: "0")
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
                        name: "fk_importdetails_material",
                        column: x => x.materialid,
                        principalTable: "material",
                        principalColumn: "materialid");
                    table.ForeignKey(
                        name: "fk_importdetails_supplier",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "supplierid");
                });

            migrationBuilder.CreateTable(
                name: "materialsupplier",
                columns: table => new
                {
                    materialsupplierid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    materialid = table.Column<int>(type: "integer", nullable: false),
                    supplierid = table.Column<int>(type: "integer", nullable: false),
                    supplydate = table.Column<DateOnly>(type: "date", nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("materialsupplier_pkey", x => x.materialsupplierid);
                    table.ForeignKey(
                        name: "materialsupplier_materialid_fkey",
                        column: x => x.materialid,
                        principalTable: "material",
                        principalColumn: "materialid");
                    table.ForeignKey(
                        name: "materialsupplier_supplierid_fkey",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "supplierid");
                });

            migrationBuilder.CreateTable(
                name: "invoicedetails",
                columns: table => new
                {
                    invoicedetailid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    invoiceid = table.Column<int>(type: "integer", nullable: true),
                    foodid = table.Column<int>(type: "integer", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_consumedmaterials_materialid",
                table: "consumedmaterials",
                column: "materialid");

            migrationBuilder.CreateIndex(
                name: "IX_food_foodcategoryid",
                table: "food",
                column: "foodcategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_importdetails_importid",
                table: "importdetails",
                column: "importid");

            migrationBuilder.CreateIndex(
                name: "IX_importdetails_materialid",
                table: "importdetails",
                column: "materialid");

            migrationBuilder.CreateIndex(
                name: "IX_importdetails_supplierid",
                table: "importdetails",
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
                name: "IX_invoices_customerid",
                table: "invoices",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "IX_materialsupplier_materialid",
                table: "materialsupplier",
                column: "materialid");

            migrationBuilder.CreateIndex(
                name: "IX_materialsupplier_supplierid",
                table: "materialsupplier",
                column: "supplierid");
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
                name: "materialsupplier");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "imports");

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
                name: "customer");
        }
    }
}
