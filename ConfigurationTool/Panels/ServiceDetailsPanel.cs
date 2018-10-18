// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class ServiceDetailsPanel : BasePanel
    {
        private Service _service;

        public ServiceDetailsPanel()
        {
            InitializeComponent();

            _priceComboBox.Items.Add(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol);
            _priceComboBox.Items.Add("%");
        }

        public void SetService(Service service)
        {
            _service = service;
            _nameBox.Text = _service.Name;
            _descriptionBox.Text = _service.Description;
            _priceBox.FloatValue = _service.Price;
            _priceComboBox.SelectedIndex = (_service.IsPriceFixed) ? 0 : 1;
            _isPermanentCheckbox.Checked = !_service.IsPermanent;
        }

        private void _descriptionBox_TextChanged(object sender, EventArgs e)
        {
            _service.Description = _descriptionBox.Text;
        }

        private void _priceBox_TextChanged(object sender, EventArgs e)
        {
            _service.Price = _priceBox.FloatValue;
        }

        private void _priceBox_Validating(object sender, CancelEventArgs e)
        {
            if (_priceBox.FloatValue <= 0)
            {
                ShowToolTip(RM.GetString("PriceError"), _priceBox);
                e.Cancel = true;
            }
        }

        private void _isPermanentCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _service.IsPermanent = !_isPermanentCheckbox.Checked;
        }

        private void _priceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _service.IsPriceFixed = _priceComboBox.SelectedIndex == 0;
        }
    }
}