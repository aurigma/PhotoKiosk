// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;

namespace ODS.Service.DPOF
{
    public class VuqVnm : DpofParameter
    {
        private readonly string _value;
        private readonly string _atribute;

        public override string Prefix
        {
            get { return "VUQ"; }
        }

        public override string Suffix
        {
            get { return "VNM"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Optional; }
        }

        public override string AuxiliaryValue1
        {
            get { return string.IsNullOrEmpty(_atribute) ? string.Empty : "ATR"; }
        }

        public override string AuxiliaryValue2
        {
            get { return string.IsNullOrEmpty(_atribute) ? string.Empty : string.Format("\"{0}\"", _atribute); }
        }

        public override string Value
        {
            get { return string.Format("\"{0}\"", _value); }
        }

        public VuqVnm(string value, string atribute)
        {
            if (!string.IsNullOrWhiteSpace(value) && value.Length > 127) throw new ArgumentOutOfRangeException("value");
            if (!string.IsNullOrWhiteSpace(atribute) && atribute.Length > 127) throw new ArgumentOutOfRangeException("atribute");

            _value = value;
            _atribute = atribute;
        }
    }
}