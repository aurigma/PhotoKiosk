// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using BinaryAnalysis.UnidecodeSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace ODS.Service.RulesEngine.Tasks
{
    internal enum OverwriteMode
    {
        Rename,
        Replace,
        Skip
    }

    internal class CopyTask : TaskBase
    {
        private readonly string _target;
        private readonly bool _transliteratePath;

        public CopyTask(ITask parent, IEnumerable<ITask> children, string target, bool transliteratePath)
            : base(parent, children)
        {
            _target = target;
            _transliteratePath = transliteratePath;
        }

        private OverwriteMode _overrideMode = OverwriteMode.Rename;

        public OverwriteMode OverrideMode
        {
            get { return _overrideMode; }
            set { _overrideMode = value; }
        }

        protected override string TaskName
        {
            get { return string.Format("COPY"); }
        }

        public override int Priority
        {
            get { return 99; }
        }

        protected override TaskOutput Execute(TaskInput inputFiles, ITaskContext context)
        {
            var outputFiles = new List<TaskFile>();
            foreach (var file in inputFiles.Files)
            {
                var target = _target;

                var @params = new List<KeyValuePair<string, string>>();
                @params.AddRange(context.CommonParams);
                if (file.Params != null) { @params.AddRange(file.Params); }

                foreach (var pair in @params)
                {
                    target = target.Replace(pair.Key, pair.Value);
                }

                target = target.Replace("\\\\", "\\");
                if (target.StartsWith("\\"))
                    target = target.Substring(1);

                var sourceFileInfo = new FileInfo(file.Path);
                var targetFilePath = Path.Combine(context.WorkingDirectory, target);

                targetFilePath = GetSafeFilePath(targetFilePath);

                var targetFileInfo = new FileInfo(targetFilePath);
                Directory.CreateDirectory(targetFileInfo.DirectoryName);

                if (string.IsNullOrEmpty(targetFileInfo.Name))
                    targetFilePath = Path.Combine(targetFilePath, sourceFileInfo.Name);

                if (!Path.GetFullPath(targetFilePath).StartsWith(Path.GetFullPath(context.WorkingDirectory), StringComparison.InvariantCultureIgnoreCase))
                {
                    Logger.Error("target file path is outside working directory:", Path.GetFullPath(targetFilePath));
                    throw new ArgumentException("Target file path is outside working directory.");
                }

                Logger.Info("copy from {0} to {1}",
                            Path.GetFullPath(file.Path),
                            Path.GetFullPath(targetFilePath));

                if (File.Exists(targetFilePath))
                {
                    Logger.Warn("warning: file exists ({0}), conflict resolve mode: {1}", Path.GetFullPath(targetFilePath), OverrideMode);
                    switch (OverrideMode)
                    {
                        case OverwriteMode.Replace:
                            File.Copy(file.Path, targetFilePath, true);
                            break;

                        case OverwriteMode.Rename:
                            var newName = Path.GetFileNameWithoutExtension(targetFilePath);
                            var fi = new FileInfo(targetFilePath);
                            var dirPath = fi.DirectoryName;
                            var i = 1;
                            while (i < 99)
                            {
                                newName = string.Format("{0}_{1}{2}", newName, i, fi.Extension);
                                var newPath = Path.Combine(dirPath, newName);
                                if (!File.Exists(newPath))
                                {
                                    targetFilePath = newPath;
                                    break;
                                }
                                i++;
                            }
                            Logger.Warn("new file path is: {0}", Path.GetFullPath(targetFilePath));
                            File.Copy(file.Path, targetFilePath, true);
                            break;

                        case OverwriteMode.Skip:
                            break;
                    }
                }
                else
                    File.Copy(file.Path, targetFilePath, true);

                var targetFile = new TaskFile(targetFilePath, file.Params);
                outputFiles.Add(targetFile);
            }
            return new TaskOutput(outputFiles);
        }

        private string GetSafeFilePath(string targetFilePath)
        {
            if (_transliteratePath)
                targetFilePath = targetFilePath.Unidecode();

            //string regexSearch = string.Format("{0}{1}", new string(Path.GetInvalidFileNameChars()), new string(Path.GetInvalidPathChars()));
            //var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));

            var result = targetFilePath.Replace("×", "x").Replace("'", "");
            return result;
        }

        protected override void ChildrenExecuted(TaskInput childrenInput, IEnumerable<TaskOutput> childrenOutput)
        {
        }
    }
}