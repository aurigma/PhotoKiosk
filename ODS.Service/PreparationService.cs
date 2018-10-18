// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using ICSharpCode.SharpZipLib.Zip;
using ODS.Core;
using ODS.Service.RulesEngine;
using ODS.Service.RulesEngine.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ODS.Service
{
    public class PreparationService : IPreparationService
    {
        private NLog.Logger _logger;

        protected NLog.Logger Logger
        {
            get { return _logger ?? (_logger = NLog.LogManager.GetLogger("ODS")); }
        }

        private readonly TransformationRuleParser _parcer;

        public PreparationService()
        {
            _parcer = new TransformationRuleParser();
        }

        public string Prepare(IEnumerable<IFile> inputFiles, IEnumerable<IFile> documents, ITransformationRule rule)
        {
            Logger.Info("=============================");
            Logger.Info("START: {0}", rule.Title);

            if (inputFiles == null) throw new ArgumentNullException("inputFiles");
            if (rule == null) throw new ArgumentNullException("rule");

            Logger.Info("input: order files count: {0}, document files count: {1}", inputFiles.Count(), documents != null ? documents.Count() : 0);

            Logger.Info("rule file parsing: {0}", Path.GetFullPath(rule.RuleFilePath));
            TransformationRule parsedRule;
            try
            {
                parsedRule = _parcer.Parse(rule);
                Logger.Info("rule file parsing: OK (file rules count: {0}, document rules count: {1})",
                    parsedRule.OrderFileRules != null ? parsedRule.OrderFileRules.Count() : 0,
                    parsedRule.DocumentFileRules != null ? parsedRule.DocumentFileRules.Count() : 0);
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                throw;
            }

            var replacementValues = new List<KeyValuePair<string, string>>();
            if (rule.ReplacementValues != null)
                replacementValues.AddRange(rule.ReplacementValues);

            replacementValues.Add(new KeyValuePair<string, string>("%date_now%", DateTime.Now.ToString("yyyy-mm-dd")));
            replacementValues.Add(new KeyValuePair<string, string>("%rule_title%", rule.Title));

            var workingDirectoryName = parsedRule.CreateRandomSubfolderInOutput ? Path.GetRandomFileName() : string.Empty;

            var workingDirectory = Path.Combine(rule.OutputPath, workingDirectoryName);
            if (parsedRule.CreateRandomSubfolderInOutput && Directory.Exists(workingDirectory)) Directory.Delete(workingDirectory, true);
            Directory.CreateDirectory(workingDirectory);

            Logger.Info("working directory is: {0}", Path.GetFullPath(workingDirectory));

            var context = new TaskContext(workingDirectory);

            context.CommonParams.Clear();
            foreach (var pair in replacementValues)
            {
                context.CommonParams.Add(pair.Key, pair.Value);
            }

            if (parsedRule.OrderFileRules != null)
            {
                Logger.Info("executing order file rules...");
                foreach (var orderFileRule in parsedRule.OrderFileRules)
                {
                    orderFileRule.Apply(inputFiles, context);
                }
            }

            if (parsedRule.DocumentFileRules != null && documents != null)
            {
                Logger.Info("executing document file rules...");
                foreach (var documentFileRule in parsedRule.DocumentFileRules)
                {
                    documentFileRule.Apply(documents, context);
                }
            }

            if (parsedRule.ZipResult && parsedRule.CreateRandomSubfolderInOutput)
            {
                try
                {
                    var resultPath = ZipFolder(workingDirectory);
                    Directory.Delete(workingDirectory, true);
                    return resultPath;
                }
                catch (Exception) { throw; }
            }
            else
            {
                return workingDirectory;
            }
        }

        private static string ZipFolder(string workingDirectory)
        {
            if (!Directory.Exists(workingDirectory)) throw new ArgumentException("Folder not exists");

            var zipFilePath = workingDirectory + ".zip";
            if (File.Exists(zipFilePath))
                File.Delete(zipFilePath);

            var zip = new FastZip();
            zip.CreateZip(zipFilePath, workingDirectory, true, null);

            return zipFilePath;
        }
    }
}