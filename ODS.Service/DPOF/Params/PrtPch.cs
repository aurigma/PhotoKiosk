// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;

namespace ODS.Service.DPOF
{
    /// <summary>
    /// Noritsu Channel settings
    /// </summary>
    public class PrtPch : DpofParameter
    {
        private readonly int _channel;

        public override string Prefix
        {
            get { return "PRT"; }
        }

        public override string Suffix
        {
            get { return "PCH"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Mandatory; }
        }

        public override string Value { get { return _channel.ToString("000"); } }

        public PrtPch(int channel)
        {
            if (channel < 1 || channel > 999) throw new ArgumentOutOfRangeException("channel", "channel should be between 1 and 999");

            _channel = channel;
        }
    }
}