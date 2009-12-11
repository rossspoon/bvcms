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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Print
            // 
            this.Print.AutoSize = true;
            this.Print.BackColor = System.Drawing.Color.LightGreen;
            this.Print.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateBlue;
            this.Print.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Print.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Print.ForeColor = System.Drawing.Color.Black;
            this.Print.Location = new System.Drawing.Point(588, 676);
            this.Print.Margin = new System.Windows.Forms.Padding(4);
            this.Print.Name = "Print";
            this.Print.Size = new System.Drawing.Size(432, 66);
            this.Print.TabIndex = 10;
            this.Print.Text = "Print Labels, Return";
            this.Print.UseVisualStyleBackColor = false;
            this.Print.Click += new System.EventHandler(this.GoBack_Click);
            this.Print.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AttendeeKeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 22.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(5, 661);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(386, 46);
            this.label1.TabIndex = 11;
            this.label1.Text = "Touch blue buttons";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 22.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(68, 707);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(420, 46);
            this.label2.TabIndex = 12;
            this.label2.Text = "to record attendance";
            // 
            // Attendees
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Print);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Attendees";
            this.Size = new System.Drawing.Size(1024, 768);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button Print;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

    }
}
