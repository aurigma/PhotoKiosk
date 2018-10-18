// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class ContactInfoPanel : BasePanel
    {
        public ContactInfoPanel()
        {
            InitializeComponent();
        }

        private void OrderingPanel_Load(object sender, EventArgs e)
        {
            if (Config != null)
            {
                _userNameOrderIdCheckBox.Checked = Config.EnableCustomerNameOrderId.Value;
                _customIdCheckbox.Checked = Config.EnableCustomerOrderId.Value;
                _emailCheckbox.Checked = Config.EnableCustomerEmail.Value;
                _phoneCheckbox.Checked = Config.EnableCustomerPhone.Value;
            }
        }

        private void _customIdCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableCustomerOrderId.Value = _customIdCheckbox.Checked;
        }

        private void _emailCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableCustomerEmail.Value = _emailCheckbox.Checked;
        }

        private void _phoneCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableCustomerPhone.Value = _phoneCheckbox.Checked;
        }

        private void _userNameOrderIdCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableCustomerNameOrderId.Value = _userNameOrderIdCheckBox.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Config.SkipContactInfo.Value = _skipContactInfoCheckBox.Checked;
        }
    }
}