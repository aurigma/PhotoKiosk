// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace ODS.Service.DPOF
{
    public class DpofNoritsuJobSection : DpofJobSection
    {
        private readonly IEnumerable<DpofParameter> _parameters;
        public override IEnumerable<DpofParameter> Parameters { get { return _parameters; } }

        public DpofNoritsuJobSection(int printId, string relativeFilePath, int count, DpofImageFormat format) : base(printId, relativeFilePath, count, format)
        {
            var parameters = new List<DpofParameter>
                                 {
                                     new PrtPid(printId),
                                     new PrtTypStd(),
                                     new PrtQty(count),
                                     new ImgFmt(format),
                                     new ImgSrc(relativeFilePath),
                                     new VuqRgn(VuqRgn.VuqRgnType.Begin),
                                     new VuqVnm("NORITSU KOKI", "QSSPrint"),
                                     new VuqVer(1, 0),
                                     new VuqRgn(VuqRgn.VuqRgnType.End)
                                 };

            _parameters = parameters;
        }
    }
}