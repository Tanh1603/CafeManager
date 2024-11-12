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

            modelBuilder.Entity("CafeManager.Core.Data.Appuser", b =>
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
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("displayname")
                        .HasDefaultValueSql("'Unkown'::character varying");

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

            modelBuilder.Entity("CafeManager.Core.Data.Coffeetable", b =>
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

                    b.Property<string>("Notes")
                        .HasColumnType("text")
                        .HasColumnName("notes");

                    b.Property<int?>("Seatingcapacity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(4)
                        .HasColumnName("seatingcapacity");

                    b.Property<string>("Statustable")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("statustable")
                        .HasDefaultValueSql("'Đang sử dụng'::character varying");

                    b.Property<int>("Tablenumber")
                        .HasColumnType("integer")
                        .HasColumnName("tablenumber");

                    b.HasKey("Coffeetableid")
                        .HasName("pk_coffeetable");

                    b.HasIndex(new[] { "Tablenumber" }, "coffeetable_tablenumber_key")
                        .IsUnique();

                    b.ToTable("coffeetable", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Consumedmaterial", b =>
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

                    b.HasKey("Consumedmaterialid")
                        .HasName("pk_consumedmaterials");

                    b.HasIndex("Materialid");

                    b.ToTable("consumedmaterials", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Food", b =>
                {
                    b.Property<int>("Foodid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("foodid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Foodid"));

                    b.Property<decimal?>("Discountfood")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(5, 2)
                        .HasColumnType("numeric(5,2)")
                        .HasColumnName("discountfood")
                        .HasDefaultValueSql("0");

                    b.Property<int>("Foodcategoryid")
                        .HasColumnType("integer")
                        .HasColumnName("foodcategoryid");

                    b.Property<string>("Foodname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("foodname");

                    b.Property<string>("Imagefood")
                        .HasColumnType("text")
                        .HasColumnName("imagefood");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<decimal?>("Price")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("price")
                        .HasDefaultValueSql("0");

                    b.HasKey("Foodid")
                        .HasName("pk_food");

                    b.HasIndex("Foodcategoryid");

                    b.ToTable("food", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Foodcategory", b =>
                {
                    b.Property<int>("Foodcategoryid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("foodcategoryid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Foodcategoryid"));

                    b.Property<string>("Foodcategoryname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("foodcategoryname");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.HasKey("Foodcategoryid")
                        .HasName("pk_foodcategory");

                    b.ToTable("foodcategory", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Import", b =>
                {
                    b.Property<int>("Importid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("importid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Importid"));

                    b.Property<string>("Deliveryperson")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("deliveryperson");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("phone");

                    b.Property<DateTime>("Receiveddate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("receiveddate");

                    b.Property<string>("Shippingcompany")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("shippingcompany");

                    b.Property<int>("Staffid")
                        .HasColumnType("integer")
                        .HasColumnName("staffid");

                    b.Property<int>("Supplierid")
                        .HasColumnType("integer")
                        .HasColumnName("supplierid");

                    b.HasKey("Importid")
                        .HasName("pk_imports");

                    b.HasIndex("Staffid");

                    b.HasIndex("Supplierid");

                    b.ToTable("imports", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Importdetail", b =>
                {
                    b.Property<int>("Importdetailid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("importdetailid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Importdetailid"));

                    b.Property<int>("Importid")
                        .HasColumnType("integer")
                        .HasColumnName("importid");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<int>("Materialid")
                        .HasColumnType("integer")
                        .HasColumnName("materialid");

                    b.Property<decimal?>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("quantity")
                        .HasDefaultValueSql("0");

                    b.HasKey("Importdetailid")
                        .HasName("pk_importdetails");

                    b.HasIndex("Importid");

                    b.HasIndex("Materialid");

                    b.ToTable("importdetails", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Invoice", b =>
                {
                    b.Property<int>("Invoiceid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("invoiceid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Invoiceid"));

                    b.Property<int?>("Coffeetableid")
                        .HasColumnType("integer")
                        .HasColumnName("coffeetableid");

                    b.Property<decimal?>("Discountinvoice")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(5, 2)
                        .HasColumnType("numeric(5,2)")
                        .HasColumnName("discountinvoice")
                        .HasDefaultValueSql("0");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<DateTime?>("Paymentenddate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("paymentenddate");

                    b.Property<string>("Paymentmethod")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("paymentmethod")
                        .HasDefaultValueSql("'Thanh toán tiền mặt'::character varying");

                    b.Property<DateTime>("Paymentstartdate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("paymentstartdate");

                    b.Property<string>("Paymentstatus")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("paymentstatus")
                        .HasDefaultValueSql("'Chưa thanh toán'::character varying");

                    b.Property<int>("Staffid")
                        .HasColumnType("integer")
                        .HasColumnName("staffid");

                    b.HasKey("Invoiceid")
                        .HasName("pk_invoices");

                    b.HasIndex("Coffeetableid");

                    b.HasIndex("Staffid");

                    b.ToTable("invoices", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Invoicedetail", b =>
                {
                    b.Property<int>("Invoicedetailid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("invoicedetailid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Invoicedetailid"));

                    b.Property<int>("Foodid")
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

            modelBuilder.Entity("CafeManager.Core.Data.Material", b =>
                {
                    b.Property<int>("Materialid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("materialid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Materialid"));

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Materialname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("materialname");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("unit");

                    b.HasKey("Materialid")
                        .HasName("pk_material");

                    b.HasIndex(new[] { "Materialname" }, "material_materialname_key")
                        .IsUnique();

                    b.ToTable("material", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Materialsupplier", b =>
                {
                    b.Property<int>("Materialsupplierid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("materialsupplierid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Materialsupplierid"));

                    b.Property<DateTime>("Expirationdate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("expirationdate");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<DateTime>("Manufacturedate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("manufacturedate");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("manufacturer");

                    b.Property<int>("Materialid")
                        .HasColumnType("integer")
                        .HasColumnName("materialid");

                    b.Property<string>("Original")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("original");

                    b.Property<decimal?>("Price")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("price")
                        .HasDefaultValueSql("0");

                    b.Property<int>("Supplierid")
                        .HasColumnType("integer")
                        .HasColumnName("supplierid");

                    b.HasKey("Materialsupplierid")
                        .HasName("pk_materialsupplier");

                    b.HasIndex("Materialid");

                    b.HasIndex("Supplierid");

                    b.ToTable("materialsupplier", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Staff", b =>
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
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("phone");

                    b.Property<string>("Role")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("role");

                    b.Property<char?>("Sex")
                        .HasMaxLength(1)
                        .HasColumnType("character(1)")
                        .HasColumnName("sex");

                    b.Property<string>("Staffname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("staffname");

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

            modelBuilder.Entity("CafeManager.Core.Data.Supplier", b =>
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

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Notes")
                        .HasColumnType("text")
                        .HasColumnName("notes");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("phone");

                    b.Property<string>("Representativesupplier")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("representativesupplier");

                    b.Property<string>("Suppliername")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("suppliername");

                    b.HasKey("Supplierid")
                        .HasName("pk_supplier");

                    b.ToTable("supplier", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Consumedmaterial", b =>
                {
                    b.HasOne("CafeManager.Core.Data.Material", "Material")
                        .WithMany("Consumedmaterials")
                        .HasForeignKey("Materialid")
                        .HasConstraintName("pk_consumedmaterials_material");

                    b.Navigation("Material");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Food", b =>
                {
                    b.HasOne("CafeManager.Core.Data.Foodcategory", "Foodcategory")
                        .WithMany("Foods")
                        .HasForeignKey("Foodcategoryid")
                        .IsRequired()
                        .HasConstraintName("fk_food_foodcategory");

                    b.Navigation("Foodcategory");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Import", b =>
                {
                    b.HasOne("CafeManager.Core.Data.Staff", "Staff")
                        .WithMany("Imports")
                        .HasForeignKey("Staffid")
                        .IsRequired()
                        .HasConstraintName("fk_imports_staff");

                    b.HasOne("CafeManager.Core.Data.Supplier", "Supplier")
                        .WithMany("Imports")
                        .HasForeignKey("Supplierid")
                        .IsRequired()
                        .HasConstraintName("fk_imports_supplier");

                    b.Navigation("Staff");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Importdetail", b =>
                {
                    b.HasOne("CafeManager.Core.Data.Import", "Import")
                        .WithMany("Importdetails")
                        .HasForeignKey("Importid")
                        .IsRequired()
                        .HasConstraintName("fk_importdetails_imports");

                    b.HasOne("CafeManager.Core.Data.Material", "Material")
                        .WithMany("Importdetails")
                        .HasForeignKey("Materialid")
                        .IsRequired()
                        .HasConstraintName("fk_importdetails_material");

                    b.Navigation("Import");

                    b.Navigation("Material");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Invoice", b =>
                {
                    b.HasOne("CafeManager.Core.Data.Coffeetable", "Coffeetable")
                        .WithMany("Invoices")
                        .HasForeignKey("Coffeetableid")
                        .HasConstraintName("fk_invoices_coffeetable");

                    b.HasOne("CafeManager.Core.Data.Staff", "Staff")
                        .WithMany("Invoices")
                        .HasForeignKey("Staffid")
                        .IsRequired()
                        .HasConstraintName("fk_invoices_staff");

                    b.Navigation("Coffeetable");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Invoicedetail", b =>
                {
                    b.HasOne("CafeManager.Core.Data.Food", "Food")
                        .WithMany("Invoicedetails")
                        .HasForeignKey("Foodid")
                        .IsRequired()
                        .HasConstraintName("fk_invoicedetails_food");

                    b.HasOne("CafeManager.Core.Data.Invoice", "Invoice")
                        .WithMany("Invoicedetails")
                        .HasForeignKey("Invoiceid")
                        .HasConstraintName("fk_invoicedetails_invoices");

                    b.Navigation("Food");

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Materialsupplier", b =>
                {
                    b.HasOne("CafeManager.Core.Data.Material", "Material")
                        .WithMany("Materialsuppliers")
                        .HasForeignKey("Materialid")
                        .IsRequired()
                        .HasConstraintName("fk_material_supplier");

                    b.HasOne("CafeManager.Core.Data.Supplier", "Supplier")
                        .WithMany("Materialsuppliers")
                        .HasForeignKey("Supplierid")
                        .IsRequired()
                        .HasConstraintName("fk_supplier_material");

                    b.Navigation("Material");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Coffeetable", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Food", b =>
                {
                    b.Navigation("Invoicedetails");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Foodcategory", b =>
                {
                    b.Navigation("Foods");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Import", b =>
                {
                    b.Navigation("Importdetails");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Invoice", b =>
                {
                    b.Navigation("Invoicedetails");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Material", b =>
                {
                    b.Navigation("Consumedmaterials");

                    b.Navigation("Importdetails");

                    b.Navigation("Materialsuppliers");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Staff", b =>
                {
                    b.Navigation("Imports");

                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Supplier", b =>
                {
                    b.Navigation("Imports");

                    b.Navigation("Materialsuppliers");
                });
#pragma warning restore 612, 618
        }
    }
}
