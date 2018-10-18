// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using ODS.Core;
using System;
using System.Text.RegularExpressions;

namespace ODS.Service.RulesEngine.Constraints
{
    public class PathConstraint : IConstraint
    {
        public Regex Pattern { get; private set; }

        public PathConstraint(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) throw new ArgumentNullException("pattern");
            Pattern = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public bool IsMatch(IFile file)
        {
            if (file == null) throw new ArgumentNullException("file");
            return Pattern.IsMatch(file.Path);
        }
    }
}