﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace CafeManager.Core.Data;

public partial class Supplier
{
    public int Supplierid { get; set; }

    public string Suppliername { get; set; }

    public string Representativesupplier { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public string Notes { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual ICollection<Import> Imports { get; set; } = new List<Import>();

    public virtual ICollection<Materialsupplier> Materialsuppliers { get; set; } = new List<Materialsupplier>();
}