using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace CafeManager.WPF.Services
{
    public class FileDialogService
    {
        public string? OpenFileDialog(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = filter
            };

            DialogResult result = openFileDialog.ShowDialog();
            return result == DialogResult.OK ? openFileDialog.FileName : null;
        }
    }
}