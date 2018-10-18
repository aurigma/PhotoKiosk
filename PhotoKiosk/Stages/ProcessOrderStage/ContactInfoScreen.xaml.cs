// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aurigma.PhotoKiosk
{
    public partial class ContactInfoScreen : Page
    {
        public ContactInfoScreen()
        {
            if (ExecutionEngine.Instance != null)
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);

            InitializeComponent();

            _enteredPhone.Visibility = (ExecutionEngine.Config.EnableCustomerPhone.Value ? Visibility.Visible : Visibility.Collapsed);
            _enteredEmail.Visibility = (ExecutionEngine.Config.EnableCustomerEmail.Value ? Visibility.Visible : Visibility.Collapsed);
        }

        public ContactInfoScreen(ProcessOrderStage stage)
            : this()
        {
            _stage = stage;
        }

        private void ContactInfoScreenLoadedHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
                _contactInfoHeader.TextContent = (string)FindResource(Constants.ContactInfoCdBurningTextKey);
            else
                _contactInfoHeader.TextContent = (string)FindResource(Constants.ContactInfoTextKey);
        }

        private void ButtonNextStageClickHandler(object sender, RoutedEventArgs e)
        {
            if (_enteredName.Text.Length < 2)
            {
                MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageEnterYourNameKey]);
                return;
            }

            if (ExecutionEngine.Config.EnableCustomerPhone.Value && _enteredPhone.Text.Length < 2)
            {
                MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageEnterYourPhoneKey]);
                return;
            }

            if (ExecutionEngine.Config.EnableCustomerEmail.Value && (_enteredEmail.Text.Length < 2 /*|| !EmailValidator.IsValid(_enteredEmail.Text)*/))
            {
                MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageEnterYourEmailKey]);
                return;
            }

            _stage.CurrentOrder.UserName = _enteredName.Text;
            _stage.CurrentOrder.UserPhone = _enteredPhone.Text;
            _stage.CurrentOrder.UserEmail = _enteredEmail.Text;

            if (ExecutionEngine.Config.EnableCustomerOrderId.Value)
            {
                _stage.SwitchToOrderId();
            }
            else if (ExecutionEngine.Config.EnableCustomerNameOrderId.Value)
            {
                _stage.CurrentOrder.OrderId = string.Format("{0}{1}", _stage.CurrentOrder.UserName, FileOrderStorage.GetOrderIdFromFile() + 1);
                _stage.CompleteOrder();
            }
            else
            {
                _stage.CurrentOrder.OrderId = string.Format("{0}{1}", ExecutionEngine.Config.PhotoKioskId.Value, FileOrderStorage.GetOrderIdFromFile() + 1);
                _stage.CompleteOrder();
            }
        }

        private void ButtonPrevStageClickHandler(object sender, RoutedEventArgs e)
        {
            _stage.SwitchToConfirmOrder();
        }

        private void KeyboardControlsLoaded(object sender, RoutedEventArgs e)
        {
            _screenKeyboard.BindedControl = _enteredName;
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