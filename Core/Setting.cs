// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Configuration;

namespace Aurigma.PhotoKiosk.Core
{
    public class Setting<T>
    {
        private readonly bool _isReadonly;
        private readonly string _key;
        private readonly T _defValue;
        private readonly bool _canBeEmpty;

        private T _value;

        internal Setting(bool isReadonly, string key, T value, bool canBeEmpty, SettingChangedHandler handler)
        {
            _isReadonly = isReadonly;
            _key = key;
            _value = value;
            _defValue = value;
            _canBeEmpty = canBeEmpty;

            if (handler != null)
                ValueChanged += new SettingChangedHandler(handler);
        }

        internal Setting(bool isReadonly, string key, T value, bool canBeEmpty)
            : this(isReadonly, key, value, canBeEmpty, null)
        {
        }

        internal void Init(T value)
        {
            _value = value;
        }

        internal bool CanBeEmpty
        {
            get { return _canBeEmpty; }
        }

        internal string Key
        {
            get { return _key; }
        }

        public T DefaultValue
        {
            get { return _defValue; }
        }

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (!_value.Equals(value))
                    {
                        _value = value;
                        if (ValueChanged != null)
                            ValueChanged(this);
                    }
                }
                else
                    throw new InvalidOperationException(_key + " setting is readonly.");
            }
        }

        public delegate void SettingChangedHandler(object sender);

        public event SettingChangedHandler ValueChanged;
    }

    public class ScreenSetting
    {
        private readonly string _key;

        public readonly Setting<string> Background;
        public readonly Setting<string> Header;

        internal ScreenSetting(bool isReadonly, string key, Setting<string>.SettingChangedHandler handler)
        {
            _key = key;
            Background = new Setting<string>(isReadonly, _key + "Background", "", true, handler);
            Header = new Setting<string>(isReadonly, _key + "Header", "", true, handler);
        }

        internal void Init(Configuration config)
        {
            try
            {
                Background.Init(config.AppSettings.Settings[Background.Key].Value);
                Header.Init(config.AppSettings.Settings[Header.Key].Value);
            }
            catch (Exception)
            {
            }
        }

        internal void Save(Configuration config)
        {
            Config.SaveStringSetting(Background, config);
            Config.SaveStringSetting(Header, config);
        }

        public string Key
        {
            get { return _key; }
        }
    }
}