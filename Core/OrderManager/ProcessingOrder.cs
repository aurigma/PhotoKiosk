// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using ODS.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Aurigma.PhotoKiosk.Core.OrderManager
{
    internal class ProcessingOrder
    {
        public IEnumerable<IFile> OrderFiles { get; private set; }
        public IEnumerable<IFile> OrderDocs { get; private set; }
        public ITransformationRule Rule { get; private set; }

        internal ProcessingOrder(string orderFolder, OrderManager config)
        {
            if (Directory.Exists(orderFolder) && File.Exists(orderFolder + "\\" + Constants.OrderInfoXmlFileName))
            {
                if (!File.Exists(Config.TransformFile))
                    Config.RestoreDefaultTransformFile();

                var rule = new Rule(config);
                var files = new List<IFile>();
                var docs = new List<IFile>
                    {
                        new ProcessingDoc(orderFolder + "\\" + Constants.OrderInfoXmlFileName, new KeyValuePair<string, string>("%xslt_file_path%", Config.TransformFile))
                    };

                var settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreProcessingInstructions = true;
                settings.IgnoreWhitespace = true;

                using (var reader = XmlReader.Create(orderFolder + "\\" + Constants.OrderInfoXmlFileName, settings))
                {
                    reader.ReadStartElement("order");

                    reader.ReadStartElement("info");
                    string action = reader.ReadElementString();
                    if (action != "OrderPhotos")
                        throw new NotSupportedException("Unsupported action.");

                    rule.ReplacementValues.Add(OrderManager.KioskIdTag, reader.ReadElementString());
                    rule.ReplacementValues.Add(OrderManager.DateTag, reader.ReadElementString().Substring(0, 10));
                    rule.ReplacementValues.Add(OrderManager.OrderIdTag, reader.ReadElementString());
                    rule.ReplacementValues.Add(OrderManager.PaperTypeTag, reader.ReadElementString());
                    rule.ReplacementValues.Add(OrderManager.CropModeTag, reader.ReadElementString());
                    
                    // orderCost
                    reader.ReadElementString();

                    // totalCost
                    reader.ReadElementString(); 
                    reader.ReadEndElement();

                    reader.ReadStartElement("customer");
                    rule.ReplacementValues.Add(OrderManager.CustomerNameTag, reader.ReadElementString());
                    rule.ReplacementValues.Add(OrderManager.CustomerPhoneTag, reader.ReadElementString());
                    rule.ReplacementValues.Add(OrderManager.CustomerEmailTag, reader.ReadElementString());
                    reader.ReadEndElement();

                    reader.ReadStartElement("formats");
                    while (reader.Name == "format")
                    {
                        if (!reader.IsEmptyElement)
                        {
                            string format = reader.GetAttribute("name");
                            string channel = reader.GetAttribute("channel");

                            reader.Read();
                            while (reader.Name == "photo")
                            {
                                var file = new ProcessingFile(orderFolder + "\\" + reader.GetAttribute("file"));
                                file.Params.Add(new KeyValuePair<string, string>(OrderManager.FormatTag, format));
                                file.Params.Add(new KeyValuePair<string, string>(OrderManager.PrintsQuantityTag, reader.GetAttribute("quantity")));

                                if (config.IsDpof.Value && !string.IsNullOrEmpty(channel))
                                {
                                    file.Params.Add(new KeyValuePair<string, string>(OrderManager.DpofPSizeTag, format + " " + rule.ReplacementValues[OrderManager.PaperTypeTag]));
                                    file.Params.Add(new KeyValuePair<string, string>(OrderManager.ChannelTag, channel));
                                }

                                string[] crop = reader.GetAttribute("crop").Split(',');
                                file.Params.Add(new KeyValuePair<string, string>("%cropX%", crop[0]));
                                file.Params.Add(new KeyValuePair<string, string>("%cropY%", crop[1]));
                                file.Params.Add(new KeyValuePair<string, string>("%cropWidth%", crop[2]));
                                file.Params.Add(new KeyValuePair<string, string>("%cropHeight%", crop[3]));

                                files.Add(file);

                                reader.Read();
                            }
                        }
                        reader.Read();
                    }
                    reader.ReadEndElement();
                }

                Rule = rule;
                OrderFiles = files;
                OrderDocs = docs;
            }
            else
                throw new FileNotFoundException(orderFolder + "\\" + Constants.OrderInfoXmlFileName);
        }
    }
}