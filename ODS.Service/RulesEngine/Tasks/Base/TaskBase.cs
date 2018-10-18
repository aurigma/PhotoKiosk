// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;
using System.Diagnostics;

namespace ODS.Service.RulesEngine.Tasks
{
    public abstract class TaskBase : ITask
    {
        protected abstract string TaskName { get; }

        private NLog.Logger _logger;

        protected NLog.Logger Logger
        {
            get { return _logger ?? (_logger = NLog.LogManager.GetLogger("ODS")); }
        }

        private readonly IEnumerable<ITask> _children;

        public abstract int Priority { get; }
        public ITask ParentTask { get; set; }

        public IEnumerable<ITask> Children { get { return _children; } }

        protected TaskBase(ITask parent, IEnumerable<ITask> children)
        {
            ParentTask = parent;
            _children = children;
        }

        protected abstract TaskOutput Execute(TaskInput input, ITaskContext context);

        protected abstract void ChildrenExecuted(TaskInput childrenInput, IEnumerable<TaskOutput> childrenOutput);

        public TaskOutput ExecuteTaskTree(TaskInput input, ITaskContext context)
        {
            var watch = new Stopwatch();
            Logger.Info("{0} task started", TaskName);
            watch.Start();
            var result = Execute(input, context);
            watch.Stop();
            Logger.Info("{0} task ended, run time: {1} ms", TaskName, watch.ElapsedMilliseconds);

            var list = new List<TaskOutput>();
            if (Children != null)
            {
                Logger.Info("executing {0} task children", TaskName);
                foreach (var child in Children)
                {
                    list.Add(child.ExecuteTaskTree(new TaskInput(result), context));
                }
            }
            ChildrenExecuted(new TaskInput(result), list);

            return result;
        }
    }
}