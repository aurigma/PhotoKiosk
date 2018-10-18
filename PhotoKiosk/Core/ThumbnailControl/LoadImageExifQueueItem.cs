// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.IO;

namespace Aurigma.PhotoKiosk
{
    public class LoadImageExifQueueItem : IQueueItem
    {
        private ThumbnailItem _item;

        public LoadImageExifQueueItem(ThumbnailItem item)
        {
            _item = item;
        }

        public void Process()
        {
            if (_item.Photo.ExifThumbnail == null)
            {
                try
                {
                    _item.Photo.LoadExifThumbnail();
                    _item.RefreshPhoto();
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
            }
        }
    }
}