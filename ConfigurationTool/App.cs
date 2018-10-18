// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    internal static class App
    {
        internal static bool IsAdmin
        {
            get
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal p = new WindowsPrincipal(id);
                return p.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private static Image _uacShieldImage;

        private static Image UacShieldImage
        {
            get
            {
                if (_uacShieldImage == null)
                {
                    using (Image image = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Aurigma.PhotoKiosk.ConfigurationTool.UAC_shield.png")))
                    {
                        _uacShieldImage = ResizeImage(image, new Size(16, 16));
                    }
                }
                return _uacShieldImage;
            }
        }

        public static Image ResizeImage(Image image, Size size)
        {
            Bitmap bitmap = new Bitmap(size.Width, size.Height);

            using (Graphics g = Graphics.FromImage((Image)bitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, size.Width, size.Height);
            }

            return bitmap;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const uint BCM_SETSHIELD = 0x0000160C;

        internal static void SetUacShield(Button button)
        {
            if (IsAdmin)
                return;

            OperatingSystem osInfo = Environment.OSVersion;
            
            // Windows Vista or later
            if (osInfo.Version > new Version(6, 0)) 
            {
                button.FlatStyle = FlatStyle.System;
                IntPtr wParam = new IntPtr(0);
                IntPtr lParam = new IntPtr(1);
                SendMessage(button.Handle, BCM_SETSHIELD, wParam, lParam);
            }
        }
    }
}