// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public partial class OrderParamsControl : UserControl
    {
        public OrderParamsControl()
        {
            InitializeComponent();

            if (ExecutionEngine.Context.Contains(Constants.OrderContextName))
                _currentOrder = (Order)ExecutionEngine.Context[Constants.OrderContextName];

            Update();
        }

        public void Update()
        {
            _paperTypeLabel.Text = _currentOrder.OrderPaperType.Name;
            _cropModeLabel.Text = Constants.CropToFillModeName == _currentOrder.CropMode ?
                ExecutionEngine.Instance.Resource[Constants.CropToFillTextKey].ToString() :
                ExecutionEngine.Instance.Resource[Constants.ResizeToFitTextKey].ToString();
        }

        private Order _currentOrder;
    }
}