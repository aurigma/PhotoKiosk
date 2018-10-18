// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections;

namespace Aurigma.PhotoKiosk
{
    public class DataContext
    {
        private Hashtable _objectStorage = new Hashtable();

        public DataContext()
        {
            ExecutionEngine.EventLogger.Write("DataContext created");
        }

        public object this[string key]
        {
            get
            {
                return _objectStorage[key];
            }
            set
            {
                if (_objectStorage.Contains(key))
                {
                    _objectStorage[key] = value;
                }
                else
                {
                    _objectStorage.Add(key, value);
                }
            }
        }

        public bool Contains(string key)
        {
            return _objectStorage.Contains(key);
        }

        public void Remove(string key)
        {
            if (_objectStorage.Contains(key))
            {
                _objectStorage.Remove(key);
            }
        }
    }
}