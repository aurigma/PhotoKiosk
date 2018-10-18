// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.GraphicsMill.Codecs;
using System;
using System.Collections.Generic;

namespace ODS.Service.DPOF
{
    public enum DpofFormatType
    {
        Standart = 0,
        Noritsu
    }

    public abstract class DpofDocument
    {
        private readonly string _miscDirectoryPath;
        public abstract DpofFormatType Type { get; }

        protected abstract DpofAutoPrintFile AutoPrintFile { get; }
        protected abstract DpofUnicodeTextDescriptionFile UnicodeTextDescriptionFile { get; }

        protected DpofDocument(string miscDirectoryPath)
        {
            _miscDirectoryPath = miscDirectoryPath;
        }

        public IEnumerable<string> GetRenderedFilesPath()
        {
            var paths = new List<string>();
            if (AutoPrintFile != null)
                paths.Add(AutoPrintFile.RenderFileAndGetPath());

            if (UnicodeTextDescriptionFile != null)
                paths.Add(UnicodeTextDescriptionFile.RenderFileAndGetPath());

            return paths;
        }

        protected string GetRelativeToWorkingDirectory(string filePath)
        {
            var fromUri = new Uri(_miscDirectoryPath);
            var toUri = new Uri(filePath);
            var relativeUri = fromUri.MakeRelativeUri(toUri);
            return relativeUri.ToString();
        }

        protected DpofImageFormat GetImageFormat(string filePath)
        {
            ExifDictionary exif = null;
            if (filePath.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase) || filePath.EndsWith("jpeg", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    using (var jpegReader = new JpegReader(filePath))
                    {
                        exif = jpegReader.Exif;
                        if (exif != null)
                        {
                            try
                            {
                                var version = exif.GetItemString(ExifDictionary.ExifVersion);
                                if (version == "0220") return DpofImageFormat.EXIF2J;
                                if (version == "0210") return DpofImageFormat.EXIF1J;
                            }
                            catch (Exception) { }
                        }
                        return DpofImageFormat.JFIF;
                    }
                }
                catch (Exception) { exif = null; }
            }
            if (filePath.EndsWith("tiff", StringComparison.InvariantCultureIgnoreCase) || filePath.EndsWith("tif", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    using (var tiffReader = new TiffReader(filePath))
                    {
                        exif = tiffReader.Exif;
                        if (exif != null)
                        {
                            try
                            {
                                var version = exif.GetItemString(ExifDictionary.ExifVersion);
                                if (version == "0220") return DpofImageFormat.EXIF2T;
                                if (version == "0210") return DpofImageFormat.EXIF1T;
                            }
                            catch (Exception) { }
                        }
                        return DpofImageFormat.UNDEF;
                    }
                }
                catch (Exception) { exif = null; }
            }

            if (exif != null)
            {
            }
            return DpofImageFormat.UNDEF;
        }
    }
}