// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Aurigma.PhotoKiosk
{
    public class FilterTab
    {
        public static FilterTab GetFilterTab(RadioButton button)
        {
            return (FilterTab)_filterStorage[button];
        }

        private static Hashtable _filterStorage = new Hashtable();

        public FilterTab(IThumbnailListFilter filter, string filterName, RadioButton filterButton)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            if (filterName == null)
                throw new ArgumentNullException("filterName");
            if (filterButton == null)
                throw new ArgumentNullException("filterButton");

            _filter = filter;
            _filterName = filterName;
            _filterButton = filterButton;
            _curPageIndex = 0;

            _filterStorage.Add(_filterButton, this);
        }

        public IThumbnailListFilter Filter
        {
            get { return _filter; }
        }

        public RadioButton FilterButton
        {
            get { return _filterButton; }
        }

        private int _curPageIndex;

        internal int CurrentPageIndex
        {
            get { return _curPageIndex; }
            set { _curPageIndex = value; }
        }

        private bool _enabled = true;

        internal bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        private IThumbnailListFilter _filter;
        private string _filterName;
        private RadioButton _filterButton;

        public static int MaxTabWidth = 200;
        public static int MinTabWidth = 90;
    }

    public partial class ThumbnailListControl : UserControl, IDisposable
    {
        #region [DependencyProperties]

        public static readonly DependencyProperty PageNumbersOffsetProperty =
            DependencyProperty.Register(
                "PageNumbersOffset",
                typeof(double),
                typeof(ThumbnailListControl),
                new FrameworkPropertyMetadata((double)0, PageNumbersOffsetChangedHandler, PageNumbersOffsetCheckHandler)
                );

        public static readonly DependencyProperty CenterItemOffsetProperty =
            DependencyProperty.Register(
                "CenterItemOffset",
                typeof(double),
                typeof(ThumbnailListControl),
                new FrameworkPropertyMetadata((double)0.0)
                );

        public static readonly DependencyProperty CurrentPageNumbersOffsetProperty =
            DependencyProperty.Register(
                "CurrentPageNumbersOffset",
                typeof(double),
                typeof(ThumbnailListControl),
                new FrameworkPropertyMetadata((double)0.0)
                );

        public static readonly DependencyProperty PagesInfoTextProperty =
            DependencyProperty.Register(
                "PagesInfoText",
                typeof(string),
                typeof(ThumbnailListControl),
                new FrameworkPropertyMetadata("0")
                );

        private static object PageNumbersOffsetCheckHandler(DependencyObject target, object value)
        {
            double offset = (double)value;

            System.Windows.Controls.Primitives.IScrollInfo scrollInfo = ((ThumbnailListControl)target)._listBoxHolder as System.Windows.Controls.Primitives.IScrollInfo;

            if (offset < 0)
            {
                offset = 0;
            }
            else if (offset > scrollInfo.ExtentWidth - scrollInfo.ViewportWidth)
            {
                offset = scrollInfo.ExtentWidth - scrollInfo.ViewportWidth;
            }
            return offset;
        }

        private static void PageNumbersOffsetChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ThumbnailListControl control = (ThumbnailListControl)sender;
            control.CurrentPageNumbersOffset = (double)e.NewValue;
        }

        #endregion [DependencyProperties]

        #region [Events]

        public event EventHandler ListEmpty;

        #endregion [Events]

        #region [Construction / destruction]

        public ThumbnailListControl(string styleName)
        {
            InitializeComponent();

            _filterTabs = new List<FilterTab>();
            _showEmptyFilterTabs = true;
            _list = new ThumbnailList(styleName, this);

            Binding thumbListBinding = new Binding();
            thumbListBinding.Source = _list.VisibleItems;

            _thumbListbox.SetBinding(ListBox.ItemsSourceProperty, thumbListBinding);
            _thumbListbox.ItemContainerStyleSelector = new SeparatorStyleSelector(_list);

            if (_listBoxHolder is IScrollInfo)
            {
                ((IScrollInfo)_listBoxHolder).CanHorizontallyScroll = true;
                ((IScrollInfo)_listBoxHolder).ScrollOwner = _scrollViewer;
            }

            _list.ItemChanged += new EventHandler(ListItemContentChangedHandler);
        }

        protected override void OnInitialized(EventArgs e)
        {
            this.Loaded += ThumbnailListControlLoadedHandler;
            this.Unloaded += ThumbnailListControlUnloadedHandler;

            base.OnInitialized(e);
        }

        ~ThumbnailListControl()
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
                if (_list != null)
                {
                    _list.Dispose();
                    _list = null;
                }

                _isDisposed = true;
            }
        }

        private void CheckDisposedState()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("ThumbnailListControl");
        }

        #endregion [Construction / destruction]

        #region [Event handlers]

        private void ThumbnailListControlLoadedHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
                _totalSizePanel.Visibility = Visibility.Visible;
            else
                _totalSizePanel.Visibility = Visibility.Collapsed;

            if (!_wasInitialized)
            {
                System.Drawing.Size thumbListboxSize = new System.Drawing.Size((int)_thumbListbox.ActualWidth, (int)ActualHeight);

                thumbListboxSize.Height = (int)(_listControlHolder.ActualHeight - _indexListboxHolder.ActualHeight);

                _list.ControlSize = thumbListboxSize;

                if (_filterTabs.Count > 0)
                {
                    UpdateFilterControls();
                    UpdateTotalSizeInfo();

                    bool isActivated = false;

                    if (_filterTabs[1].Filter is PaperSizeFilter)
                    {
                        for (int i = 1; i < _filterTabs.Count; i++)
                        {
                            if (_filterTabs[i].Enabled)
                            {
                                ActivateFilterTab(_filterTabs[i]);
                                isActivated = true;
                                break;
                            }
                        }
                    }

                    if (!isActivated)
                        ActivateFilterTab(_filterTabs[0]);
                }
                else
                {
                    ExecutionEngine.EventLogger.Write("ThumbnailListControl:ThumbnailListControlLoadedHandler _filterTabs.Count == 0");
                }
                _wasInitialized = true;
            }
            else
            {
                ActivateCurrentTab();
            }
        }

        private void ThumbnailListControlUnloadedHandler(object sender, RoutedEventArgs e)
        {
            _list.StopReadingItems();
        }

        private void PageIndexChangeHandler(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null)
            {
                int index = listBox.Items.IndexOf(listBox.SelectedItem);
                GoToPageByIndex(index);
            }
        }

        private void CheckAllButtonClick(object sender, EventArgs e)
        {
            List.CheckAll();
        }

        private void FilterTabButtonClickHandler(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button != null)
            {
                FilterTab filterTab = FilterTab.GetFilterTab(button);
                ActivateFilterTab(filterTab);
            }
        }

        #endregion [Event handlers]

        #region [Public Methods and props]

        public void AddFilterTab(IThumbnailListFilter filter, string filterName)
        {
            CheckDisposedState();

            var text = new TextBlock();
            text.TextTrimming = TextTrimming.CharacterEllipsis;
            text.Text = filterName;

            var filterButton = new RadioButton();
            filterButton.Content = text;
            filterButton.Click += FilterTabButtonClickHandler;
            filterButton.Style = (Style)FindResource("TabStyle");

            _filterTabs.Add(new FilterTab(filter, filterName, filterButton));
        }

        public void ClearFilterTabs()
        {
            _filterTabs.Clear();
        }

        public void UpdateFilterControls()
        {
            CheckDisposedState();

            if (_showEmptyFilterTabs)
            {
                if (_filterTabs.Count != _filterTabsPanel.Children.Count)
                {
                    _filterTabsPanel.Children.Clear();
                    foreach (FilterTab filterTab in _filterTabs)
                    {
                        filterTab.FilterButton.MaxWidth = _filterTabsPanel.ActualWidth / _filterTabs.Count - 1;
                        filterTab.Enabled = true;
                        _filterTabsPanel.Children.Add(filterTab.FilterButton);
                    }
                }
            }
            else
            {
                _filterTabsPanel.Children.Clear();
                bool needToSwitchFilter = false;

                foreach (FilterTab filterTab in _filterTabs)
                {
                    if (filterTab.Filter is ApplyAllFilter)
                    {
                        _filterTabsPanel.Children.Add(filterTab.FilterButton);
                        filterTab.Enabled = true;
                    }
                    else
                    {
                        int itemCount = 0;
                        for (int i = 0; i < _list.Items.Count && itemCount == 0; i++)
                        {
                            if (filterTab.Filter.Includes(_list.Items[i]))
                                itemCount++;
                        }

                        if (itemCount > 0)
                        {
                            _filterTabsPanel.Children.Add(filterTab.FilterButton);
                            filterTab.Enabled = true;
                        }
                        else if (filterTab == _currentFilterTab)
                        {
                            needToSwitchFilter = true;
                            _currentFilterTab.FilterButton.IsChecked = false;
                            filterTab.Enabled = false;
                        }
                        else
                            filterTab.Enabled = false;
                    }
                }

                // Setting Max button widths
                double width = Math.Min(Math.Max(_filterTabsPanel.ActualWidth / _filterTabsPanel.Children.Count, FilterTab.MinTabWidth), FilterTab.MaxTabWidth);
                foreach (UIElement child in _filterTabsPanel.Children)
                    (child as FrameworkElement).MaxWidth = width;

                UpdateTabsVisibility();

                if (needToSwitchFilter)
                    ActivateFilterTab(_filterTabs[0]);
            }
        }

        public void UpdateTabsVisibility()
        {
            CheckDisposedState();
            double totalWidth = 0;
            foreach (UIElement child in _filterTabsPanel.Children)
                totalWidth += (child as FrameworkElement).MaxWidth;

            if (totalWidth > _filterTabsPanel.ActualWidth)
            {
                int visibleTabsCount = (int)(_filterTabsPanel.ActualWidth / ((_filterTabsPanel.Children[0] as FrameworkElement).MaxWidth));
                int centerTabIndex = (int)((double)visibleTabsCount / 2 + 0.5);

                int curIndex = _filterTabsPanel.Children.IndexOf(_currentFilterTab.FilterButton);
                for (int i = 0; i < _filterTabsPanel.Children.Count; i++)
                {
                    if ((i <= curIndex - centerTabIndex && i + visibleTabsCount < _filterTabsPanel.Children.Count) ||
                        (i >= curIndex + centerTabIndex && i - visibleTabsCount >= 0))
                        _filterTabsPanel.Children[i].Visibility = Visibility.Collapsed;
                    else
                        _filterTabsPanel.Children[i].Visibility = Visibility.Visible;
                }
            }
        }

        public void UpdateTotalSizeInfo()
        {
            CheckDisposedState();
            int selectedItemsCount = 0;
            long totalSizeInBytes = 0;
            foreach (ThumbnailItem item in _list.Items)
            {
                if (item.Checked)
                {
                    selectedItemsCount++;
                    var file = new FileInfo(item.Photo.SourceFileName);
                    totalSizeInBytes += file.Length;
                    file = null;
                }
            }

            _photosCountLabel.Text = selectedItemsCount.ToString();

            string formattedSize = "";
            if (totalSizeInBytes < 1048576)
                formattedSize = string.Format("{0:F2}{1}", (float)totalSizeInBytes / 1024.0, (string)FindResource(Constants.SelectStepKBLabelTextKey));
            else if (totalSizeInBytes < 1073741824)
                formattedSize = string.Format("{0:F2}{1}", (float)totalSizeInBytes / 1048576.0, (string)FindResource(Constants.SelectStepMBLabelTextKey));
            else
                formattedSize = string.Format("{0:F2}{1}", (float)totalSizeInBytes / 1073741824.0, (string)FindResource(Constants.SelectStepGBLabelTextKey));
            _totalSizeLabel.Text = formattedSize;
        }

        public void UpdatePageIndexList()
        {
            CheckDisposedState();

            int i;
            if (_listbox.Items.Count < _list.MaxPageIndex)
            {
                for (i = _listbox.Items.Count + 1; i <= _list.MaxPageIndex; i++)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = new OutlinedText(i.ToString(), (Style)this.FindResource("PageNumberTextStyle"));
                    item.Style = (Style)this.FindResource("IndexListBoxItemStyle");
                    _listbox.Items.Add(item);
                }
            }

            for (i = 0; i < _list.MaxPageIndex; i++)
                ((ListBoxItem)_listbox.Items[i]).Visibility = Visibility.Visible;
            for (; i < _listbox.Items.Count; i++)
                ((ListBoxItem)_listbox.Items[i]).Visibility = Visibility.Collapsed;

            _listbox.SelectedIndex = _list.CurrentPageIndex;
            _itemIndexForCenterOffset = -1;
            UpdateCenterOffset();
        }

        public void UpdatePagesInfo()
        {
            CheckDisposedState();

            PagesInfoText = Parser.CreateFormatString(
                    (string)TryFindResource(Constants.PagesInfoKey),
                    new string[2]
                        {
                            List.FilteredItems.Count.ToString(CultureInfo.InvariantCulture),
                            List.MaxPageIndex.ToString(CultureInfo.InvariantCulture)
                        }
                    );
        }

        public ThumbnailList List
        {
            get
            {
                CheckDisposedState();
                return _list;
            }
        }

        public void SetItemsLayout(int itemsPerRow, int itemsPerColumn)
        {
            CheckDisposedState();
            _list.SetItemsLayout(itemsPerRow, itemsPerColumn);

            UpdatePageIndexList();
        }

        public double CurrentPageNumbersOffset
        {
            get
            {
                CheckDisposedState();
                return (double)GetValue(CurrentPageNumbersOffsetProperty);
            }
            set
            {
                CheckDisposedState();

                double offset = value;
                if (offset < 0)
                    offset = 0;

                SetValue(CurrentPageNumbersOffsetProperty, offset);
                SetPageNumbersOffset(offset);
            }
        }

        public double CenterItemOffset
        {
            get
            {
                CheckDisposedState();
                return (double)GetValue(CenterItemOffsetProperty);
            }
            set
            {
                CheckDisposedState();
                SetValue(CenterItemOffsetProperty, value);
            }
        }

        public bool ShowEmptyFilterTabs
        {
            get
            {
                CheckDisposedState();
                return _showEmptyFilterTabs;
            }
            set
            {
                CheckDisposedState();
                _showEmptyFilterTabs = value;
            }
        }

        public string PagesInfoText
        {
            get
            {
                CheckDisposedState();
                return (string)GetValue(PagesInfoTextProperty);
            }
            set
            {
                CheckDisposedState();
                SetValue(PagesInfoTextProperty, value);
            }
        }

        public bool ShowSelectAllButton
        {
            get
            {
                CheckDisposedState();
                return _selectAllButton.Visibility == Visibility.Visible;
            }
            set
            {
                CheckDisposedState();
                if (value)
                    _selectAllButton.Visibility = Visibility.Visible;
                else
                    _selectAllButton.Visibility = Visibility.Collapsed;
            }
        }

        public bool WasInitialized
        {
            get
            {
                CheckDisposedState();
                return _wasInitialized;
            }
            set
            {
                CheckDisposedState();
                _wasInitialized = value;
            }
        }

        #endregion [Public Methods and props]

        private Point _dragStartPoint;
        private Canvas _currentCanvas;

        private static bool EnableMoving()
        {
            return ((ExecutionEngine.Context[Constants.OrderContextName] as Order).CropMode == Constants.CropToFillModeName && ExecutionEngine.Instance.SelectedPaperFormat != null && !ExecutionEngine.Instance.SelectedPaperFormat.IsFree);
        }

        public void MouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (EnableMoving())
            {
                _dragStartPoint = e.GetPosition(sender as Canvas);
                _currentCanvas = sender as Canvas;
            }
        }

        public void MouseMoveEventHandler(object sender, MouseEventArgs e)
        {
            if (EnableMoving() && e.LeftButton == MouseButtonState.Pressed)
            {
                Canvas canvas = sender as Canvas;
                if (_currentCanvas != null && _currentCanvas == canvas && canvas.Children[0] is Image)
                {
                    Image image = canvas.Children[0] as Image;
                    Point p = e.GetPosition(canvas);

                    double deltaX = p.X - _dragStartPoint.X;
                    double deltaY = p.Y - _dragStartPoint.Y;

                    if (Math.Abs(deltaX) >= 1 || Math.Abs(deltaY) >= 1)
                    {
                        double x = Canvas.GetLeft(image) + deltaX;
                        double y = Canvas.GetTop(image) + deltaY;

                        x = Math.Min(0, Math.Max(x, canvas.ActualWidth - image.Width));
                        y = Math.Min(0, Math.Max(y, canvas.ActualHeight - image.Height));

                        ThumbnailItem item = List.GetActiveItem();
                        item.ImageLeft = x;
                        item.ImageTop = y;

                        Canvas.SetLeft(image, x);
                        Canvas.SetTop(image, y);
                        _dragStartPoint = p;
                    }
                }
            }
        }

        public void MouseUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (EnableMoving())
                _currentCanvas = null;
        }

        #region [Private methods]

        private void SetPageNumbersOffset(double offset)
        {
            if (_listBoxHolder != null && _listBoxHolder is IScrollInfo)
            {
                ((IScrollInfo)_listBoxHolder).SetHorizontalOffset(offset);
                UpdateScrollButtons(offset);
            }
        }

        private void UpdateCenterOffset()
        {
            if (_itemIndexForCenterOffset == _listbox.SelectedIndex)
                return;

            double offset = 0;
            for (int i = 0; i < _listbox.SelectedIndex; i++)
            {
                ListBoxItem item = (ListBoxItem)_listbox.Items[i];
                offset += item.Margin.Left + item.Margin.Right + item.ActualWidth;
            }

            offset -= (double)_listBoxHolder.ViewportWidth / 2.0;
            if (offset < 0)
                offset = 0;

            UpdateScrollButtons(offset);
            SetValue(CenterItemOffsetProperty, offset);
            _itemIndexForCenterOffset = _listbox.SelectedIndex;
        }

        private void GoToPageByIndex(int index)
        {
            if (index >= 0 && index < _list.MaxPageIndex)
            {
                UpdateCenterOffset();
                _list.GoToPageByIndex(index);
            }
        }

        private void ActivateFilterTab(FilterTab filterTab)
        {
            if (filterTab != null)
            {
                if (_currentFilterTab != null)
                    _currentFilterTab.CurrentPageIndex = _list.CurrentPageIndex;

                _currentFilterTab = filterTab;

                if (filterTab.Filter is PaperSizeFilter)
                {
                    PaperSizeFilter filter = filterTab.Filter as PaperSizeFilter;
                    ExecutionEngine.Instance.SelectedPaperFormat = filter.PaperFormat;
                }
                else
                    ExecutionEngine.Instance.SelectedPaperFormat = null;

                _list.ApplyFilter(_currentFilterTab.Filter);

                if (_list.FilteredItems.Count == 0 && !(filterTab.Filter is ApplyAllFilter))
                    ActivateFilterTab(_filterTabs[0]);

                if (_currentFilterTab.CurrentPageIndex <= _list.MaxPageIndex)
                    GoToPageByIndex(_currentFilterTab.CurrentPageIndex);
                else
                    GoToPageByIndex(_list.CurrentPageIndex);

                UpdatePageIndexList();
                UpdatePagesInfo();
                UpdateTabsVisibility();

                _currentFilterTab.FilterButton.IsChecked = true;
            }
        }

        private void ActivateCurrentTab()
        {
            if (_currentFilterTab == null)
                return;

            int oldPageIndex = _list.CurrentPageIndex;

            _list.ApplyLastFilter();

            if (_list.FilteredItems.Count == 0)
                ActivateFilterTab(_filterTabs[0]);

            GoToPageByIndex(_list.MaxPageIndex < oldPageIndex ? 0 : oldPageIndex);

            UpdatePageIndexList();

            this.UpdateScrollButtons(CurrentPageNumbersOffset);

            UpdatePagesInfo();
            UpdateFilterControls();
            UpdateTotalSizeInfo();
        }

        private double GetListBoxItemWidth()
        {
            if (_listbox.Items.Count > 0 && _listbox.Items[0] is ListBoxItem)
            {
                return ((ListBoxItem)_listbox.Items[0]).ActualWidth;
            }
            else
            {
                return Constants.DefaultListBoxItemWidth;
            }
        }

        private void UpdateScrollButtons(double horizontalOffset)
        {
            IScrollInfo scrollInfo = (IScrollInfo)_listBoxHolder;

            // Due to postpone item's stack panel ExtendedWidth changing, we have to get width manually

            double allVisibleItemsWidth = 0;

            foreach (ListBoxItem item in _listbox.Items)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    allVisibleItemsWidth += item.Margin.Left + item.Margin.Right + item.ActualWidth;
                }
            }

            _scrollLeftButton.IsEnabled = horizontalOffset > 0;

            if (scrollInfo != null)
            {
                _scrollRightButton.IsEnabled = horizontalOffset < allVisibleItemsWidth - scrollInfo.ViewportWidth;
            }
        }

        private void ListItemContentChangedHandler(object sender, EventArgs e)
        {
            if (_list.Items.Count == 0)
            {
                FireListIsEmptyEvent();
            }
        }

        private void FireListIsEmptyEvent()
        {
            if (ListEmpty != null)
            {
                ListEmpty(this, EventArgs.Empty);
            }
        }

        private void ScrollRightMouseDownHandler(object sender, RoutedEventArgs e)
        {
            _isScrollRightMouseDown = true;

            Storyboard moveStoryboard = (Storyboard)FindResource("MoveScrollToRight");
            moveStoryboard.Completed += new EventHandler(MoveRightAnimationCompleteHandler);
            moveStoryboard.Begin(this);
        }

        private void ScrollRightMouseUpHandler(object sender, RoutedEventArgs e)
        {
            _isScrollRightMouseDown = false;
        }

        private void ScrollLeftMouseDownHandler(object sender, RoutedEventArgs e)
        {
            _isScrollLeftMouseDown = true;

            Storyboard moveStoryboard = (Storyboard)FindResource("MoveScrollToLeft");
            moveStoryboard.Completed += new EventHandler(MoveLeftAnimationCompleteHandler);
            moveStoryboard.Begin(this);
        }

        private void ScrollLeftMouseUpHandler(object sender, RoutedEventArgs e)
        {
            _isScrollLeftMouseDown = false;
        }

        private void MoveRightAnimationCompleteHandler(object sender, EventArgs e)
        {
            if (_isScrollRightMouseDown)
            {
                Storyboard moveStoryboard = (Storyboard)FindResource("MoveScrollToRight");
                moveStoryboard.Begin(this);
            }
        }

        private void MoveLeftAnimationCompleteHandler(object sender, EventArgs e)
        {
            if (_isScrollLeftMouseDown)
            {
                Storyboard moveStoryboard = (Storyboard)FindResource("MoveScrollToLeft");
                moveStoryboard.Begin(this);
            }
        }

        #endregion [Private methods]

        #region [Variables]

        private ThumbnailList _list;
        private int _itemIndexForCenterOffset;
        private List<FilterTab> _filterTabs;
        private bool _showEmptyFilterTabs;
        private FilterTab _currentFilterTab;

        private bool _wasInitialized;
        private bool _isDisposed;

        private bool _isScrollLeftMouseDown = false;
        private bool _isScrollRightMouseDown = false;

        #endregion [Variables]
    }

    // Because of non interface StyleSelector nature, we have to create separate class with style selection logic
    public class SeparatorStyleSelector : StyleSelector
    {
        public SeparatorStyleSelector(ThumbnailList list)
        {
            _list = list;
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            if (item == null)
                throw new ArgumentNullException("item");

            return (Style)((FrameworkElement)container).FindResource(_list.ItemStyleName);
        }

        private ThumbnailList _list;
    }
}