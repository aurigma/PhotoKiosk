// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Aurigma.PhotoKiosk
{
    public partial class OrderFormingScreen : Page, IDisposable
    {
        #region [Construction / destruction]

        public OrderFormingScreen()
        {
            if (ExecutionEngine.Instance != null)
            {
                Resources.MergedDictionaries.Add(ExecutionEngine.Instance.Resource);
            }

            InitializeComponent();
        }

        public OrderFormingScreen(SelectStage stage)
            : this()
        {
            _stage = stage;
            _listControl = new ThumbnailListControl(Constants.OrderItemStyleName);

            if (!_listControl.Resources.MergedDictionaries.Contains(Resources))
                _listControl.Resources.MergedDictionaries.Add(Resources);

            _listControl.AddFilterTab(new ApplyAllFilter(), (string)TryFindResource(Constants.TabAllTextKey));

            foreach (PaperFormat format in ExecutionEngine.PriceManager.PaperFormats)
                _listControl.AddFilterTab(new PaperSizeFilter(format), format.Name);

            _listControl.ShowEmptyFilterTabs = false;

            ContentFrame.Content = _listControl;

            if (ExecutionEngine.Context.Contains(Constants.OrderContextName))
                _orderInfoControl.SetSource((Order)ExecutionEngine.Context[Constants.OrderContextName]);

            _listControl.List.ItemChanged += new EventHandler(ListItemContentChangedHandler);
            _listControl.SetItemsLayout(Constants.OrderFormingScreenItemsInRow, Constants.OrderFormingScreenItemsInColumn);
            _listControl.ShowSelectAllButton = false;

            _currentOrder = (Order)ExecutionEngine.Context[Constants.OrderContextName];
        }

        ~OrderFormingScreen()
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
                throw new ObjectDisposedException("OrderFormingScreen");
        }

        #endregion [Construction / destruction]

        #region [Public methods and props]

        public ThumbnailListControl ListControl
        {
            get
            {
                CheckDisposedState();
                return _listControl;
            }
        }

        #endregion [Public methods and props]

        #region [EventHandlers]

        private void OrderFormingScreenLoadHandler(object sender, RoutedEventArgs e)
        {
            _orderParamsControl.Update();
        }

        private void ButtonNextClickHandler(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();

            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.OrderPhotos && ExecutionEngine.PriceManager.HasServicesToChoose())
                _stage.SwitchToAdditionalServicesScreen();
            else
                _stage.SwitchToProcessOrderStage();
        }

        private void ButtonPrevStageClickHandler(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();
            _stage.ShowSelectScreen();
        }

        private void SetFormatForAllItems(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();

            Dictionary<PaperFormat, int> paperSizeCounts = ChoosePaperSizes.ShowChoosePaperSizesDialog(null);

            if (paperSizeCounts != null && ExecutionEngine.Context.Contains(Constants.OrderContextName))
            {
                Order currentOrder = (Order)ExecutionEngine.Context[Constants.OrderContextName];

                foreach (OrderItem orderItem in currentOrder.OrderItems)
                {
                    foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                    {
                        if (paperSizeCounts[format] == 0)
                            orderItem.ClearCrop(format);
                        else if (orderItem.GetCount(format) == 0)
                            orderItem.SetCrop(format);

                        orderItem.SetCount(format, paperSizeCounts[format]);
                    }
                    orderItem.UpdateImprintsCountText();
                }

                if (currentOrder.OrderItems[0].IsEmpty())
                {
                    foreach (ThumbnailItem item in _listControl.List.Items)
                    {
                        item.Checked = false;
                    }

                    currentOrder.OrderItems.Clear();
                    _listControl.List.ClearAll();

                    _stage.ShowSelectScreen();
                }

                _orderInfoControl.Update();
                _listControl.UpdateFilterControls();
            }
        }

        private void SetPaperTypeButtonClickHandler(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();

            if (ChangeOrderParams.ShowChoosePaperTypeDialog())
            {
                _orderInfoControl.Update();
                _orderParamsControl.Update();
                _listControl.List.Refresh();
            }
        }

        private void ListItemContentChangedHandler(object sender, EventArgs e)
        {
            CheckDisposedState();

            ThumbnailItem[] itemList = new ThumbnailItem[_listControl.List.Items.Count];
            _listControl.List.Items.CopyTo(itemList, 0);

            foreach (ThumbnailItem item in itemList)
            {
                if (item.Order.IsEmpty())
                {
                    _listControl.List.RemoveItem(item, false);
                }
            }

            if (_listControl.List.Items.Count == 0)
            {
                _stage.ShowSelectScreen();
            }

            _orderInfoControl.Update();
        }

        #endregion [EventHandlers]

        #region [Variables]

        private Aurigma.PhotoKiosk.SelectStage _stage;
        private ThumbnailListControl _listControl;
        private Order _currentOrder;
        private bool _isDisposed;

        #endregion [Variables]
    }
}