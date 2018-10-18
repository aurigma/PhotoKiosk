// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace ODS.Service.RulesEngine.Tasks
{
    public class TaskOutput
    {
        private readonly Dictionary<string, string> _params;
        public IEnumerable<TaskFile> Files { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> Params { get { return _params; } }

        public TaskOutput(IEnumerable<TaskFile> files, IEnumerable<KeyValuePair<string, string>> resultParams = null)
        {
            Files = files;

            _params = new Dictionary<string, string>();
            if (resultParams != null)
            {
                foreach (var pair in resultParams)
                {
                    _params.Add(pair.Key, pair.Value);
                }
            }
        }
    }
}