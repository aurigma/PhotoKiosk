// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Aurigma.PhotoKiosk
{
    public partial class BurningScreen : Page
    {
        private ProcessOrderStage _stage;
        private AddonManager _cdBurner;
        private DispatcherTimer _timer;
        private string _orderFolder;

        public BurningScreen()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += TimerTickHandler;
        }

        public BurningScreen(ProcessOrderStage stage)
            : this()
        {
            _stage = stage;
        }

        private void BurningScreenLoadedHandler(object sender, RoutedEventArgs e)
        {
            ResetScreen();
        }

        private void BurnButtonClick(object sender, RoutedEventArgs e)
        {
            _orderFolder = (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.ProcessOrder) ? _stage.OrderStorage.CurrentOrderPath : _stage.DelayedOrderFolder;

            string args = string.Format(@"{0} ""{1}"" ""{2}""", ExecutionEngine.Config.DriveLetter.Value, _orderFolder, ExecutionEngine.Config.DiscLabel.Value);
            try
            {
                try
                {
                    File.Move(_orderFolder + "//" + Constants.OrderInfoXmlFileName, Config.TempStoragePath + "//" + Constants.OrderInfoXmlFileName);
                }
                catch
                {
                }

                _cdBurner = new AddonManager(Config.CDBurnerFile, args);
                _cdBurner.Start();
            }
            catch (Exception ex)
            {
                ExecutionEngine.ErrorLogger.WriteExceptionInfo(ex);
                ShowErrorMessage();
                return;
            }

            _timer.Start();

            _burnInstructionsLabel.TextContent = (string)FindResource(Constants.BurningScreenInProcessTextKey);
            _progressBar.Visibility = Visibility.Visible;
            _burnButton.Visibility = Visibility.Hidden;
            _backButton.IsEnabled = false;
        }

        private void ButtonPrevStageClickHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.ProcessOrder)
            {
                RemoveImageFolder();
                _stage.SwitchToContactInfo();
            }
            else
                _stage.SwitchToOrderId();
        }

        private void TimerTickHandler(object sender, EventArgs e)
        {
            if (_cdBurner.Completed)
            {
                _timer.Stop();

                if (_cdBurner.ErrorCode != 0)
                {
                    ExecutionEngine.ErrorLogger.Write(_cdBurner.Output);

                    switch (_cdBurner.ErrorCode)
                    {
                        case unchecked((int)0xC0AA0202):
                            ShowWarningMessage(StringResources.GetString("MessageNoMedia"));
                            break;

                        case unchecked((int)0xC0AA0203):
                            ShowWarningMessage(StringResources.GetString("MessageUnknownMedia"));
                            break;

                        case unchecked((int)0xC0AA0204):
                            ShowWarningMessage(StringResources.GetString("MessageMediaUpsideDown"));
                            break;

                        case unchecked((int)0xC0AA0205):
                            ShowWarningMessage(StringResources.GetString("MessageDriveIsNotReady"));
                            break;

                        case unchecked((int)0xC0AAB15C):
                            ShowWarningMessage(StringResources.GetString("MessageUnsupportedMultisession"));
                            break;

                        case unchecked((int)0xC0AAB101):
                            ShowWarningMessage(StringResources.GetString("MessageUnsupportedMedia"));
                            break;

                        case unchecked((int)0xC0AA0209):
                            ShowWarningMessage(StringResources.GetString("MessageMediaIsReadOnly"));
                            break;

                        case unchecked((int)0xC0AAB120):
                            ShowWarningMessage(StringResources.GetString("MessageToManyFiles"));
                            break;

                        default:
                            ShowErrorMessage();
                            break;
                    }

                    try
                    {
                        File.Move(Config.TempStoragePath + "//" + Constants.OrderInfoXmlFileName, _orderFolder + "//" + Constants.OrderInfoXmlFileName);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    ExecutionEngine.EventLogger.Write(_cdBurner.Output);
                    RemoveImageFolder();

                    try
                    {
                        File.Delete(Config.TempStoragePath + "//" + Constants.OrderInfoXmlFileName);
                    }
                    catch
                    {
                    }
                    _stage.SwitchToThankYou();
                }
            }
        }

        private void ResetScreen()
        {
            _burnInstructionsLabel.TextContent = (string)FindResource(Constants.BurningScreenBeforeTextKey);
            _progressBar.Visibility = Visibility.Hidden;
            _burnButton.Visibility = Visibility.Visible;
            _backButton.IsEnabled = true;
        }

        private void RemoveImageFolder()
        {
            try
            {
                Directory.Delete(_orderFolder, true);
            }
            catch (Exception ex)
            {
                ExecutionEngine.EventLogger.Write("Unable to remove image folder " + _orderFolder + ". " + ex.Message);
            }
        }

        private void ShowWarningMessage(string message)
        {
            if (MessageDialog.ShowTryAgainCancelMessage(message))
            {
                ResetScreen();
            }
            else
            {
                if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.ProcessOrder)
                    RemoveImageFolder();
                _stage.SwitchToThankYouCancel();
            }
        }

        private void ShowErrorMessage()
        {
            MessageDialog.Show(StringResources.GetString("MessageBurningError"));
            if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.ProcessOrder)
                RemoveImageFolder();
            _stage.SwitchToThankYouCancel();
        }
    }
}