// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Aurigma.PhotoKiosk
{
    public class SelectStage : StageBase, System.IDisposable
    {
        #region [Construction / destruction]

        public SelectStage()
            : base(Constants.SelectStageName, ExecutionEngine.Config.InactivityTimeout.Value)
        {
            _selectScreen = new SelectScreen(this);
            _orderFormingScreen = new OrderFormingScreen(this);
            _additionalServicesScreen = new AdditionalServicesScreen(this);

            _selectScreen.Loaded += SelectScreenLoadedHandler;

            LastVisitedPage = _selectScreen;

            ExecutionEngine.EventLogger.Write("SelectStage created");
        }

        ~SelectStage()
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
            if (disposing && !_isDisposed)
            {
                if (_orderFormingScreen != null)
                {
                    _orderFormingScreen.Dispose();
                    _orderFormingScreen = null;
                }
                if (_selectScreen != null)
                {
                    _selectScreen.Dispose();
                    _selectScreen = null;
                }

                _isDisposed = true;
            }
        }

        private void CheckDisposedState()
        {
            if (_isDisposed)
                throw new System.ObjectDisposedException("SelectStage");
        }

        #endregion [Construction / destruction]

        #region [Public overriden methods]

        public override void Activate(ExecutionEngine engine)
        {
            if (ExecutionEngine.Context.Contains(Constants.OrderContextName))
            {
                _currentOrder = (Order)ExecutionEngine.Context[Constants.OrderContextName];
            }

            // Adding photo from Image Editor.
            if (ExecutionEngine.Context.Contains(Constants.ImageEditorResultKey))
            {
                PhotoItem imageEditorItem = (PhotoItem)ExecutionEngine.Context[Constants.ImageEditorResultKey];
                imageEditorItem.CreateCacheData();

                ThumbnailItem imageEditorResult = new ThumbnailItem(imageEditorItem);
                imageEditorResult.HostList = _orderFormingScreen.ListControl.List;

                ExecutionEngine.Context.Remove(Constants.ImageEditorResultKey);

                imageEditorResult.SetCheckedProperty(true);

                _currentOrder.OrderItems.Add(imageEditorResult.Order);
                imageEditorResult.Order.Reset();

                _orderFormingScreen.ListControl.List.AddItem(imageEditorResult);
            }

            base.Activate(engine);
            ExecutionEngine.EventLogger.Write("SelectStage:Activate");
        }

        public override void Reset()
        {
            LastVisitedPage = _selectScreen;

            _selectScreen.ListControl.List.ClearAll();
            _orderFormingScreen.ListControl.List.ClearAll();

            _selectScreen.ListControl.WasInitialized = false;
            _orderFormingScreen.ListControl.WasInitialized = false;

            ExecutionEngine.EventLogger.Write("SelectStage:Reset");
        }

        #endregion [Public overriden methods]

        #region [Public methods and props]

        public void ShowSelectScreen()
        {
            ExecutionEngine.EventLogger.Write("SelectStage:ShowSelectScreen");
            CheckDisposedState();

            LastVisitedPage = _selectScreen;
            Engine.ExecuteCommand(new SwitchToScreenCommand(_selectScreen));
        }

        public void ShowOrderFormingScreen()
        {
            ExecutionEngine.EventLogger.Write("SelectStage:ShowOrderFormingScreen");
            CheckDisposedState();

            foreach (ThumbnailItem item in _selectScreen.ListControl.List.Items)
            {
                if (item.Checked && !_orderFormingScreen.ListControl.List.Items.Contains(item))
                {
                    item.HostList = _orderFormingScreen.ListControl.List;
                    _orderFormingScreen.ListControl.List.AddItem(item);

                    item.Order.Reset();

                    _currentOrder.OrderItems.Add(item.Order);
                }
                else if (!item.Checked && _orderFormingScreen.ListControl.List.Items.Contains(item))
                {
                    _orderFormingScreen.ListControl.List.RemoveItemWithoutUIUpdate(item);

                    _currentOrder.OrderItems.Remove(item.Order);
                    item.Order.Reset();
                }
            }

            _orderFormingScreen.ListControl.WasInitialized = false;

            if (_orderFormingScreen.ListControl.List.Items.Count > 0)
            {
                LastVisitedPage = _orderFormingScreen;
                Engine.ExecuteCommand(new SwitchToScreenCommand(_orderFormingScreen));
            }
            else
            {
                MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageCheckSomePhotosKey]);
            }
        }

        public void SwitchToAdditionalServicesScreen()
        {
            LastVisitedPage = _additionalServicesScreen;
            Engine.ExecuteCommand(new SwitchToScreenCommand(_additionalServicesScreen));
        }

        public void SwitchToFindingPhotosSage()
        {
            CheckDisposedState();
            Engine.ExecuteCommand(new SwitchToStageCommand(Constants.FindingPhotosStageName));
        }

        public void SwitchToProcessOrderStage()
        {
            CheckDisposedState();
            if (Engine.PrimaryAction == PrimaryActionType.BurnCd)
            {
                foreach (ThumbnailItem item in _selectScreen.ListControl.List.Items)
                {
                    if (item.Checked && !_currentOrder.OrderItems.Contains(item.Order))
                    {
                        item.Order.Reset();
                        _currentOrder.OrderItems.Add(item.Order);
                    }
                    else if (!item.Checked && _currentOrder.OrderItems.Contains(item.Order))
                    {
                        _currentOrder.OrderItems.Remove(item.Order);
                        item.Order.Reset();
                    }
                }

                if (_currentOrder.OrderItems.Count > 0)
                    Engine.ExecuteCommand(new SwitchToStageCommand(Constants.ProcessStageName));
                else
                    MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageCheckSomePhotosKey]);
            }
            else
            {
                if (_currentOrder.GetTotalCost() >= ExecutionEngine.PriceManager.MinimumCost.Value)
                    Engine.ExecuteCommand(new SwitchToStageCommand(Constants.ProcessStageName));
                else
                    MessageDialog.Show(string.Format(
                        (string)ExecutionEngine.Instance.Resource[Constants.MessageMinimumCostKey],
                        _currentOrder.GetTotalCost().ToString("c", NumberFormatInfo.CurrentInfo),
                        ExecutionEngine.PriceManager.MinimumCost.Value.ToString("c", NumberFormatInfo.CurrentInfo)));
            }
        }

        public Order CurrentOrder
        {
            get
            {
                CheckDisposedState();
                return _currentOrder;
            }
        }

        #endregion [Public methods and props]

        #region [Private methods]

        private void SelectScreenLoadedHandler(object sender, RoutedEventArgs e)
        {
            CheckDisposedState();
            ThumbnailList list = _selectScreen.ListControl.List;

            if (ExecutionEngine.Context.Contains(Constants.PhotosToRemove))
            {
                System.Collections.Generic.List<string> filesToRemove = (System.Collections.Generic.List<string>)ExecutionEngine.Context[Constants.PhotosToRemove];

                list.Items.
                    Where(item => filesToRemove.Contains(item.Photo.SourceFileName)).
                    ToList().
                    ForEach(item => list.RemoveItem(item, false));

                ExecutionEngine.Context.Remove(Constants.PhotosToRemove);
            }

            if (ExecutionEngine.Context.Contains(Constants.FoundPhotos))
            {
                var foundPhotos = (System.Collections.ObjectModel.Collection<PhotoItem>)ExecutionEngine.Context[Constants.FoundPhotos];
                var newPhotosList = foundPhotos.Where(photo => !list.Items.Any(item => photo.SourceFileName.Equals(item.Photo.SourceFileName))).ToList<PhotoItem>();
                var newPhotosCollection = new System.Collections.ObjectModel.Collection<PhotoItem>(newPhotosList);
                list.LoadItems(newPhotosCollection);
                ExecutionEngine.Context.Remove(Constants.FoundPhotos);
            }
        }

        #endregion [Private methods]

        #region [Variables]

        private SelectScreen _selectScreen;
        private OrderFormingScreen _orderFormingScreen;
        private AdditionalServicesScreen _additionalServicesScreen;
        private Order _currentOrder;
        private bool _isDisposed;

        #endregion [Variables]
    }
}