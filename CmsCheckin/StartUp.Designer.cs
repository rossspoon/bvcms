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
            this.LeadHours = new System.Windows.Forms.ComboBox();
            this.Printer = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.LateMinutes = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.AskEmFriend = new System.Windows.Forms.CheckBox();
            this.AskGrade = new System.Windows.Forms.CheckBox();
            this.AskChurch = new System.Windows.Forms.CheckBox();
            this.EnableTimer = new System.Windows.Forms.CheckBox();
            this.AskChurchName = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.KioskName = new System.Windows.Forms.TextBox();
            this.AskLabels = new System.Windows.Forms.CheckBox();
            this.KioskMode = new System.Windows.Forms.CheckBox();
            this.TwoInchLabel = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbCampusId
            // 
            this.cbCampusId.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCampusId.FormattingEnabled = true;
            this.cbCampusId.Location = new System.Drawing.Point(229, 22);
            this.cbCampusId.Name = "cbCampusId";
            this.cbCampusId.Size = new System.Drawing.Size(538, 34);
            this.cbCampusId.TabIndex = 1;
            // 
            // cbDayOfWeek
            // 
            this.cbDayOfWeek.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDayOfWeek.FormattingEnabled = true;
            this.cbDayOfWeek.Location = new System.Drawing.Point(229, 77);
            this.cbDayOfWeek.Name = "cbDayOfWeek";
            this.cbDayOfWeek.Size = new System.Drawing.Size(538, 34);
            this.cbDayOfWeek.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(663, 434);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 64);
            this.button1.TabIndex = 22;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(115, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Campus Id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(98, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Day Of Week";
            // 
            // HideCursor
            // 
            this.HideCursor.AutoSize = true;
            this.HideCursor.Checked = true;
            this.HideCursor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HideCursor.Location = new System.Drawing.Point(532, 478);
            this.HideCursor.Name = "HideCursor";
            this.HideCursor.Size = new System.Drawing.Size(81, 17);
            this.HideCursor.TabIndex = 21;
            this.HideCursor.Text = "Hide Cursor";
            this.HideCursor.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(43, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Early Check In Hours";
            // 
            // LeadHours
            // 
            this.LeadHours.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeadHours.FormattingEnabled = true;
            this.LeadHours.Items.AddRange(new object[] {
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
            this.LeadHours.Location = new System.Drawing.Point(229, 131);
            this.LeadHours.Name = "LeadHours";
            this.LeadHours.Size = new System.Drawing.Size(538, 34);
            this.LeadHours.TabIndex = 5;
            this.LeadHours.Text = "5";
            // 
            // Printer
            // 
            this.Printer.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Printer.FormattingEnabled = true;
            this.Printer.Location = new System.Drawing.Point(229, 239);
            this.Printer.Name = "Printer";
            this.Printer.Size = new System.Drawing.Size(538, 34);
            this.Printer.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(146, 247);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Printer";
            // 
            // LateMinutes
            // 
            this.LateMinutes.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LateMinutes.FormattingEnabled = true;
            this.LateMinutes.Items.AddRange(new object[] {
            "30",
            "60",
            "90",
            "120",
            "180",
            "240",
            "1440"});
            this.LateMinutes.Location = new System.Drawing.Point(229, 185);
            this.LateMinutes.Name = "LateMinutes";
            this.LateMinutes.Size = new System.Drawing.Size(538, 34);
            this.LateMinutes.TabIndex = 7;
            this.LateMinutes.Text = "1440";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(33, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Late Check In Minutes";
            // 
            // AskEmFriend
            // 
            this.AskEmFriend.AutoSize = true;
            this.AskEmFriend.Checked = true;
            this.AskEmFriend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AskEmFriend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AskEmFriend.Location = new System.Drawing.Point(23, 360);
            this.AskEmFriend.Name = "AskEmFriend";
            this.AskEmFriend.Size = new System.Drawing.Size(188, 24);
            this.AskEmFriend.TabIndex = 18;
            this.AskEmFriend.Text = "Ask Emergency Friend";
            this.AskEmFriend.UseVisualStyleBackColor = true;
            // 
            // AskGrade
            // 
            this.AskGrade.AutoSize = true;
            this.AskGrade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AskGrade.Location = new System.Drawing.Point(23, 450);
            this.AskGrade.Name = "AskGrade";
            this.AskGrade.Size = new System.Drawing.Size(104, 24);
            this.AskGrade.TabIndex = 19;
            this.AskGrade.Text = "Ask Grade";
            this.AskGrade.UseVisualStyleBackColor = true;
            // 
            // AskChurch
            // 
            this.AskChurch.AutoSize = true;
            this.AskChurch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AskChurch.Location = new System.Drawing.Point(23, 390);
            this.AskChurch.Name = "AskChurch";
            this.AskChurch.Size = new System.Drawing.Size(110, 24);
            this.AskChurch.TabIndex = 20;
            this.AskChurch.Text = "Ask Church";
            this.AskChurch.UseVisualStyleBackColor = true;
            // 
            // EnableTimer
            // 
            this.EnableTimer.AutoSize = true;
            this.EnableTimer.Checked = true;
            this.EnableTimer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnableTimer.Location = new System.Drawing.Point(271, 420);
            this.EnableTimer.Name = "EnableTimer";
            this.EnableTimer.Size = new System.Drawing.Size(121, 24);
            this.EnableTimer.TabIndex = 16;
            this.EnableTimer.Text = "Enable Timer";
            this.EnableTimer.UseVisualStyleBackColor = true;
            // 
            // AskChurchName
            // 
            this.AskChurchName.AutoSize = true;
            this.AskChurchName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AskChurchName.Location = new System.Drawing.Point(23, 420);
            this.AskChurchName.Name = "AskChurchName";
            this.AskChurchName.Size = new System.Drawing.Size(156, 24);
            this.AskChurchName.TabIndex = 20;
            this.AskChurchName.Text = "Ask Church Name";
            this.AskChurchName.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(108, 306);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 20);
            this.label6.TabIndex = 23;
            this.label6.Text = "Kiosk Name";
            // 
            // KioskName
            // 
            this.KioskName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KioskName.Location = new System.Drawing.Point(229, 299);
            this.KioskName.Name = "KioskName";
            this.KioskName.Size = new System.Drawing.Size(538, 31);
            this.KioskName.TabIndex = 24;
            // 
            // AskLabels
            // 
            this.AskLabels.AutoSize = true;
            this.AskLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AskLabels.Location = new System.Drawing.Point(271, 360);
            this.AskLabels.Name = "AskLabels";
            this.AskLabels.Size = new System.Drawing.Size(214, 24);
            this.AskLabels.TabIndex = 25;
            this.AskLabels.Text = "Ask whether labels printed";
            this.AskLabels.UseVisualStyleBackColor = true;
            // 
            // KioskMode
            // 
            this.KioskMode.AutoSize = true;
            this.KioskMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KioskMode.Location = new System.Drawing.Point(271, 450);
            this.KioskMode.Name = "KioskMode";
            this.KioskMode.Size = new System.Drawing.Size(156, 24);
            this.KioskMode.TabIndex = 26;
            this.KioskMode.Text = "Kiosk Registration";
            this.KioskMode.UseVisualStyleBackColor = true;
            // 
            // TwoInchLabel
            // 
            this.TwoInchLabel.AutoSize = true;
            this.TwoInchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TwoInchLabel.Location = new System.Drawing.Point(271, 390);
            this.TwoInchLabel.Name = "TwoInchLabel";
            this.TwoInchLabel.Size = new System.Drawing.Size(174, 24);
            this.TwoInchLabel.TabIndex = 27;
            this.TwoInchLabel.Text = "Two Inch Label Style";
            this.TwoInchLabel.UseVisualStyleBackColor = true;
            // 
            // StartUp
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(796, 519);
            this.Controls.Add(this.TwoInchLabel);
            this.Controls.Add(this.KioskMode);
            this.Controls.Add(this.AskLabels);
            this.Controls.Add(this.KioskName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.EnableTimer);
            this.Controls.Add(this.AskChurchName);
            this.Controls.Add(this.AskChurch);
            this.Controls.Add(this.AskGrade);
            this.Controls.Add(this.AskEmFriend);
            this.Controls.Add(this.LateMinutes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Printer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LeadHours);
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
            this.Load += new System.EventHandler(this.StartUp_Load);
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
        public System.Windows.Forms.ComboBox LeadHours;
        public System.Windows.Forms.ComboBox Printer;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox LateMinutes;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.CheckBox AskEmFriend;
        public System.Windows.Forms.CheckBox AskGrade;
        public System.Windows.Forms.CheckBox AskChurch;
        public System.Windows.Forms.CheckBox EnableTimer;
        public System.Windows.Forms.CheckBox AskChurchName;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox KioskName;
        public System.Windows.Forms.CheckBox AskLabels;
        public System.Windows.Forms.CheckBox KioskMode;
        public System.Windows.Forms.CheckBox TwoInchLabel;
    }
}