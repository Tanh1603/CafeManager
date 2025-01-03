﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace CafeManager.Core.Data;

public partial class Invoice
{
    public int Invoiceid { get; set; }

    public DateTime Paymentstartdate { get; set; }

    public DateTime? Paymentenddate { get; set; }

    public string Paymentstatus { get; set; }

    public string Paymentmethod { get; set; }

    public decimal? Discountinvoice { get; set; }

    public int? Coffeetableid { get; set; }

    public int Staffid { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual Coffeetable Coffeetable { get; set; }

    public virtual ICollection<Invoicedetail> Invoicedetails { get; set; } = new List<Invoicedetail>();

    public virtual Staff Staff { get; set; }
}