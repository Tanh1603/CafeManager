﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace CafeManager.Core.Data;

public partial class Staffsalaryhistory
{
    public int Staffsalaryhistoryid { get; set; }

    public int Staffid { get; set; }

    public decimal Salary { get; set; }

    public DateOnly Effectivedate { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual Staff Staff { get; set; }
}