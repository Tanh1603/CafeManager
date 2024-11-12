﻿using CafeManager.Core.Data;
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

        public event Action? ChangeAccount;

        public void SetAccount(Appuser account)
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