// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;

namespace ODS.Service.DPOF
{
    /// <summary>
    /// Date and time when Auto Print File was last saved a new or overwritten. When the content of the
    /// Auto Print File are edited and saved, this parameter shall be rewritten. A DPOF Writer without a
    /// (regularly operating) timer function must delete the Condition Expression.
    /// </summary>
    public class GenDtm : DpofParameter
    {
        private readonly DateTime _createdDate;

        public override string Prefix
        {
            get { return "GEN"; }
        }

        public override string Suffix
        {
            get { return "DTM"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Optional; }
        }

        public override string Value
        {
            get
            {
                return _createdDate.ToString("yyyy:MM:dd:HH:mm:ss");
            }
        }

        public GenDtm(DateTime createdDate)
        {
            _createdDate = createdDate;
        }
    }
}