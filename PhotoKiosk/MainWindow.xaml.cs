// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Aurigma.PhotoKiosk
{
    public partial class MainWindow : System.Windows.Window, System.IDisposable
    {
        #region [Construction / destruction]

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                _executionEngine = new ExecutionEngine(MainFrame, this);
                _executionEngine.ExecuteCommand(new SwitchToStageCommand(Constants.WelcomeStageName));
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                throw;
            }
        }

        ~MainWindow()
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
                System.GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _executionEngine.Dispose();
                _disposed = true;
            }
        }

        private void CheckDisposedState()
        {
            if (_disposed)
                throw new System.ObjectDisposedException("MainWindow");
        }

        #endregion [Construction / destruction]

        #region [Protected methods]

        protected override void OnInitialized(EventArgs e)
        {
            CheckDisposedState();

            this.Loaded += WindowLoadedHandler;
            base.OnInitialized(e);
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            CheckDisposedState();

            base.OnPreviewMouseDown(e);

            if (_executionEngine != null)
                _executionEngine.RegisterUserAction();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            CheckDisposedState();

            base.OnPreviewMouseMove(e);

            if (_executionEngine != null)
                _executionEngine.RegisterUserAction();
        }

        #endregion [Protected methods]

        #region [Public methods and props]

        public Rectangle BackgroundRect
        {
            get
            {
                CheckDisposedState();
                return _backgroundRect;
            }
        }

        public void SetCancelButtonVisibility(Visibility vis)
        {
            CheckDisposedState();
            _cancelOrderButton.Visibility = vis;
        }

        #endregion [Public methods and props]

        #region [Private methods]

        private void WindowLoadedHandler(object sender, RoutedEventArgs e)
        {
            _queryCancelAutoPlayMessageId = NativeMethods.RegisterWindowMessage("QueryCancelAutoPlay");

            System.Windows.Interop.HwndSource source = System.Windows.Interop.HwndSource.FromHwnd(new System.Windows.Interop.WindowInteropHelper(this).Handle);

            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == _queryCancelAutoPlayMessageId)
            {
                handled = true;
                return new IntPtr(1);
            }
            return IntPtr.Zero;
        }

        private void CancelButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (_executionEngine == null)
                throw new Aurigma.GraphicsMill.UnexpectedException("_executionEngine cannot be null");

            if (MessageDialog.ShowOkCancelMessage((string)this.Resources[Constants.MessageSureWantToCancelOrderKey]))
            {
                ExecutionEngine.RunCancelOrderProcess();
                _executionEngine.ExecuteCommand(new ResetOrderDataCommand());
            }
        }

        #endregion [Private methods]

        #region [Variables]

        private ExecutionEngine _executionEngine;
        private uint _queryCancelAutoPlayMessageId;
        private bool _disposed;

        #endregion [Variables]
    }
}