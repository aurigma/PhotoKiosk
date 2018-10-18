// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Drawing;
using System.IO;

namespace Aurigma.PhotoKiosk
{
    public class LoadImageQueueItem : IQueueItem
    {
        private ThumbnailItem _item;
        private Size _thumbnailSize;

        public LoadImageQueueItem(ThumbnailItem item, Size thumbnailSize)
        {
            _item = item;
            _thumbnailSize = thumbnailSize;
        }

        public void Process()
        {
            try
            {
                _item.IsCorrupted = true;
                _item.Photo.LoadImage(_thumbnailSize);
                _item.IsCorrupted = false;
            }
            catch (Aurigma.GraphicsMill.Exception e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
            }
            catch (IOException e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.Write("System.Exception");
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                throw;
            }
            _item.RefreshPhoto();
        }
    }
}