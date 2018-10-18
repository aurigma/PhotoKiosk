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
    public partial class PrintingScreen : Page
    {
        private ProcessOrderStage _stage;
        private DispatcherTimer _timer;
        private AddonManager _photoPrinter;
        private string _orderFolder;

        public PrintingScreen()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += TimerTickHandler;
        }

        public PrintingScreen(ProcessOrderStage stage)
            : this()
        {
            _stage = stage;
        }

        private void PrintingScreenLoadedHandler(object sender, RoutedEventArgs e)
        {
            _orderFolder = (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.ProcessOrder) ? _stage.OrderStorage.CurrentOrderPath : _stage.DelayedOrderFolder;

            string args = string.Format(@"""{0}\\"" ""{1}""", _orderFolder, Constants.MaxCopiesCount);
            try
            {
                _photoPrinter = new AddonManager(Config.PhotoPrinterFile, args);
                _photoPrinter.Start();
            }
            catch (Exception ex)
            {
                ExecutionEngine.ErrorLogger.WriteExceptionInfo(ex);
                ShowErrorMessage();
                return;
            }

            _timer.Start();
        }

        private void TimerTickHandler(object sender, EventArgs e)
        {
            if (_photoPrinter.Completed)
            {
                _timer.Stop();

                if (_photoPrinter.ErrorCode != 0)
                {
                    ExecutionEngine.ErrorLogger.Write(_photoPrinter.Output);
                    ShowErrorMessage();
                }
                else
                {
                    ExecutionEngine.EventLogger.Write(_photoPrinter.Output);
                    try
                    {
                        Directory.Delete(_orderFolder, true);
                    }
                    catch
                    {
                    }

                    _stage.SwitchToThankYou();
                }
            }
        }

        private void ShowErrorMessage()
        {
            MessageDialog.Show(StringResources.GetString("MessagePhotoPrinterError"));
            _stage.SwitchToThankYouCancel();
        }
    }
}