// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;

namespace ODS.Service.DPOF
{
    /// <summary>
    /// Specifies the Unicode string to identify the user who has made the Auto Print File (or DPOF Writer’s
    /// owner who has made the Auto Print File). Text is recorded in a Unicode Text Description File.
    /// </summary>
    public class UsrUnm : DpofParameter, IUnicodeTextProvider
    {
        private readonly string _value;
        private readonly string _givenName;
        private readonly string _middleName;

        public override string Prefix
        {
            get { return "USR"; }
        }

        public override string Suffix
        {
            get { return "UNM"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Optional; }
        }

        public override string Value
        {
            get
            {
                var result = "NM1";
                if (_givenName != null)
                {
                    result += string.Format(", {0}", "NM2");
                    if (_middleName != null)
                    {
                        result += string.Format(", {0}", "NM3");
                    }
                }
                return result;
            }
        }

        public UsrUnm(string familyName, string givenName = null, string middleName = null)
        {
            if (familyName == null) throw new ArgumentNullException("familyName");
            if (familyName.Length > 127) throw new ArgumentOutOfRangeException("familyName", "length should be less or equal to 127 characters");
            if (givenName != null && givenName.Length > 127) throw new ArgumentOutOfRangeException("givenName", "length should be less or equal to 127 characters");
            if (middleName != null && middleName.Length > 127) throw new ArgumentOutOfRangeException("middleName", "length should be less or equal to 127 characters");

            _value = familyName;
            _givenName = givenName;
            _middleName = middleName;
        }

        public string[] GetUnicodeStrings()
        {
            var strings = new List<string> { string.Format("NM1 = \"{0}\"", _value) };
            if (_givenName != null)
            {
                strings.Add(string.Format("NM2 = \"{0}\"", _givenName));
                if (_middleName != null)
                {
                    strings.Add(string.Format("NM3 = \"{0}\"", _middleName));
                }
            }
            return strings.ToArray();
        }
    }
}