// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using BinaryAnalysis.UnidecodeSharp;
using ODS.Service.DPOF;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ODS.Service.RulesEngine.Tasks
{
    public class DpofTask : TaskBase
    {
        private readonly string _copyCountKey;
        private readonly string _paperSizeKey;
        private readonly string _channelDirectoryName;
        private readonly string _printChannelKey;
        private readonly string _targetDirectory;
        private readonly bool _transliteratePath;

        public DpofTask(ITask parent, IEnumerable<ITask> children, string copyCountKey, string paperSizeKey,
            string channelDirectoryName, string printChannelKey, string targetDirectory, bool transliteratePath)
            : base(parent, children)
        {
            _copyCountKey = copyCountKey;
            _paperSizeKey = paperSizeKey;
            _channelDirectoryName = channelDirectoryName;
            _printChannelKey = printChannelKey;
            _targetDirectory = targetDirectory;
            _transliteratePath = transliteratePath;
        }

        protected override string TaskName
        {
            get { return string.Format("DPOF"); }
        }

        public override int Priority
        {
            get { return 1; }
        }

        protected override TaskOutput Execute(TaskInput input, ITaskContext context)
        {
            var channelFilesDictionary = new Dictionary<string, List<TaskFile>>();
            foreach (var file in input.Files)
            {
                if (file.Params.ContainsKey(_printChannelKey))
                {
                    var channal = file.Params[_printChannelKey];
                    if (!channelFilesDictionary.ContainsKey(channal))
                        channelFilesDictionary[channal] = new List<TaskFile>();

                    channelFilesDictionary[channal].Add(file);
                }
            }

            var outputFiles = new List<TaskFile>();
            foreach (var channelFiles in channelFilesDictionary)
            {
                var channel = channelFiles.Key;
                Logger.Info("Found {0} files for {1} channel", channelFiles.Value.Count, channel);

                var @params = new Dictionary<string, string>(context.CommonParams);
                if (!@params.ContainsKey(_printChannelKey))
                    @params.Add(_printChannelKey, channel);

                var miscDirectoryPath = GetMiscDirectoryPath(context.WorkingDirectory, @params);
                var filesDirectoryPath = GetFilesDirectoryPath(context.WorkingDirectory, @params);

                Logger.Info("dpof FILES directory path: {0}", Path.GetFullPath(filesDirectoryPath));
                Logger.Info("dpof MISC directory path: {0}", Path.GetFullPath(miscDirectoryPath));

                var printFiles = new List<PrintFileInfo>();
                foreach (var file in channelFiles.Value)
                {
                    var sourceFile = new FileInfo(file.Path);
                    var fileName = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(file.Path).Unidecode().Trim(), sourceFile.Extension).Replace(" ", "_");
                    var newPath = Path.Combine(filesDirectoryPath, fileName);

                    if (File.Exists(newPath))
                    {
                        Logger.Warn("warning: file exists ({0})", Path.GetFullPath(newPath));

                        var newName = Path.GetFileNameWithoutExtension(newPath);
                        var fi = new FileInfo(newPath);
                        var dirPath = fi.DirectoryName;
                        var i = 1;
                        while (i < 99)
                        {
                            newName = string.Format("{0}_{1}{2}", newName, i, fi.Extension);
                            var path = Path.Combine(dirPath, newName);
                            if (!File.Exists(path))
                            {
                                newPath = path;
                                break;
                            }
                            i++;
                        }
                        Logger.Warn("new file path is: {0}", Path.GetFullPath(newPath));
                        File.Copy(file.Path, newPath, true);
                    }
                    else
                        File.Copy(sourceFile.FullName, newPath, false);

                    Logger.Info("file copy: {0} to {1}", Path.GetFullPath(sourceFile.FullName), newPath);
                    printFiles.Add(new PrintFileInfo() { CopyCount = int.Parse(file.Params[_copyCountKey]), FilePath = newPath });

                    outputFiles.Add(new TaskFile(newPath, file.Params));
                }

                var paperSize = channelFiles.Value.First().Params[_paperSizeKey];
                var doc = new NoritsuDpofDocument(miscDirectoryPath, printFiles, paperSize, int.Parse(channel));
                var result = doc.GetRenderedFilesPath();
                foreach (var filePath in result)
                {
                    Logger.Info("dpof result file: {0}", Path.GetFullPath(filePath));
                    outputFiles.Add(new TaskFile(filePath, null));
                }
            }
            return new TaskOutput(outputFiles);
        }

        private string GetChannelDirectoryPath(string workingDirectory, Dictionary<string, string> @params)
        {
            var resultPath = string.Format("{0}\\{1}\\", _targetDirectory, _channelDirectoryName);
            foreach (var pair in @params)
            {
                resultPath = resultPath.Replace(pair.Key, pair.Value);
            }
            if (resultPath.StartsWith("\\"))
                resultPath = resultPath.Substring(1);

            if (_transliteratePath)
                resultPath = resultPath.Unidecode();

            resultPath = Path.Combine(workingDirectory, resultPath).Replace("\\\\", "\\");
            Directory.CreateDirectory(resultPath);
            return resultPath;
        }

        private string GetMiscDirectoryPath(string workingDirectory, Dictionary<string, string> @params)
        {
            var resultPath = Path.Combine(GetChannelDirectoryPath(workingDirectory, @params), "MISC\\").Replace("\\\\", "\\");
            Directory.CreateDirectory(resultPath);
            return resultPath;
        }

        private string GetFilesDirectoryPath(string workingDirectory, Dictionary<string, string> @params)
        {
            var resultPath = Path.Combine(GetChannelDirectoryPath(workingDirectory, @params), "FILES\\").Replace("\\\\", "\\");
            Directory.CreateDirectory(resultPath);
            return resultPath;
        }

        protected override void ChildrenExecuted(TaskInput childrenInput, IEnumerable<TaskOutput> childrenOutput)
        {
        }
    }
}