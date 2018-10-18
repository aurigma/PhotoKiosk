namespace Aurigma.PhotoKiosk.ConfigurationTool.Panels
{
    partial class CropsPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CropsPanel));
            this._cropsView = new Aurigma.PhotoKiosk.ConfigurationTool.SortableDataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WidthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HeightColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._cropsView)).BeginInit();
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
            // _cropsView
            // 
            resources.ApplyResources(this._cropsView, "_cropsView");
            this._cropsView.AllowDrop = true;
            this._cropsView.AllowUserToResizeRows = false;
            this._cropsView.BackgroundColor = System.Drawing.SystemColors.Control;
            this._cropsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._cropsView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.WidthColumn,
            this.HeightColumn});
            this._cropsView.GridColor = System.Drawing.SystemColors.Control;
            this._cropsView.MultiSelect = false;
            this._cropsView.Name = "_cropsView";
            this._cropsView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._cropsView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._cropsView_CellValueChanged);
            this._cropsView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this._cropsView_EditingControlShowing);
            this._cropsView.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this._cropsView_UserAddedRow);
            this._cropsView.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this._cropsView_UserDeletedRow);
            this._cropsView.DragDrop += new System.Windows.Forms.DragEventHandler(this._cropsView_DragDrop);
            this._cropsView.Validating += new System.ComponentModel.CancelEventHandler(this._cropsView_Validating);
            // 
            // NameColumn
            // 
            resources.ApplyResources(this.NameColumn, "NameColumn");
            this.NameColumn.Name = "NameColumn";
            // 
            // WidthColumn
            // 
            resources.ApplyResources(this.WidthColumn, "WidthColumn");
            this.WidthColumn.Name = "WidthColumn";
            // 
            // HeightColumn
            // 
            resources.ApplyResources(this.HeightColumn, "HeightColumn");
            this.HeightColumn.Name = "HeightColumn";
            // 
            // CropsPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._cropsView);
            this.Name = "CropsPanel";
            this.Load += new System.EventHandler(this.CropsPanel_Load);
            this.VisibleChanged += new System.EventHandler(this.CropsPanel_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this._cropsView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SortableDataGridView _cropsView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn WidthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn HeightColumn;
    }
}
