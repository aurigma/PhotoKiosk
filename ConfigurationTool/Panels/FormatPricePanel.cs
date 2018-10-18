// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class FormatPricePanel : BasePanel
    {
        private bool _isLoaded = false;
        private Product _product;

        public FormatPricePanel()
        {
            InitializeComponent();
        }

        public void SetProduct(Product product, string name)
        {
            _isLoaded = false;
            _product = product;
            _nameBox.Text = name;

            _discountsView.Rows.Clear();
            foreach (Discount discount in _product.Discounts)
            {
                int index = _discountsView.Rows.Add();
                _discountsView["FromColumn", index].Value = discount.Start;
                _discountsView["ToColumn", index].Value = discount.End == int.MaxValue ? "-" : discount.End.ToString(CultureInfo.CurrentCulture);
                _discountsView["PriceColumn", index].Value = discount.Price;
            }

            _isLoaded = true;
        }

        private void SetError(DataGridViewCell cell, string text)
        {
            cell.ErrorText = text;
            if (string.IsNullOrEmpty(text))
            {
                cell.OwningColumn.ReadOnly = false;
            }
            else
            {
                cell.OwningColumn.ReadOnly = true;
                cell.ReadOnly = false;
            }
        }

        private void _discountsView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_isLoaded)
            {
                var cell = _discountsView[e.ColumnIndex, e.RowIndex];
                if (cell.ColumnIndex == _discountsView.Columns["ToColumn"].Index)
                {
                    int toValue = int.MaxValue;
                    if (cell.Value == null || (!cell.Value.ToString().Equals("-") && !int.TryParse(cell.Value.ToString(), out toValue)) || toValue <= 0)
                    {
                        SetError(cell, RM.GetString("DiscountValueToLessError"));
                        return;
                    }

                    if (toValue == int.MaxValue)
                        cell.Value = "-";

                    int fromValue = int.Parse(_discountsView["FromColumn", e.RowIndex].Value.ToString(), CultureInfo.CurrentCulture);

                    if (e.RowIndex < _discountsView.Rows.Count - 1)
                    {
                        int nextToValue = int.MaxValue;
                        if (_discountsView["ToColumn", e.RowIndex + 1].Value != null && !_discountsView["ToColumn", e.RowIndex + 1].Value.ToString().Equals("-"))
                            int.TryParse(_discountsView["ToColumn", e.RowIndex + 1].Value.ToString(), out nextToValue);
                        if (toValue < fromValue || toValue > nextToValue - 2)
                        {
                            SetError(cell, string.Format(RM.GetString("DiscountValueOutOfRangeError"), fromValue, nextToValue - 2));
                            return;
                        }
                    }
                    else
                    {
                        if (toValue < fromValue)
                        {
                            SetError(cell, string.Format(RM.GetString("DiscountValueToBigError"), fromValue));
                            return;
                        }

                        _discountsView.Rows.Add(toValue + 1, "-", _product.Price);
                    }

                    SetError(cell, "");
                    _discountsView["FromColumn", e.RowIndex + 1].Value = toValue + 1;
                }
                else if (cell.ColumnIndex == _discountsView.Columns["PriceColumn"].Index)
                {
                    float testValue;
                    if (cell.Value == null || !float.TryParse(cell.Value.ToString(), out testValue) || testValue <= 0)
                        SetError(cell, RM.GetString("PriceError"));
                    else
                    {
                        SetError(cell, "");
                        if (e.RowIndex == 0)
                            _product.Price = testValue;
                    }
                }

                _discountsView_Validating(null, null);
            }
        }

        private void _discountsView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Index == 0)
            {
                e.Cancel = true;
            }
            else if (e.Row.Index == _discountsView.Rows.Count - 1)
            {
                _isLoaded = false;
                _discountsView["ToColumn", e.Row.Index - 1].Value = "-";
                _isLoaded = true;
            }
            else
                _discountsView["FromColumn", e.Row.Index].Value =
                    int.Parse(_discountsView["ToColumn", e.Row.Index - 1].Value.ToString(), CultureInfo.CurrentCulture) + 1;

            _discountsView_Validating(null, null);
        }

        private void _discountsView_Validating(object sender, CancelEventArgs e)
        {
            if (!BasePanel.HasErrors(_discountsView, true))
            {
                _product.Discounts.Clear();
                foreach (DataGridViewRow row in _discountsView.Rows)
                {
                    _product.AddDiscount(new Discount(
                        int.Parse(row.Cells["FromColumn"].Value.ToString(), CultureInfo.CurrentCulture),
                        row.Cells["ToColumn"].Value.ToString().Equals("-") ? int.MaxValue : int.Parse(row.Cells["ToColumn"].Value.ToString(), CultureInfo.CurrentCulture),
                        float.Parse(row.Cells["PriceColumn"].Value.ToString(), CultureInfo.CurrentCulture)));
                }
            }
            else
                e.Cancel = true;
        }

        private void _discountsView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            (e.Control as TextBox).KeyPress += cellEdit_KeyPress;
        }

        private void cellEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' &&
                (_discountsView.CurrentCell.OwningColumn.Name == "ToColumn" ||
                _discountsView.CurrentCell.OwningColumn.Name == "PriceColumn" && !e.KeyChar.ToString().Equals(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)))
                e.Handled = true;
        }

        private void _discountsView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_discountsView.Columns[e.ColumnIndex].Name == "PriceColumn")
            {
                float value;

                if (e.Value is float)
                {
                    value = (float)e.Value;
                }
                else if (e.Value is string)
                {
                    try
                    {
                        value = float.Parse((string)e.Value, CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        value = 0;
                    }
                }
                else
                    value = 0;

                e.Value = value.ToString("N2", CultureInfo.CurrentCulture);
                e.FormattingApplied = true;
            }
        }
    }
}