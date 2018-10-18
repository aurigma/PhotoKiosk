// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
namespace ODS.Service.DPOF
{
    /// <summary>
    /// Specifies the version of the DPOF specification applied to the Auto Print File.
    /// </summary>
    public class GenRev : DpofParameter
    {
        public override string Prefix
        {
            get { return "GEN"; }
        }

        public override string Suffix
        {
            get { return "REV"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Mandatory; }
        }

        public override string Value
        {
            get { return "01.10"; }
        }
    }
}