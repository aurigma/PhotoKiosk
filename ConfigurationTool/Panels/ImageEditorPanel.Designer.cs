namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class ImageEditorPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageEditorPanel));
            this._cropFileButton = new System.Windows.Forms.Button();
            this._cropFileBox = new System.Windows.Forms.TextBox();
            this._cropFileLable = new System.Windows.Forms.Label();
            this._redEyeCheckbox = new System.Windows.Forms.CheckBox();
            this._effectsCheckbox = new System.Windows.Forms.CheckBox();
            this._colorCheckbox = new System.Windows.Forms.CheckBox();
            this._cropCheckbox = new System.Windows.Forms.CheckBox();
            this._flipCheckbox = new System.Windows.Forms.CheckBox();
            this._rotationCheckbox = new System.Windows.Forms.CheckBox();
            this._imageEditorCheckbox = new System.Windows.Forms.CheckBox();
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
            // _cropFileButton
            // 
            resources.ApplyResources(this._cropFileButton, "_cropFileButton");
            this._cropFileButton.Name = "_cropFileButton";
            this._cropFileButton.UseVisualStyleBackColor = true;
            this._cropFileButton.Click += new System.EventHandler(this._cropFileButton_Click);
            // 
            // _cropFileBox
            // 
            resources.ApplyResources(this._cropFileBox, "_cropFileBox");
            this._cropFileBox.Name = "_cropFileBox";
            this._cropFileBox.TextChanged += new System.EventHandler(this._cropFileBox_TextChanged);
            // 
            // _cropFileLable
            // 
            resources.ApplyResources(this._cropFileLable, "_cropFileLable");
            this._cropFileLable.Name = "_cropFileLable";
            // 
            // _redEyeCheckbox
            // 
            resources.ApplyResources(this._redEyeCheckbox, "_redEyeCheckbox");
            this._redEyeCheckbox.Name = "_redEyeCheckbox";
            this._redEyeCheckbox.UseVisualStyleBackColor = true;
            this._redEyeCheckbox.CheckedChanged += new System.EventHandler(this._redEyeCheckbox_CheckedChanged);
            // 
            // _effectsCheckbox
            // 
            resources.ApplyResources(this._effectsCheckbox, "_effectsCheckbox");
            this._effectsCheckbox.Name = "_effectsCheckbox";
            this._effectsCheckbox.UseVisualStyleBackColor = true;
            this._effectsCheckbox.CheckedChanged += new System.EventHandler(this._effectsCheckbox_CheckedChanged);
            // 
            // _colorCheckbox
            // 
            resources.ApplyResources(this._colorCheckbox, "_colorCheckbox");
            this._colorCheckbox.Name = "_colorCheckbox";
            this._colorCheckbox.UseVisualStyleBackColor = true;
            this._colorCheckbox.CheckedChanged += new System.EventHandler(this._colorCheckbox_CheckedChanged);
            // 
            // _cropCheckbox
            // 
            resources.ApplyResources(this._cropCheckbox, "_cropCheckbox");
            this._cropCheckbox.Name = "_cropCheckbox";
            this._cropCheckbox.UseVisualStyleBackColor = true;
            this._cropCheckbox.CheckedChanged += new System.EventHandler(this._cropCheckbox_CheckedChanged);
            // 
            // _flipCheckbox
            // 
            resources.ApplyResources(this._flipCheckbox, "_flipCheckbox");
            this._flipCheckbox.Name = "_flipCheckbox";
            this._flipCheckbox.UseVisualStyleBackColor = true;
            this._flipCheckbox.CheckedChanged += new System.EventHandler(this._flipCheckbox_CheckedChanged);
            // 
            // _rotationCheckbox
            // 
            resources.ApplyResources(this._rotationCheckbox, "_rotationCheckbox");
            this._rotationCheckbox.Name = "_rotationCheckbox";
            this._rotationCheckbox.UseVisualStyleBackColor = true;
            this._rotationCheckbox.CheckedChanged += new System.EventHandler(this._rotationCheckbox_CheckedChanged);
            // 
            // _imageEditorCheckbox
            // 
            resources.ApplyResources(this._imageEditorCheckbox, "_imageEditorCheckbox");
            this._imageEditorCheckbox.Name = "_imageEditorCheckbox";
            this._imageEditorCheckbox.UseVisualStyleBackColor = true;
            this._imageEditorCheckbox.CheckedChanged += new System.EventHandler(this._imageEditorCheckbox_CheckedChanged);
            // 
            // ImageEditorPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._cropFileButton);
            this.Controls.Add(this._cropFileBox);
            this.Controls.Add(this._cropFileLable);
            this.Controls.Add(this._redEyeCheckbox);
            this.Controls.Add(this._effectsCheckbox);
            this.Controls.Add(this._colorCheckbox);
            this.Controls.Add(this._cropCheckbox);
            this.Controls.Add(this._flipCheckbox);
            this.Controls.Add(this._rotationCheckbox);
            this.Controls.Add(this._imageEditorCheckbox);
            this.Name = "ImageEditorPanel";
            this.Load += new System.EventHandler(this.ImageEditorPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _cropFileButton;
        private System.Windows.Forms.TextBox _cropFileBox;
        private System.Windows.Forms.Label _cropFileLable;
        private System.Windows.Forms.CheckBox _redEyeCheckbox;
        private System.Windows.Forms.CheckBox _effectsCheckbox;
        private System.Windows.Forms.CheckBox _colorCheckbox;
        private System.Windows.Forms.CheckBox _cropCheckbox;
        private System.Windows.Forms.CheckBox _flipCheckbox;
        private System.Windows.Forms.CheckBox _rotationCheckbox;
        private System.Windows.Forms.CheckBox _imageEditorCheckbox;
    }
}
