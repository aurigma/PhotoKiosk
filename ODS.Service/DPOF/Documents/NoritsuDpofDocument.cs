// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;
using System.Linq;

namespace ODS.Service.DPOF
{
    public class NoritsuDpofDocument : DpofDocument
    {
        private readonly DpofAutoPrintFile _autoPrintFile;
        private readonly DpofUnicodeTextDescriptionFile _unicodeTextDescriptionFile;

        protected override DpofAutoPrintFile AutoPrintFile
        {
            get { return _autoPrintFile; }
        }

        protected override DpofUnicodeTextDescriptionFile UnicodeTextDescriptionFile
        {
            get { return _unicodeTextDescriptionFile; }
        }

        public override DpofFormatType Type { get { return DpofFormatType.Noritsu; } }

        public NoritsuDpofDocument(string miscDirectoryPath, IEnumerable<PrintFileInfo> printFileInfos, string paperSize, int channelNumber) : base(miscDirectoryPath)
        {
            var headerSection = new DpofNoritsuHeaderSection(paperSize, channelNumber);

            var jobSections = new List<DpofNoritsuJobSection>();
            var counter = 1;
            foreach (var file in printFileInfos)
            {
                var job = new DpofNoritsuJobSection(counter++, GetRelativeToWorkingDirectory(file.FilePath), file.CopyCount, GetImageFormat(file.FilePath));
                jobSections.Add(job);
            }

            _autoPrintFile = new DpofAutoPrintFile(miscDirectoryPath, headerSection, jobSections);

            var unicodeTexts = headerSection.Parameters.Where(p => p is IUnicodeTextProvider).Cast<IUnicodeTextProvider>().ToList();
            foreach (var section in jobSections)
            {
                unicodeTexts.AddRange(section.Parameters.Where(p => p is IUnicodeTextProvider).Cast<IUnicodeTextProvider>().ToList());
            }
            _unicodeTextDescriptionFile = new DpofUnicodeTextDescriptionFile(miscDirectoryPath, new DpofStringSection(unicodeTexts));
        }
    }
}