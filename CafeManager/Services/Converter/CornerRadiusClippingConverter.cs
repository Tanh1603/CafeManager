using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CafeManager.WPF.Services.Converter
{
    public class CornerRadiusClippingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 ||
                !(values[0] is double width) ||
                !(values[1] is double height) ||
                !(values[2] is CornerRadius cornerRadius))
            {
                return Geometry.Empty;
            }

            //var rect = new Rect(0, 0, width, height);
            //var geometry = new RectangleGeometry(rect, cornerRadius.TopLeft, cornerRadius.TopLeft);
            var geometry = new StreamGeometry();
            using (var ctx = geometry.Open())
            {
                // Lấy bán kính cho từng góc
                double topLeft = cornerRadius.TopLeft;
                double topRight = cornerRadius.TopRight;
                double bottomRight = cornerRadius.BottomRight;
                double bottomLeft = cornerRadius.BottomLeft;

                // Vẽ hình chữ nhật với các góc bo khác nhau
                ctx.BeginFigure(new Point(topLeft, 0), true, true); // Góc trên bên trái

                // Top line
                ctx.LineTo(new Point(width - topRight, 0), false, false);
                ctx.ArcTo(new Point(width, topRight), new Size(topRight, topRight), 0, false, SweepDirection.Clockwise, true, false);

                // Right line
                ctx.LineTo(new Point(width, height - bottomRight), false, false);
                ctx.ArcTo(new Point(width - bottomRight, height), new Size(bottomRight, bottomRight), 0, false, SweepDirection.Clockwise, true, false);

                // Bottom line
                ctx.LineTo(new Point(bottomLeft, height), false, false);
                ctx.ArcTo(new Point(0, height - bottomLeft), new Size(bottomLeft, bottomLeft), 0, false, SweepDirection.Clockwise, true, false);

                // Left line
                ctx.LineTo(new Point(0, topLeft), false, false);
                ctx.ArcTo(new Point(topLeft, 0), new Size(topLeft, topLeft), 0, false, SweepDirection.Clockwise, true, false);
            }
            geometry.Freeze();
            return geometry;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}