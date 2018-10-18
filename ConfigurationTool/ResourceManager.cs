// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Reflection;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    internal class RM
    {
        private const string ApplicationName = "Aurigma.PhotoKiosk.ConfigurationTool";
        private static ApplicationResourceManager _resource;

        public static string GetString(string resourceName)
        {
            if (_resource == null)
                _resource = new ApplicationResourceManager(ApplicationName, Assembly.GetExecutingAssembly());

            return _resource.GetString(resourceName);
        }
    }
}