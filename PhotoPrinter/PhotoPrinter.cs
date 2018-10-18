// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace Aurigma.PhotoKiosk.PhotoPrinter
{
    internal class PhotoPrinter
    {
        private const string OrderInfoFilename = "OrderInfo.xml";
        private readonly string _orderFolder;
        private readonly int _maxCopiesCount;

        private bool _success = false;
        private PrintQueue _printer;
        private string _cropMode;
        private int _width;
        private int _height;

        public PhotoPrinter(string orderFolder, int maxCopiesCount)
        {
            if (string.IsNullOrEmpty(orderFolder))
                throw new ArgumentNullException("orderFolder");

            if (!File.Exists(orderFolder + OrderInfoFilename))
                throw new FileNotFoundException(orderFolder + OrderInfoFilename);

            if (maxCopiesCount <= 0 || maxCopiesCount >= int.MaxValue)
                throw new ArgumentOutOfRangeException("maxCopiesCount");

            _orderFolder = orderFolder;
            _maxCopiesCount = maxCopiesCount;
        }

        public void Print()
        {
            try
            {
                var settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreProcessingInstructions = true;
                settings.IgnoreWhitespace = true;

                using (var reader = XmlReader.Create(_orderFolder + OrderInfoFilename, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.Name == "cropMode")
                        {
                            _cropMode = reader.ReadElementString();
                            continue;
                        }

                        if (reader.Name == "formats")
                        {
                            reader.Read();
                            while (reader.Name == "format")
                            {
                                if (!reader.IsEmptyElement)
                                {
                                    try
                                    {
                                        _printer = GetPrinter(reader.GetAttribute("printer"));
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("    Unable to init printer. Error: " + e.Message);
                                        reader.Skip();
                                        continue;
                                    }

                                    _width = int.Parse(reader.GetAttribute("width"), CultureInfo.InvariantCulture);
                                    _height = int.Parse(reader.GetAttribute("height"), CultureInfo.InvariantCulture);

                                    Console.WriteLine("\n    Init " + _printer.Name + " printer for " + _width.ToString() + "x" + _height.ToString() + " photos.");

                                    reader.Read();
                                    while (reader.Name == "photo")
                                    {
                                        string filename = reader.GetAttribute("file");
                                        Int32Rect crop = ParseRectangle(reader.GetAttribute("crop"));
                                        int quantity = int.Parse(reader.GetAttribute("quantity"), CultureInfo.InvariantCulture);
                                        quantity = Math.Min(quantity, _maxCopiesCount);

                                        PrintImage(filename, crop, quantity);

                                        reader.Read();
                                    }

                                    reader.ReadEndElement();
                                }
                                else
                                    reader.Read();
                            }
                            reader.ReadEndElement();
                        }
                    }
                }
            }
            catch (XmlException e)
            {
                throw new ArgumentException("Unable to parse " + _orderFolder + OrderInfoFilename + " file.", e);
            }
            catch (Exception e)
            {
                throw e;
            }

            if (!_success)
                throw new Exception("No photos were printed.");
        }

        private void PrintImage(string fileName, Int32Rect crop, int quantity)
        {
            Console.WriteLine("    Print image " + fileName + " in " + quantity.ToString() + " copies.");

            PrintTicket ticket = _printer.UserPrintTicket;
            Size paper = new Size(Math.Max(ticket.PageMediaSize.Width.Value, ticket.PageMediaSize.Height.Value), Math.Min(ticket.PageMediaSize.Width.Value, ticket.PageMediaSize.Height.Value));
            Size format = new Size(Math.Max(_width, _height) * 96 / 25.4, Math.Min(_width, _height) * 96 / 25.4);

            double printableWidth;
            double printableHeight;

            if (format.Width <= paper.Width && format.Height <= paper.Height)
            {
                printableWidth = format.Width;
                printableHeight = format.Height;
            }
            else if (format.Width <= paper.Height && format.Height <= paper.Width)
            {
                printableWidth = format.Height;
                printableHeight = format.Width;
            }
            else
            {
                printableWidth = paper.Width;
                printableHeight = paper.Height;
            }

            // Swap printable width and height if page orientation is portarait
            if (ticket.PageOrientation == PageOrientation.Portrait || ticket.PageOrientation == PageOrientation.ReversePortrait)
            {
                double temp = printableWidth;
                printableWidth = printableHeight;
                printableHeight = temp;
            }

            fileName = _orderFolder + fileName;

            // Create FixedDocument containing the image
            var fixedDocument = new FixedDocument();
            var pageContent = new PageContent();
            var fixedPage = new FixedPage();
            var image = CreateImage(fileName, printableWidth, printableHeight, crop);

            fixedPage.Width = printableWidth;
            fixedPage.Height = printableHeight;
            fixedPage.Children.Add(image);
            ((IAddChild)pageContent).AddChild(fixedPage);
            fixedDocument.Pages.Add(pageContent);

            // Save FixedDocument to XPS file
            string xpsFilePath = fileName + _width.ToString() + "x" + _height.ToString() + ".xps";
            using (FileStream outputStream = File.Create(xpsFilePath))
            {
                var package = Package.Open(outputStream, FileMode.Create);
                var xpsDoc = new XpsDocument(package, CompressionOption.Normal);
                var xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsDoc);
                var fixedDocSeq = new FixedDocumentSequence();
                var docRef = new DocumentReference();
                docRef.BeginInit();
                docRef.SetDocument(fixedDocument);
                docRef.EndInit();
                ((IAddChild)fixedDocSeq).AddChild(docRef);

                xpsWriter.Write(fixedDocSeq.DocumentPaginator);

                xpsDoc.Close();
                package.Close();
            }

            // Add the XPS file to print queue
            for (int i = 0; i < quantity; i++)
            {
                _printer.AddJob("Photo Kiosk", xpsFilePath, false);
                _success = true;
            }
        }

        private Image CreateImage(string fileName, double width, double height, Int32Rect crop)
        {
            try
            {
                var image = new Image();
                var bitmap = new BitmapImage();
                var transformedBitmap = new TransformedBitmap();

                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fileName, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.None;
                if (_cropMode == "cropToFillFormat")
                    bitmap.SourceRect = crop;
                bitmap.EndInit();

                transformedBitmap.BeginInit();
                transformedBitmap.Source = bitmap;
                if (height > width) //Portrait orientation
                {
                    if (bitmap.PixelWidth > bitmap.PixelHeight)
                        transformedBitmap.Transform = new RotateTransform(90);
                }
                else //Landscape orientation
                {
                    if (bitmap.PixelHeight > bitmap.PixelWidth)
                        transformedBitmap.Transform = new RotateTransform(90);
                }
                transformedBitmap.EndInit();

                image.Source = transformedBitmap;
                image.Width = Math.Min(transformedBitmap.PixelWidth, width);
                image.Height = Math.Min(transformedBitmap.PixelHeight, height);

                return image;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to create printable image.", e);
            }
        }

        private static PrintQueue GetPrinter(string printerName)
        {
            if (string.IsNullOrEmpty(printerName))
                throw new ArgumentNullException("printerName");

            try
            {
                using (var printServer = new LocalPrintServer())
                {
                    EnumeratedPrintQueueTypes[] enumerationFlags = { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections };

                    foreach (PrintQueue queue in printServer.GetPrintQueues(enumerationFlags))
                    {
                        if (queue.Name == printerName)
                        {
                            return queue;
                        }
                    }
                    throw new ArgumentException("The specified " + printerName + " printer was not found.");
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("The specified " + printerName + " printer was not found.", e);
            }
        }

        private static Int32Rect ParseRectangle(string rect)
        {
            string[] numbers = rect.Split(',');
            int x = 0;
            int.TryParse(numbers[0], out x);
            int y = 0;
            int.TryParse(numbers[1], out y);
            int width = 0;
            int.TryParse(numbers[2], out width);
            int height = 0;
            int.TryParse(numbers[3], out height);

            return new Int32Rect(x, y, width, height);
        }
    }
}