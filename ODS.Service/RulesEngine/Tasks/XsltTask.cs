// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace ODS.Service.RulesEngine.Tasks
{
    public class XsltTask : TaskBase
    {
        private readonly string _xsltFilePathKey;

        public XsltTask(string xsltFilePathKey, ITask parent, IEnumerable<ITask> children) : base(parent, children)
        {
            _xsltFilePathKey = xsltFilePathKey;
        }

        protected override string TaskName
        {
            get { return string.Format("XSLT"); }
        }

        public override int Priority
        {
            get { return 1; }
        }

        protected override TaskOutput Execute(TaskInput input, ITaskContext context)
        {
            var xsltPath = context.CommonParams[_xsltFilePathKey];
            if (!File.Exists(xsltPath)) throw new ArgumentException(string.Format("Xslt file not found: {0}", xsltPath));

            var outputFiles = new List<TaskFile>();
            foreach (var taskFile in input.Files)
            {
                using (var inputStream = new StreamReader(taskFile.Path, true))
                {
                    var source = new XPathDocument(inputStream);
                    var xslt = new XslCompiledTransform();
                    xslt.Load(xsltPath);
                    using (var myWriter = new XmlTextWriter(taskFile.Path, Encoding.UTF8))
                    {
                        xslt.Transform(source, null, myWriter);
                    }
                }
                outputFiles.Add(new TaskFile(taskFile));
            }
            return new TaskOutput(outputFiles, input.Params);
        }

        protected override void ChildrenExecuted(TaskInput childrenInput, IEnumerable<TaskOutput> childrenOutput)
        {
        }
    }
}