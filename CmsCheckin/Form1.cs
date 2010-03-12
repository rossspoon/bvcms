using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml.Linq;

namespace CmsCheckin
{
    public partial class Form1 : Form
    {
        PhoneNumber phone;
        Attendees attendees;
        NameSeach namesearch;
        NameResults searchresults;
        ClassResults classresults;
        Families families;

        public Form1()
        {
            InitializeComponent();
        }

        void phone_Go(object sender, EventArgs<string> e)
        {
            if (e.Value == "411")
            {
                phone.Visible = false;
                namesearch = AddControl<NameSeach>(new NameSeach());
                namesearch.GoBack += new EventHandler(name_GoBack);
                namesearch.Go += new EventHandler<EventArgs<string>>(namesearch_Go);
                return;
            }
            if (e.Value.StartsWith("0"))
            {
                MemberList.PrintList(e.Value.Substring(1));
                return;
            }
            phone.Visible = false;

            var x = this.GetDocument("Checkin/Match/" + Util.GetDigits(e.Value) + Program.QueryString);

            if (x.Document.Root.Name == "Families")
            {
                families = AddControl<Families>(new Families());
                families.GoBack += new EventHandler(families_GoBack);
                families.Go += new EventHandler<EventArgs<int>>(families_Go);
                families.ShowFamilies(x);
            }
            else
            {
                attendees = AddControl<Attendees>(new Attendees());
                attendees.GoBack += new EventHandler(attendees_GoBack);
                attendees.GoClasses += new EventHandler<EventArgs<int>>(Go_Classes);
                attendees.FindAttendees(x);
            }
        }

        void namesearch_Go(object sender, EventArgs<string> e)
        {
            this.Controls.Remove(namesearch);
            namesearch = null;

            var x = this.GetDocument("Checkin/NameSearch/" + e.Value);
            searchresults = AddControl<NameResults>(new NameResults());
            searchresults.GoBack += new EventHandler<EventArgs<string>>(SearchResults_GoBack);
            searchresults.ShowResults(x);
        }

        void SearchResults_GoBack(object sender, EventArgs<string> e)
        {
            RemoveControl(searchresults, PhoneNumber.FmtFone(e.Value));
        }
        void ClassResults_GoBack(object sender, EventArgs<string> e)
        {
            RemoveControl(classresults, string.Empty);
        }
        void families_Go(object sender, EventArgs<int> e)
        {
            this.Controls.Remove(families);
            families = null;

            var x = this.GetDocument("Checkin/Family/" + e.Value + Program.QueryString);
            attendees = AddControl<Attendees>(new Attendees());
            attendees.GoBack += new EventHandler(attendees_GoBack);
            attendees.GoClasses += new EventHandler<EventArgs<int>>(Go_Classes);
            attendees.FindAttendees(x);
        }
        void Go_Classes(object sender, EventArgs<int> e)
        {
            this.Controls.Remove(attendees);
            attendees = null;

            var x = this.GetDocument("Checkin/Classes/" 
                + e.Value + Program.QueryString + "&page=1");
            classresults = AddControl<ClassResults>(new ClassResults());
            classresults.GoBack += new EventHandler<EventArgs<int>>(classresults_GoBack);
            classresults.ShowResults(x);
       }

        void classresults_GoBack(object sender, EventArgs<int> e)
        {
            this.Controls.Remove(classresults);
            classresults = null;
            var x = this.GetDocument("Checkin/Family/" + e.Value + Program.QueryString);
            attendees = AddControl<Attendees>(new Attendees());
            attendees.GoBack += new EventHandler(attendees_GoBack);
            attendees.GoClasses += new EventHandler<EventArgs<int>>(Go_Classes);
            attendees.FindAttendees(x);
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            phone.Left = (this.Width / 2) - (phone.Width / 2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            phone = AddControl<PhoneNumber>(new PhoneNumber());
            phone.Go += new EventHandler<EventArgs<string>>(phone_Go);
            this.Resize += new EventHandler(Form1_Resize);
            if (Program.HideCursor)
                Cursor.Hide();
        }

        void attendees_GoBack(object sender, EventArgs e)
        {
            RemoveControl(attendees, string.Empty);
        }
        void name_GoBack(object sender, EventArgs e)
        {
            RemoveControl(namesearch, string.Empty);
        }
        void families_GoBack(object sender, EventArgs e)
        {
            RemoveControl(families, string.Empty);
        }
        private void RemoveControl(UserControl c, string s)
        {
            this.Controls.Remove(c);
            phone.Visible = true;
            phone.textBox1.Text = PhoneNumber.FmtFone(s);
            phone.textBox1.Focus();
            phone.textBox1.Select(phone.textBox1.Text.Length, 0);
        }
        private T AddControl<T>(T c)
        {
            var ctl = c as UserControl;
            ctl.Location = new Point { X = (this.Width / 2) - (ctl.Width / 2), Y = 0 };
            this.Controls.Add(ctl);
            return c;
        }
    }
}
