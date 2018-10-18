// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using ODS.Core;

namespace ODS.Service.RulesEngine.Constraints
{
    public interface IConstraint
    {
        bool IsMatch(IFile file);
    }
}