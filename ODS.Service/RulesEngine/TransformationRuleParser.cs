// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using ODS.Core;
using ODS.Service.RulesEngine.Constraints;
using ODS.Service.RulesEngine.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ODS.Service.RulesEngine
{
    public class TransformationRuleParser
    {
        public TransformationRule Parse(ITransformationRule ruleInfo)
        {
            if (ruleInfo == null) throw new ArgumentNullException("ruleInfo");
            if (!File.Exists(ruleInfo.RuleFilePath))
                throw new FileNotFoundException("Rules file not found.", ruleInfo.RuleFilePath);

            try
            {
                var document = new XmlDocument();
                document.Load(ruleInfo.RuleFilePath);

                var result = new TransformationRule();

                var rootNode = document.SelectSingleNode("rule");

                result.Title = rootNode.Attributes["title"].Value;
                result.ZipResult = rootNode.Attributes["zip"] != null && rootNode.Attributes["zip"].Value == "true";
                result.CreateRandomSubfolderInOutput = rootNode.Attributes["createRandomSubfolderInOutput"] == null
                    || rootNode.Attributes["createRandomSubfolderInOutput"].Value == "true";

                List<FileRule> orderFileRules = null;
                List<FileRule> docsFileRules = null;

                // processing order files
                var orderFilesRulesNode = rootNode.SelectSingleNode("files");
                if (orderFilesRulesNode != null)
                {
                    orderFileRules = new List<FileRule>();
                    var fileRuleNodes = orderFilesRulesNode.SelectNodes("file");
                    if (fileRuleNodes != null)
                    {
                        foreach (XmlNode ruleNode in fileRuleNodes)
                        {
                            var fileRule = ParseFileRuleNode(ruleNode);
                            orderFileRules.Add(fileRule);
                        }
                    }
                }

                // processing document files
                var docsFilesRulesNode = rootNode.SelectSingleNode("docs");
                if (docsFilesRulesNode != null)
                {
                    docsFileRules = new List<FileRule>();
                    var fileRuleNodes = docsFilesRulesNode.SelectNodes("file");
                    if (fileRuleNodes != null)
                    {
                        foreach (XmlNode ruleNode in fileRuleNodes)
                        {
                            var fileRule = ParseFileRuleNode(ruleNode);
                            docsFileRules.Add(fileRule);
                        }
                    }
                }

                result.OrderFileRules = orderFileRules;
                result.DocumentFileRules = docsFileRules;

                return result;
            }
            catch (Exception)
            {
                throw new InvalidDataException("Rules file is not well formatted XML");
            }
        }

        private static FileRule ParseFileRuleNode(XmlNode ruleNode)
        {
            if (ruleNode == null) throw new ArgumentNullException("ruleNode");
            if (ruleNode.Name != "file") throw new ArgumentException("ruleNode is not file node");

            var constraints = new List<IConstraint>();
            var constraintsNode = ruleNode.SelectSingleNode("constraints");
            if (constraintsNode != null)
            {
                foreach (XmlNode constraintNode in constraintsNode.ChildNodes)
                {
                    if (constraintNode.NodeType != XmlNodeType.Element) continue;

                    IConstraint constraint = null;
                    switch (constraintNode.Name)
                    {
                        case "param":
                            constraint = new ParamConstraint(constraintNode.Attributes["key"].Value, constraintNode.Attributes["pattern"].Value);
                            break;

                        case "path":
                            constraint = new PathConstraint(constraintNode.Attributes["pattern"].Value);
                            break;
                    }
                    if (constraint != null)
                        constraints.Add(constraint);
                }
            }

            IEnumerable<ITask> tasks = null;
            var taskNodes = ruleNode.SelectSingleNode("tasks");
            if (taskNodes != null)
            {
                tasks = ParseChildren(taskNodes);
            }

            string title = string.Empty;
            if (ruleNode.Attributes != null && ruleNode.Attributes["title"] != null)
                title = ruleNode.Attributes["title"].Value;

            return new FileRule(title, tasks, constraints);
        }

        private static IEnumerable<ITask> ParseChildren(XmlNode parent)
        {
            if (parent == null || parent.ChildNodes.Count == 0) return null;

            var tasks = new List<ITask>();
            foreach (XmlNode child in parent.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;
                var children = ParseChildren(child);

                ITask task = null;
                var transliteratePath = child.Attributes["transliteratePath"] != null && bool.Parse(child.Attributes["transliteratePath"].Value);
                switch (child.Name)
                {
                    case "copy":

                        var copy = new CopyTask(null, children, child.Attributes["target"].Value, transliteratePath);
                        var overrideMode = OverwriteMode.Rename;
                        if (child.Attributes["overwriteMode"] != null)
                            overrideMode = (OverwriteMode)Enum.Parse(typeof(OverwriteMode), child.Attributes["overwriteMode"].Value, true);
                        copy.OverrideMode = overrideMode;
                        task = copy;
                        break;

                    case "xslt":
                        task = new XsltTask(child.Attributes["filePathKey"].Value, null, children);
                        break;

                    case "crop":
                        task = new CropTask(child.Attributes["xKey"].Value, child.Attributes["yKey"].Value,
                            child.Attributes["widthKey"].Value, child.Attributes["heightKey"].Value, null, children);
                        break;

                    case "watermark":
                        task = new WatermarkTask(child.Attributes["textKey"].Value, child.Attributes["textPositionKey"].Value, null, children);
                        break;

                    case "temp":
                        task = new TempTask(null, children);
                        break;

                    case "delete":
                        task = new DeleteTask(null, children, bool.Parse(child.Attributes["deleteEmptyFolders"].Value));
                        break;

                    case "dpof":
                        var targetDirectory = child.Attributes["targetDirectory"].Value;
                        var channelDirectoryName = child.Attributes["channelDirectoryName"].Value;
                        var copyCountKey = child.Attributes["copyCountKey"].Value;
                        var paperSizeKey = child.Attributes["paperSizeKey"].Value;
                        var channelKey = child.Attributes["printChannelKey"].Value;
                        task = new DpofTask(null, children, copyCountKey, paperSizeKey, channelDirectoryName, channelKey, targetDirectory, transliteratePath);
                        break;

                    case "convert":
                        ConvertTask.ConvertFormat format;
                        Enum.TryParse(child.Attributes["to"].Value, true, out format);
                        task = new ConvertTask(format, null, children);
                        break;
                }
                if (task != null)
                {
                    if (children != null)
                    {
                        foreach (var ch in children)
                        {
                            ch.ParentTask = task;
                        }
                    }
                    tasks.Add(task);
                }
            }
            return tasks;
        }
    }
}