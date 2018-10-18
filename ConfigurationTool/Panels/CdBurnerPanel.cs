// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class CdBurnerPanel : BasePanel
    {
        public CdBurnerPanel()
        {
            InitializeComponent();

            var drives = new List<string>();
            foreach (string drive in Directory.GetLogicalDrives())
            {
                var driveInfo = new DriveInfo(drive);
                if (driveInfo.DriveType == DriveType.CDRom)
                    drives.Add(drive);
            }

            _driveBox.Items.AddRange(drives.ToArray());
        }

        private void CdBurnerPanel_Load(object sender, EventArgs e)
        {
            if (Config != null)
            {
                _cdBurnerCheckbox.Checked = Config.EnableCDBurning.Value;
                if (_driveBox.Items.Contains(Config.DriveLetter.Value))
                    _driveBox.SelectedIndex = _driveBox.Items.IndexOf(Config.DriveLetter.Value);
                else if (string.IsNullOrEmpty(Config.DriveLetter.Value) && _driveBox.Items.Count > 0)
                    _driveBox.SelectedIndex = 0;

                _discLabelBox.Text = Config.DiscLabel.Value;
                _priceBox.FloatValue = Config.CDBurningCost.Value;
                _confirmCheckbox.Checked = Config.CDBurningRequireConfirm.Value;
                _paymentInfoBox.Text = Config.CDBurningPaymentInstructions.Value;

                UpdateControlsState();
            }
        }

        private void UpdateControlsState()
        {
            _driveLabel.Enabled = Config.EnableCDBurning.Value;
            _driveBox.Enabled = Config.EnableCDBurning.Value;
            _discLabel.Enabled = Config.EnableCDBurning.Value;
            _discLabelBox.Enabled = Config.EnableCDBurning.Value;
            _priceLabel.Enabled = Config.EnableCDBurning.Value;
            _priceBox.Enabled = Config.EnableCDBurning.Value;
            _confirmCheckbox.Enabled = Config.EnableCDBurning.Value;

            _paymentInfoLabel.Enabled = Config.CDBurningRequireConfirm.Value;
            _paymentInfoBox.Enabled = Config.CDBurningRequireConfirm.Value;
        }

        private void _cdBurnerCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableCDBurning.Value = _cdBurnerCheckbox.Checked;
            UpdateControlsState();
        }

        private void _driveBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.DriveLetter.Value = _driveBox.SelectedItem.ToString();
        }

        private void _discLabelBox_TextChanged(object sender, EventArgs e)
        {
            Config.DiscLabel.Value = _discLabelBox.Text;
        }

        private void _discLabelBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(_discLabelBox.Text))
            {
                ShowToolTip(RM.GetString("DiscLabelError"), _discLabelBox);
                e.Cancel = true;
            }
        }

        private void _priceBox_TextChanged(object sender, EventArgs e)
        {
            Config.CDBurningCost.Value = _priceBox.FloatValue;
        }

        private void _priceBox_Validating(object sender, CancelEventArgs e)
        {
            if (_priceBox.FloatValue <= 0.0f)
            {
                ShowToolTip(RM.GetString("PriceError"), _priceBox);
                e.Cancel = true;
            }
        }

        private void _confirmCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.CDBurningRequireConfirm.Value = _confirmCheckbox.Checked;
            UpdateControlsState();
        }

        private void _paymentInfoBox_TextChanged(object sender, EventArgs e)
        {
            Config.CDBurningPaymentInstructions.Value = _paymentInfoBox.Text;
        }
    }
}