// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Aurigma.PhotoKiosk
{
    public partial class ChoosePaperSizes : Window
    {
        private const string OrderItemResourceKey = "OrderKey";
        private const string ResultResourceKey = "ResultKey";

        private OrderItem _orderItem;

        public static Dictionary<PaperFormat, int> ShowChoosePaperSizesDialog(OrderItem item)
        {
            ChoosePaperSizes dialog;

            if (item != null)
                dialog = new ChoosePaperSizes(item);
            else
                dialog = new ChoosePaperSizes();

            ExecutionEngine.Instance.RegisterModalWidow(dialog);

            dialog.Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);
            dialog.MouseMove += delegate (object mouseMoveSender, MouseEventArgs mouseMoveEventArg)
            {
                ExecutionEngine.Instance.RegisterUserAction();
            };
            dialog.ShowDialog();

            return dialog.DialogResult.Value ? dialog._orderInfoControl.ResultValues : null;
        }

        public ChoosePaperSizes()
        {
            InitializeComponent();
        }

        public ChoosePaperSizes(OrderItem item)
            : this()
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _orderItem = item;
            _orderInfoControl.SetSource(item);
        }

        protected override void OnInitialized(EventArgs e)
        {
            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;

            base.OnInitialized(e);
        }

        private void ButtonOkClickHandler(object sender, RoutedEventArgs e)
        {
            if (_orderItem != null)
            {
                foreach (KeyValuePair<PaperFormat, int> pair in _orderInfoControl.ResultValues)
                {
                    if (pair.Value == 0)
                        _orderItem.ClearCrop(pair.Key);
                    else if (_orderItem.GetCount(pair.Key) == 0)
                        _orderItem.SetCrop(pair.Key);

                    _orderItem.SetCount(pair.Key, pair.Value);
                }

                _orderItem.UpdateImprintsCountText();
                DialogResult = true;
            }
            else // Entire order
            {
                DialogResult = MessageDialog.ShowYesNoMessage(FindResource(Constants.ChoosePaperFormatsWarningTextKey).ToString());
            }

            Close();
        }

        private void ButtonCancelClickHandler(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (_orderItem != null)
                _image.Source = _orderItem.SourcePhotoItem.GetBitmapSource();
            else
                _mainGrid.ColumnDefinitions[0].Width = new GridLength(0);

            ExecutionEngine.Instance.SetBackgroundWindowVisibility(true);
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            ExecutionEngine.Instance.SetBackgroundWindowVisibility(false);
        }

        private void ControlLoadedHandler(object sender, RoutedEventArgs e)
        {
            if (_orderInfoControl.IsScrollBarVisible)
            {
                double actualHeight = this.ActualHeight;
                this.SizeToContent = SizeToContent.Width;
                this.Height = actualHeight;
                this.MinHeight = actualHeight;
                this.MaxHeight = actualHeight;

                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = (SystemParameters.PrimaryScreenWidth - this.ActualWidth) / 2;
                this.Top = (SystemParameters.PrimaryScreenHeight - actualHeight) / 2;
            }
        }
    }
}