// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Windows;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public partial class WelcomeScreen : Page
    {
        public WelcomeScreen()
        {
            if (ExecutionEngine.Instance != null)
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);

            InitializeComponent();

            // Hint text
            if (ExecutionEngine.Config.IsMultiAction(ExecutionEngine.PriceManager))
            {
                if (ExecutionEngine.Config.IsBluetoothEnabled())
                    _welcomeScreenHint.TextContent = (string)FindResource(Constants.WelcomeNoticeBluetoothEnabledMultiActionTextKey);
                else
                    _welcomeScreenHint.TextContent = (string)FindResource(Constants.WelcomeNoticeBluetoothDisabledMultiActionTextKey);
            }
            else
            {
                if (ExecutionEngine.Config.IsBluetoothEnabled())
                    _welcomeScreenHint.TextContent = (string)FindResource(Constants.WelcomeNoticeBluetoothEnabledTextKey);
                else
                    _welcomeScreenHint.TextContent = (string)FindResource(Constants.WelcomeNoticeBluetoothDisabledTextKey);
            }

            // Order photos
            if (ExecutionEngine.Config.EnablePhotoOrdering.Value &&
                ExecutionEngine.PriceManager.MinilabPaperFormats.Count > 0 && ExecutionEngine.PriceManager.MinilabPaperTypes.Count > 0)
            {
                _orderPhotosButton.Visibility = Visibility.Visible;

                if (!ExecutionEngine.Config.IsMultiAction(ExecutionEngine.PriceManager))
                    _orderPhotosButtonText.TextContent = (string)FindResource(Constants.WelcomeStageButtonNextTextKey);
            }
            else
                _orderPhotosButton.Visibility = Visibility.Collapsed;

            // Print photos
            if (ExecutionEngine.Config.IsPhotoPrintingEnabled(ExecutionEngine.PriceManager))
            {
                _printPhotosButton.Visibility = Visibility.Visible;

                if (!ExecutionEngine.Config.IsMultiAction(ExecutionEngine.PriceManager))
                    _printPhotosButtonText.TextContent = (string)FindResource(Constants.WelcomeStageButtonNextTextKey);
            }
            else
                _printPhotosButton.Visibility = Visibility.Collapsed;

            // Burn CD
            if (ExecutionEngine.Config.IsCDBurningEnabled())
            {
                _burnCdButton.Visibility = Visibility.Visible;

                if (!ExecutionEngine.Config.IsMultiAction(ExecutionEngine.PriceManager))
                    _burnCdButtonText.TextContent = (string)FindResource(Constants.WelcomeStageButtonNextTextKey);
            }
            else
                _burnCdButton.Visibility = Visibility.Collapsed;

            // Process order
            if (ExecutionEngine.Config.IsPhotoPrintingEnabled(ExecutionEngine.PriceManager) && ExecutionEngine.Config.PhotoPrintingRequireConfirm.Value ||
                ExecutionEngine.Config.IsCDBurningEnabled() && ExecutionEngine.Config.CDBurningRequireConfirm.Value)
                _processButton.Visibility = Visibility.Visible;
            else
                _processButton.Visibility = Visibility.Collapsed;
        }

        public WelcomeScreen(WelcomeStage stage)
            : this()
        {
            _welcomeStage = stage;
        }

        private void OrderButtonClick(object sender, RoutedEventArgs e)
        {
            ExecutionEngine.Instance.PrimaryAction = PrimaryActionType.OrderPhotos;
            _welcomeStage.SwitchToFindingPhotosStage();
        }

        private void PrintButtonClick(object sender, RoutedEventArgs e)
        {
            ExecutionEngine.Instance.PrimaryAction = PrimaryActionType.PrintPhotos;
            _welcomeStage.SwitchToFindingPhotosStage();
        }

        private void BurnButtonClick(object sender, RoutedEventArgs e)
        {
            ExecutionEngine.Instance.PrimaryAction = PrimaryActionType.BurnCd;
            _welcomeStage.SwitchToFindingPhotosStage();
        }

        private void ProcessButtonClick(object sender, RoutedEventArgs e)
        {
            ExecutionEngine.Instance.PrimaryAction = PrimaryActionType.ProcessOrder;
            _welcomeStage.SwitchToProcessOrderStage();
        }

        private WelcomeStage _welcomeStage;
    }
}