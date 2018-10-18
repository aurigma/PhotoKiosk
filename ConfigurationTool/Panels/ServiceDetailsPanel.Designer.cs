namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class ServiceDetailsPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceDetailsPanel));
            this._priceComboBox = new System.Windows.Forms.ComboBox();
            this._priceLabel = new System.Windows.Forms.Label();
            this._descriptionBox = new System.Windows.Forms.TextBox();
            this._descriptionLabel = new System.Windows.Forms.Label();
            this._nameBox = new System.Windows.Forms.TextBox();
            this._nameLabel = new System.Windows.Forms.Label();
            this._isPermanentCheckbox = new System.Windows.Forms.CheckBox();
            this._priceBox = new Aurigma.PhotoKiosk.ConfigurationTool.NumericTextBox();
            this.SuspendLayout();
            // 
            // _priceComboBox
            // 
            this._priceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._priceComboBox.FormattingEnabled = true;
            resources.ApplyResources(this._priceComboBox, "_priceComboBox");
            this._priceComboBox.Name = "_priceComboBox";
            this._priceComboBox.SelectedIndexChanged += new System.EventHandler(this._priceComboBox_SelectedIndexChanged);
            // 
            // _priceLabel
            // 
            resources.ApplyResources(this._priceLabel, "_priceLabel");
            this._priceLabel.Name = "_priceLabel";
            // 
            // _descriptionBox
            // 
            resources.ApplyResources(this._descriptionBox, "_descriptionBox");
            this._descriptionBox.Name = "_descriptionBox";
            this._descriptionBox.TextChanged += new System.EventHandler(this._descriptionBox_TextChanged);
            // 
            // _descriptionLabel
            // 
            resources.ApplyResources(this._descriptionLabel, "_descriptionLabel");
            this._descriptionLabel.Name = "_descriptionLabel";
            // 
            // _nameBox
            // 
            resources.ApplyResources(this._nameBox, "_nameBox");
            this._nameBox.Name = "_nameBox";
            this._nameBox.ReadOnly = true;
            // 
            // _nameLabel
            // 
            resources.ApplyResources(this._nameLabel, "_nameLabel");
            this._nameLabel.Name = "_nameLabel";
            // 
            // _isPermanentCheckbox
            // 
            resources.ApplyResources(this._isPermanentCheckbox, "_isPermanentCheckbox");
            this._isPermanentCheckbox.Name = "_isPermanentCheckbox";
            this._isPermanentCheckbox.UseVisualStyleBackColor = true;
            this._isPermanentCheckbox.CheckedChanged += new System.EventHandler(this._isPermanentCheckbox_CheckedChanged);
            // 
            // _priceBox
            // 
            this._priceBox.FloatValue = 0F;
            this._priceBox.IntValue = 0;
            this._priceBox.IsFloat = true;
            resources.ApplyResources(this._priceBox, "_priceBox");
            this._priceBox.Name = "_priceBox";
            this._priceBox.TextChanged += new System.EventHandler(this._priceBox_TextChanged);
            this._priceBox.Validating += new System.ComponentModel.CancelEventHandler(this._priceBox_Validating);
            // 
            // ServiceDetailsPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._isPermanentCheckbox);
            this.Controls.Add(this._priceComboBox);
            this.Controls.Add(this._priceBox);
            this.Controls.Add(this._priceLabel);
            this.Controls.Add(this._descriptionBox);
            this.Controls.Add(this._descriptionLabel);
            this.Controls.Add(this._nameBox);
            this.Controls.Add(this._nameLabel);
            this.Name = "ServiceDetailsPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _nameLabel;
        private System.Windows.Forms.TextBox _nameBox;
        private System.Windows.Forms.Label _descriptionLabel;
        private System.Windows.Forms.TextBox _descriptionBox;
        private System.Windows.Forms.Label _priceLabel;
        private NumericTextBox _priceBox;
        private System.Windows.Forms.ComboBox _priceComboBox;
        private System.Windows.Forms.CheckBox _isPermanentCheckbox;
    }
}
