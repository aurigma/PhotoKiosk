// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public sealed partial class SelectScreen : Page, IDisposable
    {
        #region "Construction / destruction"

        public SelectScreen()
        {
            if (ExecutionEngine.Instance != null)
            {
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);
            }

            InitializeComponent();
        }

        public SelectScreen(Aurigma.PhotoKiosk.SelectStage stage)
            : this()
        {
            _selectStage = stage;
            _listControl = new ThumbnailListControl(Constants.SelectItemStyleName);

            if (_listControl.Resources.MergedDictionaries.Contains(Resources) == false)
                _listControl.Resources.MergedDictionaries.Add(Resources);

            _listControl.AddFilterTab(new ApplyAllFilter(), (string)TryFindResource(Constants.TabAllTextKey));
            _listControl.AddFilterTab(new ItemCheckedFilter(true), (string)TryFindResource(Constants.TabCheckedTextKey));
            _listControl.AddFilterTab(new ItemCheckedFilter(false), (string)TryFindResource(Constants.TabUncheckedTextKey));

            _listControl.ShowEmptyFilterTabs = false;

            _listControl.SetItemsLayout(Constants.SelectScreenItemsInRow, Constants.SelectScreenItemsInColumn);

            this.ContentFrame.Content = _listControl;

            _listControl.ListEmpty += ListEmptyHandler;
        }

        ~SelectScreen()
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

        private void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                if (_listControl != null)
                {
                    _listControl.Dispose();
                    _listControl = null;
                }

                _isDisposed = true;
            }
        }

        private void CheckDisposedState()
        {
            if (_isDisposed)
                throw new System.ObjectDisposedException("SelectScreen");
        }

        #endregion "Construction / destruction"

        private void PageLoadedHandler(object sender, RoutedEventArgs e)
        {
            if ((ExecutionEngine.Config.EnableFolderSelection.Value && ExecutionEngine.Instance.IsShowFolders) || ExecutionEngine.Instance.IsBluetooth)
                _prevButton.Visibility = Visibility.Visible;
            else
                _prevButton.Visibility = Visibility.Collapsed;

            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
            {
                _selectScreenHeader.TextContent = (string)FindResource(Constants.SelectStepCdBurningTextKey);
                _selectScreenHint.TextContent = (string)FindResource(Constants.SelectStepNoticeCdBurningTextKey);
            }
            else
            {
                _selectScreenHeader.TextContent = (string)FindResource(Constants.SelectStepTextKey);
                _selectScreenHint.TextContent = (string)FindResource(Constants.SelectStepNoticeTextKey);
            }
        }

        private void ButtonPrevStageClickHandler(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();
            _selectStage.SwitchToFindingPhotosSage();
        }

        private void ButtonNextStageClickHandler(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();
            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
                _selectStage.SwitchToProcessOrderStage();
            else
                _selectStage.ShowOrderFormingScreen();
        }

        public ThumbnailListControl ListControl
        {
            get
            {
                CheckDisposedState();
                return _listControl;
            }
        }

        private void ListEmptyHandler(object sender, EventArgs e)
        {
            ExecutionEngine.Instance.ExecuteCommand(new ResetOrderDataCommand());
        }

        #region "[private] Member variables"

        private SelectStage _selectStage;
        private ThumbnailListControl _listControl;
        private bool _isDisposed;

        #endregion "[private] Member variables"
    }
}