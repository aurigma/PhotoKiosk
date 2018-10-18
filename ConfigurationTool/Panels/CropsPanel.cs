// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool.Panels
{
    public partial class CropsPanel : BasePanel
    {
        private bool _isLoaded = false;

        public CropsPanel()
        {
            InitializeComponent();
        }

        private void CropsPanel_Load(object sender, EventArgs e)
        {
            if (CropManager != null)
                CropsUpdated();
        }

        private void CropsPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (Config != null && Visible)
            {
                _cropsView.Enabled = Config.IsImageEditorEnabled();

                _cropsView.Columns["WidthColumn"].HeaderText = string.Format(RM.GetString("WidthColumnHeader"), RM.GetString(Config.PaperSizeUnits.Value));
                _cropsView.Columns["HeightColumn"].HeaderText = string.Format(RM.GetString("HeightColumnHeader"), RM.GetString(Config.PaperSizeUnits.Value));
            }
        }

        protected override void CropsUpdated()
        {
            _isLoaded = false;
            _cropsView.Rows.Clear();
            foreach (CropFormat format in CropManager.CropFormats)
            {
                int index = _cropsView.Rows.Add(format.Name, format.Width, format.Height);
                _cropsView.Rows[index].Tag = format;
            }
            _cropsView.AllowUserToAddRows = _cropsView.Rows.Count <= Constants.MaxCropsCount;
            _isLoaded = true;
        }

        private void ValidateCells(CancelEventArgs e)
        {
            for (int i = _cropsView.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow curRow = _cropsView.Rows[i];
                if (!curRow.IsNewRow)
                {
                    curRow.ErrorText = string.Empty;
                    curRow.Cells["WidthColumn"].ErrorText = string.Empty;
                    curRow.Cells["HeightColumn"].ErrorText = string.Empty;

                    string curName = curRow.Cells["NameColumn"].Value != null ? curRow.Cells["NameColumn"].Value.ToString() : string.Empty;
                    if (!string.IsNullOrEmpty(curName))
                    {
                        foreach (DataGridViewRow row in _cropsView.Rows)
                        {
                            if (row != curRow && string.IsNullOrEmpty(row.ErrorText) && curName.Equals(row.Cells["NameColumn"].Value))
                                curRow.ErrorText = RM.GetString("CropFormatDuplicateError");
                        }
                    }
                    else
                        curRow.ErrorText = RM.GetString("CropFormatEmptyError");

                    float width = 0.0f;
                    if (curRow.Cells["WidthColumn"].Value == null || !float.TryParse(curRow.Cells["WidthColumn"].Value.ToString(), out width) || width < 0)
                        curRow.Cells["WidthColumn"].ErrorText = RM.GetString("CropWidthError");

                    float height = 0.0f;
                    if (curRow.Cells["HeightColumn"].Value == null || !float.TryParse(curRow.Cells["HeightColumn"].Value.ToString(), out height) || height < 0)
                        curRow.Cells["HeightColumn"].ErrorText = RM.GetString("CropHeightError");

                    if ((width == 0 && height > 0) || (width > 0 && height == 0))
                        curRow.ErrorText = RM.GetString("FreeCropError");
                }
            }

            if (BasePanel.HasErrors(_cropsView, false) && e != null)
                e.Cancel = true;
        }

        private void _cropsView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_isLoaded && e.RowIndex >= 0)
            {
                CropFormat format = _cropsView.Rows[e.RowIndex].Tag as CropFormat;
                string newName = _cropsView["NameColumn", e.RowIndex].Value != null ? _cropsView["NameColumn", e.RowIndex].Value.ToString() : string.Empty;

                float newWidth = 0.0f;
                if (_cropsView["WidthColumn", e.RowIndex].Value != null)
                    float.TryParse(_cropsView["WidthColumn", e.RowIndex].Value.ToString(), out newWidth);

                float newHeight = 0.0f;
                if (_cropsView["HeightColumn", e.RowIndex].Value != null)
                    float.TryParse(_cropsView["HeightColumn", e.RowIndex].Value.ToString(), out newHeight);

                if (format != null)
                {
                    format.Name = newName;
                    format.Width = newWidth;
                    format.Height = newHeight;
                }
                else
                {
                    format = CropManager.AddCropFormat(newName, newWidth, newHeight, false);
                    _cropsView.Rows[e.RowIndex].Tag = format;
                }

                ValidateCells(null);
            }
        }

        private void _cropsView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            CropFormat format = e.Row.Tag as CropFormat;
            CropManager.RemoveCropFormat(format);

            ValidateCells(null);

            _cropsView.AllowUserToAddRows = _cropsView.Rows.Count <= Constants.MaxCropsCount;
        }

        private void _cropsView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            _cropsView.AllowUserToAddRows = _cropsView.Rows.Count <= Constants.MaxCropsCount;
        }

        private void _cropsView_DragDrop(object sender, DragEventArgs e)
        {
            var newFormats = new CropFormat[CropManager.CropFormats.Count];
            for (int i = 0; i < _cropsView.Rows.Count; i++)
            {
                CropFormat type = _cropsView.Rows[i].Tag as CropFormat;
                if (type != null)
                    newFormats[i] = type;
            }

            CropManager.UpdateCropFormats(newFormats);

            ValidateCells(null);
        }

        private void _cropsView_Validating(object sender, CancelEventArgs e)
        {
            ValidateCells(e);
        }

        private void _cropsView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            (e.Control as TextBox).KeyPress += cellEdit_KeyPress;
        }

        private void cellEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_cropsView.CurrentCell.OwningColumn.Name == "WidthColumn" || _cropsView.CurrentCell.OwningColumn.Name == "HeightColumn")
            {
                e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && !e.KeyChar.ToString().Equals(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            }
        }
    }
}