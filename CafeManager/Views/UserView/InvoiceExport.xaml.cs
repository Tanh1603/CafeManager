using CafeManager.WPF.Services;
using System.Windows;

namespace CafeManager.WPF.Views.UserView
{
    /// <summary>
    /// Interaction logic for InvoiceExport.xaml
    /// </summary>
    public partial class InvoiceExport : Window
    {
        public InvoiceExport()
        {
            InitializeComponent();
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            btnChoice.Visibility = Visibility.Collapsed;
            var bitmap = PrintInvoiceServices.RenderViewToBitmap(this, this.Width, this.Height);
            PrintInvoiceServices.PrintToPrinter(bitmap);
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            btnChoice.Visibility = Visibility.Collapsed;
            var bitmap = PrintInvoiceServices.RenderViewToBitmap(this, this.Width, this.Height);
            string? pdfPath = PrintInvoiceServices.ShowSaveFileDialog();
            if (!string.IsNullOrEmpty(pdfPath))
            {
                PrintInvoiceServices.SaveBitmapToPdf(bitmap, pdfPath);
            }
            this.Close();
        }
    }
}