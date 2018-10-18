// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core.OrderManager;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class ArbitraryFormatPanel : BasePanel
    {
        private TextBox _selectedTextBox;

        public ArbitraryFormatPanel()
        {
            InitializeComponent();

            string[] tags = new string[OrderManager.TagsCount];
            OrderManager.TagList.Keys.CopyTo(tags, 0);
            _tagsBox.Items.AddRange(tags);
            _tagsBox.SelectedIndex = OrderManager.TagsCount - 1;

            _selectedTextBox = _photoTemplateBox;
        }

        private void ArbitraryFormatPanel_Load(object sender, EventArgs e)
        {
            if (OrderManager != null)
            {
                _photoTemplateBox.Text = OrderManager.PhotoTemplate.Value;
                _orderInfoTemplateBox.Text = OrderManager.InfoTemplate.Value;
            }
        }

        private void ArbitraryFormatPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (OrderManager != null && Config != null && Visible)
            {
                bool condition = !OrderManager.IsDpof.Value && Config.EnablePhotoOrdering.Value;
                _photoTemplateLabel.Enabled = condition;
                _photoTemplateBox.Enabled = condition;
                _clearPhotoTemplateButton.Enabled = condition;
                _orderInfoTemplateLabel.Enabled = condition;
                _orderInfoTemplateBox.Enabled = condition;
                _clearOrderInfoTemplateButton.Enabled = condition;
                _tagsLabel.Enabled = condition;
                _tagsBox.Enabled = condition;
                _insertTagButton.Enabled = condition;
            }
        }

        private void _photoTemplateBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_photoTemplateBox.Text))
                OrderManager.PhotoTemplate.Value = _photoTemplateBox.Text;
            else
                _clearPhotoTemplateButton_Click(null, null);
        }

        private void _photoTemplateBox_Validating(object sender, CancelEventArgs e)
        {
            if (!_photoTemplateBox.Text.EndsWith(OrderManager.FilenameTag))
                ShowToolTip(string.Format(RM.GetString("PhotoTemplateError"), OrderManager.FilenameTag), _photoTemplateBox);
        }

        private void _clearPhotoTemplateButton_Click(object sender, EventArgs e)
        {
            _photoTemplateBox.Text = OrderManager.FilenameTag;
        }

        private void _orderInfoTemplateBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_orderInfoTemplateBox.Text))
                OrderManager.InfoTemplate.Value = _orderInfoTemplateBox.Text;
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
    }
}