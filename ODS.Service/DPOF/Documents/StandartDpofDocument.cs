// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;
using System.Linq;

namespace ODS.Service.DPOF
{
    public class StandartDpofDocument : DpofDocument
    {
        private readonly DpofAutoPrintFile _autoPrintFile;
        private DpofUnicodeTextDescriptionFile _unicodeTextDescriptionFile;

        protected override DpofAutoPrintFile AutoPrintFile
        {
            get { return _autoPrintFile; }
        }

        protected override DpofUnicodeTextDescriptionFile UnicodeTextDescriptionFile
        {
            get { return _unicodeTextDescriptionFile; }
        }

        public override DpofFormatType Type
        {
            get { return DpofFormatType.Standart; }
        }

        public StandartDpofDocument(string miscDirectoryPath, IEnumerable<PrintFileInfo> printFileInfos) : base(miscDirectoryPath)
        {
            var headerSection = new DpofHeaderSection();
            var jobSections = new List<DpofJobSection>();
            var counter = 1;
            foreach (var file in printFileInfos)
            {
                var job = new DpofJobSection(counter++, GetRelativeToWorkingDirectory(file.FilePath), file.CopyCount, GetImageFormat(file.FilePath));
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