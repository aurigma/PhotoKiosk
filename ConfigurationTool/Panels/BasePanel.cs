// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using Aurigma.PhotoKiosk.Core.OrderManager;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class BasePanel : UserControl
    {
        public Config Config;
        public OrderManager OrderManager;

        private PriceManager _price;
        private CropManager _crop;

        public BasePanel()
        {
            InitializeComponent();

            _toolTip.ToolTipTitle = RM.GetString("Warning");
        }

        public PriceManager PriceManager
        {
            get
            {
                return _price;
            }
            set
            {
                if (value != null)
                {
                    _price = value;
                    _price.Updated += new PriceManager.UpdatedHandler(PriceUpdated);
                }
            }
        }

        public CropManager CropManager
        {
            get
            {
                return _crop;
            }
            set
            {
                if (value != null)
                {
                    _crop = value;
                    _crop.Updated += new CropManager.UpdatedHandler(CropsUpdated);
                }
            }
        }

        virtual protected void PriceUpdated()
        {
        }

        virtual protected void CropsUpdated()
        {
        }

        public void ShowToolTip(string text, Control control)
        {
            _toolTip.Show(text, control, 3000);
        }

        public static bool HasErrors(DataGridView grid, bool searchCells)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (!string.IsNullOrEmpty(row.ErrorText))
                    return true;

                if (searchCells)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (!string.IsNullOrEmpty(cell.ErrorText))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}