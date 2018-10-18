// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using Aurigma.PhotoKiosk.Core.OrderManager;
using System;
using System.IO;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class OrderManagerPanel : BasePanel
    {
        public OrderManagerPanel()
        {
            InitializeComponent();
        }

        private void OrderManagerPanel_Load(object sender, EventArgs e)
        {
            if (Config != null && OrderManager != null)
            {
                _checkBox.Checked = Config.EnablePhotoOrdering.Value;
                _pathBox.Text = OrderManager.DestinationPath.Value;
                _convertCheckbox.Checked = OrderManager.ConvertToJpeg.Value;
                _cleanupCheckbox.Checked = OrderManager.EnableCleanup.Value;
                _transliterationCheckbox.Checked = OrderManager.TransliteratePath.Value;
                _dpofCheckbox.Checked = OrderManager.IsDpof.Value;

                UpdateControlStates();
            }
        }

        private void UpdateControlStates()
        {
            _pathLabel.Enabled = Config.EnablePhotoOrdering.Value;
            _pathBox.Enabled = Config.EnablePhotoOrdering.Value;
            _pathButton.Enabled = Config.EnablePhotoOrdering.Value;
            _convertCheckbox.Enabled = Config.EnablePhotoOrdering.Value;
            _cleanupCheckbox.Enabled = Config.EnablePhotoOrdering.Value;
            _transliterationCheckbox.Enabled = Config.EnablePhotoOrdering.Value;
            _dpofCheckbox.Enabled = Config.EnablePhotoOrdering.Value;
        }

        private void _checkBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnablePhotoOrdering.Value = _checkBox.Checked;
            if (Parent is MainForm)
                (Parent as MainForm).UpdatePriceNodes();

            UpdateControlStates();
        }

        private void _pathBox_TextChanged(object sender, EventArgs e)
        {
            OrderManager.DestinationPath.Value = _pathBox.Text;
        }

        private void _pathButton_Click(object sender, EventArgs e)
        {
            _folderBrowserDialog.SelectedPath = _pathBox.Text;

            if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
                _pathBox.Text = _folderBrowserDialog.SelectedPath;
        }

        private void _pathBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(_pathBox.Text) || !Directory.Exists(_pathBox.Text))
                ShowToolTip(RM.GetString("DestinationPathError"), _pathBox);
        }

        private void _convertCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            OrderManager.ConvertToJpeg.Value = _convertCheckbox.Checked;
        }

        private void _cleanupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            OrderManager.EnableCleanup.Value = _cleanupCheckbox.Checked;
        }

        private void _transliterationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            OrderManager.TransliteratePath.Value = _transliterationCheckbox.Checked;
        }

        private void _dpofCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            OrderManager.IsDpof.Value = _dpofCheckbox.Checked;
        }
    }
}