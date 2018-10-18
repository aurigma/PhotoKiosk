// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.GraphicsMill;
using Aurigma.GraphicsMill.Codecs;
using Aurigma.GraphicsMill.Transforms;
using Aurigma.PhotoKiosk.Core;
using System;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Aurigma.PhotoKiosk
{
    public class PhotoItem : ICloneable, IDisposable
    {
        private enum PhotoItemState
        {
            Empty = 0,
            Cached = 1
        }

        private static PhotoItemsCache _cache = new PhotoItemsCache();
        private static int _generatedFileNameIndex;

        private string _sourceFileName;
        private string _hddFileName = "";
        private Bitmap _image;
        private Bitmap _exifThumbnail;
        private int _exifAngle;
        private Size _imageSize;
        private PhotoItemState _itemState;
        private bool _isDisposed;

        public PhotoItem(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            _image = new Bitmap();
            _itemState = PhotoItemState.Empty;
            _sourceFileName = fileName;
            _exifAngle = 0;
        }

        ~PhotoItem()
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
                if (_image != null)
                    _image.Dispose();
                if (_exifThumbnail != null)
                    _exifThumbnail.Dispose();

                _isDisposed = true;
            }
        }

        private void CheckDisposedState()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("PhotoItem");
        }

        public object Clone()
        {
            CheckDisposedState();

            PhotoItem item = new PhotoItem(_sourceFileName);
            item._itemState = _itemState;
            item._imageSize = _imageSize;

            lock (this)
            {
                if (_exifThumbnail != null)
                {
                    item._exifThumbnail = new Bitmap();
                    item._exifThumbnail.Create(_exifThumbnail);
                }

                if (_image != null)
                {
                    item._image = new Bitmap();
                    item._image.Create(_image);
                }
            }

            item._hddFileName = _hddFileName;
            return item;
        }

        public void LoadImage(System.Drawing.Size size)
        {
            CheckDisposedState();
            lock (this)
            {
                if (_itemState == PhotoItemState.Cached)
                {
                    if (size.Width > _image.Width && size.Height > _image.Height)
                    {
                        _image.LoadThumbnail(_hddFileName, (int)size.Width, (int)size.Height);
                        RotateImage();
                    }
                    else
                    {
                        int resultWidth = size.Width, resultHeight = size.Height;
                        Resize.CalculateProportionalDimensions(_image.Width, _image.Height, ref resultWidth, ref resultHeight, Aurigma.GraphicsMill.Transforms.ResizeMode.Fit);
                        if (_image.Width != resultWidth || _image.Height != resultHeight)
                        {
                            _image.Transforms.Resize(resultWidth, resultHeight);
                        }
                    }
                }
                else if (_itemState == PhotoItemState.Empty)
                {
                    _hddFileName = PhotoItem.Cache.CacheSource(this);
                    _image.LoadThumbnail(_hddFileName, (int)size.Width, (int)size.Height);

                    using (IFormatReader formatReader = FormatManager.CreateFormatReader(_hddFileName))
                    using (IFrame frame = formatReader.LoadFrame(0))
                    {
                        _imageSize = new Size(frame.Width, frame.Height);
                    }
                    RotateImage();
                    _itemState = PhotoItemState.Cached;
                }
                else
                {
                    ExecutionEngine.EventLogger.Write("Unknown PhotoItem state");
                }
            }
        }

        public void LoadImage()
        {
            CheckDisposedState();

            if (_hddFileName.Length > 0)
            {
                _image.Load(_hddFileName);
            }
            else
            {
                _hddFileName = PhotoItem.Cache.CacheSource(this);
                _image.Load(_hddFileName);
                _itemState = PhotoItemState.Cached;
            }
            RotateImage();
        }

        public Bitmap LoadExifThumbnail()
        {
            CheckDisposedState();

            if (_exifThumbnail == null)
            {
                IFormatReader formatReader = FormatManager.CreateFormatReader(_sourceFileName);
                try
                {
                    if (formatReader.MediaFormat == FormatManager.JpegFormat)
                    {
                        JpegReader jpgReader = (JpegReader)formatReader;
                        if (jpgReader.Exif != null && jpgReader.Exif.Contains(ExifDictionary.Thumbnail))
                        {
                            _exifThumbnail = (Bitmap)jpgReader.Exif[ExifDictionary.Thumbnail];

                            if (jpgReader.Exif.Contains(ExifDictionary.Orientation))
                                _exifAngle = ExifOrientationToAngle(jpgReader.Exif[ExifDictionary.Orientation]);

                            if (_exifAngle > 0 && !_exifThumbnail.IsEmpty)
                                _exifThumbnail.Transforms.Rotate(_exifAngle);

                            return _exifThumbnail;
                        }
                    }
                    else if (formatReader.MediaFormat == FormatManager.TiffFormat)
                    {
                        TiffReader tiffReader = (TiffReader)formatReader;
                        if (tiffReader.Exif != null && tiffReader.Exif.Contains(ExifDictionary.Thumbnail))
                        {
                            _exifThumbnail = (Bitmap)tiffReader.Exif[ExifDictionary.Thumbnail];

                            if (tiffReader.Exif.Contains(ExifDictionary.Orientation))
                                _exifAngle = ExifOrientationToAngle(tiffReader.Exif[ExifDictionary.Orientation]);

                            if (_exifAngle > 0 && !_exifThumbnail.IsEmpty)
                                _exifThumbnail.Transforms.Rotate(_exifAngle);

                            return _exifThumbnail;
                        }
                    }
                    else if (formatReader.MediaFormat == FormatManager.PsdFormat)
                    {
                        PsdReader psdReader = (PsdReader)formatReader;
                        if (psdReader.Exif != null && psdReader.Exif.Contains(ExifDictionary.Thumbnail))
                        {
                            _exifThumbnail = (Bitmap)psdReader.Exif[ExifDictionary.Thumbnail];

                            if (psdReader.Exif.Contains(ExifDictionary.Orientation))
                                _exifAngle = ExifOrientationToAngle(psdReader.Exif[ExifDictionary.Orientation]);

                            if (_exifAngle > 0 && !_exifThumbnail.IsEmpty)
                                _exifThumbnail.Transforms.Rotate(_exifAngle);

                            return _exifThumbnail;
                        }
                    }
                }
                finally
                {
                    if (formatReader != null)
                    {
                        formatReader.Close();
                        formatReader.Dispose();
                    }
                }
            }
            return _exifThumbnail;
        }

        public BitmapSource GetBitmapSource()
        {
            CheckDisposedState();

            lock (this)
            {
                if (_image.IsEmpty)
                {
                    if (_exifThumbnail != null && !_exifThumbnail.IsEmpty)
                    {
                        return CreateBitmapSource(_exifThumbnail);
                    }
                    else
                    {
                        if (ExecutionEngine.Instance.Resource.Contains(Constants.ImagePleaseWaitKey))
                            return (BitmapFrame)ExecutionEngine.Instance.Resource[Constants.ImagePleaseWaitKey];
                        else
                            return null;
                    }
                }
                else
                {
                    PhotoItem.Cache.RegisterItemAccessTime(this);
                    return CreateBitmapSource(_image);
                }
            }
        }

        public void UpdateSource(Bitmap source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            CheckDisposedState();

            lock (this)
            {
                _image = source;
                _exifAngle = 0;

                if (_itemState == PhotoItemState.Empty)
                    _hddFileName = PhotoItem.Cache.CacheSource(this);
                else if (_itemState == PhotoItemState.Cached)
                    _image.Save(_hddFileName, new JpegEncoderOptions(100, true));

                _imageSize.Width = _image.Width;
                _imageSize.Height = _image.Height;
            }
        }

        public void CreateCacheData()
        {
            if (_image == null)
                throw new ArgumentNullException("_image");

            CheckDisposedState();

            _exifThumbnail = _image;
            _hddFileName = PhotoItem.Cache.CacheThumbnail(this);
            _sourceFileName = _hddFileName;
            _imageSize.Width = _image.Width;
            _imageSize.Height = _image.Height;

            _itemState = PhotoItemState.Cached;
        }

        private void RotateImage()
        {
            if (_exifAngle > 0)
            {
                if (!_image.IsEmpty)
                    _image.Transforms.Rotate(_exifAngle);

                if (_exifAngle % 180 != 0 && _itemState == PhotoItemState.Empty)
                {
                    double tmp = _imageSize.Width;
                    _imageSize.Width = _imageSize.Height;
                    _imageSize.Height = tmp;
                }
            }
        }

        public static PhotoItemsCache Cache
        {
            get { return _cache; }
        }

        public static int GeneratedFileNameIndex
        {
            get { return _generatedFileNameIndex++; }
        }

        public string ActualFileName
        {
            get
            {
                CheckDisposedState();

                if (PhotoItemState.Cached == _itemState)
                    return _hddFileName;
                else
                    return _sourceFileName;
            }
        }

        public string SourceFileNameWithoutPath
        {
            get
            {
                CheckDisposedState();
                return Path.GetFileName(_sourceFileName);
            }
        }

        public Aurigma.GraphicsMill.Bitmap Image
        {
            get
            {
                CheckDisposedState();
                return _image;
            }
        }

        public string SourceFileName
        {
            get
            {
                CheckDisposedState();
                return _sourceFileName;
            }
        }

        public Bitmap ExifThumbnail
        {
            get
            {
                CheckDisposedState();
                return _exifThumbnail;
            }
        }

        public int MemoryUsed
        {
            get
            {
                CheckDisposedState();

                // Approximation
                if (_image != null)
                    return _image.MemoryUsed;
                else
                    return 0;
            }
        }

        public int UndoStepCount
        {
            get
            {
                CheckDisposedState();

                if (_image != null && !_image.IsEmpty)
                    return _image.UndoStepCount;
                else
                    return 0;
            }
        }

        public Size ImageSize
        {
            get
            {
                if (_imageSize.IsEmpty || (_imageSize.Width == 0 && _imageSize.Height == 0))
                {
                    try
                    {
                        using (Aurigma.GraphicsMill.Codecs.IFormatReader formatReader = Aurigma.GraphicsMill.Codecs.FormatManager.CreateFormatReader(ActualFileName))
                        using (Aurigma.GraphicsMill.Codecs.IFrame frame = formatReader.LoadFrame(0))
                        {
                            _imageSize = new Size(frame.Width, frame.Height);
                        }
                        RotateImage();
                    }
                    catch (System.Exception e)
                    {
                        ExecutionEngine.EventLogger.Write("Unable to get actual image size of " + ActualFileName);
                        ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                    }
                }
                return _imageSize;
            }
        }

        public int ExifAngle
        {
            get { return _exifAngle; }
        }

        public static BitmapSource CreateBitmapSource(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            if (bitmap.IsCmyk || bitmap.IsExtended || bitmap.IsGrayScale)
                bitmap.ColorManagement.Convert(PixelFormat.Format24bppRgb);

            IntPtr hBitmap = bitmap.Handle;

            BitmapSource result = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return result;
        }

        private static int ExifOrientationToAngle(Object exifOrientation)
        {
            try
            {
                uint orientation = (uint)exifOrientation;
                switch (orientation)
                {
                    case 8:
                        return 270;

                    case 3:
                        return 180;

                    case 6:
                        return 90;

                    case 1:
                    default:
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}