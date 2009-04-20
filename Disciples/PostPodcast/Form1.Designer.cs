namespace PostPodcast
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.FileName = new System.Windows.Forms.TextBox();
            this.Post = new System.Windows.Forms.Button();
            this.Description = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BrowseFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.Author = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Cancel = new System.Windows.Forms.Button();
            this.canPostFor = new System.Windows.Forms.ComboBox();
            this.Part = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Time = new System.Windows.Forms.ComboBox();
            this.posted = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Part)).BeginInit();
            this.SuspendLayout();
            // 
            // FileName
            // 
            this.FileName.Location = new System.Drawing.Point(71, 43);
            this.FileName.Name = "FileName";
            this.FileName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.FileName.Size = new System.Drawing.Size(325, 20);
            this.FileName.TabIndex = 3;
            // 
            // Post
            // 
            this.Post.Location = new System.Drawing.Point(402, 152);
            this.Post.Name = "Post";
            this.Post.Size = new System.Drawing.Size(58, 23);
            this.Post.TabIndex = 17;
            this.Post.Text = "P&ost";
            this.Post.UseVisualStyleBackColor = true;
            this.Post.Click += new System.EventHandler(this.Post_Click);
            // 
            // Description
            // 
            this.Description.Location = new System.Drawing.Point(71, 152);
            this.Description.Multiline = true;
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(325, 90);
            this.Description.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "D&escription";
            // 
            // BrowseFile
            // 
            this.BrowseFile.Location = new System.Drawing.Point(402, 41);
            this.BrowseFile.Name = "BrowseFile";
            this.BrowseFile.Size = new System.Drawing.Size(58, 23);
            this.BrowseFile.TabIndex = 4;
            this.BrowseFile.Text = "Browse";
            this.BrowseFile.UseVisualStyleBackColor = true;
            this.BrowseFile.Click += new System.EventHandler(this.BrowseFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "&Mp3 File";
            // 
            // Title
            // 
            this.Title.Location = new System.Drawing.Point(71, 95);
            this.Title.Name = "Title";
            this.Title.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Title.Size = new System.Drawing.Size(325, 20);
            this.Title.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "&Title";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(71, 122);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(104, 20);
            this.dateTimePicker1.TabIndex = 10;
            this.dateTimePicker1.Value = new System.DateTime(2007, 6, 3, 0, 0, 0, 0);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "&Date";
            // 
            // Author
            // 
            this.Author.Location = new System.Drawing.Point(71, 69);
            this.Author.Name = "Author";
            this.Author.ReadOnly = true;
            this.Author.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Author.Size = new System.Drawing.Size(325, 20);
            this.Author.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "&Author";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "mp3 Files|*.mp3|All files|*.*";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 262);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(388, 23);
            this.progressBar1.TabIndex = 20;
            // 
            // Cancel
            // 
            this.Cancel.Enabled = false;
            this.Cancel.Location = new System.Drawing.Point(402, 262);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(58, 23);
            this.Cancel.TabIndex = 19;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // canPostFor
            // 
            this.canPostFor.FormattingEnabled = true;
            this.canPostFor.Location = new System.Drawing.Point(71, 12);
            this.canPostFor.Name = "canPostFor";
            this.canPostFor.Size = new System.Drawing.Size(325, 21);
            this.canPostFor.TabIndex = 1;
            // 
            // Part
            // 
            this.Part.Location = new System.Drawing.Point(361, 121);
            this.Part.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.Part.Name = "Part";
            this.Part.Size = new System.Drawing.Size(35, 20);
            this.Part.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "&Speaker";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(329, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "&Part";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(185, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "T&ime";
            // 
            // Time
            // 
            this.Time.FormattingEnabled = true;
            this.Time.Location = new System.Drawing.Point(221, 122);
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(78, 21);
            this.Time.TabIndex = 12;
            this.Time.Text = "8:00 am";
            // 
            // posted
            // 
            this.posted.AutoSize = true;
            this.posted.ForeColor = System.Drawing.Color.Green;
            this.posted.Location = new System.Drawing.Point(411, 182);
            this.posted.Name = "posted";
            this.posted.Size = new System.Drawing.Size(42, 13);
            this.posted.TabIndex = 18;
            this.posted.Text = "posted!";
            this.posted.Visible = false;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "help";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightBlue;
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(402, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 18);
            this.label2.TabIndex = 21;
            this.label2.Text = "?";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label2, "Zero here means that the podcast \r\nis not divided up into parts \r\nfor this speake" +
                    "r, date and time.");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 295);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.posted);
            this.Controls.Add(this.Time);
            this.Controls.Add(this.Part);
            this.Controls.Add(this.canPostFor);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.Author);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Description);
            this.Controls.Add(this.BrowseFile);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Post);
            this.Controls.Add(this.FileName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Post Podcast to Bellevue Teachers Web Site";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Part)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.Button Post;
        private System.Windows.Forms.TextBox Description;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BrowseFile;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox Title;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox Author;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.ComboBox canPostFor;
        private System.Windows.Forms.NumericUpDown Part;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox Time;
        private System.Windows.Forms.Label posted;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
    }
}

