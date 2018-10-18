namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class ServicesPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServicesPanel));
            this._servicesView = new Aurigma.PhotoKiosk.ConfigurationTool.SortableDataGridView();
            this.ServiceNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._servicesView)).BeginInit();
            this.SuspendLayout();
            // 
            // _servicesView
            // 
            this._servicesView.AllowDrop = true;
            this._servicesView.AllowUserToResizeColumns = false;
            this._servicesView.AllowUserToResizeRows = false;
            this._servicesView.BackgroundColor = System.Drawing.SystemColors.Control;
            this._servicesView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._servicesView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ServiceNameColumn});
            this._servicesView.GridColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this._servicesView, "_servicesView");
            this._servicesView.Name = "_servicesView";
            this._servicesView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._servicesView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._servicesView_CellValueChanged);
            this._servicesView.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this._servicesView_UserDeletedRow);
            this._servicesView.DragDrop += new System.Windows.Forms.DragEventHandler(this._servicesView_DragDrop);
            this._servicesView.Validating += new System.ComponentModel.CancelEventHandler(this._servicesView_Validating);
            // 
            // ServiceNameColumn
            // 
            resources.ApplyResources(this.ServiceNameColumn, "ServiceNameColumn");
            this.ServiceNameColumn.Name = "ServiceNameColumn";
            this.ServiceNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ServicesPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._servicesView);
            this.Name = "ServicesPanel";
            this.Load += new System.EventHandler(this.ServicesPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this._servicesView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SortableDataGridView _servicesView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceNameColumn;
    }
}
