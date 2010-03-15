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
    public partial class BaseForm : Form
    {

        public BaseForm()
        {
            InitializeComponent();
        }

        private void BaseForm_Load(object sender, EventArgs e)
        {
            if (Program.HideCursor)
                Cursor.Hide();

            Program.home = new Home();
            ControlsAdd(Program.home);
            Program.home.Visible = true;

            Program.namesearch = new EnterText("Name Search", true);
            Program.namesearch.GoBack += new EventHandler(namesearch_GoBack);
            Program.namesearch.GoNext += new EventHandler(namesearch_GoNext);
            ControlsAdd(Program.namesearch);

            Program.families = new ListFamilies();
            ControlsAdd(Program.families);

            Program.names = new ListNames();
            ControlsAdd(Program.names);

            Program.family = new ListFamily();
            ControlsAdd(Program.family);

            Program.classes = new ListClasses();
            ControlsAdd(Program.classes);

            Program.first = new EnterText("First Name", true);
            Program.first.GoBack += new EventHandler(first_GoBack);
            Program.first.GoNext += new EventHandler(first_GoNext);
            ControlsAdd(Program.first);

            Program.goesby = new EnterText("Goes By", true);
            Program.goesby.GoBack += new EventHandler(goesby_GoBack);
            Program.goesby.GoNext += new EventHandler(goesby_GoNext);
            ControlsAdd(Program.goesby);

            Program.last = new EnterText("Last Name", true);
            Program.last.GoBack += new EventHandler(last_GoBack);
            Program.last.GoNext += new EventHandler(last_GoNext);
            ControlsAdd(Program.last);

            Program.email = new EnterText("Email");
            Program.email.GoBack += new EventHandler(email_GoBack);
            Program.email.GoNext += new EventHandler(email_GoNext);
            ControlsAdd(Program.email);

            Program.addr = new EnterText("Address", true);
            Program.addr.GoBack += new EventHandler(addr_GoBack);
            Program.addr.GoNext += new EventHandler(addr_GoNext);
            ControlsAdd(Program.addr);

            Program.zip = new EnterText("Zip");
            Program.zip.GoBack += new EventHandler(zip_GoBack);
            Program.zip.GoNext += new EventHandler(zip_GoNext);
            ControlsAdd(Program.zip);

            Program.dob = new EnterDate("Birthday");
            Program.dob.GoBack += new EventHandler(dob_GoBack);
            Program.dob.GoNext += new EventHandler(dob_GoNext);
            ControlsAdd(Program.dob);

            Program.cellphone = new EnterPhone("Cell Phone");
            Program.cellphone.GoBack += new EventHandler(cellphone_GoBack);
            Program.cellphone.GoNext += new EventHandler(cellphone_GoNext);
            ControlsAdd(Program.cellphone);

            Program.homephone = new EnterPhone("Home Phone");
            Program.homephone.GoBack += new EventHandler(homephone_GoBack);
            Program.homephone.GoNext += new EventHandler(homephone_GoNext);
            ControlsAdd(Program.homephone);

            Program.gendermarital = new EnterGenderMarital();
            ControlsAdd(Program.gendermarital);
        }

        void namesearch_GoNext(object sender, EventArgs e)
        {
            Program.namesearch.Swap(Program.names);
            Program.names.ShowResults(Program.namesearch.textBox1.Text, 1);
        }
        void namesearch_GoBack(object sender, EventArgs e)
        {
            Program.namesearch.GoHome(string.Empty);
        }
        void first_GoNext(object sender, EventArgs e)
        {
            Program.first.Swap(Program.goesby);
        }
        void first_GoBack(object sender, EventArgs e)
        {
            Program.ClearFields();
            Program.first.GoHome(string.Empty);
        }
        void goesby_GoBack(object sender, EventArgs e)
        {
            Program.goesby.Swap(Program.first);
        }
        void goesby_GoNext(object sender, EventArgs e)
        {
            Program.goesby.Swap(Program.last);
        }
        void last_GoBack(object sender, EventArgs e)
        {
            Program.last.Swap(Program.goesby);
        }
        void last_GoNext(object sender, EventArgs e)
        {
            Program.last.Swap(Program.email);
        }
        void email_GoBack(object sender, EventArgs e)
        {
            Program.email.Swap(Program.last);
        }
        void email_GoNext(object sender, EventArgs e)
        {
            Program.email.Swap(Program.addr);
        }
        void addr_GoBack(object sender, EventArgs e)
        {
            Program.addr.Swap(Program.email);
        }
        void addr_GoNext(object sender, EventArgs e)
        {
            Program.addr.Swap(Program.zip);
        }
        void zip_GoBack(object sender, EventArgs e)
        {
            Program.zip.Swap(Program.addr);
        }
        void zip_GoNext(object sender, EventArgs e)
        {
            Program.zip.Swap(Program.dob);
        }
        void dob_GoBack(object sender, EventArgs e)
        {
            Program.dob.Swap( Program.zip);
        }
        void dob_GoNext(object sender, EventArgs e)
        {
            Program.dob.Swap( Program.cellphone);
        }
        void cellphone_GoBack(object sender, EventArgs e)
        {
            Program.cellphone.Swap(Program.dob);
        }
        void cellphone_GoNext(object sender, EventArgs e)
        {
            Program.cellphone.Swap(Program.homephone);
        }
        void homephone_GoNext(object sender, EventArgs e)
        {
            Program.homephone.Swap(Program.gendermarital);
            Program.gendermarital.ShowScreen();
        }
        void homephone_GoBack(object sender, EventArgs e)
        {
            Program.homephone.Swap(Program.cellphone);
        }
        private void ControlsAdd(UserControl ctl)
        {
            ctl.Location = new Point { X = (this.Width / 2) - (ctl.Width / 2), Y = 0 };
            ctl.Visible = false;
            Controls.Add(ctl);
        }
    }
}
