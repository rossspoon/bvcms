using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CmsCheckin
{
    public partial class EnterGenderMarital : UserControl
    {
        public EnterGenderMarital()
        {
            InitializeComponent();
        }
        public void ShowScreen()
        {
            first.Text = Program.first.textBox1.Text;
            goesby.Text = Program.goesby.textBox1.Text;
            last.Text = Program.last.textBox1.Text;
            email.Text = Program.email.textBox1.Text;
            dob.Text = Program.dob.textBox1.Text;
            cellphone.Text = Program.cellphone.textBox1.Text;
            homephone.Text = Program.homephone.textBox1.Text;
            addr.Text = Program.addr.textBox1.Text;
            zip.Text = Program.zip.textBox1.Text;
            AddAnother.Visible = !Program.editing;
        }

        private void buttongo_Click(object sender, EventArgs e)
        {
            if (Marital == 0 || Gender == 0)
                return;
            
           AddPersonGoHome();
        }
        private void AddPersonGoHome()
        {
            var gender = Gender;
            var marital = Marital;
            if (Program.editing)
                this.EditPerson(Program.PeopleId, first.Text, last.Text, goesby.Text, dob.Text, email.Text, addr.Text, zip.Text, cellphone.Text, homephone.Text, marital, gender);
            else
                this.AddPerson(first.Text, last.Text, goesby.Text, dob.Text, email.Text, addr.Text, zip.Text, cellphone.Text, homephone.Text, marital, gender);

            string ph;
            if (!string.IsNullOrEmpty(homephone.Text))
                ph =homephone.Text;
            else
                if (!string.IsNullOrEmpty(cellphone.Text))
                    ph = cellphone.Text;
                else
                    ph = "";
            Program.ClearFields();
            this.GoHome(ph);
        }

        public int Gender
        {
            get
            {
                return Male.Checked ? 1 : Female.Checked ? 2 : 0;
            }
            set
            {
                switch (value)
                {
                    case 1:
                        Male.Checked = true;
                        break;
                    case 2:
                        Female.Checked = true;
                        break;
                    default:
                        Male.Checked = false;
                        Female.Checked = false;
                        break;
                }
            }
        }
        public int Marital
        {
            get
            {
                var marital = 0;
                if (married.Checked)
                    marital = 20;
                else if (single.Checked)
                    marital = 10;
                else if (separated.Checked)
                    marital = 30;
                else if (divorced.Checked)
                    marital = 40;
                else if (widowed.Checked)
                    marital = 50;
                else
                    marital = 0;
                return marital;
            }
            set
            {
                switch (value)
                {
                    case 10:
                        single.Checked = true;
                        break;
                    case 20:
                        married.Checked = true;
                        break;
                    case 30:
                        separated.Checked = true;
                        break;
                    case 40:
                        divorced.Checked = true;
                        break;
                    case 50:
                        widowed.Checked = true;
                        break;
                    default:
                        single.Checked = false;
                        married.Checked = false;
                        separated.Checked = false;
                        divorced.Checked = false;
                        widowed.Checked = false;
                        break;
                }
            }
        }
        private void GoBack_Click(object sender, EventArgs e)
        {
            this.Swap(Program.homephone);
        }

        private void AddAnother_Click(object sender, EventArgs e)
        {
            if (Marital == 0 || Gender == 0)
                return;
            this.AddPerson(first.Text, last.Text, goesby.Text, dob.Text, email.Text, addr.Text, zip.Text, cellphone.Text, homephone.Text, Marital, Gender);
            Program.ClearFields();
            Program.SetFields(last.Text, email.Text, addr.Text, zip.Text, homephone.Text);
            this.Swap(Program.first);
        }
    }
}
