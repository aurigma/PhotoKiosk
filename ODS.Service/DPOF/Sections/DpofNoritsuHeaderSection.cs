// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;

namespace ODS.Service.DPOF
{
    public class DpofNoritsuHeaderSection : DpofHeaderSection
    {
        private readonly IEnumerable<DpofParameter> _parameters;
        public override IEnumerable<DpofParameter> Parameters { get { return _parameters; } }

        public DpofNoritsuHeaderSection(string paperSize, int printChannel)
        {
            var parameters = new List<DpofParameter>
                                 {
                                     new GenRev(),
                                     new GenCrt("NORITSU KOKI", 1, 0),
                                     new GenDtm(DateTime.Now),
                                     new UsrUnm("", "", ""),
                                     new UsrTel("", "", ""),
                                     new VuqRgn(VuqRgn.VuqRgnType.Begin),
                                     new VuqVnm("NORITSU KOKI", "QSSPrint"),
                                     new VuqVer(1, 0),
                                     new NoritsuPrtPsl(paperSize),
                                     new PrtPch(printChannel),
                                     new VuqRgn(VuqRgn.VuqRgnType.End)
                                 };
            _parameters = parameters;
        }
    }
}