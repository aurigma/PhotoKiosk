// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class ScreenPanel : BasePanel
    {
        private ScreenSetting _setting;

        public ScreenPanel()
        {
            InitializeComponent();
        }

        public void SetScreen(ScreenSetting setting)
        {
            _setting = setting;

            _headerBox.Text = _setting.Header.Value;
            _backgroundBox.Text = _setting.Background.Value;

            _bannerLabel.Visible = IsWelcomeScreen();
            _bannerBox.Visible = IsWelcomeScreen();
            _bannerButton.Visible = IsWelcomeScreen();

            if (IsWelcomeScreen())
                _bannerBox.Text = Config.MainBanner.Value;
        }

        private bool IsWelcomeScreen()
        {
            return _setting.Key == "WelcomeScreen";
        }

        private void _headerBox_TextChanged(object sender, EventArgs e)
        {
            _setting.Header.Value = _headerBox.Text;
        }

        private void _headerButton_Click(object sender, EventArgs e)
        {
            _openFileDialog.FileName = _headerBox.Text;

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
                _headerBox.Text = _openFileDialog.FileName;
        }

        private void _backgroundBox_TextChanged(object sender, EventArgs e)
        {
            _setting.Background.Value = _backgroundBox.Text;
        }

        private void _backgroundButton_Click(object sender, EventArgs e)
        {
            _openFileDialog.FileName = _backgroundBox.Text;

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
                _backgroundBox.Text = _openFileDialog.FileName;
        }

        private void _bannerBox_TextChanged(object sender, EventArgs e)
        {
            if (IsWelcomeScreen())
                Config.MainBanner.Value = _bannerBox.Text;
        }

        private void _bannerButton_Click(object sender, EventArgs e)
        {
            if (IsWelcomeScreen())
            {
                _openFileDialog.FileName = _bannerBox.Text;

                if (_openFileDialog.ShowDialog() == DialogResult.OK)
                    _bannerBox.Text = _openFileDialog.FileName;
            }
        }

        private void _box_Validating(object sender, CancelEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (!string.IsNullOrEmpty(box.Text) && !File.Exists(box.Text))
                ShowToolTip(RM.GetString("ScreenImageError"), box);
        }
    }
}