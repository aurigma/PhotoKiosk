// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using ODS.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Aurigma.PhotoKiosk.Core.OrderManager
{
    internal class Rule : ITransformationRule
    {
        private string _title;

        public string Title
        {
            get { return _title; }
        }

        private string _outFolder;

        public string OutputPath
        {
            get { return _outFolder; }
        }

        public string RuleFilePath
        {
            get { return Config.GetFullPath("rule.xml"); }
        }

        private readonly IDictionary<string, string> _replacementValues;

        public IDictionary<string, string> ReplacementValues
        {
            get { return _replacementValues; }
        }

        public Rule(OrderManager config)
        {
            _outFolder = config.DestinationPath.Value;
            _replacementValues = new Dictionary<string, string>();

            _title = config.IsDpof.Value ? "DpofRule" : "ArbitraryRule";

            try
            {
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = false;
                settings.Encoding = Encoding.UTF8;

                var stream = new MemoryStream(4096);
                try
                {
                    using (var writer = XmlWriter.Create(stream, settings))
                    {
                        writer.WriteStartElement("rule");
                        writer.WriteAttributeString("title", Title);
                        writer.WriteAttributeString("zip", "false");
                        writer.WriteAttributeString("createRandomSubfolderInOutput", "false");

                        if (!config.IsDpof.Value)
                        {
                            writer.WriteStartElement("files");
                            writer.WriteStartElement("file");
                            writer.WriteAttributeString("title", "arbitrary");
                            writer.WriteStartElement("tasks");
                            writer.WriteStartElement("copy");
                            if (config.ConvertToJpeg.Value)
                                writer.WriteAttributeString("target", config.PhotoTemplate.Value.Replace(OrderManager.FilenameTag, "%file_name%.jpg"));
                            else
                                writer.WriteAttributeString("target", config.PhotoTemplate.Value.Replace(OrderManager.FilenameTag, "%file%"));
                            writer.WriteAttributeString("transliteratePath", config.TransliteratePath.Value.ToString());
                            writer.WriteAttributeString("overwriteMode", "rename");

                            if (config.ConvertToJpeg.Value)
                            {
                                writer.WriteStartElement("convert");
                                writer.WriteAttributeString("to", "jpeg");
                                writer.WriteEndElement();
                            }

                            writer.WriteStartElement("crop");
                            writer.WriteAttributeString("xKey", "%cropX%");
                            writer.WriteAttributeString("yKey", "%cropY%");
                            writer.WriteAttributeString("widthKey", "%cropWidth%");
                            writer.WriteAttributeString("heightKey", "%cropHeight%");
                            writer.WriteEndElement();

                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();

                            writer.WriteEndElement();

                            writer.WriteStartElement("docs");
                            writer.WriteStartElement("file");
                            writer.WriteStartElement("tasks");
                            writer.WriteStartElement("copy");
                            writer.WriteAttributeString("target", config.InfoTemplate.Value);
                            writer.WriteAttributeString("transliteratePath", config.TransliteratePath.Value.ToString());
                            writer.WriteAttributeString("overwriteMode", "rename");
                            writer.WriteStartElement("xslt");
                            writer.WriteAttributeString("filePathKey", "%xslt_file_path%");
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        else
                        {
                            writer.WriteStartElement("files");
                            writer.WriteStartElement("file");
                            writer.WriteAttributeString("title", "dpof");
                            writer.WriteStartElement("tasks");
                            writer.WriteStartElement("copy");
                            if (config.ConvertToJpeg.Value)
                                writer.WriteAttributeString("target", OrderManager.KioskIdTag + @"\tmp_" + OrderManager.ChannelTag + @"\%file_name%.jpg");
                            else
                                writer.WriteAttributeString("target", OrderManager.KioskIdTag + @"\tmp_" + OrderManager.ChannelTag + @"\%file%");
                            writer.WriteAttributeString("transliteratePath", config.TransliteratePath.Value.ToString());
                            writer.WriteAttributeString("overwriteMode", "rename");

                            if (config.ConvertToJpeg.Value)
                            {
                                writer.WriteStartElement("convert");
                                writer.WriteAttributeString("to", "jpeg");
                                writer.WriteEndElement();
                            }

                            writer.WriteStartElement("dpof");
                            writer.WriteAttributeString("targetDirectory", config.DpofOrderTemplate.Value);
                            writer.WriteAttributeString("transliteratePath", config.TransliteratePath.Value.ToString());
                            writer.WriteAttributeString("channelDirectoryName", "print-channel-" + OrderManager.ChannelTag);
                            writer.WriteAttributeString("copyCountKey", OrderManager.PrintsQuantityTag);
                            writer.WriteAttributeString("paperSizeKey", OrderManager.DpofPSizeTag);
                            writer.WriteAttributeString("printChannelKey", OrderManager.ChannelTag);

                            writer.WriteStartElement("crop");
                            writer.WriteAttributeString("xKey", "%cropX%");
                            writer.WriteAttributeString("yKey", "%cropY%");
                            writer.WriteAttributeString("widthKey", "%cropWidth%");
                            writer.WriteAttributeString("heightKey", "%cropHeight%");
                            writer.WriteEndElement();
                            writer.WriteEndElement();

                            writer.WriteStartElement("delete");
                            writer.WriteAttributeString("deleteEmptyFolders", "true");
                            writer.WriteEndElement();

                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();

                            writer.WriteEndElement();

                            writer.WriteStartElement("docs");
                            writer.WriteStartElement("file");
                            writer.WriteStartElement("tasks");
                            writer.WriteStartElement("copy");
                            writer.WriteAttributeString("target", config.DpofInfoTemplate.Value);
                            writer.WriteAttributeString("transliteratePath", config.TransliteratePath.Value.ToString());
                            writer.WriteAttributeString("overwriteMode", "rename");
                            writer.WriteStartElement("xslt");
                            writer.WriteAttributeString("filePathKey", "%xslt_file_path%");
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }
                }
                catch
                {
                    stream.Close();
                }

                Tools.SaveStreamToFile(stream, RuleFilePath);
                stream.Close();
            }
            catch (Exception)
            {
            }
        }

        ~Rule()
        {
            try
            {
                File.Delete(RuleFilePath);
            }
            catch
            {
            }
        }
    }
}