// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using Aurigma.PhotoKiosk.Core.OrderManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class DpofPanel : BasePanel
    {
        private bool _isUpdating = true;
        private TextBox _selectedTextBox;

        public DpofPanel()
        {
            InitializeComponent();

            string[] tags = new string[OrderManager.TagsCount];
            OrderManager.TagList.Keys.CopyTo(tags, 0);
            _tagsBox.Items.AddRange(tags);
            _tagsBox.SelectedIndex = OrderManager.TagsCount - 1;

            _selectedTextBox = _orderTemplateBox;
        }

        private void DpofPanel_Load(object sender, EventArgs e)
        {
            if (OrderManager != null)
            {
                _orderTemplateBox.Text = OrderManager.DpofOrderTemplate.Value;
                _orderInfoTemplateBox.Text = OrderManager.DpofInfoTemplate.Value;

                UpdatePapers();
            }
        }

        private void DpofPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (OrderManager != null && Config != null && Visible)
            {
                bool condition = OrderManager.IsDpof.Value && Config.EnablePhotoOrdering.Value;
                _orderTemplateLabel.Enabled = condition;
                _orderTemplateBox.Enabled = condition;
                _clearOrderTemplateButton.Enabled = condition;
                _orderInfoTemplateLabel.Enabled = condition;
                _orderInfoTemplateBox.Enabled = condition;
                _clearOrderInfoTemplateButton.Enabled = condition;
                _tagsLabel.Enabled = condition;
                _tagsBox.Enabled = condition;
                _insertTagButton.Enabled = condition;
                _channelsView.Enabled = condition;

                UpdatePapers();
            }
        }

        private string GetProductKey(KeyValuePair<PaperFormat, PaperType> key)
        {
            return key.Key.Name + key.Value.Name;
        }

        private void UpdatePapers()
        {
            _isUpdating = true;
            _channelsView.Rows.Clear();
            foreach (PaperFormat format in PriceManager.PaperFormats)
            {
                foreach (PaperType type in PriceManager.PaperTypes)
                {
                    var key = new KeyValuePair<PaperFormat, PaperType>(format, type);
                    int index = _channelsView.Rows.Add();

                    if (PriceManager.ContainsProduct(GetProductKey(key)))
                        _channelsView["ChannelColumn", index].Value = PriceManager.GetProduct(GetProductKey(key)).Channel;
                    else
                        _channelsView["ChannelColumn", index].Value = "";

                    _channelsView.Rows[index].Tag = key;
                    _channelsView["PaperColumn", index].Value = format.Name + " " + type.Name;
                }
            }
            _isUpdating = false;
        }

        private void _orderTemplateBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_orderTemplateBox.Text))
                OrderManager.DpofOrderTemplate.Value = _orderTemplateBox.Text;
            else
                _clearOrderTemplateButton_Click(null, null);
        }

        private void _clearOrderTemplateButton_Click(object sender, EventArgs e)
        {
            _orderTemplateBox.Text = OrderManager.OrderIdTag;
        }

        private void _orderInfoTemplateBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_orderInfoTemplateBox.Text))
                OrderManager.DpofInfoTemplate.Value = _orderInfoTemplateBox.Text;
            else
                _clearOrderInfoTemplateButton_Click(null, null);
        }

        private void _clearOrderInfoTemplateButton_Click(object sender, EventArgs e)
        {
            _orderInfoTemplateBox.Text = OrderManager.OrderInfoFilename;
        }

        private void _insertTagButton_Click(object sender, EventArgs e)
        {
            string tag = OrderManager.TagList[_tagsBox.SelectedItem.ToString()];

            int selectionStart = _selectedTextBox.SelectionStart;
            _selectedTextBox.Text = _selectedTextBox.Text.Substring(0, selectionStart) + tag + _selectedTextBox.Text.Substring(selectionStart);
            _selectedTextBox.SelectionStart = selectionStart + tag.Length;
            _selectedTextBox.Focus();
        }

        private void _textBox_Enter(object sender, EventArgs e)
        {
            _selectedTextBox = sender as TextBox;
        }

        private void _channelsView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isUpdating)
            {
                var key = (KeyValuePair<PaperFormat, PaperType>)_channelsView.Rows[e.RowIndex].Tag;
                string newChannel = _channelsView["ChannelColumn", e.RowIndex].Value != null ? _channelsView["ChannelColumn", e.RowIndex].Value.ToString() : string.Empty;

                _channelsView.Rows[e.RowIndex].ErrorText = "";
                if (!string.IsNullOrEmpty(newChannel))
                {
                    foreach (DataGridViewRow row in _channelsView.Rows)
                    {
                        if (row.Index != e.RowIndex)
                        {
                            string name = _channelsView["ChannelColumn", row.Index].Value != null ? _channelsView["ChannelColumn", row.Index].Value.ToString() : string.Empty;
                            if (name.Equals(newChannel))
                            {
                                _channelsView.Rows[e.RowIndex].ErrorText = RM.GetString("ChannelError");
                                break;
                            }
                        }
                    }
                }

                if (PriceManager.ContainsProduct(GetProductKey(key)))
                {
                    PriceManager.GetProduct(GetProductKey(key)).Channel = newChannel;
                }
            }
        }

        private void _channelsView_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (BasePanel.HasErrors(_channelsView, false))
                e.Cancel = true;
        }

        private void _channelsView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            (e.Control as TextBox).KeyPress += cellEdit_KeyPress;
        }

        private void cellEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_channelsView.CurrentCell.OwningColumn.Name == "ChannelColumn")
            {
                e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != '\b';
            }
        }

        private void _orderTemplateLabel_Click(object sender, EventArgs e)
        {
        }
    }
}