// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.IO;
using System.IO.Packaging;
using System.Printing;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;

namespace Aurigma.PhotoKiosk.Core
{
    public class Receipt
    {
        private FixedDocument _template;
        private string _receiptFile;

        public Receipt(ReceiptData data, string receiptFile)
        {
            _receiptFile = receiptFile;

            if (!File.Exists(_receiptFile))
                Config.RestoreDefaultReceiptFile(_receiptFile);

            using (FileStream inputStream = File.OpenRead(_receiptFile))
            {
                _template = (FixedDocument)XamlReader.Load(inputStream);
            }

            _template.DataContext = data;
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.SystemIdle, new DispatcherOperationCallback(delegate { return null; }), null);
        }

        public void Print(string printerName)
        {
            string receiptPath = Path.GetDirectoryName(_receiptFile) + "\\" + Path.GetFileNameWithoutExtension(_receiptFile) + ".xps";
            if (File.Exists(receiptPath))
                File.Delete(receiptPath);

            using (FileStream outputStream = File.Create(receiptPath))
            {
                var package = Package.Open(outputStream, FileMode.Create);
                var xpsDoc = new XpsDocument(package, CompressionOption.Normal);
                var xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsDoc);

                var fixedDocSeq = new FixedDocumentSequence();
                var docRef = new DocumentReference();
                docRef.BeginInit();
                docRef.SetDocument(_template);
                docRef.EndInit();
                ((IAddChild)fixedDocSeq).AddChild(docRef);

                xpsWriter.Write(fixedDocSeq.DocumentPaginator);

                xpsDoc.Close();
                package.Close();
            }

            using (var printServer = new LocalPrintServer())
            {
                EnumeratedPrintQueueTypes[] enumerationFlags = { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections };

                foreach (PrintQueue queue in printServer.GetPrintQueues(enumerationFlags))
                {
                    if (queue.Name == printerName)
                    {
                        queue.AddJob("Photo Kiosk", receiptPath, false);
                        break;
                    }
                }
            }
        }
    }
}