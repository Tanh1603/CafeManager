using CafeManager.Core.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class ModifyInvoiceViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;

        [ObservableProperty]
        private InvoiceDTO _currentInvoiceDTO = new();

        public ModifyInvoiceViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void RecieveInvoiceDTO(InvoiceDTO invoiceDTO)
        {
            CurrentInvoiceDTO = invoiceDTO;
        }
    }
}