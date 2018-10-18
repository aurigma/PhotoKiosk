// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using System.Threading;

namespace Aurigma.PhotoKiosk
{
    // Note: Just after creation Enabled property of the object is set to false by default.
    public abstract class LogBase
    {
        private bool _enabled;

        protected LogBase(bool catchUnhandledExceptions)
        {
            if (catchUnhandledExceptions)
            {
                var currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);
            }
        }

        public abstract void Close();

        public abstract void Write(string eventDescription);

        public abstract void WriteExceptionInfo(Exception exception);

        protected abstract void UpdateEnabledState();

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    UpdateEnabledState();
                }
            }
        }

        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Write("Unhandled exception raised");
            WriteExceptionInfo(e);
            Close();
        }
    }

    public class FileLog : LogBase
    {
        private string _logFileName;
        private TextWriter _logFile;

        public FileLog(string logFileName, bool catchUnhandledExceptions)
            : base(catchUnhandledExceptions)
        {
            _logFileName = logFileName;
        }

        public override void Write(string eventDescription)
        {
            if (_logFile == null || !base.Enabled)
                return;

            lock (this)
            {
                _logFile.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} [{2}], {1}", DateTime.Now.ToString(CultureInfo.InvariantCulture), eventDescription, Thread.CurrentThread.ManagedThreadId));
                _logFile.Flush();
            }
        }

        public override void WriteExceptionInfo(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            if (_logFile == null || !base.Enabled)
                return;

            lock (this)
            {
                _logFile.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}, Exception occured. Message: {1}, stack trace: {2}", DateTime.Now.ToString(CultureInfo.InvariantCulture), exception.Message, exception.StackTrace));
                _logFile.Flush();
            }
        }

        public override void Close()
        {
            lock (this)
            {
                if (_logFile != null)
                {
                    _logFile.Close();
                    _logFile = null;
                }
            }
        }

        protected override void UpdateEnabledState()
        {
            if (_logFile == null && base.Enabled)
                CreateLogFile();
        }

        private void CreateLogFile()
        {
            if (_logFileName == null)
                return;

            try
            {
                if (File.Exists(_logFileName))
                    _logFile = File.AppendText(_logFileName);
                else
                {
                    if (!Directory.Exists(Path.GetDirectoryName(_logFileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(_logFileName));

                    _logFile = File.CreateText(_logFileName);
                }
            }
            catch (Exception)
            {
                MessageDialog.Show(string.Format("{0}{1}", StringResources.GetString("MessageUnableToCreateLog"), _logFileName));
            }
        }
    }

    public class WindowsAppLog : LogBase
    {
        private EventLog _eventLog;

        public WindowsAppLog(bool catchUnhandledExceptions)
            : base(catchUnhandledExceptions)
        {
            try
            {
                if (!EventLog.SourceExists(Config.EventLogSource))
                    EventLog.CreateEventSource(Config.EventLogSource, "Application");

                _eventLog = new EventLog("Application", ".", Config.EventLogSource);
            }
            catch (SecurityException)
            {
                _eventLog = null;
            }
        }

        public override void Write(string eventDescription)
        {
            if (this.Enabled)
            {
                lock (this)
                {
                    if (_eventLog == null || this.Enabled == false)
                        return;

                    _eventLog.WriteEntry(eventDescription);
                }
            }
        }

        public override void Close()
        {
            lock (this)
            {
                if (_eventLog == null || this.Enabled == false)
                    return;

                _eventLog.Close();
                _eventLog.Dispose();
                _eventLog = null;
            }
        }

        protected override void UpdateEnabledState()
        {
            // We have nothing to update...
        }

        public override void WriteExceptionInfo(Exception exception)
        {
            lock (this)
            {
                if (_eventLog == null || this.Enabled == false)
                    return;

                try
                {
                    _eventLog.WriteEntry(string.Format(CultureInfo.InvariantCulture,
                        "{0}, Exception occured. Message: {1}, stack trace: {2}",
                        DateTime.Now.ToString(CultureInfo.InvariantCulture),
                        exception.Message, exception.StackTrace), EventLogEntryType.Error);
                }
                catch (Win32Exception)
                {
                    // We should be silent if event log is already full.
                }
                catch (SecurityException)
                {
                    MessageDialog.Show(StringResources.GetString("MessageUnableToUseWinLog"));
                }
            }
        }
    }
}