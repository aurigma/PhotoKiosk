// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Resources;

namespace Aurigma.PhotoKiosk
{
    internal sealed class StringResources
    {
        private StringResources()
        {
        }

        public static string GetString(string name)
        {
            ResourceManager resourceManager = new ResourceManager("Aurigma.PhotoKiosk.Properties.Resources", typeof(StringResources).Assembly);
            return resourceManager.GetString(name);
        }
    }
}