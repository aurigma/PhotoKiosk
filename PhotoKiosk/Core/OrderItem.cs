// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Aurigma.PhotoKiosk
{
    public class OrderItem
    {
        private PhotoItem _photoItem;
        private string _orderStoreFileName;
        private Dictionary<PaperFormat, int> _photoCounts;
        private Dictionary<PaperFormat, Rect> _photoCrops;

        private bool _isActive;
        private OutlinedText _imprintsCountText;
        private Image _imageCommandImage;
        private TextBlock _imageCommandText;
        private Button _imageCommandButton;

        public OrderItem(PhotoItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _photoItem = item;
            _photoCounts = new Dictionary<PaperFormat, int>(ExecutionEngine.Instance.PaperFormats.Count);
            _photoCrops = new Dictionary<PaperFormat, Rect>(ExecutionEngine.Instance.PaperFormats.Count);

            foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
            {
                _photoCounts.Add(format, 0);
                _photoCrops.Add(format, new Rect());
            }

            _imprintsCountText = new OutlinedText();
            _imprintsCountText.Style = (Style)_imprintsCountText.FindResource("ImprintsCountTextStyle");

            _imageCommandButton = new Button();
            _imageCommandButton.Style = (Style)_imageCommandButton.FindResource("ImageCommandButtonStyle");

            var grid = new Grid();
            grid.Width = 60;
            grid.Height = 60;
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions[0].Height = new GridLength(40);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions[1].Height = new GridLength(20);

            _imageCommandImage = new Image();
            _imageCommandImage.Stretch = Stretch.None;

            Grid.SetRow(_imageCommandImage, 0);

            _imageCommandText = new TextBlock();
            _imageCommandText.Margin = new Thickness(3, -5, 3, 0);
            _imageCommandText.Style = (Style)_imageCommandText.FindResource("ImageCommandTextStyle");
            _imageCommandText.VerticalAlignment = VerticalAlignment.Center;
            _imageCommandText.HorizontalAlignment = HorizontalAlignment.Center;
            _imageCommandText.TextAlignment = TextAlignment.Center;
            _imageCommandText.LineHeight = 10;
            _imageCommandText.LineStackingStrategy = LineStackingStrategy.BlockLineHeight;
            Grid.SetRow(_imageCommandText, 1);

            grid.Children.Add(_imageCommandImage);
            grid.Children.Add(_imageCommandText);
            _imageCommandButton.Content = grid;
        }

        public void Reset()
        {
            bool firstAdded = false;
            foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
            {
                if (!firstAdded)
                {
                    _photoCounts[format] = 1;
                    SetCrop(format);
                    firstAdded = true;
                }
                else
                {
                    _photoCounts[format] = 0;
                    ClearCrop(format);
                }
            }

            UpdateImprintsCountText();
        }

        public void ClearCrop(PaperFormat format)
        {
            _photoCrops[format] = new Rect();
        }

        public void SetCrop(PaperFormat format)
        {
            Rect crop = new Rect(_photoItem.ImageSize);

            if (!format.IsFree && (ExecutionEngine.Context[Constants.OrderContextName] as Order).CropMode == Constants.CropToFillModeName)
            {
                double ratio = (double)format.Width / (double)format.Height;
                if (_photoItem.ImageSize.Height > _photoItem.ImageSize.Width)
                    ratio = 1 / ratio;

                crop.Height = _photoItem.ImageSize.Height;
                crop.Width = _photoItem.ImageSize.Height * ratio;
                if (crop.Width > _photoItem.ImageSize.Width)
                {
                    crop.Width = _photoItem.ImageSize.Width;
                    crop.Height = _photoItem.ImageSize.Width / ratio;
                }

                crop.X = (_photoItem.ImageSize.Width - crop.Width) / 2;
                crop.Y = (_photoItem.ImageSize.Height - crop.Height) / 2;
            }

            _photoCrops[format] = crop;
        }

        public void SetCrop(PaperFormat format, Rect crop)
        {
            var rect = new Rect(_photoItem.ImageSize);

            if (!format.IsFree && !rect.Contains(crop))
                crop.Intersect(rect);

            _photoCrops[format] = crop;
        }

        public Rect GetCrop(PaperFormat format)
        {
            return _photoCrops[format];
        }

        public string GetCropString(PaperFormat format)
        {
            Rect rect = GetCrop(format);

            int angle = _photoItem.ExifAngle;
            if (angle > 0)
            {
                double tmp;
                switch (angle)
                {
                    case 270:
                        tmp = SourcePhotoItem.ImageSize.Height - rect.Height - rect.Y;
                        rect.Y = rect.X;
                        rect.X = tmp;
                        tmp = rect.Width;
                        rect.Width = rect.Height;
                        rect.Height = tmp;
                        break;

                    case 180:
                        rect.X = SourcePhotoItem.ImageSize.Width - rect.Width - rect.X;
                        rect.Y = SourcePhotoItem.ImageSize.Height - rect.Height - rect.Y;
                        break;

                    case 90:
                        tmp = SourcePhotoItem.ImageSize.Width - rect.Width - rect.X;
                        rect.X = rect.Y;
                        rect.Y = tmp;
                        tmp = rect.Width;
                        rect.Width = rect.Height;
                        rect.Height = tmp;
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }

            return string.Format("{0}, {1}, {2}, {3}", (int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        public void SetCount(PaperFormat format, int count)
        {
            _photoCounts[format] = count;
        }

        public int GetCount(PaperFormat format)
        {
            return _photoCounts[format];
        }

        public bool IsEmpty()
        {
            int totalCount = 0;
            foreach (int count in _photoCounts.Values)
            {
                if (totalCount == 0)
                    totalCount += count;
                else
                    return false;
            }

            return (totalCount == 0);
        }

        public void UpdateImprintsCountText()
        {
            int count = 0;
            if (ExecutionEngine.Instance.SelectedPaperFormat == null)
            {
                foreach (int photoCount in _photoCounts.Values)
                    count += photoCount;
            }
            else
                count = _photoCounts[ExecutionEngine.Instance.SelectedPaperFormat];

            _imprintsCountText.TextContent = count.ToString();
        }

        public void UpdateImageCommandButton()
        {
            if (ExecutionEngine.Instance.SelectedPaperFormat != null)
            {
                if ((ExecutionEngine.Context[Constants.OrderContextName] as Order).CropMode == Constants.CropToFillModeName && !ExecutionEngine.Instance.SelectedPaperFormat.IsFree)
                {
                    _imageCommandImage.Source = ExecutionEngine.Instance.Resource[Constants.ImageSetCropFrameButtonKey] as ImageSource;
                    _imageCommandText.Text = ExecutionEngine.Instance.Resource[Constants.ThumbnailSetFrameTextKey] as string;
                    _imageCommandButton.Command = ThumbnailItem.ShowImageCropCommand;
                }
                else
                {
                    _imageCommandImage.Source = ExecutionEngine.Instance.Resource[Constants.ImageMagnifierKey] as ImageSource;
                    _imageCommandText.Text = ExecutionEngine.Instance.Resource[Constants.ThumbnailPreviewTextKey] as string;
                    _imageCommandButton.Command = ThumbnailItem.ShowImageViewerCommand;
                }
            }
            else if (ExecutionEngine.Config.IsImageEditorEnabled())
            {
                _imageCommandButton.Command = ThumbnailItem.ShowImageEditorCommand;
                _imageCommandImage.Source = ExecutionEngine.Instance.Resource[Constants.ImageEditKey] as ImageSource;
                _imageCommandText.Text = ExecutionEngine.Instance.Resource[Constants.ThumbnailEditTextKey] as string;
            }
            else
                _imageCommandButton.Visibility = Visibility.Collapsed;
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public PhotoItem SourcePhotoItem
        {
            get { return _photoItem; }
        }

        public string OrderStoreFileName
        {
            get { return _orderStoreFileName; }
            set { _orderStoreFileName = value; }
        }

        public OutlinedText ImprintsCountText
        {
            get
            {
                UpdateImprintsCountText();
                return _imprintsCountText;
            }
        }

        public Button ImageCommandButton
        {
            get
            {
                UpdateImageCommandButton();
                return _imageCommandButton;
            }
        }
    }
}