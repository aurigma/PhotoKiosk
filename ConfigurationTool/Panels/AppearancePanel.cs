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
    public partial class AppearancePanel : BasePanel
    {
        public AppearancePanel()
        {
            InitializeComponent();
        }

        private void AppearancePanel_Load(object sender, EventArgs e)
        {
            if (Config != null)
            {
                _themeBox.Text = Config.ThemeFile.Value;
                _localizationBox.Text = Config.LocalizationFile.Value;
            }
        }

        private void _themeBox_TextChanged(object sender, EventArgs e)
        {
            Config.ThemeFile.Value = _themeBox.Text;
        }

        private void _themeButton_Click(object sender, EventArgs e)
        {
            _openFileDialog.Filter = "XAML files(*.xaml)|*.xaml";
            _openFileDialog.FileName = _themeBox.Text;

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
                _themeBox.Text = _openFileDialog.FileName;
        }

        private void _themeBox_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(_themeBox.Text) && !File.Exists(_themeBox.Text))
                ShowToolTip(RM.GetString("ThemeFileError"), _themeBox);
        }

        private void _localizationBox_TextChanged(object sender, EventArgs e)
        {
            Config.LocalizationFile.Value = _localizationBox.Text;
        }

        private void _localizationBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(_localizationBox.Text) || !File.Exists(_localizationBox.Text))
                ShowToolTip(RM.GetString("LocalizationFileError"), _localizationBox);
        }

        private void _localizationButton_Click(object sender, EventArgs e)
        {
            _openFileDialog.Filter = "XML files(*.xml)|*.xml";
            _openFileDialog.FileName = _localizationBox.Text;

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
                _localizationBox.Text = _openFileDialog.FileName;
        }
    }
}