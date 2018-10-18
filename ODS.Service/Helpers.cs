// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ODS.Service
{
    internal static class Helpers
    {
        public static string ReplaceWithValues(this string source, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            if (pairs == null) return source;
            return pairs.Aggregate(source, (current, pair) => current.Replace(pair.Key, pair.Value));
        }

        public static bool IsChildOf(this string path, string parentPath)
        {
            return Path.GetFullPath(path).StartsWith(Path.GetFullPath(parentPath), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}