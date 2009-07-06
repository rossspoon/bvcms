namespace CmsCheckin
{
    partial class Match
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
            this.ReturnPhone = new System.Windows.Forms.Button();
            this.matchBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.matchBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.matchBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // ReturnPhone
            // 
            this.ReturnPhone.AutoSize = true;
            this.ReturnPhone.BackColor = System.Drawing.Color.DodgerBlue;
            this.ReturnPhone.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateBlue;
            this.ReturnPhone.FlatAppearance.BorderSize = 2;
            this.ReturnPhone.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ReturnPhone.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReturnPhone.ForeColor = System.Drawing.Color.White;
            this.ReturnPhone.Location = new System.Drawing.Point(3, 3);
            this.ReturnPhone.Name = "ReturnPhone";
            this.ReturnPhone.Size = new System.Drawing.Size(134, 43);
            this.ReturnPhone.TabIndex = 0;
            this.ReturnPhone.Text = "Go Back";
            this.ReturnPhone.UseVisualStyleBackColor = false;
            this.ReturnPhone.Click += new System.EventHandler(this.ReturnPhone_Click);
            // 
            // matchBindingSource
            // 
            this.matchBindingSource.DataSource = typeof(CmsCheckin.Match);
            // 
            // matchBindingSource1
            // 
            this.matchBindingSource1.DataSource = typeof(CmsCheckin.Match);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(204, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose your family";
            // 
            // Match
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReturnPhone);
            this.Name = "Match";
            this.Size = new System.Drawing.Size(1024, 768);
            ((System.ComponentModel.ISupportInitialize)(this.matchBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ReturnPhone;
        private System.Windows.Forms.BindingSource matchBindingSource;
        private System.Windows.Forms.BindingSource matchBindingSource1;
        private System.Windows.Forms.Label label1;
    }
}
