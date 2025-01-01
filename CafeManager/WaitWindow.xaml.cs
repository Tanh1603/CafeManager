using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CafeManager.WPF
{
    /// <summary>
    /// Interaction logic for WaitWindow.xaml
    /// </summary>
    public partial class WaitWindow : Window
    {
        private DispatcherTimer _timer;
        private int _dotCount = 1;  // Chúng ta sẽ thay đổi số lượng dấu chấm

        public WaitWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.5); // Cập nhật mỗi 0.5 giây
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string loadingText = "Loading";
            // Cập nhật số lượng dấu chấm
            for (int i = 0; i < _dotCount; i++)
            {
                loadingText += ".";
            }

            // Cập nhật text trong TextBlock
            LoadingTextBlock.Text = loadingText;

            // Thay đổi số dấu chấm (1 -> 2 -> 3 -> 1)
            _dotCount = (_dotCount % 3) + 1;
        }
    }
}