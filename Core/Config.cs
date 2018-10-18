// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Aurigma.PhotoKiosk.Core
{
    public class Config
    {
        private Configuration _kioskConfig;

        // Constants
        internal const int XorValue = 0xB2;

        public const string PhotoKioskVersion = "7.0";

        public const string PhotoKioskLocalization = "en-US";

        public delegate void ModifiedHandler();

        public event ModifiedHandler Modified;

        // General
        public readonly Setting<string> PhotoKioskId;

        public readonly Setting<int> MemoryCacheSize;
        public readonly Setting<int> SearchProcessDuration;
        public readonly Setting<int> InactivityTimeout;
        public readonly Setting<bool> EnableEventLogging;
        public readonly Setting<string> SourcePaths;
        public readonly Setting<bool> EnableFolderSelection;
        public readonly Setting<string> PaperSizeUnits;

        // Bluetooth
        public readonly Setting<bool> EnableBluetooth;

        public readonly Setting<string> BluetoothHost;
        public readonly Setting<string> BluetoothFolder;

        // Callbacks
        public readonly Setting<string> OnStartCallback;

        public readonly Setting<string> OnCancelCallback;
        public readonly Setting<string> OnCompleteCallback;

        // Image Editor
        public readonly Setting<bool> EnableImageEditor;

        public readonly Setting<bool> EnableRotation;
        public readonly Setting<bool> EnableFlip;
        public readonly Setting<bool> EnableCrop;
        public readonly Setting<bool> EnableColorCorrection;
        public readonly Setting<bool> EnableEffects;
        public readonly Setting<bool> EnableRedEyeRemoval;
        public readonly Setting<string> CropFile;

        // Contact info
        public readonly Setting<bool> EnableCustomerPhone;

        public readonly Setting<bool> EnableCustomerEmail;
        public readonly Setting<bool> EnableCustomerOrderId;
        public readonly Setting<bool> EnableCustomerNameOrderId;
        public readonly Setting<bool> SkipContactInfo;

        // PhotoOrdering
        public readonly Setting<bool> EnablePhotoOrdering;

        // Price file
        public readonly Setting<string> PriceFile;

        // CDBurner
        public readonly Setting<bool> EnableCDBurning;

        public readonly Setting<bool> CDBurningRequireConfirm;
        public readonly Setting<string> DriveLetter;
        public readonly Setting<string> DiscLabel;
        public readonly Setting<float> CDBurningCost;
        public readonly Setting<string> CDBurningPaymentInstructions;

        // PhotoPrinter
        public readonly Setting<bool> EnablePhotoPrinting;

        public readonly Setting<bool> PhotoPrintingRequireConfirm;
        public readonly Setting<string> PhotoPrintingPaymentInstructions;

        // ReceiptPrinter
        public readonly Setting<bool> EnableReceiptPrinter;

        public readonly Setting<string> ReceiptPrinterName;
        public readonly Setting<string> ReceiptTemplateFile;
        public readonly Setting<int> PhotosInReceipt;

        // Appearance
        public readonly Setting<string> ThemeFile;

        public readonly Setting<string> LocalizationFile;
        public readonly Setting<string> MainBanner;

        // Screens
        public readonly List<ScreenSetting> Screens = new List<ScreenSetting>(16);

        public static string TempStoragePath
        {
            get { return GetFullPath("TempStorage"); }
        }

        public static string LastOrderIdFile
        {
            get { return GetFullPath("LastOrderId.txt"); }
        }

        public static string EventLogFile
        {
            get { return GetFullPath("EventsLog.txt"); }
        }

        public static string TransformFile
        {
            get { return GetFullPath("Transform.xslt"); }
        }

        public static string EventLogSource
        {
            get { return RM.GetString("EventLogSourceName") + PhotoKioskVersion; }
        }

        public static string CurrentDirectory
        {
            get { return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location); }
        }

        public static string CDBurnerFile
        {
            get { return CurrentDirectory + Path.DirectorySeparatorChar + "Aurigma.PhotoKiosk.CdBurner.exe"; }
        }

        public static string PhotoPrinterFile
        {
            get { return CurrentDirectory + Path.DirectorySeparatorChar + "Aurigma.PhotoKiosk.PhotoPrinter.exe"; }
        }

        public static string OrderManagerFile
        {
            get { return CurrentDirectory + Path.DirectorySeparatorChar + "Aurigma.PhotoKiosk.OrderManager.exe"; }
        }

        public static string HelpFile
        {
            get { return CurrentDirectory + Path.DirectorySeparatorChar + RM.GetString("HelpFile"); }
        }

        public static string InstallationPath
        {
            get
            {
                return Environment.GetEnvironmentVariable("ProgramFiles") + Path.DirectorySeparatorChar + RM.GetString("InstallPath") + " " + PhotoKioskVersion + Path.DirectorySeparatorChar;
            }
        }

        public static CultureInfo Localization
        {
            get
            {
                CultureInfo culture;
                try
                {
                    culture = new CultureInfo(PhotoKioskLocalization);
                }
                catch (Exception)
                {
                    culture = new CultureInfo("en-US");
                }

                return culture;
            }
        }

        public Config(bool isReadonly)
        {
            // General
            PhotoKioskId = new Setting<string>(isReadonly, "PhotoKioskId", "", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            MemoryCacheSize = new Setting<int>(isReadonly, "MemoryCacheSize", 100, false, new Setting<int>.SettingChangedHandler(SettingValueChanged));
            SearchProcessDuration = new Setting<int>(isReadonly, "SearchProcessDuration", 7000, false, new Setting<int>.SettingChangedHandler(SettingValueChanged));
            InactivityTimeout = new Setting<int>(isReadonly, "InactivityTimeout", 420000, false, new Setting<int>.SettingChangedHandler(SettingValueChanged));
            EnableEventLogging = new Setting<bool>(isReadonly, "EnableEventLogging", false, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            SourcePaths = new Setting<string>(isReadonly, "SourcePaths", "", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            EnableFolderSelection = new Setting<bool>(isReadonly, "EnableFolderSelection", true, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            PaperSizeUnits = new Setting<string>(isReadonly, "PaperSizeUnits", Constants.MmUnits, false, new Setting<string>.SettingChangedHandler(SettingValueChanged));

            // Bluetooth
            EnableBluetooth = new Setting<bool>(isReadonly, "EnableBluetooth", false, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            BluetoothHost = new Setting<string>(isReadonly, "BluetoothHost", RM.GetString("BluetoothHostNameDefValue"), true, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            BluetoothFolder = new Setting<string>(isReadonly, "BluetoothFolder", "", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));

            // Callback
            OnStartCallback = new Setting<string>(isReadonly, "OnStartCallback", "", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            OnCancelCallback = new Setting<string>(isReadonly, "OnCancelCallback", "", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            OnCompleteCallback = new Setting<string>(isReadonly, "OnCompleteCallback", "", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));

            // Image Editor
            EnableImageEditor = new Setting<bool>(isReadonly, "EnableImageEditor", true, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            EnableRotation = new Setting<bool>(isReadonly, "EnableRotation", true, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            EnableFlip = new Setting<bool>(isReadonly, "EnableFlip", true, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            EnableCrop = new Setting<bool>(isReadonly, "EnableCrop", true, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            EnableColorCorrection = new Setting<bool>(isReadonly, "EnableColorCorrection", true, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            EnableEffects = new Setting<bool>(isReadonly, "EnableEffects", true, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            EnableRedEyeRemoval = new Setting<bool>(isReadonly, "EnableRedEyeRemoval", true, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            CropFile = new Setting<string>(isReadonly, "CropFile", GetFullPath("Crop.xml"), false, new Setting<string>.SettingChangedHandler(SettingValueChanged));

            // Contact info
            EnableCustomerPhone = new Setting<bool>(isReadonly, "EnableCustomerPhone", true, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            EnableCustomerEmail = new Setting<bool>(isReadonly, "EnableCustomerEmail", false, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            EnableCustomerOrderId = new Setting<bool>(isReadonly, "EnableCustomerOrderId", false, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            EnableCustomerNameOrderId = new Setting<bool>(isReadonly, "EnableCustomerNameOrderId", true, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            SkipContactInfo = new Setting<bool>(isReadonly, "SkipContactInfo", false, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));

            // Photo ordering
            EnablePhotoOrdering = new Setting<bool>(isReadonly, "EnablePhotoOrdering", true, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));

            // Price file
            PriceFile = new Setting<string>(isReadonly, "PriceFile", GetFullPath("Price.xml"), false, new Setting<string>.SettingChangedHandler(SettingValueChanged));

            // CDburner
            EnableCDBurning = new Setting<bool>(isReadonly, "EnableCDBurner", false, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            CDBurningRequireConfirm = new Setting<bool>(isReadonly, "CDBurnerRequireConfirm", false, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            DriveLetter = new Setting<string>(isReadonly, "DriveLetter", "", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            DiscLabel = new Setting<string>(isReadonly, "DiscLabel", "Photo_Kiosk", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            CDBurningCost = new Setting<float>(isReadonly, "CDBurningCost", 10.0f, true, new Setting<float>.SettingChangedHandler(SettingValueChanged));
            CDBurningPaymentInstructions = new Setting<string>(isReadonly, "CDBurnerPaymentInstructions", RM.GetString("CDBurningPaymentInstructions"), true, new Setting<string>.SettingChangedHandler(SettingValueChanged));

            // PhotoPrinter
            EnablePhotoPrinting = new Setting<bool>(isReadonly, "EnablePhotoPrinter", false, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            PhotoPrintingRequireConfirm = new Setting<bool>(isReadonly, "PhotoPrinterRequireConfirm", false, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            PhotoPrintingPaymentInstructions = new Setting<string>(isReadonly, "PhotoPrintingPaymentInstructions", RM.GetString("PhotoPrintingPaymentInstructions"), true, new Setting<string>.SettingChangedHandler(SettingValueChanged));

            // ReceiptPrinter
            EnableReceiptPrinter = new Setting<bool>(isReadonly, "EnableReceiptPrinter", false, true, new Setting<bool>.SettingChangedHandler(SettingValueChanged));
            ReceiptPrinterName = new Setting<string>(isReadonly, "ReceiptPrinterName", "", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            ReceiptTemplateFile = new Setting<string>(isReadonly, "ReceiptTemplateFile", GetFullPath("ReceiptTemplate.xaml"), false, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            PhotosInReceipt = new Setting<int>(isReadonly, "PhotosInReceipt", 20, true, new Setting<int>.SettingChangedHandler(SettingValueChanged));

            // Appearance
            ThemeFile = new Setting<string>(isReadonly, "ThemeFile", "", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            LocalizationFile = new Setting<string>(isReadonly, "LocalizationFile", GetFullPath("Resources.xml"), false, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            MainBanner = new Setting<string>(isReadonly, "MainBanner", "", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));

            // Screens
            Screens.Add(new ScreenSetting(isReadonly, "WelcomeScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "ChooseUploadWayScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "SelectFoldersScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "UploadingPhotosScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "ProgressScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "SelectScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "OrderFormingScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "AdditionalServicesScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "ImageEditorScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "ConfirmOrderScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "ContactInfoScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "OrderIdScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "BurningScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "PrintingScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "ThankYouScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));
            Screens.Add(new ScreenSetting(isReadonly, "ThankYouCancelScreen", new Setting<string>.SettingChangedHandler(SettingValueChanged)));

            var configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = GetFullPath(RM.GetString("ConfigFileName"));
            _kioskConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            Load();
        }

        private void SettingValueChanged(object sender)
        {
            if (Modified != null)
                Modified();
        }

        public void Save()
        {
            _kioskConfig.AppSettings.Settings.Clear();

            // General
            SaveStringSetting(PhotoKioskId, _kioskConfig);
            SaveIntSetting(MemoryCacheSize, _kioskConfig);
            SaveIntSetting(SearchProcessDuration, _kioskConfig);
            SaveIntSetting(InactivityTimeout, _kioskConfig);
            SaveBoolSetting(EnableEventLogging, _kioskConfig);
            SaveStringSetting(SourcePaths, _kioskConfig);
            SaveBoolSetting(EnableFolderSelection, _kioskConfig);
            SaveStringSetting(PaperSizeUnits, _kioskConfig);

            // Bluetooth
            SaveBoolSetting(EnableBluetooth, _kioskConfig);
            SaveStringSetting(BluetoothHost, _kioskConfig);
            SaveStringSetting(BluetoothFolder, _kioskConfig);

            // Callbacks
            SaveStringSetting(OnStartCallback, _kioskConfig);
            SaveStringSetting(OnCancelCallback, _kioskConfig);
            SaveStringSetting(OnCompleteCallback, _kioskConfig);

            // Image Editor
            SaveBoolSetting(EnableImageEditor, _kioskConfig);
            SaveBoolSetting(EnableRotation, _kioskConfig);
            SaveBoolSetting(EnableFlip, _kioskConfig);
            SaveBoolSetting(EnableCrop, _kioskConfig);
            SaveBoolSetting(EnableColorCorrection, _kioskConfig);
            SaveBoolSetting(EnableEffects, _kioskConfig);
            SaveBoolSetting(EnableRedEyeRemoval, _kioskConfig);
            SaveStringSetting(CropFile, _kioskConfig);

            // Contact info
            SaveBoolSetting(EnableCustomerPhone, _kioskConfig);
            SaveBoolSetting(EnableCustomerEmail, _kioskConfig);
            SaveBoolSetting(EnableCustomerOrderId, _kioskConfig);
            SaveBoolSetting(EnableCustomerNameOrderId, _kioskConfig);
            SaveBoolSetting(SkipContactInfo, _kioskConfig);

            // Photo ordering
            SaveBoolSetting(EnablePhotoOrdering, _kioskConfig);

            // Price file
            SaveStringSetting(PriceFile, _kioskConfig);

            // CDBurner
            SaveBoolSetting(EnableCDBurning, _kioskConfig);
            SaveBoolSetting(CDBurningRequireConfirm, _kioskConfig);
            SaveStringSetting(DriveLetter, _kioskConfig);
            SaveStringSetting(DiscLabel, _kioskConfig);
            SaveFloatSetting(CDBurningCost, _kioskConfig);
            SaveStringSetting(CDBurningPaymentInstructions, _kioskConfig);

            // PhotoPrinter
            SaveBoolSetting(EnablePhotoPrinting, _kioskConfig);
            SaveBoolSetting(PhotoPrintingRequireConfirm, _kioskConfig);
            SaveStringSetting(PhotoPrintingPaymentInstructions, _kioskConfig);

            // ReceiptPrinter
            SaveBoolSetting(EnableReceiptPrinter, _kioskConfig);
            SaveStringSetting(ReceiptPrinterName, _kioskConfig);
            SaveStringSetting(ReceiptTemplateFile, _kioskConfig);
            SaveIntSetting(PhotosInReceipt, _kioskConfig);

            // Appearance
            SaveStringSetting(ThemeFile, _kioskConfig);
            SaveStringSetting(LocalizationFile, _kioskConfig);
            SaveStringSetting(MainBanner, _kioskConfig);

            // Screens
            foreach (ScreenSetting screen in Screens)
                screen.Save(_kioskConfig);

            _kioskConfig.Save(ConfigurationSaveMode.Modified);
        }

        public bool IsBluetoothEnabled()
        {
            return EnableBluetooth.Value && !string.IsNullOrEmpty(BluetoothFolder.Value) && Directory.Exists(BluetoothFolder.Value) && !string.IsNullOrEmpty(BluetoothHost.Value);
        }

        public bool IsImageEditorEnabled()
        {
            return EnableImageEditor.Value &&
                (EnableRotation.Value || EnableFlip.Value || EnableCrop.Value || EnableColorCorrection.Value || EnableEffects.Value || EnableRedEyeRemoval.Value);
        }

        public bool IsCDBurningEnabled()
        {
            return EnableCDBurning.Value && !string.IsNullOrEmpty(DriveLetter.Value);
        }

        public bool IsPhotoPrintingEnabled(PriceManager priceManager)
        {
            return EnablePhotoPrinting.Value && priceManager.InstantPaperFormats.Count > 0 && priceManager.InstantPaperTypes.Count > 0;
        }

        public bool IsMultiAction(PriceManager priceManager)
        {
            return (EnablePhotoOrdering.Value && IsPhotoPrintingEnabled(priceManager)) ||
                   (EnablePhotoOrdering.Value && IsCDBurningEnabled()) ||
                   (IsCDBurningEnabled() && IsPhotoPrintingEnabled(priceManager)) ||
                   (IsPhotoPrintingEnabled(priceManager) && PhotoPrintingRequireConfirm.Value ||
                    IsCDBurningEnabled() && CDBurningRequireConfirm.Value);
        }

        public ScreenSetting GetScreen(string key)
        {
            foreach (ScreenSetting screen in Screens)
            {
                if (screen.Key == key)
                    return screen;
            }
            return null;
        }

        private void Load()
        {
            if (_kioskConfig.HasFile)
            {
                // General
                InitStringSetting(PhotoKioskId);
                InitIntSetting(MemoryCacheSize);
                InitIntSetting(SearchProcessDuration);
                InitIntSetting(InactivityTimeout);
                InitBoolSetting(EnableEventLogging);
                InitStringSetting(SourcePaths);
                InitBoolSetting(EnableFolderSelection);
                InitStringSetting(PaperSizeUnits);

                // Bluetooth
                InitBoolSetting(EnableBluetooth);
                InitStringSetting(BluetoothHost);
                InitStringSetting(BluetoothFolder);

                // Callbacks
                InitStringSetting(OnStartCallback);
                InitStringSetting(OnCancelCallback);
                InitStringSetting(OnCompleteCallback);

                // Image Editor
                InitBoolSetting(EnableImageEditor);
                InitBoolSetting(EnableRotation);
                InitBoolSetting(EnableFlip);
                InitBoolSetting(EnableCrop);
                InitBoolSetting(EnableColorCorrection);
                InitBoolSetting(EnableEffects);
                InitBoolSetting(EnableRedEyeRemoval);
                InitStringSetting(CropFile);

                // Contact info
                InitBoolSetting(EnableCustomerPhone);
                InitBoolSetting(EnableCustomerEmail);
                InitBoolSetting(EnableCustomerOrderId);
                InitBoolSetting(EnableCustomerNameOrderId);
                InitBoolSetting(SkipContactInfo);

                // Price file
                InitStringSetting(PriceFile);

                // Photo ordering
                InitBoolSetting(EnablePhotoOrdering);

                // CDBurner
                InitBoolSetting(EnableCDBurning);
                InitBoolSetting(CDBurningRequireConfirm);
                InitStringSetting(DriveLetter);
                InitStringSetting(DiscLabel);
                InitFloatSetting(CDBurningCost);
                InitStringSetting(CDBurningPaymentInstructions);

                // PhotoPrinter
                InitBoolSetting(EnablePhotoPrinting);
                InitBoolSetting(PhotoPrintingRequireConfirm);
                InitStringSetting(PhotoPrintingPaymentInstructions);

                // ReceiptPrinter
                InitBoolSetting(EnableReceiptPrinter);
                InitStringSetting(ReceiptPrinterName);
                InitStringSetting(ReceiptTemplateFile);
                InitIntSetting(PhotosInReceipt);

                // Appearance
                InitStringSetting(ThemeFile);
                InitStringSetting(LocalizationFile);
                InitStringSetting(MainBanner);

                // Screens
                foreach (ScreenSetting screen in Screens)
                    screen.Init(_kioskConfig);
            }
            else
            {
                // Create default config file with default settings
                Save(); 
            }
        }

        private void InitStringSetting(Setting<string> setting)
        {
            try
            {
                setting.Init(_kioskConfig.AppSettings.Settings[setting.Key].Value);
            }
            catch (Exception)
            {
            }
        }

        private void InitIntSetting(Setting<int> setting)
        {
            try
            {
                setting.Init(int.Parse((_kioskConfig.AppSettings.Settings[setting.Key].Value), CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
            }
        }

        private void InitBoolSetting(Setting<bool> setting)
        {
            try
            {
                setting.Init(bool.Parse((_kioskConfig.AppSettings.Settings[setting.Key].Value)));
            }
            catch (Exception)
            {
            }
        }

        private void InitFloatSetting(Setting<float> setting)
        {
            try
            {
                setting.Init(float.Parse((_kioskConfig.AppSettings.Settings[setting.Key].Value), CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
            }
        }

        internal static void SaveStringSetting(Setting<string> setting, Configuration config)
        {
            string value = !setting.CanBeEmpty && string.IsNullOrEmpty(setting.Value) ? setting.DefaultValue : setting.Value;
            config.AppSettings.Settings.Add(setting.Key, value);
        }

        internal static void SaveIntSetting(Setting<int> setting, Configuration config)
        {
            string value = !setting.CanBeEmpty && setting.Value == 0 ? setting.DefaultValue.ToString(CultureInfo.InvariantCulture) : setting.Value.ToString(CultureInfo.InvariantCulture);
            config.AppSettings.Settings.Add(setting.Key, value);
        }

        internal static void SaveFloatSetting(Setting<float> setting, Configuration config)
        {
            string value = !setting.CanBeEmpty && setting.Value == 0.0f ? setting.DefaultValue.ToString(CultureInfo.InvariantCulture) : setting.Value.ToString(CultureInfo.InvariantCulture);
            config.AppSettings.Settings.Add(setting.Key, value);
        }

        internal static void SaveBoolSetting(Setting<bool> setting, Configuration config)
        {
            string value = !setting.CanBeEmpty && !setting.Value ? setting.DefaultValue.ToString(CultureInfo.InvariantCulture) : setting.Value.ToString(CultureInfo.InvariantCulture);
            config.AppSettings.Settings.Add(setting.Key, value);
        }

        public static string GetFullPath(string fileName)
        {
            if (fileName != "")
            {
                if (Path.IsPathRooted(fileName))
                    return fileName;
                else
                    return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}",
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        Path.DirectorySeparatorChar,
                        RM.GetString("UserConfigPath") + Config.PhotoKioskVersion,
                        Path.DirectorySeparatorChar,
                        fileName);
            }
            else
                return fileName;
        }

        public static bool ApplicationPathExists()
        {
            return (Directory.Exists(string.Format(
                CultureInfo.InvariantCulture, "{0}{1}{2}",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Path.DirectorySeparatorChar,
                RM.GetString("UserConfigPath") + Config.PhotoKioskVersion)));
        }

        public void RestoreDefaultResourcesFile()
        {
            Tools.SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(RM.GetString("ResourcesFile")), LocalizationFile.Value);
        }

        public static void RestoreDefaultTransformFile()
        {
            Tools.SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(RM.GetString("TransformFile")), TransformFile);
        }

        public static void RestoreDefaultCropsFile(string filename)
        {
            Tools.SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(RM.GetString("CropFile")), filename);
        }

        public static void RestoreDefaultPriceFile(string filename)
        {
            Tools.SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(RM.GetString("PriceFile")), filename);
        }

        public static void RestoreDefaultReceiptFile(string filename)
        {
            Tools.SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(RM.GetString("ReceiptFile")), filename);
        }
    }
}