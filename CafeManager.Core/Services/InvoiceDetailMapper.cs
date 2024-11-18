using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Services
{
    public static class InvoiceDetailMapper
    {
        public static InvoiceDetailDTO ToDTO(Invoicedetail invoicedetail)
        {
            return new InvoiceDetailDTO()
            {
                Invoicedetailid = invoicedetail.Invoicedetailid,
                Invoiceid = invoicedetail.Invoiceid,
                Foodid = invoicedetail.Foodid,
                Quantity = invoicedetail.Quantity ?? 0,
                Isdeleted = invoicedetail.Isdeleted,
                FoodDTO = FoodMapper.ToDTO(invoicedetail.Food),
            };
        }

        public static Invoicedetail ToEntity(InvoiceDetailDTO invoicedetailDTO)
        {
            return new Invoicedetail()
            {
                Invoicedetailid = invoicedetailDTO.Invoicedetailid,
                Invoiceid = invoicedetailDTO.Invoiceid,
                Foodid = invoicedetailDTO.Foodid,
                Quantity = invoicedetailDTO.Quantity,
                Isdeleted = invoicedetailDTO.Isdeleted,
            };
        }
    }
}