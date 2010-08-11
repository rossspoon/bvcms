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
            Program.home = new Home();
            Program.CursorHide();
            ControlsAdd(Program.home);
            Program.home.Visible = true;
            Program.home.textBox1.Focus();

            Program.namesearch = new EnterText("Name Search", true);
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
            ControlsAdd(Program.first);

            Program.goesby = new EnterText("Goes By", true);
            ControlsAdd(Program.goesby);

            Program.last = new EnterText("Last Name", true);
            ControlsAdd(Program.last);

            Program.email = new EnterText("Email");
            ControlsAdd(Program.email);

            Program.addr = new EnterText("Address", true);
            ControlsAdd(Program.addr);

            Program.zip = new EnterText("Zip");
            ControlsAdd(Program.zip);

            Program.dob = new EnterDate("Birthday");
            ControlsAdd(Program.dob);

            Program.cellphone = new EnterPhone("Cell Phone");
            ControlsAdd(Program.cellphone);

            Program.homephone = new EnterPhone("Home Phone");
            ControlsAdd(Program.homephone);

            Program.allergy = new EnterText("Allergies");
            ControlsAdd(Program.allergy);

            Program.gendermarital = new EnterGenderMarital();
            ControlsAdd(Program.gendermarital);

            Program.namesearch.GoBack += new EventHandler(namesearch_GoBack);
            Program.namesearch.GoNext += new EventHandler(namesearch_GoNext);

            Program.first.GoBack += new EventHandler(first_GoBack);
            Program.first.SetBackNext(null, Program.goesby);
            Program.goesby.SetBackNext(Program.first, Program.last);
            Program.last.SetBackNext(Program.goesby, Program.email);
            Program.email.SetBackNext(Program.last, Program.addr);
            Program.addr.SetBackNext(Program.email, Program.zip);
            Program.zip.SetBackNext(Program.addr, Program.dob);

            if (Program.AskGrade)
            {
                Program.grade = new EnterNumber("Grade");
                ControlsAdd(Program.grade);

                Program.dob.SetBackNext(Program.zip, Program.grade);
                Program.grade.SetBackNext(Program.dob, Program.cellphone);
                Program.cellphone.SetBackNext(Program.grade, Program.homephone);
            }
            else
            {
                Program.dob.SetBackNext(Program.zip, Program.cellphone);
                Program.cellphone.SetBackNext(Program.dob, Program.homephone);
            }

            if (Program.AskEmFriend)
            {
                Program.parent = new EnterText("Parent Name", true);
                ControlsAdd(Program.parent);
                Program.emfriend = new EnterText("Emergency Friend", true);
                ControlsAdd(Program.emfriend);
                Program.emphone = new EnterPhone("Emergency Phone");
                ControlsAdd(Program.emphone);

                Program.homephone.SetBackNext(Program.cellphone, Program.parent);
                Program.parent.SetBackNext(Program.homephone, Program.emfriend);
                Program.emfriend.SetBackNext(Program.parent, Program.emphone);
                if (Program.AskChurchName)
                {
                    Program.church = new EnterText("Church Name", true);
                    ControlsAdd(Program.church);

                    Program.emphone.SetBackNext(Program.emfriend, Program.church);
                    Program.church.SetBackNext(Program.emphone, Program.allergy);
                    Program.allergy.SetBackNext(Program.church, null);
                }
                else
                {
                    Program.emphone.SetBackNext(Program.emfriend, Program.allergy);
                    Program.allergy.SetBackNext(Program.emphone, null);
                }
            }
            else
            {
                if (Program.AskChurchName)
                {
                    Program.church = new EnterText("Church Name", true);
                    ControlsAdd(Program.church);

                    Program.homephone.SetBackNext(Program.cellphone, Program.church);
                    Program.church.SetBackNext(Program.homephone, Program.allergy);
                    Program.allergy.SetBackNext(Program.church, null);
                }
                else
                {
                    Program.homephone.SetBackNext(Program.cellphone, Program.allergy);
                    Program.allergy.SetBackNext(Program.homephone, null);
                }
            }
            Program.allergy.GoNext += new EventHandler(allergy_GoNext);
        }

        void first_GoBack(object sender, EventArgs e)
        {
            Program.ClearFields();
            Program.first.GoHome(string.Empty);
        }
        void namesearch_GoNext(object sender, EventArgs e)
        {
            Program.namesearch.Swap(Program.names);
            string name = Program.namesearch.textBox1.Text;
            Program.namesearch.textBox1.Text = String.Empty;
            Program.names.ShowResults(name, 1);
        }
        void namesearch_GoBack(object sender, EventArgs e)
        {
            Program.namesearch.textBox1.Text = String.Empty;
            Program.namesearch.GoHome(string.Empty);
        }
        void allergy_GoNext(object sender, EventArgs e)
        {
            Program.allergy.Swap(Program.gendermarital);
            Program.gendermarital.ShowScreen();
        }

        private void ControlsAdd(UserControl ctl)
        {
            ctl.Location = new Point { X = (this.Width / 2) - (ctl.Width / 2), Y = 0 };
            ctl.Visible = false;
            Controls.Add(ctl);
        }
    }
}
