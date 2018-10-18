// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.Threading;

namespace Aurigma.PhotoKiosk
{
    public class AsyncThumbnailsLoader : IDisposable
    {
        private const int CurrentQueuePackIndex = 0;
        private const int NextQueuePackIndex = 1;
        private const int PrevQueuePackIndex = 2;

        private IList<ThumbnailItem> _thumbnailItems;
        private ThumbnailList _thumbnailList;
        private QueueProcessingThread _workerThread;
        private bool _isDisposed;

        public AsyncThumbnailsLoader(IList<ThumbnailItem> itemsList, ThumbnailList list)
        {
            _thumbnailItems = itemsList;
            _thumbnailList = list;

            _workerThread = new QueueProcessingThread(ThreadPriority.BelowNormal, Thread.CurrentThread);
            ExecutionEngine.EventLogger.Write("AsyncThumbnailsLoader created");
        }

        ~AsyncThumbnailsLoader()
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

                if (_workerThread != null)
                {
                    _workerThread.Dispose();
                    _workerThread = null;
                }
            }
        }

        private void CheckDisposedState()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("AsyncThumbnailsLoader");
        }

        public void Stop()
        {
            ExecutionEngine.EventLogger.Write("AsyncThumbnailsLoader:Stop");
            CheckDisposedState();
            _workerThread.Stop();
        }

        public void Clear()
        {
            ExecutionEngine.EventLogger.Write("AsyncThumbnailsLoader:ClearQueues");
            CheckDisposedState();

            _workerThread.ReloadQueue(delegate (IList<QueueItemPack> queue)
            {
                queue[PrevQueuePackIndex].Clear();
                queue[CurrentQueuePackIndex].Clear();
                queue[NextQueuePackIndex].Clear();
            });
        }

        public void ReloadItems()
        {
            ExecutionEngine.EventLogger.Write("AsyncThumbnailsLoader:ReloadItems");
            CheckDisposedState();

            if (_thumbnailList.FirstVisibleItemIndex < 0 || _thumbnailList.ItemsPerScreen <= 0)
                return;

            _workerThread.ReloadQueue(delegate (IList<QueueItemPack> queue)
            {
                if (queue.Count != 3)
                    throw new ArgumentException("Queue should contain 3 packs only.", "queue");

                queue[0].Clear();
                queue[1].Clear();
                queue[2].Clear();

                int firstIndex, lastIndex;
                firstIndex = _thumbnailList.FirstVisibleItemIndex;
                lastIndex = Math.Min(firstIndex + _thumbnailList.ItemsPerScreen, _thumbnailItems.Count);
                PlaceItemsToPack(queue[CurrentQueuePackIndex], firstIndex, lastIndex);

                firstIndex = lastIndex;
                lastIndex = Math.Min(_thumbnailItems.Count, _thumbnailList.FirstVisibleItemIndex + 2 * _thumbnailList.ItemsPerScreen);
                PlaceItemsToPack(queue[NextQueuePackIndex], firstIndex, lastIndex);

                firstIndex = Math.Max(0, _thumbnailList.FirstVisibleItemIndex - _thumbnailList.ItemsPerScreen);
                lastIndex = _thumbnailList.FirstVisibleItemIndex - 1;
                PlaceItemsToPack(queue[PrevQueuePackIndex], firstIndex, lastIndex);
            });

            _workerThread.Start();
        }

        private void PlaceItemsToPack(QueueItemPack pack, int firstItemIndex, int lastItemIndex)
        {
            List<IQueueItem> exifGetQueue = new List<IQueueItem>();
            List<IQueueItem> thumbGetQueue = new List<IQueueItem>();

            for (int i = firstItemIndex; i < lastItemIndex; i++)
            {
                exifGetQueue.Add(new LoadImageExifQueueItem(_thumbnailItems[i]));
                thumbGetQueue.Add(new LoadImageQueueItem(_thumbnailItems[i], _thumbnailList.ThumbnailSize));
            }

            pack.Add(exifGetQueue);
            pack.Add(thumbGetQueue);
        }
    }
}