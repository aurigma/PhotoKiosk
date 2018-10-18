// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ODS.Service.DPOF
{
    /// <summary>
    /// Specifies the Unicode string to identify the user who has made the Auto Print File (or DPOF Writer’s
    /// owner who has made the Auto Print File). Text is recorded in a Unicode Text Description File.
    /// </summary>
    public class UsrUad : DpofParameter, IUnicodeTextProvider
    {
        private readonly string _postalCode;
        private readonly string _region;
        private readonly string _city;
        private readonly string _address;
        private readonly string _country;

        public override string Prefix
        {
            get { return "USR"; }
        }

        public override string Suffix
        {
            get { return "UAD"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Optional; }
        }

        public override string Value
        {
            get
            {
                return string.Format("\"{0}\", \"{1}\", AD1, AD2, AD3", _country.ToUpper(), _postalCode);
            }
        }

        public UsrUad(RegionInfo country, string postalCode, string region, string city, string address)
        {
            if (country == null) throw new ArgumentNullException("country");
            _country = country.ThreeLetterISORegionName;
            if (string.IsNullOrWhiteSpace(_country))
            {
                throw new ArgumentOutOfRangeException("country", "unknown country specified");
            }

            if (postalCode.Length > 15) throw new ArgumentOutOfRangeException("postalCode", "length should be less or equal to 15 characters");
            if (region.Length > 31) throw new ArgumentOutOfRangeException("region", "length should be less or equal to 31 characters");
            if (city.Length > 31) throw new ArgumentOutOfRangeException("city", "length should be less or equal to 31 characters");
            if (address.Length > 127) throw new ArgumentOutOfRangeException("address", "length should be less or equal to 127 characters");

            _postalCode = postalCode;
            _region = region;
            _city = city;
            _address = address;
        }

        public string[] GetUnicodeStrings()
        {
            var strings = new List<string> { string.Format("AD1 = \"{0}\"", _region) };
            if (_city != null)
            {
                strings.Add(string.Format("AD2 = \"{0}\"", _city));
                if (_address != null)
                {
                    strings.Add(string.Format("AD3 = \"{0}\"", _address));
                }
            }
            return strings.ToArray();
        }
    }
}