// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace Aurigma.PhotoKiosk
{
    public partial class BluetoothLoadScreen : Page
    {
        private FindingPhotosStage _findingPhotosStage;
        private bool _isDisposed;
        private DispatcherTimer _timer;
        private List<string> _bluetoothPaths;
        private int _uploadedFilesCount;

        public BluetoothLoadScreen()
        {
            if (ExecutionEngine.Instance != null)
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);

            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(Constants.InactionTimerInterval);
            _timer.Tick += TimerTickHandler;

            _uploadedFilesCount = 0;

            string instructionsText = (string)FindResource(Constants.HowToLoadPhotosUsingBluetoothTextKey);

            if (instructionsText.Contains(Constants.BluetoothHostNamePlaceHolder))
            {
                _bluetoothDescription.Inlines.Add(new Run(instructionsText.Substring(0, instructionsText.IndexOf(Constants.SpecialKeyFramer))));
                _bluetoothDescription.Inlines.Add(new Bold(new Underline(new Run(ExecutionEngine.Config.BluetoothHost.Value))));
                _bluetoothDescription.Inlines.Add(new Run(instructionsText.Substring(instructionsText.IndexOf(Constants.SpecialKeyFramer) + Constants.BluetoothHostNamePlaceHolder.Length)));
            }
            else
                _bluetoothDescription.Text = instructionsText;

            _bluetoothPaths = new List<string>();
        }

        public BluetoothLoadScreen(FindingPhotosStage stage)
            : this()
        {
            _findingPhotosStage = stage;
        }

        ~BluetoothLoadScreen()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            try
            {
                Dispose(true);
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
                _isDisposed = true;
        }

        private void CheckDisposedState()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("SelectDeviceScreen");
        }

        private void LoadedEventHandler(object sender, RoutedEventArgs e)
        {
            _timer.Start();
            _filesCountTextBlock.Text = (string)FindResource(Constants.ReadyToReceivePhotosTextKey);
            _nextButton.IsEnabled = false;
            _bluetoothPaths.Clear();
        }

        private void UnloadedEventHandler(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void ButtonPrevStageClickHandler(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();
            _findingPhotosStage.SwitchToSelectDeviceScreen();
        }

        private void ButtonNextClickHandler(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();
            _findingPhotosStage.SelectedFolders = _bluetoothPaths;
            _findingPhotosStage.SwitchToProcessScreen();
        }

        private void TimerTickHandler(object sender, EventArgs args)
        {
            int filesCount = GetFilesCount();
            if (filesCount == 0)
            {
                _filesCountTextBlock.Text = (string)FindResource(Constants.ReadyToReceivePhotosTextKey);
                _nextButton.IsEnabled = false;
            }
            else if (filesCount != _uploadedFilesCount)
            {
                _uploadedFilesCount = filesCount;
                _filesCountTextBlock.Text = (string)FindResource(Constants.ReceivedPhotosCountTextKey) + _uploadedFilesCount.ToString();
                _nextButton.IsEnabled = true;
                ExecutionEngine.Instance.RegisterUserAction();
            }
        }

        private int GetFilesCount()
        {
            int filesCount = 0;

            var paths = new List<string>();
            paths.Add(ExecutionEngine.Config.BluetoothFolder.Value);

            while (paths.Count > 0)
            {
                if (System.IO.Directory.Exists(paths[0]))
                {
                    _bluetoothPaths.Add(paths[0]);
                    var dirInfo = new DirectoryInfo(paths[0]);
                    paths.RemoveAt(0);
                    try
                    {
                        filesCount += dirInfo.GetFiles().Length;

                        foreach (DirectoryInfo subdirInfo in dirInfo.GetDirectories())
                        {
                            paths.Add(subdirInfo.FullName);
                        }
                    }
                    catch (Exception e)
                    {
                        ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                    }
                }
            }
            return filesCount;
        }
    }
}