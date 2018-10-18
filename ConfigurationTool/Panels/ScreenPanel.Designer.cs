namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class ScreenPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenPanel));
            this._headerButton = new System.Windows.Forms.Button();
            this._headerBox = new System.Windows.Forms.TextBox();
            this._headerLabel = new System.Windows.Forms.Label();
            this._backgroundButton = new System.Windows.Forms.Button();
            this._backgroundBox = new System.Windows.Forms.TextBox();
            this._backgroundLabel = new System.Windows.Forms.Label();
            this._bannerButton = new System.Windows.Forms.Button();
            this._bannerBox = new System.Windows.Forms.TextBox();
            this._bannerLabel = new System.Windows.Forms.Label();
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
            // _headerButton
            // 
            resources.ApplyResources(this._headerButton, "_headerButton");
            this._headerButton.Name = "_headerButton";
            this._headerButton.UseVisualStyleBackColor = true;
            this._headerButton.Click += new System.EventHandler(this._headerButton_Click);
            // 
            // _headerBox
            // 
            resources.ApplyResources(this._headerBox, "_headerBox");
            this._headerBox.Name = "_headerBox";
            this._headerBox.TextChanged += new System.EventHandler(this._headerBox_TextChanged);
            this._headerBox.Validating += new System.ComponentModel.CancelEventHandler(this._box_Validating);
            // 
            // _headerLabel
            // 
            resources.ApplyResources(this._headerLabel, "_headerLabel");
            this._headerLabel.Name = "_headerLabel";
            // 
            // _backgroundButton
            // 
            resources.ApplyResources(this._backgroundButton, "_backgroundButton");
            this._backgroundButton.Name = "_backgroundButton";
            this._backgroundButton.UseVisualStyleBackColor = true;
            this._backgroundButton.Click += new System.EventHandler(this._backgroundButton_Click);
            // 
            // _backgroundBox
            // 
            resources.ApplyResources(this._backgroundBox, "_backgroundBox");
            this._backgroundBox.Name = "_backgroundBox";
            this._backgroundBox.TextChanged += new System.EventHandler(this._backgroundBox_TextChanged);
            this._backgroundBox.Validating += new System.ComponentModel.CancelEventHandler(this._box_Validating);
            // 
            // _backgroundLabel
            // 
            resources.ApplyResources(this._backgroundLabel, "_backgroundLabel");
            this._backgroundLabel.Name = "_backgroundLabel";
            // 
            // _bannerButton
            // 
            resources.ApplyResources(this._bannerButton, "_bannerButton");
            this._bannerButton.Name = "_bannerButton";
            this._bannerButton.UseVisualStyleBackColor = true;
            this._bannerButton.Click += new System.EventHandler(this._bannerButton_Click);
            // 
            // _bannerBox
            // 
            resources.ApplyResources(this._bannerBox, "_bannerBox");
            this._bannerBox.Name = "_bannerBox";
            this._bannerBox.TextChanged += new System.EventHandler(this._bannerBox_TextChanged);
            this._bannerBox.Validating += new System.ComponentModel.CancelEventHandler(this._box_Validating);
            // 
            // _bannerLabel
            // 
            resources.ApplyResources(this._bannerLabel, "_bannerLabel");
            this._bannerLabel.Name = "_bannerLabel";
            // 
            // ScreenPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._bannerButton);
            this.Controls.Add(this._bannerBox);
            this.Controls.Add(this._bannerLabel);
            this.Controls.Add(this._backgroundButton);
            this.Controls.Add(this._backgroundBox);
            this.Controls.Add(this._backgroundLabel);
            this.Controls.Add(this._headerButton);
            this.Controls.Add(this._headerBox);
            this.Controls.Add(this._headerLabel);
            this.Name = "ScreenPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _headerButton;
        private System.Windows.Forms.TextBox _headerBox;
        private System.Windows.Forms.Label _headerLabel;
        private System.Windows.Forms.Button _backgroundButton;
        private System.Windows.Forms.TextBox _backgroundBox;
        private System.Windows.Forms.Label _backgroundLabel;
        private System.Windows.Forms.Button _bannerButton;
        private System.Windows.Forms.TextBox _bannerBox;
        private System.Windows.Forms.Label _bannerLabel;
    }
}
