namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class ArbitraryFormatPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArbitraryFormatPanel));
            this._clearOrderInfoTemplateButton = new System.Windows.Forms.Button();
            this._orderInfoTemplateBox = new System.Windows.Forms.TextBox();
            this._orderInfoTemplateLabel = new System.Windows.Forms.Label();
            this._photoTemplateLabel = new System.Windows.Forms.Label();
            this._tagsLabel = new System.Windows.Forms.Label();
            this._insertTagButton = new System.Windows.Forms.Button();
            this._tagsBox = new System.Windows.Forms.ComboBox();
            this._clearPhotoTemplateButton = new System.Windows.Forms.Button();
            this._photoTemplateBox = new System.Windows.Forms.TextBox();
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
            // _clearOrderInfoTemplateButton
            // 
            resources.ApplyResources(this._clearOrderInfoTemplateButton, "_clearOrderInfoTemplateButton");
            this._clearOrderInfoTemplateButton.Name = "_clearOrderInfoTemplateButton";
            this._clearOrderInfoTemplateButton.UseVisualStyleBackColor = true;
            this._clearOrderInfoTemplateButton.Click += new System.EventHandler(this._clearOrderInfoTemplateButton_Click);
            // 
            // _orderInfoTemplateBox
            // 
            resources.ApplyResources(this._orderInfoTemplateBox, "_orderInfoTemplateBox");
            this._orderInfoTemplateBox.Name = "_orderInfoTemplateBox";
            this._orderInfoTemplateBox.TextChanged += new System.EventHandler(this._orderInfoTemplateBox_TextChanged);
            this._orderInfoTemplateBox.Enter += new System.EventHandler(this._textBox_Enter);
            // 
            // _orderInfoTemplateLabel
            // 
            resources.ApplyResources(this._orderInfoTemplateLabel, "_orderInfoTemplateLabel");
            this._orderInfoTemplateLabel.Name = "_orderInfoTemplateLabel";
            // 
            // _photoTemplateLabel
            // 
            resources.ApplyResources(this._photoTemplateLabel, "_photoTemplateLabel");
            this._photoTemplateLabel.Name = "_photoTemplateLabel";
            // 
            // _tagsLabel
            // 
            resources.ApplyResources(this._tagsLabel, "_tagsLabel");
            this._tagsLabel.Name = "_tagsLabel";
            // 
            // _insertTagButton
            // 
            resources.ApplyResources(this._insertTagButton, "_insertTagButton");
            this._insertTagButton.Name = "_insertTagButton";
            this._insertTagButton.UseVisualStyleBackColor = true;
            this._insertTagButton.Click += new System.EventHandler(this._insertTagButton_Click);
            // 
            // _tagsBox
            // 
            resources.ApplyResources(this._tagsBox, "_tagsBox");
            this._tagsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._tagsBox.FormattingEnabled = true;
            this._tagsBox.Name = "_tagsBox";
            // 
            // _clearPhotoTemplateButton
            // 
            resources.ApplyResources(this._clearPhotoTemplateButton, "_clearPhotoTemplateButton");
            this._clearPhotoTemplateButton.Name = "_clearPhotoTemplateButton";
            this._clearPhotoTemplateButton.UseVisualStyleBackColor = true;
            this._clearPhotoTemplateButton.Click += new System.EventHandler(this._clearPhotoTemplateButton_Click);
            // 
            // _photoTemplateBox
            // 
            resources.ApplyResources(this._photoTemplateBox, "_photoTemplateBox");
            this._photoTemplateBox.Name = "_photoTemplateBox";
            this._photoTemplateBox.TextChanged += new System.EventHandler(this._photoTemplateBox_TextChanged);
            this._photoTemplateBox.Enter += new System.EventHandler(this._textBox_Enter);
            this._photoTemplateBox.Validating += new System.ComponentModel.CancelEventHandler(this._photoTemplateBox_Validating);
            // 
            // ArbitraryFormatPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._clearOrderInfoTemplateButton);
            this.Controls.Add(this._orderInfoTemplateBox);
            this.Controls.Add(this._orderInfoTemplateLabel);
            this.Controls.Add(this._photoTemplateLabel);
            this.Controls.Add(this._tagsLabel);
            this.Controls.Add(this._insertTagButton);
            this.Controls.Add(this._tagsBox);
            this.Controls.Add(this._clearPhotoTemplateButton);
            this.Controls.Add(this._photoTemplateBox);
            this.Name = "ArbitraryFormatPanel";
            this.Load += new System.EventHandler(this.ArbitraryFormatPanel_Load);
            this.VisibleChanged += new System.EventHandler(this.ArbitraryFormatPanel_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _tagsLabel;
        private System.Windows.Forms.Button _insertTagButton;
        private System.Windows.Forms.ComboBox _tagsBox;
        private System.Windows.Forms.Button _clearPhotoTemplateButton;
        private System.Windows.Forms.TextBox _photoTemplateBox;
        private System.Windows.Forms.Label _photoTemplateLabel;
        private System.Windows.Forms.Label _orderInfoTemplateLabel;
        private System.Windows.Forms.Button _clearOrderInfoTemplateButton;
        private System.Windows.Forms.TextBox _orderInfoTemplateBox;
    }
}
