// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;
using System.Text;

namespace ODS.Service.DPOF
{
    public class DpofUnicodeTextDescriptionFile : DpofFile
    {
        private readonly IEnumerable<DpofSection> _sections;

        protected override IEnumerable<DpofSection> Sections
        {
            get { return _sections; }
        }

        protected override string FileName { get { return "UNICODE.MRK"; } }
        protected override Encoding FileEncoding { get { return Encoding.Unicode; } }

        public DpofUnicodeTextDescriptionFile(string miscDirectoryPath, DpofStringSection section) : base(miscDirectoryPath)
        {
            _sections = new List<DpofSection> { section };
        }
    }
}