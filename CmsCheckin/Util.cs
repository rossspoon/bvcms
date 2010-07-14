using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using System.Net;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace CmsCheckin
{
    public static class Util
    {
        public static string ServiceUrl()
        {
            string serviceurl = ConfigurationManager.AppSettings["ServiceUrl"];
            if (Program.TestMode)
                serviceurl = ConfigurationManager.AppSettings["ServiceUrlTest"];
            return serviceurl;
        }
        public static string GetDigits(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            var digits = new StringBuilder();
            foreach (var c in s.ToCharArray())
                if (Char.IsDigit(c))
                    digits.Append(c);
            return digits.ToString();
        }
        public static string Fmt(this string fmt, params object[] p)
        {
            return string.Format(fmt, p);
        }
        public static XDocument GetDocument(this Control f, string page)
        {
            var wc = new WebClient();
            var url = new Uri(new Uri(Util.ServiceUrl()), page);

            var str = wc.DownloadString(url);

            var x = XDocument.Parse(str);
            return x;
        }
        public static int ToInt(this string s)
        {
            int i = 0;
            int.TryParse(s, out i);
            return i;
        }
        public static int? ToInt2(this string s)
        {
            int? r = null;
            int i;
            if (int.TryParse(s, out i))
                r = i;
            return r;
        }

        public static void AddPerson(this Control f,
            string first,
            string last,
            string goesby,
            string dob,
            string email,
            string addr,
            string zip,
            string cell,
            string home,
            string allergies,
            string grade,
            string parent, 
            string emfriend,
            string emphone,
            CheckState activeother,
            int marital,
            int gender)
        {
            var wc = new WebClient();
            var coll = new NameValueCollection();
            coll.Add("first", first);
            coll.Add("last", last);
            coll.Add("goesby", goesby);
            coll.Add("dob", dob);
            coll.Add("email", email);
            coll.Add("addr", addr);
            coll.Add("zip", zip);
            coll.Add("cell", cell);
            if (home.HasValue())
                coll.Add("home", home);
            else
                coll.Add("home", cell);
            coll.Add("marital", marital.ToString());
            coll.Add("gender", gender.ToString());
            coll.Add("campusid", Program.CampusId.ToString());
            coll.Add("allergies", allergies);
            coll.Add("grade", grade);
            coll.Add("parent", parent);
            coll.Add("emfriend", emfriend);
            coll.Add("emphone", emphone.GetDigits());
            coll.Add("activeother", (activeother == CheckState.Checked).ToString());

            var url = new Uri(new Uri(Util.ServiceUrl()), "Checkin/AddPerson/" + Program.FamilyId);

            var resp = wc.UploadValues(url, "POST", coll);

            var s = Encoding.ASCII.GetString(resp);
            Program.FamilyId = s.ToInt();
        }
        public static void EditPerson(this Control f,
            int id,
            string first,
            string last,
            string goesby,
            string dob,
            string email,
            string addr,
            string zip,
            string cell,
            string home,
            string allergies,
            string grade,
            string parent, 
            string emfriend,
            string emphone,
            CheckState activeother,
            int marital,
            int gender)
        {
            var wc = new WebClient();
            var coll = new NameValueCollection();
            coll.Add("first", first);
            coll.Add("last", last);
            coll.Add("goesby", goesby);
            coll.Add("dob", dob);
            coll.Add("email", email);
            coll.Add("addr", addr);
            coll.Add("zip", zip);
            coll.Add("cell", cell);
            if (home.HasValue())
                coll.Add("home", home);
            else
                coll.Add("home", cell);
            coll.Add("marital", marital.ToString());
            coll.Add("gender", gender.ToString());
            coll.Add("campusid", Program.CampusId.ToString());
            coll.Add("allergies", allergies);
            coll.Add("grade", grade);
            coll.Add("parent", parent);
            coll.Add("emfriend", emfriend);
            coll.Add("emphone", emphone.GetDigits());
            coll.Add("activeother", (activeother == CheckState.Checked).ToString());
            var url = new Uri(new Uri(Util.ServiceUrl()), "Checkin/EditPerson/" + id);

            var resp = wc.UploadValues(url, "POST", coll);

            var s = Encoding.ASCII.GetString(resp);
        }
        public static bool AllDigits(string str)
        {
            Regex patt = new Regex("[^0-9]");
            return !(patt.IsMatch(str));
        }
        public static string FmtFone(this string phone)
        {
            var ph = phone.GetDigits();
            if (string.IsNullOrEmpty(ph))
                return "";
            var t = new StringBuilder(ph);

            if (ph.Length >= 4)
                t.Insert(3, "-");
            if (ph.Length >= 8)
                t.Insert(7, "-");
            return t.ToString();
        }
        public static string FmtDate(this string d)
        {
            var dt = d.GetDigits();
            if (string.IsNullOrEmpty(dt))
                return "";
            var t = new StringBuilder(dt);

            if (dt.Length >= 2)
                t.Insert(2, "/");
            if (dt.Length >= 8)
                t.Insert(7, "-");
            return t.ToString();
        }
        public static string FmtZip(this string zip)
        {
            if (!zip.HasValue())
                return "";
            var t = new StringBuilder(zip.GetDigits());
            if (t.Length != 9)
                return zip;
            t.Insert(5, "-");
            return t.ToString();
        }

        public static bool DateIsOK(string s)
        {
            var re = new Regex(@"\A(([1-9]|1[012])(/(?<day>3[01]|[12][0-9]|[1-9])?)?(?<g3>/([0-9]{1,2})?)?)\Z");
            if (re.IsMatch(s))
            {
                var m = re.Match(s);
                if (m.Groups["day"].Length == 0 && m.Groups["g3"].Length > 0)
                    return false;
                return true;
            }
            return false;
        }
        public static bool DateValid(string s)
        {
            var dt = DateTime.MinValue;
            if (!s.HasValue())
                return true;
            if (Regex.IsMatch(s, @"\A(?:\A(0?[1-9]|1[012])[-/](0?[1-9]|[12][0-9]|3[01])[-/](19|20)?[0-9]{2}\s*\z)\Z"))
                if (DateTime.TryParse(s, out dt))
                {
                    //if (dt > DateTime.Now.Date)
                    //    dt = dt.AddYears(-100);
                    return true;
                }
            return false;
        }
        public static bool HasValue(this string s)
        {
            if (s != null)
                s = s.Trim();
            return !string.IsNullOrEmpty(s);
        }
        public static string ToTitleCase(this string s)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s);
        }
        public static void Swap(this UserControl c1, UserControl c2)
        {
            Program.TimerStop();
            c1.Visible = false;
            c2.Visible = true;
        }
        public static void GoHome(this UserControl c, string s)
        {
            c.Swap(Program.home);
            Program.ClearFields();
            Program.CursorHide();
            Program.home.textBox1.Text = s.FmtFone();
            Program.home.textBox1.Focus();
            Program.home.textBox1.Select(Program.home.textBox1.Text.Length, 0);
        }
        public static string Age(this string birthday)
        {
            DateTime bd;
            if (!DateTime.TryParse(birthday, out bd))
                return "?";
            DateTime td = DateTime.Now;
            int age = td.Year - bd.Year;
            if (td.Month < bd.Month || (td.Month == bd.Month && td.Day < bd.Day))
                age--;
            if (age < 0)
                return "?";
            return age.ToString();
        }
        public class ClassCheckedInfo
        {
            public bool ischecked { get; set; }
            public ClassInfo c { get; set; }
        }
        public static void CheckUnCheckClass(ClassCheckedInfo info)
        {
            if (info.c.oid == 0)
                return;
            try
            {
                var wc = new WebClient();
                var coll = new NameValueCollection();
                coll.Add("PeopleId", info.c.pid.ToString());
                coll.Add("OrgId", info.c.oid.ToString());
                Uri url = null;
                if (Program.KioskMode)
                {
                    coll.Add("Member", info.ischecked.ToString());
                    url = new Uri(new Uri(Util.ServiceUrl()), "Checkin/Membership/");
                }
                else
                {
                    coll.Add("Present", info.ischecked.ToString());
                    coll.Add("thisday", Program.ThisDay.ToString());
                    url = new Uri(new Uri(Util.ServiceUrl()), "Checkin/RecordAttend/");
                }

                var resp = wc.UploadValues(url, "POST", coll);
#if DEBUG
                //System.Threading.Thread.Sleep(1500);
#endif
                var s = Encoding.ASCII.GetString(resp);
            }
            catch (Exception)
            {
            }
        }
        public static void UnLockFamily()
        {
            if (Program.FamilyId == 0)
                return;
            try
            {
                var wc = new WebClient();
                var coll = new NameValueCollection();
                coll.Add("fid", Program.FamilyId.ToString());
                var url = new Uri(new Uri(Util.ServiceUrl()), "Checkin/UnLockFamily/");
                var resp = wc.UploadValues(url, "POST", coll);
                var s = Encoding.ASCII.GetString(resp);
            }
            catch (Exception)
            {
            }
        }

    }
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            m_value = value;
        }

        private T m_value;

        public T Value
        {
            get { return m_value; }
        }
    }
}
