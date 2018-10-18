// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Aurigma.PhotoKiosk
{
    public partial class OrderInfoControl : UserControl
    {
        public static readonly DependencyProperty TotalTextStyleProperty =
            DependencyProperty.Register(
                "TotalTextStyle",
                typeof(Style),
                typeof(OrderInfoControl),
                new FrameworkPropertyMetadata(null)
                );

        public static readonly DependencyProperty TotalTextContentProperty =
            DependencyProperty.Register(
                "TotalTextContent",
                typeof(string),
                typeof(OrderInfoControl),
                new FrameworkPropertyMetadata("")
            );

        public static readonly DependencyProperty MaxFormatCountProperty =
            DependencyProperty.Register(
                "MaxFormatCount",
                typeof(int),
                typeof(OrderInfoControl),
                new FrameworkPropertyMetadata(int.MaxValue)
            );

        private OrderItem _sourceItem;
        private Order _sourceOrder;

        private Dictionary<PaperFormat, int> _photoCounts = new Dictionary<PaperFormat, int>(ExecutionEngine.Instance.PaperFormats.Count);
        private Dictionary<PaperFormat, TextBlock> _photoCountViews = new Dictionary<PaperFormat, TextBlock>(ExecutionEngine.Instance.PaperFormats.Count);
        private Dictionary<PaperFormat, TextBlock> _photoCostViews = new Dictionary<PaperFormat, TextBlock>(ExecutionEngine.Instance.PaperFormats.Count);
        private Dictionary<PaperFormat, TextBlock> _paperFormatViews = new Dictionary<PaperFormat, TextBlock>(ExecutionEngine.Instance.PaperFormats.Count);

        private OutlinedText _totalCountLabel;
        private OutlinedText _totalCostLabel;
        private OutlinedText _totalOriginalCostLabel;
        private OutlinedText _salesTaxCostLabel;
        private OutlinedText _totalPaymentLabel;

        private Style _nonEmptyItemStyle;
        private Style _emptyItemStyle;

        private bool _showChangeCountButtons = true;
        private bool _showOriginalCostColumn = false;
        private bool _showSalesTaxGrid = false;
        private bool _showServices = false;
        private bool _showPriceColumn = true;
        private bool _isCreated;
        private Order _currentOrder;

        public OrderInfoControl()
        {
            InitializeComponent();

            if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.BurnCd)
            {
                bool firstAdded = false;
                foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                {
                    if (!firstAdded)
                    {
                        _photoCounts.Add(format, 1);
                        firstAdded = true;
                    }
                    else
                        _photoCounts.Add(format, 0);
                }
            }

            _nonEmptyItemStyle = (Style)FindResource("NonEmptyInfoItemStyle");
            _emptyItemStyle = (Style)FindResource("EmptyInfoItemStyle");

            if (ExecutionEngine.Context.Contains(Constants.OrderContextName))
                _currentOrder = (Order)ExecutionEngine.Context[Constants.OrderContextName];

            _isCreated = false;
        }

        private void OrderInfoControlLoadedHandler(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void OrderInfoControlUnloadedHandler(object sender, RoutedEventArgs e)
        {
            ClearControl();
        }

        private void _grid_SizeChanged(object sender, RoutedEventArgs e)
        {
            if (_header.ColumnDefinitions.Count == _grid.ColumnDefinitions.Count &&
                _total.ColumnDefinitions.Count == _grid.ColumnDefinitions.Count &&
                _salesTax.ColumnDefinitions.Count == _grid.ColumnDefinitions.Count)
                for (int i = 0; i < _grid.ColumnDefinitions.Count; i++)
                {
                    _header.ColumnDefinitions[i].Width = new GridLength(_grid.ColumnDefinitions[i].ActualWidth);
                    _total.ColumnDefinitions[i].Width = new GridLength(_grid.ColumnDefinitions[i].ActualWidth);
                    _salesTax.ColumnDefinitions[i].Width = new GridLength(_grid.ColumnDefinitions[i].ActualWidth);
                }
        }

        private void ButtonIncreaseClickHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.BurnCd)
            {
                PaperFormat format = (sender as Button).Tag as PaperFormat;
                if (format == null || !ExecutionEngine.Instance.PaperFormats.Contains(format) || _photoCounts[format] == int.MaxValue)
                    return;

                _photoCounts[format]++;
                _photoCountViews[format].Text = _photoCounts[format].ToString(CultureInfo.CurrentCulture);
                if (ShowPriceColumn || ShowOriginalCostColumn)
                {
                    if (_sourceItem == null)
                        _photoCostViews[format].Text = GetCostString(ExecutionEngine.PriceManager.GetDiscountPrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant, _currentOrder.OrderItems.Count) * _photoCounts[format]);
                    else
                        _photoCostViews[format].Text = GetCostString(ExecutionEngine.PriceManager.GetBasePrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant) * _photoCounts[format]);
                }
                UpdateTotals();
            }
        }

        private void ButtonDecreaseClickHandler(object sender, RoutedEventArgs e)
        {
            if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.BurnCd)
            {
                PaperFormat format = (sender as Button).Tag as PaperFormat;
                if (format == null || !ExecutionEngine.Instance.PaperFormats.Contains(format) || _photoCounts[format] == 0)
                    return;

                _photoCounts[format]--;
                _photoCountViews[format].Text = _photoCounts[format].ToString(CultureInfo.CurrentCulture);
                if (ShowPriceColumn || ShowOriginalCostColumn)
                {
                    if (_sourceItem == null)
                        _photoCostViews[format].Text = GetCostString(ExecutionEngine.PriceManager.GetDiscountPrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant, _currentOrder.OrderItems.Count) * _photoCounts[format]);
                    else
                        _photoCostViews[format].Text = GetCostString(ExecutionEngine.PriceManager.GetBasePrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant) * _photoCounts[format]);
                }
                UpdateTotals();
            }
        }

        public void Update()
        {
            if (!IsLoaded)
                return;

            if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.BurnCd && _sourceOrder != null && _showOriginalCostColumn)
            {
                foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                {
                    int formatTotalCount = 0;
                    foreach (OrderItem item in _sourceOrder.OrderItems)
                        formatTotalCount += item.GetCount(format);

                    _photoCounts[format] = formatTotalCount;
                }
            }

            if (!_isCreated)
                CreateControls();

            if (ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.BurnCd)
            {
                // Set for photo
                if (_sourceItem != null)
                {
                    foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                    {
                        _photoCounts[format] = _sourceItem.GetCount(format);
                        _photoCountViews[format].Text = _sourceItem.GetCount(format).ToString(CultureInfo.CurrentCulture);
                        if (ShowPriceColumn || ShowOriginalCostColumn)
                        {
                            _photoCostViews[format].Text = GetCostString(ExecutionEngine.PriceManager.GetBasePrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant) * _sourceItem.GetCount(format));
                        }
                    }
                }
                // Entire order
                else if (_sourceOrder != null)
                {
                    foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                    {
                        int formatTotalCount = 0;
                        float formatTotalCost = 0.0f;

                        foreach (OrderItem item in _sourceOrder.OrderItems)
                            formatTotalCount += item.GetCount(format);

                        if (formatTotalCount == 0 && _emptyItemStyle != null)
                        {
                            if (ShowPriceColumn || ShowOriginalCostColumn)
                            {
                                _photoCostViews[format].Style = _emptyItemStyle;
                            }
                            _paperFormatViews[format].Style = _emptyItemStyle;
                            _photoCountViews[format].Style = _emptyItemStyle;
                        }
                        else if (_nonEmptyItemStyle != null)
                        {
                            if (ShowPriceColumn || ShowOriginalCostColumn)
                            {
                                _photoCostViews[format].Style = _nonEmptyItemStyle;
                            }
                            _paperFormatViews[format].Style = _nonEmptyItemStyle;
                            _photoCountViews[format].Style = _nonEmptyItemStyle;
                        }

                        formatTotalCost += ExecutionEngine.PriceManager.GetTotalPrice(format, _sourceOrder.OrderPaperType, ExecutionEngine.Instance.Instant, formatTotalCount);

                        _photoCounts[format] = formatTotalCount;
                        _photoCountViews[format].Text = formatTotalCount.ToString(CultureInfo.CurrentCulture);
                        if (ShowPriceColumn || ShowOriginalCostColumn)
                        {
                            _photoCostViews[format].Text = GetCostString(formatTotalCost);
                        }
                    }
                }
                // Set for All
                else
                {
                    if (ShowPriceColumn || ShowOriginalCostColumn)
                    {
                        foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                        {
                            _photoCostViews[format].Text = GetCostString(ExecutionEngine.PriceManager.GetDiscountPrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant, _currentOrder.OrderItems.Count));
                        }
                    }
                }
            }

            UpdateTotals();
        }

        public void SetSource(OrderItem item)
        {
            _sourceItem = item;
        }

        public void SetSource(Order order)
        {
            _sourceOrder = order;
        }

        public Dictionary<PaperFormat, int> ResultValues
        {
            get { return _photoCounts; }
        }

        public bool ShowChangeCountButtons
        {
            get { return _showChangeCountButtons; }
            set { _showChangeCountButtons = value; }
        }

        public bool ShowOriginalCostColumn
        {
            get
            {
                if (_showOriginalCostColumn)
                {
                    foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                    {
                        float originalPrice = ExecutionEngine.PriceManager.GetBasePrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant) * _photoCounts[format];
                        float discountPrice = ExecutionEngine.PriceManager.GetTotalPrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant, _photoCounts[format]);
                        if (originalPrice > discountPrice)
                            return true;
                    }
                }
                return false;
            }
            set { _showOriginalCostColumn = value; }
        }

        public bool ShowPriceColumn
        {
            get { return _showPriceColumn; }
            set { _showPriceColumn = value; }
        }

        public bool ShowSalesTaxGrid
        {
            get
            {
                return (_showSalesTaxGrid && ExecutionEngine.PriceManager.SalesTaxPercent.Value > 0);
            }
            set { _showSalesTaxGrid = value; }
        }

        public bool ShowServices
        {
            get { return _showServices; }
            set { _showServices = value; }
        }

        public bool IsScrollBarVisible
        {
            get { return _scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible; }
        }

        public Style TotalTextStyle
        {
            get { return (Style)GetValue(TotalTextStyleProperty); }
            set
            {
                if (this.TotalTextStyle != value)
                    SetValue(TotalTextStyleProperty, value);
            }
        }

        public string TotalTextContent
        {
            get { return (string)GetValue(TotalTextContentProperty); }
            set
            {
                if (this.TotalTextContent != value)
                    SetValue(TotalTextContentProperty, value);
            }
        }

        public int MaxFormatCount
        {
            get { return (int)GetValue(MaxFormatCountProperty); }
            set
            {
                if (this.MaxFormatCount != value)
                    SetValue(MaxFormatCountProperty, value);
            }
        }

        private void UpdateTotals()
        {
            int totalCount = 0;
            float totalCost = 0.0f;
            float totalOriginalCost = 0.0f;

            // Entire order
            if (_sourceItem == null && _sourceOrder != null)
            {
                foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                {
                    totalCount += _photoCounts[format];
                    totalCost += ExecutionEngine.PriceManager.GetTotalPrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant, _photoCounts[format]);
                    totalOriginalCost += ExecutionEngine.PriceManager.GetBasePrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant) * _photoCounts[format];
                }

                if (_showServices && ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.OrderPhotos)
                {
                    float photosCost = totalCost;
                    float originalPhotosCost = totalOriginalCost;
                    foreach (Service service in _currentOrder.Services)
                    {
                        if (service.IsPriceFixed)
                        {
                            totalCost += service.Price;
                            totalOriginalCost += service.Price;
                        }
                        else
                        {
                            float price = (photosCost * service.Price) / 100.0f;
                            float originalPrice = (originalPhotosCost * service.Price) / 100.0f;
                            totalCost += price;
                            totalOriginalCost += originalPrice;

                            foreach (UIElement element in _grid.Children)
                            {
                                if (element is TextBlock && (element as TextBlock).Tag != null)
                                {
                                    if ((element as TextBlock).Tag.Equals(service.Name))
                                        (element as TextBlock).Text = GetCostString(price) + string.Format(" ({0}%)", service.Price);
                                    else if ((element as TextBlock).Tag.Equals("original" + service.Name))
                                        (element as TextBlock).Text = GetCostString(originalPrice) + string.Format(" ({0}%)", service.Price);
                                }
                            }
                        }
                    }
                }
            }
            // Set for All
            else if (_sourceItem == null && _sourceOrder == null)
            {
                foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                {
                    totalCount += _photoCounts[format] * _currentOrder.OrderItems.Count;
                    totalCost += ExecutionEngine.PriceManager.GetTotalPrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant, _photoCounts[format] * _currentOrder.OrderItems.Count);
                }
            }
            // Set for Photo
            else if (_sourceItem != null && _sourceOrder == null)
            {
                foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                {
                    totalCount += _photoCounts[format];
                    totalCost += ExecutionEngine.PriceManager.GetBasePrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant) * _photoCounts[format];
                }
            }

            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
                totalCost = _currentOrder.GetTotalCost();

            if (_totalCountLabel != null)
                _totalCountLabel.TextContent = totalCount.ToString(CultureInfo.CurrentCulture);

            if (_totalCostLabel != null)
                _totalCostLabel.TextContent = GetCostString(totalCost);

            if (_totalOriginalCostLabel != null)
                _totalOriginalCostLabel.TextContent = GetCostString(totalOriginalCost);

            if (_salesTaxCostLabel != null)
            {
                _salesTaxCostLabel.TextContent = string.Format("{0} ({1}%)", GetCostString(totalCost * ExecutionEngine.PriceManager.SalesTaxPercent.Value), ExecutionEngine.PriceManager.SalesTaxPercent.Value * 100);
                if (!string.IsNullOrEmpty(ExecutionEngine.PriceManager.SalesTaxComment.Value))
                    _salesTaxCostLabel.TextContent += " " + ExecutionEngine.PriceManager.SalesTaxComment.Value;
            }

            if (_totalPaymentLabel != null)
                _totalPaymentLabel.TextContent = GetCostString(totalCost * (ExecutionEngine.PriceManager.SalesTaxPercent.Value + 1));
        }

        private static string GetCostString(float cost)
        {
            return cost.ToString("c", NumberFormatInfo.CurrentInfo);
        }

        private Size GetStringSize(string str)
        {
            var text = new FormattedText(
                       str,
                       CultureInfo.CurrentCulture,
                       this.FlowDirection,
                       new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch),
                       this.FontSize,
                       this.Foreground);
            return new Size(text.Width, text.Height);
        }

        private void ClearControl()
        {
            _photoCountViews.Clear();
            _photoCostViews.Clear();
            _paperFormatViews.Clear();

            _totalCountLabel = null;
            _totalCostLabel = null;

            _grid.ColumnDefinitions.Clear();
            _grid.RowDefinitions.Clear();
            _grid.Children.Clear();

            _salesTax.ColumnDefinitions.Clear();
            _salesTax.RowDefinitions.Clear();
            _salesTax.Children.Clear();

            _header.Children.Clear();
            _header.ColumnDefinitions.Clear();
            _total.Children.Clear();
            _total.ColumnDefinitions.Clear();

            _isCreated = false;
        }

        private void CreateControls()
        {
            if (_isCreated)
                return;

            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
            {
                for (int i = 0; i < 2; i++)
                    _grid.ColumnDefinitions.Add(new ColumnDefinition());

                _grid.ColumnDefinitions[0].MinWidth = GetStringSize((string)FindResource(Constants.OrderInfoControlServiceTextKey)).Width;
                _grid.ColumnDefinitions[1].MinWidth = GetStringSize((string)FindResource(Constants.OrderInfoControlCostTextKey)).Width;

                // Add cd burning line
                _grid.RowDefinitions.Add(new RowDefinition());

                var serviceName = new TextBlock();
                serviceName.Text = (string)FindResource(Constants.OrderInfoControlCdBurningTextKey);
                serviceName.VerticalAlignment = VerticalAlignment.Center;
                serviceName.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetRow(serviceName, 0);
                Grid.SetColumn(serviceName, 0);
                _grid.Children.Add(serviceName);

                var serviceCost = new TextBlock();
                serviceCost.Text = GetCostString(_currentOrder.GetTotalCost());
                serviceCost.VerticalAlignment = VerticalAlignment.Center;
                serviceCost.HorizontalAlignment = HorizontalAlignment.Right;
                serviceCost.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetRow(serviceCost, 0);
                Grid.SetColumn(serviceCost, 1);
                _grid.Children.Add(serviceCost);

                _grid.UpdateLayout();

                // Add header
                foreach (ColumnDefinition column in _grid.ColumnDefinitions)
                {
                    var newColumn = new ColumnDefinition();
                    newColumn.Width = new GridLength(column.ActualWidth);
                    newColumn.MaxWidth = column.MaxWidth;

                    _header.ColumnDefinitions.Add(newColumn);
                }

                var serviceLabel = new TextBlock();
                serviceLabel.Style = (Style)FindResource("OrderInfoTableHeaderTextStyle");
                serviceLabel.Text = (string)FindResource(Constants.OrderInfoControlServiceTextKey);
                serviceLabel.VerticalAlignment = VerticalAlignment.Center;
                serviceLabel.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetColumn(serviceLabel, 0);
                _header.Children.Add(serviceLabel);

                // cost
                if (ShowPriceColumn || ShowOriginalCostColumn)
                {
                    var costLabel = new TextBlock();
                    costLabel.Style = (Style)FindResource("OrderInfoTableHeaderTextStyle");
                    costLabel.Text = (string)FindResource(Constants.OrderInfoControlCostTextKey);
                    costLabel.VerticalAlignment = VerticalAlignment.Center;
                    costLabel.HorizontalAlignment = HorizontalAlignment.Right;
                    costLabel.Margin = new Thickness(10, 0, 10, 0);
                    Grid.SetColumn(costLabel, 1);
                    _header.Children.Add(costLabel);
                }

                // Add totals
                foreach (ColumnDefinition column in _grid.ColumnDefinitions)
                {
                    var newColumn = new ColumnDefinition();
                    newColumn.Width = new GridLength(column.ActualWidth);
                    newColumn.MaxWidth = column.MaxWidth;

                    _total.ColumnDefinitions.Add(newColumn);
                }

                var totalLabel = new OutlinedText();
                totalLabel.Style = this.TotalTextStyle;
                totalLabel.TextContent = this.TotalTextContent;
                totalLabel.FontSize = this.FontSize * 1.1;
                totalLabel.VerticalAlignment = VerticalAlignment.Center;
                totalLabel.HorizontalAlignment = HorizontalAlignment.Left;
                totalLabel.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetColumn(totalLabel, 0);
                _total.Children.Add(totalLabel);

                if (_totalCostLabel == null)
                    _totalCostLabel = new OutlinedText();
                _totalCostLabel.Style = this.TotalTextStyle;
                _totalCostLabel.FontSize = this.FontSize * 1.1;
                _totalCostLabel.TextContent = GetCostString(_currentOrder.GetTotalCost());
                _totalCostLabel.VerticalAlignment = VerticalAlignment.Center;
                _totalCostLabel.HorizontalAlignment = HorizontalAlignment.Right;
                _totalCostLabel.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetColumn(_totalCostLabel, 1);
                _total.Children.Add(_totalCostLabel);
            }
            else
            {
                for (int i = 0; i < 6; i++)
                    _grid.ColumnDefinitions.Add(new ColumnDefinition());

                if (!_showChangeCountButtons)
                {
                    _grid.ColumnDefinitions[0].Width = new GridLength(1.6, GridUnitType.Star);
                    _grid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                    _grid.ColumnDefinitions[1].MaxWidth = 0;
                    _grid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
                    _grid.ColumnDefinitions[2].MaxWidth = 0;
                    _grid.ColumnDefinitions[3].Width = new GridLength(2, GridUnitType.Auto);
                    _grid.ColumnDefinitions[3].MinWidth = GetStringSize((string)FindResource(Constants.OrderInfoControlCountTextKey)).Width + 20;
                    _grid.ColumnDefinitions[5].Width = new GridLength(2, GridUnitType.Star);

                    if (ShowOriginalCostColumn)
                    {
                        _grid.ColumnDefinitions[4].Width = new GridLength(2, GridUnitType.Star);
                        _grid.ColumnDefinitions[4].MinWidth = GetStringSize((string)FindResource(Constants.OrderInfoControlCostTextKey)).Width + 20;
                        _grid.ColumnDefinitions[5].MinWidth = GetStringSize((string)FindResource(Constants.OrderInfoControlDiscountCostTextKey)).Width + 20;
                    }
                    else if (ShowPriceColumn)
                    {
                        _grid.ColumnDefinitions[4].Width = new GridLength(0, GridUnitType.Pixel);
                        _grid.ColumnDefinitions[4].MaxWidth = 0;
                        _grid.ColumnDefinitions[5].MinWidth = GetStringSize((string)FindResource(Constants.OrderInfoControlCostTextKey)).Width + 20;
                    }
                    else
                    {
                        _grid.ColumnDefinitions[5].MinWidth = (_grid.ActualWidth) / 3.5;
                    }
                }
                else
                {
                    _grid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                    _grid.ColumnDefinitions[1].MinWidth = GetStringSize((string)FindResource(Constants.OrderInfoControlPriceTextKey)).Width + 20;
                    _grid.ColumnDefinitions[2].Width = new GridLength();
                    _grid.ColumnDefinitions[3].Width = new GridLength(1, GridUnitType.Star);
                    _grid.ColumnDefinitions[3].MinWidth = (_grid.ActualWidth) / 10;
                    _grid.ColumnDefinitions[4].Width = new GridLength();
                    _grid.ColumnDefinitions[5].Width = new GridLength(1, GridUnitType.Star);
                    _grid.ColumnDefinitions[5].MinWidth = (_grid.ActualWidth) / 3.5;
                }

                if (!ShowSalesTaxGrid)
                    _grid.ColumnDefinitions[0].MinWidth = GetStringSize((string)FindResource(Constants.OrderInfoControlFormatTextKey)).Width + 20;
                else
                    _grid.ColumnDefinitions[0].MinWidth = GetStringSize((string)FindResource(Constants.OrderInfoControlOrderTotalTextKey)).Width * 1.2 + 20;

                // Adding rows to grid for each paper format
                int formatCount = 0;
                foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                {
                    formatCount++;
                    if (formatCount <= MaxFormatCount)
                        _grid.RowDefinitions.Add(new RowDefinition());

                    // Format name label
                    var formatName = new TextBlock();
                    formatName.Text = format.Name;
                    formatName.Margin = new Thickness(10, 0, 10, 0);
                    formatName.VerticalAlignment = VerticalAlignment.Center;
                    _paperFormatViews.Add(format, formatName);
                    if (formatCount <= MaxFormatCount)
                    {
                        if (formatCount == MaxFormatCount && ExecutionEngine.Instance.PaperFormats.Count > MaxFormatCount)
                        {
                            var formatNameDots = new TextBlock();
                            formatNameDots.Text = "...";
                            formatNameDots.Margin = new Thickness(10, 0, 10, 0);
                            Grid.SetColumn(formatNameDots, 0);
                            Grid.SetRow(formatNameDots, _grid.RowDefinitions.Count - 1);
                            _grid.Children.Add(formatNameDots);

                            var countDots = new TextBlock();
                            countDots.Text = "...";
                            countDots.Margin = new Thickness(10, 0, 10, 0);
                            countDots.HorizontalAlignment = HorizontalAlignment.Center;
                            Grid.SetColumn(countDots, 3);
                            Grid.SetRow(countDots, _grid.RowDefinitions.Count - 1);
                            _grid.Children.Add(countDots);

                            var costDots = new TextBlock();
                            costDots.Text = "...";
                            costDots.Margin = new Thickness(10, 0, 10, 0);
                            costDots.HorizontalAlignment = HorizontalAlignment.Right;
                            Grid.SetColumn(costDots, 5);
                            Grid.SetRow(costDots, _grid.RowDefinitions.Count - 1);
                            _grid.Children.Add(costDots);
                        }
                        else
                        {
                            Grid.SetColumn(formatName, 0);
                            Grid.SetRow(formatName, _grid.RowDefinitions.Count - 1);
                            _grid.Children.Add(formatName);
                        }
                    }

                    // Price label
                    if ((formatCount < MaxFormatCount || (formatCount == MaxFormatCount && ExecutionEngine.Instance.PaperFormats.Count == MaxFormatCount)) && ShowPriceColumn)
                    {
                        var pricePerOne = new TextBlock();
                        if (_sourceItem != null)
                            pricePerOne.Text = GetCostString(ExecutionEngine.PriceManager.GetBasePrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant));
                        else
                            pricePerOne.Text = GetCostString(ExecutionEngine.PriceManager.GetDiscountPrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant, _currentOrder.OrderItems.Count));
                        pricePerOne.VerticalAlignment = VerticalAlignment.Center;
                        pricePerOne.HorizontalAlignment = HorizontalAlignment.Right;
                        pricePerOne.Margin = new Thickness(10, 0, 10, 0);
                        Grid.SetColumn(pricePerOne, 1);
                        Grid.SetRow(pricePerOne, _grid.RowDefinitions.Count - 1);
                        _grid.Children.Add(pricePerOne);
                    }

                    // Decrease button
                    if (_showChangeCountButtons)
                    {
                        var decreaseButton = new Button();
                        var minusText = new OutlinedText();
                        minusText.Style = (Style)FindResource("PlusMinusButtonsTextStyle");
                        minusText.TextContent = "-";
                        minusText.VerticalAlignment = VerticalAlignment.Center;
                        decreaseButton.Content = minusText;
                        decreaseButton.Style = (Style)FindResource("ChangeCountButtonStyle");
                        decreaseButton.VerticalAlignment = VerticalAlignment.Center;
                        decreaseButton.Margin = new Thickness(5);
                        decreaseButton.Tag = format;
                        decreaseButton.Click += ButtonDecreaseClickHandler;
                        Grid.SetColumn(decreaseButton, 2);
                        Grid.SetRow(decreaseButton, _grid.RowDefinitions.Count - 1);
                        _grid.Children.Add(decreaseButton);
                    }

                    // Count label
                    var inprintCount = new TextBlock();
                    inprintCount.Text = _photoCounts[format].ToString(CultureInfo.CurrentCulture);
                    inprintCount.HorizontalAlignment = HorizontalAlignment.Center;
                    inprintCount.VerticalAlignment = VerticalAlignment.Center;
                    inprintCount.Margin = new Thickness(10, 0, 10, 0);
                    _photoCountViews.Add(format, inprintCount);
                    if (formatCount < MaxFormatCount || (formatCount == MaxFormatCount && ExecutionEngine.Instance.PaperFormats.Count == MaxFormatCount))
                    {
                        Grid.SetColumn(inprintCount, 3);
                        Grid.SetRow(inprintCount, _grid.RowDefinitions.Count - 1);
                        _grid.Children.Add(inprintCount);
                    }

                    // Increase button
                    if (_showChangeCountButtons)
                    {
                        var increaseButton = new Button();
                        var plusText = new OutlinedText();
                        plusText.Style = (Style)FindResource("PlusMinusButtonsTextStyle");
                        plusText.TextContent = "+";
                        plusText.VerticalAlignment = VerticalAlignment.Center;
                        increaseButton.Content = plusText;
                        increaseButton.Style = (Style)FindResource("ChangeCountButtonStyle");
                        increaseButton.VerticalAlignment = VerticalAlignment.Center;
                        increaseButton.Click += ButtonIncreaseClickHandler;
                        increaseButton.Margin = new Thickness(5);
                        increaseButton.Tag = format;
                        Grid.SetColumn(increaseButton, 4);
                        Grid.SetRow(increaseButton, _grid.RowDefinitions.Count - 1);
                        _grid.Children.Add(increaseButton);
                    }
                    else if (ShowOriginalCostColumn)
                    {
                        float originalPrice = ExecutionEngine.PriceManager.GetBasePrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant) * _photoCounts[format];
                        var originalCost = new TextBlock();
                        originalCost.Text = GetCostString(originalPrice);
                        originalCost.VerticalAlignment = VerticalAlignment.Center;
                        originalCost.HorizontalAlignment = HorizontalAlignment.Right;
                        originalCost.Margin = new Thickness(10, 0, 10, 0);
                        Grid.SetColumn(originalCost, 4);
                        Grid.SetRow(originalCost, _grid.RowDefinitions.Count - 1);
                        _grid.Children.Add(originalCost);
                    }
                    if (ShowPriceColumn || ShowOriginalCostColumn)
                    {
                        // Total cost label
                        float cost = ExecutionEngine.PriceManager.GetTotalPrice(format, _currentOrder.OrderPaperType, ExecutionEngine.Instance.Instant, _photoCounts[format]);
                        var totalCost = new TextBlock();
                        totalCost.Text = GetCostString(cost);
                        totalCost.VerticalAlignment = VerticalAlignment.Center;
                        totalCost.HorizontalAlignment = HorizontalAlignment.Right;
                        totalCost.Margin = new Thickness(10, 0, 10, 0);
                        _photoCostViews.Add(format, totalCost);
                        if (formatCount < MaxFormatCount || (formatCount == MaxFormatCount && ExecutionEngine.Instance.PaperFormats.Count == MaxFormatCount))
                        {
                            Grid.SetColumn(totalCost, 5);
                            Grid.SetRow(totalCost, _grid.RowDefinitions.Count - 1);
                            _grid.Children.Add(totalCost);
                        }
                    }
                }

                if (_showServices && ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.OrderPhotos)
                {
                    foreach (Service service in _currentOrder.Services)
                    {
                        _grid.RowDefinitions.Add(new RowDefinition());

                        var serviceLabel = new TextBlock();
                        serviceLabel.Text = service.Name;
                        serviceLabel.VerticalAlignment = VerticalAlignment.Center;
                        serviceLabel.Margin = new Thickness(10, 0, 10, 0);
                        Grid.SetColumn(serviceLabel, 0);
                        if (ShowOriginalCostColumn)
                            Grid.SetColumnSpan(serviceLabel, 4);
                        Grid.SetRow(serviceLabel, _grid.RowDefinitions.Count - 1);
                        _grid.Children.Add(serviceLabel);

                        if (ShowOriginalCostColumn)
                        {
                            var originalCost = new TextBlock();
                            if (service.IsPriceFixed)
                                originalCost.Text = GetCostString(service.Price);
                            else
                                originalCost.Tag = "original" + service.Name;
                            originalCost.VerticalAlignment = VerticalAlignment.Center;
                            originalCost.HorizontalAlignment = HorizontalAlignment.Right;
                            originalCost.Margin = new Thickness(10, 0, 10, 0);
                            Grid.SetColumn(originalCost, 4);
                            Grid.SetRow(originalCost, _grid.RowDefinitions.Count - 1);
                            _grid.Children.Add(originalCost);
                        }

                        var serviceCost = new TextBlock();
                        if (service.IsPriceFixed)
                            serviceCost.Text = GetCostString(service.Price);
                        else
                            serviceCost.Tag = service.Name;
                        serviceCost.VerticalAlignment = VerticalAlignment.Center;
                        serviceCost.HorizontalAlignment = HorizontalAlignment.Right;
                        serviceCost.Margin = new Thickness(10, 0, 10, 0);
                        Grid.SetColumn(serviceCost, 5);
                        Grid.SetRow(serviceCost, _grid.RowDefinitions.Count - 1);
                        _grid.Children.Add(serviceCost);
                    }
                }

                _grid.UpdateLayout();

                // Add header
                foreach (ColumnDefinition column in _grid.ColumnDefinitions)
                {
                    var newColumn = new ColumnDefinition();
                    newColumn.Width = new GridLength(column.ActualWidth);
                    newColumn.MaxWidth = column.MaxWidth;

                    _header.ColumnDefinitions.Add(newColumn);
                }

                var formatLabel = new TextBlock();
                formatLabel.Style = (Style)FindResource("OrderInfoTableHeaderTextStyle");
                formatLabel.Text = (string)FindResource(Constants.OrderInfoControlFormatTextKey);
                formatLabel.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetColumn(formatLabel, 0);
                _header.Children.Add(formatLabel);

                if (ShowPriceColumn)
                {
                    var priceLabel = new TextBlock();
                    priceLabel.Style = (Style)FindResource("OrderInfoTableHeaderTextStyle");
                    priceLabel.Text = (string)FindResource(Constants.OrderInfoControlPriceTextKey);
                    priceLabel.Margin = new Thickness(10, 0, 10, 0);
                    priceLabel.HorizontalAlignment = HorizontalAlignment.Right;
                    Grid.SetColumn(priceLabel, 1);
                    _header.Children.Add(priceLabel);
                }

                var countLabel = new TextBlock();
                countLabel.Style = (Style)FindResource("OrderInfoTableHeaderTextStyle");
                countLabel.Text = (string)FindResource(Constants.OrderInfoControlCountTextKey);
                countLabel.Margin = new Thickness(10, 0, 10, 0);
                countLabel.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetColumn(countLabel, 2);
                if (!ShowOriginalCostColumn)
                    Grid.SetColumnSpan(countLabel, 3);
                else
                    Grid.SetColumnSpan(countLabel, 2);
                _header.Children.Add(countLabel);

                if (ShowOriginalCostColumn)
                {
                    var originalCostLabel = new TextBlock();
                    originalCostLabel.Style = (Style)FindResource("OrderInfoTableHeaderTextStyle");
                    originalCostLabel.Text = (string)FindResource(Constants.OrderInfoControlCostTextKey);
                    originalCostLabel.Margin = new Thickness(10, 0, 10, 0);
                    originalCostLabel.HorizontalAlignment = HorizontalAlignment.Right;
                    Grid.SetColumn(originalCostLabel, 4);
                    _header.Children.Add(originalCostLabel);
                }

                if (ShowPriceColumn || ShowOriginalCostColumn)
                {
                    var costLabel = new TextBlock();
                    costLabel.Style = (Style)FindResource("OrderInfoTableHeaderTextStyle");
                    costLabel.Margin = new Thickness(10, 0, 10, 0);
                    if (ShowOriginalCostColumn)
                        costLabel.Text = (string)FindResource(Constants.OrderInfoControlDiscountCostTextKey);
                    else if (ShowPriceColumn)
                        costLabel.Text = (string)FindResource(Constants.OrderInfoControlCostTextKey);
                    costLabel.HorizontalAlignment = HorizontalAlignment.Right;
                    Grid.SetColumn(costLabel, 5);
                    _header.Children.Add(costLabel);
                }

                // Add totals
                foreach (ColumnDefinition column in _grid.ColumnDefinitions)
                {
                    var newColumn = new ColumnDefinition();
                    newColumn.Width = new GridLength(column.ActualWidth);
                    newColumn.MaxWidth = column.MaxWidth;

                    _total.ColumnDefinitions.Add(newColumn);
                }

                var totalLabel = new OutlinedText();
                totalLabel.Style = this.TotalTextStyle;
                totalLabel.FontSize = this.FontSize * 1.1;
                if (ShowSalesTaxGrid)
                    totalLabel.TextContent = (string)FindResource(Constants.OrderInfoControlOrderTotalTextKey);
                else
                    totalLabel.TextContent = this.TotalTextContent;
                totalLabel.HorizontalAlignment = HorizontalAlignment.Left;
                totalLabel.VerticalAlignment = VerticalAlignment.Center;
                totalLabel.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetColumn(totalLabel, 0);
                Grid.SetColumnSpan(totalLabel, 2);
                _total.Children.Add(totalLabel);

                if (_totalCountLabel == null)
                    _totalCountLabel = new OutlinedText();
                _totalCountLabel.Style = this.TotalTextStyle;
                _totalCountLabel.FontSize = this.FontSize * 1.1;
                _totalCountLabel.Margin = new Thickness(10, 0, 10, 0);
                _totalCountLabel.HorizontalAlignment = HorizontalAlignment.Center;
                _totalCountLabel.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(_totalCountLabel, 2);
                if (!ShowOriginalCostColumn)
                    Grid.SetColumnSpan(_totalCountLabel, 3);
                else
                    Grid.SetColumnSpan(_totalCountLabel, 2);
                _total.Children.Add(_totalCountLabel);

                if (ShowOriginalCostColumn)
                {
                    if (_totalOriginalCostLabel == null)
                        _totalOriginalCostLabel = new OutlinedText();
                    _totalOriginalCostLabel.Style = this.TotalTextStyle;
                    _totalOriginalCostLabel.FontSize = this.FontSize * 1.1;
                    _totalOriginalCostLabel.HorizontalAlignment = HorizontalAlignment.Right;
                    _totalOriginalCostLabel.VerticalAlignment = VerticalAlignment.Center;
                    _totalOriginalCostLabel.Margin = new Thickness(10, 0, 10, 0);
                    Grid.SetColumn(_totalOriginalCostLabel, 4);
                    _total.Children.Add(_totalOriginalCostLabel);
                }

                if ((ShowPriceColumn || ShowOriginalCostColumn) && _totalCostLabel == null)
                {
                    _totalCostLabel = new OutlinedText();
                    _totalCostLabel.Style = this.TotalTextStyle;
                    _totalCostLabel.FontSize = this.FontSize * 1.1;
                    _totalCostLabel.HorizontalAlignment = HorizontalAlignment.Right;
                    _totalCostLabel.VerticalAlignment = VerticalAlignment.Center;
                    _totalCostLabel.Margin = new Thickness(10, 0, 10, 0);
                    Grid.SetColumn(_totalCostLabel, 5);
                    _total.Children.Add(_totalCostLabel);
                }
            }

            if (ShowSalesTaxGrid)
            {
                _salesTax.Visibility = Visibility.Visible;
                _salesTaxDelimiter.Visibility = Visibility.Visible;
                foreach (ColumnDefinition column in _grid.ColumnDefinitions)
                {
                    var newColumn = new ColumnDefinition();
                    newColumn.Width = new GridLength(column.ActualWidth);
                    newColumn.MaxWidth = column.MaxWidth;

                    _salesTax.ColumnDefinitions.Add(newColumn);
                }

                _salesTax.RowDefinitions.Add(new RowDefinition());

                var salesTaxLabel = new OutlinedText((string)FindResource(Constants.OrderInfoControlSalesTaxTextKey), this.TotalTextStyle);
                salesTaxLabel.FontSize = this.FontSize * 1.1;
                salesTaxLabel.HorizontalAlignment = HorizontalAlignment.Left;
                salesTaxLabel.VerticalAlignment = VerticalAlignment.Center;
                salesTaxLabel.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetRow(salesTaxLabel, 0);
                Grid.SetColumn(salesTaxLabel, 0);
                if (ShowOriginalCostColumn)
                    Grid.SetColumnSpan(salesTaxLabel, 4);
                _salesTax.Children.Add(salesTaxLabel);

                if (_salesTaxCostLabel == null)
                    _salesTaxCostLabel = new OutlinedText();
                _salesTaxCostLabel.Style = this.TotalTextStyle;
                _salesTaxCostLabel.FontSize = this.FontSize * 1.1;
                _salesTaxCostLabel.HorizontalAlignment = HorizontalAlignment.Right;
                _salesTaxCostLabel.VerticalAlignment = VerticalAlignment.Center;
                _salesTaxCostLabel.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetRow(_salesTaxCostLabel, 0);

                if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
                {
                    Grid.SetColumn(_salesTaxCostLabel, 1);
                }
                else
                {
                    if (ShowOriginalCostColumn)
                    {
                        Grid.SetColumn(_salesTaxCostLabel, 4);
                        Grid.SetColumnSpan(_salesTaxCostLabel, 2);
                    }
                    else
                    {
                        Grid.SetColumn(_salesTaxCostLabel, 3);
                        Grid.SetColumnSpan(_salesTaxCostLabel, 3);
                    }
                }
                _salesTax.Children.Add(_salesTaxCostLabel);

                _salesTax.RowDefinitions.Add(new RowDefinition());

                var totalPayment = new OutlinedText((string)FindResource(Constants.OrderInfoControlTotalPaymentKey), this.TotalTextStyle);
                totalPayment.FontSize = this.FontSize * 1.1;
                totalPayment.HorizontalAlignment = HorizontalAlignment.Left;
                totalPayment.VerticalAlignment = VerticalAlignment.Center;
                totalPayment.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetRow(totalPayment, 1);
                Grid.SetColumn(totalPayment, 0);
                if (ShowOriginalCostColumn)
                    Grid.SetColumnSpan(totalPayment, 4);
                _salesTax.Children.Add(totalPayment);

                if (_totalPaymentLabel == null)
                    _totalPaymentLabel = new OutlinedText();

                _totalPaymentLabel.Style = this.TotalTextStyle;
                _totalPaymentLabel.FontSize = this.FontSize * 1.1;
                _totalPaymentLabel.HorizontalAlignment = HorizontalAlignment.Right;
                _totalPaymentLabel.VerticalAlignment = VerticalAlignment.Center;
                _totalPaymentLabel.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetRow(_totalPaymentLabel, 1);
                if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
                {
                    Grid.SetColumn(_totalPaymentLabel, 1);
                }
                else
                {
                    if (ShowOriginalCostColumn)
                    {
                        Grid.SetColumn(_totalPaymentLabel, 4);
                        Grid.SetColumnSpan(_totalPaymentLabel, 2);
                    }
                    else
                    {
                        Grid.SetColumn(_totalPaymentLabel, 3);
                        Grid.SetColumnSpan(_totalPaymentLabel, 3);
                    }
                }
                _salesTax.Children.Add(_totalPaymentLabel);
            }
            else
            {
                _salesTax.Visibility = Visibility.Collapsed;
                _salesTaxDelimiter.Visibility = Visibility.Collapsed;
            }

            _isCreated = true;
        }
    }
}