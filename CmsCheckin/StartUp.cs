using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;
using System.Drawing.Printing;
using System.Xml.Serialization;
using System.IO;

namespace CmsCheckin
{
    public partial class StartUp : Form
    {
        public int CampusId
        {
            get
            {
                var c = cbCampusId.SelectedItem as Campus;
                if (c != null)
                    return c.Id;
                return 0;
            }
        }
        public int DayOfWeek
        {
            get
            {
                var d = cbDayOfWeek.SelectedItem as DayOfWeek;
                return d.Day;
            }
        }
        public StartUp()
        {
            InitializeComponent();
            cbDayOfWeek.Items.Add(new DayOfWeek { Day = 0, Text = "Sunday" });
            cbDayOfWeek.Items.Add(new DayOfWeek { Day = 1, Text = "Monday" });
            cbDayOfWeek.Items.Add(new DayOfWeek { Day = 2, Text = "Tuesday" });
            cbDayOfWeek.Items.Add(new DayOfWeek { Day = 3, Text = "Wednesday" });
            cbDayOfWeek.Items.Add(new DayOfWeek { Day = 4, Text = "Thursday" });
            cbDayOfWeek.Items.Add(new DayOfWeek { Day = 5, Text = "Friday" });
            cbDayOfWeek.Items.Add(new DayOfWeek { Day = 6, Text = "Saturday" });
            cbDayOfWeek.SelectedIndex = (int)DateTime.Now.DayOfWeek;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbCampusId.Items.Count > 0)
                Settings1.Default.Campus = ((Campus)(cbCampusId.SelectedItem)).Name;
            Settings1.Default.Printer = Printer.Text;
            Settings1.Default.AskChurch = AskChurch.Checked;
            Settings1.Default.AskEmFriend = AskEmFriend.Checked;
            Settings1.Default.AskGrade = AskGrade.Checked;
            Settings1.Default.KioskMode = KioskMode.Checked;
            Settings1.Default.LateMinutes = LateMinutes.Text.ToInt();
            Settings1.Default.LeadHours = LeadHours.Text.ToInt();
            Settings1.Default.LateMinutes = LateMinutes.Text.ToInt();
            Settings1.Default.Save();
            this.Hide();
        }

        TextBox current = null;
        private void StartUp_Load(object sender, EventArgs e)
        {
#if DEBUG
            cbDayOfWeek.SelectedIndex = 0;
            HideCursor.Checked = false;
#endif
            var prtdoc = new PrintDocument();
            var defp = prtdoc.PrinterSettings.PrinterName;
            foreach (var s in PrinterSettings.InstalledPrinters)
                Printer.Items.Add(s);
            Printer.SelectedIndex = Printer.FindStringExact(defp);
            if (Settings1.Default.Printer.HasValue())
                Printer.SelectedIndex = Printer.FindStringExact(Settings1.Default.Printer);

            var wc = new WebClient();
            var url = new Uri(new Uri(Util.ServiceUrl()), "Checkin/Campuses");
            var str = wc.DownloadString(url);
            var x = XDocument.Parse(str);
            foreach (var i in x.Descendants("campus"))
                cbCampusId.Items.Add(new Campus
                {
                    Id = int.Parse(i.Attribute("id").Value),
                    Name = i.Attribute("name").Value
                });
            if (cbCampusId.Items.Count > 0)
                cbCampusId.SelectedIndex = 0;
            var ii = cbCampusId.FindStringExact(Settings1.Default.Campus);
            if (ii >= 0)
                cbCampusId.SelectedIndex = ii;
            ii = LeadHours.FindStringExact(Settings1.Default.LeadHours.ToString());
            if (ii >= 0)
                LeadHours.SelectedIndex = ii;
            ii = LateMinutes.FindStringExact(Settings1.Default.LateMinutes.ToString());
            if (ii >= 0)
                LateMinutes.SelectedIndex = ii;
            AskEmFriend.Checked = Settings1.Default.AskEmFriend;
            KioskMode.Checked = Settings1.Default.KioskMode;
            AskGrade.Checked = Settings1.Default.AskGrade;
            AskChurch.Checked = Settings1.Default.AskChurch;

        }
    }
    class DayOfWeek
    {
        public string Text { get; set; }
        public int Day { get; set; }
        public override string ToString()
        {
            return Text;
        }
    }
    class Campus
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
