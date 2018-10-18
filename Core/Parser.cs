// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Globalization;

namespace Aurigma.PhotoKiosk.Core
{
    public sealed class Parser
    {
        private const string FormatArgPrefix = "#";

        private Parser()
        {
        }

        public static string CreateFormatString(string format, string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            string formatString = format;

            for (int i = 0; i < args.Length; i++)
            {
                formatString = formatString.Replace(string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", Constants.SpecialKeyFramer, i.ToString(CultureInfo.InvariantCulture), Constants.SpecialKeyFramer), args[i]);
            }

            return formatString;
        }
    }
}