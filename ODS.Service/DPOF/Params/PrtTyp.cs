// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
namespace ODS.Service.DPOF
{
    /// <summary>
    /// Specifies the Print type in the Job Section ([JOB]).
    /// </summary>
    public abstract class PrtTyp : DpofParameter
    {
        public override string Prefix
        {
            get { return "PRT"; }
        }

        public override string Suffix
        {
            get { return "TYP"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Mandatory; }
        }
    }
}