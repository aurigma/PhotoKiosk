// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Windows;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public partial class AdditionalServicesScreen : Page
    {
        private SelectStage _stage;

        public AdditionalServicesScreen()
        {
            InitializeComponent();

            if (ExecutionEngine.Instance != null)
            {
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);
            }

            foreach (Service service in ExecutionEngine.PriceManager.Services)
            {
                if (!service.IsPermanent)
                {
                    StackPanel serviceButtonPanel = new StackPanel();

                    TextBlock serviceTextUp = new TextBlock();
                    serviceTextUp.Style = (Style)FindResource("SelectDeviceTextUpStyle");
                    serviceTextUp.Text = service.Name;
                    serviceButtonPanel.Children.Add(serviceTextUp);

                    TextBlock serviceTexDown = new TextBlock();
                    serviceTexDown.Style = (Style)FindResource("SelectDeviceTextDownStyle");
                    serviceTexDown.TextWrapping = TextWrapping.Wrap;
                    serviceTexDown.Text = string.Format((string)FindResource(Constants.AdditionalServicePriceTextKey), service.Description, service.GetPriceString());

                    serviceButtonPanel.Children.Add(serviceTexDown);

                    CheckBox serviceButton = new CheckBox();
                    serviceButton.Style = (Style)FindResource("ServiceCheckboxStyle");
                    serviceButton.Content = serviceButtonPanel;
                    serviceButton.Tag = service;
                    serviceButton.Checked += new RoutedEventHandler(serviceButton_Checked);
                    serviceButton.Unchecked += new RoutedEventHandler(serviceButton_Unchecked);

                    _servicesPanel.Children.Add(serviceButton);
                }
            }
        }

        public AdditionalServicesScreen(SelectStage stage)
            : this()
        {
            _stage = stage;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox serviceBox in _servicesPanel.Children)
            {
                Service service = serviceBox.Tag as Service;
                serviceBox.IsChecked = (service != null && _stage.CurrentOrder.Services.Contains(service));
            }
        }

        private void ButtonPrevStageClickHandler(object sender, RoutedEventArgs e)
        {
            _stage.ShowOrderFormingScreen();
        }

        private void ButtonNextClickHandler(object sender, RoutedEventArgs e)
        {
            _stage.SwitchToProcessOrderStage();
        }

        private void serviceButton_Checked(object sender, RoutedEventArgs e)
        {
            Service service = (sender as CheckBox).Tag as Service;
            if (!_stage.CurrentOrder.Services.Contains(service))
                _stage.CurrentOrder.Services.Add(service);
        }

        private void serviceButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Service service = (sender as CheckBox).Tag as Service;
            if (_stage.CurrentOrder.Services.Contains(service))
                _stage.CurrentOrder.Services.Remove(service);
        }
    }
}