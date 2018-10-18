// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class ReceiptPrinterPanel : BasePanel
    {
        public ReceiptPrinterPanel()
        {
            InitializeComponent();

            _printerNameBox.Items.AddRange(Tools.GetAvailablePrinters().ToArray());
        }

        private void ReceiptPrinterPanel_Load(object sender, EventArgs e)
        {
            if (Config != null)
            {
                _receiptPrinterCheckbox.Checked = Config.EnableReceiptPrinter.Value;
                if (_printerNameBox.Items.Contains(Config.ReceiptPrinterName.Value))
                    _printerNameBox.SelectedIndex = _printerNameBox.Items.IndexOf(Config.ReceiptPrinterName.Value);
                else if (string.IsNullOrEmpty(Config.ReceiptPrinterName.Value) && _printerNameBox.Items.Count > 0)
                    _printerNameBox.SelectedIndex = 0;

                _countBox.IntValue = Config.PhotosInReceipt.Value;
                _templateBox.Text = Config.ReceiptTemplateFile.Value;

                UpdateControlsState();
            }
        }

        private void UpdateControlsState()
        {
            _printerNameLabel.Enabled = Config.EnableReceiptPrinter.Value;
            _printerNameBox.Enabled = Config.EnableReceiptPrinter.Value;
            _countLabel.Enabled = Config.EnableReceiptPrinter.Value;
            _countBox.Enabled = Config.EnableReceiptPrinter.Value;
            _countLabelNote.Enabled = Config.EnableReceiptPrinter.Value;
            _templateLabel.Enabled = Config.EnableReceiptPrinter.Value;
            _templateBox.Enabled = Config.EnableReceiptPrinter.Value;
            _templateButton.Enabled = Config.EnableReceiptPrinter.Value;
            _testButton.Enabled = Config.EnableReceiptPrinter.Value;
        }

        private void _receiptPrinterCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableReceiptPrinter.Value = _receiptPrinterCheckbox.Checked;
            UpdateControlsState();
        }

        private void _printerNameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.ReceiptPrinterName.Value = _printerNameBox.SelectedItem.ToString();
        }

        private void _countBox_TextChanged(object sender, EventArgs e)
        {
            Config.PhotosInReceipt.Value = _countBox.IntValue;
        }

        private void _templateBox_TextChanged(object sender, EventArgs e)
        {
            Config.ReceiptTemplateFile.Value = _templateBox.Text;
        }

        private void _templateBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(_templateBox.Text) || !File.Exists(_templateBox.Text))
                ShowToolTip(RM.GetString("ReceiptTemplateError"), _templateBox);
        }

        private void _templateButton_Click(object sender, EventArgs e)
        {
            _openFileDialog.FileName = _templateBox.Text;

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
                _templateBox.Text = _openFileDialog.FileName;
        }

        private void _testButton_Click(object sender, EventArgs e)
        {
            try
            {
                var receiptData = new ReceiptData();
                receiptData.Id = string.Format(RM.GetString("RecieptTestOrderId"));
                receiptData.UserName = string.Format(RM.GetString("RecieptTestCustomerName"));
                receiptData.UserPhone = "1234567890";
                receiptData.BurnCd = false;
                receiptData.OrderPhotos = true;
                receiptData.CropMode = string.Format(RM.GetString("RecieptTestCropMode"));
                receiptData.PaperType = string.Format(RM.GetString("RecieptTestPaperType"));
                receiptData.OrderDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                receiptData.PhotosCount = 10;
                receiptData.PrintsCount = 20;
                receiptData.OrderCost = (receiptData.PrintsCount * 10).ToString("c", NumberFormatInfo.CurrentInfo);
                receiptData.Formats.Add(new FormatInfo("10x15", 10));
                receiptData.Formats.Add(new FormatInfo("20x30", 10));
                for (int i = 0; i < receiptData.PhotosCount; i++)
                {
                    receiptData.Photos.Add(new PhotoInfo("P000000" + i.ToString() + ".JPG", 1, "10x15"));
                    receiptData.Photos.Add(new PhotoInfo("P000000" + i.ToString() + ".JPG", 1, "20x30"));
                }
                receiptData.MorePhotos = false;

                var receipt = new Receipt(receiptData, Config.ReceiptTemplateFile.Value);
                receipt.Print(Config.ReceiptPrinterName.Value);
            }
            catch (Exception ex)
            {
                MainForm.ShowWarningMessage(string.Format(RM.GetString("PrintingError"), ex.Message));
            }
        }
    }
}