// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class CallbackPanel : BasePanel
    {
        public CallbackPanel()
        {
            InitializeComponent();
        }

        private void CallbackPanel_Load(object sender, EventArgs e)
        {
            if (Config != null)
            {
                _startBox.Text = Config.OnStartCallback.Value;
                _completeBox.Text = Config.OnCompleteCallback.Value;
                _cancelBox.Text = Config.OnCancelCallback.Value;
            }
        }

        private void _startBox_TextChanged(object sender, EventArgs e)
        {
            Config.OnStartCallback.Value = _startBox.Text;
        }

        private void _startButton_Click(object sender, EventArgs e)
        {
            _openFileDialog.FileName = _startBox.Text;

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
                _startBox.Text = _openFileDialog.FileName;
        }

        private void _completeBox_TextChanged(object sender, EventArgs e)
        {
            Config.OnCompleteCallback.Value = _completeBox.Text;
        }

        private void _completeButton_Click(object sender, EventArgs e)
        {
            _openFileDialog.FileName = _completeBox.Text;

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
                _completeBox.Text = _openFileDialog.FileName;
        }

        private void _cancelBox_TextChanged(object sender, EventArgs e)
        {
            Config.OnCancelCallback.Value = _cancelBox.Text;
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            _openFileDialog.FileName = _cancelBox.Text;

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
                _cancelBox.Text = _openFileDialog.FileName;
        }

        private void _box_Validating(object sender, CancelEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (!string.IsNullOrEmpty(box.Text) && !File.Exists(box.Text))
                ShowToolTip(RM.GetString("CallbackFileError"), box);
        }
    }
}