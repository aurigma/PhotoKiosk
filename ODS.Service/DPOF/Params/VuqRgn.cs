// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
namespace ODS.Service.DPOF
{
    /// <summary>
    /// Indicates the beginning and the end of the indicated Vender Unique region. The region between the
    /// beginning and the end is made the Vender Unique region. At first (immediately after VUQRGN =
    /// BGN) the Vender name that the Vender Unique region belongs to shall be indicated based on the
    /// VUQVNM parameter (see 6-4-4-2.) The beginning and the end value shall be used in a pair. Any
    /// Vender Unique region shall NOT include other Vender Unique regions (If a Vender Unique region is not
    /// ended, the next Vender Unique region shall NOT start). The Vender Unique region shall be so written
    /// as there will be no syntax errors for the Auto Print File if the region reading is skipped. The
    /// IMGSRC Condition Expression shall NOT be enclosed with &lt; and &gt; in the Vender Unique region.
    /// </summary>
    public class VuqRgn : DpofParameter
    {
        private readonly VuqRgnType _type;

        public enum VuqRgnType
        {
            Begin,
            End
        }

        public override string Prefix
        {
            get { return "VUQ"; }
        }

        public override string Suffix
        {
            get { return "RGN"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Optional; }
        }

        public override string Value
        {
            get
            {
                return _type == VuqRgnType.Begin ? "BGN" : "END";
            }
        }

        public VuqRgn(VuqRgnType type)
        {
            _type = type;
        }
    }
}