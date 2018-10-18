// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using ODS.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace ODS.Service.RulesEngine.Tasks
{
    public class TaskFile : IFile
    {
        public const string FileNameKey = "%file_name%";
        public const string FileExtKey = "%file_ext%";
        public const string FileKey = "%file%";

        private readonly string _path;
        private readonly IDictionary<string, string> _params;

        public string Path
        {
            get { return _path; }
        }

        public IDictionary<string, string> Params
        {
            get { return _params; }
        }

        public TaskFile(string path, IEnumerable<KeyValuePair<string, string>> @params)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");

            _path = path;
            _params = new Dictionary<string, string>();

            if (@params != null)
            {
                foreach (var param in @params)
                {
                    _params.Add(param.Key, param.Value);
                }
            }

            var info = new FileInfo(_path);

            if (_params.ContainsKey(FileNameKey)) _params[FileNameKey] = info.Name.Replace(info.Extension, string.Empty);
            else _params.Add(FileNameKey, info.Name.Replace(info.Extension, string.Empty));

            if (_params.ContainsKey(FileExtKey)) _params[FileExtKey] = info.Extension.Substring(1);
            else _params.Add(FileExtKey, info.Extension.Substring(1));

            if (_params.ContainsKey(FileKey)) _params[FileKey] = info.Name;
            else _params.Add(FileKey, info.Name);
        }

        public TaskFile(IFile file) : this(file.Path, file.Params)
        {
            if (file == null) throw new ArgumentNullException("file");
        }
    }
}