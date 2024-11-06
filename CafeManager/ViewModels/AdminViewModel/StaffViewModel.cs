using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class StaffViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;

        public StaffViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }
    }
}