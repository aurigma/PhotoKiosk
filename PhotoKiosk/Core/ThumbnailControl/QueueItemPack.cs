// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace Aurigma.PhotoKiosk
{
    public class QueueItemPack
    {
        private List<IQueueItem> _itemQueue = new List<IQueueItem>();

        public QueueItemPack()
        {
        }

        public void Add(IQueueItem item)
        {
            lock (_itemQueue)
            {
                _itemQueue.Add(item);
            }
        }

        public void Add(IList<IQueueItem> queue)
        {
            lock (_itemQueue)
            {
                foreach (IQueueItem item in queue)
                {
                    _itemQueue.Add(item);
                }
            }
        }

        public IQueueItem PopFirstItem()
        {
            IQueueItem item = null;
            lock (_itemQueue)
            {
                if (_itemQueue.Count > 0)
                {
                    item = _itemQueue[0];
                    _itemQueue.RemoveAt(0);
                }
            }
            return item;
        }

        public void Clear()
        {
            lock (_itemQueue)
            {
                _itemQueue.Clear();
            }
        }

        public IList<IQueueItem> ItemQueue
        {
            get { return _itemQueue; }
        }
    }
}