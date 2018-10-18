// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aurigma.PhotoKiosk
{
    public partial class MessageDialog : System.Windows.Window
    {
        #region [Public static]

        public static bool Show(string message)
        {
            MessageDialog dialog = new MessageDialog(true, ExecutionEngine.Instance.Resource[Constants.MessageOkTextKey] as string, null);
            dialog.MessageText = message;
            ExecutionEngine.Instance.RegisterModalWidow(dialog);
            dialog.ShowDialog();

            return dialog.DialogResult.Value;
        }

        public static bool ShowNotifyMessage(string message)
        {
            MessageDialog dialog = new MessageDialog(false, ExecutionEngine.Instance.Resource[Constants.MessageOkTextKey] as string, null);
            dialog.MessageText = message;
            ExecutionEngine.Instance.RegisterModalWidow(dialog);
            dialog.ShowDialog();

            return dialog.DialogResult.Value;
        }

        public static bool ShowCancelMessage(string message)
        {
            MessageDialog dialog = new MessageDialog(false, null, ExecutionEngine.Instance.Resource[Constants.MessageCancelTextKey] as string);
            dialog.MessageText = message;
            ExecutionEngine.Instance.RegisterModalWidow(dialog);
            dialog.ShowDialog();

            return dialog.DialogResult.Value;
        }

        public static bool ShowOkCancelMessage(string message)
        {
            MessageDialog dialog = new MessageDialog(true, ExecutionEngine.Instance.Resource[Constants.MessageOkTextKey] as string, ExecutionEngine.Instance.Resource[Constants.MessageCancelTextKey] as string);
            dialog.MessageText = message;
            ExecutionEngine.Instance.RegisterModalWidow(dialog);
            dialog.ShowDialog();

            return dialog.DialogResult.Value;
        }

        public static bool ShowYesNoMessage(string message)
        {
            MessageDialog dialog = new MessageDialog(true, ExecutionEngine.Instance.Resource[Constants.MessageYesTextKey] as string, ExecutionEngine.Instance.Resource[Constants.MessageNoTextKey] as string);
            dialog.MessageText = message;
            ExecutionEngine.Instance.RegisterModalWidow(dialog);
            dialog.ShowDialog();

            return dialog.DialogResult.Value;
        }

        public static bool ShowTryAgainCancelMessage(string message)
        {
            MessageDialog dialog = new MessageDialog(true, ExecutionEngine.Instance.Resource[Constants.BurningScreenMessageTryAgainTextKey] as string, ExecutionEngine.Instance.Resource[Constants.BurningScreenMessageCancelTextKey] as string);
            dialog.MessageText = message;
            ExecutionEngine.Instance.RegisterModalWidow(dialog);
            dialog.ShowDialog();

            return dialog.DialogResult.Value;
        }

        public static bool ShowBackToOrderMessage()
        {
            MessageDialog dialog = new MessageDialog(false, null, ExecutionEngine.Instance.Resource[Constants.BackToOrderButtonKey] as string);
            dialog.MessageText = ExecutionEngine.Instance.Resource[Constants.MessageBeforeResetKey].ToString();
            ExecutionEngine.Instance.RegisterModalWidow(dialog);
            dialog.ShowDialog();

            return dialog.DialogResult.Value;
        }

        #endregion [Public static]

        #region [Constructors]

        private MessageDialog(bool needMouseEventsHandling, string okButtonText, string cancelButtonText)
        {
            InitializeComponent();
            _needMouseEventsHandling = needMouseEventsHandling;

            if (!string.IsNullOrEmpty(okButtonText))
            {
                _okButton.Visibility = Visibility.Visible;
                (_okButton.Content as TextBlock).Text = okButtonText;
            }
            else
                _okButton.Visibility = Visibility.Collapsed;

            if (!string.IsNullOrEmpty(cancelButtonText))
            {
                _cancelButton.Visibility = Visibility.Visible;
                (_cancelButton.Content as TextBlock).Text = cancelButtonText;
            }
            else
                _cancelButton.Visibility = Visibility.Collapsed;
        }

        #endregion [Constructors]

        #region [Protected Override]

        protected override void OnInitialized(EventArgs e)
        {
            Loaded += MessageDialogLoadedHandler;
            Unloaded += MessageDialogUnloadedHandler;

            base.OnInitialized(e);
        }

        #endregion [Protected Override]

        #region [Public methods and props]

        public string MessageText
        {
            get
            {
                return _messageBlock.Text;
            }
            set
            {
                _messageBlock.Text = value;
            }
        }

        #endregion [Public methods and props]

        #region [Event handlers]

        private void ButtonOkClickHandler(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonCancelClickHandler(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            ExecutionEngine.Instance.RegisterUserAction();
        }

        private void MessageDialogLoadedHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Instance != null)
            {
                ExecutionEngine.Instance.SetBackgroundWindowVisibility(true);
            }

            Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);

            if (_needMouseEventsHandling)
            {
                this.MouseMove += MouseMoveHandler;
            }
        }

        private void MessageDialogUnloadedHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Instance != null)
            {
                ExecutionEngine.Instance.SetBackgroundWindowVisibility(false);
            }
        }

        #endregion [Event handlers]

        #region [Variables]

        private bool _needMouseEventsHandling;

        #endregion [Variables]
    }
}