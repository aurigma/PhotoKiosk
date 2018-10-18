// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Globalization;
using System.Reflection;

namespace Aurigma.PhotoKiosk.Core
{
    public sealed class ApplicationResourceManager
    {
        private string _applicationName;
        private Assembly _assembly;

        #region Instance methods and data

        ///// <summary>
        ///// Private constructor
        ///// </summary>
        public ApplicationResourceManager(string applicationFullName, Assembly assembly)
        {
            _applicationName = applicationFullName;
            _assembly = assembly;
            _rm = new System.Resources.ResourceManager(applicationFullName + ".Properties.Resources", assembly);
        }

        /// <summary>
        /// Return the resource manager for the assembly
        /// </summary>
        private System.Resources.ResourceManager Resources
        {
            get { return _rm; }
        }

        /// <summary>
        /// Store the resource manager
        /// </summary>
        private System.Resources.ResourceManager _rm;

        #endregion Instance methods and data

        #region Static methods and data

        /// <summary>
        /// Return the static loader instance
        /// </summary>
        /// <returns></returns>
        private ApplicationResourceManager GetLoader()
        {
            if (null == _loader)
            {
                lock (_lock)
                {
                    if (null == _loader)
                        _loader = new ApplicationResourceManager(_applicationName, _assembly);
                }
            }

            return _loader;
        }

        /// <summary>
        /// Get a string resource
        /// </summary>
        /// <param name="name">The resource name</param>
        /// <returns>The localized resource</returns>
        public string GetString(string name)
        {
            ApplicationResourceManager loader = GetLoader();
            string localized = null;

            if (null != loader)
                localized = loader.Resources.GetString(name, Config.Localization);

            return localized;
        }

        /// <summary>
        /// Get the localized string for a particular culture
        /// </summary>
        /// <param name="culture">The culture for which the string is desired</param>
        /// <param name="name">The resource name</param>
        /// <returns>The localized resource</returns>
        public string GetString(CultureInfo culture, string name)
        {
            ApplicationResourceManager loader = GetLoader();
            string localized = null;

            if (null != loader)
                localized = loader.Resources.GetString(name, culture ?? Config.Localization);

            return localized;
        }

        /// <summary>
        /// Cache the one and only instance of the loader
        /// </summary>
        private ApplicationResourceManager _loader = null;

        /// <summary>
        /// Object used to lock
        /// </summary>
        private static object _lock = new object();

        #endregion Static methods and data
    }

    internal class RM
    {
        private const string ApplicationName = "Aurigma.PhotoKiosk.Core";
        private static ApplicationResourceManager _resource;

        public static string GetString(string resourceName)
        {
            if (_resource == null)
                _resource = new ApplicationResourceManager(ApplicationName, Assembly.GetExecutingAssembly());

            return _resource.GetString(resourceName);
        }
    }
}