// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;

namespace ODS.Service.DPOF
{
    public class DpofHeaderSection : DpofSection
    {
        private readonly IEnumerable<DpofParameter> _parameters;
        protected override string Name { get { return "HDR"; } }
        public override IEnumerable<DpofParameter> Parameters { get { return _parameters; } }

        public DpofHeaderSection()
        {
            var parameters = new List<DpofParameter>
                                 {
                                     new GenRev(),
                                     new GenDtm(DateTime.Now),
                                     new UsrUnm("", "", ""),
                                     new UsrTel("", "", "")
                                 };
            _parameters = parameters;
        }
    }
}