namespace TakeUploadPicture
{
    partial class StandAloneForm
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
            this.imageResizer1 = new TakeUploadPicture.ImageResizer();
            this.SuspendLayout();
            // 
            // imageResizer1
            // 
            this.imageResizer1.Location = new System.Drawing.Point(0, 0);
            this.imageResizer1.Margin = new System.Windows.Forms.Padding(4);
            this.imageResizer1.MinimumSize = new System.Drawing.Size(733, 369);
            this.imageResizer1.Name = "imageResizer1";
            this.imageResizer1.OffsetX = 0;
            this.imageResizer1.OffsetY = 0;
            this.imageResizer1.RequiredHeight = 0;
            this.imageResizer1.RequiredWidth = 0;
            this.imageResizer1.Size = new System.Drawing.Size(800, 482);
            this.imageResizer1.TabIndex = 0;
            // 
            // StandAloneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 484);
            this.Controls.Add(this.imageResizer1);
            this.Name = "StandAloneForm";
            this.Text = "Take Upload Picture";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StandAloneForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageResizer imageResizer1;
    }
}