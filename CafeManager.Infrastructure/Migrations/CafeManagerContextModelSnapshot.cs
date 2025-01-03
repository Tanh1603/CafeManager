﻿// <auto-generated />
using System;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("CafeManager.Core.Data.Appuser", b =>
                {
                    b.Property<int>("Appuserid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("appuserid");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("BLOB")
                        .HasColumnName("avatar");

                    b.Property<string>("Displayname")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("displayname")
                        .HasDefaultValueSql("'Unkown'");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT")
                        .HasColumnName("password");

                    b.Property<int?>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0)
                        .HasColumnName("role");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("username");

                    b.HasKey("Appuserid")
                        .HasName("pk_appuserid");

                    b.ToTable("appuser", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Coffeetable", b =>
                {
                    b.Property<int>("Coffeetableid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("coffeetableid");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT")
                        .HasColumnName("notes");

                    b.Property<int?>("Seatingcapacity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(4)
                        .HasColumnName("seatingcapacity");

                    b.Property<string>("Statustable")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("statustable")
                        .HasDefaultValueSql("'Đang sử dụng'");

                    b.Property<int>("Tablenumber")
                        .HasColumnType("INTEGER")
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
                        .HasColumnType("INTEGER")
                        .HasColumnName("consumedmaterialid");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<int?>("Materialsupplierid")
                        .HasColumnType("INTEGER")
                        .HasColumnName("materialsupplierid");

                    b.Property<decimal?>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(10, 2)
                        .HasColumnType("TEXT")
                        .HasColumnName("quantity")
                        .HasDefaultValueSql("0");

                    b.Property<DateOnly>("Usagedate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("usagedate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Consumedmaterialid")
                        .HasName("pk_consumedmaterials");

                    b.HasIndex("Materialsupplierid");

                    b.ToTable("consumedmaterials", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Food", b =>
                {
                    b.Property<int>("Foodid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("foodid");

                    b.Property<decimal?>("Discountfood")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(5, 2)
                        .HasColumnType("TEXT")
                        .HasColumnName("discountfood")
                        .HasDefaultValueSql("0");

                    b.Property<int>("Foodcategoryid")
                        .HasColumnType("INTEGER")
                        .HasColumnName("foodcategoryid");

                    b.Property<string>("Foodname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("foodname");

                    b.Property<byte[]>("Imagefood")
                        .HasColumnType("BLOB")
                        .HasColumnName("imagefood");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<decimal?>("Price")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(10, 2)
                        .HasColumnType("TEXT")
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
                        .HasColumnType("INTEGER")
                        .HasColumnName("foodcategoryid");

                    b.Property<string>("Foodcategoryname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("foodcategoryname");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
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
                        .HasColumnType("INTEGER")
                        .HasColumnName("importid");

                    b.Property<string>("Deliveryperson")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("deliveryperson");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .HasColumnName("phone");

                    b.Property<DateTime>("Receiveddate")
                        .HasColumnType("Datetime")
                        .HasColumnName("receiveddate");

                    b.Property<string>("Shippingcompany")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("shippingcompany");

                    b.Property<int>("Staffid")
                        .HasColumnType("INTEGER")
                        .HasColumnName("staffid");

                    b.Property<int>("Supplierid")
                        .HasColumnType("INTEGER")
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
                        .HasColumnType("INTEGER")
                        .HasColumnName("importdetailid");

                    b.Property<int>("Importid")
                        .HasColumnType("INTEGER")
                        .HasColumnName("importid");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<int>("Materialsupplierid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("materialsupplierid");

                    b.Property<decimal?>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(10, 2)
                        .HasColumnType("TEXT")
                        .HasColumnName("quantity")
                        .HasDefaultValueSql("0");

                    b.HasKey("Importdetailid")
                        .HasName("pk_importdetails");

                    b.HasIndex("Importid");

                    b.HasIndex("Materialsupplierid");

                    b.ToTable("importdetails", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Invoice", b =>
                {
                    b.Property<int>("Invoiceid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("invoiceid");

                    b.Property<int?>("Coffeetableid")
                        .HasColumnType("INTEGER")
                        .HasColumnName("coffeetableid");

                    b.Property<decimal?>("Discountinvoice")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(5, 2)
                        .HasColumnType("TEXT")
                        .HasColumnName("discountinvoice")
                        .HasDefaultValueSql("0");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<DateTime?>("Paymentenddate")
                        .HasColumnType("Datetime")
                        .HasColumnName("paymentenddate");

                    b.Property<string>("Paymentmethod")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("paymentmethod")
                        .HasDefaultValueSql("'Thanh toán tiền mặt'");

                    b.Property<DateTime>("Paymentstartdate")
                        .HasColumnType("Datetime")
                        .HasColumnName("paymentstartdate");

                    b.Property<string>("Paymentstatus")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("paymentstatus")
                        .HasDefaultValueSql("'Chưa thanh toán'");

                    b.Property<int>("Staffid")
                        .HasColumnType("INTEGER")
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
                        .HasColumnType("INTEGER")
                        .HasColumnName("invoicedetailid");

                    b.Property<int>("Foodid")
                        .HasColumnType("INTEGER")
                        .HasColumnName("foodid");

                    b.Property<int?>("Invoiceid")
                        .HasColumnType("INTEGER")
                        .HasColumnName("invoiceid");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<int?>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
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
                        .HasColumnType("INTEGER")
                        .HasColumnName("materialid");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Materialname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("materialname");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
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
                        .HasColumnType("INTEGER")
                        .HasColumnName("materialsupplierid");

                    b.Property<DateTime>("Expirationdate")
                        .HasColumnType("Datetime")
                        .HasColumnName("expirationdate");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<DateTime>("Manufacturedate")
                        .HasColumnType("Datetime")
                        .HasColumnName("manufacturedate");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("manufacturer");

                    b.Property<int>("Materialid")
                        .HasColumnType("INTEGER")
                        .HasColumnName("materialid");

                    b.Property<string>("Original")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .HasColumnName("original");

                    b.Property<decimal?>("Price")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(10, 2)
                        .HasColumnType("TEXT")
                        .HasColumnName("price")
                        .HasDefaultValueSql("0");

                    b.Property<int>("Supplierid")
                        .HasColumnType("INTEGER")
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
                        .HasColumnType("INTEGER")
                        .HasColumnName("staffid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("address");

                    b.Property<DateOnly>("Birthday")
                        .HasColumnType("TEXT")
                        .HasColumnName("birthday");

                    b.Property<DateOnly?>("Endworkingdate")
                        .HasColumnType("TEXT")
                        .HasColumnName("endworkingdate");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .HasColumnName("phone");

                    b.Property<string>("Role")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("role");

                    b.Property<bool?>("Sex")
                        .HasColumnType("INTEGER")
                        .HasColumnName("sex");

                    b.Property<string>("Staffname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("staffname");

                    b.Property<DateOnly>("Startworkingdate")
                        .HasColumnType("TEXT")
                        .HasColumnName("startworkingdate");

                    b.HasKey("Staffid")
                        .HasName("pk_staff");

                    b.ToTable("staff", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Staffsalaryhistory", b =>
                {
                    b.Property<int>("Staffsalaryhistoryid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("staffsalaryhistoryid");

                    b.Property<DateOnly>("Effectivedate")
                        .HasColumnType("TEXT")
                        .HasColumnName("effectivedate");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<decimal>("Salary")
                        .HasPrecision(10, 2)
                        .HasColumnType("TEXT")
                        .HasColumnName("salary");

                    b.Property<int>("Staffid")
                        .HasColumnType("INTEGER")
                        .HasColumnName("staffid");

                    b.HasKey("Staffsalaryhistoryid")
                        .HasName("pk_staffsalaryhistory");

                    b.HasIndex("Staffid");

                    b.ToTable("staffsalaryhistory", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Supplier", b =>
                {
                    b.Property<int>("Supplierid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("supplierid");

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT")
                        .HasColumnName("address");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.Property<bool?>("Isdeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("isdeleted");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT")
                        .HasColumnName("notes");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .HasColumnName("phone");

                    b.Property<string>("Representativesupplier")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("representativesupplier");

                    b.Property<string>("Suppliername")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("suppliername");

                    b.HasKey("Supplierid")
                        .HasName("pk_supplier");

                    b.ToTable("supplier", (string)null);
                });

            modelBuilder.Entity("CafeManager.Core.Data.Consumedmaterial", b =>
                {
                    b.HasOne("CafeManager.Core.Data.Materialsupplier", "Materialsupplier")
                        .WithMany("Consumedmaterials")
                        .HasForeignKey("Materialsupplierid")
                        .HasConstraintName("pk_consumedmaterials_materialsupplierid");

                    b.Navigation("Materialsupplier");
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

                    b.HasOne("CafeManager.Core.Data.Materialsupplier", "Materialsupplier")
                        .WithMany("Importdetails")
                        .HasForeignKey("Materialsupplierid")
                        .IsRequired()
                        .HasConstraintName("fk_importdetails_materialsupplier");

                    b.Navigation("Import");

                    b.Navigation("Materialsupplier");
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

            modelBuilder.Entity("CafeManager.Core.Data.Staffsalaryhistory", b =>
                {
                    b.HasOne("CafeManager.Core.Data.Staff", "Staff")
                        .WithMany("Staffsalaryhistories")
                        .HasForeignKey("Staffid")
                        .IsRequired()
                        .HasConstraintName("fk_staffsalaryhistory_staff");

                    b.Navigation("Staff");
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
                    b.Navigation("Materialsuppliers");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Materialsupplier", b =>
                {
                    b.Navigation("Consumedmaterials");

                    b.Navigation("Importdetails");
                });

            modelBuilder.Entity("CafeManager.Core.Data.Staff", b =>
                {
                    b.Navigation("Imports");

                    b.Navigation("Invoices");

                    b.Navigation("Staffsalaryhistories");
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
