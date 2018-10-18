// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace ODS.Service.RulesEngine.Tasks
{
    internal class TaskContext : ITaskContext
    {
        public string WorkingDirectory { get; private set; }

        public Dictionary<string, string> CommonParams { get; private set; }

        public TaskContext(string workingDirectory)
        {
            WorkingDirectory = workingDirectory;
            CommonParams = new Dictionary<string, string>();
        }
    }
}