namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class CallbackPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CallbackPanel));
            this._startButton = new System.Windows.Forms.Button();
            this._startBox = new System.Windows.Forms.TextBox();
            this._startLabel = new System.Windows.Forms.Label();
            this._completeLabel = new System.Windows.Forms.Label();
            this._completeButton = new System.Windows.Forms.Button();
            this._completeBox = new System.Windows.Forms.TextBox();
            this._cancelLabel = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this._cancelBox = new System.Windows.Forms.TextBox();
            this._callbackNote = new System.Windows.Forms.Label();
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
            // _startButton
            // 
            resources.ApplyResources(this._startButton, "_startButton");
            this._startButton.Name = "_startButton";
            this._startButton.UseVisualStyleBackColor = true;
            this._startButton.Click += new System.EventHandler(this._startButton_Click);
            // 
            // _startBox
            // 
            resources.ApplyResources(this._startBox, "_startBox");
            this._startBox.Name = "_startBox";
            this._startBox.TextChanged += new System.EventHandler(this._startBox_TextChanged);
            this._startBox.Validating += new System.ComponentModel.CancelEventHandler(this._box_Validating);
            // 
            // _startLabel
            // 
            resources.ApplyResources(this._startLabel, "_startLabel");
            this._startLabel.Name = "_startLabel";
            // 
            // _completeLabel
            // 
            resources.ApplyResources(this._completeLabel, "_completeLabel");
            this._completeLabel.Name = "_completeLabel";
            // 
            // _completeButton
            // 
            resources.ApplyResources(this._completeButton, "_completeButton");
            this._completeButton.Name = "_completeButton";
            this._completeButton.UseVisualStyleBackColor = true;
            this._completeButton.Click += new System.EventHandler(this._completeButton_Click);
            // 
            // _completeBox
            // 
            resources.ApplyResources(this._completeBox, "_completeBox");
            this._completeBox.Name = "_completeBox";
            this._completeBox.TextChanged += new System.EventHandler(this._completeBox_TextChanged);
            this._completeBox.Validating += new System.ComponentModel.CancelEventHandler(this._box_Validating);
            // 
            // _cancelLabel
            // 
            resources.ApplyResources(this._cancelLabel, "_cancelLabel");
            this._cancelLabel.Name = "_cancelLabel";
            // 
            // _cancelButton
            // 
            resources.ApplyResources(this._cancelButton, "_cancelButton");
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // _cancelBox
            // 
            resources.ApplyResources(this._cancelBox, "_cancelBox");
            this._cancelBox.Name = "_cancelBox";
            this._cancelBox.TextChanged += new System.EventHandler(this._cancelBox_TextChanged);
            this._cancelBox.Validating += new System.ComponentModel.CancelEventHandler(this._box_Validating);
            // 
            // _callbackNote
            // 
            resources.ApplyResources(this._callbackNote, "_callbackNote");
            this._callbackNote.Name = "_callbackNote";
            // 
            // CallbackPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._callbackNote);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._cancelBox);
            this.Controls.Add(this._cancelLabel);
            this.Controls.Add(this._completeButton);
            this.Controls.Add(this._completeBox);
            this.Controls.Add(this._completeLabel);
            this.Controls.Add(this._startLabel);
            this.Controls.Add(this._startButton);
            this.Controls.Add(this._startBox);
            this.Name = "CallbackPanel";
            this.Load += new System.EventHandler(this.CallbackPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _startButton;
        private System.Windows.Forms.TextBox _startBox;
        private System.Windows.Forms.Label _startLabel;
        private System.Windows.Forms.Label _completeLabel;
        private System.Windows.Forms.Button _completeButton;
        private System.Windows.Forms.TextBox _completeBox;
        private System.Windows.Forms.Label _cancelLabel;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.TextBox _cancelBox;
        private System.Windows.Forms.Label _callbackNote;
    }
}
