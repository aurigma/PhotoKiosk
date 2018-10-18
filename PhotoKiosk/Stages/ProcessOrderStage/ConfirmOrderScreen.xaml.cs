// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Windows;

namespace Aurigma.PhotoKiosk
{
    public partial class ConfirmOrderScreen : System.Windows.Controls.Page
    {
        public ConfirmOrderScreen()
        {
            if (ExecutionEngine.Instance != null)
            {
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);
            }

            InitializeComponent();
        }

        private void ProcessOrderScreenLoadHandler(object sender, RoutedEventArgs e)
        {
            _orderInfoControl.SetSource(_stage.CurrentOrder);
            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
            {
                _orderParamsControl.Visibility = Visibility.Collapsed;
                _confirmOrderTitle.TextContent = (string)FindResource(Constants.OrderConfirmCdBurningTextKey);
                _confirmOrderHint.TextContent = (string)FindResource(Constants.OrderConfirmNoticeCdBurningTextKey);
            }
            else
            {
                _orderParamsControl.Visibility = Visibility.Visible;
                _orderParamsControl.Update();
                _confirmOrderTitle.TextContent = (string)FindResource(Constants.OrderConfirmTextKey);
                _confirmOrderHint.TextContent = (string)FindResource(Constants.OrderConfirmNoticeTextKey);
            }
        }

        public ConfirmOrderScreen(Aurigma.PhotoKiosk.ProcessOrderStage stage)
            : this()
        {
            _stage = stage;
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            _stage.SwitchToContactInfo();
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            _stage.SwitchToSelectStage();
        }

        private Aurigma.PhotoKiosk.ProcessOrderStage _stage;
    }
}