// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace ODS.Service.RulesEngine.Tasks
{
    public interface ITask
    {
        int Priority { get; }
        ITask ParentTask { get; set; }
        IEnumerable<ITask> Children { get; }

        TaskOutput ExecuteTaskTree(TaskInput input, ITaskContext context);
    }
}