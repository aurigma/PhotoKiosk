// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ODS.Service.DPOF
{
    public abstract class DpofFile
    {
        private readonly string _miscDirectoryPath;
        protected abstract IEnumerable<DpofSection> Sections { get; }
        protected abstract string FileName { get; }

        protected abstract Encoding FileEncoding { get; }

        protected DpofFile(string miscDirectoryPath)
        {
            _miscDirectoryPath = miscDirectoryPath;
        }

        public virtual string RenderFileAndGetPath()
        {
            var result = string.Empty;
            foreach (var section in Sections)
            {
                result += section.Render() + "\r\n";
            }

            var filePath = Path.Combine(_miscDirectoryPath, FileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
            File.WriteAllText(filePath, result, FileEncoding);

            return filePath;
        }
    }
}