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
            this.imageResizer2 = new TakeUploadPicture.ImageResizer();
            this.SuspendLayout();
            // 
            // imageResizer2
            // 
            this.imageResizer2.Location = new System.Drawing.Point(0, 0);
            this.imageResizer2.Name = "imageResizer2";
            this.imageResizer2.OffsetX = 0;
            this.imageResizer2.OffsetY = 0;
            this.imageResizer2.RequiredHeight = 0;
            this.imageResizer2.RequiredWidth = 0;
            this.imageResizer2.Size = new System.Drawing.Size(773, 485);
            this.imageResizer2.TabIndex = 0;
            // 
            // StandAloneForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(769, 486);
            this.Controls.Add(this.imageResizer2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "StandAloneForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Take Upload Picture";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StandAloneForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageResizer imageResizer2;


    }
}