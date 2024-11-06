using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.Stores
{
    public partial class NavigationStore
    {
        private readonly IServiceProvider _provider;

        private ObservableObject _navigation;

        public ObservableObject Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                if (value != null)
                {
                    NavigationStoreChanged?.Invoke();
                }
            }
        }

        public event Action NavigationStoreChanged;

        public NavigationStore(IServiceProvider provider)
        {
            _provider = provider;
        }
    }
}