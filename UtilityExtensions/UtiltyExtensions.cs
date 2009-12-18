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
            if (req.QueryString[param].IsNotNull())
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
            if (s != null)
                s = s.Trim();
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
        public static string FormatDateTm(this DateTime dt)
        {
            return dt.ToString("M/d/yy H:mm");
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
        public static DateTime Now2
        {
            get
            {
                var daysoffset = (double?)HttpContext.Current.Application["daysoffset"];
                if(daysoffset.HasValue)
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
                return t.Substring(0,5);
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
            if (!Regex.IsMatch(dt, @"\A(?:\A(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[0-9]{2})\Z"))
                return false;
            var s = dt.Substring(0, 2) + "/" + dt.Substring(2, 2) + "/" + dt.Substring(4, 2);
            if (DateTime.TryParse(s, out dt2))
                return true;
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
                string host = string.Empty;
                if (HttpContext.Current != null)
                    if (HttpContext.Current.Session != null)
                        if (HttpContext.Current.Session[STR_Host] != null)
                            host = HttpContext.Current.Session[STR_Host].ToString();
                if (host.HasValue())
                    return host;
                return HttpContext.Current.Request.Url.Authority;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session[STR_Host] = value;
            }
        }
        public static string Host1
        {
            get
            {
                var a = Host.Split('.');
                return a[0];
            }
        }

        public static string CmsHost
        {
            get
            {
                var h = ConfigurationManager.AppSettings["cmshost"];
                return h.Replace("{church}", Host1);
            }
        }
        public static string ConnectionString
        {
            get
            {
                var cs = ConfigurationManager.ConnectionStrings["CMSHosted"];
                if (cs == null)
                    return ConfigurationManager.ConnectionStrings["CMS"].ConnectionString;

                var cb = new SqlConnectionStringBuilder(cs.ConnectionString);
                cb.InitialCatalog = "CMS_{0}".Fmt(Host1);
                return cb.ConnectionString;
            }
        }
        public static string ConnectionStringImage
        {
            get
            {
                var cs = ConfigurationManager.ConnectionStrings["CMSHosted"];
                if (cs == null)
                    return ConfigurationManager.ConnectionStrings["CMSImage"].ConnectionString;

                var cb = new SqlConnectionStringBuilder(cs.ConnectionString);
                cb.InitialCatalog = "CMS_{0}_img".Fmt(Host1);
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
            s = HttpUtility.HtmlEncode(s);

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
            if (ValidEmail(address))
                return new MailAddress(address, name);
            else
                return null;
        }
        public static bool ValidEmail(string email)
        {
            if (!email.HasValue())
                return false;
            var re = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);
            return re.IsMatch(email);
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

