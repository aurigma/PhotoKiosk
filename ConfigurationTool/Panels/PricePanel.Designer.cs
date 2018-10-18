namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class PricePanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PricePanel));
            this._priceFileButton = new System.Windows.Forms.Button();
            this._priceFileBox = new System.Windows.Forms.TextBox();
            this._priceFileLable = new System.Windows.Forms.Label();
            this._commentBox = new System.Windows.Forms.TextBox();
            this._taxComment = new System.Windows.Forms.Label();
            this._salesTaxBox = new Aurigma.PhotoKiosk.ConfigurationTool.NumericTextBox();
            this._salesTaxLabel = new System.Windows.Forms.Label();
            this._minPriceBox = new Aurigma.PhotoKiosk.ConfigurationTool.NumericTextBox();
            this._minPriceLabel = new System.Windows.Forms.Label();
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
            // _priceFileButton
            // 
            resources.ApplyResources(this._priceFileButton, "_priceFileButton");
            this._priceFileButton.Name = "_priceFileButton";
            this._priceFileButton.UseVisualStyleBackColor = true;
            this._priceFileButton.Click += new System.EventHandler(this._priceFileButton_Click);
            // 
            // _priceFileBox
            // 
            resources.ApplyResources(this._priceFileBox, "_priceFileBox");
            this._priceFileBox.Name = "_priceFileBox";
            this._priceFileBox.TextChanged += new System.EventHandler(this._priceFileBox_TextChanged);
            this._priceFileBox.Validating += new System.ComponentModel.CancelEventHandler(this._priceFileBox_Validating);
            // 
            // _priceFileLable
            // 
            resources.ApplyResources(this._priceFileLable, "_priceFileLable");
            this._priceFileLable.Name = "_priceFileLable";
            // 
            // _commentBox
            // 
            resources.ApplyResources(this._commentBox, "_commentBox");
            this._commentBox.Name = "_commentBox";
            this._commentBox.TextChanged += new System.EventHandler(this._commentBox_TextChanged);
            // 
            // _taxComment
            // 
            resources.ApplyResources(this._taxComment, "_taxComment");
            this._taxComment.Name = "_taxComment";
            // 
            // _salesTaxBox
            // 
            resources.ApplyResources(this._salesTaxBox, "_salesTaxBox");
            this._salesTaxBox.FloatValue = 0F;
            this._salesTaxBox.IntValue = 0;
            this._salesTaxBox.IsFloat = true;
            this._salesTaxBox.Name = "_salesTaxBox";
            this._salesTaxBox.TextChanged += new System.EventHandler(this._salesTaxBox_TextChanged);
            // 
            // _salesTaxLabel
            // 
            resources.ApplyResources(this._salesTaxLabel, "_salesTaxLabel");
            this._salesTaxLabel.Name = "_salesTaxLabel";
            // 
            // _minPriceBox
            // 
            resources.ApplyResources(this._minPriceBox, "_minPriceBox");
            this._minPriceBox.FloatValue = 0F;
            this._minPriceBox.IntValue = 0;
            this._minPriceBox.IsFloat = true;
            this._minPriceBox.Name = "_minPriceBox";
            this._minPriceBox.TextChanged += new System.EventHandler(this._minPriceBox_TextChanged);
            // 
            // _minPriceLabel
            // 
            resources.ApplyResources(this._minPriceLabel, "_minPriceLabel");
            this._minPriceLabel.Name = "_minPriceLabel";
            // 
            // PricePanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._priceFileButton);
            this.Controls.Add(this._priceFileBox);
            this.Controls.Add(this._priceFileLable);
            this.Controls.Add(this._commentBox);
            this.Controls.Add(this._taxComment);
            this.Controls.Add(this._salesTaxBox);
            this.Controls.Add(this._salesTaxLabel);
            this.Controls.Add(this._minPriceBox);
            this.Controls.Add(this._minPriceLabel);
            this.Name = "PricePanel";
            this.Load += new System.EventHandler(this.PricePanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _minPriceLabel;
        private NumericTextBox _minPriceBox;
        private System.Windows.Forms.Label _salesTaxLabel;
        private NumericTextBox _salesTaxBox;
        private System.Windows.Forms.Label _taxComment;
        private System.Windows.Forms.TextBox _commentBox;
        private System.Windows.Forms.Button _priceFileButton;
        private System.Windows.Forms.TextBox _priceFileBox;
        private System.Windows.Forms.Label _priceFileLable;
    }
}
