namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class OrderManagerPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderManagerPanel));
            this._checkBox = new System.Windows.Forms.CheckBox();
            this._pathButton = new System.Windows.Forms.Button();
            this._pathBox = new System.Windows.Forms.TextBox();
            this._pathLabel = new System.Windows.Forms.Label();
            this._cleanupCheckbox = new System.Windows.Forms.CheckBox();
            this._convertCheckbox = new System.Windows.Forms.CheckBox();
            this._dpofCheckbox = new System.Windows.Forms.CheckBox();
            this._cleanUpNote = new System.Windows.Forms.Label();
            this._transliterationCheckbox = new System.Windows.Forms.CheckBox();
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
            // _checkBox
            // 
            resources.ApplyResources(this._checkBox, "_checkBox");
            this._checkBox.Name = "_checkBox";
            this._checkBox.UseVisualStyleBackColor = true;
            this._checkBox.CheckedChanged += new System.EventHandler(this._checkBox_CheckedChanged);
            // 
            // _pathButton
            // 
            resources.ApplyResources(this._pathButton, "_pathButton");
            this._pathButton.Name = "_pathButton";
            this._pathButton.UseVisualStyleBackColor = true;
            this._pathButton.Click += new System.EventHandler(this._pathButton_Click);
            // 
            // _pathBox
            // 
            resources.ApplyResources(this._pathBox, "_pathBox");
            this._pathBox.Name = "_pathBox";
            this._pathBox.TextChanged += new System.EventHandler(this._pathBox_TextChanged);
            this._pathBox.Validating += new System.ComponentModel.CancelEventHandler(this._pathBox_Validating);
            // 
            // _pathLabel
            // 
            resources.ApplyResources(this._pathLabel, "_pathLabel");
            this._pathLabel.Name = "_pathLabel";
            // 
            // _cleanupCheckbox
            // 
            resources.ApplyResources(this._cleanupCheckbox, "_cleanupCheckbox");
            this._cleanupCheckbox.Name = "_cleanupCheckbox";
            this._cleanupCheckbox.UseVisualStyleBackColor = true;
            this._cleanupCheckbox.CheckedChanged += new System.EventHandler(this._cleanupCheckbox_CheckedChanged);
            // 
            // _convertCheckbox
            // 
            resources.ApplyResources(this._convertCheckbox, "_convertCheckbox");
            this._convertCheckbox.Name = "_convertCheckbox";
            this._convertCheckbox.UseVisualStyleBackColor = true;
            this._convertCheckbox.CheckedChanged += new System.EventHandler(this._convertCheckbox_CheckedChanged);
            // 
            // _dpofCheckbox
            // 
            resources.ApplyResources(this._dpofCheckbox, "_dpofCheckbox");
            this._dpofCheckbox.Name = "_dpofCheckbox";
            this._dpofCheckbox.UseVisualStyleBackColor = true;
            this._dpofCheckbox.CheckedChanged += new System.EventHandler(this._dpofCheckbox_CheckedChanged);
            // 
            // _cleanUpNote
            // 
            resources.ApplyResources(this._cleanUpNote, "_cleanUpNote");
            this._cleanUpNote.Name = "_cleanUpNote";
            // 
            // _transliterationCheckbox
            // 
            resources.ApplyResources(this._transliterationCheckbox, "_transliterationCheckbox");
            this._transliterationCheckbox.Name = "_transliterationCheckbox";
            this._transliterationCheckbox.UseVisualStyleBackColor = true;
            this._transliterationCheckbox.CheckedChanged += new System.EventHandler(this._transliterationCheckbox_CheckedChanged);
            // 
            // OrderManagerPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._transliterationCheckbox);
            this.Controls.Add(this._cleanUpNote);
            this.Controls.Add(this._dpofCheckbox);
            this.Controls.Add(this._convertCheckbox);
            this.Controls.Add(this._cleanupCheckbox);
            this.Controls.Add(this._pathButton);
            this.Controls.Add(this._pathBox);
            this.Controls.Add(this._pathLabel);
            this.Controls.Add(this._checkBox);
            this.Name = "OrderManagerPanel";
            this.Load += new System.EventHandler(this.OrderManagerPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _checkBox;
        private System.Windows.Forms.Button _pathButton;
        private System.Windows.Forms.TextBox _pathBox;
        private System.Windows.Forms.Label _pathLabel;
        private System.Windows.Forms.CheckBox _cleanupCheckbox;
        private System.Windows.Forms.CheckBox _convertCheckbox;
        private System.Windows.Forms.CheckBox _dpofCheckbox;
        private System.Windows.Forms.Label _cleanUpNote;
        private System.Windows.Forms.CheckBox _transliterationCheckbox;
    }
}
