﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace CafeManager.Core.Data;

public partial class Staff
{
    public int Staffid { get; set; }

    public string Staffname { get; set; }

    public string Phone { get; set; }

    public bool? Sex { get; set; }

    public DateOnly Birthday { get; set; }

    public string Address { get; set; }

    public DateOnly Startworkingdate { get; set; }

    public DateOnly? Endworkingdate { get; set; }

    public string Role { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual ICollection<Import> Imports { get; set; } = new List<Import>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Staffsalaryhistory> Staffsalaryhistories { get; set; } = new List<Staffsalaryhistory>();
}