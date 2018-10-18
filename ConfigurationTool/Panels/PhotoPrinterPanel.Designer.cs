namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class PhotoPrinterPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhotoPrinterPanel));
            this._photoPrinterCheckbox = new System.Windows.Forms.CheckBox();
            this._printersView = new System.Windows.Forms.DataGridView();
            this._confirmCheckbox = new System.Windows.Forms.CheckBox();
            this._paymentInfoBox = new System.Windows.Forms.TextBox();
            this._paymentInfoLabel = new System.Windows.Forms.Label();
            this.PaperColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrinterColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._printersView)).BeginInit();
            this.SuspendLayout();
            // 
            // _photoPrinterCheckbox
            // 
            resources.ApplyResources(this._photoPrinterCheckbox, "_photoPrinterCheckbox");
            this._photoPrinterCheckbox.Name = "_photoPrinterCheckbox";
            this._photoPrinterCheckbox.UseVisualStyleBackColor = true;
            this._photoPrinterCheckbox.CheckedChanged += new System.EventHandler(this._photoPrinterCheckbox_CheckedChanged);
            // 
            // _printersView
            // 
            this._printersView.AllowUserToAddRows = false;
            this._printersView.AllowUserToDeleteRows = false;
            this._printersView.AllowUserToResizeRows = false;
            this._printersView.BackgroundColor = System.Drawing.SystemColors.Control;
            this._printersView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._printersView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PaperColumn,
            this.PrinterColumn});
            this._printersView.GridColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this._printersView, "_printersView");
            this._printersView.MultiSelect = false;
            this._printersView.Name = "_printersView";
            this._printersView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._printersView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._printersView_CellValueChanged);
            // 
            // _confirmCheckbox
            // 
            resources.ApplyResources(this._confirmCheckbox, "_confirmCheckbox");
            this._confirmCheckbox.Name = "_confirmCheckbox";
            this._confirmCheckbox.UseVisualStyleBackColor = true;
            this._confirmCheckbox.CheckedChanged += new System.EventHandler(this._confirmCheckbox_CheckedChanged);
            // 
            // _paymentInfoBox
            // 
            resources.ApplyResources(this._paymentInfoBox, "_paymentInfoBox");
            this._paymentInfoBox.Name = "_paymentInfoBox";
            this._paymentInfoBox.TextChanged += new System.EventHandler(this._paymentInfoBox_TextChanged);
            // 
            // _paymentInfoLabel
            // 
            resources.ApplyResources(this._paymentInfoLabel, "_paymentInfoLabel");
            this._paymentInfoLabel.Name = "_paymentInfoLabel";
            // 
            // PaperColumn
            // 
            resources.ApplyResources(this.PaperColumn, "PaperColumn");
            this.PaperColumn.Name = "PaperColumn";
            this.PaperColumn.ReadOnly = true;
            // 
            // PrinterColumn
            // 
            resources.ApplyResources(this.PrinterColumn, "PrinterColumn");
            this.PrinterColumn.Name = "PrinterColumn";
            // 
            // PhotoPrinterPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._paymentInfoBox);
            this.Controls.Add(this._paymentInfoLabel);
            this.Controls.Add(this._confirmCheckbox);
            this.Controls.Add(this._printersView);
            this.Controls.Add(this._photoPrinterCheckbox);
            this.Name = "PhotoPrinterPanel";
            this.Load += new System.EventHandler(this.PhotoPrinterPanel_Load);
            this.VisibleChanged += new System.EventHandler(this.PhotoPrinterPanel_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this._printersView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _photoPrinterCheckbox;
        private System.Windows.Forms.DataGridView _printersView;
        private System.Windows.Forms.CheckBox _confirmCheckbox;
        private System.Windows.Forms.TextBox _paymentInfoBox;
        private System.Windows.Forms.Label _paymentInfoLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaperColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn PrinterColumn;
    }
}
