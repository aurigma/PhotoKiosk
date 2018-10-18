// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using Aurigma.PhotoKiosk.Core;
using Aurigma.PhotoKiosk.Core.OrderManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public partial class MainForm : Form
    {
        private Dictionary<string, BasePanel> _panelCollection = new Dictionary<string, BasePanel>();
        private Config _config;
        private PriceManager _priceManager;
        private OrderManager _orderManager;
        private CropManager _cropManager;
        private TreeNode _pricesGeneralNode;

        public MainForm()
        {
            InitializeComponent();

            _panelCollection.Add("BehaviourNode", _behaviourPanel);
            _panelCollection.Add("BehaviourGeneralNode", _behaviourPanel);
            _panelCollection.Add("BehaviourContactInfoNode", _contactInfoPanel);
            _panelCollection.Add("BehaviourCallbackNode", _callbackPanel);
            _panelCollection.Add("BluetoothNode", _bluetoothPanel);
            _panelCollection.Add("ImageEditorNode", _imageEditorPanel);
            _panelCollection.Add("ImageEditorGeneralNode", _imageEditorPanel);
            _panelCollection.Add("CropsNode", _cropsPanel);            
            _panelCollection.Add("AppearanceNode", _appearancePanel);
            _panelCollection.Add("AppearanceGeneralNode", _appearancePanel);
            _panelCollection.Add("PaperFormatsNode", _paperFormatsPanel);
            _panelCollection.Add("PaperTypesNode", _paperTypesPanel);
            _panelCollection.Add("PricesNode", _pricePanel);
            _panelCollection.Add("PricesGeneralNode", _pricePanel);
            _panelCollection.Add("ServicesNode", _servicesPanel);
            _panelCollection.Add("OrderManagerNode", _orderManagerPanel);
            _panelCollection.Add("OrderManagerGeneralNode", _orderManagerPanel);
            _panelCollection.Add("ArbitraryFormatNode", _arbitraryFormatPanel);
            _panelCollection.Add("DpofNode", _dpofPanel);
            _panelCollection.Add("PhotoPrinterNode", _photoPrinterPanel);
            _panelCollection.Add("CdBurnerNode", _cdBurnerPanel);
            _panelCollection.Add("ReceiptPrinterNode", _receiptPrinterPanel);

            _pricesGeneralNode = _optionsTree.Nodes["PricesNode"].Nodes["PricesGeneralNode"];

            try
            {
                _config = new Config(false);
            }
            catch (Exception e)
            {
                ShowErrorMessage(string.Format(RM.GetString("PhotoKioskConfigError"), e.Message));
                return;
            }

            try
            {
                _priceManager = new PriceManager(false, _config.PriceFile.Value, new PriceManager.ModifiedHandler(_settings_Modified));
            }
            catch (Exception e)
            {
                ShowErrorMessage(string.Format(RM.GetString("PriceReadingError"), e.Message));
                return;
            }

            try
            {
                _orderManager = new OrderManager();
            }
            catch (Exception e)
            {
                ShowErrorMessage(string.Format(RM.GetString("OrderManagerConfigError"), e.Message));
                return;
            }

            try
            {
                _cropManager = new CropManager(false, _config.CropFile.Value, new CropManager.ModifiedHandler(_settings_Modified));
            }
            catch (Exception e)
            {
                ShowErrorMessage((string.Format(RM.GetString("CropReadingError"), e.Message)));
                return;
            }

            _config.Modified += new Config.ModifiedHandler(_settings_Modified);
            _orderManager.Modified += new OrderManager.ModifiedHandler(_settings_Modified);

            foreach (BasePanel panel in _panelCollection.Values)
            {
                panel.Config = _config;
                panel.PriceManager = _priceManager;
                panel.OrderManager = _orderManager;
                panel.CropManager = _cropManager;
            }
            _screenPanel.Config = _config;
        }

        private void _settings_Modified()
        {
            _applyButton.Enabled = true;
        }

        public void UpdatePriceNodes()
        {
            _optionsTree.Nodes["PricesNode"].Nodes.Clear();
            _optionsTree.Nodes["PricesNode"].Nodes.Add(_pricesGeneralNode);

            foreach (PaperFormat format in _priceManager.PaperFormats)
            {
                foreach (PaperType type in _priceManager.PaperTypes)
                {
                    Product product;
                    if (_config.EnablePhotoOrdering.Value)
                    {
                        product = _priceManager.GetProduct(format.Name + type.Name);
                        if (product == null)
                            product = _priceManager.AddProduct(format, type, "");
                        _optionsTree.Nodes["PricesNode"].Nodes.Add(product.ToString(), format.Name + " " + type.Name);
                    }

                    if (_priceManager.ContainsProduct(format.Name + type.Name + Constants.InstantKey) && _config.EnablePhotoPrinting.Value)
                    {
                        product = _priceManager.GetProduct(format.Name + type.Name + Constants.InstantKey);
                        _optionsTree.Nodes["PricesNode"].Nodes.Add(product.ToString(), format.Name + " " + type.Name + RM.GetString("InstantPostfix"));
                    }
                }
            }
        }

        public void UpdateServiceNodes()
        {
            _optionsTree.Nodes["ServicesNode"].Nodes.Clear();
            foreach (Service service in _priceManager.Services)
            {
                _optionsTree.Nodes["ServicesNode"].Nodes.Add(service.Name, service.Name);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_config != null && _priceManager != null && _orderManager != null)
            {
                UpdatePriceNodes();
                UpdateServiceNodes();

                foreach (ScreenSetting screen in _config.Screens)
                    _optionsTree.Nodes["AppearanceNode"].Nodes.Add(screen.Key, RM.GetString(screen.Key));

                _applyButton.Enabled = false;
                _helpButton.Enabled = File.Exists(Config.HelpFile);
            }
            else
                Close();
        }

        private void _optionsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (BasePanel panel in _panelCollection.Values)
                panel.Visible = false;

            _screenPanel.Visible = false;
            _formatPricePanel.Visible = false;
            _serviceDetailsPanel.Visible = false;

            TreeNode node = (sender as TreeView).SelectedNode;
            if (_panelCollection.ContainsKey(node.Name))
            {
                _panelCollection[node.Name].Visible = true;
            }
            else if (node.Parent.Name == "PricesNode")
            {
                _formatPricePanel.Visible = true;
                _formatPricePanel.SetProduct(_priceManager.GetProduct(node.Name), node.Text);
            }
            else if (node.Parent.Name == "ServicesNode")
            {
                _serviceDetailsPanel.Visible = true;
                _serviceDetailsPanel.SetService(_priceManager.GetService(node.Name));
            }
            else if (node.Parent.Name == "AppearanceNode")
            {
                _screenPanel.Visible = true;
                _screenPanel.SetScreen(_config.GetScreen(node.Name));
            }
        }

        private void _okButton_Click(object sender, EventArgs e)
        {
            _applyButton_Click(null, null);
            Close();
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _applyButton_Click(object sender, EventArgs e)
        {
            _config.Save();
            _priceManager.Save(_config.PriceFile.Value);
            _orderManager.Save();
            _cropManager.Save(_config.CropFile.Value);
            _applyButton.Enabled = false;
        }

        private void _helpButton_Click(object sender, EventArgs e)
        {
            Process.Start(Config.HelpFile);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                _helpButton_Click(null, null);
        }

        public static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, RM.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, RM.GetString("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowInfoMessage(string message)
        {
            MessageBox.Show(message, RM.GetString("Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}