namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class BehaviourPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BehaviourPanel));
            this._pathsBox = new System.Windows.Forms.GroupBox();
            this._folderSelectionCheckbox = new System.Windows.Forms.CheckBox();
            this._sourcePathsNoteLabel = new System.Windows.Forms.Label();
            this._sourcePathsButton = new System.Windows.Forms.Button();
            this._sourcePathsBox = new System.Windows.Forms.TextBox();
            this._sourcePathsLabel = new System.Windows.Forms.Label();
            this._settingsBox = new System.Windows.Forms.GroupBox();
            this._unitsBox = new System.Windows.Forms.ComboBox();
            this._unitsLabel = new System.Windows.Forms.Label();
            this._timeoutBox = new Aurigma.PhotoKiosk.ConfigurationTool.NumericTextBox();
            this._timeoutLabel = new System.Windows.Forms.Label();
            this._eventLoggingCheckbox = new System.Windows.Forms.CheckBox();
            this._searchTimeoutBox = new Aurigma.PhotoKiosk.ConfigurationTool.NumericTextBox();
            this._searchTimeoutLabel = new System.Windows.Forms.Label();
            this._cacheSizeBox = new Aurigma.PhotoKiosk.ConfigurationTool.NumericTextBox();
            this._cacheSizeLabel = new System.Windows.Forms.Label();
            this._photoKioskIdBox = new System.Windows.Forms.TextBox();
            this._photoKioskIdLabel = new System.Windows.Forms.Label();
            this._pathsBox.SuspendLayout();
            this._settingsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pathsBox
            // 
            this._pathsBox.Controls.Add(this._folderSelectionCheckbox);
            this._pathsBox.Controls.Add(this._sourcePathsNoteLabel);
            this._pathsBox.Controls.Add(this._sourcePathsButton);
            this._pathsBox.Controls.Add(this._sourcePathsBox);
            this._pathsBox.Controls.Add(this._sourcePathsLabel);
            resources.ApplyResources(this._pathsBox, "_pathsBox");
            this._pathsBox.Name = "_pathsBox";
            this._pathsBox.TabStop = false;
            // 
            // _folderSelectionCheckbox
            // 
            resources.ApplyResources(this._folderSelectionCheckbox, "_folderSelectionCheckbox");
            this._folderSelectionCheckbox.Name = "_folderSelectionCheckbox";
            this._folderSelectionCheckbox.UseVisualStyleBackColor = true;
            this._folderSelectionCheckbox.CheckedChanged += new System.EventHandler(this._folderSelectionCheckbox_CheckedChanged);
            // 
            // _sourcePathsNoteLabel
            // 
            resources.ApplyResources(this._sourcePathsNoteLabel, "_sourcePathsNoteLabel");
            this._sourcePathsNoteLabel.Name = "_sourcePathsNoteLabel";
            // 
            // _sourcePathsButton
            // 
            resources.ApplyResources(this._sourcePathsButton, "_sourcePathsButton");
            this._sourcePathsButton.Name = "_sourcePathsButton";
            this._sourcePathsButton.UseVisualStyleBackColor = true;
            this._sourcePathsButton.Click += new System.EventHandler(this._sourcePathsButton_Click);
            // 
            // _sourcePathsBox
            // 
            resources.ApplyResources(this._sourcePathsBox, "_sourcePathsBox");
            this._sourcePathsBox.Name = "_sourcePathsBox";
            this._sourcePathsBox.TextChanged += new System.EventHandler(this._sourcePathsBox_TextChanged);
            // 
            // _sourcePathsLabel
            // 
            resources.ApplyResources(this._sourcePathsLabel, "_sourcePathsLabel");
            this._sourcePathsLabel.Name = "_sourcePathsLabel";
            // 
            // _settingsBox
            // 
            this._settingsBox.Controls.Add(this._unitsBox);
            this._settingsBox.Controls.Add(this._unitsLabel);
            this._settingsBox.Controls.Add(this._timeoutBox);
            this._settingsBox.Controls.Add(this._timeoutLabel);
            this._settingsBox.Controls.Add(this._eventLoggingCheckbox);
            this._settingsBox.Controls.Add(this._searchTimeoutBox);
            this._settingsBox.Controls.Add(this._searchTimeoutLabel);
            this._settingsBox.Controls.Add(this._cacheSizeBox);
            this._settingsBox.Controls.Add(this._cacheSizeLabel);
            this._settingsBox.Controls.Add(this._photoKioskIdBox);
            this._settingsBox.Controls.Add(this._photoKioskIdLabel);
            resources.ApplyResources(this._settingsBox, "_settingsBox");
            this._settingsBox.Name = "_settingsBox";
            this._settingsBox.TabStop = false;
            // 
            // _unitsBox
            // 
            this._unitsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._unitsBox.FormattingEnabled = true;
            resources.ApplyResources(this._unitsBox, "_unitsBox");
            this._unitsBox.Name = "_unitsBox";
            this._unitsBox.SelectedIndexChanged += new System.EventHandler(this._unitsBox_SelectedIndexChanged);
            // 
            // _unitsLabel
            // 
            resources.ApplyResources(this._unitsLabel, "_unitsLabel");
            this._unitsLabel.Name = "_unitsLabel";
            // 
            // _timeoutBox
            // 
            this._timeoutBox.FloatValue = 0F;
            this._timeoutBox.IntValue = 0;
            this._timeoutBox.IsFloat = false;
            resources.ApplyResources(this._timeoutBox, "_timeoutBox");
            this._timeoutBox.Name = "_timeoutBox";
            this._timeoutBox.TextChanged += new System.EventHandler(this._timeoutBox_TextChanged);
            this._timeoutBox.Validating += new System.ComponentModel.CancelEventHandler(this._timeoutBox_Validating);
            // 
            // _timeoutLabel
            // 
            resources.ApplyResources(this._timeoutLabel, "_timeoutLabel");
            this._timeoutLabel.Name = "_timeoutLabel";
            // 
            // _eventLoggingCheckbox
            // 
            resources.ApplyResources(this._eventLoggingCheckbox, "_eventLoggingCheckbox");
            this._eventLoggingCheckbox.Name = "_eventLoggingCheckbox";
            this._eventLoggingCheckbox.UseVisualStyleBackColor = true;
            this._eventLoggingCheckbox.CheckedChanged += new System.EventHandler(this._eventLoggingCheckbox_CheckedChanged);
            // 
            // _searchTimeoutBox
            // 
            this._searchTimeoutBox.FloatValue = 0F;
            this._searchTimeoutBox.IntValue = 0;
            this._searchTimeoutBox.IsFloat = false;
            resources.ApplyResources(this._searchTimeoutBox, "_searchTimeoutBox");
            this._searchTimeoutBox.Name = "_searchTimeoutBox";
            this._searchTimeoutBox.TextChanged += new System.EventHandler(this._searchTimeoutBox_TextChanged);
            this._searchTimeoutBox.Validating += new System.ComponentModel.CancelEventHandler(this._searchTimeoutBox_Validating);
            // 
            // _searchTimeoutLabel
            // 
            resources.ApplyResources(this._searchTimeoutLabel, "_searchTimeoutLabel");
            this._searchTimeoutLabel.Name = "_searchTimeoutLabel";
            // 
            // _cacheSizeBox
            // 
            this._cacheSizeBox.FloatValue = 0F;
            this._cacheSizeBox.IntValue = 0;
            this._cacheSizeBox.IsFloat = false;
            resources.ApplyResources(this._cacheSizeBox, "_cacheSizeBox");
            this._cacheSizeBox.Name = "_cacheSizeBox";
            this._cacheSizeBox.TextChanged += new System.EventHandler(this._cacheSizeBox_TextChanged);
            this._cacheSizeBox.Validating += new System.ComponentModel.CancelEventHandler(this._cacheSizeBox_Validating);
            // 
            // _cacheSizeLabel
            // 
            resources.ApplyResources(this._cacheSizeLabel, "_cacheSizeLabel");
            this._cacheSizeLabel.Name = "_cacheSizeLabel";
            // 
            // _photoKioskIdBox
            // 
            resources.ApplyResources(this._photoKioskIdBox, "_photoKioskIdBox");
            this._photoKioskIdBox.Name = "_photoKioskIdBox";
            this._photoKioskIdBox.TextChanged += new System.EventHandler(this._photoKioskIdBox_TextChanged);
            // 
            // _photoKioskIdLabel
            // 
            resources.ApplyResources(this._photoKioskIdLabel, "_photoKioskIdLabel");
            this._photoKioskIdLabel.Name = "_photoKioskIdLabel";
            // 
            // BehaviourPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._pathsBox);
            this.Controls.Add(this._settingsBox);
            this.Name = "BehaviourPanel";
            this.Load += new System.EventHandler(this.BehaviourPanel_Load);
            this._pathsBox.ResumeLayout(false);
            this._pathsBox.PerformLayout();
            this._settingsBox.ResumeLayout(false);
            this._settingsBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _settingsBox;
        private System.Windows.Forms.GroupBox _pathsBox;
        private System.Windows.Forms.Label _photoKioskIdLabel;
        private System.Windows.Forms.TextBox _photoKioskIdBox;
        private System.Windows.Forms.CheckBox _eventLoggingCheckbox;
        private System.Windows.Forms.Label _cacheSizeLabel;
        private NumericTextBox _cacheSizeBox;
        private NumericTextBox _searchTimeoutBox;
        private System.Windows.Forms.Label _searchTimeoutLabel;
        private NumericTextBox _timeoutBox;
        private System.Windows.Forms.Label _timeoutLabel;
        private System.Windows.Forms.TextBox _sourcePathsBox;
        private System.Windows.Forms.Label _sourcePathsLabel;
        private System.Windows.Forms.Button _sourcePathsButton;
        private System.Windows.Forms.Label _sourcePathsNoteLabel;
        private System.Windows.Forms.CheckBox _folderSelectionCheckbox;
        private System.Windows.Forms.Label _unitsLabel;
        private System.Windows.Forms.ComboBox _unitsBox;
    }
}
