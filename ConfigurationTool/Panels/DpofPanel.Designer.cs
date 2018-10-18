namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class DpofPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DpofPanel));
            this._channelsView = new System.Windows.Forms.DataGridView();
            this._clearOrderInfoTemplateButton = new System.Windows.Forms.Button();
            this._orderInfoTemplateBox = new System.Windows.Forms.TextBox();
            this._orderInfoTemplateLabel = new System.Windows.Forms.Label();
            this._orderTemplateLabel = new System.Windows.Forms.Label();
            this._tagsLabel = new System.Windows.Forms.Label();
            this._insertTagButton = new System.Windows.Forms.Button();
            this._tagsBox = new System.Windows.Forms.ComboBox();
            this._clearOrderTemplateButton = new System.Windows.Forms.Button();
            this._orderTemplateBox = new System.Windows.Forms.TextBox();
            this._paperColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._channelColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._channelsView)).BeginInit();
            this.SuspendLayout();
            // 
            // _channelsView
            // 
            this._channelsView.AllowUserToAddRows = false;
            this._channelsView.AllowUserToDeleteRows = false;
            this._channelsView.AllowUserToResizeRows = false;
            this._channelsView.BackgroundColor = System.Drawing.SystemColors.Control;
            this._channelsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._channelsView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._paperColumn,
            this._channelColumn});
            this._channelsView.GridColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this._channelsView, "_channelsView");
            this._channelsView.MultiSelect = false;
            this._channelsView.Name = "_channelsView";
            this._channelsView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._channelsView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._channelsView_CellValueChanged);
            this._channelsView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this._channelsView_EditingControlShowing);
            this._channelsView.Validating += new System.ComponentModel.CancelEventHandler(this._channelsView_Validating);
            // 
            // _clearOrderInfoTemplateButton
            // 
            resources.ApplyResources(this._clearOrderInfoTemplateButton, "_clearOrderInfoTemplateButton");
            this._clearOrderInfoTemplateButton.Name = "_clearOrderInfoTemplateButton";
            this._clearOrderInfoTemplateButton.UseVisualStyleBackColor = true;
            this._clearOrderInfoTemplateButton.Click += new System.EventHandler(this._clearOrderInfoTemplateButton_Click);
            // 
            // _orderInfoTemplateBox
            // 
            resources.ApplyResources(this._orderInfoTemplateBox, "_orderInfoTemplateBox");
            this._orderInfoTemplateBox.Name = "_orderInfoTemplateBox";
            this._orderInfoTemplateBox.TextChanged += new System.EventHandler(this._orderInfoTemplateBox_TextChanged);
            this._orderInfoTemplateBox.Enter += new System.EventHandler(this._textBox_Enter);
            // 
            // _orderInfoTemplateLabel
            // 
            resources.ApplyResources(this._orderInfoTemplateLabel, "_orderInfoTemplateLabel");
            this._orderInfoTemplateLabel.Name = "_orderInfoTemplateLabel";
            // 
            // _orderTemplateLabel
            // 
            resources.ApplyResources(this._orderTemplateLabel, "_orderTemplateLabel");
            this._orderTemplateLabel.Name = "_orderTemplateLabel";
            this._orderTemplateLabel.Click += new System.EventHandler(this._orderTemplateLabel_Click);
            // 
            // _tagsLabel
            // 
            resources.ApplyResources(this._tagsLabel, "_tagsLabel");
            this._tagsLabel.Name = "_tagsLabel";
            // 
            // _insertTagButton
            // 
            resources.ApplyResources(this._insertTagButton, "_insertTagButton");
            this._insertTagButton.Name = "_insertTagButton";
            this._insertTagButton.UseVisualStyleBackColor = true;
            this._insertTagButton.Click += new System.EventHandler(this._insertTagButton_Click);
            // 
            // _tagsBox
            // 
            this._tagsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._tagsBox.FormattingEnabled = true;
            resources.ApplyResources(this._tagsBox, "_tagsBox");
            this._tagsBox.Name = "_tagsBox";
            // 
            // _clearOrderTemplateButton
            // 
            resources.ApplyResources(this._clearOrderTemplateButton, "_clearOrderTemplateButton");
            this._clearOrderTemplateButton.Name = "_clearOrderTemplateButton";
            this._clearOrderTemplateButton.UseVisualStyleBackColor = true;
            this._clearOrderTemplateButton.Click += new System.EventHandler(this._clearOrderTemplateButton_Click);
            // 
            // _orderTemplateBox
            // 
            resources.ApplyResources(this._orderTemplateBox, "_orderTemplateBox");
            this._orderTemplateBox.Name = "_orderTemplateBox";
            this._orderTemplateBox.TextChanged += new System.EventHandler(this._orderTemplateBox_TextChanged);
            this._orderTemplateBox.Enter += new System.EventHandler(this._textBox_Enter);
            // 
            // PaperColumn
            // 
            resources.ApplyResources(this._paperColumn, "PaperColumn");
            this._paperColumn.Name = "PaperColumn";
            this._paperColumn.ReadOnly = true;
            // 
            // ChannelColumn
            // 
            resources.ApplyResources(this._channelColumn, "ChannelColumn");
            this._channelColumn.Name = "ChannelColumn";
            this._channelColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._channelColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DpofPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._channelsView);
            this.Controls.Add(this._clearOrderInfoTemplateButton);
            this.Controls.Add(this._orderInfoTemplateBox);
            this.Controls.Add(this._orderInfoTemplateLabel);
            this.Controls.Add(this._orderTemplateLabel);
            this.Controls.Add(this._tagsLabel);
            this.Controls.Add(this._insertTagButton);
            this.Controls.Add(this._tagsBox);
            this.Controls.Add(this._clearOrderTemplateButton);
            this.Controls.Add(this._orderTemplateBox);
            this.Name = "DpofPanel";
            this.Load += new System.EventHandler(this.DpofPanel_Load);
            this.VisibleChanged += new System.EventHandler(this.DpofPanel_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this._channelsView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _clearOrderInfoTemplateButton;
        private System.Windows.Forms.TextBox _orderInfoTemplateBox;
        private System.Windows.Forms.Label _orderInfoTemplateLabel;
        private System.Windows.Forms.Label _orderTemplateLabel;
        private System.Windows.Forms.Label _tagsLabel;
        private System.Windows.Forms.Button _insertTagButton;
        private System.Windows.Forms.ComboBox _tagsBox;
        private System.Windows.Forms.Button _clearOrderTemplateButton;
        private System.Windows.Forms.TextBox _orderTemplateBox;
        private System.Windows.Forms.DataGridView _channelsView;
        private System.Windows.Forms.DataGridViewTextBoxColumn _paperColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _channelColumn;
    }
}
