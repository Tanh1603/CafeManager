﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace CafeManager.Core.Data;

public partial class Material
{
    public int Materialid { get; set; }

    public string Materialname { get; set; }

    public string Unit { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual ICollection<Materialsupplier> Materialsuppliers { get; set; } = new List<Materialsupplier>();
}