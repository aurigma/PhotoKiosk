// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    internal class NumericTextBox : TextBox
    {
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            var numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string keyInput = e.KeyChar.ToString();

            if (!Char.IsDigit(e.KeyChar) && !keyInput.Equals(decimalSeparator) && e.KeyChar != '\b')
                e.Handled = true;
        }

        protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
        {
            base.OnValidating(e);

            if (_isFloat)
                Text = FloatValue.ToString("N2", CultureInfo.CurrentCulture);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
                Text = "0";

            base.OnTextChanged(e);
        }

        private bool _isFloat = false;

        public bool IsFloat
        {
            get { return _isFloat; }
            set { _isFloat = value; }
        }

        public int IntValue
        {
            get
            {
                try
                {
                    return int.Parse(Text, CultureInfo.CurrentCulture);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                Text = value.ToString(CultureInfo.CurrentCulture);
            }
        }

        public float FloatValue
        {
            get
            {
                try
                {
                    return float.Parse(Text, CultureInfo.CurrentCulture);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                Text = value.ToString("N2", CultureInfo.CurrentCulture);
            }
        }
    }
}