// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace ODS.Core
{
    public interface ITransformationRule
    {
        string Title { get; }
        string OutputPath { get; }
        string RuleFilePath { get; }
        IDictionary<string, string> ReplacementValues { get; }
    }
}