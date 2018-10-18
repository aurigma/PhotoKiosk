// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ODS.Service.RulesEngine.Tasks
{
    public class DeleteTask : TaskBase
    {
        private readonly bool _deleteEmptyFolders;

        public DeleteTask(ITask parent, IEnumerable<ITask> children, bool deleteEmptyFolders)
            : base(parent, children)
        {
            _deleteEmptyFolders = deleteEmptyFolders;
        }

        protected override string TaskName
        {
            get { return "DELETE"; }
        }

        public override int Priority
        {
            get { return 9; }
        }

        protected override void ChildrenExecuted(TaskInput childrenInput, IEnumerable<TaskOutput> childrenOutput)
        {
        }

        protected override TaskOutput Execute(TaskInput input, ITaskContext context)
        {
            if (input == null) throw new ArgumentNullException("input");

            foreach (var taskFile in input.Files.Where(taskFile => File.Exists(taskFile.Path)))
            {
                if (!Path.GetFullPath(taskFile.Path).StartsWith(Path.GetFullPath(context.WorkingDirectory), StringComparison.InvariantCultureIgnoreCase))
                {
                    Logger.Error("target file path is outside working directory:", Path.GetFullPath(taskFile.Path));
                    continue;
                }

                File.Delete(taskFile.Path);
                if (!_deleteEmptyFolders) continue;

                var folderPath = new FileInfo(taskFile.Path).DirectoryName;
                if (Directory.GetFiles(folderPath).Length == 0)
                {
                    Directory.Delete(folderPath);
                }
            }
            return new TaskOutput(new List<TaskFile>(), null);
        }
    }
}