namespace CmsCheckin
{
    partial class PrintingServer
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
            this.label1 = new System.Windows.Forms.Label();
            this.CheckNow = new System.Windows.Forms.Button();
            this.Countdown = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Countdown for next Check";
            // 
            // CheckNow
            // 
            this.CheckNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckNow.Location = new System.Drawing.Point(292, 66);
            this.CheckNow.Name = "CheckNow";
            this.CheckNow.Size = new System.Drawing.Size(185, 62);
            this.CheckNow.TabIndex = 1;
            this.CheckNow.Text = "Check Now";
            this.CheckNow.UseVisualStyleBackColor = true;
            this.CheckNow.Click += new System.EventHandler(this.CheckNow_Click);
            // 
            // Countdown
            // 
            this.Countdown.AutoSize = true;
            this.Countdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Countdown.Location = new System.Drawing.Point(106, 74);
            this.Countdown.Name = "Countdown";
            this.Countdown.Size = new System.Drawing.Size(0, 24);
            this.Countdown.TabIndex = 2;
            // 
            // PrintingServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 188);
            this.Controls.Add(this.Countdown);
            this.Controls.Add(this.CheckNow);
            this.Controls.Add(this.label1);
            this.Name = "PrintingServer";
            this.Text = "PrintingServer";
            this.Load += new System.EventHandler(this.PrintingServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CheckNow;
        private System.Windows.Forms.Label Countdown;
    }
}