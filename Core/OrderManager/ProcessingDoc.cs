// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using ODS.Core;
using System.Collections.Generic;

namespace Aurigma.PhotoKiosk.Core.OrderManager
{
    internal class ProcessingDoc : IFile
    {
        public string Path { get; private set; }

        private readonly IDictionary<string, string> _params;

        public IDictionary<string, string> Params
        {
            get { return _params; }
        }

        public ProcessingDoc(string path, params KeyValuePair<string, string>[] @params)
        {
            Path = path;
            _params = new Dictionary<string, string>();
            if (@params != null)
            {
                foreach (var pair in @params)
                    _params.Add(pair);
            }
        }
    }
}