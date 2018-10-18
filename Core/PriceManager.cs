// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Aurigma.PhotoKiosk.Core
{
    public class PaperFormat
    {
        private readonly bool _isReadonly;

        private string _name;
        private float _width;
        private float _height;
        private int _dpi;
        private PriceManager.ModifiedHandler _handler;

        public PaperFormat(bool isReadonly, string name, float width, float height, int dpi, PriceManager.ModifiedHandler handler)
        {
            _isReadonly = isReadonly;
            _name = name;
            _width = width;
            _height = height;
            _dpi = dpi;
            _handler = handler;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_name != value)
                    {
                        _name = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_width != value)
                    {
                        _width = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_height != value)
                    {
                        _height = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public int Dpi
        {
            get
            {
                return _dpi;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_dpi != value)
                    {
                        _dpi = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public bool IsFree
        {
            get
            {
                return (_width == 0 && _height == 0 && _dpi == 0);
            }
        }
    }

    public class PaperType
    {
        private readonly bool _isReadonly;
        private string _name;
        private string _description;
        private PriceManager.ModifiedHandler _handler;

        internal PaperType(bool isReadonly, string name, string description, PriceManager.ModifiedHandler handler)
        {
            _isReadonly = isReadonly;
            _name = name;
            _description = description;
            _handler = handler;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_name != value)
                    {
                        _name = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_description != value)
                    {
                        _description = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new InvalidOperationException();
            }
        }
    }

    public class Discount
    {
        private int _start;
        private int _end;
        private float _price;

        public Discount(int start, int end, float price)
        {
            _start = start;
            _end = end;
            _price = price;
        }

        public int Start
        {
            get { return _start; }
        }

        public int End
        {
            get { return _end; }
        }

        public float Price
        {
            get { return _price; }
        }
    }

    public class Product
    {
        private readonly bool _isReadonly;
        private PaperFormat _format;
        private PaperType _type;
        private string _printer;
        private string _channel;
        private float _price;
        private List<Discount> _discounts = new List<Discount>();
        private PriceManager.ModifiedHandler _handler;

        internal Product(bool isReadonly, PaperFormat format, PaperType type, string printer, string channel, float price, PriceManager.ModifiedHandler handler)
        {
            _isReadonly = isReadonly;
            _format = format;
            _type = type;
            _printer = printer;
            _channel = channel;
            _price = price;
            _handler = handler;
        }

        public override string ToString()
        {
            return _format.Name + _type.Name + ((!string.IsNullOrEmpty(_printer) && !_format.IsFree) ? Constants.InstantKey : "");
        }

        public PaperFormat PaperFormat
        {
            get { return _format; }
        }

        public PaperType PaperType
        {
            get { return _type; }
        }

        public string Printer
        {
            get
            {
                return _printer;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_printer != value)
                    {
                        _printer = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new NotSupportedException();
            }
        }

        public string Channel
        {
            get
            {
                return _channel;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_channel != value)
                    {
                        _channel = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new NotSupportedException();
            }
        }

        public float Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_price != value)
                    {
                        _price = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new NotSupportedException();
            }
        }

        public IList<Discount> Discounts
        {
            get
            {
                return _discounts;
            }
        }

        public void AddDiscount(Discount discount)
        {
            if (!_isReadonly)
            {
                _discounts.Add(discount);
                if (_handler != null)
                    _handler();
            }
            else
                throw new NotSupportedException();
        }

        internal float GetDiscountPrice(int count)
        {
            foreach (Discount discount in _discounts)
            {
                if (count >= discount.Start && count <= discount.End)
                    return discount.Price;
            }

            return _price;
        }
    }

    public class Service
    {
        private readonly bool _isReadonly;
        private string _name;
        private string _description;
        private float _price;
        private bool _isPriceFixed;
        private bool _isPermanent;
        private PriceManager.ModifiedHandler _handler;

        public Service(bool isReadonly, string name, string description, float price, bool isPriceFixed, bool isPermanent, PriceManager.ModifiedHandler handler)
        {
            _isReadonly = isReadonly;
            _name = name;
            _description = description;
            _price = price;
            _isPriceFixed = isPriceFixed;
            _isPermanent = isPermanent;
            _handler = handler;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_name != value)
                    {
                        _name = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_description != value)
                    {
                        _description = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public float Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_price != value)
                    {
                        _price = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public bool IsPriceFixed
        {
            get
            {
                return _isPriceFixed;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_isPriceFixed != value)
                    {
                        _isPriceFixed = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public bool IsPermanent
        {
            get
            {
                return _isPermanent;
            }
            set
            {
                if (!_isReadonly)
                {
                    if (_isPermanent != value)
                    {
                        _isPermanent = value;
                        if (_handler != null)
                            _handler();
                    }
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public string GetPriceString()
        {
            if (_isPriceFixed)
                return _price.ToString("c", NumberFormatInfo.CurrentInfo);
            else
                return string.Format("{0}%", _price);
        }
    }

    public class PriceManager
    {
        private readonly bool _isReadonly;
        private string _filename;
        private Dictionary<string, Product> _products = new Dictionary<string, Product>();
        private List<Service> _services = new List<Service>();

        private List<PaperFormat> _formats = new List<PaperFormat>();
        private List<PaperFormat> _minilabFormats = new List<PaperFormat>();
        private List<PaperFormat> _instantFormats = new List<PaperFormat>();

        private List<PaperType> _types = new List<PaperType>();
        private List<PaperType> _minilabTypes = new List<PaperType>();
        private List<PaperType> _instantTypes = new List<PaperType>();

        public readonly Setting<float> SalesTaxPercent;
        public readonly Setting<string> SalesTaxComment;
        public readonly Setting<float> MinimumCost;

        public delegate void ModifiedHandler();

        public event ModifiedHandler Modified;

        public delegate void UpdatedHandler();

        public event UpdatedHandler Updated;

        public PriceManager(bool isReadonly, string filename, ModifiedHandler handler)
        {
            _isReadonly = isReadonly;
            _filename = filename;

            SalesTaxPercent = new Setting<float>(isReadonly, "SalesTaxPercent", 0.0f, true, new Setting<float>.SettingChangedHandler(SettingValueChanged));
            SalesTaxComment = new Setting<string>(isReadonly, "SalesTaxComment", "", true, new Setting<string>.SettingChangedHandler(SettingValueChanged));
            MinimumCost = new Setting<float>(isReadonly, "MinimumCost", 0.0f, true, new Setting<float>.SettingChangedHandler(SettingValueChanged));

            try
            {
                Load(_filename, handler);
            }
            catch (FileNotFoundException)
            {
                Config.RestoreDefaultPriceFile(_filename);
                Load(_filename, handler);
            }
            catch (Exception e)
            {
                throw e;
            }

            Modified += handler;
        }

        public PriceManager(bool isReadonly, string filename)
            : this(isReadonly, filename, null)
        {
        }

        private void SettingValueChanged(object sender)
        {
            if (Modified != null)
                Modified();
        }

        private void Load(string filename, ModifiedHandler handler)
        {
            _products.Clear();
            _services.Clear();

            _formats.Clear();
            _minilabFormats.Clear();
            _instantFormats.Clear();

            _types.Clear();
            _minilabTypes.Clear();
            _instantTypes.Clear();

            var settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;

            using (var reader = XmlReader.Create(filename, settings))
            {
                reader.ReadStartElement("price");
                if (reader.Name == "settings" && !reader.IsEmptyElement)
                {
                    reader.Read();
                    if (reader.Name == "setting" && reader.GetAttribute("key") == SalesTaxPercent.Key)
                        SalesTaxPercent.Init(float.Parse(reader.GetAttribute("value"), CultureInfo.InvariantCulture));

                    reader.Read();
                    if (reader.Name == "setting" && reader.GetAttribute("key") == SalesTaxComment.Key)
                        SalesTaxComment.Init(reader.GetAttribute("value"));

                    reader.Read();
                    if (reader.Name == "setting" && reader.GetAttribute("key") == MinimumCost.Key)
                        MinimumCost.Init(float.Parse(reader.GetAttribute("value"), CultureInfo.InvariantCulture));

                    reader.Read();
                    reader.ReadEndElement();
                }

                if (reader.Name == "formats" && !reader.IsEmptyElement)
                {
                    reader.Read();
                    while (reader.Name == "format")
                    {
                        PaperFormat format = new PaperFormat(
                                _isReadonly,
                                reader.GetAttribute("name"),
                                float.Parse(reader.GetAttribute("width"), CultureInfo.InvariantCulture),
                                float.Parse(reader.GetAttribute("height"), CultureInfo.InvariantCulture),
                                int.Parse(reader.GetAttribute("dpi"), CultureInfo.InvariantCulture),
                                handler);

                        if (!ContainsPaperFormat(format.Name))
                            _formats.Add(format);

                        reader.Read();
                    }
                    reader.ReadEndElement();
                }

                if (reader.Name == "types" && !reader.IsEmptyElement)
                {
                    reader.Read();
                    while (reader.Name == "type")
                    {
                        PaperType type = new PaperType(_isReadonly, reader.GetAttribute("name"), reader.GetAttribute("description"), handler);

                        if (!ContainsPaperType(type.Name))
                            _types.Add(type);

                        reader.Read();
                    }
                    reader.ReadEndElement();
                }

                if (reader.Name == "products" && !reader.IsEmptyElement)
                {
                    reader.Read();
                    while (reader.Name == "product")
                    {
                        PaperFormat format = GetPaperFormat(reader.GetAttribute("format"));
                        PaperType type = GetPaperType(reader.GetAttribute("type"));
                        string printer = reader.GetAttribute("printer");
                        string channel = reader.GetAttribute("channel");

                        if (format != null && type != null && !_products.ContainsKey(format.Name + type.Name + (!string.IsNullOrEmpty(printer) ? Constants.InstantKey : "")))
                        {
                            var product = new Product(_isReadonly, format, type, printer, channel, float.Parse(reader.GetAttribute("price"), CultureInfo.InvariantCulture), handler);

                            if (string.IsNullOrEmpty(printer) || format.IsFree)
                            {
                                if (!_minilabFormats.Contains(format))
                                    _minilabFormats.Add(format);

                                if (!_minilabTypes.Contains(type))
                                    _minilabTypes.Add(type);
                            }
                            else
                            {
                                if (!_instantFormats.Contains(format))
                                    _instantFormats.Add(format);

                                if (!_instantTypes.Contains(type))
                                    _instantTypes.Add(type);
                            }

                            reader.Read();
                            if (reader.Name == "discounts" && !reader.IsEmptyElement)
                            {
                                reader.Read();
                                while (reader.Name == "discount")
                                {
                                    product.Discounts.Add(new Discount(
                                        int.Parse(reader.GetAttribute("start"), CultureInfo.InvariantCulture),
                                        string.IsNullOrEmpty(reader.GetAttribute("end")) ? int.MaxValue : int.Parse(reader.GetAttribute("end"), CultureInfo.InvariantCulture),
                                        float.Parse(reader.GetAttribute("price"), CultureInfo.InvariantCulture)));

                                    reader.Read();
                                }
                                reader.ReadEndElement();
                            }
                            _products.Add(product.ToString(), product);

                            reader.Read();
                        }
                        else
                            reader.Skip();
                    }
                    reader.ReadEndElement();
                }

                if (reader.Name == "services" && !reader.IsEmptyElement)
                {
                    reader.Read();
                    while (reader.Name == "service")
                    {
                        Service service = new Service(
                            _isReadonly,
                            reader.GetAttribute("name"),
                            reader.GetAttribute("description"),
                            float.Parse(reader.GetAttribute("price"), CultureInfo.InvariantCulture),
                            bool.Parse(reader.GetAttribute("fixed")),
                            bool.Parse(reader.GetAttribute("permanent")),
                            handler);

                        if (!ContainsService(service.Name))
                            _services.Add(service);

                        reader.Read();
                    }
                    reader.ReadEndElement();
                }
            }
        }

        public string FileName
        {
            get { return _filename; }
            set
            {
                if (!_isReadonly)
                {
                    if (_filename != value && File.Exists(value))
                    {
                        try
                        {
                            Load(value, Modified);
                            _filename = value;

                            if (Updated != null)
                                Updated();
                        }
                        catch
                        {
                            Load(_filename, Modified);
                        }
                    }
                }
                else
                    throw new NotSupportedException("PriceManager is readonly, so FileName cannot be changed.");
            }
        }

        public void Save(string filename)
        {
            if (_isReadonly)
                throw new NotSupportedException("PriceManager is readonly, so it cannot be saved.");

            try
            {
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = false;
                settings.Encoding = Encoding.UTF8;

                var stream = new MemoryStream(4096);
                try
                {
                    using (var writer = XmlWriter.Create(stream, settings))
                    {
                        writer.WriteStartElement("price");

                        writer.WriteStartElement("settings");

                        // SalesTaxPercent
                        writer.WriteStartElement("setting");
                        writer.WriteAttributeString("key", SalesTaxPercent.Key);
                        writer.WriteAttributeString("value", SalesTaxPercent.Value.ToString(CultureInfo.InvariantCulture));
                        writer.WriteEndElement();

                        // SalesTaxComment
                        writer.WriteStartElement("setting");
                        writer.WriteAttributeString("key", SalesTaxComment.Key);
                        writer.WriteAttributeString("value", SalesTaxComment.Value);
                        writer.WriteEndElement();

                        // MinimumCost
                        writer.WriteStartElement("setting");
                        writer.WriteAttributeString("key", MinimumCost.Key);
                        writer.WriteAttributeString("value", MinimumCost.Value.ToString(CultureInfo.InvariantCulture));
                        writer.WriteEndElement();
                        writer.WriteEndElement();

                        writer.WriteStartElement("formats");
                        foreach (PaperFormat format in _formats)
                        {
                            writer.WriteStartElement("format");
                            writer.WriteAttributeString("name", format.Name);
                            writer.WriteAttributeString("width", format.Width.ToString(CultureInfo.InvariantCulture));
                            writer.WriteAttributeString("height", format.Height.ToString(CultureInfo.InvariantCulture));
                            writer.WriteAttributeString("dpi", format.Dpi.ToString(CultureInfo.InvariantCulture));
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();

                        writer.WriteStartElement("types");
                        foreach (PaperType type in _types)
                        {
                            writer.WriteStartElement("type");
                            writer.WriteAttributeString("name", type.Name);
                            writer.WriteAttributeString("description", type.Description);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();

                        writer.WriteStartElement("products");
                        foreach (PaperFormat format in _formats)
                        {
                            foreach (PaperType type in _types)
                            {
                                SaveProduct(writer, GetProduct(format.Name + type.Name));
                                SaveProduct(writer, GetProduct(format.Name + type.Name + Constants.InstantKey));
                            }
                        }
                        writer.WriteEndElement();

                        writer.WriteStartElement("services");
                        foreach (Service service in _services)
                        {
                            writer.WriteStartElement("service");
                            writer.WriteAttributeString("name", service.Name);
                            writer.WriteAttributeString("description", service.Description);
                            writer.WriteAttributeString("price", service.Price.ToString(CultureInfo.InvariantCulture));
                            writer.WriteAttributeString("fixed", service.IsPriceFixed.ToString());
                            writer.WriteAttributeString("permanent", service.IsPermanent.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                    }
                }
                catch
                {
                    stream.Close();
                    throw;
                }

                Tools.SaveStreamToFile(stream, filename);
                stream.Close();
            }
            catch (Exception)
            {
            }

            _filename = filename;
        }

        private void SaveProduct(XmlWriter writer, Product product)
        {
            if (product != null)
            {
                writer.WriteStartElement("product");
                writer.WriteAttributeString("format", product.PaperFormat.Name);
                writer.WriteAttributeString("type", product.PaperType.Name);
                if (!string.IsNullOrEmpty(product.Printer))
                    writer.WriteAttributeString("printer", product.Printer);
                if (!string.IsNullOrEmpty(product.Channel))
                    writer.WriteAttributeString("channel", product.Channel);
                writer.WriteAttributeString("price", product.Price.ToString(CultureInfo.InvariantCulture));

                if (product.Discounts.Count > 0)
                {
                    writer.WriteStartElement("discounts");
                    foreach (Discount discount in product.Discounts)
                    {
                        writer.WriteStartElement("discount");
                        writer.WriteAttributeString("start", discount.Start.ToString(CultureInfo.InvariantCulture));
                        if (discount.End < int.MaxValue)
                            writer.WriteAttributeString("end", discount.End.ToString(CultureInfo.InvariantCulture));
                        writer.WriteAttributeString("price", discount.Price.ToString(CultureInfo.InvariantCulture));
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }

        #region PaperFormat methods and properties

        public IList<PaperFormat> PaperFormats
        {
            get
            {
                if (_isReadonly)
                    return _formats.AsReadOnly();
                else
                    return _formats;
            }
        }

        public IList<PaperFormat> MinilabPaperFormats
        {
            get { return _minilabFormats.AsReadOnly(); }
        }

        public IList<PaperFormat> InstantPaperFormats
        {
            get { return _instantFormats.AsReadOnly(); }
        }

        public PaperFormat GetPaperFormat(string name)
        {
            foreach (PaperFormat format in _formats)
            {
                if (format.Name == name)
                    return format;
            }
            return null;
        }

        public PaperFormat AddPaperFormat(string name, float width, float height, int dpi)
        {
            if (!_isReadonly)
            {
                PaperFormat format = new PaperFormat(_isReadonly, name, width, height, dpi, Modified);
                _formats.Add(format);
                if (Modified != null)
                    Modified();
                return format;
            }
            else
                throw new NotSupportedException();
        }

        public void RemovePaperFormat(PaperFormat format)
        {
            if (!_isReadonly)
            {
                if (format != null && _formats.Contains(format))
                {
                    _formats.Remove(format);
                    if (Modified != null)
                        Modified();
                }
            }
            else
                throw new NotSupportedException();
        }

        public void UpdatePaperFormats(PaperFormat[] formats)
        {
            _formats.Clear();
            foreach (PaperFormat format in formats)
            {
                _formats.Add(format);
            }
            if (Modified != null)
                Modified();
        }

        private bool ContainsPaperFormat(string name)
        {
            return GetPaperFormat(name) != null;
        }

        #endregion PaperFormat methods and properties

        #region PaperType methods and props

        public IList<PaperType> PaperTypes
        {
            get
            {
                if (_isReadonly)
                    return _types.AsReadOnly();
                else
                    return _types;
            }
        }

        public IList<PaperType> MinilabPaperTypes
        {
            get { return _minilabTypes.AsReadOnly(); }
        }

        public IList<PaperType> InstantPaperTypes
        {
            get { return _instantTypes.AsReadOnly(); }
        }

        public PaperType GetPaperType(string name)
        {
            foreach (PaperType type in _types)
            {
                if (type.Name == name)
                    return type;
            }
            return null;
        }

        public PaperType AddPaperType(string name, string description)
        {
            if (!_isReadonly)
            {
                PaperType type = new PaperType(_isReadonly, name, description, Modified);
                _types.Add(type);
                if (Modified != null)
                    Modified();

                return type;
            }
            else
                throw new NotSupportedException();
        }

        public void RemovePaperType(PaperType type)
        {
            if (!_isReadonly)
            {
                if (type != null && _types.Contains(type))
                {
                    _types.Remove(type);
                    if (Modified != null)
                        Modified();
                }
            }
            else
                throw new NotSupportedException();
        }

        public void UpdatePaperTypes(PaperType[] types)
        {
            _types.Clear();
            foreach (PaperType type in types)
            {
                _types.Add(type);
            }
            if (Modified != null)
                Modified();
        }

        private bool ContainsPaperType(string name)
        {
            return GetPaperType(name) != null;
        }

        #endregion PaperType methods and props

        #region Product methods and properties

        public Product GetProduct(string key)
        {
            if (_products.ContainsKey(key))
                return _products[key];
            else
                return null;
        }

        public Product AddProduct(PaperFormat format, PaperType type, string printer)
        {
            if (!_isReadonly)
            {
                Product newProduct = new Product(_isReadonly, format, type, printer, null, 1, Modified);
                newProduct.AddDiscount(new Discount(1, int.MaxValue, 1));
                _products.Add(newProduct.ToString(), newProduct);
                if (Modified != null)
                    Modified();
                return newProduct;
            }
            else
                throw new NotSupportedException();
        }

        public void RemoveProduct(string key)
        {
            if (!_isReadonly)
            {
                if (_products.ContainsKey(key))
                {
                    _products.Remove(key);
                    if (Modified != null)
                        Modified();
                }
            }
            else
                throw new NotSupportedException();
        }

        public bool ContainsProduct(string key)
        {
            return _products.ContainsKey(key);
        }

        public float GetTotalPrice(PaperFormat format, PaperType type, string instant, int count)
        {
            return _products[format.Name + type.Name + instant].GetDiscountPrice(count) * count;
        }

        public float GetBasePrice(PaperFormat format, PaperType type, string instant)
        {
            return _products[format.Name + type.Name + instant].Price;
        }

        public float GetDiscountPrice(PaperFormat format, PaperType type, string instant, int count)
        {
            return _products[format.Name + type.Name + instant].GetDiscountPrice(count);
        }

        #endregion Product methods and properties

        #region Service methods and properties

        public IList<Service> Services
        {
            get
            {
                if (_isReadonly)
                    return _services.AsReadOnly();
                else
                    return _services;
            }
        }

        public Service GetService(string name)
        {
            foreach (Service service in _services)
            {
                if (service.Name == name)
                    return service;
            }

            return null;
        }

        public Service AddService(string name)
        {
            if (!_isReadonly)
            {
                Service newService = new Service(_isReadonly, name, "", 1.0f, true, true, Modified);
                _services.Add(newService);
                if (Modified != null)
                    Modified();

                return newService;
            }
            else
                throw new NotSupportedException();
        }

        public void RemoveService(Service service)
        {
            if (!_isReadonly)
            {
                if (service != null && _services.Contains(service))
                {
                    _services.Remove(service);
                    if (Modified != null)
                        Modified();
                }
            }
            else
                throw new NotSupportedException();
        }

        public void UpdateServices(Service[] services)
        {
            _services.Clear();
            foreach (Service servie in services)
            {
                _services.Add(servie);
            }
            if (Modified != null)
                Modified();
        }

        public bool HasServicesToChoose()
        {
            foreach (Service service in _services)
            {
                if (!service.IsPermanent)
                    return true;
            }
            return false;
        }

        private bool ContainsService(string name)
        {
            return GetService(name) != null;
        }

        #endregion Service methods and properties
    }
}