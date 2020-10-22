using System;
using System.Linq;
using System.Collections.Generic;

using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.Models
{
    public class DataTracker
    {
        private readonly Dictionary<string, bool> _dataTable;

        private bool _isUpdateRequired;

        public bool IsUpdateRequired
        {
            get => _isUpdateRequired;
        }

        public DataTracker(params string[] subscribers)
        {
            _dataTable = new Dictionary<string, bool>();
            _isUpdateRequired = false;

            foreach (var subscriber in subscribers)
            {
                _dataTable.Add(subscriber, _isUpdateRequired);
            }
        }

        public void ChangeData(string key)
        {
            var exist = _dataTable.CheckStringKey(key);

            if (exist)
            {
                _isUpdateRequired = true;
                _dataTable[key] = true;
            }
        }
        public bool IsDataChanged(string key)
        {
            var exist = _dataTable.CheckStringKey(key);

            if (exist)
            {
                return _dataTable[key];
            }
            else
            {
                throw new ArgumentException("Key Not Found!");
            }
        }
        public void Reset()
        {
            _isUpdateRequired = false;
            var keys = _dataTable.Keys.ToArray();
            foreach (var key in keys)
            {
                _dataTable[key] = false;
            }
        }
    }
}