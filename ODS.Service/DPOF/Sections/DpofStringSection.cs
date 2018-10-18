// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace ODS.Service.DPOF
{
    public class DpofStringSection : DpofSection
    {
        private readonly IEnumerable<IUnicodeTextProvider> _unicodeTextProviders;
        protected override string Name { get { return "STRING"; } }

        public override IEnumerable<DpofParameter> Parameters
        {
            get { throw new System.NotImplementedException(); }
        }

        public DpofStringSection(IEnumerable<IUnicodeTextProvider> unicodeTextProviders)
        {
            _unicodeTextProviders = unicodeTextProviders;
        }

        public override string Render()
        {
            var result = string.Format("[{0}]\r\n", Name);
            foreach (var param in _unicodeTextProviders)
            {
                var strings = param.GetUnicodeStrings();
                if (strings != null)
                {
                    foreach (var s in strings)
                    {
                        result += s + "\r\n";
                    }
                }
            }
            return result + "\r\n";
        }
    }
}