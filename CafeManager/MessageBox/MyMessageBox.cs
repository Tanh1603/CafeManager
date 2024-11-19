using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.MessageBox
{
    public class MyMessageBox
    {
        public enum Buttons { OK, Yes_No, Yes_No_Cancel}

        public enum Icons { None, Information, Warning, Error, Question}


        public static string Show(string Text)
        {
            return Show(Text, Buttons.OK, Icons.None);
        }

        public static string Show(string Text, Buttons buttons, Icons icon)
        {
            MyMessageBoxView messageBox = new MyMessageBoxView(Text, buttons, icon);
            messageBox.Show();
            return messageBox.ReturnString;
        }

        public static string ShowDialog(string Text)
        {
            return ShowDialog(Text, Buttons.OK, Icons.None);
        }

        public static string ShowDialog(string Text, Buttons buttons, Icons icon)
        {
            MyMessageBoxView messageBox = new MyMessageBoxView(Text, buttons, icon);
            messageBox.ShowDialog();
            return messageBox.ReturnString;
        }
    }
}
