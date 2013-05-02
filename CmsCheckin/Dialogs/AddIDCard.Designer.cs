namespace CmsCheckin.Dialogs
{
	partial class AddIDCard
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
			this.ScanForLabel = new System.Windows.Forms.Label();
			this.ScanForName = new System.Windows.Forms.Label();
			this.OK = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.ScanID = new CmsCheckin.Controls.EnterTextBox();
			this.SuspendLayout();
			// 
			// ScanForLabel
			// 
			this.ScanForLabel.AutoSize = true;
			this.ScanForLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ScanForLabel.Location = new System.Drawing.Point(113, 11);
			this.ScanForLabel.Name = "ScanForLabel";
			this.ScanForLabel.Size = new System.Drawing.Size(137, 20);
			this.ScanForLabel.TabIndex = 0;
			this.ScanForLabel.Text = "Scan ID Card For:";
			// 
			// ScanForName
			// 
			this.ScanForName.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.ScanForName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ScanForName.Location = new System.Drawing.Point(6, 37);
			this.ScanForName.Name = "ScanForName";
			this.ScanForName.Size = new System.Drawing.Size(350, 20);
			this.ScanForName.TabIndex = 2;
			this.ScanForName.Text = "Your Name Here";
			this.ScanForName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OK
			// 
			this.OK.Location = new System.Drawing.Point(89, 110);
			this.OK.Name = "OK";
			this.OK.Size = new System.Drawing.Size(75, 23);
			this.OK.TabIndex = 4;
			this.OK.Text = "OK";
			this.OK.UseVisualStyleBackColor = true;
			this.OK.Click += new System.EventHandler(this.OK_Click);
			// 
			// Cancel
			// 
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.Location = new System.Drawing.Point(198, 110);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(75, 23);
			this.Cancel.TabIndex = 5;
			this.Cancel.Text = "Cancel";
			this.Cancel.UseVisualStyleBackColor = true;
			// 
			// ScanID
			// 
			this.ScanID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ScanID.Location = new System.Drawing.Point(27, 69);
			this.ScanID.Name = "ScanID";
			this.ScanID.Size = new System.Drawing.Size(309, 26);
			this.ScanID.TabIndex = 1;
			this.ScanID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ScanID.UseSystemPasswordChar = true;
			this.ScanID.WordWrap = false;
			this.ScanID.TextChanged += new System.EventHandler(this.ScanID_TextChanged);
			this.ScanID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScanID_KeyDown);
			this.ScanID.Leave += new System.EventHandler(this.ScanID_Leave);
			// 
			// AddIDCard
			// 
			this.AcceptButton = this.OK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(363, 145);
			this.ControlBox = false;
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.OK);
			this.Controls.Add(this.ScanForName);
			this.Controls.Add(this.ScanID);
			this.Controls.Add(this.ScanForLabel);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(371, 179);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(371, 179);
			this.Name = "AddIDCard";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Add ID Card";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label ScanForLabel;
		private System.Windows.Forms.Label ScanForName;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private Controls.EnterTextBox ScanID;
	}
}