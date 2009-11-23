using System;
using System.Runtime.InteropServices;
namespace TakeUploadPicture
{
    partial class StandAloneTestForm
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
            if (cam != null)
            {
                cam.Dispose();
            }
            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandAloneTestForm));
            this.imageResizer1 = new TakeUploadPicture.ImageResizer();
            this.SuspendLayout();
            // 
            // imageResizer1
            // 
            this.imageResizer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageResizer1.Location = new System.Drawing.Point(0, 0);
            this.imageResizer1.Margin = new System.Windows.Forms.Padding(4);
            this.imageResizer1.MinimumSize = new System.Drawing.Size(733, 369);
            this.imageResizer1.Name = "imageResizer1";
            this.imageResizer1.OffsetX = 0;
            this.imageResizer1.OffsetY = 0;
            this.imageResizer1.RequiredHeight = 0;
            this.imageResizer1.RequiredWidth = 0;
            this.imageResizer1.Size = new System.Drawing.Size(795, 480);
            this.imageResizer1.TabIndex = 0;
            this.imageResizer1.Resize += new System.EventHandler(this.imageResizer1_Resize);
            // 
            // StandAloneTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 480);
            this.Controls.Add(this.imageResizer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "StandAloneTestForm";
            this.Text = "Take Upload Picture";
            this.ResumeLayout(false);

        }
        private ImageResizer imageResizer1;
        #endregion
    }
}