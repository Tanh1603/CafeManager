using CafeManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.DTOs
{
    public class MaterialDetailDTO
    {
        public string? Materialname { get; set; }
        public string? Suppliername { get; set; }
        public string? Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Original { get; set; }
        public string? Manufacturer { get; set; }
        public DateTime Manufacturedate { get; set; }
        public DateTime Expirationdate { get; set; }
    }
}