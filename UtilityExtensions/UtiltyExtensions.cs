/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Configuration;
using System.Web.Mvc;
using System.Net.Mail;

namespace UtilityExtensions
{
    public static partial class Util
    {

        public static T QueryString<T>(this System.Web.UI.Page page, string param)
        {
            if (HttpContext.Current.Request.QueryString[param].HasValue())
                return (T)HttpContext.Current.Request[param].ChangeType(typeof(T));
            return default(T);
        }
        public static object ChangeType(this object value, Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                var conv = new NullableConverter(type);
                type = conv.UnderlyingType;
            }
            return Convert.ChangeType(value, type);
        }
        public static int ToInt(this string s)
        {
            int i = 0;
            int.TryParse(s, out i);
            return i;
        }
        public static DateTime? ToDate(this string s)
        {
            DateTime dt;
            if (DateTime.TryParse(s, out dt))
                return dt;
            return null;
        }
        public static int? ToInt2(this string s)
        {
            int? r = null;
            int i;
            if (int.TryParse(s, out i))
                r = i;
            return r;
        }
        public static bool? ToBool2(this string s)
        {
            bool b;
            bool.TryParse(s, out b);
            return b;
        }
        public static bool ToBool(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            bool b;
            bool.TryParse(s, out b);
            return b;
        }
        public static int? ToInt2(this object o)
        {
            int? r = null;
            if (o == null || o == DBNull.Value)
                return r;
            if (o.ToString() == "")
                return r;
            return (int)o.ChangeType(typeof(int));
        }
        public static int ToInt(this object o)
        {
            if (o == null || o == DBNull.Value)
                return 0;
            return (int)o.ChangeType(typeof(int));
        }
        public static bool IsNull(this object o)
        {
            return o == null;
        }
        public static bool IsNotNull(this object o)
        {
            return o != null;
        }
        public static string DefaultTo(this string s, string defaultstr)
        {
            if (s.HasValue())
                return s;
            else
                return defaultstr;
        }
        public static string[] SplitStr(this string s, string delimiter)
        {
            if (s == null)
                return "".Split(delimiter.ToCharArray());
            return s.Split(delimiter.ToCharArray());
        }
        public static string[] SplitStr(this string s, string delimiter, int nitems)
        {
            return s.Split(delimiter.ToCharArray(), nitems);
        }
        public static int? IntOrNull(this string s)
        {
            int? i = null;
            if (s.HasValue())
                i = int.Parse(s);
            return i;
        }
        public static bool HasValue(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }
        public static string Fmt(this string fmt, params object[] p)
        {
            return string.Format(fmt, p);
        }
        public static string EmailHref(this string addr, string name)
        {
            if (!addr.HasValue())
                return "";
            if (name.HasValue())
                return "mailto:{0} <{1}>".Fmt(name, addr);
            else
                return "mailto:" + addr;
        }
        public static string FormatBirthday(int? y, int? m, int? d)
        {
            string dt = "";
            if (m.HasValue && d.HasValue)
                dt = "{0}/{1}".Fmt(m, d);
            if (y.HasValue)
                if (dt != "")
                    dt = dt + "/" + y;
                else
                    dt = y.ToString();
            return dt;
        }
        public static string FormatDate(this DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToString("d");
            return "";
        }
        public static bool DateTryParse(this string date, out DateTime dt)
        {
            return DateTime.TryParse(date, out dt);
        }
        public static string Age(this string birthday)
        {
            DateTime bd;
            if (!birthday.DateTryParse(out bd))
                return "?";
            DateTime td = Now;
            int age = td.Year - bd.Year;
            if (td.Month < bd.Month || (td.Month == bd.Month && td.Day < bd.Day))
                age--;
            return age.ToString();
        }
        public static int AgeAsOf(this DateTime bd, DateTime dt)
        {
            int age = dt.Year - bd.Year;
            if (dt.Month < bd.Month || (dt.Month == bd.Month && dt.Day < bd.Day))
                age--;
            return age;
        }
        public static DateTime Now
        {
            get { return DateTime.Now; }
        }
        public static string FormatCSZ(string city, string st, string zip)
        {
            string csz = city ?? string.Empty;
            if (st.HasValue())
                csz += ", " + st;
            if (zip.HasValue())
                csz += " " + zip.Substring(0, 5);
            return csz.Trim();
        }
        public static string FormatCSZ4(string city, string st, string zip)
        {
            string csz = city ?? string.Empty;
            if (st.HasValue())
                csz += ", " + st;
            csz += " " + FmtZip(zip);
            return csz.Trim();
        }
        public static string FmtFone(this string phone, string prefix)
        {
            phone = phone.FmtFone();
            if (phone.HasValue())
                return prefix + phone;
            return "";
        }
        public static bool AllDigits(this string str)
        {
            Regex patt = new Regex("[^0-9]");
            return !(patt.IsMatch(str));
        }
        public static string FmtFone(this string phone)
        {
            if (!phone.HasValue())
                return "";
            string ext = phone.Substring(phone.LastIndexOfAny("1234567890 -().".ToCharArray()) + 1);
            List<string> tok = new List<string>();
            StringBuilder fone = new StringBuilder(phone.Substring(0, phone.Length - ext.Length));
            fone.Replace("-", " ");
            fone.Replace("(", " ");
            fone.Replace(")", " ");
            fone.Replace(".", " ");

            string[] a = null;
            a = fone.ToString().Split(' ');
            if (a.Length == 0)
                return phone;

            StringBuilder t = new StringBuilder(); // digits only string
            foreach (string i in a)
                if (i != "") // skip over empty parts
                {
                    if (!i.AllDigits())
                        return phone;
                    t.Append(i); // digits only string
                    tok.Add(i);
                }
            if (tok.Count > 3)
                return phone;

            switch (tok.Count) // number of parts
            {
                case 3:
                    if (tok[0].Length != 3)
                        return phone;
                    if (tok[1].Length != 3)
                        return phone;
                    if (tok[2].Length != 4)
                        return phone;
                    break;
                case 2:
                    switch (t.Length)
                    {
                        case 7:
                            if (tok[1].Length != 3)
                                return phone;
                            break;
                        case 10:
                            if (tok[1].Length != 3 && tok[2].Length != 4)
                                return phone;
                            break;
                        default:
                            return phone;
                    }
                    break;
            }
            switch (t.Length)
            {
                case 7:
                    t.Insert(3, "-");
                    break;
                case 10:
                    t.Insert(6, "-");
                    t.Insert(3, ") ");
                    t.Insert(0, "(");
                    break;
                default:
                    return phone; // gotta be 7 or 10 digits
            }
            if (ext != "") // tack extension onto end
                t.Append(" " + ext);
            return t.ToString();
        }
        public static string GetDigits(this string zip)
        {
            if (!zip.HasValue())
                return "";
            var digits = new StringBuilder();
            foreach (var c in zip.ToCharArray())
                if (Char.IsDigit(c))
                    digits.Append(c);
            return digits.ToString();
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

        public static string FmtAttendStr(this string attendstr)
        {
            if (!attendstr.HasValue())
                return " ";
            return attendstr;
        }
        public static string ConnectionString
        {
            get
            {
                var host = HttpContext.Current.Request.Url.Authority;
                if (ConfigurationManager.ConnectionStrings[host].IsNotNull())
                    return ConfigurationManager.ConnectionStrings[host].ConnectionString;
                else
                    return ConfigurationManager.ConnectionStrings["CMS"].ConnectionString;
            }
        }
        public static string ConnectionStringImage
        {
            get
            {
                var host = HttpContext.Current.Request.Url.Authority + ".image";
                if (ConfigurationManager.ConnectionStrings[host].IsNotNull())
                    return ConfigurationManager.ConnectionStrings[host].ConnectionString;
                else
                    return ConfigurationManager.ConnectionStrings["CMSImage"].ConnectionString;
            }
        }
        public static string ConnectionStringTest
        {
            get
            {
                string cms = "CMS2Test";
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session["CMS2Test"] != null)
                            cms = HttpContext.Current.Session["CMS2Test"] as string;
                return ConfigurationManager.ConnectionStrings[cms].ConnectionString;
            }
        }
        public static string UserName
        {
            get
            {

                if (HttpContext.Current != null)
                    return GetUserName(HttpContext.Current.User.Identity.Name);
                return ConfigurationManager.AppSettings["TestName"];
            }
        }
        private const string STR_UserId = "UserId";
        public static int UserId
        {
            get
            {
                int id = 0;
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_UserId] != null)
                            id = HttpContext.Current.Session[STR_UserId].ToInt();
                if (id == 0)
                    id = ConfigurationManager.AppSettings["TestId"].ToInt();
                return id;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_UserId] = value;
            }
        }
        public static int UserId1
        {
            get { return UserId == 0 ? 1 : UserId; }
        }
        private const string STR_UserPeopleId = "UserPeopleId";
        public static int? UserPeopleId
        {
            get
            {
                int? id = null;
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_UserPeopleId] != null)
                            id = HttpContext.Current.Session[STR_UserPeopleId].ToInt();
                return id;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_UserPeopleId] = value;
            }
        }

        public static string GetUserName(string name)
        {
            var a = name.Split('\\');
            if (a.Length == 2)
                return a[1];
            return a[0];
        }
        public static string ScratchPad
        {
            get { return "<--ScratchPad-->"; }
        }
        private const string STR_CurrentTag = "CurrentTag";
        private const string STR_DefaultTag = "UnNamed";
        public static string CurrentTag
        {
            get
            {
                var tag = STR_DefaultTag;
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_CurrentTag] != null)
                            tag = HttpContext.Current.Session[STR_CurrentTag].ToString();
                return tag;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_CurrentTag] = value;
            }
        }
        const string STR_QBScratchPad = "QBScratchPad";
        public static int QueryBuilderScratchPadId
        {
            get
            {
                int id = 0;
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_QBScratchPad] != null)
                            id = HttpContext.Current.Session[STR_QBScratchPad].ToInt();
                return id;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_QBScratchPad] = value;
            }
        }
        const string STR_ActiveOrganizationId = "ActiveOrganizationId";
        public static int CurrentOrgId
        {
            get
            {
                int id = 0;
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_ActiveOrganizationId] != null)
                            id = HttpContext.Current.Session[STR_ActiveOrganizationId].ToInt();
                return id;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_ActiveOrganizationId] = value;
            }
        }
        const string STR_ActiveGroupId = "ActiveGroupId";
        public static int CurrentGroupId
        {
            get
            {
                int id = 0;
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_ActiveGroupId] != null)
                            id = HttpContext.Current.Session[STR_ActiveGroupId].ToInt();
                return id;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_ActiveGroupId] = value;
            }
        }
        const string STR_ActivePersonId = "ActivePersonId";
        public static int CurrentPeopleId
        {
            get
            {
                int id = 0;
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_ActivePersonId] != null)
                            id = HttpContext.Current.Session[STR_ActivePersonId].ToInt();
                return id;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_ActivePersonId] = value;
            }
        }
        public static int? CurrentTagOwnerId
        {
            get
            {
                var pid = Util.UserPeopleId;
                var a = Util.CurrentTag.Split(':');
                if (a.Length > 1)
                    pid = a[0].ToInt2();
                return pid;
            }
        }
        public static string CurrentTagName
        {
            get
            {
                string tag = Util.CurrentTag;
                var a = tag.Split(':');
                if (a.Length == 2)
                    return a[1];
                return tag;
            }
        }
        private const string STR_VisitLookbackDays = "VisitLookbackDays";
        public static int VisitLookbackDays
        {
            get
            {
                var lookback = 180;
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_VisitLookbackDays] != null)
                            lookback = HttpContext.Current.Session[STR_VisitLookbackDays].ToInt();
                return lookback;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_VisitLookbackDays] = value;
            }
        }
        public static string SessionId
        {
            get { return HttpContext.Current.Session.SessionID; }
        }
        public const string STR_OrgMembersOnly = "OrgMembersOnly";
        public static bool OrgMembersOnly
        {
            get
            {
                bool tf = false;
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_OrgMembersOnly] != null)
                            tf = (bool)HttpContext.Current.Session[STR_OrgMembersOnly];
                return tf;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_OrgMembersOnly] = value;
            }
        }
        private const string STR_SessionStarting = "SessionStarting";
        public static bool SessionStarting
        {
            get { return (bool)HttpContext.Current.Session[STR_SessionStarting]; }
            set { HttpContext.Current.Session[STR_SessionStarting] = value; }
        }
        private const string STR_FormsBasedAuthentication = "FormsBasedAuthentication";
        public static bool FormsBasedAuthentication
        {
            get
            {
                object tf = HttpContext.Current.Session[STR_FormsBasedAuthentication];
                if (tf.IsNotNull())
                    return (bool)tf;
                else return false;
            }
            set { HttpContext.Current.Session[STR_FormsBasedAuthentication] = value; }
        }
        private const string STR_Auditing = "Auditing";
        public static bool Auditing
        {
            get
            {
                object tf = HttpContext.Current.Items[STR_Auditing];
                if (tf.IsNotNull())
                    return (bool)tf;
                else return true;
            }
            set { HttpContext.Current.Items[STR_Auditing] = value; }
        }
        public static string SafeFormat(string s)
        {
            if (s == null)
                return null;
            s = HttpContext.Current.Server.HtmlEncode(s);

            s = Regex.Replace(s, "(http://([^\\s]*))", "<a target=\"_new\" href=\"$1\">$2</a>", RegexOptions.Singleline);
            s = HtmlFormat(s, "\\*\\*\\*", "u");
            s = HtmlFormat(s, "\\*\\*", "b");
            s = HtmlFormat(s, "\\*", "i");
            s = Regex.Replace(s, @"&quot;(.*?)&quot;\s*=\s*&quot;(.*?)&quot;",
                "<a target=\"_new\" href=\"http://$2\">$1</a>", RegexOptions.Singleline);
            s = Regex.Replace(s, "&gt;&gt;&gt;(?:\r\n)?(.*?)(?:\r\n)?&lt;&lt;&lt;(?:\r\n)?",
                "<p style='margin-left:.5in'>$1</p>", RegexOptions.Singleline);
            s = s.Replace(System.Environment.NewLine, "\n");
            return s.Replace("\n", "<br/>");
        }

        private static string HtmlFormat(string s, string lookfor, string htmlcode)
        {
            return Regex.Replace(s, "{0}(.*?){0}".Fmt(lookfor),
                "<{0}>$1</{0}>".Fmt(htmlcode), RegexOptions.Singleline);
        }
        public const string STR_Preferences = "Preferences";
        public const string STR_PageSize = "PageSize";
        public static void SetPageSizeCookie(int value)
        {
            var cookie = new HttpCookie(STR_Preferences);
            cookie.Values[STR_PageSize] = value.ToString();
            cookie.Expires = DateTime.MaxValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        public static int GetPageSizeCookie()
        {
            HttpRequest r = null;
            if (HttpContext.Current != null)
                r = HttpContext.Current.Request;
            if (r != null && r.Cookies[STR_Preferences] != null)
            {
                var cookie = r.Cookies[STR_Preferences];
                if (cookie != null && cookie.Values[STR_PageSize] != null)
                    return cookie.Values[STR_PageSize].ToInt();
            }
            return 10;
        }
        public static string IpAddress() 
        { 
            string strIpAddress;
            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strIpAddress == null)
                strIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; 
            return strIpAddress; 
        }
        public static void Cookie(string name, string value, int days)
        {
            if (Cookie(name) == value)
                return;
            var c = new HttpCookie(name, value);
            c.Expires = DateTime.Now.AddDays(days);
            HttpContext.Current.Response.Cookies.Add(c);
            HttpContext.Current.Items["tCookie-" + name] = value;
        }
        public static string Cookie(string name)
        {
            return Cookie(name, null);
        }
        public static string Cookie(string name, string defaultValue)
        {
            var v = (string)HttpContext.Current.Items["tCookie-" + name];
            if (v.HasValue())
                return v;
            var c = HttpContext.Current.Request.Cookies[name];
            if (c != null && c.Value.HasValue())
                return c.Value;
            return defaultValue;
        }

        public static IEnumerable<SelectListItem> PageSizes()
        {
            var sizes = new int[] { 10, 25, 50, 75, 100, 200 };
            var list = new List<SelectListItem>();
            foreach (var size in sizes)
                list.Add(new SelectListItem { Text = size.ToString() });
            return list;
        }
        public static List<SelectListItem> WithNotSpecified(this IEnumerable<SelectListItem> q)
        {
            return q.WithNotSpecified("0");
        }
        public static List<SelectListItem> WithNotSpecified(this IEnumerable<SelectListItem> q, string value)
        {
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = value, Text = "(not specified)" });
            return list;
        }
        public static void EndShowMessage(this HttpResponse Response, string message)
        {
            Response.EndShowMessage(message, "javascript: history.go(-1)", "Go Back");
        }
        public static void EndShowMessage(this HttpResponse Response, string message, string href, string text)
        {
            Response.Clear();
            Response.Write("<h3 style='color:red'>{0}</h3>\n<a href='{1}'>{2}</a>".Fmt(message,href,text));
            Response.End();
        }
        public static MailAddress TryGetMailAddress(string address, string name)
        {
            try
            {
                var m = new MailAddress(address, name);
                return m;
            }
            catch
            {
                return null;
            }
        }
        public static bool ValidEmail(string email)
        {
            try
            {
                var a = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class EventArg<T> : EventArgs
    {
        private T value;
        public T Value { get { return value; } }
        public EventArg(T value)
        {
            this.value = value;
        }
    }
}

