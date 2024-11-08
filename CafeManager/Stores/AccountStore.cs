using CafeManager.Core.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CafeManager.WPF.Stores
{
    public class AccountStore
    {
        private readonly IServiceProvider _provider;

        public Appuser? Account { get; private set; }

        public AccountStore(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void SetAccount(Appuser account)
        {
            if (Account == null)
            {
                Account = account;
            }
        }

        public void ClearAccount()
        {
            Account = null;
        }
    }
}