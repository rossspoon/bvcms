namespace CmsCheckin
{
    partial class ListFamily
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
            this.button1 = new System.Windows.Forms.Button();
            this.pgdn = new System.Windows.Forms.Button();
            this.pgup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Print
            // 
            this.Print.BackColor = System.Drawing.Color.LightGreen;
            this.Print.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateBlue;
            this.Print.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Print.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Print.ForeColor = System.Drawing.Color.Black;
            this.Print.Location = new System.Drawing.Point(588, 696);
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
            this.label1.Location = new System.Drawing.Point(90, 658);
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
            this.label2.Location = new System.Drawing.Point(90, 707);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(420, 46);
            this.label2.TabIndex = 12;
            this.label2.Text = "to record attendance";
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.button1.Location = new System.Drawing.Point(3, 674);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 88);
            this.button1.TabIndex = 13;
            this.button1.Text = ".";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pgdn
            // 
            this.pgdn.AutoSize = true;
            this.pgdn.BackColor = System.Drawing.Color.LightGray;
            this.pgdn.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateBlue;
            this.pgdn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pgdn.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pgdn.ForeColor = System.Drawing.Color.Black;
            this.pgdn.Location = new System.Drawing.Point(900, 618);
            this.pgdn.Margin = new System.Windows.Forms.Padding(4);
            this.pgdn.Name = "pgdn";
            this.pgdn.Size = new System.Drawing.Size(120, 53);
            this.pgdn.TabIndex = 15;
            this.pgdn.Text = "PgDn";
            this.pgdn.UseVisualStyleBackColor = false;
            this.pgdn.Click += new System.EventHandler(this.pgdn_Click);
            // 
            // pgup
            // 
            this.pgup.AutoSize = true;
            this.pgup.BackColor = System.Drawing.Color.LightGray;
            this.pgup.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateBlue;
            this.pgup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pgup.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pgup.ForeColor = System.Drawing.Color.Black;
            this.pgup.Location = new System.Drawing.Point(906, 5);
            this.pgup.Margin = new System.Windows.Forms.Padding(4);
            this.pgup.Name = "pgup";
            this.pgup.Size = new System.Drawing.Size(118, 53);
            this.pgup.TabIndex = 14;
            this.pgup.Text = "PgUp";
            this.pgup.UseVisualStyleBackColor = false;
            this.pgup.Click += new System.EventHandler(this.pgup_Click);
            // 
            // ListFamily
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.pgdn);
            this.Controls.Add(this.pgup);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Print);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ListFamily";
            this.Size = new System.Drawing.Size(1024, 768);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button Print;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button pgdn;
        public System.Windows.Forms.Button pgup;

    }
}
