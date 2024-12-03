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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CafeManager.WPF.MessageBox
{
    /// <summary>
    /// Interaction logic for MyMessageBox.xaml
    /// </summary>
    public partial class MyMessageBoxView : Window
    {
        public string ReturnString { get; set; }
        public MyMessageBoxView(string Text, MyMessageBox.Buttons buttons, MyMessageBox.Icons icon)
        {
            InitializeComponent();
            txbText.Text = Text;
            SetButton(buttons);
            SetIcon(icon);

        }



    private void SetButton(MyMessageBox.Buttons buttons)
        {
            switch (buttons)
            {
                case MyMessageBox.Buttons.OK:
                    bntOk.Visibility = Visibility.Visible;
                    break;
                case MyMessageBox.Buttons.Yes_No:
                    bntYes.Visibility = Visibility.Visible;
                    bntNo.Visibility = Visibility.Visible;
                    break;
                case MyMessageBox.Buttons.Yes_No_Cancel:
                    bntYes.Visibility = Visibility.Visible;
                    bntNo.Visibility = Visibility.Visible;
                    bntCancel.Visibility = Visibility.Visible;
                    break;
            }
        }

    private void SetIcon(MyMessageBox.Icons icon)
    {
            string iconPath = string.Empty;
            switch (icon)
            {
                case MyMessageBox.Icons.Information:
                    iconPath = "/Assets/Images/information.png";
                    break;
                case MyMessageBox.Icons.Warning:
                    iconPath = "/Assets/Images/warning.png";
                    break;
                case MyMessageBox.Icons.Error:
                    iconPath = "/Assets/Images/error.png";
                    break;
                case MyMessageBox.Icons.Question:
                    iconPath = "/Assets/Images/question.png";
                    break;
                default:
                    messIcon.Visibility = Visibility.Collapsed;
                    return;
            }

            messIcon.Source = new BitmapImage(new Uri(iconPath, UriKind.Relative));
            messIcon.Visibility = Visibility.Visible;
        }


        DoubleAnimation anim;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.2));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Height = (txbText.LineCount * 27) + gBar.Height + 50;
        }

        private void bntReturnValue_Click(object sender, RoutedEventArgs e)
        {
            ReturnString = ((Button)sender).Uid.ToString();
            Close();
        }
    }
}
