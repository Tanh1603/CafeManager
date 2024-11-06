using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class InvoiceViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;

        public InvoiceViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }
    }
}