﻿// <auto-generated />
using System;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CafeManager.Infrastructure.Migrations
{
    [DbContext(typeof(CafeManagerContext))]
    partial class CafeManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Appuser", b =>
                {
                    b.Property<int>("Appuserid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("appuserid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Appuserid"));

                    b.Property<string>("Avatar")
                        .HasColumnType("text")
                        .HasColumnName("avatar");

                    b.Property<string>("Displayname")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("displayname");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.Property<int?>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("role");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("username");

                    b.HasKey("Appuserid")
                        .HasName("pk_appuserid");

                    b.ToTable("appuser", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Coffeetable", b =>
                {
                    b.Property<int>("Coffeetableid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("coffeetableid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Coffeetableid"));

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Statustable")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("statustable");

                    b.HasKey("Coffeetableid")
                        .HasName("pk_coffeetable");

                    b.ToTable("coffeetable", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Consumedmaterial", b =>
                {
                    b.Property<int>("Consumedmaterialid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("consumedmaterialid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Consumedmaterialid"));

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<int?>("Materialid")
                        .HasColumnType("integer")
                        .HasColumnName("materialid");

                    b.Property<decimal?>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("quantity")
                        .HasDefaultValueSql("0");

                    b.Property<string>("Unit")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("unit");

                    b.HasKey("Consumedmaterialid")
                        .HasName("pk_consumedmaterials");

                    b.HasIndex("Materialid");

                    b.ToTable("consumedmaterials", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Customer", b =>
                {
                    b.Property<int>("Customerid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("customerid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Customerid"));

                    b.Property<DateTime?>("Buydate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("buydate");

                    b.Property<string>("Displayname")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("displayname");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<decimal?>("Totalspent")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("totalspent");

                    b.Property<string>("Type")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("type");

                    b.HasKey("Customerid")
                        .HasName("pk_customer");

                    b.ToTable("customer", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Food", b =>
                {
                    b.Property<int>("Foodid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("foodid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Foodid"));

                    b.Property<decimal?>("Discountfood")
                        .HasPrecision(5, 2)
                        .HasColumnType("numeric(5,2)")
                        .HasColumnName("discountfood");

                    b.Property<string>("Displayname")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("displayname");

                    b.Property<int?>("Foodcategoryid")
                        .HasColumnType("integer")
                        .HasColumnName("foodcategoryid");

                    b.Property<string>("Imagefood")
                        .HasColumnType("text")
                        .HasColumnName("imagefood");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<decimal?>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("price");

                    b.HasKey("Foodid")
                        .HasName("pk_food");

                    b.HasIndex("Foodcategoryid");

                    b.ToTable("food", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Foodcategory", b =>
                {
                    b.Property<int>("Foodcategoryid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("foodcategoryid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Foodcategoryid"));

                    b.Property<string>("Displayname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("displayname");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.HasKey("Foodcategoryid")
                        .HasName("pk_foodcategory");

                    b.ToTable("foodcategory", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Import", b =>
                {
                    b.Property<int>("Importid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("importid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Importid"));

                    b.Property<DateOnly?>("Importdate")
                        .HasColumnType("date")
                        .HasColumnName("importdate");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.HasKey("Importid")
                        .HasName("pk_imports");

                    b.ToTable("imports", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Importdetail", b =>
                {
                    b.Property<int>("Importdetailid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("importdetailid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Importdetailid"));

                    b.Property<int>("Importid")
                        .HasColumnType("integer")
                        .HasColumnName("importid");

                    b.Property<int>("Materialid")
                        .HasColumnType("integer")
                        .HasColumnName("materialid");

                    b.Property<int?>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("quantity");

                    b.Property<int>("Supplierid")
                        .HasColumnType("integer")
                        .HasColumnName("supplierid");

                    b.Property<decimal?>("Totalprice")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("totalprice")
                        .HasDefaultValueSql("0");

                    b.Property<decimal?>("Unitprice")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("unitprice")
                        .HasDefaultValueSql("0");

                    b.HasKey("Importdetailid")
                        .HasName("pk_importdetails");

                    b.HasIndex("Importid");

                    b.HasIndex("Materialid");

                    b.HasIndex("Supplierid");

                    b.ToTable("importdetails", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Invoice", b =>
                {
                    b.Property<int>("Invoiceid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("invoiceid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Invoiceid"));

                    b.Property<int?>("Coffeetableid")
                        .HasColumnType("integer")
                        .HasColumnName("coffeetableid");

                    b.Property<int?>("Customerid")
                        .HasColumnType("integer")
                        .HasColumnName("customerid");

                    b.Property<decimal?>("Discountinvoice")
                        .HasPrecision(5, 2)
                        .HasColumnType("numeric(5,2)")
                        .HasColumnName("discountinvoice");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<DateTime?>("Paymentdate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("paymentdate");

                    b.Property<string>("Paymentstatus")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("paymentstatus");

                    b.HasKey("Invoiceid")
                        .HasName("pk_invoices");

                    b.HasIndex("Coffeetableid");

                    b.HasIndex("Customerid");

                    b.ToTable("invoices", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Invoicedetail", b =>
                {
                    b.Property<int>("Invoicedetailid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("invoicedetailid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Invoicedetailid"));

                    b.Property<int?>("Foodid")
                        .HasColumnType("integer")
                        .HasColumnName("foodid");

                    b.Property<int?>("Invoiceid")
                        .HasColumnType("integer")
                        .HasColumnName("invoiceid");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<int?>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("quantity");

                    b.HasKey("Invoicedetailid")
                        .HasName("pk_invoicedetails");

                    b.HasIndex("Foodid");

                    b.HasIndex("Invoiceid");

                    b.ToTable("invoicedetails", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Material", b =>
                {
                    b.Property<int>("Materialid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("materialid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Materialid"));

                    b.Property<string>("Displayname")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("displayname");

                    b.Property<DateOnly?>("Expirydate")
                        .HasColumnType("date")
                        .HasColumnName("expirydate");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<decimal?>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("price");

                    b.Property<string>("Unit")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("unit");

                    b.HasKey("Materialid")
                        .HasName("pk_material");

                    b.ToTable("material", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Materialsupplier", b =>
                {
                    b.Property<int>("Materialsupplierid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("materialsupplierid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Materialsupplierid"));

                    b.Property<int>("Materialid")
                        .HasColumnType("integer")
                        .HasColumnName("materialid");

                    b.Property<decimal?>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("price");

                    b.Property<int>("Supplierid")
                        .HasColumnType("integer")
                        .HasColumnName("supplierid");

                    b.Property<DateOnly?>("Supplydate")
                        .HasColumnType("date")
                        .HasColumnName("supplydate");

                    b.HasKey("Materialsupplierid")
                        .HasName("materialsupplier_pkey");

                    b.HasIndex("Materialid");

                    b.HasIndex("Supplierid");

                    b.ToTable("materialsupplier", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Staff", b =>
                {
                    b.Property<int>("Staffid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("staffid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Staffid"));

                    b.Property<decimal?>("Basicsalary")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("basicsalary");

                    b.Property<DateOnly?>("Birthday")
                        .HasColumnType("date")
                        .HasColumnName("birthday");

                    b.Property<string>("Displayname")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("displayname");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("phone");

                    b.Property<string>("Role")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("role");

                    b.Property<decimal?>("Salary")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("salary");

                    b.Property<char?>("Sex")
                        .HasMaxLength(1)
                        .HasColumnType("character(1)")
                        .HasColumnName("sex");

                    b.Property<DateOnly?>("Startworking")
                        .HasColumnType("date")
                        .HasColumnName("startworking");

                    b.Property<decimal?>("Workinghours")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("workinghours");

                    b.Property<string>("Workingstatus")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("workingstatus");

                    b.HasKey("Staffid")
                        .HasName("pk_staff");

                    b.ToTable("staff", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Supplier", b =>
                {
                    b.Property<int>("Supplierid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("supplierid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Supplierid"));

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("address");

                    b.Property<DateOnly?>("Contractdate")
                        .HasColumnType("date")
                        .HasColumnName("contractdate");

                    b.Property<string>("Displayname")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("displayname");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("phone");

                    b.HasKey("Supplierid")
                        .HasName("pk_supplier");

                    b.ToTable("supplier", (string)null);
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Consumedmaterial", b =>
                {
                    b.HasOne("CafeManager.Infrastructure.Models.Data.Material", "Material")
                        .WithMany("Consumedmaterials")
                        .HasForeignKey("Materialid")
                        .HasConstraintName("fk_consumedmaterials_material");

                    b.Navigation("Material");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Food", b =>
                {
                    b.HasOne("CafeManager.Infrastructure.Models.Data.Foodcategory", "Foodcategory")
                        .WithMany("Foods")
                        .HasForeignKey("Foodcategoryid")
                        .HasConstraintName("fk_food_foodcategory");

                    b.Navigation("Foodcategory");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Importdetail", b =>
                {
                    b.HasOne("CafeManager.Infrastructure.Models.Data.Import", "Import")
                        .WithMany("Importdetails")
                        .HasForeignKey("Importid")
                        .IsRequired()
                        .HasConstraintName("fk_importdetails_imports");

                    b.HasOne("CafeManager.Infrastructure.Models.Data.Material", "Material")
                        .WithMany("Importdetails")
                        .HasForeignKey("Materialid")
                        .IsRequired()
                        .HasConstraintName("fk_importdetails_material");

                    b.HasOne("CafeManager.Infrastructure.Models.Data.Supplier", "Supplier")
                        .WithMany("Importdetails")
                        .HasForeignKey("Supplierid")
                        .IsRequired()
                        .HasConstraintName("fk_importdetails_supplier");

                    b.Navigation("Import");

                    b.Navigation("Material");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Invoice", b =>
                {
                    b.HasOne("CafeManager.Infrastructure.Models.Data.Coffeetable", "Coffeetable")
                        .WithMany("Invoices")
                        .HasForeignKey("Coffeetableid")
                        .HasConstraintName("fk_invoices_coffeetable");

                    b.HasOne("CafeManager.Infrastructure.Models.Data.Customer", "Customer")
                        .WithMany("Invoices")
                        .HasForeignKey("Customerid")
                        .HasConstraintName("fk_invoices_customer");

                    b.Navigation("Coffeetable");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Invoicedetail", b =>
                {
                    b.HasOne("CafeManager.Infrastructure.Models.Data.Food", "Food")
                        .WithMany("Invoicedetails")
                        .HasForeignKey("Foodid")
                        .HasConstraintName("fk_invoicedetails_food");

                    b.HasOne("CafeManager.Infrastructure.Models.Data.Invoice", "Invoice")
                        .WithMany("Invoicedetails")
                        .HasForeignKey("Invoiceid")
                        .HasConstraintName("fk_invoicedetails_invoices");

                    b.Navigation("Food");

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Materialsupplier", b =>
                {
                    b.HasOne("CafeManager.Infrastructure.Models.Data.Material", "Material")
                        .WithMany("Materialsuppliers")
                        .HasForeignKey("Materialid")
                        .IsRequired()
                        .HasConstraintName("materialsupplier_materialid_fkey");

                    b.HasOne("CafeManager.Infrastructure.Models.Data.Supplier", "Supplier")
                        .WithMany("Materialsuppliers")
                        .HasForeignKey("Supplierid")
                        .IsRequired()
                        .HasConstraintName("materialsupplier_supplierid_fkey");

                    b.Navigation("Material");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Coffeetable", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Customer", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Food", b =>
                {
                    b.Navigation("Invoicedetails");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Foodcategory", b =>
                {
                    b.Navigation("Foods");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Import", b =>
                {
                    b.Navigation("Importdetails");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Invoice", b =>
                {
                    b.Navigation("Invoicedetails");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Material", b =>
                {
                    b.Navigation("Consumedmaterials");

                    b.Navigation("Importdetails");

                    b.Navigation("Materialsuppliers");
                });

            modelBuilder.Entity("CafeManager.Infrastructure.Models.Data.Supplier", b =>
                {
                    b.Navigation("Importdetails");

                    b.Navigation("Materialsuppliers");
                });
#pragma warning restore 612, 618
        }
    }
}
