// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;

namespace ODS.Service.DPOF
{
    /// <summary>
    /// Specifies the number of copies of the Same Print Group. A Job Section ([JOB]) may include a single PRT QTY parameter.
    /// </summary>
    public class PrtQty : DpofParameter
    {
        private readonly int _quantity;

        public override string Prefix
        {
            get { return "PRT"; }
        }

        public override string Suffix
        {
            get { return "QTY"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Optional; }
        }

        public override string Value
        {
            get { return _quantity.ToString("000"); }
        }

        public PrtQty(int quantity)
        {
            if (quantity <= 0 || quantity > 999)
                throw new ArgumentOutOfRangeException("quantity");
            _quantity = quantity;
        }
    }
}