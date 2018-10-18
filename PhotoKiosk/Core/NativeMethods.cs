// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
namespace Aurigma.PhotoKiosk
{
    internal class NativeMethods
    {
        private NativeMethods()
        {
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        internal static extern bool DeleteObject(System.IntPtr hObject);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern uint RegisterWindowMessage(string str);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        internal static extern uint SetErrorMode(uint uMode);

        internal const int SEM_FAILCRITICALERRORS = 1;
    }
}