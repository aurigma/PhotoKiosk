// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace ODS.Service.DPOF
{
    public class DpofJobSection : DpofSection
    {
        protected override string Name { get { return "JOB"; } }
        private readonly IEnumerable<DpofParameter> _parameters;
        public override IEnumerable<DpofParameter> Parameters { get { return _parameters; } }

        public DpofJobSection(int printId, string relativeFilePath, int count, DpofImageFormat format)
        {
            var parameters = new List<DpofParameter>
                                 {
                                     new PrtPid(printId),
                                     new PrtTypStd(),
                                     new PrtQty(count),
                                     new ImgFmt(format),
                                     new ImgSrc(relativeFilePath)
                                 };

            _parameters = parameters;
        }
    }
}