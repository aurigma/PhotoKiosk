// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;

namespace ODS.Service.DPOF
{
    /// <summary>
    /// Specifies the telephone number of the user who has made the Auto Print File (or DPOF Writer’s owner
    /// who has made the Auto Print File).
    /// </summary>
    public class UsrTel : DpofParameter
    {
        private readonly string _country;
        private readonly string _area;
        private readonly string _phone;

        public override string Prefix
        {
            get { return "USR"; }
        }

        public override string Suffix
        {
            get { return "TEL"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Mandatory; }
        }

        public override string Value
        {
            get { return string.Format("\"{0}\", \"{1}\", \"{2}\"", _country, _area, _phone); }
        }

        public UsrTel(string country, string area, string local)
        {
            if (!string.IsNullOrWhiteSpace(country) && country.Length > 15) throw new ArgumentOutOfRangeException("country");
            if (!string.IsNullOrWhiteSpace(area) && area.Length > 15) throw new ArgumentOutOfRangeException("area");
            if (!string.IsNullOrWhiteSpace(local) && local.Length > 15) throw new ArgumentOutOfRangeException("local");

            _country = country ?? string.Empty;
            _area = area ?? string.Empty;
            _phone = local ?? string.Empty;
        }
    }
}