// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
namespace ODS.Service.DPOF
{
    /// <summary>
    /// Specifies the image file for the Standard Print, the Multiple Image Print and the Specific Size Print in
    /// the Auto Print File with a relative path starting from the ‘MISC’ directory where the Auto Print File is
    /// stored.
    /// </summary>
    public class ImgSrc : DpofConditionalParameter
    {
        private readonly string _src;

        public override string Prefix
        {
            get { return "IMG"; }
        }

        public override string Suffix
        {
            get { return "SRC"; }
        }

        public override IndicationLevel IndicationLevel
        {
            get { return IndicationLevel.Mandatory; }
        }

        public override string Value
        {
            get { return string.Format("\"{0}\"", _src); }
        }

        public ImgSrc(string imagePath)
        {
            _src = imagePath;
        }
    }
}