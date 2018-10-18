// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using ODS.Core;
using ODS.Service.RulesEngine.Constraints;
using ODS.Service.RulesEngine.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ODS.Service.RulesEngine
{
    public class FileRule
    {
        private NLog.Logger _logger;

        protected NLog.Logger Logger
        {
            get { return _logger ?? (_logger = NLog.LogManager.GetLogger("ODS")); }
        }

        private readonly IEnumerable<ITask> _tasks;
        private readonly IEnumerable<IConstraint> _constraints;
        public string Title { get; private set; }

        public FileRule(string title, IEnumerable<ITask> tasks, IEnumerable<IConstraint> constraints)
        {
            if (tasks == null) throw new ArgumentNullException("tasks");

            Title = title;
            _tasks = tasks;
            _constraints = constraints;
        }

        private bool IsMatch(IFile file)
        {
            if (file == null) return false;

            if (_constraints == null) return true;

            var isMatch = true;
            foreach (var constraint in _constraints)
            {
                isMatch = isMatch && constraint.IsMatch(file);
            }
            return isMatch;
        }

        public void Apply(IEnumerable<IFile> files, ITaskContext context)
        {
            if (files == null) throw new ArgumentNullException("files");
            if (_tasks == null) return;

            var filteredFiles = (from file in files where IsMatch(file) && File.Exists(file.Path) select new TaskFile(file));

            Logger.Info("RULE {0}: applicable files count {1} from {2} total", Title, filteredFiles.Count(), files.Count());

            var @params = GetEqualParamsFromFiles(filteredFiles);
            foreach (var p in @params)
            {
                if (!context.CommonParams.ContainsKey(p.Key))
                    context.CommonParams.Add(p.Key, p.Value);
                else
                {
                    context.CommonParams[p.Key] = p.Value;
                }
            }

            var taskSource = new TaskInput(filteredFiles, context.CommonParams);
            var orderedTasks = _tasks.OrderBy(t => t.ParentTask == null);
            foreach (var task in orderedTasks)
            {
                task.ExecuteTaskTree(taskSource, context);
            }
        }

        private IEnumerable<KeyValuePair<string, string>> GetEqualParamsFromFiles(IEnumerable<TaskFile> filteredFiles)
        {
            var allParams = new Dictionary<string, string>();
            var notUniqueKeys = new List<string>();
            foreach (var file in filteredFiles)
            {
                foreach (var p in file.Params)
                {
                    if (!allParams.ContainsKey(p.Key))
                        allParams.Add(p.Key, p.Value);
                    else
                    {
                        if (allParams[p.Key] != p.Value)
                            notUniqueKeys.Add(p.Key);
                    }
                }
            }
            foreach (var key in notUniqueKeys)
            {
                allParams.Remove(key);
            }
            return allParams;
        }
    }
}