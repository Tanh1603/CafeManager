using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace CafeManager.WPF.Services
{
    public class PrintInvoice
    {
        public static string ShowSaveFileDialog()
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
            // Chuyển BitmapSource sang MemoryStream
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using var memoryStream = new MemoryStream();
            encoder.Save(memoryStream);

            // Tạo file PDF
            var pdfDocument = new PdfDocument();
            var page = pdfDocument.AddPage();
            var graphics = XGraphics.FromPdfPage(page);

            // Chèn hình ảnh vào PDF
            using (XImage pdfImage = XImage.FromStream(memoryStream))
            {
                graphics.DrawImage(pdfImage, 0, 0, page.Width, page.Height);
            }

                // Lưu file
                pdfDocument.Save(outputPath);
        }
    }

}
