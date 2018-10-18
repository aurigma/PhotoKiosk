// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace Aurigma.PhotoKiosk.Core
{
    public class AddonManager
    {
        private Process _process;
        private string _fileName;
        private string _arguments;
        private int _errorCode;
        private bool _completed;
        private string _output = "";

        public AddonManager(string fileName, string arguments)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("filename");

            if (string.IsNullOrEmpty(arguments))
                throw new ArgumentNullException("arguments");

            if (!File.Exists(fileName))
                throw new FileNotFoundException("Can't find an addon file[" + fileName + "] with arguments[" + arguments + "].", fileName);

            _process = new Process();
            _fileName = fileName;
            _arguments = arguments;
            _errorCode = 0;
            _completed = false;
        }

        public void Start()
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = _fileName;
            startInfo.Arguments = _arguments;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;

            _process.StartInfo = startInfo;
            _process.EnableRaisingEvents = true;
            _process.Exited += ProcessExitedHandler;
            _process.OutputDataReceived += DataReceivedHandler;

            _process.Start();
            _process.BeginOutputReadLine();
        }

        private void ProcessExitedHandler(object sender, EventArgs e)
        {
            _completed = true;
            _errorCode = _process.ExitCode;
        }

        private void DataReceivedHandler(object sender,
            DataReceivedEventArgs outLine)
        {
            _output += outLine.Data + "\n";
        }

        public int ErrorCode
        {
            get { return _errorCode; }
        }

        public bool Completed
        {
            get { return _completed; }
        }

        public string Output
        {
            get { return _output; }
        }
    }

    public class OrderManagerAddon
    {
        private AddonManager _manager = null;
        private List<string> _ordersToProcess = new List<string>();
        private Timer _timer = new Timer(10000);

        public OrderManagerAddon()
        {
            _timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
            _timer.Enabled = false;
        }

        public void AddOrder(string folderName)
        {
            folderName = string.Format(@"""{0}""", folderName);
            if (_manager == null || _manager.Completed)
            {
                _manager = new AddonManager(Config.OrderManagerFile, folderName);
                _manager.Start();
            }
            else
            {
                if (!_ordersToProcess.Contains(folderName))
                    _ordersToProcess.Add(folderName);
            }

            _timer.Start();
        }

        private void OnTimerElapsed(object source, ElapsedEventArgs e)
        {
            if (_manager != null && _manager.Completed)
            {
                if (_manager.ErrorCode != 0)
                    ExecutionEngine.ErrorLogger.Write(_manager.Output);
                else
                    ExecutionEngine.EventLogger.Write(_manager.Output);

                ExecutionEngine.RunCompleteOrderProcess();

                if (_ordersToProcess.Count > 0)
                {
                    _manager = new AddonManager(Config.OrderManagerFile, _ordersToProcess[0]);
                    _ordersToProcess.Remove(_ordersToProcess[0]);
                    _manager.Start();
                }
                else
                    _timer.Stop();
            }
        }
    }
}