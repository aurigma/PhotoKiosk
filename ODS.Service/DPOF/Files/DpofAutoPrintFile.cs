// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;
using System.Text;

namespace ODS.Service.DPOF
{
    public class DpofAutoPrintFile : DpofFile
    {
        private readonly DpofHeaderSection _headerSection;
        private readonly IEnumerable<DpofJobSection> _jobSections;

        private List<DpofSection> _sections;

        protected override IEnumerable<DpofSection> Sections
        {
            get
            {
                if (_sections == null)
                {
                    _sections = new List<DpofSection> { _headerSection };
                    _sections.AddRange(_jobSections);
                }
                return _sections;
            }
        }

        protected override string FileName { get { return "AUTPRINT.MRK"; } }
        protected override Encoding FileEncoding { get { return Encoding.ASCII; } }

        public DpofAutoPrintFile(string miscDirectoryPath, DpofHeaderSection headerSection, IEnumerable<DpofJobSection> jobSections) : base(miscDirectoryPath)
        {
            _headerSection = headerSection;
            _jobSections = jobSections;
        }
    }
}