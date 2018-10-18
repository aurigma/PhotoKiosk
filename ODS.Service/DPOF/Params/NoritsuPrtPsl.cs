// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
namespace ODS.Service.DPOF
{
    public class NoritsuPrtPsl : PrtPsl
    {
        private readonly string _paperSize;

        public override string Prefix
        {
            get { return "PRT"; }
        }

        public override string Suffix
        {
            get { return "PSL"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Optional; }
        }

        public override string Value
        {
            get { return "NML"; }
        }

        public override string AuxiliaryValue1
        {
            get { return "PSIZE"; }
        }

        public override string AuxiliaryValue2
        {
            get { return string.Format("\"{0}\"", _paperSize); }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="paperSize">Looks like «6" Matt»</param>
        public NoritsuPrtPsl(string paperSize)
        {
            _paperSize = paperSize;
        }
    }
}