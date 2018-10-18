// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.GraphicsMill.Codecs;
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;

namespace Aurigma.PhotoKiosk
{
    public class PhotoItemsCache
    {
        private const string BaseName = "PK_item_";
        private const string TempSubfolder = @"PhotoKiosk\";

        private string _cacheDir;
        private static int _itemIndex;

        private Hashtable _cachedSources;
        private List<KeyValuePair<PhotoItem, int>> _lastAccessedItems;
        private int _memoryAllocatedForItems;
        private int _maxCacheSize;

        public PhotoItemsCache()
        {
            _cachedSources = new Hashtable();
            _lastAccessedItems = new List<KeyValuePair<PhotoItem, int>>();
            _cacheDir = Path.GetTempPath() + TempSubfolder;

            if (Directory.Exists(_cacheDir))
            {
                try
                {
                    Directory.Delete(_cacheDir, true);
                }
                catch (IOException e)
                {
                    ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                }
                catch (UnauthorizedAccessException e)
                {
                    ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                }
            }

            /*
            what for:
                    if the cache will be placed in existing folder we'll have to clear it's content before usage.
                    In order to avoid extra loading app time we create new folder with unique name.
             */
            _cacheDir += Guid.NewGuid().ToString(null, CultureInfo.InvariantCulture) + @"\";

            try
            {
                Directory.CreateDirectory(_cacheDir);
            }
            catch (IOException e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
            }
            catch (UnauthorizedAccessException e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
            }

            // MaxCacheSize in megabytes
            _maxCacheSize = ExecutionEngine.Config.MemoryCacheSize.Value * 1024 * 1024;
            ExecutionEngine.EventLogger.Write("PhotoItemsCache created");
        }

        ~PhotoItemsCache()
        {
            try
            {
                Clear(true);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "~PhotoItemsCache(), {0}", e.Message));
                throw;
            }
        }

        public string CacheSource(PhotoItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            string cachedFileName;
            lock (_cachedSources)
            {
                if (_cachedSources.Contains(item.SourceFileName))
                {
                    cachedFileName = (string)_cachedSources[item.SourceFileName];
                }
                else
                {
                    cachedFileName = GenerateSourceFileName();

                    try
                    {
                        File.Copy(item.SourceFileName, cachedFileName);
                        File.SetAttributes(cachedFileName, FileAttributes.Temporary);
                        _cachedSources.Add(item.SourceFileName, cachedFileName);
                    }
                    catch (IOException e)
                    {
                        ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                    }
                    catch (Exception e)
                    {
                        ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                        throw;
                    }
                }
            }

            return cachedFileName;
        }

        public string CacheThumbnail(PhotoItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (item.Image != null && !item.Image.IsEmpty)
            {
                string newFileName = GenerateEditedFileName();

                item.Image.Save(newFileName, new JpegEncoderOptions());
                return newFileName;
            }
            else
            {
                throw new Aurigma.GraphicsMill.Exception("There is no Thumbnail to cache.");
            }
        }

        public void Clear(bool withFolder)
        {
            ExecutionEngine.EventLogger.Write("PhotoItemsCache:Clear");

            lock (_cachedSources)
            {
                _cachedSources.Clear();
            }

            lock (_lastAccessedItems)
            {
                _lastAccessedItems.Clear();
            }
            _memoryAllocatedForItems = 0;

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(_cacheDir);
                foreach (FileInfo fileInfo in dirInfo.GetFiles())
                {
                    try
                    {
                        fileInfo.Attributes = FileAttributes.Archive;
                        File.Delete(fileInfo.FullName);
                    }
                    catch (IOException e)
                    {
                        ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                    }
                }

                if (withFolder)
                {
                    Directory.Delete(_cacheDir);
                }
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
            }

            Interlocked.Exchange(ref _itemIndex, 0);
        }

        // Why thread sync is absent here:
        //      RegisterItemAccessTime is called only during BitmapSource requesting.
        //      GUI changes can be made only from owner WPF thread => only one WPF thread can
        //      get BitmapSource => thread sync is not necessary at this time.
        //                                              I hope so...
        //                                                              Eugene Kosmin

        public void RegisterItemAccessTime(PhotoItem item)
        {
            int oldItemIndex = _lastAccessedItems.FindIndex(delegate (KeyValuePair<PhotoItem, int> itemToCompare)
            {
                return (itemToCompare.Key == item);
            });

            if (oldItemIndex >= 0)
            {
                _memoryAllocatedForItems += item.MemoryUsed - _lastAccessedItems[oldItemIndex].Value;

                _lastAccessedItems.RemoveAt(oldItemIndex);
                _lastAccessedItems.Add(new KeyValuePair<PhotoItem, int>(item, item.MemoryUsed));
            }
            else
            {
                _lastAccessedItems.Add(new KeyValuePair<PhotoItem, int>(item, item.MemoryUsed));
                _memoryAllocatedForItems += item.MemoryUsed;
            }

            if (_memoryAllocatedForItems >= _maxCacheSize)
            {
                DisposeLastPhotoItems();
            }
        }

        private string GenerateSourceFileName()
        {
            string result = null;
            lock (this)
            {
                result = _GenerateSourceFileName();
            }
            return result;
        }

        private string GenerateEditedFileName()
        {
            return string.Format(CultureInfo.InvariantCulture, _cacheDir + Constants.EditedPhotoFileNameFormat, PhotoItem.GeneratedFileNameIndex);
        }

        private string _GenerateSourceFileName()
        {
            Interlocked.Increment(ref _itemIndex);
            return string.Format(CultureInfo.InvariantCulture, _cacheDir + BaseName + "{0:D4}.tmp", _itemIndex);
        }

        private void DisposeLastPhotoItems()
        {
            ExecutionEngine.EventLogger.Write("PhotoItemsCache:DisposeLastPhotoItems");
            int disposedMemoryCount = 0;
            int i = 0;

            for (; _memoryAllocatedForItems - disposedMemoryCount >= _maxCacheSize * Constants.CacheClearingRatio && i < _lastAccessedItems.Count; i++)
            {
                disposedMemoryCount += _lastAccessedItems[i].Value;
                _lastAccessedItems[i].Key.Image.Unload();
            }

            System.GC.Collect();

            _lastAccessedItems.RemoveRange(0, i);
            _memoryAllocatedForItems -= disposedMemoryCount;
        }
    }
}