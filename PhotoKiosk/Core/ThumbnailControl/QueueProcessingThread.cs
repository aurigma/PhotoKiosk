// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.Threading;

namespace Aurigma.PhotoKiosk
{
    public interface IQueueItem
    {
        void Process();
    }

    public class QueueProcessingThread : IDisposable
    {
        private const int WaitInterval = 200;

        private Thread _workingThread;
        private Thread _callingThread;
        private bool _isStopped;
        private AutoResetEvent _startGetting;
        private AutoResetEvent _processStopped;
        private List<QueueItemPack> _queue;
        private bool _isDisposed;

        public delegate void ReloadQueueHandler(IList<QueueItemPack> queue);

        public QueueProcessingThread(ThreadPriority priority, Thread callingThread)
        {
            _isStopped = true;
            _startGetting = new AutoResetEvent(false);
            _processStopped = new AutoResetEvent(false);
            _queue = new List<QueueItemPack>();

            _workingThread = new Thread(new ThreadStart(ProcessItems));
            _workingThread.Priority = priority;
            _callingThread = callingThread;

            _queue.Add(new QueueItemPack());
            _queue.Add(new QueueItemPack());
            _queue.Add(new QueueItemPack());

            try
            {
                _workingThread.Start();
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                throw;
            }

            ExecutionEngine.EventLogger.Write("QueueProcessingThread created");
        }

        ~QueueProcessingThread()
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
                if (_startGetting != null)
                {
                    _startGetting.Close();
                    _startGetting = null;
                }
                if (_processStopped != null)
                {
                    _processStopped.Close();
                    _processStopped = null;
                }

                _isDisposed = true;
            }
        }

        private void CheckDisposedState()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("QueueProcessingThread");
        }

        public void Stop()
        {
            ExecutionEngine.EventLogger.Write("QueueProcessingThread:Stop");
            CheckDisposedState();

            if (!_isStopped)
            {
                try
                {
                    _processStopped.Reset();
                    _isStopped = true;
                    _processStopped.WaitOne();
                }
                catch (Exception e)
                {
                    ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                    throw;
                }
            }
        }

        public void Start()
        {
            ExecutionEngine.EventLogger.Write("QueueProcessingThread:Start");
            CheckDisposedState();

            if (_isStopped)
            {
                _isStopped = false;
                _startGetting.Set();
            }
        }

        public void ReloadQueue(ReloadQueueHandler callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            CheckDisposedState();

            lock (this)
            {
                callback(_queue);
            }
        }

        private bool IsCallerThreadAlive()
        {
            return ((_callingThread.ThreadState & ThreadState.Stopped) == 0);
        }

        private IQueueItem GetNextQueuedItem()
        {
            IQueueItem result = null;

            lock (this)
            {
                int currentPackIndex = 0;
                result = _queue[currentPackIndex].PopFirstItem();
                while (result == null && currentPackIndex < _queue.Count - 1)
                    result = _queue[++currentPackIndex].PopFirstItem();
            }

            return result;
        }

        private void ProcessItems()
        {
            ExecutionEngine.EventLogger.Write("QueueProcessingThread:ProcessItems");
            try
            {
                while (IsCallerThreadAlive())
                {
                    while (!_isStopped)
                    {
                        IQueueItem currentItem = GetNextQueuedItem();

                        if (!_isStopped && currentItem != null && IsCallerThreadAlive())
                        {
                            currentItem.Process();
                        }
                        else
                            _isStopped = true;
                    }

                    _processStopped.Set();
                    _startGetting.WaitOne(WaitInterval, false);

                    ExecutionEngine.EventLogger.Write("QueueProcessingThread:ProcessItems:1");
                }
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                throw;
            }

            ExecutionEngine.EventLogger.Write("QueueProcessingThread:ProcessItem:2");
        }
    }
}