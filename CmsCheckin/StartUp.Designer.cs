namespace CmsCheckin
{
    partial class StartUp
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
            this.cbCampusId = new System.Windows.Forms.ComboBox();
            this.cbDayOfWeek = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.HideCursor = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LeadTime = new System.Windows.Forms.ComboBox();
            this.Printer = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.EarlyCheckin = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.AskEmFriend = new System.Windows.Forms.CheckBox();
            this.JoinOrgNotAttend = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbCampusId
            // 
            this.cbCampusId.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCampusId.FormattingEnabled = true;
            this.cbCampusId.Location = new System.Drawing.Point(153, 22);
            this.cbCampusId.Name = "cbCampusId";
            this.cbCampusId.Size = new System.Drawing.Size(260, 39);
            this.cbCampusId.TabIndex = 0;
            // 
            // cbDayOfWeek
            // 
            this.cbDayOfWeek.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDayOfWeek.FormattingEnabled = true;
            this.cbDayOfWeek.Location = new System.Drawing.Point(153, 77);
            this.cbDayOfWeek.Name = "cbDayOfWeek";
            this.cbDayOfWeek.Size = new System.Drawing.Size(260, 39);
            this.cbDayOfWeek.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(286, 420);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 64);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Campus Id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Day Of Week";
            // 
            // HideCursor
            // 
            this.HideCursor.AutoSize = true;
            this.HideCursor.Checked = true;
            this.HideCursor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HideCursor.Location = new System.Drawing.Point(17, 443);
            this.HideCursor.Name = "HideCursor";
            this.HideCursor.Size = new System.Drawing.Size(105, 21);
            this.HideCursor.TabIndex = 5;
            this.HideCursor.Text = "Hide Cursor";
            this.HideCursor.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Early Check In Hours";
            // 
            // LeadTime
            // 
            this.LeadTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeadTime.FormattingEnabled = true;
            this.LeadTime.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "24",
            "36"});
            this.LeadTime.Location = new System.Drawing.Point(153, 131);
            this.LeadTime.Name = "LeadTime";
            this.LeadTime.Size = new System.Drawing.Size(260, 39);
            this.LeadTime.TabIndex = 7;
            this.LeadTime.Text = "5";
            // 
            // Printer
            // 
            this.Printer.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Printer.FormattingEnabled = true;
            this.Printer.Location = new System.Drawing.Point(153, 239);
            this.Printer.Name = "Printer";
            this.Printer.Size = new System.Drawing.Size(260, 39);
            this.Printer.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(97, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Printer";
            // 
            // EarlyCheckin
            // 
            this.EarlyCheckin.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EarlyCheckin.FormattingEnabled = true;
            this.EarlyCheckin.Items.AddRange(new object[] {
            "30",
            "60",
            "90",
            "120",
            "180",
            "240",
            "1440"});
            this.EarlyCheckin.Location = new System.Drawing.Point(153, 185);
            this.EarlyCheckin.Name = "EarlyCheckin";
            this.EarlyCheckin.Size = new System.Drawing.Size(260, 39);
            this.EarlyCheckin.TabIndex = 11;
            this.EarlyCheckin.Text = "1440";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 201);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(147, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Late Check In Minutes";
            // 
            // AskEmFriend
            // 
            this.AskEmFriend.AutoSize = true;
            this.AskEmFriend.Location = new System.Drawing.Point(153, 310);
            this.AskEmFriend.Name = "AskEmFriend";
            this.AskEmFriend.Size = new System.Drawing.Size(224, 21);
            this.AskEmFriend.TabIndex = 12;
            this.AskEmFriend.Text = "Ask For Emergency Friend Info";
            this.AskEmFriend.UseVisualStyleBackColor = true;
            // 
            // JoinOrgNotAttend
            // 
            this.JoinOrgNotAttend.AutoSize = true;
            this.JoinOrgNotAttend.Location = new System.Drawing.Point(153, 356);
            this.JoinOrgNotAttend.Name = "JoinOrgNotAttend";
            this.JoinOrgNotAttend.Size = new System.Drawing.Size(252, 21);
            this.JoinOrgNotAttend.TabIndex = 13;
            this.JoinOrgNotAttend.Text = "Join Organization Instead of Attend";
            this.JoinOrgNotAttend.UseVisualStyleBackColor = true;
            // 
            // StartUp
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(462, 496);
            this.Controls.Add(this.JoinOrgNotAttend);
            this.Controls.Add(this.AskEmFriend);
            this.Controls.Add(this.EarlyCheckin);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Printer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LeadTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.HideCursor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbDayOfWeek);
            this.Controls.Add(this.cbCampusId);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartUp";
            this.Text = "StartUp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbCampusId;
        private System.Windows.Forms.ComboBox cbDayOfWeek;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.CheckBox HideCursor;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox LeadTime;
        public System.Windows.Forms.ComboBox Printer;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox EarlyCheckin;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.CheckBox AskEmFriend;
        public System.Windows.Forms.CheckBox JoinOrgNotAttend;
    }
}