// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Aurigma.PhotoKiosk
{
    public partial class PhotoFrame : UserControl
    {
        private Canvas _canvas;
        private Image _photo;

        private OrderItem _item;
        private Size _previewSize;
        private Size _imageSize;
        private Size _frameSize;

        private Rect _rect;
        private double _scale;
        private bool _isDragged;
        private Point _dragPoint;

        public PhotoFrame()
        {
            InitializeComponent();

            _canvas = new Canvas();
            _canvas.Background = Brushes.White;
            _canvas.ClipToBounds = true;

            _photo = new Image();
            _photo.Stretch = Stretch.Uniform;
            _photo.StretchDirection = StretchDirection.DownOnly;
        }

        public OrderItem OrderItem
        {
            set { _item = value; }
        }

        public Image Photo
        {
            get { return _photo; }
            set { _photo = value; }
        }

        public Size ImageSize
        {
            get { return _imageSize; }
        }

        public void Reset()
        {
            _photo.PreviewMouseDown -= new MouseButtonEventHandler(PhotoMouseDownEventHandler);
            _photo.PreviewMouseMove -= new MouseEventHandler(PhotoMouseMoveEventHandler);
            _photo.PreviewMouseUp -= new MouseButtonEventHandler(PhotoMouseUpEventHandler);
            _photo.MouseLeave -= new MouseEventHandler(PhotoMouseLeaveEventHandler);

            _polygon12.Visibility = Visibility.Collapsed;
            _polygon21.Visibility = Visibility.Collapsed;
            _polygon23.Visibility = Visibility.Collapsed;
            _polygon32.Visibility = Visibility.Collapsed;

            _previewSize = new Size(_grid.ActualWidth * 0.88, _grid.ActualHeight * 0.88);

            if (GetMode() == ImageEditorMode.Crop)
            {
                if (ExecutionEngine.Instance.SelectedPaperFormat != null && !ExecutionEngine.Instance.SelectedPaperFormat.IsFree && !GetWhitespaces())
                {
                    Rect r = _item.GetCrop(ExecutionEngine.Instance.SelectedPaperFormat);
                    _frameSize = Inscribe(r.Size, _previewSize);
                    _imageSize = Describe(_item.SourcePhotoItem.ImageSize, _frameSize);

                    _photo.PreviewMouseDown += new MouseButtonEventHandler(PhotoMouseDownEventHandler);
                    _photo.PreviewMouseMove += new MouseEventHandler(PhotoMouseMoveEventHandler);
                    _photo.PreviewMouseUp += new MouseButtonEventHandler(PhotoMouseUpEventHandler);
                    _photo.MouseLeave += new MouseEventHandler(PhotoMouseLeaveEventHandler);

                    _polygon12.Visibility = Visibility.Visible;
                    _polygon21.Visibility = Visibility.Visible;
                    _polygon23.Visibility = Visibility.Visible;
                    _polygon32.Visibility = Visibility.Visible;

                    _scale = Math.Min(_frameSize.Width / r.Width, _frameSize.Height / r.Height);
                    _rect = new Rect(new Point(r.X * _scale, r.Y * _scale), _frameSize);

                    int cropWidth = (int)_item.GetCrop(ExecutionEngine.Instance.SelectedPaperFormat).Width;
                    int cropHeight = (int)_item.GetCrop(ExecutionEngine.Instance.SelectedPaperFormat).Height;
                    int imageWidth = (int)_item.SourcePhotoItem.ImageSize.Width;
                    int imageHeight = (int)_item.SourcePhotoItem.ImageSize.Height;

                    if (cropHeight < imageHeight && imageHeight - cropHeight > 1)
                    {
                        // Vertical arrows are enabled
                        _polygon21.Style = TryFindResource("PhotoFrameArrowEnabledStyle") as Style;
                        _polygon23.Style = TryFindResource("PhotoFrameArrowEnabledStyle") as Style;
                        _polygon12.Style = TryFindResource("PhotoFrameArrowDisabledStyle") as Style;
                        _polygon32.Style = TryFindResource("PhotoFrameArrowDisabledStyle") as Style;
                    }
                    else if (cropWidth < imageWidth && imageWidth - cropWidth > 1)
                    {
                        // Horizontal arrows are enabled
                        _polygon12.Style = TryFindResource("PhotoFrameArrowEnabledStyle") as Style;
                        _polygon32.Style = TryFindResource("PhotoFrameArrowEnabledStyle") as Style;
                        _polygon21.Style = TryFindResource("PhotoFrameArrowDisabledStyle") as Style;
                        _polygon23.Style = TryFindResource("PhotoFrameArrowDisabledStyle") as Style;
                    }
                    else
                    {
                        // Disable all arrows
                        _polygon12.Style = TryFindResource("PhotoFrameArrowDisabledStyle") as Style;
                        _polygon32.Style = TryFindResource("PhotoFrameArrowDisabledStyle") as Style;
                        _polygon21.Style = TryFindResource("PhotoFrameArrowDisabledStyle") as Style;
                        _polygon23.Style = TryFindResource("PhotoFrameArrowDisabledStyle") as Style;
                    }
                }
                else
                {
                    double ratio = GetRatio();
                    if (_item.SourcePhotoItem.ImageSize.Width < _item.SourcePhotoItem.ImageSize.Height)
                        ratio = 1 / ratio;
                    _frameSize.Height = _previewSize.Height;
                    _frameSize.Width = _previewSize.Height * ratio;
                    if (_frameSize.Width > _previewSize.Width)
                    {
                        _frameSize.Width = _previewSize.Width;
                        _frameSize.Height = _previewSize.Width / ratio;
                    }

                    _imageSize = Inscribe(_item.SourcePhotoItem.ImageSize, _frameSize);

                    _rect = new Rect(_frameSize);
                    _rect.X = (_imageSize.Width - _frameSize.Width) / 2;
                    _rect.Y = (_imageSize.Height - _frameSize.Height) / 2;
                }

                _border.Width = _frameSize.Width;
                _border.Height = _frameSize.Height;

                UpdateLocation();
            }
            else
                _imageSize = Inscribe(_item.SourcePhotoItem.ImageSize, _previewSize);
        }

        public void Update()
        {
            if (GetMode() != ImageEditorMode.Crop)
            {
                Size s = Inscribe(new Size(_photo.Source.Width, _photo.Source.Height), _previewSize);
                _border.Width = s.Width;
                _border.Height = s.Height;
            }
        }

        private ImageEditorMode GetMode()
        {
            return (ImageEditorMode)ExecutionEngine.Context[Constants.ImageEditorModeKey];
        }

        private bool GetWhitespaces()
        {
            Order order = (Order)ExecutionEngine.Context[Constants.OrderContextName];
            return (order.CropMode == Constants.ResizeToFitModeName);
        }

        private double GetRatio()
        {
            if (GetMode() == ImageEditorMode.Crop && ExecutionEngine.Instance.SelectedPaperFormat != null && !ExecutionEngine.Instance.SelectedPaperFormat.IsFree)
                return (double)ExecutionEngine.Instance.SelectedPaperFormat.Width / (double)ExecutionEngine.Instance.SelectedPaperFormat.Height;
            else
                return _item.SourcePhotoItem.ImageSize.Width / _item.SourcePhotoItem.ImageSize.Height;
        }

        private void UpdateLocation()
        {
            Canvas.SetLeft(_photo, -_rect.X);
            Canvas.SetTop(_photo, -_rect.Y);
        }

        private void PhotoFrame_Loaded(object sender, RoutedEventArgs e)
        {
            _canvas.Children.Clear();

            if ((ImageEditorMode)ExecutionEngine.Context[Constants.ImageEditorModeKey] == ImageEditorMode.Crop)
            {
                _border.Child = _canvas;
                _canvas.Children.Add(_photo);
            }
            else
            {
                _border.Child = _photo;
                _border.Width = double.NaN;
                _border.Height = double.NaN;
            }
        }

        private void PhotoMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            _isDragged = true;
            _dragPoint = e.GetPosition(_canvas);
        }

        private void PhotoMouseMoveEventHandler(object sender, MouseEventArgs e)
        {
            if (_isDragged)
            {
                double x = _rect.X - (e.GetPosition(_canvas).X - _dragPoint.X);
                double y = _rect.Y - (e.GetPosition(_canvas).Y - _dragPoint.Y);
                _rect.X = Math.Max(0, Math.Min(x, _imageSize.Width - _rect.Width));
                _rect.Y = Math.Max(0, Math.Min(y, _imageSize.Height - _rect.Height));

                UpdateLocation();

                _dragPoint = e.GetPosition(_canvas);
            }
        }

        private void PhotoMouseUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            _isDragged = false;
            if (ExecutionEngine.Instance.SelectedPaperFormat != null)
                _item.SetCrop(ExecutionEngine.Instance.SelectedPaperFormat, new Rect(_rect.X / _scale, _rect.Y / _scale, _rect.Width / _scale, _rect.Height / _scale));
        }

        private void PhotoMouseLeaveEventHandler(object sender, MouseEventArgs e)
        {
            if (_isDragged)
            {
                _isDragged = false;
                if (ExecutionEngine.Instance.SelectedPaperFormat != null)
                    _item.SetCrop(ExecutionEngine.Instance.SelectedPaperFormat, new Rect(_rect.X / _scale, _rect.Y / _scale, _rect.Width / _scale, _rect.Height / _scale));
            }
        }

        public static Size Inscribe(Size src, Size dst)
        {
            double height, width;
            double ratio = src.Width / src.Height;

            height = Math.Min(dst.Height, src.Height);
            width = height * ratio;
            if (width > dst.Width)
            {
                width = Math.Min(dst.Width, src.Width);
                height = width / ratio;
            }

            return new Size(width, height);
        }

        public static Size Describe(Size src, Size dst)
        {
            if (src.Width > dst.Width && src.Height > dst.Height)
            {
                double height, width;
                double ratio = src.Width / src.Height;

                width = dst.Width;
                height = width / ratio;
                if (height < dst.Height)
                {
                    height = dst.Height;
                    width = height * ratio;
                }

                return new Size(width, height);
            }
            else
                return src;
        }
    }
}