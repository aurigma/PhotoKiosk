// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Windows;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public partial class SelectDeviceScreen : Page
    {
        private FindingPhotosStage _findingPhotosStage;
        private bool _isDisposed;

        public SelectDeviceScreen()
        {
            if (ExecutionEngine.Instance != null)
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);

            InitializeComponent();
        }

        public SelectDeviceScreen(FindingPhotosStage stage)
            : this()
        {
            _findingPhotosStage = stage;
            _storageDeviceButton.IsChecked = true;
        }

        ~SelectDeviceScreen()
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

        private void ButtonNextClickHandler(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();
            if (_storageDeviceButton.IsChecked.Value)
                _findingPhotosStage.UseStorage();
            else
                _findingPhotosStage.UseBluetooth();
        }
    }
}