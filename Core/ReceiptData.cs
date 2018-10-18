// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Collections.Generic;

namespace Aurigma.PhotoKiosk.Core
{
    public class ReceiptData
    {
        public ReceiptData()
        {
            Formats = new List<FormatInfo>();
            Photos = new List<PhotoInfo>();
            Services = new List<ServiceInfo>();
        }

        public string Id { get; set; }
        public string OrderDate { get; set; }

        public string UserName { get; set; }
        public string UserPhone { get; set; }

        public string CropMode { get; set; }
        public string PaperType { get; set; }

        public int PhotosCount { get; set; }
        public int PrintsCount { get; set; }

        public string OrderCost { get; set; }
        public string SalesTax { get; set; }
        public string TotalCost { get; set; }

        public string Service { get; set; }

        public List<FormatInfo> Formats { get; private set; }
        public List<PhotoInfo> Photos { get; private set; }
        public List<ServiceInfo> Services { get; private set; }

        public bool MorePhotos { get; set; }
        public bool OrderPhotos { get; set; }
        public bool BurnCd { get; set; }

        public bool ShowSalesTax
        {
            get { return !string.IsNullOrEmpty(SalesTax); }
        }

        public bool ShowServices
        {
            get { return Services.Count > 0; }
        }
    }

    public class FormatInfo
    {
        public FormatInfo(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public string Name { get; private set; }
        public int Count { get; private set; }
    }

    public class PhotoInfo
    {
        public PhotoInfo(string filename, int count, string format)
        {
            Filename = filename;
            Count = count;
            Format = format;
        }

        public string Filename { get; private set; }
        public int Count { get; private set; }
        public string Format { get; private set; }
    }

    public class ServiceInfo
    {
        public ServiceInfo(string name, string cost)
        {
            Name = name;
            Cost = cost;
        }

        public string Name { get; private set; }
        public string Cost { get; private set; }
    }
}