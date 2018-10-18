// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Globalization;

namespace Aurigma.PhotoKiosk
{
    public class ProcessOrderStage : StageBase
    {
        #region [Constructors]

        public ProcessOrderStage()
            : base(Constants.ProcessStageName, ExecutionEngine.Config.InactivityTimeout.Value)
        {
            _confirmOrderScreen = new ConfirmOrderScreen(this);
            _contactInfoScreen = new ContactInfoScreen(this);
            _orderIdScreen = new OrderIdScreen(this);
            _burningScreen = new BurningScreen(this);
            _printingScreen = new PrintingScreen(this);
            _thankYouScreen = new ThankYouScreen(this);
            _thankYouCancelScreen = new ThankYouCancelScreen(this);

            LastVisitedPage = _confirmOrderScreen;

            _orderStoreManager = new FileOrderStorage();
            ExecutionEngine.EventLogger.Write("ProcessOrderStage created");
        }

        #endregion [Constructors]

        #region [Public overriden methods]

        public override void Activate(ExecutionEngine engine)
        {
            if (ExecutionEngine.Context.Contains(Constants.OrderContextName))
                _orderStoreManager.Order = (Order)ExecutionEngine.Context[Constants.OrderContextName];

            base.Activate(engine);

            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.ProcessOrder)
            {
                LastVisitedPage = _orderIdScreen;
                Engine.ExecuteCommand(new SwitchToScreenCommand(_orderIdScreen));
            }

            ExecutionEngine.EventLogger.Write("ProcessOrderStage:Activate");
        }

        public override void Reset()
        {
            this.LastVisitedPage = _confirmOrderScreen;
            _contactInfoScreen._enteredName.Text = string.Empty;
            _contactInfoScreen._enteredPhone.Text = string.Empty;
            _contactInfoScreen._enteredEmail.Text = string.Empty;
            _orderIdScreen._enteredId.Text = string.Empty;
            ExecutionEngine.EventLogger.Write("ProcessOrderStage:Reset");
        }

        #endregion [Public overriden methods]

        #region [Public methods and props]

        public void SwitchToStartStage()
        {
            ExecutionEngine.EventLogger.Write("ProcessOrderStage:SwitchToStartPage");
            Engine.ExecuteCommand(new ResetOrderDataCommand());
        }

        public void CompleteOrder()
        {
            DisableTimeout();

            ProgressDialog progressDialog;
            ExecutionEngine.EventLogger.Write("ProcessOrderStage:CompleteOrder");

            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
                progressDialog = new ProgressDialog((string)ExecutionEngine.Instance.Resource[Constants.MessagePreparingImageKey]);
            else
                progressDialog = new ProgressDialog((string)ExecutionEngine.Instance.Resource[Constants.MessageProgressOrderKey]);

            ExecutionEngine.EventLogger.Write("ProgressOrderStage:StartOrderProgress");

            _orderStoreManager.Add((IProgressCallback)progressDialog);
            bool result = false;
            DateTime showDialogTime = DateTime.Now.AddSeconds(Constants.ProgressDialogTimeout);
            while (DateTime.Now.Second < showDialogTime.Second)
            {
                if (progressDialog.IsComplete)
                {
                    result = true;
                    break;
                }
            }

            if (!result)
            {
                progressDialog.ShowDialog();
                result = progressDialog.DialogResult.Value;
            }

            if (result)
            {
                if (Engine.PrimaryAction == PrimaryActionType.BurnCd)
                {
                    if (ExecutionEngine.Config.CDBurningRequireConfirm.Value)
                        SwitchToThankYou();
                    else
                        SwitchToBurningScreen(null);
                }
                else if (Engine.PrimaryAction == PrimaryActionType.PrintPhotos)
                {
                    if (ExecutionEngine.Config.PhotoPrintingRequireConfirm.Value)
                        SwitchToThankYou();
                    else
                        SwitchToPrintingScreen(null);
                }
                else
                    SwitchToThankYou();
            }
            else
            {
                SwitchToThankYouCancel();
            }

            if (ExecutionEngine.Instance.IsBluetooth)
                FileOrderStorage.ClearBluetoothDirectory();
        }

        public void SwitchToContactInfo()
        {
            if (ExecutionEngine.Config.SkipContactInfo.Value)
            {
                this.CurrentOrder.OrderId = string.Format("{0}{1}", ExecutionEngine.Config.PhotoKioskId.Value, FileOrderStorage.GetOrderIdFromFile() + 1);
                this.CompleteOrder();
            }
            else
            {
                ExecutionEngine.EventLogger.Write("ProcessOrderStage:SwitchToContactInfo");
                LastVisitedPage = _contactInfoScreen;
                Engine.ExecuteCommand(new SwitchToScreenCommand(_contactInfoScreen));
            }
        }

        public void SwitchToOrderId()
        {
            ExecutionEngine.EventLogger.Write("ProcessOrderStage:SwitchToOrderId");
            LastVisitedPage = _orderIdScreen;
            Engine.ExecuteCommand(new SwitchToScreenCommand(_orderIdScreen));
        }

        public void SwitchToThankYou()
        {
            ExecutionEngine.EventLogger.Write("ProcessOrderStage:SwitchToThankYou");
            LastVisitedPage = _thankYouScreen;

            if (ExecutionEngine.Config.EnableReceiptPrinter.Value && ExecutionEngine.Instance.PrimaryAction != PrimaryActionType.ProcessOrder)
            {
                try
                {
                    var receiptData = new ReceiptData();
                    receiptData.Id = CurrentOrder.OrderId;
                    receiptData.UserName = CurrentOrder.UserName;
                    receiptData.UserPhone = CurrentOrder.UserPhone;
                    receiptData.OrderPhotos = true;
                    receiptData.BurnCd = false;

                    if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
                    {
                        receiptData.Service = ExecutionEngine.Instance.Resource[Constants.OrderInfoControlCdBurningTextKey].ToString();
                        receiptData.OrderPhotos = false;
                        receiptData.BurnCd = true;
                    }

                    receiptData.CropMode = Constants.CropToFillModeName == CurrentOrder.CropMode ?
                        ExecutionEngine.Instance.Resource[Constants.CropToFillTextKey].ToString() :
                        ExecutionEngine.Instance.Resource[Constants.ResizeToFitTextKey].ToString();
                    receiptData.PaperType = CurrentOrder.OrderPaperType.Name;
                    receiptData.OrderDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                    receiptData.PhotosCount = CurrentOrder.OrderItems.Count;
                    receiptData.PrintsCount = CurrentOrder.GetItemCount();
                    receiptData.OrderCost = CurrentOrder.GetTotalCost().ToString("c", NumberFormatInfo.CurrentInfo);
                    if (ExecutionEngine.PriceManager.SalesTaxPercent.Value > 0)
                    {
                        string taxString = string.Format("{0} ({1}%)", (CurrentOrder.GetTotalCost() * ExecutionEngine.PriceManager.SalesTaxPercent.Value).ToString("c", CultureInfo.CurrentCulture), ExecutionEngine.PriceManager.SalesTaxPercent.Value * 100);
                        if (!string.IsNullOrEmpty(ExecutionEngine.PriceManager.SalesTaxComment.Value))
                            taxString += " " + ExecutionEngine.PriceManager.SalesTaxComment.Value;
                        receiptData.SalesTax = taxString;
                        receiptData.TotalCost = (CurrentOrder.GetTotalCost() * (1 + ExecutionEngine.PriceManager.SalesTaxPercent.Value)).ToString("c", NumberFormatInfo.CurrentInfo);
                    }

                    foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                    {
                        if (CurrentOrder.GetItemCount(format) > 0)
                            receiptData.Formats.Add(new FormatInfo(format.Name, CurrentOrder.GetItemCount(format)));
                    }

                    int maxPhotosCount = Math.Min(CurrentOrder.OrderItems.Count, ExecutionEngine.Config.PhotosInReceipt.Value);
                    for (int i = 0; i < maxPhotosCount; i++)
                    {
                        foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                        {
                            if (CurrentOrder.OrderItems[i].GetCount(format) > 0)
                                receiptData.Photos.Add(new PhotoInfo(CurrentOrder.OrderItems[i].OrderStoreFileName, CurrentOrder.OrderItems[i].GetCount(format), format.Name));
                        }
                    }
                    receiptData.MorePhotos = (maxPhotosCount < CurrentOrder.OrderItems.Count);

                    if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.OrderPhotos)
                    {
                        foreach (Service service in CurrentOrder.Services)
                        {
                            string servicePrice;

                            if (service.IsPriceFixed)
                            {
                                servicePrice = service.GetPriceString();
                            }
                            else
                            {
                                servicePrice = (CurrentOrder.GetTotalCost() * service.Price / 100.0f).ToString("c", CultureInfo.CurrentCulture) + " (" + service.GetPriceString() + ")";
                            }

                            receiptData.Services.Add(new ServiceInfo(service.Name, servicePrice));
                        }
                    }

                    var receipt = new Receipt(receiptData, ExecutionEngine.Config.ReceiptTemplateFile.Value);
                    receipt.Print(ExecutionEngine.Config.ReceiptPrinterName.Value);
                }
                catch (System.Exception e)
                {
                    ExecutionEngine.ErrorLogger.WriteExceptionInfo(e);
                    MessageDialog.Show(StringResources.GetString("MessageReceiptPrinterError"));
                }
            }

            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.OrderPhotos)
                ExecutionEngine.Instance.OrderManager.AddOrder(_orderStoreManager.CurrentOrderPath);

            EnableTimeout();
            Engine.ExecuteCommand(new SwitchToScreenCommand(_thankYouScreen));
        }

        public void SwitchToThankYouCancel()
        {
            ExecutionEngine.EventLogger.Write("ProcessOrderStage:SwitchToThankYouCancel");
            LastVisitedPage = _thankYouCancelScreen;
            ExecutionEngine.RunCancelOrderProcess();
            EnableTimeout();
            Engine.ExecuteCommand(new SwitchToScreenCommand(_thankYouCancelScreen));
        }

        public void SwitchToBurningScreen(string delayedOrderFolder)
        {
            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.ProcessOrder && !string.IsNullOrEmpty(delayedOrderFolder))
                _delayedOrderFolder = delayedOrderFolder;

            LastVisitedPage = _burningScreen;
            Engine.ExecuteCommand(new SwitchToScreenCommand(_burningScreen));
        }

        public void SwitchToPrintingScreen(string delayedOrderFolder)
        {
            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.ProcessOrder && !string.IsNullOrEmpty(delayedOrderFolder))
                _delayedOrderFolder = delayedOrderFolder;

            LastVisitedPage = _printingScreen;
            Engine.ExecuteCommand(new SwitchToScreenCommand(_printingScreen));
        }

        public void SwitchToConfirmOrder()
        {
            ExecutionEngine.EventLogger.Write("ProcessOrderStage:SwitchToConfirmOrder");
            LastVisitedPage = _confirmOrderScreen;
            Engine.ExecuteCommand(new SwitchToScreenCommand(_confirmOrderScreen));
        }

        public void SwitchToSelectStage()
        {
            ExecutionEngine.EventLogger.Write("ProcessOrderStage:SwitchToSelectStage");
            Engine.ExecuteCommand(new SwitchToStageCommand(Constants.SelectStageName));
        }

        public Order CurrentOrder
        {
            get { return _orderStoreManager.Order; }
        }

        public FileOrderStorage OrderStorage
        {
            get { return _orderStoreManager; }
        }

        public string DelayedOrderFolder
        {
            get { return _delayedOrderFolder; }
        }

        #endregion [Public methods and props]

        #region [Variables]

        private ConfirmOrderScreen _confirmOrderScreen;
        private ContactInfoScreen _contactInfoScreen;
        private OrderIdScreen _orderIdScreen;
        private BurningScreen _burningScreen;
        private PrintingScreen _printingScreen;
        private ThankYouScreen _thankYouScreen;
        private ThankYouCancelScreen _thankYouCancelScreen;
        private FileOrderStorage _orderStoreManager;
        private string _delayedOrderFolder;

        #endregion [Variables]
    }
}