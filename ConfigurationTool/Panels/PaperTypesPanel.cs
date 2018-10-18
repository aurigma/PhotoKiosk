// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class PaperTypesPanel : BasePanel
    {
        private bool _isLoaded = false;

        public PaperTypesPanel()
        {
            InitializeComponent();
        }

        private void PaperTypesPanel_Load(object sender, EventArgs e)
        {
            if (PriceManager != null)
            {
                PriceUpdated();
            }
        }

        protected override void PriceUpdated()
        {
            _isLoaded = false;
            _typesView.Rows.Clear();
            foreach (PaperType type in PriceManager.PaperTypes)
            {
                int index = _typesView.Rows.Add(type.Name, type.Description);
                _typesView.Rows[index].Tag = type;
            }
            _isLoaded = true;
        }

        private void ValidateCells(CancelEventArgs e)
        {
            for (int i = _typesView.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow curRow = _typesView.Rows[i];
                if (!curRow.IsNewRow)
                {
                    curRow.ErrorText = string.Empty;

                    string curName = curRow.Cells["NameColumn"].Value != null ? curRow.Cells["NameColumn"].Value.ToString() : string.Empty;
                    if (!string.IsNullOrEmpty(curName))
                    {
                        foreach (DataGridViewRow row in _typesView.Rows)
                        {
                            if (row != curRow && string.IsNullOrEmpty(row.ErrorText) && curName.Equals(row.Cells["NameColumn"].Value))
                                curRow.ErrorText = RM.GetString("PaperTypeDuplicateError");
                        }
                    }
                    else
                        curRow.ErrorText = RM.GetString("PaperTypeEmptyError");
                }
            }

            if (!BasePanel.HasErrors(_typesView, false))
            {
                if (Parent is MainForm)
                    (Parent as MainForm).UpdatePriceNodes();
            }
            else if (e != null)
                e.Cancel = true;
        }

        private void _typesView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_isLoaded)
            {
                PaperType type = _typesView.Rows[e.RowIndex].Tag as PaperType;
                string newName = _typesView["NameColumn", e.RowIndex].Value != null ? _typesView["NameColumn", e.RowIndex].Value.ToString() : string.Empty;
                string newDescription = _typesView["DescriptionColumn", e.RowIndex].Value != null ? _typesView["DescriptionColumn", e.RowIndex].Value.ToString() : string.Empty;
                if (type != null)
                {
                    type.Name = newName;
                    type.Description = newDescription;
                }
                else
                {
                    type = PriceManager.AddPaperType(newName, newDescription);
                    _typesView.Rows[e.RowIndex].Tag = type;
                }

                ValidateCells(null);
            }
        }

        private void _typesView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            PaperType type = e.Row.Tag as PaperType;
            PriceManager.RemovePaperType(type);

            ValidateCells(null);
        }

        private void _typesView_DragDrop(object sender, DragEventArgs e)
        {
            var newTypes = new PaperType[PriceManager.PaperTypes.Count];
            for (int i = 0; i < _typesView.Rows.Count; i++)
            {
                PaperType type = _typesView.Rows[i].Tag as PaperType;
                if (type != null)
                    newTypes[i] = type;
            }

            PriceManager.UpdatePaperTypes(newTypes);

            ValidateCells(null);
        }

        private void _typesView_Validating(object sender, CancelEventArgs e)
        {
            ValidateCells(e);
        }
    }
}