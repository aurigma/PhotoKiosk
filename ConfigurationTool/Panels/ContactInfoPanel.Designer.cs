namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class ContactInfoPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContactInfoPanel));
            this._phoneCheckbox = new System.Windows.Forms.CheckBox();
            this._emailCheckbox = new System.Windows.Forms.CheckBox();
            this._customIdCheckbox = new System.Windows.Forms.CheckBox();
            this._userNameOrderIdCheckBox = new System.Windows.Forms.CheckBox();
            this._skipContactInfoCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // _phoneCheckbox
            // 
            resources.ApplyResources(this._phoneCheckbox, "_phoneCheckbox");
            this._phoneCheckbox.Name = "_phoneCheckbox";
            this._phoneCheckbox.UseVisualStyleBackColor = true;
            this._phoneCheckbox.CheckedChanged += new System.EventHandler(this._phoneCheckbox_CheckedChanged);
            // 
            // _emailCheckbox
            // 
            resources.ApplyResources(this._emailCheckbox, "_emailCheckbox");
            this._emailCheckbox.Name = "_emailCheckbox";
            this._emailCheckbox.UseVisualStyleBackColor = true;
            this._emailCheckbox.CheckedChanged += new System.EventHandler(this._emailCheckbox_CheckedChanged);
            // 
            // _customIdCheckbox
            // 
            resources.ApplyResources(this._customIdCheckbox, "_customIdCheckbox");
            this._customIdCheckbox.Name = "_customIdCheckbox";
            this._customIdCheckbox.UseVisualStyleBackColor = true;
            this._customIdCheckbox.CheckedChanged += new System.EventHandler(this._customIdCheckbox_CheckedChanged);
            // 
            // _userNameOrderIdCheckBox
            // 
            resources.ApplyResources(this._userNameOrderIdCheckBox, "_userNameOrderIdCheckBox");
            this._userNameOrderIdCheckBox.Name = "_userNameOrderIdCheckBox";
            this._userNameOrderIdCheckBox.UseVisualStyleBackColor = true;
            this._userNameOrderIdCheckBox.CheckedChanged += new System.EventHandler(this._userNameOrderIdCheckbox_CheckedChanged);
            // 
            // _skipContactInfoCheckBox
            // 
            resources.ApplyResources(this._skipContactInfoCheckBox, "_skipContactInfoCheckBox");
            this._skipContactInfoCheckBox.Name = "_skipContactInfoCheckBox";
            this._skipContactInfoCheckBox.UseVisualStyleBackColor = true;
            this._skipContactInfoCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ContactInfoPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._skipContactInfoCheckBox);
            this.Controls.Add(this._userNameOrderIdCheckBox);
            this.Controls.Add(this._phoneCheckbox);
            this.Controls.Add(this._emailCheckbox);
            this.Controls.Add(this._customIdCheckbox);
            this.Name = "ContactInfoPanel";
            this.Load += new System.EventHandler(this.OrderingPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _phoneCheckbox;
        private System.Windows.Forms.CheckBox _emailCheckbox;
        private System.Windows.Forms.CheckBox _customIdCheckbox;
        private System.Windows.Forms.CheckBox _userNameOrderIdCheckBox;
        private System.Windows.Forms.CheckBox _skipContactInfoCheckBox;
    }
}
