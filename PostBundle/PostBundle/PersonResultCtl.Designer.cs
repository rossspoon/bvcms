namespace PostBundle
{
    partial class PersonResultCtl
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
            this.NameLink = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NameLink
            // 
            this.NameLink.AutoSize = true;
            this.NameLink.Location = new System.Drawing.Point(4, 4);
            this.NameLink.Name = "NameLink";
            this.NameLink.Size = new System.Drawing.Size(55, 13);
            this.NameLink.TabIndex = 0;
            this.NameLink.TabStop = true;
            this.NameLink.Text = "NameLink";
            this.NameLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.NameLink_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // PersonResultCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NameLink);
            this.Name = "PersonResultCtl";
            this.Size = new System.Drawing.Size(150, 81);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel NameLink;
        private System.Windows.Forms.Label label1;
    }
}
