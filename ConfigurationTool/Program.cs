// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    internal static class Program
    {
        [DllImport("User32.dll")]
        public static extern int ShowWindowAsync(IntPtr hWnd, int swCommand);

        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                string name = Assembly.GetExecutingAssembly().GetName().Name;
                Process[] RunningProcesses = Process.GetProcessesByName(name);
                if (RunningProcesses.Length <= 1)
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
                    Thread.CurrentThread.CurrentUICulture = Config.Localization;

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                else
                {
                    ShowWindowAsync(RunningProcesses[0].MainWindowHandle, 2);
                    ShowWindowAsync(RunningProcesses[0].MainWindowHandle, 9);
                }
            }
        }

        internal static string ElevatedExecute(NameValueCollection parameters)
        {
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, ConstructQueryString(parameters));

            try
            {
                var startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                var uri = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase);
                startInfo.FileName = uri.LocalPath;
                startInfo.Arguments = "\"" + tempFile + "\"";
                startInfo.Verb = "runas";
                Process p = Process.Start(startInfo);
                p.WaitForExit();
                return File.ReadAllText(tempFile);
            }
            catch (Win32Exception)
            {
                return RM.GetString("UserCancelsOperation");
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        internal static String ConstructQueryString(NameValueCollection parameters)
        {
            List<String> items = new List<String>();

            foreach (string name in parameters)
            {
                string value = parameters[name];
                if (value == null)
                    value = string.Empty;
                items.Add(string.Concat(name, "=", Uri.EscapeDataString(value)));
            }

            return String.Join("&", items.ToArray());
        }
    }
}