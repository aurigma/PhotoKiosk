// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class BluetoothPanel : BasePanel
    {
        public BluetoothPanel()
        {
            InitializeComponent();
        }

        private void BluetoothPanel_Load(object sender, EventArgs e)
        {
            if (Config != null)
            {
                _bluetoothCheckbox.Checked = Config.EnableBluetooth.Value;
                _bluetoothHostBox.Text = Config.BluetoothHost.Value;
                _bluetoothFolderBox.Text = Config.BluetoothFolder.Value;

                UpdateControlsState();
            }
        }

        private void UpdateControlsState()
        {
            _bluetoothHostLabel.Enabled = Config.EnableBluetooth.Value;
            _bluetoothHostBox.Enabled = Config.EnableBluetooth.Value;
            _bluetoothFolderLabel.Enabled = Config.EnableBluetooth.Value;
            _bluetoothFolderBox.Enabled = Config.EnableBluetooth.Value;
            _bluetoothFolderButton.Enabled = Config.EnableBluetooth.Value;
        }

        private void _bluetoothCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableBluetooth.Value = _bluetoothCheckbox.Checked;
            UpdateControlsState();

            if (Config.EnableBluetooth.Value)
            {
                _bluetoothHostBox_Validating(_bluetoothHostBox, new CancelEventArgs());
                _bluetoothFolderBox_Validating(_bluetoothFolderBox, new CancelEventArgs());
            }
        }

        private void _bluetoothHostBox_TextChanged(object sender, EventArgs e)
        {
            Config.BluetoothHost.Value = _bluetoothHostBox.Text;
        }

        private void _bluetoothHostBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(_bluetoothHostBox.Text))
                ShowToolTip(RM.GetString("BluetoothHostError"), _bluetoothHostBox);
        }

        private void _bluetoothFolderBox_TextChanged(object sender, EventArgs e)
        {
            Config.BluetoothFolder.Value = _bluetoothFolderBox.Text;
        }

        private void _bluetoothFolderBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(_bluetoothFolderBox.Text) || !Directory.Exists(_bluetoothFolderBox.Text))
                ShowToolTip(RM.GetString("BluetoothFolderError"), _bluetoothFolderBox);
        }

        private void _bluetoothFolderButton_Click(object sender, EventArgs e)
        {
            _folderBrowserDialog.SelectedPath = _bluetoothFolderBox.Text;

            if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
                _bluetoothFolderBox.Text = _folderBrowserDialog.SelectedPath;
        }
    }
}