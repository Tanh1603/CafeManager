using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels
{
    public partial class FoodListViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;

        public FoodListViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }
    }
}