namespace CmsCheckin
{
	partial class Attendant
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
			this.ShowCheckin = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// ShowCheckin
			// 
			this.ShowCheckin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ShowCheckin.Location = new System.Drawing.Point(12, 12);
			this.ShowCheckin.Name = "ShowCheckin";
			this.ShowCheckin.Size = new System.Drawing.Size(100, 56);
			this.ShowCheckin.TabIndex = 0;
			this.ShowCheckin.Text = "Show Checkin";
			this.ShowCheckin.UseVisualStyleBackColor = true;
			this.ShowCheckin.Click += new System.EventHandler(this.ShowCheckin_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(307, 142);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// Attendant
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(566, 381);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ShowCheckin);
			this.Name = "Attendant";
			this.Text = "Attendant";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ShowCheckin;
		public System.Windows.Forms.Label label1;
	}
}