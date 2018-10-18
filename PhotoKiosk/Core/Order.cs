// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using System.Collections.Generic;

namespace Aurigma.PhotoKiosk
{
    public class Order
    {
        private List<OrderItem> _items = new List<OrderItem>();
        private List<Service> _services = new List<Service>(ExecutionEngine.PriceManager.Services.Count);
        private PaperType _paperType;
        private string _cropMode;
        private string _orderId;
        private string _userName;
        private string _userPhone;
        private string _userEmail;

        public Order()
        {
            ExecutionEngine.EventLogger.Write("Order created");
            Reset();
        }

        public float GetTotalCost()
        {
            float orderCost = GetCost();
            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.OrderPhotos)
            {
                foreach (Service service in _services)
                    orderCost += GetCost(service);
            }

            return orderCost;
        }

        public float GetCost()
        {
            if (ExecutionEngine.Instance.PrimaryAction == PrimaryActionType.BurnCd)
            {
                return ExecutionEngine.Config.CDBurningCost.Value;
            }
            else
            {
                float orderCost = 0f;
                foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
                    orderCost += GetCost(format);
                return orderCost;
            }
        }

        public float GetCost(PaperFormat format)
        {
            return ExecutionEngine.PriceManager.GetTotalPrice(format, _paperType, ExecutionEngine.Instance.Instant, GetItemCount(format));
        }

        public float GetCost(Service service)
        {
            if (service.IsPriceFixed)
                return service.Price;
            else
                return (GetCost() * service.Price) / 100.0f;
        }

        public int GetItemCount()
        {
            int itemsCount = 0;
            foreach (PaperFormat format in ExecutionEngine.Instance.PaperFormats)
            {
                itemsCount += GetItemCount(format);
            }

            return itemsCount;
        }

        public int GetItemCount(PaperFormat format)
        {
            int itemsCount = 0;
            foreach (OrderItem item in _items)
            {
                itemsCount += item.GetCount(format);
            }

            return itemsCount;
        }

        public void Reset()
        {
            _items.Clear();
            _services.Clear();
            foreach (Service service in ExecutionEngine.PriceManager.Services)
            {
                if (service.IsPermanent)
                    _services.Add(service);
            }

            _orderId = ExecutionEngine.Config.PhotoKioskId.Value + "0";
            _paperType = ExecutionEngine.Instance.PaperTypes.Count > 0 ? ExecutionEngine.Instance.PaperTypes[0] : null;
            _cropMode = Constants.CropToFillModeName;

            _userName = "";
            _userPhone = "";
            _userEmail = "";
        }

        public List<OrderItem> OrderItems
        {
            get { return _items; }
        }

        public List<Service> Services
        {
            get { return _services; }
        }

        public string OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        public PaperType OrderPaperType
        {
            get { return _paperType; }
            set { _paperType = value; }
        }

        public string CropMode
        {
            get { return _cropMode; }
            set { _cropMode = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string UserPhone
        {
            get { return _userPhone; }
            set { _userPhone = value; }
        }

        public string UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value; }
        }
    }
}