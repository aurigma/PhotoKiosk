// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class PricePanel : BasePanel
    {
        public PricePanel()
        {
            InitializeComponent();

            _minPriceLabel.Text = string.Format(RM.GetString("MinimumPriceLabel"), CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol);
        }

        private void PricePanel_Load(object sender, EventArgs e)
        {
            if (Config != null && PriceManager != null)
            {
                _priceFileBox.Text = Config.PriceFile.Value;
                PriceUpdated();
            }
        }

        protected override void PriceUpdated()
        {
            _minPriceBox.FloatValue = PriceManager.MinimumCost.Value;
            _salesTaxBox.FloatValue = PriceManager.SalesTaxPercent.Value * 100;
            _commentBox.Text = PriceManager.SalesTaxComment.Value;
        }

        private void _minPriceBox_TextChanged(object sender, EventArgs e)
        {
            PriceManager.MinimumCost.Value = _minPriceBox.FloatValue;
        }

        private void _salesTaxBox_TextChanged(object sender, EventArgs e)
        {
            PriceManager.SalesTaxPercent.Value = _salesTaxBox.FloatValue / 100.0f;
        }

        private void _commentBox_TextChanged(object sender, EventArgs e)
        {
            PriceManager.SalesTaxComment.Value = _commentBox.Text;
        }

        private void _priceFileBox_TextChanged(object sender, EventArgs e)
        {
            Config.PriceFile.Value = _priceFileBox.Text;
            PriceManager.FileName = _priceFileBox.Text;
        }

        private void _priceFileBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(_priceFileBox.Text) || !File.Exists(_priceFileBox.Text))
                ShowToolTip(RM.GetString("PriceFileError"), _priceFileBox);
        }

        private void _priceFileButton_Click(object sender, EventArgs e)
        {
            _openFileDialog.FileName = _priceFileBox.Text;

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
                _priceFileBox.Text = _openFileDialog.FileName;
        }
    }
}