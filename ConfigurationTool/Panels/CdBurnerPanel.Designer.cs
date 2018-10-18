namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class CdBurnerPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CdBurnerPanel));
            this._confirmCheckbox = new System.Windows.Forms.CheckBox();
            this._priceBox = new Aurigma.PhotoKiosk.ConfigurationTool.NumericTextBox();
            this._priceLabel = new System.Windows.Forms.Label();
            this._discLabelBox = new System.Windows.Forms.TextBox();
            this._discLabel = new System.Windows.Forms.Label();
            this._driveLabel = new System.Windows.Forms.Label();
            this._driveBox = new System.Windows.Forms.ComboBox();
            this._cdBurnerCheckbox = new System.Windows.Forms.CheckBox();
            this._paymentInfoLabel = new System.Windows.Forms.Label();
            this._paymentInfoBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _confirmCheckbox
            // 
            resources.ApplyResources(this._confirmCheckbox, "_confirmCheckbox");
            this._confirmCheckbox.Name = "_confirmCheckbox";
            this._confirmCheckbox.UseVisualStyleBackColor = true;
            this._confirmCheckbox.CheckedChanged += new System.EventHandler(this._confirmCheckbox_CheckedChanged);
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
            // _priceLabel
            // 
            resources.ApplyResources(this._priceLabel, "_priceLabel");
            this._priceLabel.Name = "_priceLabel";
            // 
            // _discLabelBox
            // 
            resources.ApplyResources(this._discLabelBox, "_discLabelBox");
            this._discLabelBox.Name = "_discLabelBox";
            this._discLabelBox.TextChanged += new System.EventHandler(this._discLabelBox_TextChanged);
            this._discLabelBox.Validating += new System.ComponentModel.CancelEventHandler(this._discLabelBox_Validating);
            // 
            // _discLabel
            // 
            resources.ApplyResources(this._discLabel, "_discLabel");
            this._discLabel.Name = "_discLabel";
            // 
            // _driveLabel
            // 
            resources.ApplyResources(this._driveLabel, "_driveLabel");
            this._driveLabel.Name = "_driveLabel";
            // 
            // _driveBox
            // 
            this._driveBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._driveBox.FormattingEnabled = true;
            resources.ApplyResources(this._driveBox, "_driveBox");
            this._driveBox.Name = "_driveBox";
            this._driveBox.SelectedIndexChanged += new System.EventHandler(this._driveBox_SelectedIndexChanged);
            // 
            // _cdBurnerCheckbox
            // 
            resources.ApplyResources(this._cdBurnerCheckbox, "_cdBurnerCheckbox");
            this._cdBurnerCheckbox.Name = "_cdBurnerCheckbox";
            this._cdBurnerCheckbox.UseVisualStyleBackColor = true;
            this._cdBurnerCheckbox.CheckedChanged += new System.EventHandler(this._cdBurnerCheckbox_CheckedChanged);
            // 
            // _paymentInfoLabel
            // 
            resources.ApplyResources(this._paymentInfoLabel, "_paymentInfoLabel");
            this._paymentInfoLabel.Name = "_paymentInfoLabel";
            // 
            // _paymentInfoBox
            // 
            resources.ApplyResources(this._paymentInfoBox, "_paymentInfoBox");
            this._paymentInfoBox.Name = "_paymentInfoBox";
            this._paymentInfoBox.TextChanged += new System.EventHandler(this._paymentInfoBox_TextChanged);
            // 
            // CdBurnerPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._paymentInfoBox);
            this.Controls.Add(this._paymentInfoLabel);
            this.Controls.Add(this._confirmCheckbox);
            this.Controls.Add(this._priceBox);
            this.Controls.Add(this._priceLabel);
            this.Controls.Add(this._discLabelBox);
            this.Controls.Add(this._discLabel);
            this.Controls.Add(this._driveLabel);
            this.Controls.Add(this._driveBox);
            this.Controls.Add(this._cdBurnerCheckbox);
            this.Name = "CdBurnerPanel";
            this.Load += new System.EventHandler(this.CdBurnerPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _cdBurnerCheckbox;
        private System.Windows.Forms.ComboBox _driveBox;
        private System.Windows.Forms.Label _driveLabel;
        private System.Windows.Forms.Label _discLabel;
        private System.Windows.Forms.TextBox _discLabelBox;
        private System.Windows.Forms.Label _priceLabel;
        private NumericTextBox _priceBox;
        private System.Windows.Forms.CheckBox _confirmCheckbox;
        private System.Windows.Forms.Label _paymentInfoLabel;
        private System.Windows.Forms.TextBox _paymentInfoBox;
    }
}
