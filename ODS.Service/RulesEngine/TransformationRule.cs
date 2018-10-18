// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;
using System.Linq;

namespace ODS.Service.RulesEngine
{
    public class TransformationRule
    {
        public string Title { get; set; }

        public IEnumerable<FileRule> OrderFileRules { get; set; }
        public IEnumerable<FileRule> DocumentFileRules { get; set; }

        public bool ZipResult { get; set; }
        public bool CreateRandomSubfolderInOutput { get; set; }

        public override string ToString()
        {
            return string.Format("{0} (file rules: {1}, docs rules: {2})", Title, OrderFileRules.Count(), DocumentFileRules.Count());
        }
    }
}