using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Services
{
    public static class InvoiceMapper
    {
        public static InvoiceDTO ToDTO(Invoice invoice)
        {
            return new InvoiceDTO()
            {
                Invoiceid = invoice.Invoiceid,
                Coffeetableid = invoice.Coffeetableid,
                Paymentstartdate = invoice.Paymentstartdate,
                Paymentenddate = invoice.Paymentenddate,
                Paymentstatus = invoice.Paymentstatus,
                Paymentmethod = invoice.Paymentmethod,
                Discountinvoice = invoice.Discountinvoice ?? 0,
                Isdeleted = invoice.Isdeleted,
                Staffid = invoice.Staffid,

                StaffDTO = StaffMapper.ToDTO(invoice.Staff),
                ListInvoiceDTO = [.. invoice.Invoicedetails.Select(x => InvoiceDetailMapper.ToDTO(x))]
            };
        }

        public static Invoice ToEntity(InvoiceDTO invoiceDTO)
        {
            return new Invoice()
            {
                Invoiceid = invoiceDTO.Invoiceid,
                Coffeetableid = invoiceDTO.Coffeetableid,
                Paymentstartdate = invoiceDTO.Paymentstartdate,
                Paymentenddate = invoiceDTO.Paymentenddate,
                Paymentstatus = invoiceDTO.Paymentstatus,
                Paymentmethod = invoiceDTO.Paymentmethod,
                Discountinvoice = invoiceDTO?.Discountinvoice,
                Isdeleted = invoiceDTO?.Isdeleted,
                Staffid = invoiceDTO.Staffid,

                Staff = StaffMapper.ToEntity(invoiceDTO.StaffDTO),
                Invoicedetails = [.. invoiceDTO.ListInvoiceDTO.Select(x => InvoiceDetailMapper.ToEntity(x))]
            };
        }
    }
}