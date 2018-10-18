// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
namespace ODS.Service.DPOF
{
    public class CfgDsc : DpofParameter
    {
        public override string Prefix
        {
            get { return "CFG"; }
        }

        public override string Suffix
        {
            get { return "DSC"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Optional; }
        }

        public override string Value
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}