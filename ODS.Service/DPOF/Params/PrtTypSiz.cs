// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;

namespace ODS.Service.DPOF
{
    public class PrtTypSiz : PrtTyp
    {
        private readonly string _nn = "00";
        private readonly int _shorterSideLengthMm;

        public override string Value
        {
            get { return "SIZ"; }
        }

        public override string AuxiliaryValue1
        {
            get { return _nn; }
        }

        public override string AuxiliaryValue2
        {
            get
            {
                return _shorterSideLengthMm == 0 ? base.AuxiliaryValue2 : _shorterSideLengthMm.ToString("0000");
            }
        }

        public PrtTypSiz(int shorterSideLengthMm)
        {
            if (shorterSideLengthMm <= 0 || shorterSideLengthMm > 9999)
                throw new ArgumentOutOfRangeException("shorterSideLengthMm");

            _shorterSideLengthMm = shorterSideLengthMm;
            _nn = "99";
        }

        public PrtTypSiz(CommonPrintTypeSize commonPrintTypeSize)
        {
            _nn = ((int)commonPrintTypeSize).ToString("00");
        }
    }
}