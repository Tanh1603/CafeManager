using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace CafeManager.WPF.Stores
{
    public class AccountStore
    {
        private readonly IServiceProvider _provider;

        public AppUserDTO? Account { get; private set; }

        public AccountStore(IServiceProvider provider)
        {
            _provider = provider;
        }

        public event Action? ChangeAccount;

        public void SetAccount(AppUserDTO account)
        {
            Account = account;
            ChangeAccount?.Invoke();
        }

        public void ClearAccount()
        {
            Account = null;
        }
    }
}