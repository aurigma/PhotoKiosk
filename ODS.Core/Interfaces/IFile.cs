// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace ODS.Core
{
    public interface IFile
    {
        string Path { get; }
        IDictionary<string, string> Params { get; }
    }
}