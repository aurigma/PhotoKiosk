// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;

namespace Aurigma.PhotoKiosk
{
    public interface IThumbnailListFilter
    {
        bool Includes(ThumbnailItem item);

        void Exclude(ThumbnailItem item);
    }

    public class ItemCheckedFilter : IThumbnailListFilter
    {
        public ItemCheckedFilter(bool itemChecked)
        {
            _itemChecked = itemChecked;
        }

        public bool Includes(ThumbnailItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return item.Checked == _itemChecked;
        }

        public void Exclude(ThumbnailItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            item.Checked = !_itemChecked;
        }

        private bool _itemChecked;
    }

    public class PaperSizeFilter : IThumbnailListFilter
    {
        public PaperSizeFilter(PaperFormat paperFormat)
        {
            if (paperFormat != null && ExecutionEngine.Instance.PaperFormats.Contains(paperFormat))
                _paperFormat = paperFormat;
        }

        public bool Includes(ThumbnailItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return (_paperFormat != null && ExecutionEngine.Instance.PaperFormats.Contains(_paperFormat) && item.Order.GetCount(_paperFormat) > 0);
        }

        public void Exclude(ThumbnailItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (_paperFormat != null)
                item.Order.SetCount(_paperFormat, 0);
        }

        public PaperFormat PaperFormat
        {
            get { return _paperFormat; }
        }

        private PaperFormat _paperFormat = null;
    }

    public class ApplyAllFilter : IThumbnailListFilter
    {
        public bool Includes(ThumbnailItem item)
        {
            return true;
        }

        public void Exclude(ThumbnailItem item)
        {
        }
    }

    public class ThumbnailList : IDisposable
    {
        public ThumbnailList(string itemStyleName, ThumbnailListControl hostControl)
        {
            _items = new List<ThumbnailItem>();
            _filteredItems = new List<ThumbnailItem>();
            _visibleItems = new ObservableCollection<ThumbnailItem>();

            _controlSize = new Size();
            _thumbnailSize = new Size();
            _lastAppliedFilter = new ApplyAllFilter();

            _itemsPerRow = 4;
            _itemsPerColumn = 2;

            _thumbnailsLoader = new AsyncThumbnailsLoader(_filteredItems, this);
            ExecutionEngine.EventLogger.Write("ThumbnailList created");

            _itemStyleName = itemStyleName;
            _hostControl = hostControl;
        }

        ~ThumbnailList()
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
                _isDisposed = true;

                ClearAll();

                _thumbnailsLoader.Dispose();
                _thumbnailsLoader = null;
                _visibleItems = null;
                _filteredItems = null;
                _items = null;

                _hostControl = null;
            }
        }

        private void CheckDisposedState()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("ThumbnailList");
        }

        public event EventHandler ItemChanged;

        public void LoadItems(Collection<PhotoItem> items)
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:LoadItems");
            CheckDisposedState();

            foreach (PhotoItem item in items)
            {
                ThumbnailItem newItem = new ThumbnailItem(item);
                newItem.HostList = this;
                _items.Add(newItem);
            }
        }

        private void ItemContentChangedHandler(object sender, EventArgs e)
        {
            CheckDisposedState();
            UpdateHostFilterControl();

            FireListItemContentChanged(EventArgs.Empty);
        }

        private void FireListItemContentChanged(EventArgs e)
        {
            if (ItemChanged != null)
            {
                ItemChanged(this, e);
            }
        }

        public int ApplyFilter(IThumbnailListFilter filter)
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:ApplyFilter");
            CheckDisposedState();

            _thumbnailsLoader.Stop();
            _filteredItems.Clear();

            foreach (ThumbnailItem item in _items)
            {
                if (filter.Includes(item))
                {
                    _filteredItems.Add(item);
                }
            }

            _lastAppliedFilter = filter;

            _firstVisibleItemIndex = 0;

            return _filteredItems.Count;
        }

        public void ApplyLastFilter()
        {
            CheckDisposedState();
            ApplyFilter(_lastAppliedFilter);
        }

        public void UpdateHostFilterControl()
        {
            CheckDisposedState();

            if (_hostControl != null)
            {
                _hostControl.UpdateFilterControls();
                _hostControl.UpdateTotalSizeInfo();
            }
        }

        public void CurrentPageCheckAll()
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:CurrentPageCheckAll");
            CheckDisposedState();

            foreach (ThumbnailItem item in _visibleItems)
                item.SetCheckedProperty(true);

            UpdateAfterBatchAction();
        }

        public void CurrentPageUncheckAll()
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:CurrentPageUncheckAll");
            CheckDisposedState();

            foreach (ThumbnailItem item in _visibleItems)
                item.SetCheckedProperty(false);

            UpdateAfterBatchAction();
        }

        public void CheckAll()
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:CheckAll");
            CheckDisposedState();

            /*
             *      Check all logic: if there is at least one unchecked item, all items will be checked.
             *      If all items are checked - they all will be unchecked.
             */

            bool isThereUncheckedItems = false;

            foreach (ThumbnailItem item in _filteredItems)
            {
                isThereUncheckedItems = !item.Checked;
                if (isThereUncheckedItems)
                {
                    break;
                }
            }

            foreach (ThumbnailItem item in _filteredItems)
            {
                item.SetCheckedProperty(isThereUncheckedItems);
            }

            UpdateAfterBatchAction();
        }

        public void UncheckAll()
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:UncheckAll");
            CheckDisposedState();

            foreach (ThumbnailItem item in _filteredItems)
                item.SetCheckedProperty(false);

            UpdateAfterBatchAction();
        }

        public void AddItem(ThumbnailItem item)
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:AddItem");
            CheckDisposedState();

            _items.Add(item);
            item.ContentChanged += this.ItemContentChangedHandler;
        }

        public void RemoveItemWithoutUIUpdate(ThumbnailItem item)
        {
            item.ContentChanged += this.ItemContentChangedHandler;

            if (_items.Contains(item))
            {
                _items.Remove(item);
            }
        }

        public void RemoveItem(ThumbnailItem item, bool fromFilteredScopeOnly)
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:RemoveItem");
            CheckDisposedState();

            if (!fromFilteredScopeOnly || (_lastAppliedFilter is ApplyAllFilter && fromFilteredScopeOnly))
            {
                if (_items.Contains(item))
                {
                    _items.Remove(item);
                }

                item.Checked = false;

                Order currentOrder = (Order)ExecutionEngine.Context[Constants.OrderContextName];
                if (currentOrder.OrderItems.Contains(item.Order))
                {
                    currentOrder.OrderItems.Remove(item.Order);
                }

                item.ContentChanged -= this.ItemContentChangedHandler;
            }
            else
            {
                _lastAppliedFilter.Exclude(item);
            }

            if (_filteredItems.Contains(item))
            {
                _filteredItems.Remove(item);
            }

            UpdateAfterItemRemoval(item);
        }

        public void RemoveItemAfterCheck(ThumbnailItem item)
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:RemoveItemAfterCheck");
            CheckDisposedState();

            UpdateHostFilterControl();

            if (_lastAppliedFilter is ApplyAllFilter)
                return;

            if (_filteredItems.Contains(item))
                _filteredItems.Remove(item);

            UpdateAfterItemRemoval(item);
        }

        public void StopReadingItems()
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:StopReadingItems");
            CheckDisposedState();
            _thumbnailsLoader.Stop();
        }

        public void Refresh()
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:Refresh");
            CheckDisposedState();

            _visibleItems.Clear();
            for (int i = _firstVisibleItemIndex; i < Math.Min(_firstVisibleItemIndex + ItemsPerScreen, _filteredItems.Count); i++)
            {
                _filteredItems[i].Width = _thumbnailSize.Width;
                _filteredItems[i].Height = _thumbnailSize.Height;
                _filteredItems[i].HostList = this;
                _visibleItems.Add(_filteredItems[i]);
            }
        }

        public void ClearAll()
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:ClearAll");
            CheckDisposedState();

            foreach (ThumbnailItem item in _items)
            {
                item.ContentChanged -= this.ItemContentChangedHandler;
            }

            _thumbnailsLoader.Clear();
            _visibleItems.Clear();
            _filteredItems.Clear();
            _items.Clear();

            _firstVisibleItemIndex = 0;
        }

        public void SetItemsLayout(int itemsPerRow, int itemsPerColumn)
        {
            ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "ThumbnailList:ChangePhotosLayout rows = {0}, columns = {1}", itemsPerRow, itemsPerColumn));
            CheckDisposedState();

            _itemsPerRow = itemsPerRow;
            _itemsPerColumn = itemsPerColumn;
            UpdateThumbnailsSize();
            Refresh();
            _thumbnailsLoader.ReloadItems();
        }

        public bool GoToPageByIndex(int index)
        {
            CheckDisposedState();

            if (index * _itemsPerColumn * _itemsPerRow < _filteredItems.Count)
            {
                _firstVisibleItemIndex = index * _itemsPerColumn * _itemsPerRow;
                Refresh();
                _thumbnailsLoader.ReloadItems();
                return true;
            }

            return false;
        }

        public void GoToPageContainigItem(ThumbnailItem item)
        {
            int itemIndex = _filteredItems.IndexOf(item);
            if (itemIndex >= 0 && itemIndex < _filteredItems.Count)
            {
                int pageIndex = itemIndex / (_itemsPerColumn * _itemsPerRow);
                if (pageIndex >= 0 && pageIndex <= MaxPageIndex)
                {
                    GoToPageByIndex(pageIndex);
                }
            }
        }

        public ThumbnailItem GetActiveItem()
        {
            foreach (ThumbnailItem item in _items)
            {
                if (item.IsActive())
                    return item;
            }
            return null;
        }

        public ObservableCollection<ThumbnailItem> VisibleItems
        {
            get
            {
                CheckDisposedState();
                return _visibleItems;
            }
        }

        public int ItemsPerScreen
        {
            get
            {
                CheckDisposedState();
                return _itemsPerRow * _itemsPerColumn;
            }
        }

        public int FirstVisibleItemIndex
        {
            get
            {
                CheckDisposedState();
                return _firstVisibleItemIndex;
            }
        }

        public Size ControlSize
        {
            get
            {
                CheckDisposedState();
                return _controlSize;
            }
            set
            {
                CheckDisposedState();
                _controlSize = value;

                UpdateThumbnailsSize();
            }
        }

        public Size ThumbnailSize
        {
            get
            {
                CheckDisposedState();
                return _thumbnailSize;
            }
        }

        public IList<ThumbnailItem> Items
        {
            get
            {
                CheckDisposedState();
                return _items;
            }
        }

        public IList<ThumbnailItem> FilteredItems
        {
            get
            {
                CheckDisposedState();
                return _filteredItems;
            }
        }

        public string ItemStyleName
        {
            get
            {
                CheckDisposedState();
                return _itemStyleName;
            }
        }

        public int CurrentPageIndex
        {
            get
            {
                CheckDisposedState();
                return _firstVisibleItemIndex / (_itemsPerRow * _itemsPerColumn);
            }
        }

        public int MaxPageIndex
        {
            get
            {
                CheckDisposedState();
                return (int)(_filteredItems.Count / (_itemsPerRow * _itemsPerColumn)) + (_filteredItems.Count % (_itemsPerRow * _itemsPerColumn) > 0 ? 1 : 0);
            }
        }

        public ThumbnailListControl HostControl
        {
            get
            {
                CheckDisposedState();
                return _hostControl;
            }
        }

        public IThumbnailListFilter LastAppliedFilter
        {
            get
            {
                CheckDisposedState();
                return _lastAppliedFilter;
            }
        }

        private void UpdateThumbnailsSize()
        {
            ExecutionEngine.EventLogger.Write("ThumbnailList:UpdateThumbnailSize");
            if (_itemsPerColumn > 0 && _itemsPerRow > 0)
            {
                _thumbnailSize.Height = (int)((_controlSize.Height) / _itemsPerColumn);
                _thumbnailSize.Width = (int)((_controlSize.Width - Constants.ScrollControlWidth) / _itemsPerRow);
            }
        }

        private void UpdateAfterBatchAction()
        {
            ApplyLastFilter();
            UpdateHostFilterControl();

            if (_hostControl != null)
            {
                _hostControl.UpdatePageIndexList();
            }
        }

        private void UpdateAfterItemRemoval(ThumbnailItem item)
        {
            item.ShowRemoveAnimation(RemoveItemAnimationCompletedHandler);
        }

        private void UpdateHostUI()
        {
            if (CurrentPageIndex >= MaxPageIndex)
            {
                GoToPageByIndex(System.Math.Max(0, MaxPageIndex - 1));
            }
            else
            {
                Refresh();
                _thumbnailsLoader.ReloadItems();
            }

            if (_hostControl != null)
            {
                if (_filteredItems.Count % ItemsPerScreen == 0)
                    _hostControl.UpdatePageIndexList();

                _hostControl.UpdateFilterControls();
                _hostControl.UpdatePagesInfo();
                _hostControl.UpdateTotalSizeInfo();
            }

            FireListItemContentChanged(EventArgs.Empty);
        }

        private void RemoveItemAnimationCompletedHandler(object sender, EventArgs e)
        {
            UpdateHostUI();
        }

        private Size _controlSize;
        private Size _thumbnailSize;
        private IThumbnailListFilter _lastAppliedFilter;

        private int _itemsPerRow;
        private int _itemsPerColumn;
        private int _firstVisibleItemIndex;
        private string _itemStyleName;

        private List<ThumbnailItem> _items;
        private List<ThumbnailItem> _filteredItems;
        private ObservableCollection<ThumbnailItem> _visibleItems;

        private AsyncThumbnailsLoader _thumbnailsLoader;
        private ThumbnailListControl _hostControl;

        private bool _isDisposed;
    }
}