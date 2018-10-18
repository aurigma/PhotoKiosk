namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._optionsTree = new System.Windows.Forms.TreeView();
            this._separator = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this._applyButton = new System.Windows.Forms.Button();
            this._helpButton = new System.Windows.Forms.Button();
            this._behaviourPanel = new Aurigma.PhotoKiosk.ConfigurationTool.BehaviourPanel();
            this._servicesPanel = new Aurigma.PhotoKiosk.ConfigurationTool.ServicesPanel();
            this._serviceDetailsPanel = new Aurigma.PhotoKiosk.ConfigurationTool.ServiceDetailsPanel();
            this._screenPanel = new Aurigma.PhotoKiosk.ConfigurationTool.ScreenPanel();
            this._receiptPrinterPanel = new Aurigma.PhotoKiosk.ConfigurationTool.ReceiptPrinterPanel();
            this._pricePanel = new Aurigma.PhotoKiosk.ConfigurationTool.PricePanel();
            this._photoPrinterPanel = new Aurigma.PhotoKiosk.ConfigurationTool.PhotoPrinterPanel();
            this._paperTypesPanel = new Aurigma.PhotoKiosk.ConfigurationTool.PaperTypesPanel();
            this._paperFormatsPanel = new Aurigma.PhotoKiosk.ConfigurationTool.PaperFormatsPanel();
            this._cropsPanel = new Aurigma.PhotoKiosk.ConfigurationTool.Panels.CropsPanel();
            this._orderManagerPanel = new Aurigma.PhotoKiosk.ConfigurationTool.OrderManagerPanel();
            this._imageEditorPanel = new Aurigma.PhotoKiosk.ConfigurationTool.ImageEditorPanel();
            this._formatPricePanel = new Aurigma.PhotoKiosk.ConfigurationTool.FormatPricePanel();
            this._dpofPanel = new Aurigma.PhotoKiosk.ConfigurationTool.DpofPanel();
            this._contactInfoPanel = new Aurigma.PhotoKiosk.ConfigurationTool.ContactInfoPanel();
            this._cdBurnerPanel = new Aurigma.PhotoKiosk.ConfigurationTool.CdBurnerPanel();
            this._callbackPanel = new Aurigma.PhotoKiosk.ConfigurationTool.CallbackPanel();
            this._bluetoothPanel = new Aurigma.PhotoKiosk.ConfigurationTool.BluetoothPanel();
            this._arbitraryFormatPanel = new Aurigma.PhotoKiosk.ConfigurationTool.ArbitraryFormatPanel();
            this._appearancePanel = new Aurigma.PhotoKiosk.ConfigurationTool.AppearancePanel();
            this.SuspendLayout();
            // 
            // _optionsTree
            // 
            resources.ApplyResources(this._optionsTree, "_optionsTree");
            this._optionsTree.Name = "_optionsTree";
            this._optionsTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes1"))),
            //((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes2"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes3"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes4"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes5"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes6"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes7"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes8"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes9"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes10"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("_optionsTree.Nodes11")))});
            this._optionsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._optionsTree_AfterSelect);
            // 
            // _separator
            // 
            resources.ApplyResources(this._separator, "_separator");
            this._separator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._separator.Name = "_separator";
            // 
            // _cancelButton
            // 
            resources.ApplyResources(this._cancelButton, "_cancelButton");
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // _okButton
            // 
            resources.ApplyResources(this._okButton, "_okButton");
            this._okButton.Name = "_okButton";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // _applyButton
            // 
            resources.ApplyResources(this._applyButton, "_applyButton");
            this._applyButton.Name = "_applyButton";
            this._applyButton.UseVisualStyleBackColor = true;
            this._applyButton.Click += new System.EventHandler(this._applyButton_Click);
            // 
            // _helpButton
            // 
            resources.ApplyResources(this._helpButton, "_helpButton");
            this._helpButton.Name = "_helpButton";
            this._helpButton.UseVisualStyleBackColor = true;
            this._helpButton.Click += new System.EventHandler(this._helpButton_Click);
            // 
            // _behaviourPanel
            // 
            resources.ApplyResources(this._behaviourPanel, "_behaviourPanel");
            this._behaviourPanel.CropManager = null;
            this._behaviourPanel.Name = "_behaviourPanel";
            this._behaviourPanel.PriceManager = null;
            // 
            // _servicesPanel
            // 
            resources.ApplyResources(this._servicesPanel, "_servicesPanel");
            this._servicesPanel.CropManager = null;
            this._servicesPanel.Name = "_servicesPanel";
            this._servicesPanel.PriceManager = null;
            // 
            // _serviceDetailsPanel
            // 
            resources.ApplyResources(this._serviceDetailsPanel, "_serviceDetailsPanel");
            this._serviceDetailsPanel.CropManager = null;
            this._serviceDetailsPanel.Name = "_serviceDetailsPanel";
            this._serviceDetailsPanel.PriceManager = null;
            // 
            // _screenPanel
            // 
            resources.ApplyResources(this._screenPanel, "_screenPanel");
            this._screenPanel.CropManager = null;
            this._screenPanel.Name = "_screenPanel";
            this._screenPanel.PriceManager = null;
            // 
            // _receiptPrinterPanel
            // 
            resources.ApplyResources(this._receiptPrinterPanel, "_receiptPrinterPanel");
            this._receiptPrinterPanel.CropManager = null;
            this._receiptPrinterPanel.Name = "_receiptPrinterPanel";
            this._receiptPrinterPanel.PriceManager = null;
            // 
            // _pricePanel
            // 
            resources.ApplyResources(this._pricePanel, "_pricePanel");
            this._pricePanel.CropManager = null;
            this._pricePanel.Name = "_pricePanel";
            this._pricePanel.PriceManager = null;
            // 
            // _photoPrinterPanel
            // 
            resources.ApplyResources(this._photoPrinterPanel, "_photoPrinterPanel");
            this._photoPrinterPanel.CropManager = null;
            this._photoPrinterPanel.Name = "_photoPrinterPanel";
            this._photoPrinterPanel.PriceManager = null;
            // 
            // _paperTypesPanel
            // 
            resources.ApplyResources(this._paperTypesPanel, "_paperTypesPanel");
            this._paperTypesPanel.CropManager = null;
            this._paperTypesPanel.Name = "_paperTypesPanel";
            this._paperTypesPanel.PriceManager = null;
            // 
            // _paperFormatsPanel
            // 
            resources.ApplyResources(this._paperFormatsPanel, "_paperFormatsPanel");
            this._paperFormatsPanel.CropManager = null;
            this._paperFormatsPanel.Name = "_paperFormatsPanel";
            this._paperFormatsPanel.PriceManager = null;
            // 
            // _cropsPanel
            // 
            resources.ApplyResources(this._cropsPanel, "_cropsPanel");
            this._cropsPanel.CropManager = null;
            this._cropsPanel.Name = "_cropsPanel";
            this._cropsPanel.PriceManager = null;
            // 
            // _orderManagerPanel
            // 
            resources.ApplyResources(this._orderManagerPanel, "_orderManagerPanel");
            this._orderManagerPanel.CropManager = null;
            this._orderManagerPanel.Name = "_orderManagerPanel";
            this._orderManagerPanel.PriceManager = null;            
            // 
            // _imageEditorPanel
            // 
            resources.ApplyResources(this._imageEditorPanel, "_imageEditorPanel");
            this._imageEditorPanel.CropManager = null;
            this._imageEditorPanel.Name = "_imageEditorPanel";
            this._imageEditorPanel.PriceManager = null;
            // 
            // _formatPricePanel
            // 
            resources.ApplyResources(this._formatPricePanel, "_formatPricePanel");
            this._formatPricePanel.CropManager = null;
            this._formatPricePanel.Name = "_formatPricePanel";
            this._formatPricePanel.PriceManager = null;
            // 
            // _dpofPanel
            // 
            resources.ApplyResources(this._dpofPanel, "_dpofPanel");
            this._dpofPanel.CropManager = null;
            this._dpofPanel.Name = "_dpofPanel";
            this._dpofPanel.PriceManager = null;
            // 
            // _contactInfoPanel
            // 
            resources.ApplyResources(this._contactInfoPanel, "_contactInfoPanel");
            this._contactInfoPanel.CropManager = null;
            this._contactInfoPanel.Name = "_contactInfoPanel";
            this._contactInfoPanel.PriceManager = null;
            // 
            // _cdBurnerPanel
            // 
            resources.ApplyResources(this._cdBurnerPanel, "_cdBurnerPanel");
            this._cdBurnerPanel.CropManager = null;
            this._cdBurnerPanel.Name = "_cdBurnerPanel";
            this._cdBurnerPanel.PriceManager = null;
            // 
            // _callbackPanel
            // 
            resources.ApplyResources(this._callbackPanel, "_callbackPanel");
            this._callbackPanel.CropManager = null;
            this._callbackPanel.Name = "_callbackPanel";
            this._callbackPanel.PriceManager = null;
            // 
            // _bluetoothPanel
            // 
            resources.ApplyResources(this._bluetoothPanel, "_bluetoothPanel");
            this._bluetoothPanel.CropManager = null;
            this._bluetoothPanel.Name = "_bluetoothPanel";
            this._bluetoothPanel.PriceManager = null;
            // 
            // _arbitraryFormatPanel
            // 
            resources.ApplyResources(this._arbitraryFormatPanel, "_arbitraryFormatPanel");
            this._arbitraryFormatPanel.CropManager = null;
            this._arbitraryFormatPanel.Name = "_arbitraryFormatPanel";
            this._arbitraryFormatPanel.PriceManager = null;
            // 
            // _appearancePanel
            // 
            resources.ApplyResources(this._appearancePanel, "_appearancePanel");
            this._appearancePanel.CropManager = null;
            this._appearancePanel.Name = "_appearancePanel";
            this._appearancePanel.PriceManager = null;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._behaviourPanel);
            this.Controls.Add(this._servicesPanel);
            this.Controls.Add(this._serviceDetailsPanel);
            this.Controls.Add(this._screenPanel);
            this.Controls.Add(this._receiptPrinterPanel);
            this.Controls.Add(this._pricePanel);
            this.Controls.Add(this._photoPrinterPanel);
            this.Controls.Add(this._paperTypesPanel);
            this.Controls.Add(this._paperFormatsPanel);
            this.Controls.Add(this._cropsPanel);
            this.Controls.Add(this._orderManagerPanel);
            this.Controls.Add(this._imageEditorPanel);
            this.Controls.Add(this._formatPricePanel);
            this.Controls.Add(this._dpofPanel);
            this.Controls.Add(this._contactInfoPanel);
            this.Controls.Add(this._cdBurnerPanel);
            this.Controls.Add(this._callbackPanel);
            this.Controls.Add(this._bluetoothPanel);
            this.Controls.Add(this._arbitraryFormatPanel);
            this.Controls.Add(this._appearancePanel);
            this.Controls.Add(this._helpButton);
            this.Controls.Add(this._applyButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._separator);
            this.Controls.Add(this._optionsTree);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView _optionsTree;
        private System.Windows.Forms.Label _separator;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _applyButton;
        private System.Windows.Forms.Button _helpButton;
        private AppearancePanel _appearancePanel;
        private ArbitraryFormatPanel _arbitraryFormatPanel;
        private BluetoothPanel _bluetoothPanel;
        private CallbackPanel _callbackPanel;
        private CdBurnerPanel _cdBurnerPanel;
        private ContactInfoPanel _contactInfoPanel;
        private DpofPanel _dpofPanel;
        private FormatPricePanel _formatPricePanel;
        private ImageEditorPanel _imageEditorPanel;
        private OrderManagerPanel _orderManagerPanel;
        private Panels.CropsPanel _cropsPanel;
        private PaperFormatsPanel _paperFormatsPanel;
        private PaperTypesPanel _paperTypesPanel;
        private PhotoPrinterPanel _photoPrinterPanel;
        private PricePanel _pricePanel;
        private ReceiptPrinterPanel _receiptPrinterPanel;
        private ScreenPanel _screenPanel;
        private ServiceDetailsPanel _serviceDetailsPanel;
        private ServicesPanel _servicesPanel;
        private BehaviourPanel _behaviourPanel;      
    }
}