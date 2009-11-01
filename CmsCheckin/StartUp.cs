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

namespace CmsCheckin
{
    public partial class StartUp : Form
    {
        public int CampusId
        {
            get
            {
                var c = cbCampusId.SelectedItem as Campus;
                return c.Id;
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

            var wc = new WebClient();
            var url = new Uri(new Uri(Form1.ServiceUrl()), "Checkin/Campuses");
            var str = wc.DownloadString(url);
            var x = XDocument.Parse(str);
            foreach (var e in x.Descendants("campus"))
                cbCampusId.Items.Add(new Campus 
                { 
                    Id = int.Parse(e.Attribute("id").Value),
                    Name = e.Attribute("name").Value
                });
            cbCampusId.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
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
