using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Printing;
using System.Drawing;

namespace CafeManager.WPF.Services
{
    public static class PrintInvoiceServices
    {
        public static string? ShowSaveFileDialog()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                Title = "Xuất hóa đơn"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }

            return null;
        }

        public static BitmapSource RenderViewToBitmap(FrameworkElement element, double width, double height)
        {
            element.Measure(new System.Windows.Size(width, height));
            element.Arrange(new Rect(0, 0, width, height));

            var renderBitmap = new RenderTargetBitmap(
                (int)width, (int)height,
                96, 96, PixelFormats.Pbgra32);

            renderBitmap.Render(element);
            return renderBitmap;
        }

        public static void SaveBitmapToPdf(BitmapSource bitmap, string outputPath)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using var memoryStream = new MemoryStream();
            encoder.Save(memoryStream);

            var pdfDocument = new PdfDocument();
            var page = pdfDocument.AddPage();
            var graphics = XGraphics.FromPdfPage(page);

            using (XImage pdfImage = XImage.FromStream(memoryStream))
            {
                graphics.DrawImage(pdfImage, 0, 0, page.Width, page.Height);
            }
            pdfDocument.Save(outputPath);
        }

        public static void PrintToPrinter(BitmapSource bitmap)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using var memoryStream = new MemoryStream();
            encoder.Save(memoryStream);

            var image = Image.FromStream(memoryStream);
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += (sender, e) =>
            {
                e.Graphics.DrawImage(image, 0, 0);
            };
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.PrinterSettings = printDialog.PrinterSettings;
                printDoc.Print();
            }
        }
    }
}