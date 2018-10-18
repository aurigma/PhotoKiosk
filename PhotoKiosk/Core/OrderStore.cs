// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using Aurigma.PhotoKiosk.Core.OrderManager;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

namespace Aurigma.PhotoKiosk
{
    public class FileOrderStorage
    {
        private Order _order;
        private string _currentOrderPath;
        private string _orderDate;

        public Order Order
        {
            get
            {
                return _order;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("order");

                _order = (Order)value;
            }
        }

        public string CurrentOrderPath
        {
            get { return _currentOrderPath; }
        }

        public void Add(IProgressCallback callback)
        {
            Thread workerThread = new Thread(new ParameterizedThreadStart(ProcessAddOrder));
            workerThread.Start((object)callback);
        }

        public static void ClearBluetoothDirectory()
        {
            if (ExecutionEngine.Config.IsBluetoothEnabled())
            {
                if (Directory.Exists(ExecutionEngine.Config.BluetoothFolder.Value))
                {
                    try
                    {
                        var dirInfo = new DirectoryInfo(ExecutionEngine.Config.BluetoothFolder.Value);

                        // Removing files from the Bluetooth folder itself
                        foreach (FileInfo fileInfo in dirInfo.GetFiles())
                        {
                            fileInfo.Delete();
                        }

                        // Cleaning all 1st level folders
                        foreach (DirectoryInfo subFolder in dirInfo.GetDirectories())
                        {
                            foreach (DirectoryInfo di in subFolder.GetDirectories())
                                di.Delete(true);

                            foreach (FileInfo fi in subFolder.GetFiles())
                                fi.Delete();
                        }
                    }
                    catch (Exception e)
                    {
                        ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                    }
                }
            }
        }

        private void ProcessAddOrder(object objCallback)
        {
            IProgressCallback callback = (IProgressCallback)objCallback;
            ExecutionEngine.EventLogger.Write("FileOrderStorage:AddOrder");

            if (callback != null)
            {
                if (callback.IsAborted)
                    return;

                callback.SetRange(0, _order.OrderItems.Count);
            }

            CreateStoreFolder();

            if (!string.IsNullOrEmpty(_currentOrderPath))
            {
                LockOrderFolder(_currentOrderPath);

                try
                {
                    foreach (OrderItem item in _order.OrderItems)
                    {
                        if (callback != null)
                        {
                            if (callback.IsAborted)
                                throw new InvalidOperationException();

                            callback.Increment();
                        }

                        try
                        {
                            item.OrderStoreFileName = VerifyFileName(_currentOrderPath, item.SourcePhotoItem.SourceFileNameWithoutPath);
                            ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "Copying file from: {0} to: {1}", item.SourcePhotoItem.ActualFileName, _currentOrderPath + "\\" + item.OrderStoreFileName));
                            File.Copy(item.SourcePhotoItem.ActualFileName, _currentOrderPath + "\\" + item.OrderStoreFileName);
                        }
                        catch (Exception e)
                        {
                            ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "File copy error {0}. Message: {1}", item.SourcePhotoItem.ActualFileName, e.Message));
                            ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                        }
                    }

                    SaveOrderInfoToFile(_order, _currentOrderPath + "\\" + Constants.OrderInfoXmlFileName);
                    UnlockOrderFolder(_currentOrderPath);
                    IncreaseOrderId();

                    if (callback != null)
                        callback.IsComplete = true;
                }
                catch (InvalidOperationException)
                {
                    callback.IsComplete = false;

                    ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "Order process was cancelled"));
                    ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "Remove Folder: {0}", _currentOrderPath));
                    try
                    {
                        Directory.Delete(_currentOrderPath, true);
                        ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "Folder {0} removed succesfully", _currentOrderPath));
                    }
                    catch (Exception e)
                    {
                        ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "Uneble to remove folder {0}. {1}", _currentOrderPath, e.Message));
                    }
                }

                callback.End();
            }
        }

        private void CreateStoreFolder()
        {
            _orderDate = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}_{3}_{4}_{5}",
               DateTime.Now.Year.ToString("0000", CultureInfo.InvariantCulture),
               DateTime.Now.Month.ToString("00", CultureInfo.InvariantCulture),
               DateTime.Now.Day.ToString("00", CultureInfo.InvariantCulture),
               DateTime.Now.Hour.ToString("00", CultureInfo.InvariantCulture),
               DateTime.Now.Minute.ToString("00", CultureInfo.InvariantCulture),
               DateTime.Now.Second.ToString("00", CultureInfo.InvariantCulture));

            string storePath = Config.TempStoragePath;
            if (!storePath.EndsWith(@"\"))
                storePath += @"\";

            _currentOrderPath = string.Format(CultureInfo.InvariantCulture, "{0}{1}_{2}", storePath, _order.OrderId, _orderDate);

            try
            {
                ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "Creating Folder: {0}", _currentOrderPath));
                Directory.CreateDirectory(_currentOrderPath);
                ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "Folder {0} created succesfully", _currentOrderPath));
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "Couldn't create order folder {0}", _currentOrderPath));
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                _currentOrderPath = "";
            }
        }

        private void SaveOrderInfoToFile(Order order, string fileName)
        {
            ExecutionEngine.EventLogger.Write("Order:SaveOrderInfoToFile");
            if (order == null)
                throw new ArgumentNullException("order");

            if (fileName == null)
                throw new ArgumentNullException("fileName");

            XmlWriter writer = null;

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = false;
                settings.Encoding = Encoding.UTF8;

                writer = XmlTextWriter.Create(fileName, settings);

                writer.WriteStartElement("order");

                // Order Info
                writer.WriteStartElement("info");
                writer.WriteElementString("action", ExecutionEngine.Instance.PrimaryAction.ToString());
                writer.WriteElementString("kioskId", ExecutionEngine.Config.PhotoKioskId.Value);
                writer.WriteElementString("date", _orderDate);
                writer.WriteElementString("orderId", order.OrderId);

                if ((ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd && ExecutionEngine.Config.CDBurningRequireConfirm.Value) ||
                    (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.PrintPhotos && ExecutionEngine.Config.PhotoPrintingRequireConfirm.Value))
                    writer.WriteElementString("activationCode", DelayedOrder.CreateActivationCode(order.OrderId));

                writer.WriteElementString("paperType", order.OrderPaperType.Name);
                if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.OrderPhotos)
                {
                    writer.WriteElementString("cropMode",
                        Constants.CropToFillModeName == order.CropMode ?
                        ExecutionEngine.Instance.Resource[Constants.CropToFillTextKey].ToString() :
                        ExecutionEngine.Instance.Resource[Constants.ResizeToFitTextKey].ToString());
                }
                else if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.PrintPhotos)
                {
                    writer.WriteElementString("cropMode", order.CropMode);
                }

                writer.WriteElementString("orderCost", order.GetTotalCost().ToString("c", CultureInfo.CurrentCulture));
                writer.WriteElementString("totalCost", (order.GetTotalCost() * (1 + Math.Max(0, ExecutionEngine.PriceManager.SalesTaxPercent.Value))).ToString("c", CultureInfo.CurrentCulture));
                writer.WriteEndElement();

                // User info
                writer.WriteStartElement("customer");
                writer.WriteElementString("name", order.UserName);
                writer.WriteElementString("phone", order.UserPhone);
                writer.WriteElementString("email", order.UserEmail);
                writer.WriteEndElement();

                // Photos info
                if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.OrderPhotos || ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.PrintPhotos)
                {
                    writer.WriteStartElement("formats");

                    foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                    {
                        writer.WriteStartElement("format");
                        writer.WriteAttributeString("name", format.Name);
                        writer.WriteAttributeString("count", order.GetItemCount(format).ToString(CultureInfo.InvariantCulture));
                        writer.WriteAttributeString("cost", order.GetCost(format).ToString("c", CultureInfo.CurrentCulture));
                        if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.PrintPhotos)
                        {
                            string printerName = ExecutionEngine.PriceManager.GetProduct(format.Name + order.OrderPaperType.Name + Constants.InstantKey).Printer;
                            writer.WriteAttributeString("printer", printerName);
                            writer.WriteAttributeString("width", format.Width.ToString(CultureInfo.InvariantCulture));
                            writer.WriteAttributeString("height", format.Height.ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            string channelName = ExecutionEngine.PriceManager.GetProduct(format.Name + order.OrderPaperType.Name).Channel;
                            if (!string.IsNullOrEmpty(channelName))
                                writer.WriteAttributeString("channel", channelName);
                        }

                        foreach (OrderItem item in order.OrderItems)
                        {
                            if (item.GetCount(format) > 0)
                            {
                                writer.WriteStartElement("photo");
                                writer.WriteAttributeString("file", item.OrderStoreFileName);
                                writer.WriteAttributeString("crop", item.GetCropString(format));
                                writer.WriteAttributeString("quantity", item.GetCount(format).ToString());
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.OrderPhotos && _order.Services.Count > 0)
                    {
                        writer.WriteStartElement("services");

                        foreach (Service service in _order.Services)
                        {
                            writer.WriteStartElement("service");
                            writer.WriteAttributeString("name", service.Name);
                            writer.WriteAttributeString("price", order.GetCost(service).ToString("c", CultureInfo.CurrentCulture));
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        private static string VerifyFileName(string destFolder, string fileName)
        {
            if (destFolder == null || fileName == null)
                ExecutionEngine.EventLogger.Write("OrderStore:VerifyFileName invalid argument");

            string destFileName = fileName;
            try
            {
                if (File.Exists(destFolder + "\\" + fileName))
                {
                    int index = 0;

                    do
                    {
                        index++;
                        var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                        var ext = Path.GetExtension(fileName);
                        destFileName = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", nameWithoutExt, index.ToString(CultureInfo.InvariantCulture), ext);
                    }
                    while (File.Exists(destFolder + "\\" + destFileName));
                }
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.Write(string.Format(CultureInfo.InvariantCulture, "OrderStore:VerifyFileName failed, destFolder = {0}, fileName = {1}", destFolder, fileName));
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
                throw;
            }

            return destFileName;
        }

        public static int GetOrderIdFromFile()
        {
            int orderId = 0;

            try
            {
                if (File.Exists(Config.LastOrderIdFile))
                {
                    using (TextReader reader = new StreamReader(Config.LastOrderIdFile))
                    {
                        orderId = int.Parse(reader.ReadLine(), CultureInfo.InvariantCulture);
                    }
                }
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
            }

            return orderId;
        }

        private void IncreaseOrderId()
        {
            int orderId = GetOrderIdFromFile();
            try
            {
                using (TextWriter writer = new StreamWriter(Config.LastOrderIdFile))
                {
                    writer.WriteLine(orderId + 1);
                }
            }
            catch (Exception e)
            {
                ExecutionEngine.EventLogger.WriteExceptionInfo(e);
            }
        }

        private void LockOrderFolder(string folderPath)
        {
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            dir.Attributes = FileAttributes.Hidden;
        }

        private void UnlockOrderFolder(string folderPath)
        {
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            dir.Attributes = FileAttributes.Normal;
        }
    }
}