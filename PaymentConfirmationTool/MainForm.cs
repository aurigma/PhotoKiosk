// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.PaymentConfirmationTool
{
    public partial class MainForm : Form
    {
        private PrintDocument _printableDoc = new PrintDocument();

        public MainForm()
        {
            InitializeComponent();

            _printableDoc.PrintPage += new PrintPageEventHandler(_printableDoc_PrintPage);
        }

        private string CreateActivationCode(string order)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(order + "magic");
            byte[] hash = (new MD5CryptoServiceProvider()).ComputeHash(bytes);
            int sum = BitConverter.ToInt32(hash, 0) + BitConverter.ToInt32(hash, 4) + BitConverter.ToInt32(hash, 8) + BitConverter.ToInt32(hash, 12);

            return sum.ToString("X2");
        }

        private void _orderIdBox_TextChanged(object sender, EventArgs e)
        {
            _generateButton.Enabled = !string.IsNullOrEmpty(_orderIdBox.Text);
        }

        private void _generateButton_Click(object sender, EventArgs e)
        {
            _codeBox.Text = CreateActivationCode(_orderIdBox.Text);
        }

        private void _orderIdBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                _generateButton_Click(null, null);
        }

        private void _codeBox_TextChanged(object sender, EventArgs e)
        {
            _printButton.Enabled = !string.IsNullOrEmpty(_codeBox.Text);
        }

        private void _printButton_Click(object sender, EventArgs e)
        {
            _printDialog.Document = _printableDoc;
            if (_printDialog.ShowDialog() == DialogResult.OK)
                _printableDoc.Print();
        }

        private void _printableDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            string text = _codeBox.Text;
            e.Graphics.DrawString(text, SystemFonts.DefaultFont, SystemBrushes.WindowText, e.MarginBounds);
        }
    }
}