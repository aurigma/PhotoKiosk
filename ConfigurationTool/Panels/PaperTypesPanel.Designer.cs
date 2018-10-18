namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class PaperTypesPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaperTypesPanel));
            this._typesView = new Aurigma.PhotoKiosk.ConfigurationTool.SortableDataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._typesView)).BeginInit();
            this.SuspendLayout();
            // 
            // _typesView
            // 
            this._typesView.AllowDrop = true;
            this._typesView.AllowUserToResizeRows = false;
            this._typesView.BackgroundColor = System.Drawing.SystemColors.Control;
            this._typesView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._typesView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.DescriptionColumn});
            this._typesView.GridColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this._typesView, "_typesView");
            this._typesView.MultiSelect = false;
            this._typesView.Name = "_typesView";
            this._typesView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._typesView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._typesView_CellValueChanged);
            this._typesView.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this._typesView_UserDeletedRow);
            this._typesView.DragDrop += new System.Windows.Forms.DragEventHandler(this._typesView_DragDrop);
            this._typesView.Validating += new System.ComponentModel.CancelEventHandler(this._typesView_Validating);
            // 
            // NameColumn
            // 
            resources.ApplyResources(this.NameColumn, "NameColumn");
            this.NameColumn.Name = "NameColumn";
            // 
            // DescriptionColumn
            // 
            resources.ApplyResources(this.DescriptionColumn, "DescriptionColumn");
            this.DescriptionColumn.Name = "DescriptionColumn";
            // 
            // PaperTypesPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._typesView);
            this.Name = "PaperTypesPanel";
            this.Load += new System.EventHandler(this.PaperTypesPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this._typesView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SortableDataGridView _typesView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionColumn;
    }
}
