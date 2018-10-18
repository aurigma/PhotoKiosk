// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace ODS.Service.DPOF
{
    public abstract class DpofSection
    {
        protected abstract string Name { get; }
        public abstract IEnumerable<DpofParameter> Parameters { get; }

        public virtual string Render()
        {
            var result = string.Format("[{0}]\r\n", Name);
            foreach (var param in Parameters)
            {
                result += param.Render() + "\r\n";
            }
            return result + "\r\n";
        }
    }
}