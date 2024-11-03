﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CafeManager.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainAdminView.xaml
    /// </summary>
    public partial class MainAdminView : UserControl
    {
        internal Func<object, object, object> CloseRequested;

        public MainAdminView()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                Window parentWindow = Window.GetWindow(this); // Lấy Window chứa UserControl
                parentWindow?.DragMove();
            }
        }

       

       
      

        

        private void closeIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            this.Visibility = Visibility.Collapsed;
            parentWindow?.Close();
        }

        private void minimizeIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            this.Visibility = Visibility.Hidden;
            parentWindow.WindowState = WindowState.Minimized;
        }

        
    }
}