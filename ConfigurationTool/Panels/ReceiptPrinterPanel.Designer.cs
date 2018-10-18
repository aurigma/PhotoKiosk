namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class ReceiptPrinterPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiptPrinterPanel));
            this._receiptPrinterCheckbox = new System.Windows.Forms.CheckBox();
            this._printerNameLabel = new System.Windows.Forms.Label();
            this._printerNameBox = new System.Windows.Forms.ComboBox();
            this._countLabel = new System.Windows.Forms.Label();
            this._countBox = new Aurigma.PhotoKiosk.ConfigurationTool.NumericTextBox();
            this._templateLabel = new System.Windows.Forms.Label();
            this._templateButton = new System.Windows.Forms.Button();
            this._templateBox = new System.Windows.Forms.TextBox();
            this._testButton = new System.Windows.Forms.Button();
            this._countLabelNote = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _folderBrowserDialog
            // 
            resources.ApplyResources(this._folderBrowserDialog, "_folderBrowserDialog");
            // 
            // _openFileDialog
            // 
            resources.ApplyResources(this._openFileDialog, "_openFileDialog");
            // 
            // _receiptPrinterCheckbox
            // 
            resources.ApplyResources(this._receiptPrinterCheckbox, "_receiptPrinterCheckbox");
            this._receiptPrinterCheckbox.Name = "_receiptPrinterCheckbox";
            this._receiptPrinterCheckbox.UseVisualStyleBackColor = true;
            this._receiptPrinterCheckbox.CheckedChanged += new System.EventHandler(this._receiptPrinterCheckbox_CheckedChanged);
            // 
            // _printerNameLabel
            // 
            resources.ApplyResources(this._printerNameLabel, "_printerNameLabel");
            this._printerNameLabel.Name = "_printerNameLabel";
            // 
            // _printerNameBox
            // 
            resources.ApplyResources(this._printerNameBox, "_printerNameBox");
            this._printerNameBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._printerNameBox.FormattingEnabled = true;
            this._printerNameBox.Name = "_printerNameBox";
            this._printerNameBox.SelectedIndexChanged += new System.EventHandler(this._printerNameBox_SelectedIndexChanged);
            // 
            // _countLabel
            // 
            resources.ApplyResources(this._countLabel, "_countLabel");
            this._countLabel.Name = "_countLabel";
            // 
            // _countBox
            // 
            resources.ApplyResources(this._countBox, "_countBox");
            this._countBox.FloatValue = 0F;
            this._countBox.IntValue = 0;
            this._countBox.Name = "_countBox";
            this._countBox.TextChanged += new System.EventHandler(this._countBox_TextChanged);
            // 
            // _templateLabel
            // 
            resources.ApplyResources(this._templateLabel, "_templateLabel");
            this._templateLabel.Name = "_templateLabel";
            // 
            // _templateButton
            // 
            resources.ApplyResources(this._templateButton, "_templateButton");
            this._templateButton.Name = "_templateButton";
            this._templateButton.UseVisualStyleBackColor = true;
            this._templateButton.Click += new System.EventHandler(this._templateButton_Click);
            // 
            // _templateBox
            // 
            resources.ApplyResources(this._templateBox, "_templateBox");
            this._templateBox.Name = "_templateBox";
            this._templateBox.TextChanged += new System.EventHandler(this._templateBox_TextChanged);
            this._templateBox.Validating += new System.ComponentModel.CancelEventHandler(this._templateBox_Validating);
            // 
            // _testButton
            // 
            resources.ApplyResources(this._testButton, "_testButton");
            this._testButton.Name = "_testButton";
            this._testButton.UseVisualStyleBackColor = true;
            this._testButton.Click += new System.EventHandler(this._testButton_Click);
            // 
            // _countLabelNote
            // 
            resources.ApplyResources(this._countLabelNote, "_countLabelNote");
            this._countLabelNote.Name = "_countLabelNote";
            // 
            // ReceiptPrinterPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._countLabelNote);
            this.Controls.Add(this._testButton);
            this.Controls.Add(this._templateButton);
            this.Controls.Add(this._templateBox);
            this.Controls.Add(this._templateLabel);
            this.Controls.Add(this._countBox);
            this.Controls.Add(this._countLabel);
            this.Controls.Add(this._printerNameBox);
            this.Controls.Add(this._printerNameLabel);
            this.Controls.Add(this._receiptPrinterCheckbox);
            this.Name = "ReceiptPrinterPanel";
            this.Load += new System.EventHandler(this.ReceiptPrinterPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _receiptPrinterCheckbox;
        private System.Windows.Forms.Label _printerNameLabel;
        private System.Windows.Forms.ComboBox _printerNameBox;
        private System.Windows.Forms.Label _countLabel;
        private NumericTextBox _countBox;
        private System.Windows.Forms.Label _templateLabel;
        private System.Windows.Forms.Button _templateButton;
        private System.Windows.Forms.TextBox _templateBox;
        private System.Windows.Forms.Button _testButton;
        private System.Windows.Forms.Label _countLabelNote;
    }
}
