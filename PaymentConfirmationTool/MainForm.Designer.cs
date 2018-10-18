namespace Aurigma.PhotoKiosk.PaymentConfirmationTool
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._orderIdLabel = new System.Windows.Forms.Label();
            this._orderIdBox = new System.Windows.Forms.TextBox();
            this._generateButton = new System.Windows.Forms.Button();
            this._codeLabel = new System.Windows.Forms.Label();
            this._codeBox = new System.Windows.Forms.TextBox();
            this._printButton = new System.Windows.Forms.Button();
            this._printDialog = new System.Windows.Forms.PrintDialog();
            this.SuspendLayout();
            // 
            // _orderIdLabel
            // 
            resources.ApplyResources(this._orderIdLabel, "_orderIdLabel");
            this._orderIdLabel.Name = "_orderIdLabel";
            // 
            // _orderIdBox
            // 
            resources.ApplyResources(this._orderIdBox, "_orderIdBox");
            this._orderIdBox.Name = "_orderIdBox";
            this._orderIdBox.TextChanged += new System.EventHandler(this._orderIdBox_TextChanged);
            this._orderIdBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._orderIdBox_KeyDown);
            // 
            // _generateButton
            // 
            resources.ApplyResources(this._generateButton, "_generateButton");
            this._generateButton.Name = "_generateButton";
            this._generateButton.UseVisualStyleBackColor = true;
            this._generateButton.Click += new System.EventHandler(this._generateButton_Click);
            // 
            // _codeLabel
            // 
            resources.ApplyResources(this._codeLabel, "_codeLabel");
            this._codeLabel.Name = "_codeLabel";
            // 
            // _codeBox
            // 
            resources.ApplyResources(this._codeBox, "_codeBox");
            this._codeBox.Name = "_codeBox";
            this._codeBox.ReadOnly = true;
            this._codeBox.TextChanged += new System.EventHandler(this._codeBox_TextChanged);
            // 
            // _printButton
            // 
            resources.ApplyResources(this._printButton, "_printButton");
            this._printButton.Name = "_printButton";
            this._printButton.UseVisualStyleBackColor = true;
            this._printButton.Click += new System.EventHandler(this._printButton_Click);
            // 
            // _printDialog
            // 
            this._printDialog.UseEXDialog = true;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._printButton);
            this.Controls.Add(this._codeBox);
            this.Controls.Add(this._codeLabel);
            this.Controls.Add(this._generateButton);
            this.Controls.Add(this._orderIdBox);
            this.Controls.Add(this._orderIdLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _orderIdLabel;
        private System.Windows.Forms.TextBox _orderIdBox;
        private System.Windows.Forms.Button _generateButton;
        private System.Windows.Forms.Label _codeLabel;
        private System.Windows.Forms.TextBox _codeBox;
        private System.Windows.Forms.Button _printButton;
        private System.Windows.Forms.PrintDialog _printDialog;
    }
}

