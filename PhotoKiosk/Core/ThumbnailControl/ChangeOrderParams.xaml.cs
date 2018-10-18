// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aurigma.PhotoKiosk
{
    public partial class ChangeOrderParams : Window
    {
        private Order _currentOrder;
        private PaperType _orderPaperType;
        private string _orderCropMode;

        public ChangeOrderParams()
        {
            InitializeComponent();

            _currentOrder = (Order)ExecutionEngine.Context[Constants.OrderContextName];
            _orderPaperType = _currentOrder == null ? ExecutionEngine.Instance.PaperTypes[0] : _currentOrder.OrderPaperType;
            _orderCropMode = _currentOrder == null ? Constants.CropToFillModeName : _currentOrder.CropMode;
        }

        public static bool ShowChoosePaperTypeDialog()
        {
            ChangeOrderParams dialog = new ChangeOrderParams();
            ExecutionEngine.Instance.RegisterModalWidow(dialog);
            dialog.Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            dialog.MouseMove += delegate (object mouseMoveSender, MouseEventArgs mouseMoveEventArg)
            {
                ExecutionEngine.Instance.RegisterUserAction();
            };

            return (bool)dialog.ShowDialog();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            // Paper types
            foreach (PaperType type in ExecutionEngine.Instance.PaperTypes)
            {
                StackPanel paperTypeButtonPanel = new StackPanel();

                TextBlock paperTypeTextUp = new TextBlock();
                paperTypeTextUp.Style = (Style)FindResource("OrderParamsTextUpStyle");
                paperTypeTextUp.Text = type.Name;
                paperTypeButtonPanel.Children.Add(paperTypeTextUp);

                TextBlock paperTypeTexDown = new TextBlock();
                paperTypeTexDown.Style = (Style)FindResource("OrderParamsTextDownStyle");
                paperTypeTexDown.Text = type.Description;
                paperTypeTexDown.TextWrapping = TextWrapping.Wrap;
                paperTypeButtonPanel.Children.Add(paperTypeTexDown);

                RadioButton paperTypeButton = new RadioButton();
                paperTypeButton.Style = (Style)FindResource("OrderParamsRadioButtonStyle");
                paperTypeButton.Content = paperTypeButtonPanel;
                paperTypeButton.Tag = type.Name;
                paperTypeButton.IsChecked = type == _orderPaperType;
                paperTypeButton.Click += SetPaperTypeButtonClickHandler;

                _paperTypePanel.Children.Add(paperTypeButton);
            }

            // Crop modes
            StackPanel cropToFillButtonPanel = new StackPanel();

            TextBlock cropToFillTextUp = new TextBlock();
            cropToFillTextUp.Style = (Style)FindResource("OrderParamsTextUpStyle");
            cropToFillTextUp.Text = (string)TryFindResource(Constants.CropToFillTextKey);
            cropToFillButtonPanel.Children.Add(cropToFillTextUp);

            TextBlock cropToFillTextDown = new TextBlock();
            cropToFillTextDown.Style = (Style)FindResource("OrderParamsTextDownStyle");
            cropToFillTextDown.Text = (string)TryFindResource(Constants.CropToFillTextDescriptionKey);
            cropToFillTextDown.TextWrapping = TextWrapping.Wrap;
            cropToFillButtonPanel.Children.Add(cropToFillTextDown);

            RadioButton cropToFillRadioButton = new RadioButton();
            cropToFillRadioButton.Style = (Style)FindResource("OrderParamsRadioButtonStyle");
            cropToFillRadioButton.Content = cropToFillButtonPanel;
            cropToFillRadioButton.Tag = Constants.CropToFillModeName;
            cropToFillRadioButton.IsChecked = Constants.CropToFillModeName == _orderCropMode;
            cropToFillRadioButton.Click += SetCropModeButtonClickHandler;
            _cropModePanel.Children.Add(cropToFillRadioButton);

            StackPanel resizeToFitButtonPanel = new StackPanel();

            TextBlock resizeToFitTextUp = new TextBlock();
            resizeToFitTextUp.Style = (Style)FindResource("OrderParamsTextUpStyle");
            resizeToFitTextUp.Text = (string)TryFindResource(Constants.ResizeToFitTextKey);
            resizeToFitButtonPanel.Children.Add(resizeToFitTextUp);

            TextBlock resizeToFitTextDown = new TextBlock();
            resizeToFitTextDown.Style = (Style)FindResource("OrderParamsTextDownStyle");
            resizeToFitTextDown.Text = (string)TryFindResource(Constants.ResizeToFitTextDescriptionKey);
            resizeToFitTextDown.TextWrapping = TextWrapping.Wrap;
            resizeToFitButtonPanel.Children.Add(resizeToFitTextDown);

            RadioButton resizeToFitRadioButton = new RadioButton();
            resizeToFitRadioButton.Style = (Style)FindResource("OrderParamsRadioButtonStyle");
            resizeToFitRadioButton.Content = resizeToFitButtonPanel;
            resizeToFitRadioButton.Tag = Constants.ResizeToFitModeName;
            resizeToFitRadioButton.IsChecked = Constants.ResizeToFitModeName == _orderCropMode;
            resizeToFitRadioButton.Click += SetCropModeButtonClickHandler;
            _cropModePanel.Children.Add(resizeToFitRadioButton);

            ExecutionEngine.Instance.SetBackgroundWindowVisibility(true);
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            ExecutionEngine.Instance.SetBackgroundWindowVisibility(false);
        }

        private void ButtonOkClickHandler(object sender, RoutedEventArgs e)
        {
            if (_currentOrder != null)
            {
                _currentOrder.OrderPaperType = _orderPaperType;
                _currentOrder.CropMode = _orderCropMode;
                foreach (OrderItem item in _currentOrder.OrderItems)
                {
                    foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                    {
                        if (item.GetCount(format) > 0)
                            item.SetCrop(format);
                    }
                    item.UpdateImageCommandButton();
                }
            }
            DialogResult = true;
            Close();
        }

        private void ButtonCancelClickHandler(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SetPaperTypeButtonClickHandler(object sender, RoutedEventArgs e)
        {
            RadioButton senderButton = sender as RadioButton;
            if (senderButton != null)
            {
                _orderPaperType = ExecutionEngine.PriceManager.GetPaperType(senderButton.Tag.ToString());
                if (_orderPaperType == null)
                {
                    ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "ChoosePaperType:SetPaperType unknown paperType {0}, set to default", _orderPaperType));
                    _orderPaperType = ExecutionEngine.Instance.PaperTypes[0];
                }
            }
        }

        private void SetCropModeButtonClickHandler(object sender, RoutedEventArgs e)
        {
            RadioButton senderButton = sender as RadioButton;
            if (senderButton != null)
            {
                _orderCropMode = senderButton.Tag.ToString();
                if ((_orderCropMode != Constants.ResizeToFitModeName) && (_orderCropMode != Constants.CropToFillModeName))
                {
                    ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "ChoosePaperType:SetCropMode unknown cropMode {0}, set to default", _orderCropMode));
                    _orderCropMode = Constants.CropToFillModeName;
                }
            }
        }
    }
}