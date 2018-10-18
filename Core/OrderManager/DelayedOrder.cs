// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Aurigma.PhotoKiosk.Core.OrderManager
{
    public class DelayedOrder
    {
        private string _folder;
        private string _action;
        private string _code;

        internal DelayedOrder(string folder, string action, string code)
        {
            _folder = folder;
            _action = action;
            _code = code;
        }

        public string Folder
        {
            get { return _folder; }
        }

        public string Action
        {
            get { return _action; }
        }

        public string Code
        {
            get { return _code; }
        }

        public static DelayedOrder GetOrder(string orderId)
        {
            if (Directory.Exists(Config.TempStoragePath))
            {
                foreach (string orderDir in Directory.GetDirectories(Config.TempStoragePath))
                {
                    string orderInfoFilename = orderDir + "\\" + Constants.OrderInfoXmlFileName;
                    if (new DirectoryInfo(orderDir).Name.StartsWith(orderId, true, CultureInfo.CurrentCulture) && (File.Exists(orderInfoFilename)))
                    {
                        var settings = new XmlReaderSettings();
                        settings.IgnoreComments = true;
                        settings.IgnoreProcessingInstructions = true;
                        settings.IgnoreWhitespace = true;

                        string action = null;
                        string code = null;
                        using (XmlReader reader = XmlReader.Create(orderInfoFilename))
                        {
                            while (reader.Read())
                            {
                                if (reader.Name == "action")
                                    action = reader.ReadElementString();
                                else if (reader.Name == "activationCode")
                                    code = reader.ReadElementString();
                                else if (reader.Name == "customer")
                                    break;
                            }
                        }
                        if (!string.IsNullOrEmpty(action) && !string.IsNullOrEmpty(code))
                            return new DelayedOrder(orderDir, action, code);
                    }
                }
            }

            return null;
        }

        public static string CreateActivationCode(string order)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(order + "magic");
            byte[] hash = (new MD5CryptoServiceProvider()).ComputeHash(bytes);
            int sum = BitConverter.ToInt32(hash, 0) + BitConverter.ToInt32(hash, 4) + BitConverter.ToInt32(hash, 8) + BitConverter.ToInt32(hash, 12);

            return sum.ToString("X2");
        }
    }
}