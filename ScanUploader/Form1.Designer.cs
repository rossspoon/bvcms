namespace ScanUploader
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.UserInfo = new System.Windows.Forms.ToolStripTextBox();
            this.SelectScanner = new System.Windows.Forms.ToolStripButton();
            this.ConfigScanner = new System.Windows.Forms.ToolStripButton();
            this.AcquireImage = new System.Windows.Forms.ToolStripButton();
            this.TypeImage = new System.Windows.Forms.ToolStripDropDownButton();
            this.vBSRegistrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volunteerAppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Search = new System.Windows.Forms.ToolStripButton();
            this.PeopleId = new System.Windows.Forms.ToolStripLabel();
            this.recRegToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UserInfo,
            this.SelectScanner,
            this.ConfigScanner,
            this.AcquireImage,
            this.TypeImage,
            this.Search,
            this.PeopleId});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(685, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // UserInfo
            // 
            this.UserInfo.Name = "UserInfo";
            this.UserInfo.Size = new System.Drawing.Size(100, 25);
            // 
            // SelectScanner
            // 
            this.SelectScanner.Image = ((System.Drawing.Image)(resources.GetObject("SelectScanner.Image")));
            this.SelectScanner.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectScanner.Name = "SelectScanner";
            this.SelectScanner.Size = new System.Drawing.Size(103, 22);
            this.SelectScanner.Text = "Select Scanner";
            this.SelectScanner.Click += new System.EventHandler(this.SelectScanner_Click);
            // 
            // ConfigScanner
            // 
            this.ConfigScanner.Image = ((System.Drawing.Image)(resources.GetObject("ConfigScanner.Image")));
            this.ConfigScanner.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ConfigScanner.Name = "ConfigScanner";
            this.ConfigScanner.Size = new System.Drawing.Size(80, 22);
            this.ConfigScanner.Text = "Configure";
            this.ConfigScanner.Click += new System.EventHandler(this.ConfigScanner_Click);
            // 
            // AcquireImage
            // 
            this.AcquireImage.Image = ((System.Drawing.Image)(resources.GetObject("AcquireImage.Image")));
            this.AcquireImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AcquireImage.Name = "AcquireImage";
            this.AcquireImage.Size = new System.Drawing.Size(68, 22);
            this.AcquireImage.Text = "Acquire";
            this.AcquireImage.Click += new System.EventHandler(this.AcquireImage_Click);
            // 
            // TypeImage
            // 
            this.TypeImage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vBSRegistrationToolStripMenuItem,
            this.recRegToolStripMenuItem,
            this.volunteerAppToolStripMenuItem});
            this.TypeImage.Image = ((System.Drawing.Image)(resources.GetObject("TypeImage.Image")));
            this.TypeImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TypeImage.Name = "TypeImage";
            this.TypeImage.Size = new System.Drawing.Size(139, 22);
            this.TypeImage.Text = "Type: Not Specified";
            // 
            // vBSRegistrationToolStripMenuItem
            // 
            this.vBSRegistrationToolStripMenuItem.Name = "vBSRegistrationToolStripMenuItem";
            this.vBSRegistrationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.vBSRegistrationToolStripMenuItem.Tag = "1";
            this.vBSRegistrationToolStripMenuItem.Text = "VBSReg";
            this.vBSRegistrationToolStripMenuItem.Click += new System.EventHandler(this.vBSRegistrationToolStripMenuItem_Click);
            // 
            // volunteerAppToolStripMenuItem
            // 
            this.volunteerAppToolStripMenuItem.Name = "volunteerAppToolStripMenuItem";
            this.volunteerAppToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.volunteerAppToolStripMenuItem.Tag = "3";
            this.volunteerAppToolStripMenuItem.Text = "VolApp";
            this.volunteerAppToolStripMenuItem.Click += new System.EventHandler(this.volunteerAppToolStripMenuItem_Click);
            // 
            // Search
            // 
            this.Search.Image = ((System.Drawing.Image)(resources.GetObject("Search.Image")));
            this.Search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(62, 22);
            this.Search.Text = "Search";
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // PeopleId
            // 
            this.PeopleId.Name = "PeopleId";
            this.PeopleId.Size = new System.Drawing.Size(53, 22);
            this.PeopleId.Text = "PeopleId";
            // 
            // recRegToolStripMenuItem
            // 
            this.recRegToolStripMenuItem.Name = "recRegToolStripMenuItem";
            this.recRegToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.recRegToolStripMenuItem.Tag = "2";
            this.recRegToolStripMenuItem.Text = "RecReg";
            this.recRegToolStripMenuItem.Click += new System.EventHandler(this.recRegToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(685, 497);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "CMS Scanner";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ConfigScanner;
        private System.Windows.Forms.ToolStripButton AcquireImage;
        private System.Windows.Forms.ToolStripButton SelectScanner;
        private System.Windows.Forms.ToolStripDropDownButton TypeImage;
        private System.Windows.Forms.ToolStripMenuItem vBSRegistrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem volunteerAppToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton Search;
        private System.Windows.Forms.ToolStripLabel PeopleId;
        private System.Windows.Forms.ToolStripTextBox UserInfo;
        private System.Windows.Forms.ToolStripMenuItem recRegToolStripMenuItem;
    }
}

