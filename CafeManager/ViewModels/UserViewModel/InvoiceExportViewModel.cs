using CafeManager.Core.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CafeManager.WPF.ViewModels.UserViewModel
{
    public partial class InvoiceExportViewModel : ObservableObject
    {
        [ObservableProperty]
        private InvoiceDTO _invoiceExport = new();
    }
}