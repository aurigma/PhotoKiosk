namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class PaperFormatsPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaperFormatsPanel));
            this._formatsView = new Aurigma.PhotoKiosk.ConfigurationTool.SortableDataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WidthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HeightColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DpiColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._formatsView)).BeginInit();
            this.SuspendLayout();
            // 
            // _formatsView
            // 
            this._formatsView.AllowDrop = true;
            this._formatsView.AllowUserToResizeRows = false;
            this._formatsView.BackgroundColor = System.Drawing.SystemColors.Control;
            this._formatsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._formatsView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.WidthColumn,
            this.HeightColumn,
            this.DpiColumn});
            this._formatsView.GridColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this._formatsView, "_formatsView");
            this._formatsView.MultiSelect = false;
            this._formatsView.Name = "_formatsView";
            this._formatsView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._formatsView.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this._formatsView_CellMouseEnter);
            this._formatsView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._formatsView_CellValueChanged);
            this._formatsView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this._formatsView_EditingControlShowing);
            this._formatsView.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this._formatsView_UserDeletedRow);
            this._formatsView.DragDrop += new System.Windows.Forms.DragEventHandler(this._formatsView_DragDrop);
            this._formatsView.Validating += new System.ComponentModel.CancelEventHandler(this._formatsView_Validating);
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
            // DpiColumn
            // 
            resources.ApplyResources(this.DpiColumn, "DpiColumn");
            this.DpiColumn.Name = "DpiColumn";
            // 
            // PaperFormatsPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._formatsView);
            this.Name = "PaperFormatsPanel";
            this.Load += new System.EventHandler(this.PaperFormatsPanel_Load);
            this.VisibleChanged += new System.EventHandler(this.PaperFormatsPanel_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this._formatsView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SortableDataGridView _formatsView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn WidthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn HeightColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DpiColumn;
    }
}
