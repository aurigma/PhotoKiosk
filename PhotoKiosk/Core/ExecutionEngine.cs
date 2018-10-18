// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Aurigma.PhotoKiosk
{
    public class ExecutionEngine : IDisposable
    {
        #region [Public static]

        public static DataContext Context
        {
            get { return _dataContext; }
        }

        public static Config Config
        {
            get { return _config; }
        }

        public static PriceManager PriceManager
        {
            get { return _priceManager; }
        }

        public static CropManager Crops
        {
            get { return _crops; }
        }

        public static LogBase EventLogger
        {
            get { return _eventLogger; }
        }

        public static LogBase ErrorLogger
        {
            get { return _errorLogger; }
        }

        public static ExecutionEngine Instance
        {
            get { return _lastInstance; }
        }

        #endregion [Public static]

        #region [Private static]

        private static Config _config;
        private static PriceManager _priceManager;

        private static DataContext _dataContext;
        private static CropManager _crops;
        private static LogBase _eventLogger;
        private static LogBase _errorLogger;
        private static ExecutionEngine _lastInstance;

        #endregion [Private static]

        #region [Constants]

        private const string MutexName = "PhotoKiosk_7_X_X_Mutex";

        #endregion [Constants]

        #region [Construction / destruction]

        public ExecutionEngine(Frame frame, MainWindow mainWindow)
        {
            _lastInstance = this;
            _mainWindow = mainWindow;
            _isBluetooth = false;
            _showingFrame = frame;
            _resource = mainWindow.Resources;
            _orderManager = new OrderManagerAddon();

            LoadDefaultResources();

            try
            {
                if (Mutex.OpenExisting(MutexName) != null)
                {
                    ShowMessageAndQuit(StringResources.GetString("MessageOnlyOneInstanceCanBeRun"));
                    return;
                }
            }
            catch (WaitHandleCannotBeOpenedException) { }    // no need to handle

            _namedMutex = new Mutex(false, MutexName);

            _errorLogger = new WindowsAppLog(true);
            _errorLogger.Enabled = true;

            try
            {
                _config = new Config(true);
            }
            catch (Exception e)
            {
                _errorLogger.WriteExceptionInfo(e);
                ShowMessageAndQuit(StringResources.GetString("MessageConfigReadingError"));
                return;
            }

            try
            {
                _priceManager = new PriceManager(true, _config.PriceFile.Value);
            }
            catch (Exception e)
            {
                _errorLogger.WriteExceptionInfo(e);
                ShowMessageAndQuit(StringResources.GetString("MessagePriceReadingError"));
                return;
            }

            try
            {
                _crops = new CropManager(true, _config.CropFile.Value);
            }
            catch (Exception e)
            {
                _errorLogger.WriteExceptionInfo(e);
                ShowMessageAndQuit(StringResources.GetString("MessageCropsReadingError"));
                return;
            }

            _eventLogger = new FileLog(Config.EventLogFile, false);
            _eventLogger.Enabled = _config.EnableEventLogging.Value;

            _dataContext = new DataContext();
            _dataContext[Constants.OrderContextName] = new Order();
            _lastUserActionTime = System.DateTime.Now;

            _eventLogger.Write(string.Format(CultureInfo.InvariantCulture, "ExecutionEngine:Load headers and backgrounds"));
            _headerDictionary = new Dictionary<string, BitmapImage>();
            _backgroundDictionary = new Dictionary<string, ImageBrush>();

            // Add main banner to resources
            try
            {
                _resource.Add(Constants.ImageBannerKey, BitmapFrame.Create(new Uri(_config.MainBanner.Value)));
            }
            catch (Exception e)
            {
                _eventLogger.Write(string.Format(StringResources.GetString("MessageImageLoadError"), _config.MainBanner.Value, e.Message));
                _resource.Add(Constants.ImageBannerKey, null);
            }

            LoadScreenSettings(_config.Screens);

            LoadCustomResources();

            _stages = new List<StageBase>();
            _stages.Add(new WelcomeStage());
            _stages.Add(new SelectStage());
            _stages.Add(new ImageEditorStage());
            _stages.Add(new ProcessOrderStage());
            _stages.Add(new FindingPhotosStage());

            // Clear bluetooth folder
            FileOrderStorage.ClearBluetoothDirectory();

            NativeMethods.SetErrorMode(NativeMethods.SEM_FAILCRITICALERRORS);
        }

        ~ExecutionEngine()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            try
            {
                Dispose(true);
            }
            finally
            {
                System.GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                _namedMutex.Close();
            }
        }

        private void CheckDisposedState()
        {
            if (_disposed)
                throw new ObjectDisposedException("ExecutionEngine");
        }

        #endregion [Construction / destruction]

        #region [Public methods and props]

        public ResourceDictionary Resource
        {
            get
            {
                CheckDisposedState();
                return _resource;
            }
        }

        public bool IsBluetooth
        {
            get { return _isBluetooth; }
            set { _isBluetooth = value; }
        }

        public bool IsShowFolders
        {
            get { return _isShowFolders; }
            set { _isShowFolders = value; }
        }

        public PrimaryActionType PrimaryAction
        {
            get { return _primaryAction; }
            set { _primaryAction = value; }
        }

        public IList<PaperFormat> PaperFormats
        {
            get
            {
                if (_primaryAction == PrimaryActionType.OrderPhotos)
                    return _priceManager.MinilabPaperFormats;
                else if (_primaryAction == PrimaryActionType.PrintPhotos)
                    return _priceManager.InstantPaperFormats;
                else
                    return _priceManager.PaperFormats;
            }
        }

        public IList<PaperType> PaperTypes
        {
            get
            {
                if (_primaryAction == PrimaryActionType.OrderPhotos)
                    return _priceManager.MinilabPaperTypes;
                else if (_primaryAction == PrimaryActionType.PrintPhotos)
                    return _priceManager.InstantPaperTypes;
                else
                    return _priceManager.PaperTypes;
            }
        }

        public string Instant
        {
            get { return _primaryAction == PrimaryActionType.PrintPhotos ? Constants.InstantKey : ""; }
        }

        public TimeSpan UserInactionTime
        {
            get
            {
                CheckDisposedState();
                return new TimeSpan(DateTime.Now.Ticks - _lastUserActionTime.Ticks);
            }
        }

        public Canvas PhotosCanvas
        {
            get { return _mainWindow._canvasForPhotos; }
        }

        public PaperFormat SelectedPaperFormat
        {
            get { return _selectedPaperFormat; }
            set { _selectedPaperFormat = value; }
        }

        public OrderManagerAddon OrderManager
        {
            get { return _orderManager; }
        }

        public void RegisterUserAction()
        {
            CheckDisposedState();
            _lastUserActionTime = DateTime.Now;
        }

        public void ExecuteCommand(EngineCommandBase command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            CheckDisposedState();

            switch (command.Name)
            {
                case Constants.SwitchToStage:
                    {
                        SwitchToStageCommand castedCommand = (SwitchToStageCommand)command;
                        SwitchToStage(castedCommand.StageName);
                        break;
                    }
                case Constants.SwitchToScreen:
                    {
                        SwitchToScreenCommand castedCommand = (SwitchToScreenCommand)command;

                        if (castedCommand.Page is WelcomeScreen || castedCommand.Page is ThankYouScreen ||
                                castedCommand.Page is ThankYouCancelScreen || castedCommand.Page is BurningScreen ||
                                castedCommand.Page is PrintingScreen || (castedCommand.Page is OrderIdScreen && PrimaryAction == PrimaryActionType.ProcessOrder))
                            _mainWindow.SetCancelButtonVisibility(Visibility.Collapsed);
                        else
                            _mainWindow.SetCancelButtonVisibility(Visibility.Visible);

                        try
                        {
                            _mainWindow._headerImage.Source = _headerDictionary[castedCommand.Page.Title];
                        }
                        catch
                        {
                            _mainWindow._headerImage.Source = null;
                        }

                        try
                        {
                            _mainWindow._backgroundImage.Background = _backgroundDictionary[castedCommand.Page.Title];
                        }
                        catch
                        {
                            _mainWindow._backgroundImage.Background = null;
                        }

                        SwitchToScreen(castedCommand.Page);
                        break;
                    }
                case Constants.ResetOrderData:
                    {
                        ResetOrderData();
                        break;
                    }
            }
        }

        public void RegisterModalWidow(Window modalWindow)
        {
            CheckDisposedState();
            if (_modalWindows == null)
                _modalWindows = new List<System.Windows.Window>();

            lock (_modalWindows)
            {
                _modalWindows.Add(modalWindow);
            }
        }

        public void SetBackgroundWindowVisibility(bool toShow)
        {
            CheckDisposedState();
            if (_mainWindow != null)
                _mainWindow.BackgroundRect.Visibility = toShow ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion [Public methods and props]

        #region [Public static methods]

        public static void RunStartOrderProcess()
        {
            _eventLogger.Write("ExecutionEngine:RunStartOrderProcess");
            RunProcess(_config.OnStartCallback.Value);
        }

        public static void RunCancelOrderProcess()
        {
            _eventLogger.Write("ExecutionEngine:RunCancelOrderProcess");
            RunProcess(_config.OnCancelCallback.Value);
        }

        public static void RunCompleteOrderProcess()
        {
            _eventLogger.Write("ExecutionEngine:RunCompleteOrderProcess");
            RunProcess(_config.OnCompleteCallback.Value);
        }

        private static void RunProcess(string command)
        {
            if (command != "")
            {
                string[] parts = command.Split('\"');
                string filename = "", arguments = "";

                for (int i = 0; i < parts.Length; i++)
                {
                    string part = parts[i].Trim();
                    if (filename == "")
                    {
                        if (part != "")
                            filename = part;
                    }
                    else
                    {
                        arguments += part + " ";
                    }
                }

                arguments = arguments.Trim();
                try
                {
                    if (arguments != "")
                        Process.Start(filename, arguments);
                    else
                        Process.Start(filename);
                }
                catch (Exception e)
                {
                    _errorLogger.WriteExceptionInfo(e);
                    MessageDialog.Show(string.Format(StringResources.GetString("MessageRunProcessError"), command));
                }
            }
        }

        #endregion [Public static methods]

        #region [Private methods and props]

        private void ShowMessageAndQuit(string message)
        {
            MessageDialog.Show(message);
            _mainWindow.Visibility = Visibility.Hidden;
            _mainWindow.Close();
        }

        private bool SwitchToStage(string stageName)
        {
            if (_stages == null)
                return false;

            StageBase newStage = null;
            foreach (StageBase stage in _stages)
            {
                if (stage.Name.CompareTo(stageName) == 0)
                {
                    newStage = stage;
                    break;
                }
            }

            if (newStage == null)
                return false;

            if (_currentActiveStage != null)
                _currentActiveStage.Deactivate();

            _currentActiveStage = newStage;
            newStage.Activate(this);

            _eventLogger.Write(string.Format(CultureInfo.InvariantCulture, "ExecutionEngine:SwitchToStage {0}", stageName));

            return true;
        }

        private void SwitchToScreen(Page page)
        {
            _eventLogger.Write(string.Format(CultureInfo.InvariantCulture, "ExecutionEngine:SwitchToPage title = {0}", page.Title));
            if (!page.Resources.MergedDictionaries.Contains(_resource))
                page.Resources.MergedDictionaries.Add(_resource);

            _showingFrame.Content = page;
        }

        private void ResetOrderData()
        {
            _eventLogger.Write("ExecutionEngine:ResetOrderData");

            foreach (StageBase stage in _stages)
            {
                stage.Reset();
            }

            PhotoItem.Cache.Clear(false);

            Order currentOrder = (Order)ExecutionEngine.Context[Constants.OrderContextName];
            currentOrder.Reset();

            if (_modalWindows != null)
            {
                lock (_modalWindows)
                {
                    foreach (Window window in _modalWindows)
                    {
                        window.Close();
                    }

                    _modalWindows.Clear();
                }
            }

            SwitchToStage(Constants.WelcomeStageName);
        }

        private void LoadScreenSettings(List<ScreenSetting> screens)
        {
            foreach (ScreenSetting screen in screens)
            {
                try
                {
                    _backgroundDictionary.Add(screen.Key, new ImageBrush(BitmapFrame.Create(new Uri(screen.Background.Value))));
                }
                catch (Exception e)
                {
                    _eventLogger.Write(string.Format(StringResources.GetString("MessageImageLoadError"), screen.Background.Value, e.Message));
                    _backgroundDictionary.Add(screen.Key, null);
                }

                try
                {
                    _headerDictionary.Add(screen.Key, new BitmapImage(new Uri(screen.Header.Value)));
                }
                catch (Exception e)
                {
                    _eventLogger.Write(string.Format(StringResources.GetString("MessageImageLoadError"), screen.Header.Value, e.Message));
                    _headerDictionary.Add(screen.Key, null);
                }
            }
        }

        private void LoadCustomResources()
        {
            try
            {
                if (!File.Exists(_config.LocalizationFile.Value))
                    _config.RestoreDefaultResourcesFile();

                var reader = XmlReader.Create(_config.LocalizationFile.Value);
                reader.ReadStartElement("resources");

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (!reader.IsEmptyElement)
                        {
                            if (Constants.ResourceTextTableName.ToLower().CompareTo(reader.Name.ToLower()) == 0)
                            {
                                string key = null;
                                object value = null;

                                if (reader.Read() && reader.IsStartElement() && Constants.ResourceKeyName.CompareTo(reader.Name.ToLower()) == 0)
                                    key = reader.ReadString();

                                if (reader.Read() && reader.IsStartElement() && Constants.ResourceValueName.CompareTo(reader.Name.ToLower()) == 0)
                                    value = reader.ReadString();

                                if (_resource.Contains(key))
                                    _resource[key] = value;
                                else
                                    _resource.Add(key, value);

                                _eventLogger.Write(string.Format(CultureInfo.InvariantCulture, "LoadCustomResources: resource added. Key = {0}, Value = {1}", key, value));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _errorLogger.WriteExceptionInfo(e);
                ShowMessageAndQuit(StringResources.GetString("MessageResourceReadingError"));
                return;
            }
        }

        private void LoadDefaultResources()
        {
            // Images
            _resource.Add(Constants.ImagePleaseWaitKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/PleaseWait.png")));
            _resource.Add(Constants.ImageMagnifierKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/Magnifier.png")));
            _resource.Add(Constants.ImageEditKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/editor.png")));
            _resource.Add(Constants.ImageBigCheckBoxKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/GreenCheckBox.png")));
            _resource.Add(Constants.ImageRemoveBigButtonKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/Remove_big.png")));
            _resource.Add(Constants.ImageAllFormatsCheckBoxKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/AllFormatsCheckBox.png")));
            _resource.Add(Constants.ImageGreenCropKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/GreenCrop.png")));
            _resource.Add(Constants.ImageUndoKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/undo.png")));
            _resource.Add(Constants.ImageRedoKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/redo.png")));
            _resource.Add(Constants.ImageRotateLeftKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/left.png")));
            _resource.Add(Constants.ImageRotateRightKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/right.png")));
            _resource.Add(Constants.ImageFlipVerticalKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/arrovs_v.png")));
            _resource.Add(Constants.ImageFlipHorizontalKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/arrovs_h.png")));
            _resource.Add(Constants.ImageSaveKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/save.png")));
            _resource.Add(Constants.ImageSaveAsNewKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/saveasnew.png")));
            _resource.Add(Constants.ImageColorCorrectionKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/ColorCorrection.png")));
            _resource.Add(Constants.ImageAutoLevelsKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/AutoLevels.png")));
            _resource.Add(Constants.ImageEffectsKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/effects.png")));
            _resource.Add(Constants.ImageRedEyeRemovalKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/redEye.png")));
            _resource.Add(Constants.ImageBlackAndWhiteKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/black-white.png")));
            _resource.Add(Constants.ImageSepiaKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/sepia.png")));
            _resource.Add(Constants.ImageRedEyeNextKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/next.png")));
            _resource.Add(Constants.ImageRedEyeManualKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/redEye-hand.png")));
            _resource.Add(Constants.ImageRedEyeRemoveKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/redEye-remove.png")));
            _resource.Add(Constants.ImageBluetoothLogoKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/bluetoothlogo.png")));
            _resource.Add(Constants.ImageBluetoothKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/Bluetooth.png")));
            _resource.Add(Constants.ImageStorageKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/Storage.png")));
            _resource.Add(Constants.ImageArrowPreviousKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/arrow-180.png")));
            _resource.Add(Constants.ImageArrowNextKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/arrow.png")));
            _resource.Add(Constants.ImageRemoveSmallButtonKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/cross.png")));
            _resource.Add(Constants.ImageSetCropFrameButtonKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/scissors-blue.png")));
            _resource.Add(Constants.ImageCheckButtonKey, BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/check.png")));

            // Common
            _resource.Add(Constants.CancelTextKey, Constants.CancelTextDefValue);
            _resource.Add(Constants.BackButtonTextKey, Constants.BackButtonTextDefValue);
            _resource.Add(Constants.NextButtonTextKey, Constants.NextButtonTextDefValue);
            _resource.Add(Constants.MessageOkTextKey, Constants.MessageOkTextDefValue);
            _resource.Add(Constants.MessageCancelTextKey, Constants.MessageCancelTextDefValue);
            _resource.Add(Constants.MessageYesTextKey, Constants.MessageYesTextDefValue);
            _resource.Add(Constants.MessageNoTextKey, Constants.MessageNoTextDefValue);
            _resource.Add(Constants.MessageSureWantToCancelOrderKey, Constants.MessageSureWantToCancelOrderDefValue);
            _resource.Add(Constants.MessageBeforeResetKey, Constants.MessageBeforeResetDefValue);
            _resource.Add(Constants.BackToOrderButtonKey, Constants.BackToOrderButtonDefValue);

            // Welcome Screen
            _resource.Add(Constants.WelcomeStageButtonNextTextKey, Constants.WelcomeStageButtonNextTextDefValue);
            _resource.Add(Constants.WelcomeStageButtonOrderPhotosTextKey, Constants.WelcomeStageButtonOrderPhotosTextDefValue);
            _resource.Add(Constants.WelcomeStageButtonPrintPhotosTextKey, Constants.WelcomeStageButtonPrintPhotosTextDefValue);
            _resource.Add(Constants.WelcomeStageButtonBurnPhotosTextKey, Constants.WelcomeStageButtonBurnPhotosTextDefValue);
            _resource.Add(Constants.WelcomeStageButtonProcessOrderTextKey, Constants.WelcomeStageButtonProcessOrderTextDefValue);
            _resource.Add(Constants.WelcomeTextKey, Constants.WelcomeTextDefValue);
            _resource.Add(Constants.WelcomeNoticeBluetoothDisabledTextKey, Constants.WelcomeNoticeBluetoothDisabledTextDefValue);
            _resource.Add(Constants.WelcomeNoticeBluetoothEnabledTextKey, Constants.WelcomeNoticeBluetoothEnabledTextDefValue);
            _resource.Add(Constants.WelcomeNoticeBluetoothDisabledMultiActionTextKey, Constants.WelcomeNoticeBluetoothDisabledMultiActionTextDefValue);
            _resource.Add(Constants.WelcomeNoticeBluetoothEnabledMultiActionTextKey, Constants.WelcomeNoticeBluetoothEnabledMultiActionTextDefValue);

            // Choose Upload Way Screen
            _resource.Add(Constants.SelectDeviceTextKey, Constants.SelectDeviceTextDefValue);
            _resource.Add(Constants.SelectDeviceNoticeTextKey, Constants.SelectDeviceNoticeTextDefValue);
            _resource.Add(Constants.MediaStorageTitleTextKey, Constants.MediaStorageTitleTextDefValue);
            _resource.Add(Constants.MediaStorageDescriptionTextKey, Constants.MediaStorageDescriptionTextDefValue);
            _resource.Add(Constants.BluetoothTitleTextKey, Constants.BluetoothTitleTextDefValue);
            _resource.Add(Constants.BluetoothDescriptionTextKey, Constants.BluetoothDescriptionTextDefValue);

            // Select Folders Screen
            _resource.Add(Constants.SelectFoldersTextKey, Constants.SelectFoldersTextDefValue);
            _resource.Add(Constants.SelectFoldersNoticeTextKey, Constants.SelectFoldersNoticeTextDefValue);
            _resource.Add(Constants.SelectedFoldersLabelTextKey, Constants.SelectedFoldersLabelTextDefValue);
            _resource.Add(Constants.PhotosLabelTextKey, Constants.PhotosLabelTextDefValue);
            _resource.Add(Constants.RemovableDriveTypeTextKey, Constants.RemovableDriveTypeTextDefValue);
            _resource.Add(Constants.CDRomDriveTypeTextKey, Constants.CDRomDriveTypeTextDefValue);
            _resource.Add(Constants.MessageNoFilesFoundKey, Constants.MessageNoFilesFoundDefValue);

            // Uploading Photos Screen
            _resource.Add(Constants.LoadPhotosTextKey, Constants.LoadPhotosTextDefValue);
            _resource.Add(Constants.ReadyToReceivePhotosTextKey, Constants.ReadyToReceivePhotosTextDefValue);
            _resource.Add(Constants.ReceivedPhotosCountTextKey, Constants.ReceivedPhotosCountTextDefValue);
            _resource.Add(Constants.HowToLoadPhotosUsingBluetoothTextKey, Constants.HowToLoadPhotosUsingBluetoothTextDefValue);

            // Finding Photos Screen
            _resource.Add(Constants.FindingPhotosStepTextKey, Constants.FindingPhotosStepTextDefValue);
            _resource.Add(Constants.FindingPhotosNoticeTextKey, Constants.FindingPhotosNoticeTextDefValue);

            // ThumbnailList Control
            _resource.Add(Constants.TabAllTextKey, Constants.TabAllTextDefValue);
            _resource.Add(Constants.TabCheckedTextKey, Constants.TabCheckedTextDefValue);
            _resource.Add(Constants.TabUncheckedTextKey, Constants.TabuncheckedTextDefValue);
            _resource.Add(Constants.FindingPhotosPagesTextKey, Constants.FindingPhotosPagesTextDefValue);
            _resource.Add(Constants.PagesInfoKey, Constants.PagesInfoDefValue);
            _resource.Add(Constants.ThumbnailCheckTextKey, Constants.ThumbnailCheckTextDefValue);
            _resource.Add(Constants.ThumbnailPreviewTextKey, Constants.ThumbnailPreviewTextDefValue);
            _resource.Add(Constants.ThumbnailPrintsTextKey, Constants.ThumbnailPrintsDefValue);
            _resource.Add(Constants.ThumbnailRemoveTextKey, Constants.ThumbnailRemoveTextDefValue);
            _resource.Add(Constants.ThumbnailSetFrameTextKey, Constants.ThumbnailSetFrameTextDefValue);
            _resource.Add(Constants.ThumbnailEditTextKey, Constants.ThumbnailEditTextDefValue);
            _resource.Add(Constants.ThumbnailLowQualityTextKey, Constants.ThumbnailLowQualityTextDefValue);
            _resource.Add(Constants.MessageLoadingImageErrorKey, Constants.MessageLoadingImageErrorDefValue);

            // Select Photos Screen
            _resource.Add(Constants.SelectStepTextKey, Constants.SelectStepTextDefValue);
            _resource.Add(Constants.SelectStepCdBurningTextKey, Constants.SelectStepCdBurningTextDefValue);
            _resource.Add(Constants.SelectStepNoticeTextKey, Constants.SelectStepNoticeTextDefValue);
            _resource.Add(Constants.SelectStepNoticeCdBurningTextKey, Constants.SelectStepNoticeCdBurningTextDefValue);
            _resource.Add(Constants.FindingPhotosAllTextKey, Constants.FindingPhotosAllTextDefValue);
            _resource.Add(Constants.SelectStepPhotosLabelTextKey, Constants.SelectStepPhotosLabelTextDefValue);
            _resource.Add(Constants.SelectStepSizeLabelTextKey, Constants.SelectStepSizeLabelTextDefValue);
            _resource.Add(Constants.SelectStepKBLabelTextKey, Constants.SelectStepKBLabelTextDefValue);
            _resource.Add(Constants.SelectStepMBLabelTextKey, Constants.SelectStepMBLabelTextDefValue);
            _resource.Add(Constants.SelectStepGBLabelTextKey, Constants.SelectStepGBLabelTextDefValue);
            _resource.Add(Constants.MessageCheckSomePhotosKey, Constants.MessageCheckSomePhotosDefValue);
            _resource.Add(Constants.MessageMinimumCostKey, Constants.MessageMinimumCostDefValue);

            // Order Forming Screen
            _resource.Add(Constants.OrderFormingStepTextKey, Constants.OrderFormingStepTextDefValue);
            _resource.Add(Constants.OrderFomingNoticeTextKey, Constants.OrderFomingNoticeTextDefValue);
            _resource.Add(Constants.AllOrderTextKey, Constants.AllOrderTextDefValue);
            _resource.Add(Constants.OrderInfoControlFormatTextKey, Constants.OrderInfoControlFormatDefValue);
            _resource.Add(Constants.OrderInfoControlCountTextKey, Constants.OrderInfoControlCountDefValue);
            _resource.Add(Constants.OrderInfoControlCostTextKey, Constants.OrderInfoControlCostTextDefValue);
            _resource.Add(Constants.OrderInfoControlDiscountCostTextKey, Constants.OrderInfoControlDiscountCostTextDefValue);
            _resource.Add(Constants.OrderInfoControlServiceTextKey, Constants.OrderInfoControlServiceTextDefValue);
            _resource.Add(Constants.OrderInfoControlTotalTextKey, Constants.OrderInfoControlTotalTextDefValue);
            _resource.Add(Constants.OrderInfoControlOrderTotalTextKey, Constants.OrderInfoControlOrderTotalTextDefValue);
            _resource.Add(Constants.OrderInfoControlSalesTaxTextKey, Constants.OrderInfoControlSalesTaxTextDefValue);
            _resource.Add(Constants.OrderInfoControlTotalPaymentKey, Constants.OrderInfoControlTotalPaymentDefValue);
            _resource.Add(Constants.OrderInfoControlPriceTextKey, Constants.OrderInfoControlPriceTextDefValue);
            _resource.Add(Constants.OrderInfoControlCdBurningTextKey, Constants.OrderInfoControlCdBurningTextDefValue);
            _resource.Add(Constants.SetForAllButtonUpTextKey, Constants.SetForAllButtonUpTextDefValue);
            _resource.Add(Constants.SetForAllButtonDownTextKey, Constants.SetForAllButtonDownTextDefValue);
            _resource.Add(Constants.ChoosePaperFormatsTextKey, Constants.ChoosePaperFormatsTextDefValue);
            _resource.Add(Constants.ChoosePaperFormatsWarningTextKey, Constants.ChoosePaperFormatsWarningDefValue);
            _resource.Add(Constants.OrderParamsHeaderTextKey, Constants.OrderParamsHeaderTextDefValue);
            _resource.Add(Constants.OrderParamsPaperTypeTextKey, Constants.OrderParamsPaperTypeTextDefValue);
            _resource.Add(Constants.OrderParamsCropModeTextKey, Constants.OrderParamsCropModeTextDefValue);
            _resource.Add(Constants.ChangeOrderParamsButtonTextKey, Constants.ChangeOrderParamsButtonTextDefValue);
            _resource.Add(Constants.SelectPaperTypeTextKey, Constants.SelectPaperTypeTextDefValue);
            _resource.Add(Constants.SelectCropModeTextKey, Constants.SelectCropModeTextDefValue);
            _resource.Add(Constants.CropToFillTextKey, Constants.CropToFillTextDefValue);
            _resource.Add(Constants.CropToFillTextDescriptionKey, Constants.CropToFillTextDescriptionDefValue);
            _resource.Add(Constants.ResizeToFitTextKey, Constants.ResizeToFitTextDefValue);
            _resource.Add(Constants.ResizeToFitTextDescriptionKey, Constants.ResizeToFitTextDescriptionDefValue);

            // Image Viewer/Editor
            _resource.Add(Constants.ImageEditorStageViewKey, Constants.ImageEditorStageViewDefValue);
            _resource.Add(Constants.ImageEditorStageEditTextKey, Constants.ImageEditorStageEditTextDefValue);
            _resource.Add(Constants.ImageEditorStageSetCropTextKey, Constants.ImageEditorStageSetCropTextDefValue);
            _resource.Add(Constants.ImageEditorReturnTextKey, Constants.ImageEditorReturnTextDefValue);
            _resource.Add(Constants.ImageEditorPreviousPhotoTextKey, Constants.ImageEditorPreviousPhotoTextDefValue);
            _resource.Add(Constants.ImageEditorNextPhotoTextKey, Constants.ImageEditorNextPhotoTextDefValue);
            _resource.Add(Constants.ImageEditorUndoTextKey, Constants.ImageEditorUndoTextDefValue);
            _resource.Add(Constants.ImageEditorRedoTextKey, Constants.ImageEditorRedoTextDefValue);
            _resource.Add(Constants.ImageEditorRotateLeftTextKey, Constants.ImageEditorRotateLeftTextDefValue);
            _resource.Add(Constants.ImageEditorRotateRightTextKey, Constants.ImageEditorRotateRightTextDefValue);
            _resource.Add(Constants.ImageEditorFlipTextKey, Constants.ImageEditorFlipTextDefValue);
            _resource.Add(Constants.ImageEditorCropTextKey, Constants.ImageEditorCropTextDefValue);
            _resource.Add(Constants.ImageEditorCropOptionTextKey, Constants.ImageEditorCropOptionTextDefValue);
            _resource.Add(Constants.ImageEditorColorCorrectionTextKey, Constants.ImageEditorColorCorrectionTextDefValue);
            _resource.Add(Constants.ImageEditorEffectsTextKey, Constants.ImageEditorEffectsTextDefValue);
            _resource.Add(Constants.ImageEditorRedEyeTextKey, Constants.ImageEditorRedEyeTextDefValue);
            _resource.Add(Constants.ImageEditorSaveTextKey, Constants.ImageEditorSaveTextDefValue);
            _resource.Add(Constants.ImageEditorSaveAsNewTextKey, Constants.ImageEditorSaveAsNewTextDefValue);
            _resource.Add(Constants.ImageEditorApplyTextKey, Constants.ImageEditorApplyTextDefValue);
            _resource.Add(Constants.ImageEditorCancelTextKey, Constants.ImageEditorCancelDefValue);
            _resource.Add(Constants.ImageEditorInvertTextKey, Constants.ImageEditorInvertTextDefValue);
            _resource.Add(Constants.ImageEditorBrightnessTextKey, Constants.ImageEditorBrightnessTextDefValue);
            _resource.Add(Constants.ImageEditorContrastTextKey, Constants.ImageEditorContrastTextDefValue);
            _resource.Add(Constants.ImageEditorAutoLevelsTextKey, Constants.ImageEditorAutoLevelsTextDefValue);
            _resource.Add(Constants.ImageEditorBlackAndWhiteTextKey, Constants.ImageEditorBlackAndWhiteDefValue);
            _resource.Add(Constants.ImageEditorSepiaTextKey, Constants.ImageEditorSepiaTextDefValue);
            _resource.Add(Constants.ImageEditorRedEyeStep1TextKey, Constants.ImageEditorRedEyeStep1TextDefValue);
            _resource.Add(Constants.ImageEditorRedEyeStep2TextKey, Constants.ImageEditorRedEyeStep2TextDefValue);
            _resource.Add(Constants.ImageEditorRedEyeStep3TextKey, Constants.ImageEditorRedEyeStep3TextDefValue);
            _resource.Add(Constants.ImageEditorRedEyeManualTextKey, Constants.ImageEditorRedEyeManualTextDefValue);
            _resource.Add(Constants.ImageEditorRedEyeNextTextKey, Constants.ImageEditorRedEyeNextTextDefValue);
            _resource.Add(Constants.ImageEditorRedEyeRemoveTextKey, Constants.ImageEditorRedEyeRemoveTextDefValue);
            _resource.Add(Constants.MessageWantToExitTextKey, Constants.MessageWantToExitDefValue);
            _resource.Add(Constants.MessageEditNextTextKey, Constants.MessageEditNextTextDefValue);
            _resource.Add(Constants.MessageEditPreviousTextKey, Constants.MessageEditPreviousTextDefValue);
            _resource.Add(Constants.MessageMinCropSizeKey, Constants.MessageMinCropSizeDefValue);
            _resource.Add(Constants.MessageTransformImageKey, Constants.MessageTransformImageDefValue);

            // Additional Services Screen
            _resource.Add(Constants.AdditionalServicesTextKey, Constants.AdditionalServicesTextDefValue);
            _resource.Add(Constants.AdditionalServicePriceTextKey, Constants.AdditionalServicePriceTextDefValue);

            // Confirm Order Screen
            _resource.Add(Constants.OrderConfirmTextKey, Constants.OrderConfirmTextDefValue);
            _resource.Add(Constants.OrderConfirmCdBurningTextKey, Constants.OrderConfirmCdBurningTexDefValue);
            _resource.Add(Constants.OrderConfirmNoticeTextKey, Constants.OrderConfirmNoticeTextDefValue);
            _resource.Add(Constants.OrderConfirmNoticeCdBurningTextKey, Constants.OrderConfirmNoticeCdBurningTextDefValue);

            // Contact Info Screen
            _resource.Add(Constants.ContactInfoTextKey, Constants.ContactInfoTextDefValue);
            _resource.Add(Constants.ContactInfoCdBurningTextKey, Constants.ContactInfoCdBurningTextDefValue);
            _resource.Add(Constants.ContactInfoNoticeTextKey, Constants.ContactInfoNoticeTextDefValue);
            _resource.Add(Constants.YourNameTextKey, Constants.YourNameTextDefValue);
            _resource.Add(Constants.YourPhoneTextKey, Constants.YourPhoneTextDefValue);
            _resource.Add(Constants.YourEmailTextKey, Constants.YourEmailTextDefValue);
            _resource.Add(Constants.KeyboardLayoutKey, Constants.KeyboardLayoutDefValue);
            _resource.Add(Constants.KeyboardShiftedLayoutKey, Constants.KeyboardShiftedLayoutDefValue);
            _resource.Add(Constants.MessageEnterYourNameKey, Constants.MessageEnterYourNameDefValue);
            _resource.Add(Constants.MessageEnterYourPhoneKey, Constants.MessageEnterYourPhoneDefValue);
            _resource.Add(Constants.MessageEnterYourEmailKey, Constants.MessageEnterYourEmailDefValue);
            _resource.Add(Constants.MessageProgressOrderKey, Constants.MessageProgressOrderDefValue);
            _resource.Add(Constants.MessagePreparingImageKey, Constants.MessagePreparingImageDefValue);

            // Order Id Screen
            _resource.Add(Constants.OrderIdTextKey, Constants.OrderIdTextDefValue);
            _resource.Add(Constants.OrderIdCdBurningTextKey, Constants.OrderIdCdBurningTextDefValue);
            _resource.Add(Constants.OrderIdProcessOrderTextKey, Constants.OrderIdProcessOrderTextDefValue);
            _resource.Add(Constants.OrderIdNoticeTextKey, Constants.OrderIdNoticeTextDefValue);
            _resource.Add(Constants.OrderIdProcessOrderNoticeTextKey, Constants.OrderIdProcessOrderNoticeTextDefValue);
            _resource.Add(Constants.YourOrderIdTextKey, Constants.YourOrderIdTextDefValue);
            _resource.Add(Constants.YourActivationCodeTextKey, Constants.YourActivationCodeTextDefValue);
            _resource.Add(Constants.MessageEnterYourOrderIdKey, Constants.MessageEnterYourOrderIdDefValue);
            _resource.Add(Constants.MessageEnterYourActivationCodeKey, Constants.MessageEnterYourActivationCodeDefValue);
            _resource.Add(Constants.MessageActivationCodeIncorrectKey, Constants.MessageActivationCodeIncorrectDefValue);

            // Burning Screen
            _resource.Add(Constants.BurningScreenTextKey, Constants.BurningScreenTextDefValue);
            _resource.Add(Constants.BurningScreenBurnButtonKey, Constants.BurningScreenBurnButtonDefValue);
            _resource.Add(Constants.BurningScreenBeforeTextKey, Constants.BurningScreenBeforeTextDefValue);
            _resource.Add(Constants.BurningScreenInProcessTextKey, Constants.BurningScreenInProcessTextDefValue);
            _resource.Add(Constants.BurningScreenMessageTryAgainTextKey, Constants.BurningScreenMessageTryAgainTextDefValue);
            _resource.Add(Constants.BurningScreenMessageCancelTextKey, Constants.BurningScreenMessageCancelTextDefValue);

            // Printing Screen
            _resource.Add(Constants.PrintingScreenTextKey, Constants.PrintingScreenTextDefValue);
            _resource.Add(Constants.PrintingScreenNoticeKey, Constants.PrintingScreenNoticeDefValue);

            // ThankYou Screen
            _resource.Add(Constants.ThankYouScreenTextKey, Constants.ThankYouScreenTextDefValue);
            _resource.Add(Constants.ThankYouScreenNoticeTextKey, Constants.ThankYouScreenNoticeTextDefValue);
            _resource.Add(Constants.ThankYouScreenOrderIdKey, Constants.ThankYouScreenOrderIdDefValue);
            _resource.Add(Constants.ThanksButtonTextKey, Constants.ThanksButtonTextDefValue);
        }

        #endregion [Private methods and props]

        #region [Variables]

        private PrimaryActionType _primaryAction;
        private PaperFormat _selectedPaperFormat = null;

        private Frame _showingFrame;
        private ResourceDictionary _resource;
        private MainWindow _mainWindow;

        private DateTime _lastUserActionTime;
        private StageBase _currentActiveStage;
        private List<StageBase> _stages;
        private List<Window> _modalWindows;
        private Mutex _namedMutex;
        private OrderManagerAddon _orderManager;

        private Dictionary<string, BitmapImage> _headerDictionary;
        private Dictionary<string, ImageBrush> _backgroundDictionary;

        private bool _disposed;
        private bool _isBluetooth;
        private bool _isShowFolders;

        #endregion [Variables]
    }

    public enum PrimaryActionType
    {
        OrderPhotos,
        BurnCd,
        PrintPhotos,
        ProcessOrder
    }
}