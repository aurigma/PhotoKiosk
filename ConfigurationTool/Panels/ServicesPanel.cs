// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class ServicesPanel : BasePanel
    {
        private bool _isLoaded = false;

        public ServicesPanel()
        {
            InitializeComponent();
        }

        private void ServicesPanel_Load(object sender, EventArgs e)
        {
            if (PriceManager != null)
                PriceUpdated();
        }

        protected override void PriceUpdated()
        {
            _isLoaded = false;
            _servicesView.Rows.Clear();
            foreach (Service service in PriceManager.Services)
            {
                int index = _servicesView.Rows.Add(service.Name);
                _servicesView.Rows[index].Tag = service;
            }
            _isLoaded = true;

            ValidateCells(null);
        }

        private void ValidateCells(CancelEventArgs e)
        {
            for (int i = _servicesView.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow curRow = _servicesView.Rows[i];
                if (!curRow.IsNewRow)
                {
                    curRow.ErrorText = string.Empty;

                    string curName = curRow.Cells["ServiceNameColumn"].Value != null ? curRow.Cells["ServiceNameColumn"].Value.ToString() : string.Empty;
                    if (!string.IsNullOrEmpty(curName))
                    {
                        foreach (DataGridViewRow row in _servicesView.Rows)
                        {
                            if (row != curRow && string.IsNullOrEmpty(row.ErrorText) && curName.Equals(row.Cells["ServiceNameColumn"].Value))
                                curRow.ErrorText = RM.GetString("ServiceNameDuplicateError");
                        }
                    }
                    else
                        curRow.ErrorText = RM.GetString("ServiceNameEmptyError");
                }
            }

            if (!BasePanel.HasErrors(_servicesView, false))
            {
                if (Parent is MainForm)
                    (Parent as MainForm).UpdateServiceNodes();
            }
            else if (e != null)
                e.Cancel = true;
        }

        private void _servicesView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_isLoaded && e.ColumnIndex == 0)
            {
                Service service = _servicesView.Rows[e.RowIndex].Tag as Service;
                string newName = _servicesView["ServiceNameColumn", e.RowIndex].Value != null ? _servicesView["ServiceNameColumn", e.RowIndex].Value.ToString() : string.Empty;
                if (service != null)
                {
                    service.Name = newName;
                }
                else
                {
                    service = PriceManager.AddService(newName);
                    _servicesView.Rows[e.RowIndex].Tag = service;
                }

                ValidateCells(null);
            }
        }

        private void _servicesView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            Service service = e.Row.Tag as Service;
            PriceManager.RemoveService(service);

            ValidateCells(null);
        }

        private void _servicesView_DragDrop(object sender, DragEventArgs e)
        {
            var newServices = new Service[PriceManager.Services.Count];
            for (int i = 0; i < _servicesView.Rows.Count; i++)
            {
                Service service = _servicesView.Rows[i].Tag as Service;
                if (service != null)
                    newServices[i] = service;
            }

            PriceManager.UpdateServices(newServices);

            ValidateCells(null);
        }

        private void _servicesView_Validating(object sender, CancelEventArgs e)
        {
            ValidateCells(e);
        }
    }
}