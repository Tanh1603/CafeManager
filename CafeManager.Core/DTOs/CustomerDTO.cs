using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.DTOs
{
    public class CustomerDTO
    {
        public string? CustomerDisplayName { get; set; }
        public string? TypeCustomer { get; set; }
        public DateTime? BuyDate { get; set; }
        public decimal? TotalSpent { get; set; }
    }
}