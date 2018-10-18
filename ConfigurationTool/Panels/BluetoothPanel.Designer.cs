namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class BluetoothPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BluetoothPanel));
            this._bluetoothNoteLabel = new System.Windows.Forms.Label();
            this._bluetoothFolderButton = new System.Windows.Forms.Button();
            this._bluetoothFolderBox = new System.Windows.Forms.TextBox();
            this._bluetoothFolderLabel = new System.Windows.Forms.Label();
            this._bluetoothHostBox = new System.Windows.Forms.TextBox();
            this._bluetoothHostLabel = new System.Windows.Forms.Label();
            this._bluetoothCheckbox = new System.Windows.Forms.CheckBox();
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
            // _bluetoothNoteLabel
            // 
            resources.ApplyResources(this._bluetoothNoteLabel, "_bluetoothNoteLabel");
            this._bluetoothNoteLabel.Name = "_bluetoothNoteLabel";
            // 
            // _bluetoothFolderButton
            // 
            resources.ApplyResources(this._bluetoothFolderButton, "_bluetoothFolderButton");
            this._bluetoothFolderButton.Name = "_bluetoothFolderButton";
            this._bluetoothFolderButton.UseVisualStyleBackColor = true;
            this._bluetoothFolderButton.Click += new System.EventHandler(this._bluetoothFolderButton_Click);
            // 
            // _bluetoothFolderBox
            // 
            resources.ApplyResources(this._bluetoothFolderBox, "_bluetoothFolderBox");
            this._bluetoothFolderBox.Name = "_bluetoothFolderBox";
            this._bluetoothFolderBox.TextChanged += new System.EventHandler(this._bluetoothFolderBox_TextChanged);
            this._bluetoothFolderBox.Validating += new System.ComponentModel.CancelEventHandler(this._bluetoothFolderBox_Validating);
            // 
            // _bluetoothFolderLabel
            // 
            resources.ApplyResources(this._bluetoothFolderLabel, "_bluetoothFolderLabel");
            this._bluetoothFolderLabel.Name = "_bluetoothFolderLabel";
            // 
            // _bluetoothHostBox
            // 
            resources.ApplyResources(this._bluetoothHostBox, "_bluetoothHostBox");
            this._bluetoothHostBox.Name = "_bluetoothHostBox";
            this._bluetoothHostBox.TextChanged += new System.EventHandler(this._bluetoothHostBox_TextChanged);
            this._bluetoothHostBox.Validating += new System.ComponentModel.CancelEventHandler(this._bluetoothHostBox_Validating);
            // 
            // _bluetoothHostLabel
            // 
            resources.ApplyResources(this._bluetoothHostLabel, "_bluetoothHostLabel");
            this._bluetoothHostLabel.Name = "_bluetoothHostLabel";
            // 
            // _bluetoothCheckbox
            // 
            resources.ApplyResources(this._bluetoothCheckbox, "_bluetoothCheckbox");
            this._bluetoothCheckbox.Name = "_bluetoothCheckbox";
            this._bluetoothCheckbox.UseVisualStyleBackColor = true;
            this._bluetoothCheckbox.CheckedChanged += new System.EventHandler(this._bluetoothCheckbox_CheckedChanged);
            // 
            // BluetoothPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._bluetoothFolderButton);
            this.Controls.Add(this._bluetoothFolderBox);
            this.Controls.Add(this._bluetoothFolderLabel);
            this.Controls.Add(this._bluetoothHostBox);
            this.Controls.Add(this._bluetoothHostLabel);
            this.Controls.Add(this._bluetoothCheckbox);
            this.Controls.Add(this._bluetoothNoteLabel);
            this.Name = "BluetoothPanel";
            this.Load += new System.EventHandler(this.BluetoothPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _bluetoothNoteLabel;
        private System.Windows.Forms.Button _bluetoothFolderButton;
        private System.Windows.Forms.TextBox _bluetoothFolderBox;
        private System.Windows.Forms.Label _bluetoothFolderLabel;
        private System.Windows.Forms.TextBox _bluetoothHostBox;
        private System.Windows.Forms.Label _bluetoothHostLabel;
        private System.Windows.Forms.CheckBox _bluetoothCheckbox;
    }
}
