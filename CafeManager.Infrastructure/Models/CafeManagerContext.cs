﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using CafeManager.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeManager.Infrastructure.Models;

public partial class CafeManagerContext : DbContext
{
    public CafeManagerContext(DbContextOptions<CafeManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appuser> Appusers { get; set; }

    public virtual DbSet<Coffeetable> Coffeetables { get; set; }

    public virtual DbSet<Consumedmaterial> Consumedmaterials { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Foodcategory> Foodcategories { get; set; }

    public virtual DbSet<Import> Imports { get; set; }

    public virtual DbSet<Importdetail> Importdetails { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Invoicedetail> Invoicedetails { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Materialsupplier> Materialsuppliers { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Staffsalaryhistory> Staffsalaryhistories { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appuser>(entity =>
        {
            entity.HasKey(e => e.Appuserid).HasName("pk_appuserid");

            entity.ToTable("appuser");

            entity.Property(e => e.Appuserid).HasColumnName("appuserid");
            entity.Property(e => e.Avatar).HasColumnName("avatar");
            entity.Property(e => e.Displayname)
                .HasMaxLength(100)
                .HasDefaultValueSql("'Unkown'::character varying")
                .HasColumnName("displayname");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasDefaultValue(0)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Coffeetable>(entity =>
        {
            entity.HasKey(e => e.Coffeetableid).HasName("pk_coffeetable");

            entity.ToTable("coffeetable");

            entity.HasIndex(e => e.Tablenumber, "coffeetable_tablenumber_key").IsUnique();

            entity.Property(e => e.Coffeetableid).HasColumnName("coffeetableid");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Seatingcapacity)
                .HasDefaultValue(4)
                .HasColumnName("seatingcapacity");
            entity.Property(e => e.Statustable)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Đang sử dụng'::character varying")
                .HasColumnName("statustable");
            entity.Property(e => e.Tablenumber).HasColumnName("tablenumber");
        });

        modelBuilder.Entity<Consumedmaterial>(entity =>
        {
            entity.HasKey(e => e.Consumedmaterialid).HasName("pk_consumedmaterials");

            entity.ToTable("consumedmaterials");

            entity.Property(e => e.Consumedmaterialid).HasColumnName("consumedmaterialid");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Materialsupplierid).HasColumnName("materialsupplierid");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("quantity");

            entity.HasOne(d => d.Materialsupplier).WithMany(p => p.Consumedmaterials)
                .HasForeignKey(d => d.Materialsupplierid)
                .HasConstraintName("pk_consumedmaterials_materialsupplierid");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.Foodid).HasName("pk_food");

            entity.ToTable("food");

            entity.Property(e => e.Foodid).HasColumnName("foodid");
            entity.Property(e => e.Discountfood)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("discountfood");
            entity.Property(e => e.Foodcategoryid).HasColumnName("foodcategoryid");
            entity.Property(e => e.Foodname)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("foodname");
            entity.Property(e => e.Imagefood).HasColumnName("imagefood");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("price");

            entity.HasOne(d => d.Foodcategory).WithMany(p => p.Foods)
                .HasForeignKey(d => d.Foodcategoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_food_foodcategory");
        });

        modelBuilder.Entity<Foodcategory>(entity =>
        {
            entity.HasKey(e => e.Foodcategoryid).HasName("pk_foodcategory");

            entity.ToTable("foodcategory");

            entity.Property(e => e.Foodcategoryid).HasColumnName("foodcategoryid");
            entity.Property(e => e.Foodcategoryname)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("foodcategoryname");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
        });

        modelBuilder.Entity<Import>(entity =>
        {
            entity.HasKey(e => e.Importid).HasName("pk_imports");

            entity.ToTable("imports");

            entity.Property(e => e.Importid).HasColumnName("importid");
            entity.Property(e => e.Deliveryperson)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("deliveryperson");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Receiveddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("receiveddate");
            entity.Property(e => e.Shippingcompany)
                .HasMaxLength(100)
                .HasColumnName("shippingcompany");
            entity.Property(e => e.Staffid).HasColumnName("staffid");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");

            entity.HasOne(d => d.Staff).WithMany(p => p.Imports)
                .HasForeignKey(d => d.Staffid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_imports_staff");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Imports)
                .HasForeignKey(d => d.Supplierid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_imports_supplier");
        });

        modelBuilder.Entity<Importdetail>(entity =>
        {
            entity.HasKey(e => e.Importdetailid).HasName("pk_importdetails");

            entity.ToTable("importdetails");

            entity.Property(e => e.Importdetailid).HasColumnName("importdetailid");
            entity.Property(e => e.Importid).HasColumnName("importid");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("quantity");

            entity.HasOne(d => d.Import).WithMany(p => p.Importdetails)
                .HasForeignKey(d => d.Importid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_importdetails_imports");

            entity.HasOne(d => d.Material).WithMany(p => p.Importdetails)
                .HasForeignKey(d => d.Materialid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_importdetails_material");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Invoiceid).HasName("pk_invoices");

            entity.ToTable("invoices");

            entity.Property(e => e.Invoiceid).HasColumnName("invoiceid");
            entity.Property(e => e.Coffeetableid).HasColumnName("coffeetableid");
            entity.Property(e => e.Discountinvoice)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("discountinvoice");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Paymentenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("paymentenddate");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Thanh toán tiền mặt'::character varying")
                .HasColumnName("paymentmethod");
            entity.Property(e => e.Paymentstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("paymentstartdate");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Chưa thanh toán'::character varying")
                .HasColumnName("paymentstatus");
            entity.Property(e => e.Staffid).HasColumnName("staffid");

            entity.HasOne(d => d.Coffeetable).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Coffeetableid)
                .HasConstraintName("fk_invoices_coffeetable");

            entity.HasOne(d => d.Staff).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Staffid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoices_staff");
        });

        modelBuilder.Entity<Invoicedetail>(entity =>
        {
            entity.HasKey(e => e.Invoicedetailid).HasName("pk_invoicedetails");

            entity.ToTable("invoicedetails");

            entity.Property(e => e.Invoicedetailid).HasColumnName("invoicedetailid");
            entity.Property(e => e.Foodid).HasColumnName("foodid");
            entity.Property(e => e.Invoiceid).HasColumnName("invoiceid");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");

            entity.HasOne(d => d.Food).WithMany(p => p.Invoicedetails)
                .HasForeignKey(d => d.Foodid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoicedetails_food");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Invoicedetails)
                .HasForeignKey(d => d.Invoiceid)
                .HasConstraintName("fk_invoicedetails_invoices");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Materialid).HasName("pk_material");

            entity.ToTable("material");

            entity.HasIndex(e => e.Materialname, "material_materialname_key").IsUnique();

            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Materialname)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("materialname");
            entity.Property(e => e.Unit)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<Materialsupplier>(entity =>
        {
            entity.HasKey(e => e.Materialsupplierid).HasName("pk_materialsupplier");

            entity.ToTable("materialsupplier");

            entity.Property(e => e.Materialsupplierid).HasColumnName("materialsupplierid");
            entity.Property(e => e.Expirationdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expirationdate");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Manufacturedate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("manufacturedate");
            entity.Property(e => e.Manufacturer)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("manufacturer");
            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Original)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("original");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("price");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");

            entity.HasOne(d => d.Material).WithMany(p => p.Materialsuppliers)
                .HasForeignKey(d => d.Materialid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_material_supplier");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Materialsuppliers)
                .HasForeignKey(d => d.Supplierid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_supplier_material");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Staffid).HasName("pk_staff");

            entity.ToTable("staff");

            entity.Property(e => e.Staffid).HasColumnName("staffid");
            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("address");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Endworkingdate).HasColumnName("endworkingdate");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Sex).HasColumnName("sex");
            entity.Property(e => e.Staffname)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("staffname");
            entity.Property(e => e.Startworkingdate).HasColumnName("startworkingdate");
        });

        modelBuilder.Entity<Staffsalaryhistory>(entity =>
        {
            entity.HasKey(e => e.Staffsalaryhistoryid).HasName("pk_staffsalaryhistory");

            entity.ToTable("staffsalaryhistory");

            entity.Property(e => e.Staffsalaryhistoryid).HasColumnName("staffsalaryhistoryid");
            entity.Property(e => e.Effectivedate).HasColumnName("effectivedate");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Salary)
                .HasPrecision(10, 2)
                .HasColumnName("salary");
            entity.Property(e => e.Staffid).HasColumnName("staffid");

            entity.HasOne(d => d.Staff).WithMany(p => p.Staffsalaryhistories)
                .HasForeignKey(d => d.Staffid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_staffsalaryhistory_staff");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Supplierid).HasName("pk_supplier");

            entity.ToTable("supplier");

            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Representativesupplier)
                .IsRequired()
                .HasColumnType("character varying")
                .HasColumnName("representativesupplier");
            entity.Property(e => e.Suppliername)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("suppliername");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.LogTo(Console.WriteLine);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}