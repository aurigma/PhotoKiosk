// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;

namespace ODS.Service.DPOF
{
    /// <summary>
    /// Specifies an ID to specify the same Print Group. One parameter description is required for each Job
    /// Section. It is desirable to provide ID numbers to the Auto Print File Job Sections ([JOB]) in order
    /// starting from 001. However, the lack or inconsecutiveness of ID number resulting from editing etc. is
    /// permitted. Multiple Job Sections that shares the same PRTPID value shall not exist in the same
    /// Auto Print File.
    /// </summary>
    public class PrtPid : DpofParameter
    {
        private readonly int _pid;

        public override string Prefix
        {
            get { return "PRT"; }
        }

        public override string Suffix
        {
            get { return "PID"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Mandatory; }
        }

        public override string Value { get { return _pid.ToString("000"); } }

        public PrtPid(int pid)
        {
            if (pid < 1 || pid > 999) throw new ArgumentOutOfRangeException("pid", "pid should be between 1 and 999");

            _pid = pid;
        }
    }
}