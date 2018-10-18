// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;

namespace Aurigma.PhotoKiosk.Core
{
    public class Tools
    {
        private Tools()
        {
        }

        public static void SaveStreamToFile(Stream stream, string fileName)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);

            try
            {
                byte[] buffer = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);
                fileStream.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                fileStream.Close();
            }
        }

        public static List<string> GetAvailablePrinters()
        {
            var printers = new List<string>();
            using (var printServer = new LocalPrintServer())
            {
                EnumeratedPrintQueueTypes[] enumerationFlags = { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections };

                foreach (PrintQueue queue in printServer.GetPrintQueues(enumerationFlags))
                {
                    printers.Add(queue.Name);
                }
            }
            return printers;
        }
    }
}