// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class ImageEditorPanel : BasePanel
    {
        public ImageEditorPanel()
        {
            InitializeComponent();
        }

        private void ImageEditorPanel_Load(object sender, EventArgs e)
        {
            if (Config != null)
            {
                _rotationCheckbox.Checked = Config.EnableRotation.Value;
                _flipCheckbox.Checked = Config.EnableFlip.Value;
                _cropCheckbox.Checked = Config.EnableCrop.Value;
                _colorCheckbox.Checked = Config.EnableColorCorrection.Value;
                _effectsCheckbox.Checked = Config.EnableEffects.Value;
                _redEyeCheckbox.Checked = Config.EnableRedEyeRemoval.Value;
                _cropFileBox.Text = Config.CropFile.Value;

                UpdateImageEditorCheckboxesState();
            }
        }

        private void UpdateImageEditorCheckboxesState()
        {
            _imageEditorCheckbox.Checked = Config.IsImageEditorEnabled() || Config.EnableImageEditor.Value;
            _rotationCheckbox.Enabled = Config.IsImageEditorEnabled() || Config.EnableImageEditor.Value;
            _flipCheckbox.Enabled = Config.IsImageEditorEnabled() || Config.EnableImageEditor.Value;
            _cropCheckbox.Enabled = Config.IsImageEditorEnabled() || Config.EnableImageEditor.Value;
            _colorCheckbox.Enabled = Config.IsImageEditorEnabled() || Config.EnableImageEditor.Value;
            _effectsCheckbox.Enabled = Config.IsImageEditorEnabled() || Config.EnableImageEditor.Value;
            _redEyeCheckbox.Enabled = Config.IsImageEditorEnabled() || Config.EnableImageEditor.Value;
            _cropFileBox.Enabled = Config.IsImageEditorEnabled() || Config.EnableImageEditor.Value;
        }

        private void _imageEditorCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableImageEditor.Value = _imageEditorCheckbox.Checked;
            UpdateImageEditorCheckboxesState();
        }

        private void _rotationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableRotation.Value = _rotationCheckbox.Checked;
            UpdateImageEditorCheckboxesState();
        }

        private void _flipCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableFlip.Value = _flipCheckbox.Checked;
            UpdateImageEditorCheckboxesState();
        }

        private void _cropCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableCrop.Value = _cropCheckbox.Checked;
            UpdateImageEditorCheckboxesState();
        }

        private void _colorCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableColorCorrection.Value = _colorCheckbox.Checked;
            UpdateImageEditorCheckboxesState();
        }

        private void _effectsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableEffects.Value = _effectsCheckbox.Checked;
            UpdateImageEditorCheckboxesState();
        }

        private void _redEyeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableRedEyeRemoval.Value = _redEyeCheckbox.Checked;
            UpdateImageEditorCheckboxesState();
        }

        private void _cropFileBox_TextChanged(object sender, EventArgs e)
        {
            Config.CropFile.Value = _cropFileBox.Text;
            CropManager.FileName = _cropFileBox.Text;
        }

        private void _cropFileBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(_cropFileBox.Text) || !File.Exists(_cropFileBox.Text))
                ShowToolTip(RM.GetString("CropFileError"), _cropFileBox);
        }

        private void _cropFileButton_Click(object sender, EventArgs e)
        {
            _openFileDialog.FileName = _cropFileBox.Text;

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
                _cropFileBox.Text = _openFileDialog.FileName;
        }
    }
}