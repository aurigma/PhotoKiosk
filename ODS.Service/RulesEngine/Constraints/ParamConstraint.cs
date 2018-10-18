// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using ODS.Core;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ODS.Service.RulesEngine.Constraints
{
    public class ParamConstraint : IConstraint
    {
        public string Key { get; private set; }
        public Regex Pattern { get; private set; }

        public ParamConstraint(string key, string pattern)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            if (string.IsNullOrEmpty(pattern)) throw new ArgumentNullException("pattern");

            Key = key;
            Pattern = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public bool IsMatch(IFile file)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (file.Params == null) return false;

            var isMatch = true;
            if (file.Params.Where(p => p.Key == Key).Any())
            {
                foreach (var pair in file.Params.Where(p => p.Key == Key))
                {
                    isMatch = isMatch && Pattern.IsMatch(pair.Value);
                }
            }
            else
            {
                return false;
            }

            return isMatch;
        }
    }
}