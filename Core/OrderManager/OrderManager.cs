// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using ODS.Service;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Aurigma.PhotoKiosk.Core.OrderManager
{
    public class OrderManager
    {
        public const int TagsCount = 11;

        public const string KioskIdTag = "%KioskId%";
        public const string OrderIdTag = "%OrderId%";
        public const string CustomerNameTag = "%CustomerName%";
        public const string CustomerPhoneTag = "%CustomerPhone%";
        public const string CustomerEmailTag = "%CustomerEmail%";
        public const string FormatTag = "%Format%";
        public const string PaperTypeTag = "%PaperType%";
        public const string CropModeTag = "%CropMode%";
        public const string PrintsQuantityTag = "%PrintsQuantity%";
        public const string DateTag = "%Date%";
        public const string FilenameTag = "%FileName%";

        public const string ChannelTag = "%Channel%";
        public const string DpofPSizeTag = "%PSize%";

        public const string OrderInfoFilename = "info.html";

        public static Dictionary<string, string> TagList = new Dictionary<string, string>(TagsCount)
        {
            {RM.GetString("KioskIdTag"), KioskIdTag},
            {RM.GetString("OrderIdTag"), OrderIdTag},
            {RM.GetString("CustomerNameTag"), CustomerNameTag},
            {RM.GetString("CustomerPhoneTag"), CustomerPhoneTag},
            {RM.GetString("CustomerEmailTag"), CustomerEmailTag},
            {RM.GetString("FormatTag"), FormatTag},
            {RM.GetString("PaperTypeTag"), PaperTypeTag},
            {RM.GetString("CropModeTag"), CropModeTag},
            {RM.GetString("PrintsQuantityTag"), PrintsQuantityTag},
            {RM.GetString("DateTag"), DateTag},
            {RM.GetString("FilenameTag"), FilenameTag}
        };

        public readonly Setting<string> DestinationPath = new Setting<string>(false, "DestinationPath", Config.GetFullPath("Orders"), false);
        public readonly Setting<string> PhotoTemplate = new Setting<string>(false, "Template", @"%KioskId%\%Date%\%CustomerName%\%Format%_%PaperType%\%PrintsQuantity%_%FileName%", false);
        public readonly Setting<string> InfoTemplate = new Setting<string>(false, "InfoTemplate", @"%KioskId%\%OrderId%_" + OrderInfoFilename, false);
        public readonly Setting<bool> EnableCleanup = new Setting<bool>(false, "EnableCleanup", false, true);
        public readonly Setting<bool> ConvertToJpeg = new Setting<bool>(false, "ConvertToJpeg", false, true);
        public readonly Setting<bool> TransliteratePath = new Setting<bool>(false, "TransliteratePath", false, true);
        public readonly Setting<bool> IsDpof = new Setting<bool>(false, "IsDpof", false, true);
        public readonly Setting<string> DpofOrderTemplate = new Setting<string>(false, "DpofPhotoTemplate", @"%KioskId%\Order_%OrderId%", false);
        public readonly Setting<string> DpofInfoTemplate = new Setting<string>(false, "DpofInfoTemplate", @"%KioskId%\Order_%OrderId%\" + OrderInfoFilename, false);

        public delegate void ModifiedHandler();

        public event ModifiedHandler Modified;

        private Configuration _config;

        public OrderManager()
        {
            var configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = Config.GetFullPath("OrderManager.exe.config");
            _config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            if (_config.HasFile)
            {
                DestinationPath.Init(_config.AppSettings.Settings[DestinationPath.Key].Value);
                PhotoTemplate.Init(_config.AppSettings.Settings[PhotoTemplate.Key].Value);
                InfoTemplate.Init(_config.AppSettings.Settings[InfoTemplate.Key].Value);
                try
                {
                    EnableCleanup.Init(bool.Parse((_config.AppSettings.Settings[EnableCleanup.Key].Value)));
                }
                catch
                {
                }

                try
                {
                    ConvertToJpeg.Init(bool.Parse((_config.AppSettings.Settings[ConvertToJpeg.Key].Value)));
                }
                catch
                {
                }

                try
                {
                    TransliteratePath.Init(bool.Parse((_config.AppSettings.Settings[TransliteratePath.Key].Value)));
                }
                catch
                {
                }

                try
                {
                    IsDpof.Init(bool.Parse((_config.AppSettings.Settings[IsDpof.Key].Value)));
                }
                catch
                {
                }

                DpofOrderTemplate.Init(_config.AppSettings.Settings[DpofOrderTemplate.Key].Value);
                DpofInfoTemplate.Init(_config.AppSettings.Settings[DpofInfoTemplate.Key].Value);
            }
            else
            {
                Save();
            }

            DestinationPath.ValueChanged += new Setting<string>.SettingChangedHandler(SettingValueChanged);
            PhotoTemplate.ValueChanged += new Setting<string>.SettingChangedHandler(SettingValueChanged);
            InfoTemplate.ValueChanged += new Setting<string>.SettingChangedHandler(SettingValueChanged);
            EnableCleanup.ValueChanged += new Setting<bool>.SettingChangedHandler(SettingValueChanged);
            ConvertToJpeg.ValueChanged += new Setting<bool>.SettingChangedHandler(SettingValueChanged);
            TransliteratePath.ValueChanged += new Setting<bool>.SettingChangedHandler(SettingValueChanged);
            IsDpof.ValueChanged += new Setting<bool>.SettingChangedHandler(SettingValueChanged);
            DpofOrderTemplate.ValueChanged += new Setting<string>.SettingChangedHandler(SettingValueChanged);
            DpofInfoTemplate.ValueChanged += new Setting<string>.SettingChangedHandler(SettingValueChanged);
        }

        private void SettingValueChanged(object sender)
        {
            if (Modified != null)
                Modified();
        }

        public void Save()
        {
            _config.AppSettings.Settings.Clear();

            Config.SaveStringSetting(DestinationPath, _config);
            Config.SaveStringSetting(PhotoTemplate, _config);
            Config.SaveStringSetting(InfoTemplate, _config);
            Config.SaveBoolSetting(EnableCleanup, _config);
            Config.SaveBoolSetting(ConvertToJpeg, _config);
            Config.SaveBoolSetting(TransliteratePath, _config);
            Config.SaveBoolSetting(IsDpof, _config);
            Config.SaveStringSetting(DpofOrderTemplate, _config);
            Config.SaveStringSetting(DpofInfoTemplate, _config);

            _config.Save(ConfigurationSaveMode.Modified);
        }

        public void ProcessOrder(string folder)
        {
            var service = new PreparationService();
            var order = new ProcessingOrder(folder, this);
            service.Prepare(order.OrderFiles, order.OrderDocs, order.Rule);

            if (EnableCleanup.Value)
            {
                try
                {
                    Directory.Delete(folder, true);
                }
                catch
                {
                }
            }
        }
    }
}