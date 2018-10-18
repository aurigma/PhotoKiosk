// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Globalization;
using System.Windows;

namespace Aurigma.PhotoKiosk
{
    public partial class ThankYouScreen : System.Windows.Controls.Page
    {
        public ThankYouScreen()
        {
            if (ExecutionEngine.Instance != null)
            {
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);
            }

            InitializeComponent();
        }

        public ThankYouScreen(Aurigma.PhotoKiosk.ProcessOrderStage stage) : this()
        {
            _stage = stage;
        }

        private void ThankYouScreen_Loaded(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.ProcessOrder)
            {
                _orderId.TextContent = Parser.CreateFormatString(
                        (string)TryFindResource(Constants.ThankYouScreenOrderIdKey),
                        new string[2]
                        {
                            _stage.CurrentOrder.OrderId.ToString(CultureInfo.InvariantCulture),
                            _stage.CurrentOrder.UserName.ToString(CultureInfo.InvariantCulture),
                        }
                        );

                if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd && ExecutionEngine.Config.CDBurningRequireConfirm.Value)
                {
                    _paymentInfo.Visibility = Visibility.Visible;
                    _paymentInfo.TextContent = ExecutionEngine.Config.CDBurningPaymentInstructions.Value;
                }
                else if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.PrintPhotos && ExecutionEngine.Config.PhotoPrintingRequireConfirm.Value)
                {
                    _paymentInfo.Visibility = Visibility.Visible;
                    _paymentInfo.TextContent = ExecutionEngine.Config.PhotoPrintingPaymentInstructions.Value;
                }
                else
                    _paymentInfo.Visibility = Visibility.Collapsed;
            }
            else
            {
                _hint.Visibility = Visibility.Hidden;
                _orderId.TextContent = "";
                _paymentInfo.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonNextClick(object sender, RoutedEventArgs e)
        {
            _stage.SwitchToStartStage();
        }

        private Aurigma.PhotoKiosk.ProcessOrderStage _stage;
    }
}