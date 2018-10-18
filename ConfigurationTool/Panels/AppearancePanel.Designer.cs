namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class AppearancePanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppearancePanel));
            this._themeLabel = new System.Windows.Forms.Label();
            this._themeBox = new System.Windows.Forms.TextBox();
            this._themeButton = new System.Windows.Forms.Button();
            this._localizationLabel = new System.Windows.Forms.Label();
            this._localizationBox = new System.Windows.Forms.TextBox();
            this._localizationButton = new System.Windows.Forms.Button();
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
            // _themeLabel
            // 
            resources.ApplyResources(this._themeLabel, "_themeLabel");
            this._themeLabel.Name = "_themeLabel";
            // 
            // _themeBox
            // 
            resources.ApplyResources(this._themeBox, "_themeBox");
            this._themeBox.Name = "_themeBox";
            this._themeBox.TextChanged += new System.EventHandler(this._themeBox_TextChanged);
            this._themeBox.Validating += new System.ComponentModel.CancelEventHandler(this._themeBox_Validating);
            // 
            // _themeButton
            // 
            resources.ApplyResources(this._themeButton, "_themeButton");
            this._themeButton.Name = "_themeButton";
            this._themeButton.UseVisualStyleBackColor = true;
            this._themeButton.Click += new System.EventHandler(this._themeButton_Click);
            // 
            // _localizationLabel
            // 
            resources.ApplyResources(this._localizationLabel, "_localizationLabel");
            this._localizationLabel.Name = "_localizationLabel";
            // 
            // _localizationBox
            // 
            resources.ApplyResources(this._localizationBox, "_localizationBox");
            this._localizationBox.Name = "_localizationBox";
            this._localizationBox.TextChanged += new System.EventHandler(this._localizationBox_TextChanged);
            this._localizationBox.Validating += new System.ComponentModel.CancelEventHandler(this._localizationBox_Validating);
            // 
            // _localizationButton
            // 
            resources.ApplyResources(this._localizationButton, "_localizationButton");
            this._localizationButton.Name = "_localizationButton";
            this._localizationButton.UseVisualStyleBackColor = true;
            this._localizationButton.Click += new System.EventHandler(this._localizationButton_Click);
            // 
            // AppearancePanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._localizationButton);
            this.Controls.Add(this._localizationBox);
            this.Controls.Add(this._localizationLabel);
            this.Controls.Add(this._themeButton);
            this.Controls.Add(this._themeBox);
            this.Controls.Add(this._themeLabel);
            this.Name = "AppearancePanel";
            this.Load += new System.EventHandler(this.AppearancePanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _themeLabel;
        private System.Windows.Forms.TextBox _themeBox;
        private System.Windows.Forms.Button _themeButton;
        private System.Windows.Forms.Label _localizationLabel;
        private System.Windows.Forms.TextBox _localizationBox;
        private System.Windows.Forms.Button _localizationButton;
    }
}
