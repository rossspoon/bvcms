namespace CmsCheckin
{
    partial class NameResults
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
            this.GoBackButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GoBackButton
            // 
            this.GoBackButton.AutoSize = true;
            this.GoBackButton.BackColor = System.Drawing.Color.LightGreen;
            this.GoBackButton.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateBlue;
            this.GoBackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GoBackButton.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GoBackButton.ForeColor = System.Drawing.Color.Black;
            this.GoBackButton.Location = new System.Drawing.Point(751, 676);
            this.GoBackButton.Margin = new System.Windows.Forms.Padding(4);
            this.GoBackButton.Name = "GoBackButton";
            this.GoBackButton.Size = new System.Drawing.Size(269, 66);
            this.GoBackButton.TabIndex = 10;
            this.GoBackButton.Text = "Go Back";
            this.GoBackButton.UseVisualStyleBackColor = false;
            this.GoBackButton.Click += new System.EventHandler(this.GoBack_Click);
            // 
            // SearchResults
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.GoBackButton);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SearchResults";
            this.Size = new System.Drawing.Size(1024, 768);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button GoBackButton;

    }
}
