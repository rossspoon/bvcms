using System;
using System.Data;
using System.Web;
using System.ComponentModel;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;

namespace CmsData
{
    public partial class Verse
    {
        private static string[] BookByNumber
        {
            get
            {
                if (HttpRuntime.Cache["BookByNumber"] == null)
                    InitBible();
                return (string[])HttpRuntime.Cache["BookByNumber"];
            }
        }
        private static string[] AbbrByNumber
        {
            get
            {
                if (HttpRuntime.Cache["AbbrByNumber"] == null)
                    InitBible();
                return (string[])HttpRuntime.Cache["AbbrByNumber"];
            }
        }
        private static Dictionary<string, int> BookByAbbr
        {
            get
            {
                if (HttpRuntime.Cache["BookByAbbr"] == null)
                    InitBible();
                return (Dictionary<string, int>)HttpRuntime.Cache["BookByAbbr"];
            }
        }
        public static Dictionary<string, int> Versions
        {
            get
            {
                if (HttpRuntime.Cache["BookVersions"] == null)
                    InitBible();
                return (Dictionary<string, int>)HttpRuntime.Cache["BookVersions"];
            }
        }
        public static Verse LoadById(int id)
        {
            return DbUtil.Db.Verses.Single(v => v.Id == id);
        }
        public void AddToCategory(int CategoryId)
        {
            var x = VerseCategoryXref.Load(CategoryId, this.Id);
            if (x == null)
            {
                x = new VerseCategoryXref();
                x.VerseCategoryId = CategoryId;
                VerseCategoryXrefs.Add(x);
            }
        }

        private static void InitBible()
        {
            string[] _BookByNumber = new string[66];
            string[] _AbbrByNumber = new string[66];
            Dictionary<string, int> _BookByAbbr = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(Resources.Resource1.books);
            foreach (XmlNode b in xd.DocumentElement.SelectNodes("book"))
            {
                int id = int.Parse(b.Attributes["id"].Value);
                string name = b.Attributes["name"].Value;
                _BookByNumber[id - 1] = name;
                _BookByAbbr[Regex.Replace(name, "\\s", "")] = id;
                foreach (XmlNode n in b.ChildNodes)
                {
                    _BookByAbbr[n.InnerText] = id;
                    string abbr = _AbbrByNumber[id - 1];
                    if (!abbr.HasValue() || n.InnerText.Length > abbr.Length)
                        abbr = n.InnerText;
                    _AbbrByNumber[id - 1] = abbr;
                }
            }
            Dictionary<string, int> _Versions = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (XmlNode v in xd.DocumentElement.SelectNodes("version"))
                _Versions[v.InnerText] = int.Parse(v.Attributes["id"].Value);
            HttpRuntime.Cache.Insert("AbbrByNumber", _AbbrByNumber);
            HttpRuntime.Cache.Insert("BookByNumber", _BookByNumber);
            HttpRuntime.Cache.Insert("BookByAbbr", _BookByAbbr);
            HttpRuntime.Cache.Insert("BookVersions", _Versions);
        }
        public static Verse Parse(string Ref)
        {
            Verse v = new Verse();
            Match m = Regex.Match(Ref,
                @"((?<bk1>\d?)\s)*(?<bk>\S+)\s(?<ch>\d+)(:|\s)(?<vs>\d+)(-(?<vs2>\d+))*");
            string bookdigit = m.Groups["bk1"].Value;
            string bookname = m.Groups["bk"].Value;
            if (BookByAbbr.ContainsKey(bookdigit + bookname))
                v.Book = BookByAbbr[bookdigit + bookname];
            else
                return null;
            bookname = BookByNumber[v.Book.Value - 1];
            v.Chapter = int.Parse(m.Groups["ch"].Value);
            v.VerseNum = int.Parse(m.Groups["vs"].Value);
            if (v.Chapter == 0 || v.VerseNum == 0)
                return null;
            string endverse = m.Groups["vs2"].Value;
            if (endverse.Length > 0) endverse = "-" + endverse;
            v.VerseRef = string.Format("{0} {1}:{2}{3}", bookname, v.Chapter, v.VerseNum, endverse);
            return v;
        }

        public static Verse VerseFromBibleGateway(string Ref, string version)
        {
            WebClient wc = new WebClient();
            const string GatewayUrl = "http://www.biblegateway.com/passage/?search={0}&version={1}";
            Verse v = Parse(Ref);
            v.Version = version;
            if (v == null)
                return null;
            DbUtil.Db.Verses.InsertOnSubmit(v);
            string url = string.Format(GatewayUrl, v.VerseRef, Versions[version]);
            string text;
            if (HttpRuntime.Cache[url] == null)
            {
                wc.Encoding = Encoding.UTF8;
                text = wc.DownloadString(url);
                HttpRuntime.Cache.Insert(url, text, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
            }
            else
                text = (string)HttpRuntime.Cache[url];

            text = Regex.Match(text, "<div\\sclass=\"result-text-style-normal\">(.*?)</div>",
                RegexOptions.Singleline).Groups[1].Value;
            v.VerseText = CleanHtml(text);
            return v;
        }
        public static Verse VerseFromBibleGateway(string Ref)
        {
            return VerseFromBibleGateway(Ref, "NKJV");
        }
        private static string CleanHtml(string text)
        {
            string e = String.Empty;
            string s = Regex.Replace(text, "<span.*?</span>", e);
            s = Regex.Replace(s, "<sup.*?</sup>", e);
            s = Regex.Replace(s, "<h4.*?</h4>", e);
            s = Regex.Replace(s, "<h5.*?</h5>", e);
            s = Regex.Replace(s, "<strong>(Footnotes|Cross).*", e, RegexOptions.Singleline);
            s = Regex.Replace(s, "<br.*?>", " ");
            s = Regex.Replace(s, "<.*?>", e);
            s = Regex.Replace(s, "&nbsp;", " ");
            s = Regex.Replace(s, "\\n", e);
            s = s.Trim();
            s = Regex.Replace(s, "^[\"?]", e);
            s = Regex.Replace(s, "[\"?]$", e);
            s = Regex.Replace(s, "\\s+", " ");
            return s;
        }
        public static Verse Lookup(string Ref, string Version)
        {
            var lookfor = Verse.Parse(Ref);
            if (lookfor == null)
                return null;
            var v = DbUtil.Db.Verses.Where(vv => vv.Version == Version
                && vv.VerseRef == lookfor.VerseRef).SingleOrDefault();
            if (v == null)
                v = VerseFromBibleGateway(Ref, Version);
            return v;
        }
        public string RefAndVersion
        {
            get { return VerseRef + " (" + Version + ")"; }
        }

        //------------------------------------------------------------------------

        public static string DailyReadingUrl(int day, string Version)
        {
            var rp = DbUtil.Db.ReadPlans.Where(readplan => readplan.Day == day).ToArray();
            string Ref = "http://www.biblegateway.com/cgi-bin/bible?version={0}&passage="
                .Fmt(Version);
            for (int s = 0; s < 4; s++)
            {
                string StartBook = AbbrByNumber[rp[s].StartBook.Value];
                string EndBook = AbbrByNumber[rp[s].EndBook.Value];
                if (s > 0)
                    Ref += ";";
                Ref += FormatRef(rp[s]);
            }
            Ref += "&showfn=off&showxref=off&interface=largeprint";
            return Ref;
        }
        public static string FormatRef(ReadPlan rp)
        {
            string Ref;
            string StartBook = AbbrByNumber[rp.StartBook.Value];
            string EndBook = AbbrByNumber[rp.EndBook.Value];
            if (StartBook == EndBook) // starting verse and ending verse are in same book
                if (rp.StartChap == rp.EndChap)
                    Ref = string.Format("{0} {1}:{2}-{3}",
                        StartBook, rp.StartChap, rp.StartVerse, rp.EndVerse);
                else
                    Ref = string.Format("{0} {1}:{2}-{3}:{4}",
                        StartBook, rp.StartChap, rp.StartVerse, rp.EndChap, rp.EndVerse);
            else // have to form two sets of verse ranges, since more than one book is involved
                if (rp.EndChap == 1)
                    Ref = string.Format("{0} {1}:{2}-;{3} 1:1-{4}",
                            StartBook, rp.StartChap, rp.StartVerse,
                            EndBook, rp.EndVerse);
                else
                    Ref = string.Format("{0} {1}:{2}-;{3} 1:1-{4}:{5}",
                            StartBook, rp.StartChap, rp.StartVerse,
                            EndBook, rp.EndChap, rp.EndVerse);
            return Ref;
        }
    }
    [DataObject]
    public class VerseController
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<Verse> GetSortedVerseCollectionFromCategory(int CatId)
        {
            var c = VerseCategory.LoadById(CatId);
            return from cx in c.VerseCategoryXrefs
                   orderby DbUtil.Db.VersePos(cx.VerseId)
                   select cx.Verse;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void RemoveVerseFromCategory(int CatId, int Id)
        {
            VerseCategoryXref.DeleteOnSubmit(CatId, Id);
            DbUtil.Db.SubmitChanges();
        }
    }
}
