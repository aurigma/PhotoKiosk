// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Windows;
using System.Windows.Threading;

namespace Aurigma.PhotoKiosk
{
    public interface IProgressCallback
    {
        void SetRange(int minimum, int maximum);

        void Increment();

        bool IsAborted
        {
            get;
        }

        bool IsComplete
        {
            get;
            set;
        }

        void End();
    }

    public partial class ProgressDialog : Window, IProgressCallback
    {
        public delegate void SetRangeDelegate(int minimum, int maximum);

        public delegate void IncrementDelegate();

        public delegate void EndDelegate();

        #region [Constructors]

        public ProgressDialog(string text)
        {
            InitializeComponent();

            _isComplete = false;
            _isAborted = false;
            _messageBlock.Text = text;

            Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);

            if (ExecutionEngine.Instance != null)
                ExecutionEngine.Instance.RegisterModalWidow(this);
        }

        #endregion [Constructors]

        #region [Public methods and properties]

        public void SetRange(int minimum, int maximum)
        {
            _progressBar.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new SetRangeDelegate(DoSetRange), minimum, maximum);
        }

        public void Increment()
        {
            _progressBar.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new IncrementDelegate(DoIncrement));
        }

        public void End()
        {
            _progressBar.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new EndDelegate(DoEnd));
        }

        public bool IsComplete
        {
            get { return _isComplete; }
            set { _isComplete = value; }
        }

        public bool IsAborted
        {
            get { return _isAborted; }
        }

        public string MessageText
        {
            get { return _messageBlock.Text; }
            set { _messageBlock.Text = value; }
        }

        #endregion [Public methods and properties]

        #region [Private methods]

        private void DoSetRange(int minimum, int maximum)
        {
            _progressBar.Minimum = minimum;
            _progressBar.Maximum = maximum;
            _progressBar.Value = minimum;
        }

        private void DoIncrement()
        {
            _progressBar.Value++;
        }

        private void DoEnd()
        {
            if (this.Visibility == Visibility.Visible)
                this.DialogResult = _isComplete;
        }

        #endregion [Private methods]

        #region [Protected Override]

        protected override void OnInitialized(EventArgs e)
        {
            Loaded += ProgressDialogLoadedHandler;
            Unloaded += ProgressDialogUnloadedHandler;

            base.OnInitialized(e);
        }

        #endregion [Protected Override]

        #region [Event handlers]

        private void ProgressDialogLoadedHandler(object sender, RoutedEventArgs e)
        {
            if (!_isComplete && ExecutionEngine.Instance != null)
                ExecutionEngine.Instance.SetBackgroundWindowVisibility(true);
        }

        private void ProgressDialogUnloadedHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Instance != null)
                ExecutionEngine.Instance.SetBackgroundWindowVisibility(false);
        }

        private void ButtonCancelClickHandler(object sender, RoutedEventArgs e)
        {
            if (!_isComplete)
                _isAborted = true;
        }

        #endregion [Event handlers]

        #region [Variables]

        private bool _isComplete;
        private bool _isAborted;

        #endregion [Variables]
    }
}