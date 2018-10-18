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
    public class CropFormat
    {
        private readonly bool _isReadonly;
        private string _name;
        private float _width;
        private float _height;
        private CropManager.ModifiedHandler _handler;

        public CropFormat(bool isReadonly, string name, float width, float height, CropManager.ModifiedHandler handler)
        {
            _isReadonly = isReadonly;
            _name = name;
            _width = width;
            _height = height;
            _handler = handler;
        }

        public string Name
        {
            get { return _name; }
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
            get { return _width; }
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
            get { return _height; }
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
    }

    public class CropManager
    {
        private readonly bool _isReadonly;
        private string _filename;
        private List<CropFormat> _crops = new List<CropFormat>();

        public delegate void ModifiedHandler();

        public event ModifiedHandler Modified;

        public delegate void UpdatedHandler();

        public event UpdatedHandler Updated;

        public CropManager(bool isReadonly, string filename, ModifiedHandler handler)
        {
            _isReadonly = isReadonly;
            _filename = filename;

            try
            {
                Load(_filename, handler);
            }
            catch (FileNotFoundException)
            {
                Config.RestoreDefaultCropsFile(_filename);
                Load(_filename, handler);
            }
            catch (Exception e)
            {
                throw e;
            }

            Modified += handler;
        }

        public CropManager(bool isReadonly, string filename)
            : this(isReadonly, filename, null)
        {
        }

        public IList<CropFormat> CropFormats
        {
            get
            {
                if (_isReadonly)
                    return _crops.AsReadOnly();
                else
                    return _crops;
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
                    throw new NotSupportedException("CropManager is readonly, so FileName cannot be changed.");
            }
        }

        private void Load(string filename, ModifiedHandler handler)
        {
            _crops.Clear();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(filename, settings))
            {
                reader.Read();
                reader.Read();
                if (reader.Name == "crops" && !reader.IsEmptyElement)
                {
                    reader.Read();
                    while (reader.IsStartElement() && reader.Name == "crop")
                    {
                        reader.Read();

                        String name = "";
                        float width = 0;
                        float height = 0;

                        for (int i = 0; i < 3; i++)
                        {
                            switch (reader.Name)
                            {
                                case "name":
                                    name = reader.ReadElementString();
                                    break;

                                case "width":
                                    width = float.Parse(reader.ReadElementString(), CultureInfo.InvariantCulture);
                                    break;

                                case "height":
                                    height = float.Parse(reader.ReadElementString(), CultureInfo.InvariantCulture);
                                    break;
                            }
                        }
                        reader.ReadEndElement();

                        if (name != null && !ContainsCrop(name, width, height) && _crops.Count < Constants.MaxCropsCount)
                            _crops.Add(new CropFormat(_isReadonly, name, width, height, handler));
                    }

                    reader.ReadEndElement();
                }
            }
        }

        public bool ContainsCrop(string name, float width, float height)
        {
            foreach (CropFormat crop in _crops)
            {
                if (crop.Name == name && crop.Width == width && crop.Height == height)
                    return true;
            }
            return false;
        }

        public CropFormat AddCropFormat(string name, float width, float height, bool update)
        {
            if (!_isReadonly)
            {
                if (!ContainsCrop(name, width, height) && _crops.Count < Constants.MaxCropsCount)
                {
                    CropFormat format = new CropFormat(_isReadonly, name, width, height, Modified);
                    _crops.Add(format);
                    if (Modified != null)
                        Modified();

                    if (update && Updated != null)
                        Updated();
                    return format;
                }
                return null;
            }
            else
                throw new NotSupportedException();
        }

        public void RemoveCropFormat(CropFormat format)
        {
            if (!_isReadonly)
            {
                if (format != null && _crops.Contains(format))
                {
                    _crops.Remove(format);
                    if (Modified != null)
                        Modified();
                }
            }
            else
                throw new NotSupportedException();
        }

        public void UpdateCropFormats(CropFormat[] formats)
        {
            _crops.Clear();
            foreach (CropFormat format in formats)
            {
                _crops.Add(format);
            }
            if (Modified != null)
                Modified();
        }

        public void Save(string fileName)
        {
            if (_isReadonly)
                throw new NotSupportedException("CropManager is readonly, so it cannot be saved.");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = false;
            settings.Encoding = Encoding.UTF8;

            MemoryStream interimStream = new MemoryStream(4096);
            try
            {
                XmlWriter writer = XmlWriter.Create(interimStream, settings);

                writer.WriteStartElement("crops");

                foreach (CropFormat cropStruct in _crops)
                {
                    writer.WriteStartElement("crop");
                    writer.WriteElementString("name", cropStruct.Name);
                    writer.WriteElementString("width", cropStruct.Width.ToString());
                    writer.WriteElementString("height", cropStruct.Height.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Close();
            }
            catch
            {
                interimStream.Close();
                throw;
            }

            Tools.SaveStreamToFile(interimStream, fileName);
            interimStream.Close();

            _filename = fileName;
        }
    }
}