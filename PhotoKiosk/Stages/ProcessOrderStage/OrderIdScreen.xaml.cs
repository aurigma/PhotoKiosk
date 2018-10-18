// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using Aurigma.PhotoKiosk.Core.OrderManager;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aurigma.PhotoKiosk
{
    public partial class OrderIdScreen : Page
    {
        public OrderIdScreen()
        {
            if (ExecutionEngine.Instance != null)
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);

            InitializeComponent();
        }

        public OrderIdScreen(ProcessOrderStage stage)
            : this()
        {
            _stage = stage;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.ProcessOrder)
            {
                if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
                    _orderIdHeader.TextContent = (string)ExecutionEngine.Instance.Resource[Constants.OrderIdTextKey];
                else
                    _orderIdHeader.TextContent = (string)ExecutionEngine.Instance.Resource[Constants.OrderIdCdBurningTextKey];
                _orderIdHint.TextContent = (string)ExecutionEngine.Instance.Resource[Constants.OrderIdNoticeTextKey];
                _enteredCode.Visibility = Visibility.Collapsed;
            }
            else
            {
                _orderIdHeader.TextContent = (string)ExecutionEngine.Instance.Resource[Constants.OrderIdProcessOrderTextKey];
                _orderIdHint.TextContent = (string)ExecutionEngine.Instance.Resource[Constants.OrderIdProcessOrderNoticeTextKey];
                _enteredCode.Visibility = Visibility.Visible;
                _enteredCode.Text = "";
            }
        }

        private void ButtonNextStageClickHandler(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_enteredId.Text))
            {
                MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageEnterYourOrderIdKey]);
                return;
            }

            if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.ProcessOrder)
            {
                _stage.CurrentOrder.OrderId = _enteredId.Text;
                _stage.CompleteOrder();
            }
            else
            {
                if (string.IsNullOrEmpty(_enteredCode.Text))
                {
                    MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageEnterYourActivationCodeKey]);
                    return;
                }

                DelayedOrder order = DelayedOrder.GetOrder(_enteredId.Text);
                if (order != null && order.Code.Equals(_enteredCode.Text, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    if (order.Action.Equals(PrimaryActionType.BurnCd.ToString()))
                        _stage.SwitchToBurningScreen(order.Folder);
                    else if (order.Action.Equals(PrimaryActionType.PrintPhotos.ToString()))
                        _stage.SwitchToPrintingScreen(order.Folder);
                }
                else
                    MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageActivationCodeIncorrectKey]);
            }
        }

        private void ButtonPrevStageClickHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.ProcessOrder)
                _stage.SwitchToContactInfo();
            else
                _stage.SwitchToStartStage();
        }

        private void KeyboardControlsLoaded(object sender, RoutedEventArgs e)
        {
            _screenKeyboard.BindedControl = _enteredId;
            _screenKeyboard.Layout = (string)TryFindResource(Constants.KeyboardLayoutKey);
            _screenKeyboard.ShiftedLayout = (string)TryFindResource(Constants.KeyboardShiftedLayoutKey);
        }

        private void TextBlockClickHandler(object sender, MouseButtonEventArgs e)
        {
            _screenKeyboard.BindedControl = (FocusableTextBlock)sender;
        }

        private ProcessOrderStage _stage;
    }
}