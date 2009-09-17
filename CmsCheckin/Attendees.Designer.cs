namespace CmsCheckin
{
    partial class Attendees
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
            this.Print = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Print
            // 
            this.Print.AutoSize = true;
            this.Print.BackColor = System.Drawing.Color.LightGreen;
            this.Print.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateBlue;
            this.Print.FlatAppearance.BorderSize = 2;
            this.Print.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Print.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Print.ForeColor = System.Drawing.Color.Black;
            this.Print.Location = new System.Drawing.Point(584, 699);
            this.Print.Name = "Print";
            this.Print.Size = new System.Drawing.Size(437, 54);
            this.Print.TabIndex = 10;
            this.Print.Text = "Return to Phone Number";
            this.Print.UseVisualStyleBackColor = false;
            this.Print.Click += new System.EventHandler(this.GoBack_Click);
            // 
            // Attendees
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Print);
            this.Name = "Attendees";
            this.Size = new System.Drawing.Size(1024, 768);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Print;
    }
}
