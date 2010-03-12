using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using System.Net;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace CmsCheckin
{
    public static class Util
    {
        public static string ServiceUrl()
        {
            string serviceurl = ConfigurationSettings.AppSettings["ServiceUrl"];
            if (Program.TestMode)
                serviceurl = ConfigurationSettings.AppSettings["ServiceUrlTest"];
            return serviceurl;
        }
        public static string GetDigits(string s)
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

            f.Cursor = Cursors.WaitCursor;
            Cursor.Show();
            var str = wc.DownloadString(url);
            if (Program.HideCursor)
                Cursor.Hide();
            f.Cursor = Cursors.Default;

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

        public static bool RecordAttend(this Control f, ClassInfo c, bool present)
        {
            if (c.OrgId == 0)
                return false;
            try
            {
                f.Cursor = Cursors.WaitCursor;
                var wc = new WebClient();
                var coll = new NameValueCollection();
                coll.Add("PeopleId", c.PeopleId.ToString());
                coll.Add("OrgId", c.OrgId.ToString());
                coll.Add("Present", present.ToString());
                coll.Add("thisday", Program.ThisDay.ToString());
                var url = new Uri(new Uri(Util.ServiceUrl()), "Checkin/RecordAttend/");

                f.Cursor = Cursors.WaitCursor;
                Cursor.Show();
                var resp = wc.UploadValues(url, "POST", coll);

                var s = Encoding.ASCII.GetString(resp);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (Program.HideCursor)
                    Cursor.Hide();
                f.Cursor = Cursors.Default;
            }
            return true;
        }
    }
}
