namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class FormatPricePanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormatPricePanel));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this._discountsBox = new System.Windows.Forms.GroupBox();
            this._discountsView = new System.Windows.Forms.DataGridView();
            this.FromColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._nameLabel = new System.Windows.Forms.Label();
            this._nameBox = new System.Windows.Forms.TextBox();
            this._discountsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._discountsView)).BeginInit();
            this.SuspendLayout();
            // 
            // _discountsBox
            // 
            this._discountsBox.Controls.Add(this._discountsView);
            resources.ApplyResources(this._discountsBox, "_discountsBox");
            this._discountsBox.Name = "_discountsBox";
            this._discountsBox.TabStop = false;
            // 
            // _discountsView
            // 
            this._discountsView.AllowUserToAddRows = false;
            this._discountsView.AllowUserToResizeColumns = false;
            this._discountsView.AllowUserToResizeRows = false;
            this._discountsView.BackgroundColor = System.Drawing.SystemColors.Control;
            this._discountsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._discountsView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FromColumn,
            this.ToColumn,
            this.PriceColumn});
            resources.ApplyResources(this._discountsView, "_discountsView");
            this._discountsView.Name = "_discountsView";
            this._discountsView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._discountsView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this._discountsView_CellFormatting);
            this._discountsView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._discountsView_CellValueChanged);
            this._discountsView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this._discountsView_EditingControlShowing);
            this._discountsView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this._discountsView_UserDeletingRow);
            this._discountsView.Validating += new System.ComponentModel.CancelEventHandler(this._discountsView_Validating);
            // 
            // FromColumn
            // 
            this.FromColumn.Frozen = true;
            resources.ApplyResources(this.FromColumn, "FromColumn");
            this.FromColumn.Name = "FromColumn";
            this.FromColumn.ReadOnly = true;
            this.FromColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ToColumn
            // 
            this.ToColumn.Frozen = true;
            resources.ApplyResources(this.ToColumn, "ToColumn");
            this.ToColumn.Name = "ToColumn";
            this.ToColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PriceColumn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.PriceColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.PriceColumn.Frozen = true;
            resources.ApplyResources(this.PriceColumn, "PriceColumn");
            this.PriceColumn.Name = "PriceColumn";
            this.PriceColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _nameLabel
            // 
            resources.ApplyResources(this._nameLabel, "_nameLabel");
            this._nameLabel.Name = "_nameLabel";
            // 
            // _nameBox
            // 
            resources.ApplyResources(this._nameBox, "_nameBox");
            this._nameBox.Name = "_nameBox";
            this._nameBox.ReadOnly = true;
            // 
            // FormatPricePanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._nameBox);
            this.Controls.Add(this._nameLabel);
            this.Controls.Add(this._discountsBox);
            this.Name = "FormatPricePanel";
            this._discountsBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._discountsView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox _discountsBox;
        private System.Windows.Forms.DataGridView _discountsView;
        private System.Windows.Forms.Label _nameLabel;
        private System.Windows.Forms.TextBox _nameBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn FromColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ToColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceColumn;
    }
}
