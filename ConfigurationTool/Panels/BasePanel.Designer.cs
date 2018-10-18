namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    partial class BasePanel
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
            this.components = new System.ComponentModel.Container();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this._folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // _toolTip
            // 
            this._toolTip.AutoPopDelay = 5000;
            this._toolTip.InitialDelay = 500;
            this._toolTip.ReshowDelay = 0;
            this._toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this._toolTip.ToolTipTitle = "Warning";
            // 
            // BasePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "BasePanel";
            this.Size = new System.Drawing.Size(482, 335);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip _toolTip;
        protected System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog;
        protected System.Windows.Forms.OpenFileDialog _openFileDialog;
    }
}
