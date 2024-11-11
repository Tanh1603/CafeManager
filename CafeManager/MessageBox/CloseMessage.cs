using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CafeManager.WPF.MessageBox
{
    public class CloseMessage
    {
        public static void AllMessageBoxes()
        {
            foreach (MyMessageBoxView window in Application.Current.Windows.OfType<MyMessageBoxView>())
                window.Close();
            while (Application.Current.Windows.OfType<MyMessageBoxView>().Count() > 0) ;
        }
    }
}
