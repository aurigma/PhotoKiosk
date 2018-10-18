// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;

namespace ODS.Service.DPOF
{
    /// <summary>
    /// The device or the software that last saved a new or overwritten Auto Print File. When the content of
    /// the Auto Print File are edited and saved, the DPOF Writer shall rewrite this parameter. The device
    /// and the software name shall be attached with vender names etc. to avoid mixing up with others with
    /// the same creator name.
    /// </summary>
    public class GenCrt : DpofParameter
    {
        private readonly string _creator;
        private readonly int _majorVersion = -1;
        private readonly int _minorVersion = -1;

        public override string Prefix
        {
            get { return "GEN"; }
        }

        public override string Suffix
        {
            get { return "CRT"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Mandatory; }
        }

        public override string Value { get { return string.Format("\"{0}\"", _creator); } }

        public override string AuxiliaryValue1
        {
            get
            {
                if (_minorVersion >= 0 && _majorVersion >= 0)
                {
                    return string.Format("{0:00}.{1:00}", _majorVersion, _minorVersion);
                }
                return string.Empty;
            }
        }

        public GenCrt(string creator)
        {
            if (creator == null) throw new ArgumentNullException("creator");
            if (creator.Length > 127) throw new ArgumentOutOfRangeException("creator", "length should be less or equal to 127 characters");

            _creator = creator;
        }

        public GenCrt(string creator, int majorVersion, int minorVersion)
        {
            if (creator == null) throw new ArgumentNullException("creator");
            if (creator.Length > 127) throw new ArgumentOutOfRangeException("creator", "length should be less or equal to 127 characters");

            if (majorVersion < 0 || majorVersion > 99) throw new ArgumentOutOfRangeException("majorVersion");
            if (minorVersion < 0 || minorVersion > 99) throw new ArgumentOutOfRangeException("minorVersion");

            _creator = creator;
            _majorVersion = majorVersion;
            _minorVersion = minorVersion;
        }
    }
}