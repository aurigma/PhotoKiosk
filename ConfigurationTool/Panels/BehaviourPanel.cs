// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class BehaviourPanel : BasePanel
    {
        private static Dictionary<string, string> _units = new Dictionary<string, string>(2);

        public BehaviourPanel()
        {
            InitializeComponent();
        }

        private void BehaviourPanel_Load(object sender, EventArgs e)
        {
            if (Config != null)
            {
                _units.Add(Constants.MmUnits, RM.GetString("UnitsComboBoxMm"));
                _units.Add(Constants.InchUnits, RM.GetString("UnitsComboBoxInch"));

                foreach (string value in _units.Values)
                    _unitsBox.Items.Add(value);

                // Settings GroupBox
                _photoKioskIdBox.Text = Config.PhotoKioskId.Value;
                _cacheSizeBox.IntValue = Config.MemoryCacheSize.Value;
                _searchTimeoutBox.IntValue = Config.SearchProcessDuration.Value;
                _timeoutBox.IntValue = Config.InactivityTimeout.Value;
                _eventLoggingCheckbox.Checked = Config.EnableEventLogging.Value;
                _unitsBox.SelectedItem = _units[Config.PaperSizeUnits.Value];

                // Paths GroupBox
                _sourcePathsBox.Text = Config.SourcePaths.Value;
                _folderSelectionCheckbox.Checked = Config.EnableFolderSelection.Value;
            }
        }

        #region Settings GroupBox

        private void _photoKioskIdBox_TextChanged(object sender, EventArgs e)
        {
            Config.PhotoKioskId.Value = _photoKioskIdBox.Text;
        }

        private void _eventLoggingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableEventLogging.Value = _eventLoggingCheckbox.Checked;
        }

        private void _cacheSizeBox_TextChanged(object sender, EventArgs e)
        {
            Config.MemoryCacheSize.Value = _cacheSizeBox.IntValue;
        }

        private void _cacheSizeBox_Validating(object sender, CancelEventArgs e)
        {
            if (_cacheSizeBox.IntValue == 0)
            {
                ShowToolTip(RM.GetString("MemoryCacheSizeError"), _cacheSizeBox);
                e.Cancel = true;
            }
        }

        private void _searchTimeoutBox_TextChanged(object sender, EventArgs e)
        {
            Config.SearchProcessDuration.Value = _searchTimeoutBox.IntValue;
        }

        private void _searchTimeoutBox_Validating(object sender, CancelEventArgs e)
        {
            if (_searchTimeoutBox.IntValue == 0)
            {
                ShowToolTip(RM.GetString("SearchProcessDurationError"), _searchTimeoutBox);
                e.Cancel = true;
            }
        }

        private void _timeoutBox_TextChanged(object sender, EventArgs e)
        {
            Config.InactivityTimeout.Value = _timeoutBox.IntValue;
        }

        private void _timeoutBox_Validating(object sender, CancelEventArgs e)
        {
            if (_timeoutBox.IntValue == 0)
            {
                ShowToolTip(RM.GetString("InactivityTimeoutError"), _timeoutBox);
                e.Cancel = true;
            }
        }

        private void _unitsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (string key in _units.Keys)
            {
                if (_unitsBox.SelectedItem.Equals(_units[key]))
                {
                    Config.PaperSizeUnits.Value = key;
                    break;
                }
            }
        }

        #endregion Settings GroupBox

        #region Paths GroupBox

        private void _sourcePathsBox_TextChanged(object sender, EventArgs e)
        {
            Config.SourcePaths.Value = _sourcePathsBox.Text;
        }

        private void _sourcePathsButton_Click(object sender, EventArgs e)
        {
            _folderBrowserDialog.SelectedPath = _sourcePathsBox.Text;

            if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                if (_sourcePathsBox.Text != "")
                    _sourcePathsBox.Text += ";";
                _sourcePathsBox.Text += _folderBrowserDialog.SelectedPath;
            }
        }

        private void _folderSelectionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableFolderSelection.Value = _folderSelectionCheckbox.Checked;
        }

        #endregion Paths GroupBox
    }
}