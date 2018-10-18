// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.IO;

namespace ODS.Service.RulesEngine.Tasks
{
    public class TempTask : TaskBase
    {
        private List<TaskFile> _createdTempFiles;

        public TempTask(ITask parent, IEnumerable<ITask> children) : base(parent, children)
        {
        }

        protected override string TaskName
        {
            get { return "TEMP"; }
        }

        public override int Priority
        {
            get { return 1; }
        }

        protected override void ChildrenExecuted(TaskInput childrenInput, IEnumerable<TaskOutput> childrenOutput)
        {
            if (_createdTempFiles == null) return;
            foreach (var file in _createdTempFiles)
            {
                var fileInfo = new FileInfo(file.Path);
                if (File.Exists(fileInfo.FullName))
                    File.Delete(fileInfo.FullName);
                if (Directory.GetFiles(fileInfo.DirectoryName).Length == 0)
                    Directory.Delete(fileInfo.DirectoryName);
            }
        }

        protected override TaskOutput Execute(TaskInput input, ITaskContext context)
        {
            if (input == null) throw new ArgumentNullException("input");

            _createdTempFiles = new List<TaskFile>();
            foreach (var taskFile in input.Files)
            {
                if (!File.Exists(taskFile.Path)) throw new ArgumentException(string.Format("File not found: {0}", taskFile.Path));

                var newPath = Path.Combine(context.WorkingDirectory, "_tmp", Path.GetRandomFileName());
                Directory.CreateDirectory(new FileInfo(newPath).DirectoryName);
                File.Copy(taskFile.Path, newPath);

                _createdTempFiles.Add(new TaskFile(newPath, input.Params));
            }
            return new TaskOutput(_createdTempFiles);
        }
    }
}