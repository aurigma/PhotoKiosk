// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Aurigma.PhotoKiosk
{
    public abstract class StageBase
    {
        #region [Constructors]

        protected StageBase(string name, int allowedInactiveInterval)
        {
            _name = name;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(Constants.InactionTimerInterval);
            _timer.Tick += TimerTickHandler;
            _isTimeoutEnabled = true;
            _allowedInactiveTime = allowedInactiveInterval;
        }

        #endregion [Constructors]

        #region [Public virtual methods]

        public virtual void Activate(ExecutionEngine engine)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");

            _engine = engine;

            _warningMessageShown = false;

            if (_allowedInactiveTime != 0)
                _timer.Start();

            if (_lastVisitedPage != null)
                _engine.ExecuteCommand(new SwitchToScreenCommand(_lastVisitedPage));
        }

        public virtual void Deactivate()
        {
            if (_allowedInactiveTime != 0)
                _timer.Stop();
        }

        public abstract void Reset();

        #endregion [Public virtual methods]

        #region [Public props]

        public string Name
        {
            get { return _name; }
        }

        #endregion [Public props]

        #region [Private methods]

        private void TimerTickHandler(object sender, EventArgs args)
        {
            if (_isTimeoutEnabled)
            {
                if (!_warningMessageShown && _allowedInactiveTime > Constants.BeforeResetMessageTime && _engine.UserInactionTime > TimeSpan.FromMilliseconds(_allowedInactiveTime - Constants.BeforeResetMessageTime))
                {
                    _warningMessageShown = true;

                    if (LastVisitedPage != null)
                        LastVisitedPage.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NoArgDelegate(InvokeShowMessage));
                }
                else if (_warningMessageShown && _engine.UserInactionTime <= TimeSpan.FromMilliseconds(_allowedInactiveTime - Constants.BeforeResetMessageTime))
                {
                    _warningMessageShown = false;
                }

                if (_engine.UserInactionTime > TimeSpan.FromMilliseconds(_allowedInactiveTime))
                {
                    ExecutionEngine.RunCancelOrderProcess();
                    _engine.ExecuteCommand(new ResetOrderDataCommand());
                }
            }
        }

        private delegate void NoArgDelegate();

        private void InvokeShowMessage()
        {
            MessageDialog.ShowBackToOrderMessage();
        }

        #endregion [Private methods]

        #region [Protected props and methods]

        protected Page LastVisitedPage
        {
            get { return _lastVisitedPage; }
            set { _lastVisitedPage = value; }
        }

        public ExecutionEngine Engine
        {
            get { return _engine; }
        }

        protected void EnableTimeout()
        {
            if (!_isTimeoutEnabled)
                _isTimeoutEnabled = true;
        }

        protected void DisableTimeout()
        {
            if (_isTimeoutEnabled)
                _isTimeoutEnabled = false;
        }

        #endregion [Protected props and methods]

        #region [Variables]

        private Page _lastVisitedPage;
        private ExecutionEngine _engine;
        private DispatcherTimer _timer;
        private int _allowedInactiveTime;
        private string _name;
        private bool _warningMessageShown;
        private bool _isTimeoutEnabled;

        #endregion [Variables]
    }
}