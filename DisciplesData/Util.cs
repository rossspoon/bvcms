using System;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.ComponentModel;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using DiscData;
using System.Text;
using System.Web.Mvc;

public static class Util
{
    public static string Remember
    {
        get { return "remember3"; }
    }
    public static void IncludeScript(Control head, string url)
    {
        url = head.ResolveUrl(url);
        //page.ClientScript.RegisterClientScriptInclude(Path.GetFileNameWithoutExtension(url), url);
        head.Controls.Add(new LiteralControl("<script type='text/javascript' src='"
            + url + "'></script>"));
    }
    public static void IncludeStartupScript(System.Web.UI.Page page, string script)
    {
        page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "startup", script, true);
    }
    public static string ResolveUrl(string url)
    {
        return System.Web.VirtualPathUtility.ToAbsolute(url);
    }
    public static string Fmt(this string fmt, params object[] p)
    {
        return string.Format(fmt, p);
    }
    public static int ToInt(this object o)
    {
        return Convert.ToInt32(o);
    }
    public static T QueryString<T>(this System.Web.HttpRequest req, string param)
    {
        if (req.QueryString[param].IsNotNull())
            return (T)req.QueryString[param].ChangeType(typeof(T));
        return default(T);
    }
    public static bool IsNotNull(this object o)
    {
        return o != null;
    }
    public static bool HasValue(this string s)
    {
        return !string.IsNullOrEmpty(s);
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
    public static string SafeFormat(string s)
    {
        s = HttpContext.Current.Server.HtmlEncode(s);
        RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase;

        Regex reg = new Regex("(http://([^\\s]*))", options);
        s = reg.Replace(s, "<a target=\"_new\" href=\"$1\">$2</a>");

        s = HtmlFormat(s, "\\*\\*\\*", "u");
        s = HtmlFormat(s, "\\*\\*", "b");
        s = HtmlFormat(s, "\\*", "i");

        reg = new Regex("&quot;(.*?)&quot;\\s*=\\s*&quot;(.*?)&quot;", options);
        s = reg.Replace(s, "<a target=\"_new\" href=\"http://$2\">$1</a>");


        reg = new Regex("&gt;&gt;&gt;(.*?)&lt;&lt;&lt;", options);
        s = reg.Replace(s, "<blockquote>$1</blockquote>");

        s = s.Replace(System.Environment.NewLine, "\n");
        return s.Replace("\n", "<br/>");
    }

    private static string HtmlFormat(string s, string lookfor, string htmlcode)
    {
        RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase;
        Regex reg = new Regex(lookfor + "(.*?)" + lookfor, options);
        return reg.Replace(s, "<" + htmlcode + ">$1</" + htmlcode + ">");
    }

    public static string SafeFormatHelp
    {
        get
        {
            return "*This is italics*, **this is bold** and ***this is underlined***<br>" +
                "This text: \"Goto Bellevue\" = \"www.bellevue.org\" is a hyperlink.";
        }
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
    public static string ConnectionString
    {
        get
        {
            string disc = "Disc";
            if (HttpContext.Current != null)
                if (HttpContext.Current.Session != null)
                    if (HttpContext.Current.Session[disc] != null)
                        disc = HttpContext.Current.Session[disc] as string;
            return ConfigurationManager.ConnectionStrings[disc].ConnectionString;
        }
    }
    private const string STR_User = "CurrentUser";
    public static User CurrentUser
    {
        get
        {
            return HttpContext.Current.Items[STR_User] as User;
        }
        set
        {
            HttpContext.Current.Items[STR_User] = value;
        }
    }
    public static User GetUser(string username)
    {
        return DbUtil.Db.Users.SingleOrDefault(u => u.Username == username);
    }
    public static User GetUser(int? id)
    {
        return DbUtil.Db.Users.SingleOrDefault(u => u.UserId == id);
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
    public static bool AllDigits(this string str)
    {
        Regex patt = new Regex("[^0-9]");
        return !(patt.IsMatch(str));
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
}
