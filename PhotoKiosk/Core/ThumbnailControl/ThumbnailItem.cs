// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Aurigma.PhotoKiosk
{
    public class ThumbnailItem : Control, INotifyPropertyChanged
    {
        private static RoutedCommand _showImageEditorCommand;
        private static RoutedCommand _showImageViewerCommand;
        private static RoutedCommand _showImageCropCommand;
        private static RoutedCommand _removeItemCommand;
        private static RoutedCommand _chooseFormatsCommand;

        public static RoutedCommand ShowImageEditorCommand
        {
            get
            {
                if (ExecutionEngine.Config.IsImageEditorEnabled() && _showImageEditorCommand == null)
                {
                    _showImageEditorCommand = new RoutedCommand();
                    CommandManager.RegisterClassCommandBinding(typeof(ThumbnailItem), new CommandBinding(_showImageEditorCommand, OnShowImageEditorCommand));
                }
                return _showImageEditorCommand;
            }
        }

        public static RoutedCommand ShowImageViewerCommand
        {
            get
            {
                if (_showImageViewerCommand == null)
                {
                    _showImageViewerCommand = new RoutedCommand();
                    CommandManager.RegisterClassCommandBinding(typeof(ThumbnailItem), new CommandBinding(_showImageViewerCommand, OnShowImageViewerCommand));
                }
                return _showImageViewerCommand;
            }
        }

        public static RoutedCommand ShowImageCropCommand
        {
            get
            {
                if (_showImageCropCommand == null)
                {
                    _showImageCropCommand = new RoutedCommand();
                    CommandManager.RegisterClassCommandBinding(typeof(ThumbnailItem), new CommandBinding(_showImageCropCommand, OnShowImageCropCommand));
                }
                return _showImageCropCommand;
            }
        }

        public static RoutedCommand RemoveItemCommand
        {
            get
            {
                if (_removeItemCommand == null)
                {
                    _removeItemCommand = new RoutedCommand();
                    CommandManager.RegisterClassCommandBinding(typeof(ThumbnailItem), new CommandBinding(_removeItemCommand, OnRemoveItemCommand));
                }
                return _removeItemCommand;
            }
        }

        public static RoutedCommand ChooseFormatsCommand
        {
            get
            {
                if (_chooseFormatsCommand == null)
                {
                    _chooseFormatsCommand = new System.Windows.Input.RoutedCommand();
                    CommandManager.RegisterClassCommandBinding(typeof(ThumbnailItem), new CommandBinding(_chooseFormatsCommand, OnChooseFormatsCommand));
                }
                return _chooseFormatsCommand;
            }
        }

        private static void OnShowImageEditorCommand(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as ThumbnailItem).OnShowImageEditor(ImageEditorMode.Edit);
        }

        private static void OnShowImageViewerCommand(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as ThumbnailItem).OnShowImageEditor(ImageEditorMode.View);
        }

        private static void OnShowImageCropCommand(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as ThumbnailItem).OnShowImageEditor(ImageEditorMode.Crop);
        }

        private static void OnRemoveItemCommand(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as ThumbnailItem).RemoveItem(true);
        }

        private static void OnChooseFormatsCommand(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as ThumbnailItem).ShowChooseFormatsWindow();
        }

        public ThumbnailItem(PhotoItem photo)
        {
            if (photo == null)
                throw new ArgumentNullException("photo");

            _photo = photo;
            _orderItem = new OrderItem(_photo);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler ContentChanged;

        protected void OnContentChangedEvent(EventArgs e)
        {
            if (ContentChanged != null)
                ContentChanged(this, e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (_hostList != null)
            {
                if (_hostList.ItemStyleName == Constants.SelectItemStyleName)
                {
                    Checked = !Checked;
                    NotifyPropertyChanged("Checked");
                }
                else
                {
                    if (_hostList.GetActiveItem() != null)
                        _hostList.GetActiveItem()._isActive = false;
                    _isActive = true;
                }
            }
        }

        public void RefreshPhoto()
        {
            if (!_isCorrupted)
                NotifyPropertyChanged("ImgSource");
            else
                _hostList.HostControl.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NoArgsDelegate(InvokeRemoveItem));
        }

        private delegate void NoArgsDelegate();

        private void InvokeRemoveItem()
        {
            _hostList.RemoveItem(this, true);
        }

        public void SetCheckedProperty(bool propChecked)
        {
            if (_orderItem.IsActive != propChecked)
            {
                _orderItem.IsActive = propChecked;
                NotifyPropertyChanged("Checked");
            }
        }

        public bool HasNext()
        {
            int index = _hostList.FilteredItems.IndexOf(this);
            return (index > -1 && index < _hostList.FilteredItems.Count - 1);
        }

        public ThumbnailItem GetNext()
        {
            if (HasNext())
            {
                ThumbnailItem item = _hostList.FilteredItems[_hostList.FilteredItems.IndexOf(this) + 1];

                try
                {
                    item.Photo.LoadImage();
                }
                catch (Exception e)
                {
                    ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                    MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageLoadingImageErrorKey]);
                    return this;
                }

                return item;
            }
            else
                return this;
        }

        public bool HasPrevious()
        {
            return (_hostList.FilteredItems.IndexOf(this) > 0);
        }

        public ThumbnailItem GetPrevious()
        {
            if (HasPrevious())
            {
                ThumbnailItem item = _hostList.FilteredItems[_hostList.FilteredItems.IndexOf(this) - 1];

                try
                {
                    item.Photo.LoadImage();
                }
                catch (Exception e)
                {
                    ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                    MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageLoadingImageErrorKey]);
                    return this;
                }

                return item;
            }
            else
                return this;
        }

        public void GoToPage()
        {
            _hostList.GoToPageContainigItem(this);
        }

        public bool IsActive()
        {
            return _isActive;
        }

        public BitmapSource ImgSource
        {
            get { return _photo.GetBitmapSource(); }
        }

        public string FileName
        {
            get { return _photo.SourceFileName; }
        }

        public bool Checked
        {
            get { return _orderItem.IsActive; }
            set
            {
                _orderItem.IsActive = value;

                if (_hostList != null)
                    _hostList.RemoveItemAfterCheck(this);
            }
        }

        public OrderItem Order
        {
            get { return _orderItem; }
        }

        public PhotoItem Photo
        {
            get { return _photo; }
        }

        public ThumbnailList HostList
        {
            get { return _hostList; }
            set { _hostList = value; }
        }

        public string DisplayText
        {
            get
            {
                if (_photo.SourceFileNameWithoutPath.Length <= Constants.MaxPhotoFilenameLength)
                    return _photo.SourceFileNameWithoutPath;
                return _photo.SourceFileNameWithoutPath.Substring(0, Constants.MaxPhotoFilenameLength - 1) + "…";
            }
        }

        public bool IsCorrupted
        {
            get { return _isCorrupted; }
            set { _isCorrupted = value; }
        }

        private Size GetBorderSize()
        {
            Size imageSize = _photo.ImageSize;
            Size thumbSize = new Size(Width - 74 /* Margins + BorderThickness */, Height - 149 /* Margins + BorderThickness + bottom panel height */);

            if (ExecutionEngine.Instance.SelectedPaperFormat != null && !ExecutionEngine.Instance.SelectedPaperFormat.IsFree)
            {
                var result = new Size();
                double ratio = (double)ExecutionEngine.Instance.SelectedPaperFormat.Width / (double)ExecutionEngine.Instance.SelectedPaperFormat.Height;
                if (_photo.ImageSize.Width < _photo.ImageSize.Height)
                    ratio = 1 / ratio;

                result.Height = thumbSize.Height;
                result.Width = thumbSize.Height * ratio;
                if (result.Width > thumbSize.Width)
                {
                    result.Width = thumbSize.Width;
                    result.Height = thumbSize.Width / ratio;
                }

                return result;
            }
            else
                return PhotoFrame.Inscribe(imageSize, thumbSize);
        }

        private Rect GetImageBounds()
        {
            var result = new Rect();

            if (ExecutionEngine.Instance.SelectedPaperFormat != null && !ExecutionEngine.Instance.SelectedPaperFormat.IsFree)
            {
                if ((ExecutionEngine.Context[Constants.OrderContextName] as Order).CropMode == Constants.ResizeToFitModeName)
                {
                    result.Size = PhotoFrame.Inscribe(_photo.ImageSize, GetBorderSize());
                    result.X = (GetBorderSize().Width - result.Width) / 2;
                    result.Y = (GetBorderSize().Height - result.Height) / 2;
                }
                else
                {
                    result.Size = PhotoFrame.Describe(_photo.ImageSize, GetBorderSize());
                    double scale = Math.Min((result.Width - 2) / _photo.ImageSize.Width, (result.Height - 2) / _photo.ImageSize.Height);
                    Rect r = _orderItem.GetCrop(ExecutionEngine.Instance.SelectedPaperFormat);

                    result.X = r.X * -scale;
                    result.Y = r.Y * -scale;
                }
            }
            else
            {
                result.Size = PhotoFrame.Inscribe(_photo.ImageSize, GetBorderSize());
                result.X = 0;
                result.Y = 0;
            }

            result.Width -= 2;
            result.Height -= 2;

            return result;
        }

        public double BorderWidth
        {
            get { return GetBorderSize().Width; }
        }

        public double BorderHeight
        {
            get { return GetBorderSize().Height; }
        }

        public double ImageWidth
        {
            get { return GetImageBounds().Width; }
        }

        public double ImageHeight
        {
            get { return GetImageBounds().Height; }
        }

        public double ImageLeft
        {
            get { return GetImageBounds().X; }
            set
            {
                if (ExecutionEngine.Instance.SelectedPaperFormat != null && !ExecutionEngine.Instance.SelectedPaperFormat.IsFree && (ExecutionEngine.Context[Constants.OrderContextName] as Order).CropMode == Constants.CropToFillModeName)
                {
                    double scale = Math.Min(GetImageBounds().Width / _photo.ImageSize.Width, GetImageBounds().Height / _photo.ImageSize.Height);
                    Rect r = _orderItem.GetCrop(ExecutionEngine.Instance.SelectedPaperFormat);
                    r.X = value / -scale;
                    _orderItem.SetCrop(ExecutionEngine.Instance.SelectedPaperFormat, r);
                }
            }
        }

        public double ImageTop
        {
            get { return GetImageBounds().Y; }
            set
            {
                if (ExecutionEngine.Instance.SelectedPaperFormat != null && !ExecutionEngine.Instance.SelectedPaperFormat.IsFree && (ExecutionEngine.Context[Constants.OrderContextName] as Order).CropMode == Constants.CropToFillModeName)
                {
                    double scale = Math.Min(GetImageBounds().Width / _photo.ImageSize.Width, GetImageBounds().Height / _photo.ImageSize.Height);
                    Rect r = _orderItem.GetCrop(ExecutionEngine.Instance.SelectedPaperFormat);
                    r.Y = value / -scale;
                    _orderItem.SetCrop(ExecutionEngine.Instance.SelectedPaperFormat, r);
                }
            }
        }

        public Visibility ArrowVisibility
        {
            get
            {
                if (ExecutionEngine.Instance.SelectedPaperFormat != null && !ExecutionEngine.Instance.SelectedPaperFormat.IsFree && (ExecutionEngine.Context[Constants.OrderContextName] as Order).CropMode == Constants.CropToFillModeName)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
        }

        public Style VerticalArrowStyle
        {
            get
            {
                if (ExecutionEngine.Instance.SelectedPaperFormat != null && !ExecutionEngine.Instance.SelectedPaperFormat.IsFree)
                {
                    int cropHeight = (int)_orderItem.GetCrop(ExecutionEngine.Instance.SelectedPaperFormat).Height;
                    int imageHeight = (int)_orderItem.SourcePhotoItem.ImageSize.Height;
                    if (cropHeight < imageHeight && imageHeight - cropHeight > 1)
                        return TryFindResource("PhotoFrameArrowEnabledStyle") as Style;
                }
                return TryFindResource("PhotoFrameArrowDisabledStyle") as Style;
            }
        }

        public Style HorizontalArrowStyle
        {
            get
            {
                if (ExecutionEngine.Instance.SelectedPaperFormat != null && !ExecutionEngine.Instance.SelectedPaperFormat.IsFree)
                {
                    int cropWidth = (int)_orderItem.GetCrop(ExecutionEngine.Instance.SelectedPaperFormat).Width;
                    int imageWidth = (int)_orderItem.SourcePhotoItem.ImageSize.Width;
                    if (cropWidth < imageWidth && imageWidth - cropWidth > 1)
                        return TryFindResource("PhotoFrameArrowEnabledStyle") as Style;
                }
                return TryFindResource("PhotoFrameArrowDisabledStyle") as Style;
            }
        }

        public Visibility QualityIndicatorVisibility
        {
            get
            {
                if (LowQualityIndicatorNeeded())
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
        }

        public SolidColorBrush ImageBorderColor
        {
            get
            {
                if (LowQualityIndicatorNeeded())
                    return new SolidColorBrush(Colors.Red);
                else
                    return new SolidColorBrush(Colors.Black);
            }
        }

        public string LowQualityText
        {
            get
            {
                if (ExecutionEngine.Instance.SelectedPaperFormat != null && LowQualityIndicatorNeeded())
                    return string.Format((string)FindResource(Constants.ThumbnailLowQualityTextKey), ExecutionEngine.Instance.SelectedPaperFormat.Name);
                else
                    return "";
            }
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        private void OnShowImageEditor(ImageEditorMode mode)
        {
            try
            {
                _photo.LoadImage();
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageLoadingImageErrorKey]);
                return;
            }

            ExecutionEngine.Context[Constants.ImageEditorModeKey] = mode;
            ExecutionEngine.Context[Constants.EditPhotoContextName] = this;

            try
            {
                ExecutionEngine.Instance.ExecuteCommand(new SwitchToStageCommand(Constants.ImageEditorStageName));
            }
            catch (OutOfMemoryException e)
            {
                ExecutionEngine.Context.Remove(Constants.ImageEditorModeKey);
                ExecutionEngine.Context.Remove(Constants.EditPhotoContextName);
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                MessageDialog.Show((string)ExecutionEngine.Instance.Resource[Constants.MessageLoadingImageErrorKey]);
            }
        }

        private void RemoveItem(bool fromFilteredScopeOnly)
        {
            if (_hostList != null)
                _hostList.RemoveItem(this, fromFilteredScopeOnly);
        }

        private void ShowChooseFormatsWindow()
        {
            if (ChoosePaperSizes.ShowChoosePaperSizesDialog(Order) == null)
                return;

            if (_hostList == null)
                throw new System.ArgumentNullException("_hostList");

            if (Order.IsEmpty())
                RemoveItem(false);
            else
            {
                if (!_hostList.LastAppliedFilter.Includes(this))
                    RemoveItem(true);
            }

            OnContentChangedEvent(EventArgs.Empty);
        }

        public void ShowRemoveAnimation(EventHandler completedHandler)
        {
            var duration = TimeSpan.FromMilliseconds(400);

            var rotateItem = new DoubleAnimation(1.0, 0.0, new Duration(duration));

            rotateItem.Completed += completedHandler;
            rotateItem.Completed += EndAnimationHandler;

            this.BeginAnimation(ThumbnailItem.OpacityProperty, rotateItem);
        }

        private void EndAnimationHandler(object sender, System.EventArgs e)
        {
            var duration = TimeSpan.FromMilliseconds(0);

            var rotateItem = new DoubleAnimation(0.0, 1.0, new System.Windows.Duration(duration));
            this.BeginAnimation(ThumbnailItem.OpacityProperty, rotateItem);
        }

        private bool LowQualityIndicatorNeeded()
        {
            PaperFormat curFormat = ExecutionEngine.Instance.SelectedPaperFormat;
            if (curFormat != null && !curFormat.IsFree && _photo.ImageSize.Width > 0 && _photo.ImageSize.Height > 0)
            {
                double curPixPerInch = Math.Max(_photo.ImageSize.Width / curFormat.Width, _photo.ImageSize.Height / curFormat.Height);
                if (ExecutionEngine.Config.PaperSizeUnits.Value == Constants.MmUnits)
                    curPixPerInch *= 25.4;

                return (curPixPerInch < curFormat.Dpi);
            }
            else
                return false;
        }

        private PhotoItem _photo;
        private OrderItem _orderItem;
        private ThumbnailList _hostList;
        private bool _isCorrupted;
        private bool _isActive;
    }
}