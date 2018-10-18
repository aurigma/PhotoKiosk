// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class PhotoPrinterPanel : BasePanel
    {
        private bool _isUpdating = true;
        private string[] _availablePrinters = Tools.GetAvailablePrinters().ToArray();

        public PhotoPrinterPanel()
        {
            InitializeComponent();
            _printersView.DataError += PrintView_DataError;

            (_printersView.Columns["PrinterColumn"] as DataGridViewComboBoxColumn).Items.Add("");
            (_printersView.Columns["PrinterColumn"] as DataGridViewComboBoxColumn).Items.AddRange(_availablePrinters);
        }

        private void PrintView_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
        }

        private void PhotoPrinterPanel_Load(object sender, EventArgs e)
        {
            if (Config != null)
            {
                _photoPrinterCheckbox.Checked = Config.EnablePhotoPrinting.Value;
                _confirmCheckbox.Checked = Config.PhotoPrintingRequireConfirm.Value;
                _paymentInfoBox.Text = Config.PhotoPrintingPaymentInstructions.Value;

                UpdatePapers();
                UpdateControlsState();
            }
        }

        private void PhotoPrinterPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (Config != null && Visible)
                UpdatePapers();
        }

        private void UpdateControlsState()
        {
            _printersView.Enabled = Config.EnablePhotoPrinting.Value;
            _confirmCheckbox.Enabled = Config.EnablePhotoPrinting.Value;

            _paymentInfoLabel.Enabled = Config.PhotoPrintingRequireConfirm.Value;
            _paymentInfoBox.Enabled = Config.PhotoPrintingRequireConfirm.Value;
        }

        private string GetProductKey(KeyValuePair<PaperFormat, PaperType> key)
        {
            return key.Key.Name + key.Value.Name + Constants.InstantKey;
        }

        private void UpdatePapers()
        {
            _isUpdating = true;
            _printersView.Rows.Clear();
            try
            {
                foreach (PaperFormat format in PriceManager.PaperFormats)
                {
                    if (!format.IsFree)
                    {
                        foreach (PaperType type in PriceManager.PaperTypes)
                        {
                            var key = new KeyValuePair<PaperFormat, PaperType>(format, type);
                            int index = _printersView.Rows.Add();

                            if (PriceManager.ContainsProduct(GetProductKey(key)))
                            {
                                var printer = PriceManager.GetProduct(GetProductKey(key)).Printer;
                                if (_availablePrinters.Contains(printer))
                                {
                                    _printersView["PrinterColumn", index].Value = printer;
                                }
                                else
                                {
                                    _printersView["PrinterColumn", index].Value = "";
                                }
                            }
                            else
                            {
                                _printersView["PrinterColumn", index].Value = "";
                            }

                            _printersView.Rows[index].Tag = key;
                            _printersView["PaperColumn", index].Value = format.Name + " " + type.Name;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            _isUpdating = false;
        }

        private void _photoPrinterCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnablePhotoPrinting.Value = _photoPrinterCheckbox.Checked;
            if (Parent is MainForm)
                (Parent as MainForm).UpdatePriceNodes();

            UpdateControlsState();
        }

        private void _printersView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isUpdating)
            {
                var key = (KeyValuePair<PaperFormat, PaperType>)_printersView.Rows[e.RowIndex].Tag;
                string newPrinter = (string)_printersView["PrinterColumn", e.RowIndex].Value;
                if (PriceManager.ContainsProduct(GetProductKey(key)))
                {
                    if (string.IsNullOrEmpty(newPrinter))
                        PriceManager.RemoveProduct(GetProductKey(key));
                    else
                        PriceManager.GetProduct(GetProductKey(key)).Printer = newPrinter;
                }
                else if (!string.IsNullOrEmpty(newPrinter))
                {
                    PriceManager.AddProduct(key.Key, key.Value, newPrinter);
                }

                if (Parent is MainForm)
                    (Parent as MainForm).UpdatePriceNodes();
            }
        }

        private void _confirmCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.PhotoPrintingRequireConfirm.Value = _confirmCheckbox.Checked;
            UpdateControlsState();
        }

        private void _paymentInfoBox_TextChanged(object sender, EventArgs e)
        {
            Config.PhotoPrintingPaymentInstructions.Value = _paymentInfoBox.Text;
        }
    }
}