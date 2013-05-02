namespace PostBundle
{
    [System.ComponentModel.ToolboxItemAttribute(false)]
    partial class UploadPane
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
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
            this.Upload = new System.Windows.Forms.Button();
            this.CreateTotalsByFund = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Upload
            // 
            this.Upload.Location = new System.Drawing.Point(16, 14);
            this.Upload.Name = "Upload";
            this.Upload.Size = new System.Drawing.Size(110, 23);
            this.Upload.TabIndex = 0;
            this.Upload.Text = "Upload";
            this.Upload.UseVisualStyleBackColor = true;
            this.Upload.Click += new System.EventHandler(this.Upload_Click);
            // 
            // CreateTotalsByFund
            // 
            this.CreateTotalsByFund.Location = new System.Drawing.Point(16, 64);
            this.CreateTotalsByFund.Name = "CreateTotalsByFund";
            this.CreateTotalsByFund.Size = new System.Drawing.Size(110, 23);
            this.CreateTotalsByFund.TabIndex = 1;
            this.CreateTotalsByFund.Text = "Totals By Fund";
            this.CreateTotalsByFund.UseVisualStyleBackColor = true;
            this.CreateTotalsByFund.Click += new System.EventHandler(this.CreateTotalsByFund_Click);
            // 
            // UploadPane
            // 
            this.AutoSize = true;
            this.Controls.Add(this.CreateTotalsByFund);
            this.Controls.Add(this.Upload);
            this.Name = "UploadPane";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Upload;
        private System.Windows.Forms.Button CreateTotalsByFund;
    }
}
