// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
namespace ODS.Service.DPOF
{
    public enum DpofImageFormat
    {
        CIFF1,
        EXIF1J,
        EXIF1T,
        EXIF2J,
        EXIF2T,
        JFIF,
        FPX1,
        UNDEF
    }

    /// <summary>
    /// Specifies the image file format of the Standard Print, Multiple Image Print and the Specific Size Print
    /// in the Auto Print File. Each Job Section ([JOB]) that specifies the Standard Print and the Specific Size
    /// Print includes one parameter. A Job Section that specifies the Index Print and the Multiple Image
    /// Print may include multiple IMGFMT parameters. The format specified by the IMGFMT is effective
    /// until the next IMGFMT specification. The object image files are as follows in Version 1.10. There is
    /// a possibility of additions due to technology trends.
    /// </summary>
    public class ImgFmt : DpofParameter
    {
        private readonly DpofImageFormat _format;

        public override string Prefix
        {
            get { return "IMG"; }
        }

        public override string Suffix
        {
            get { return "FMT"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Mandatory; }
        }

        public override string Value
        {
            get { return _format.ToString(); }
        }

        public override string AuxiliaryValue1
        {
            get
            {
                switch (_format)
                {
                    case DpofImageFormat.EXIF1J:
                    case DpofImageFormat.EXIF2J:
                        return "J";

                    case DpofImageFormat.EXIF1T:
                    case DpofImageFormat.EXIF2T:
                        return "T";
                }
                return string.Empty;
            }
        }

        public ImgFmt(DpofImageFormat format)
        {
            _format = format;
        }
    }
}