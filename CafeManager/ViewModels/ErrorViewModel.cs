﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels
{
    public class ErrorViewModel : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();
        public bool HasErrors => _propertyErrors.Any(); 

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName) => _propertyErrors.GetValueOrDefault(propertyName, null);

        public void AddError(string propertyName, string errorMessage)
        {
            if(!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Add(propertyName, new List<string>());
            }
            if (!_propertyErrors[propertyName].Contains(errorMessage))
            {
                _propertyErrors[propertyName].Add(errorMessage);
                OnErrorsChanged(propertyName);
            }
        }
        public void RemoveErrors(string propertyName)
        {
            if(_propertyErrors.Remove(propertyName))
            {
              
                OnErrorsChanged(propertyName);
              

            }
        }
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
           
        }
    }
}
