// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace Aurigma.PhotoKiosk
{
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = Config.Localization;

            this.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(new Uri("BaseTheme.xaml", UriKind.Relative)));
            this.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(new Uri("ControlTemplates.xaml", UriKind.Relative)));

            Config config;
            try
            {
                config = new Config(true);
            }
            catch (ConfigurationErrorsException)
            {
                // Show PhotoKiosk style error message on MainWindow load.
                return;
            }
            catch (XmlException)
            {
                // Show PhotoKiosk style error message on MainWindow load.
                return;
            }

            if (config.ThemeFile.Value != "")
            {
                if (!File.Exists(config.ThemeFile.Value))
                {
                    // Show PhotoKiosk style error message on MainWindow load.
                    return;
                }
                try
                {
                    var fileStream = new FileStream(config.ThemeFile.Value, FileMode.Open, FileAccess.Read);
                    var reader = new XamlReader();
                    ResourceDictionary theme = (ResourceDictionary)reader.LoadAsync(fileStream);
                    this.Resources.MergedDictionaries.Add(theme);
                }
                catch
                {
                    return;
                }
            }
        }
    }
}