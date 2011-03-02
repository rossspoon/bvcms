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
using System.Net.Mail;
using System.Web.UI;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Net;
using System.Security.Cryptography;
using System.Web.Configuration;
using System.Diagnostics;
using System.Web.Caching;

namespace UtilityExtensions
{
    public static partial class Util
    {

        public static T QueryString<T>(this System.Web.UI.Page page, string param)
        {
            return QueryString<T>(HttpContext.Current.Request, param);
        }
        public static T QueryString<T>(this System.Web.HttpRequest req, string param)
        {
            if (req.QueryString[param].HasValue())
                return (T)req.QueryString[param].ChangeType(typeof(T));
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
        public static DateTime? ToDate(this object o)
        {
            return o.ToString().ToDate();
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
        public static decimal? ToDecimal(this string s)
        {
            decimal? r = null;
            decimal i;
            if (decimal.TryParse(s, out i))
                r = i;
            return r;
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
            return !string.IsNullOrWhiteSpace(s);
        }
        public static string Fmt(this string fmt, params object[] p)
        {
            return string.Format(fmt, p);
        }
        public static string Truncate(this string source, int length)
        {
            if (source.HasValue() && source.Length > length)
                source = source.Substring(0, length).Trim();
            return source;
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
                    dt = dt + "/" + y.ToString();
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
        public static string FormatDate2(this DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToString("M/d/yy");
            return "";
        }
        public static string FormatDate(this DateTime? dt, string def)
        {
            if (dt.HasValue)
                return dt.Value.ToString("d");
            return def;
        }
        public static string FormatDateTm(this DateTime dt)
        {
            return dt.ToString("M/d/yy H:mm");
        }
        public static string FormatDateTm(this DateTime? dt)
        {
            return dt.FormatDateTm(null);
        }
        public static string FormatDateTm(this DateTime? dt, string def)
        {
            if (dt.HasValue)
                return dt.ToString2("M/d/yy H:mm");
            return def;
        }
        public static string ToString2(this int? i, string fmt)
        {
            if (i.HasValue)
                return i.Value.ToString(fmt);
            return "";
        }
        public static string ToString2(this decimal? d, string fmt)
        {
            if (d.HasValue)
                return d.Value.ToString(fmt);
            return "";
        }
        public static string ToString2(this DateTime? d, string fmt)
        {
            if (d.HasValue)
                return d.Value.ToString(fmt);
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
        public static int Age0(this string birthday)
        {
            DateTime bd;
            if (!birthday.DateTryParse(out bd))
                return -1;
            DateTime td = Now;
            int age = td.Year - bd.Year;
            if (td.Month < bd.Month || (td.Month == bd.Month && td.Day < bd.Day))
                age--;
            return age;
        }
        public static string URLCombine(string baseUrl, string relativeUrl)
        {
            if (baseUrl.Length == 0)
                return relativeUrl;
            if (relativeUrl.Length == 0)
                return baseUrl;
            return string.Format("{0}/{1}", baseUrl.TrimEnd(new char[] { '/', '\\' }), relativeUrl.TrimStart(new char[] { '/', '\\' }));
        }
        public static int AgeAsOf(this DateTime bd, DateTime dt)
        {
            int y = bd.Year;
            if (y < 1000)
                if (y < 50)
                    y = y + 2000;
                else y = y + 1900;
            int age = dt.Year - y;
            if (dt.Month < bd.Month || (dt.Month == bd.Month && dt.Day < bd.Day))
                age--;
            return age;
        }
        public static DateTime Now2
        {
            get
            {
                var daysoffset = (double?)HttpContext.Current.Application["daysoffset"];
                if (daysoffset.HasValue)
                    return DateTime.Now.AddDays(daysoffset.Value);
                return DateTime.Now;
            }
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
            if (zip.HasValue() && zip.Length >= 5)
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
        public static string FmtFone7(this string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return "";
            var ph = GetDigits(phone).PadLeft(10, '0');
            var p7 = ph.Substring(3);
            var t = new StringBuilder(p7);
            if (t.Length >= 4)
                t.Insert(3, "-");
            return t.ToString();
        }
        public static string FmtFone7(this string phone, string prefix)
        {
            phone = phone.FmtFone7();
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
            var ph = phone.GetDigits();
            if (!ph.HasValue())
                return "";
            string ext = ph.Substring(ph.LastIndexOfAny("1234567890 -().".ToCharArray()) + 1);
            List<string> tok = new List<string>();
            StringBuilder fone = new StringBuilder(ph.Substring(0, ph.Length - ext.Length));
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
        public static decimal? GetAmount(this string s)
        {
            if (!s.HasValue())
                return null;
            var digits = new StringBuilder();
            foreach (var c in s.ToCharArray())
                if (Char.IsDigit(c) || c == '.')
                    digits.Append(c);
            var a = digits.ToString().ToDecimal();
            return a;
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
        public static string Zip5(this string zip)
        {
            if (!zip.HasValue())
                return "";
            var t = zip.GetDigits();
            if (t.Length > 5)
                return t.Substring(0, 5);
            return t;
        }
        public static bool DateValid(string dt, out DateTime dt2)
        {
            dt2 = DateTime.MinValue;
            if (!dt.HasValue())
                return false;
            if (Regex.IsMatch(dt, @"\A(?:\A(0?[1-9]|1[012])[-/](0?[1-9]|[12][0-9]|3[01])[-/](19|20)?[0-9]{2}\s*\z)\Z"))
                if (DateTime.TryParse(dt, out dt2))
                    return true;
            if (!Regex.IsMatch(dt, @"\A(?:\A(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])((19|20)?[0-9]{2}))\Z"))
                return false;
            var s = dt.Substring(0, 2) + "/" + dt.Substring(2, 2) + "/" + dt.Substring(4);
            if (DateTime.TryParse(s, out dt2))
                return true;
            return false;
        }
        public static bool BirthDateValid(string dt, out DateTime dt2)
        {
            dt2 = DateTime.MinValue;
            if (!dt.HasValue())
                return false;
            if (!Regex.IsMatch(dt, @"\A(?:\A(0?[1-9]|1[012])[-/](0?[1-9]|[12][0-9]|3[01])[-/](19|20)?[0-9]{2}\s*\z)\Z")
                    && !Regex.IsMatch(dt, @"\A(?:\A(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])((19|20)?[0-9]{2}))\Z"))
                return false;
            if (DateTime.TryParse(dt, out dt2))
            {
                if (dt2 > DateTime.Now)
                    dt2 = dt2.AddYears(-100);
                return true;
            }
            return false;
        }

        public static string FmtAttendStr(this string attendstr)
        {
            if (!attendstr.HasValue())
                return " ";
            return attendstr;
        }
        private const string STR_Host = "HostString";
        public static string Host
        {
            get
            {
                var h = ConfigurationManager.AppSettings["host"];
                if (h.HasValue())
                    return h;
                if (HttpContext.Current != null)
                    return HttpContext.Current.Request.Url.Authority.SplitStr(".:")[0];
                return null;
            }
        }

        public static string CmsHost
        {
            get
            {
                var h = ConfigurationManager.AppSettings["cmshost"];
                return h.Replace("{church}", Host);
            }
        }
        private const string STR_ConnectionString = "ConnectionString";
        public static string ConnectionString
        {
            get
            {
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_ConnectionString] != null)
                            return HttpContext.Current.Session[STR_ConnectionString].ToString();
                var cs = ConfigurationManager.ConnectionStrings["CMSHosted"];
                if (cs == null)
                    return ConfigurationManager.ConnectionStrings["CMS"].ConnectionString;

                var cb = new SqlConnectionStringBuilder(cs.ConnectionString);
                cb.InitialCatalog = "CMS_{0}".Fmt(Host);
                return cb.ConnectionString;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_ConnectionString] = value;
            }
        }
        public static string GetConnectionString(string Host)
        {
            var cs = ConfigurationManager.ConnectionStrings["CMSHosted"];
            if (cs == null)
                cs = ConfigurationManager.ConnectionStrings["CMS"];
            var cb = new SqlConnectionStringBuilder(cs.ConnectionString);
            var a = Host.SplitStr(".:");
            cb.InitialCatalog = "CMS_{0}".Fmt(a[0]);
            return cb.ConnectionString;
        }
        public static string ConnectionStringImage
        {
            get
            {
                var cs = ConfigurationManager.ConnectionStrings["CMSHosted"];
                if (cs == null)
                    cs = ConfigurationManager.ConnectionStrings["CMS"];
                var cb = new SqlConnectionStringBuilder(cs.ConnectionString);
                var a = Host.SplitStr(".:");
                cb.InitialCatalog = "CMS_{0}_img".Fmt(a[0]);
                return cb.ConnectionString;
            }
        }
        //public static string ConnectionStringDisc
        //{
        //    get
        //    {
        //        var cs = ConfigurationManager.ConnectionStrings["CMSHosted"];
        //        if (cs == null)
        //            return ConfigurationManager.ConnectionStrings["Disc"].ConnectionString;

        //        var cb = new SqlConnectionStringBuilder(cs.ConnectionString);
        //        cb.InitialCatalog = "CMS_{0}_disc".Fmt(Host1);
        //        return cb.ConnectionString;
        //    }
        //}
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
                    if (HttpContext.Current.Session != null)
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
            if (name == null)
                return null;
            var a = name.Split('\\');
            if (a.Length == 2)
                return a[1];
            return a[0];
        }
        public static string ScratchPad
        {
            get { return "<--ScratchPad-->"; }
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
        public static int CreateAccountCode = -1952;
        public static string SessionId
        {
            get
            {
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        return HttpContext.Current.Session.SessionID;
                return null;
            }
        }
        private const string STR_SessionStarting = "SessionStarting";
        public static bool SessionStarting
        {
            get
            {
                bool tf = false;
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_SessionStarting] != null)
                            tf = (bool)HttpContext.Current.Session[STR_SessionStarting];
                return tf;
            }
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
        private const string STR_Helpfile = "Helpfile";
        public static string Helpfile
        {
            get
            {
                var tag = "MainPage";
                if (HttpContext.Current.Session[STR_Helpfile] != null)
                    tag = HttpContext.Current.Session[STR_Helpfile].ToString();
                return tag;
            }
            set
            {
                HttpContext.Current.Session[STR_Helpfile] = value;
            }
        }
        public static string HelpLink(string page)
        {
            var h = ConfigurationManager.AppSettings["helpurl"];
            return h.Fmt(page);
        }

        public static string SafeFormat(string s)
        {
            if (s == null)
                return null;
            s = HttpUtility.HtmlEncode(s);


            //s = Regex.Replace(s, "([^ ]*)=(https?://[^ ]*)", "<a target=\"_new\" href=\"$2\">$1</a>", RegexOptions.Singleline);
            //s = Regex.Replace(s, "[^=](https?://[^\\s]*)", "<a target=\"_new\" href=\"$1\">$1</a>", RegexOptions.Singleline);

            s = Regex.Replace(s, "(http://([^\\s]*))", "<a target=\"_new\" href=\"$1\">$1</a>", RegexOptions.Singleline);
            s = Regex.Replace(s, "(https://([^\\s]*))", "<a target=\"_new\" href=\"$1\">$1</a>", RegexOptions.Singleline);
            s = HtmlFormat(s, "\\*\\*\\*", "u");
            s = HtmlFormat(s, "\\*\\*", "b");
            s = HtmlFormat(s, "\\*", "i");
            s = Regex.Replace(s, @"&quot;(.*?)&quot;\s*=\s*&quot;(.*?)&quot;",
                "<a target=\"_new\" href=\"http://$2\">$1</a>", RegexOptions.Singleline);
            s = Regex.Replace(s, "&gt;&gt;&gt;(?:\r\n)?(.*?)(?:\r\n)?&lt;&lt;&lt;(?:\r\n)?",
                "<blockquote>$1</blockquote>", RegexOptions.Singleline);
            s = s.Replace(System.Environment.NewLine, "\n");
            return s.Replace("\n", "<br>\r\n");
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
        public static void EndShowMessage(this HttpResponse Response, string message)
        {
            Response.EndShowMessage(message, "javascript: history.go(-1)", "Go Back");
        }
        public static void EndShowMessage(this HttpResponse Response, string message, string href, string text)
        {
            Response.Clear();
            Response.Write(EndShowMessage(message, href, text));
            Response.End();
        }
        public static string EndShowMessage(string message, string href, string text)
        {
            return "<h3 style='color:red'>{0}</h3>\n<a href='{1}'>{2}</a>".Fmt(message, href, text);
        }
        public static MailAddress TryGetMailAddress(string address, string name)
        {
            if (address.HasValue())
                address = address.Trim();
            if (ValidEmail(address))
                return Util.FirstAddress(address, name);
            else
                return null;
        }
        public static bool ValidEmail(string email)
        {
            if (!email.HasValue())
                return false;
            var re1 = new Regex(@"^(.*\<)[A-Z0-9-._]+@([^.]|\w)[A-Z0-9-._]*[^.]\.[A-Z]{2,4}\>$", RegexOptions.IgnoreCase);
            var re2 = new Regex(@"^[A-Z0-9-._]+@([^.]|\w)[A-Z0-9-._]*[^.]\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);
            var a = email.SplitStr(",;");
            foreach (var m in a)
            {
                var b = re1.IsMatch(m) || re2.IsMatch(m);
                if (b)
                    return true; // at least one good email address
            }
            return false;
        }
        public static bool IsLocalNetworkRequest
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Request.IsLocal)
                        return true;
                    string hostPrefix = HttpContext.Current.Request.UserHostAddress;
                    string[] ipClass = hostPrefix.Split(new char[] { '.' });
                    int classA = Convert.ToInt16(ipClass[0]);
                    int classB = Convert.ToInt16(ipClass[1]);
                    if (classA == 10 || classA == 127)
                        return true;
                    else if (classA == 192 && classB == 168)
                        return true;
                    else if (classA == 172 && (classB > 15 && classB < 33))
                        return true;
                    return false;
                }
                return false;
            }
        }
        public static void IncludeScript(Control head, string url)
        {
            url = head.ResolveUrl(url);
            head.Controls.Add(new LiteralControl("<script type='text/javascript' src='"
                + url + "'></script>"));
        }
        public static void IncludeCss(Control head, string url)
        {
            url = head.ResolveUrl(url);
            head.Controls.Add(new LiteralControl("<link type='text/css' href='"
                + url + "' rel=\"stylesheet\"></script>"));
        }
        public static void NoCache(this HttpResponse Response)
        {
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetValidUntilExpires(false);
        }
        public static void NoCache(this HttpResponseBase Response)
        {
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetValidUntilExpires(false);
        }

        public static string AppRoot
        {
            get
            {
                var approot = Util.ResolveUrl("~");
                if (approot == "/")
                    approot = "";
                return approot;
            }
        }
        public static void ShowError(string message)
        {
            HttpContext.Current.Response.Redirect(
                "/Home/ShowError/?error={0}&url={1}".Fmt(HttpContext.Current.Server.UrlEncode(message),
                HttpContext.Current.Request.Url.OriginalString));
        }
        public static string ResolveUrl(string originalUrl)
        {
            if (originalUrl == null)
                return null;
            if (originalUrl.IndexOf("://") != -1)
                return originalUrl;
            if (originalUrl.StartsWith("~"))
                return VirtualPathUtility.ToAbsolute(originalUrl);
            return originalUrl;
        }
        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;
            var newUrl = ResolveUrl(serverUrl);
            var originalUri = HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                     "://" + originalUri.Authority + newUrl;
            return newUrl;
        }
        public static string ResolveServerUrl(string serverUrl)
        {
            return ResolveServerUrl(serverUrl, false);
        }
        public static string ServerLink(string path)
        {
            var Request = HttpContext.Current.Request;
            return Request.Url.Scheme + "://" + Request.Url.Authority + path;
        }
        public static string PickFirst(params string[] args)
        {
            foreach (var s in args)
                if (s.HasValue())
                    return s;
            return "";
        }
        public static string Disallow(this string value, string dissallow)
        {
            var v = value ?? "";
            value = value.Trim();
            if (value == dissallow)
                return "";
            return value;
        }
        public static string ObscureEmail(string email)
        {
            var a = email.Split('@');
            var rest = new string('x', a[0].Length - 2);
            return a[0].Substring(0, 2) + rest + "@" + a[1];
        }
        public static SmtpClient Smtp()
        {
            var smtp = new SmtpClient();
#if DEBUG
            smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            smtp.PickupDirectoryLocation = "c:/email";
            smtp.Host = "localhost";
#else
            string[] a = (string[])HttpRuntime.Cache["smtpcreds"];
            if (a != null)
                smtp.Credentials = new NetworkCredential(a[0], a[1]);
#endif
            return smtp;
        }
        private const string STR_SysFromEmail = "UnNamed";
        public static string SysFromEmail
        {
            get
            {
                var tag = WebConfigurationManager.AppSettings["sysfromemail"];
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_SysFromEmail] != null)
                            tag = HttpContext.Current.Session[STR_SysFromEmail].ToString();
                return tag;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_SysFromEmail] = value;
            }
        }

        public static string Serialize<T>(T m)
        {
            var ser = new DataContractSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.WriteObject(ms, m);
            var s = Encoding.Default.GetString(ms.ToArray());
            return s;
        }
        public static T DeSerialize<T>(string s)
        {
            var ser = new DataContractSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.Default.GetBytes(s));
            return (T)ser.ReadObject(ms);
        }
        public static string MaxString(this string s, int length)
        {
            if (s != null)
                if (s.Length > length)
                    s = s.Substring(0, length);
            return s;
        }
        public static string RandomPassword(int length)
        {
            var pchars = "ABCDEFGHJKMNPQRSTWXYZ".ToCharArray();
            var pdigits = "23456789".ToCharArray();
            var b = new byte[4];
            (new RNGCryptoServiceProvider()).GetBytes(b);
            var seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];
            var random = new Random(seed);
            var password = new char[length];

            for (int i = 0; i < password.Length; i++)
            {
                var r = i % 4;
                if (r == 1 || r == 2)
                    password[i] = pdigits[random.Next(pdigits.Length - 1)];
                else
                    password[i] = pchars[random.Next(pchars.Length - 1)];
            }
            return new string(password);
        }
        public static MailAddress FirstAddress(string addrs)
        {
            return FirstAddress(addrs, null);
        }
        public static MailAddress FirstAddress(string addrs, string name)
        {
            if (!addrs.HasValue())
                addrs = WebConfigurationManager.AppSettings["senderrorsto"];
            var a = addrs.SplitStr(",;");
            try
            {
                var ma = new MailAddress(a[0]);
                if (name.HasValue())
                    return new MailAddress(ma.Address, name);
                return ma;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void InsertCacheNotRemovable(string key, object value)
        {
                HttpRuntime.Cache.Insert(key, value, null,
                    System.Web.Caching.Cache.NoAbsoluteExpiration,
                    System.Web.Caching.Cache.NoSlidingExpiration,
                    CacheItemPriority.NotRemovable, null);
        }

        public static MailAddress FirstAddress2(string addrs, string name)
        {
            if (!addrs.HasValue())
                addrs = WebConfigurationManager.AppSettings["senderrorsto"];
            var a = addrs.SplitStr(",;");
            try
            {
                return new MailAddress(a[0], name);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string UrgentMessage
        {
            get
            {
                return ((string)HttpContext.Current.Application["getoff"]);
            }
            set
            {
                HttpContext.Current.Application["getoff"] = value;
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

