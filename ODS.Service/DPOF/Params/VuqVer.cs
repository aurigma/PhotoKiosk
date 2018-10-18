// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;

namespace ODS.Service.DPOF
{
    /// <summary>
    ///
    /// </summary>
    public class VuqVer : DpofParameter
    {
        private readonly int _majorVersion = -1;
        private readonly int _minorVersion = -1;

        public override string Prefix
        {
            get { return "VUQ"; }
        }

        public override string Suffix
        {
            get { return "VER"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Mandatory; }
        }

        public override string Value
        {
            get { return string.Format("{0:00}.{1:00}", _majorVersion, _minorVersion); }
        }

        public VuqVer(int majorVersion, int minorVersion)
        {
            if (majorVersion < 0 || majorVersion > 99) throw new ArgumentOutOfRangeException("majorVersion");
            if (minorVersion < 0 || minorVersion > 99) throw new ArgumentOutOfRangeException("minorVersion");

            _majorVersion = majorVersion;
            _minorVersion = minorVersion;
        }
    }
}