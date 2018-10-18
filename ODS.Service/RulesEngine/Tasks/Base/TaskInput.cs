// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace ODS.Service.RulesEngine.Tasks
{
    public class TaskInput
    {
        public IEnumerable<TaskFile> Files { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> Params { get; private set; }

        public TaskInput(IEnumerable<TaskFile> files, IEnumerable<KeyValuePair<string, string>> sourceParams)
        {
            Files = files;
            Params = sourceParams;
        }

        public TaskInput(TaskOutput output)
        {
            Files = output.Files;
            var list = new List<KeyValuePair<string, string>>();
            list.AddRange(output.Params);
            Params = list;
        }
    }
}