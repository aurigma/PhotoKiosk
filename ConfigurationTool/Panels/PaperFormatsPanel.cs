// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class PaperFormatsPanel : BasePanel
    {
        private bool _isLoaded = false;

        private ToolStripMenuItem _contextMenuItem = new ToolStripMenuItem();
        private DataGridViewCellEventArgs _mouseLocation;

        public PaperFormatsPanel()
        {
            InitializeComponent();

            _contextMenuItem.Text = RM.GetString("CopyToCrops");
            _contextMenuItem.Click += new EventHandler(_contextMenuItem_Click);

            _formatsView.ContextMenuStrip = new ContextMenuStrip();
            _formatsView.ContextMenuStrip.Opening += new CancelEventHandler(_contextMenu_DropDownOpened);
            _formatsView.ContextMenuStrip.Items.Add(_contextMenuItem);
        }

        private void _contextMenu_DropDownOpened(object sender, CancelEventArgs args)
        {
            int rowIndex = _mouseLocation.RowIndex;
            if (rowIndex >= 0 && rowIndex < _formatsView.Rows.Count)
            {
                PaperFormat format = _formatsView.Rows[rowIndex].Tag as PaperFormat;
                if (format != null)
                {
                    _contextMenuItem.Enabled = !CropManager.ContainsCrop(format.Name, format.Width, format.Height) && CropManager.CropFormats.Count < Constants.MaxCropsCount;
                }
                else
                    args.Cancel = true;
            }
            else
                args.Cancel = true;
        }

        private void _contextMenuItem_Click(object sender, EventArgs args)
        {
            int rowIndex = _mouseLocation.RowIndex;
            if (rowIndex >= 0 && rowIndex < _formatsView.Rows.Count)
            {
                PaperFormat format = _formatsView.Rows[rowIndex].Tag as PaperFormat;
                if (format != null)
                {
                    CropManager.AddCropFormat(format.Name, format.Width, format.Height, true);
                }
            }
        }

        private void _formatsView_CellMouseEnter(object sender, DataGridViewCellEventArgs location)
        {
            _mouseLocation = location;
        }

        private void PaperFormatsPanel_Load(object sender, EventArgs e)
        {
            if (PriceManager != null)
                PriceUpdated();
        }

        private void PaperFormatsPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (Config != null && Visible)
            {
                _formatsView.Columns["WidthColumn"].HeaderText = string.Format(RM.GetString("WidthColumnHeader"), RM.GetString(Config.PaperSizeUnits.Value));
                _formatsView.Columns["HeightColumn"].HeaderText = string.Format(RM.GetString("HeightColumnHeader"), RM.GetString(Config.PaperSizeUnits.Value));
            }
        }

        protected override void PriceUpdated()
        {
            _isLoaded = false;
            _formatsView.Rows.Clear();
            foreach (PaperFormat format in PriceManager.PaperFormats)
            {
                int index = _formatsView.Rows.Add(format.Name, format.Width, format.Height, format.Dpi);
                _formatsView.Rows[index].Tag = format;
            }
            _isLoaded = true;

            ValidateCells(null);
        }

        private void ValidateCells(CancelEventArgs e)
        {
            for (int i = _formatsView.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow curRow = _formatsView.Rows[i];
                if (!curRow.IsNewRow)
                {
                    curRow.ErrorText = string.Empty;
                    curRow.Cells["WidthColumn"].ErrorText = string.Empty;
                    curRow.Cells["HeightColumn"].ErrorText = string.Empty;
                    curRow.Cells["DpiColumn"].ErrorText = string.Empty;

                    string curName = curRow.Cells["NameColumn"].Value != null ? curRow.Cells["NameColumn"].Value.ToString() : string.Empty;
                    if (!string.IsNullOrEmpty(curName))
                    {
                        foreach (DataGridViewRow row in _formatsView.Rows)
                        {
                            if (row != curRow && string.IsNullOrEmpty(row.ErrorText) && curName.Equals(row.Cells["NameColumn"].Value))
                                curRow.ErrorText = RM.GetString("PaperFormatDuplicateError");
                        }
                    }
                    else
                        curRow.ErrorText = RM.GetString("PaperFormatEmptyError");

                    float width = 0.0f;
                    if (curRow.Cells["WidthColumn"].Value == null || !float.TryParse(curRow.Cells["WidthColumn"].Value.ToString(), out width) || width < 0)
                        curRow.Cells["WidthColumn"].ErrorText = RM.GetString("PaperWidthError");

                    float height = 0.0f;
                    if (curRow.Cells["HeightColumn"].Value == null || !float.TryParse(curRow.Cells["HeightColumn"].Value.ToString(), out height) || height < 0)
                        curRow.Cells["HeightColumn"].ErrorText = RM.GetString("PaperHeightError");

                    int dpi = 0;
                    if (curRow.Cells["DpiColumn"].Value == null || !int.TryParse(curRow.Cells["DpiColumn"].Value.ToString(), out dpi) || dpi < 0)
                        curRow.Cells["DpiColumn"].ErrorText = RM.GetString("PaperDpiError");

                    if ((width == 0 || height == 0 || dpi == 0) && (width > 0 || height > 0 || dpi > 0))
                        curRow.ErrorText = RM.GetString("FreeFormatError");
                }
            }

            if (!BasePanel.HasErrors(_formatsView, false))
            {
                if (Parent is MainForm)
                    (Parent as MainForm).UpdatePriceNodes();
            }
            else if (e != null)
                e.Cancel = true;
        }

        private void _formatsView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_isLoaded && e.RowIndex >= 0)
            {
                PaperFormat format = _formatsView.Rows[e.RowIndex].Tag as PaperFormat;
                string newName = _formatsView["NameColumn", e.RowIndex].Value != null ? _formatsView["NameColumn", e.RowIndex].Value.ToString() : string.Empty;

                float newWidth = 0.0f;
                if (_formatsView["WidthColumn", e.RowIndex].Value != null)
                    float.TryParse(_formatsView["WidthColumn", e.RowIndex].Value.ToString(), out newWidth);

                float newHeight = 0.0f;
                if (_formatsView["HeightColumn", e.RowIndex].Value != null)
                    float.TryParse(_formatsView["HeightColumn", e.RowIndex].Value.ToString(), out newHeight);

                int newDpi = 0;
                if (_formatsView["DpiColumn", e.RowIndex].Value != null)
                    int.TryParse(_formatsView["DpiColumn", e.RowIndex].Value.ToString(), out newDpi);

                if (format != null)
                {
                    format.Name = newName;
                    format.Width = newWidth;
                    format.Height = newHeight;
                    format.Dpi = newDpi;
                }
                else
                {
                    format = PriceManager.AddPaperFormat(newName, newWidth, newHeight, newDpi);
                    _formatsView.Rows[e.RowIndex].Tag = format;
                }

                ValidateCells(null);
            }
        }

        private void _formatsView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            PaperFormat format = e.Row.Tag as PaperFormat;
            PriceManager.RemovePaperFormat(format);

            ValidateCells(null);
        }

        private void _formatsView_DragDrop(object sender, DragEventArgs e)
        {
            var newFormats = new PaperFormat[PriceManager.PaperFormats.Count];
            for (int i = 0; i < _formatsView.Rows.Count; i++)
            {
                PaperFormat type = _formatsView.Rows[i].Tag as PaperFormat;
                if (type != null)
                    newFormats[i] = type;
            }

            PriceManager.UpdatePaperFormats(newFormats);

            ValidateCells(null);
        }

        private void _formatsView_Validating(object sender, CancelEventArgs e)
        {
            ValidateCells(e);
        }

        private void _formatsView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            (e.Control as TextBox).KeyPress += cellEdit_KeyPress;
        }

        private void cellEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_formatsView.CurrentCell.OwningColumn.Name == "WidthColumn" || _formatsView.CurrentCell.OwningColumn.Name == "HeightColumn")
            {
                e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && !e.KeyChar.ToString().Equals(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            }
            else if (_formatsView.CurrentCell.OwningColumn.Name == "DpiColumn")
            {
                e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != '\b';
            }
        }
    }
}