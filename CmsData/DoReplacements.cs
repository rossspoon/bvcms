using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CmsData.Codes;
using HtmlAgilityPack;
using UtilityExtensions;

namespace CmsData
{
    public partial class CMSDataContext
    {
        public List<MailAddress> DoReplacements(ref string text, string CmsHost, Person p, EmailQueueTo emailqueueto)
        {
            if (text == null)
                text = "(no content)";
            if (p.Name.Contains("?") || p.Name.Contains("unknown", true))
                text = text.Replace("{name}", string.Empty);
            else
                text = text.Replace("{name}", p.Name);

            if (p.PreferredName.Contains("?", true) || (p.PreferredName.Contains("unknown", true)))
                text = text.Replace("{first}", string.Empty);
            else
                text = text.Replace("{first}", p.PreferredName);
            text = text.Replace("{occupation}", p.OccupationOther);
            var eurl = Util.URLCombine(CmsHost, "Manage/Emails/View/" + emailqueueto.Id);
            text = text.Replace("{emailhref}", eurl);

            text = DoVoteLinkAnchorStyle(text, CmsHost, emailqueueto);
            text = DoVoteTag(text, CmsHost, emailqueueto);
            text = DoVoteTag2(text, CmsHost, emailqueueto);
            text = DoRegisterTag(text, CmsHost, emailqueueto);
            text = DoRegisterTag2(text, CmsHost, emailqueueto);
            text = DoExtraValueData(text, emailqueueto);
            if (emailqueueto.OrgId.HasValue)
            {
                if (text.Contains("{smallgroup:"))
                    text = DoSmallGroupData(text, emailqueueto);
                if (text.Contains("{addsmallgroup:"))
                    text = DoAddSmallGroup(text, emailqueueto);
                if (text.Contains("{nextmeetingtime}"))
                    text = DoMeetingDate(text, emailqueueto);
            }
            if (text.Contains("{createaccount}"))
                text = text.Replace("{createaccount}", DoCreateUserTag(CmsHost, emailqueueto));
            if (text.Contains("http://votelink", ignoreCase: true))
                text = DoVoteLink(text, CmsHost, emailqueueto);
            if (text.Contains("http://registerlink", ignoreCase: true))
                text = DoRegisterLink(text, CmsHost, emailqueueto);
            if (text.Contains("http://rsvplink", ignoreCase: true))
                text = DoRsvpLink(text, CmsHost, emailqueueto);
            if (text.Contains("http://volsublink", ignoreCase: true))
                text = DoVolSubLink(text, CmsHost, emailqueueto);
            if (text.Contains("http://volreqlink", ignoreCase: true))
                text = DoVolReqLink(text, CmsHost, emailqueueto);
            if (text.Contains("{barcode}", ignoreCase: true))
            {
                var link = Util.URLCombine(CmsHost, "/Track/Barcode/" + emailqueueto.PeopleId);
                text = text.Replace("{barcode}", "<img src='" + link + "' />");
            }
            if (text.Contains("{cellphone}"))
                if (p.CellPhone.HasValue())
                    text = text.Replace("{cellphone}", p.CellPhone.FmtFone());
                else
                    text = text.Replace("{cellphone}", "no cellphone on record");

            if (text.Contains("{campus}", ignoreCase: true))
                if (p.CampusId != null)
                    text = text.Replace("{campus}", p.Campu.Description);
                else
                    text = text.Replace("{campus}", "No Campus Specified");

            if (emailqueueto.Guid.HasValue)
            {
                var turl = Util.URLCombine(CmsHost, "/Track/Key/" + emailqueueto.Guid.Value.GuidToQuerystring());
                text = text.Replace("{track}", "<img src=\"{0}\" />".Fmt(turl));
            }

            var aa = GetAddressList(p);

            if (emailqueueto.AddEmail.HasValue())
                foreach (var ad in emailqueueto.AddEmail.SplitStr(","))
                    Util.AddGoodAddress(aa, ad);

            if (emailqueueto.OrgId.HasValue)
            {
                var qm = (from m in OrganizationMembers
                          where m.PeopleId == emailqueueto.PeopleId && m.OrganizationId == emailqueueto.OrgId
                          select new { m.PayLink, m.Amount, m.AmountPaid, m.RegisterEmail }).SingleOrDefault();
                if (qm != null)
                {
                    if (qm.PayLink.HasValue())
                        text = text.Replace("{paylink}", "<a href=\"{0}\">payment link</a>".Fmt(qm.PayLink));
                    text = text.Replace("{amtdue}", (qm.Amount - qm.AmountPaid).ToString2("c"));
                    Util.AddGoodAddress(aa, Util.FullEmail(qm.RegisterEmail, p.Name));
                }
            }
            return aa.DistinctEmails();
        }
        private string DoMeetingDate(string text, EmailQueueTo emailqueueto)
        {
            var mt = (from aa in Attends
                      where aa.OrganizationId == emailqueueto.OrgId
                      where aa.PeopleId == emailqueueto.PeopleId
                      where aa.Commitment == AttendCommitmentCode.Attending
                      where aa.MeetingDate > DateTime.Now
                      orderby aa.MeetingDate
                      select aa.MeetingDate).FirstOrDefault();
            text = text.Replace("{nextmeetingtime}", mt.ToString("g"));
            return text;
        }
        private string DoSmallGroupData(string text, EmailQueueTo emailqueueto)
        {
            const string RE = @"\{smallgroup:\[(?<prefix>[^\]]*)\](?:,(?<def>[^}]*)){0,1}\}";
            var re = new Regex(RE, RegexOptions.Singleline);
            var match = re.Match(text);
            while (match.Success && emailqueueto.OrgId.HasValue)
            {
                var tag = match.Value;
                var prefix = match.Groups["prefix"].Value;
                var def = match.Groups["def"].Value;
                var sg = (from mm in OrgMemMemTags
                          where mm.OrgId == emailqueueto.OrgId
                          where mm.PeopleId == emailqueueto.PeopleId
                          where mm.MemberTag.Name.StartsWith(prefix)
                          select mm.MemberTag.Name).FirstOrDefault();
                if (!sg.HasValue())
                    sg = def;
                text = text.Replace(tag, sg);
                match = match.NextMatch();
            }
            return text;
        }
        private string DoAddSmallGroup(string text, EmailQueueTo emailqueueto)
        {
            const string RE = @"\{addsmallgroup:\[(?<group>[^\]]*)\]\}";
            var re = new Regex(RE, RegexOptions.Singleline);
            var match = re.Match(text);
            if (match.Success && emailqueueto.OrgId.HasValue)
            {
                var tag = match.Value;
                var group = match.Groups["group"].Value;
                var om = (from mm in OrganizationMembers
                          where mm.OrganizationId == emailqueueto.OrgId
                          where mm.PeopleId == emailqueueto.PeopleId
                          select mm).SingleOrDefault();
                if (om != null)
                    om.AddToGroup(this, group);
                text = text.Replace(tag, "");
            }
            return text;
        }
        private string DoExtraValueData(string text, EmailQueueTo emailqueueto)
        {
            const string RE = @"{extra(?<type>.*?):(?<field>.*?)}";
            var re = new Regex(RE, RegexOptions.Singleline);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;
                var field = match.Groups["field"].Value;
                var type = match.Groups["type"].Value;
                var ev = PeopleExtras.SingleOrDefault(ee => ee.Field == field && emailqueueto.PeopleId == ee.PeopleId);
                string value = "";
                switch (type)
                {
                    case "value":
                        value = ev.StrValue;
                        break;
                    case "data":
                        value = ev.Data;
                        break;
                    case "date":
                        value = ev.DateValue.FormatDate();
                        break;
                    case "int":
                        value = ev.IntValue.ToString();
                        break;
                }
                text = text.Replace(tag, value);
                match = match.NextMatch();
            }
            return text;
        }
        private string DoVoteLinkAnchorStyle(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            var list = new Dictionary<string, OneTimeLink>();
            const string VoteLinkRE = @"{votelink(?<inside>[^}]*)}";
            var re = new Regex(VoteLinkRE, RegexOptions.Singleline | RegexOptions.Multiline);
            var match = re.Match(text);
            while (match.Success)
            {
                var votelink = match.Value;
                var anchor = "<a " + match.Groups["inside"].Value + ">text</a>";
                anchor = anchor.Replace("&quot;", "\"");
                anchor = anchor.Replace("&rdquo;", "\"");
                anchor = anchor.Replace("&ldquo;", "\"");

                var doc = new HtmlDocument();
                doc.LoadHtml(anchor);
                var ele = doc.DocumentNode.Element("a");
                var d = ele.Attributes.ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);
                var txt = "click here";
                if (d.ContainsKey("text"))
                    txt = d["text"];

                string msg = "Thank you for responding.";
                if (d.ContainsKey("message"))
                    msg = d["message"];

                string confirm = "false";
                if (d.ContainsKey("confirm"))
                    confirm = d["confirm"];

                if (!d.ContainsKey("smallgroup"))
                    throw new Exception("Votelink: no smallgroup attribute");
                var smallgroup = d["smallgroup"];
                var pre = "";
                var a = smallgroup.SplitStr(":");
                if (a.Length > 1)
                    pre = a[0];

                if (!d.ContainsKey("id"))
                    throw new Exception("Votelink: no id attribute");
                var id = d["id"];

                var url = VoteLinkUrl(text, CmsHost, emailqueueto, list, votelink, id, msg, confirm, smallgroup, pre);
                text = text.Replace(votelink, @"<a href=""{0}"">{1}</a>".Fmt(url, txt));

                match = match.NextMatch();
            }
            return text;
        }//&lt;votetag .*?&gt;(?<inside>.+?)&lt;/votetag&gt;
        private string DoVoteTag2(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            var list = new Dictionary<string, OneTimeLink>();
            const string VoteLinkRE = @"&lt;votetag .*?&gt;(?<inside>.+?)&lt;/votetag&gt;";
            var re = new Regex(VoteLinkRE, RegexOptions.Singleline | RegexOptions.Multiline);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;
                var inside = HttpUtility.HtmlDecode(match.Groups["inside"].Value);

                var doc = new HtmlDocument();
                doc.LoadHtml(tag);
                var ele = doc.DocumentNode.Element("votetag");
                var d = ele.Attributes.ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);

                string msg = "Thank you for responding.";
                if (d.ContainsKey("message"))
                    msg = d["message"];

                string confirm = "false";
                if (d.ContainsKey("confirm"))
                    confirm = d["confirm"];

                if (!d.ContainsKey("smallgroup"))
                    throw new Exception("Votelink: no smallgroup attribute");
                var smallgroup = d["smallgroup"];
                var pre = "";
                var a = smallgroup.SplitStr(":");
                if (a.Length > 1)
                    pre = a[0];

                if (!d.ContainsKey("id"))
                    throw new Exception("Votelink: no id attribute");
                var id = d["id"];

                var url = VoteLinkUrl(text, CmsHost, emailqueueto, list, tag, id, msg, confirm, smallgroup, pre);
                text = text.Replace(tag, @"<a href=""{0}"">{1}</a>".Fmt(url, inside));
                match = match.NextMatch();
            }
            return text;
        }
        private string DoCreateUserTag(string CmsHost, EmailQueueTo emailqueueto)
        {
            var user = (from u in Users
                        where u.PeopleId == emailqueueto.PeopleId
                        select u).FirstOrDefault();
            if (user != null)
            {
                user.ResetPasswordCode = Guid.NewGuid();
                user.ResetPasswordExpires = DateTime.Now.AddHours(Setting("ResetPasswordExpiresHours", "24").ToInt());
                var link = Util.URLCombine(CmsHost, "/Account/SetPassword/" + user.ResetPasswordCode.ToString());
                SubmitChanges();
                return @"<a href=""{0}"">Set password for {1}</a>".Fmt(link, user.Username);
            }
            var ot = new OneTimeLink
            {
                Id = Guid.NewGuid(),
                Querystring = emailqueueto.PeopleId.ToString()
            };
            OneTimeLinks.InsertOnSubmit(ot);
            SubmitChanges();
            var url = Util.URLCombine(CmsHost, "/Account/CreateAccount/{0}".Fmt(ot.Id.ToCode()));
            return @"<a href=""{0}"">Create Account</a>".Fmt(url);
        }
        private string DoVoteLink(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            //<a dir="ltr" href="http://votelink" id="798" rel="smallgroup" title="This is a message">test</a>
            var list = new Dictionary<string, OneTimeLink>();
            const string VoteLinkRE = "<a[^>]*?href=\"https{0,1}://votelink/{0,1}\"[^>]*>.*?</a>";
            var re = new Regex(VoteLinkRE, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;

                var doc = new HtmlDocument();
                doc.LoadHtml(tag);
                var ele = doc.DocumentNode.Element("a");
                var inside = ele.InnerHtml;
                var d = ele.Attributes.ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);

                string msg = "Thank you for responding.";
                if (d.ContainsKey("title"))
                    msg = d["title"];

                string confirm = "false";
                if (d.ContainsKey("dir") && d["dir"] == "ltr")
                    confirm = "true";

                if (!d.ContainsKey("rel"))
                    throw new Exception("Votelink: no smallgroup attribute");
                var smallgroup = d["rel"];
                var pre = "";
                var a = smallgroup.SplitStr(":");
                if (a.Length > 1)
                    pre = a[0];

                if (!d.ContainsKey("id"))
                    throw new Exception("Votelink: no id attribute");
                var id = d["id"];

                var url = VoteLinkUrl(text, CmsHost, emailqueueto, list, tag, id, msg, confirm, smallgroup, pre);
                text = text.Replace(tag, @"<a href=""{0}"">{1}</a>".Fmt(url, inside));
                match = match.NextMatch();
            }
            return text;
        }
        private string DoRsvpLink(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            //<a dir="ltr" href="http://rsvplink" id="798" rel="meetingid" title="This is a message">test</a>
            var list = new Dictionary<string, OneTimeLink>();
            const string RsvpLinkRE = "<a[^>]*?href=\"https{0,1}://rsvplink/{0,1}\"[^>]*>.*?</a>";
            var re = new Regex(RsvpLinkRE, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;

                var doc = new HtmlDocument();
                doc.LoadHtml(tag);
                var ele = doc.DocumentNode.Element("a");
                var inside = ele.InnerHtml;
                var d = ele.Attributes.ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);

                string msg = "Thank you for responding.";
                if (d.ContainsKey("title"))
                    msg = d["title"];

                string confirm = "false";
                if (d.ContainsKey("dir") && d["dir"] == "ltr")
                    confirm = "true";

                string smallgroup = null;
                if (d.ContainsKey("rel"))
                    smallgroup = d["rel"];

                if (!d.ContainsKey("id"))
                    throw new Exception("Rsvplink: no id attribute");
                var id = d["id"];

                var url = RsvpLinkUrl(CmsHost, emailqueueto, list, id, smallgroup, msg, confirm);
                text = text.Replace(tag, @"<a href=""{0}"">{1}</a>".Fmt(url, inside));
                match = match.NextMatch();
            }
            return text;
        }
        private string DoRegisterLink(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            var list = new Dictionary<string, OneTimeLink>();
            const string VoteLinkRE = "<a[^>]*?href=\"https{0,1}://(?<rlink>registerlink2{0,1})/{0,1}\"[^>]*>.*?</a>";
            var re = new Regex(VoteLinkRE, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;
                var rlink = match.Groups["rlink"].Value.ToLower();

                var doc = new HtmlDocument();
                doc.LoadHtml(tag);
                var ele = doc.DocumentNode.Element("a");
                var inside = ele.InnerHtml;
                var d = ele.Attributes.ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);

                if (!d.ContainsKey("id"))
                    throw new Exception("RegisterTag: no id attribute");
                var id = d["id"];

                var url = RegisterTagUrl(text, CmsHost, emailqueueto, list, tag, id,
                    showfamily: rlink == "registerlink2");
                text = text.Replace(tag, @"<a href=""{0}"">{1}</a>".Fmt(url, inside));
                match = match.NextMatch();
            }
            return text;
        }
        public string DoVolSubLink(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            var list = new Dictionary<string, OneTimeLink>();
            const string VolSubLinkRE = "<a[^>]*?href=\"https{0,1}://volsublink\"[^>]*>.*?</a>";
            var re = new Regex(VolSubLinkRE, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;

                var doc = new HtmlDocument();
                doc.LoadHtml(tag);
                var ele = doc.DocumentNode.Element("a");
                var inside = ele.InnerHtml;
                var d = ele.Attributes.ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);

                var qs = "{0},{1},{2},{3}"
                    .Fmt(d["aid"], d["pid"], d["ticks"], emailqueueto.PeopleId);
                OneTimeLink ot = null;
                if (list.ContainsKey(qs))
                    ot = list[qs];
                else
                {
                    ot = new OneTimeLink
                    {
                        Id = Guid.NewGuid(),
                        Querystring = qs
                    };
                    OneTimeLinks.InsertOnSubmit(ot);
                    SubmitChanges();
                    list.Add(qs, ot);
                }

                var url = Util.URLCombine(CmsHost, "/OnlineReg/ClaimVolSub/{0}/{1}".Fmt(d["ans"], ot.Id.ToCode()));
                text = text.Replace(tag, @"<a href=""{0}"">{1}</a>".Fmt(url, inside));
                match = match.NextMatch();
            }
            return text;
        }
        public string DoVolReqLink(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            var list = new Dictionary<string, OneTimeLink>();
            const string VolSubLinkRE = "<a[^>]*?href=\"https{0,1}://volreqlink\"[^>]*>.*?</a>";
            var re = new Regex(VolSubLinkRE, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;

                var doc = new HtmlDocument();
                doc.LoadHtml(tag);
                var ele = doc.DocumentNode.Element("a");
                var inside = ele.InnerHtml;
                var d = ele.Attributes.ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);

                var qs = "{0},{1},{2},{3}"
                    .Fmt(d["mid"], d["pid"], d["ticks"], emailqueueto.PeopleId);
                OneTimeLink ot = null;
                if (list.ContainsKey(qs))
                    ot = list[qs];
                else
                {
                    ot = new OneTimeLink
                    {
                        Id = Guid.NewGuid(),
                        Querystring = qs
                    };
                    OneTimeLinks.InsertOnSubmit(ot);
                    SubmitChanges();
                    list.Add(qs, ot);
                }

                var url = Util.URLCombine(CmsHost, "/OnlineReg/RequestResponse?ans={0}&guid={1}".Fmt(d["ans"], ot.Id.ToCode()));
                text = text.Replace(tag, @"<a href=""{0}"">{1}</a>".Fmt(url, inside));
                match = match.NextMatch();
            }
            return text;
        }

        private string DoVoteTag(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            var list = new Dictionary<string, OneTimeLink>();
            const string VoteLinkRE = @"<votetag[^>]*>(?<inside>.+?)</votetag>";
            var re = new Regex(VoteLinkRE, RegexOptions.Singleline | RegexOptions.Multiline);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;
                var inside = match.Groups["inside"].Value;

                var doc = new HtmlDocument();
                doc.LoadHtml(tag);
                var ele = doc.DocumentNode.Element("votetag");
                var d = ele.Attributes.ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);

                string msg = "Thank you for responding.";
                if (d.ContainsKey("message"))
                    msg = d["message"];

                string confirm = "false";
                if (d.ContainsKey("confirm"))
                    confirm = d["confirm"];

                if (!d.ContainsKey("smallgroup"))
                    throw new Exception("Votelink: no smallgroup attribute");
                var smallgroup = d["smallgroup"];
                var pre = "";
                var a = smallgroup.SplitStr(":");
                if (a.Length > 1)
                    pre = a[0];

                if (!d.ContainsKey("id"))
                    throw new Exception("Votelink: no id attribute");
                var id = d["id"];

                var url = VoteLinkUrl(text, CmsHost, emailqueueto, list, tag, id, msg, confirm, smallgroup, pre);
                text = text.Replace(tag, @"<a href=""{0}"">{1}</a>".Fmt(url, inside));
                match = match.NextMatch();
            }
            return text;
        }
        private string DoRegisterTag2(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            var list = new Dictionary<string, OneTimeLink>();
            const string VoteLinkRE = @"&lt;registertag .*?&gt;(?<inside>.+?)&lt;/registertag&gt;";
            var re = new Regex(VoteLinkRE, RegexOptions.Singleline | RegexOptions.Multiline);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;
                var inside = HttpUtility.HtmlDecode(match.Groups["inside"].Value);

                var doc = new HtmlDocument();
                doc.LoadHtml(tag);
                var ele = doc.DocumentNode.Element("registertag");
                var d = ele.Attributes.ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);

                if (!d.ContainsKey("id"))
                    throw new Exception("RegisterTag: no id attribute");
                var id = d["id"];

                var url = RegisterTagUrl(text, CmsHost, emailqueueto, list, tag, id);
                text = text.Replace(tag, @"<a href=""{0}"">{1}</a>".Fmt(url, inside));
                match = match.NextMatch();
            }
            return text;
        }
        private string DoRegisterTag(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            var list = new Dictionary<string, OneTimeLink>();
            const string VoteLinkRE = @"<registertag[^>]*>(?<inside>.+?)</registertag>";
            var re = new Regex(VoteLinkRE, RegexOptions.Singleline | RegexOptions.Multiline);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;
                var inside = match.Groups["inside"].Value;

                var doc = new HtmlDocument();
                doc.LoadHtml(tag);
                var ele = doc.DocumentNode.Element("registertag");
                var d = ele.Attributes.ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);

                if (!d.ContainsKey("id"))
                    throw new Exception("RegisterTag: no id attribute");
                var id = d["id"];

                var url = RegisterTagUrl(text, CmsHost, emailqueueto, list, tag, id);
                text = text.Replace(tag, @"<a href=""{0}"">{1}</a>".Fmt(url, inside));
                match = match.NextMatch();
            }
            return text;
        }
        private string RsvpLinkUrl(
            string CmsHost,
            EmailQueueTo emailqueueto,
            Dictionary<string, OneTimeLink> list,
            string id,
            string smallgroup,
            string msg,
            string confirm)
        {
            var qs = "{0},{1},{2},{3}".Fmt(id, emailqueueto.PeopleId, emailqueueto.Id, smallgroup);
            OneTimeLink ot;
            if (list.ContainsKey(qs))
                ot = list[qs];
            else
            {
                ot = new OneTimeLink
                {
                    Id = Guid.NewGuid(),
                    Querystring = qs
                };
                OneTimeLinks.InsertOnSubmit(ot);
                SubmitChanges();
                list.Add(qs, ot);
            }
            var url = Util.URLCombine(CmsHost, "/OnlineReg/RsvpLinkSg/{0}?confirm={1}&message={2}"
                .Fmt(ot.Id.ToCode(), confirm, HttpUtility.UrlEncode(msg)));
            return url;
        }
        private string VoteLinkUrl(string text,
            string CmsHost,
            EmailQueueTo emailqueueto,
            Dictionary<string, OneTimeLink> list,
            string votelink,
            string id,
            string msg,
            string confirm,
            string smallgroup,
            string pre)
        {
            var qs = "{0},{1},{2},{3},{4}".Fmt(id, emailqueueto.PeopleId, emailqueueto.Id, pre, smallgroup);
            OneTimeLink ot;
            if (list.ContainsKey(qs))
                ot = list[qs];
            else
            {
                ot = new OneTimeLink
                {
                    Id = Guid.NewGuid(),
                    Querystring = qs
                };
                OneTimeLinks.InsertOnSubmit(ot);
                SubmitChanges();
                list.Add(qs, ot);
            }
            var url = Util.URLCombine(CmsHost, "/OnlineReg/VoteLinkSg/{0}?confirm={1}&message={2}"
                .Fmt(ot.Id.ToCode(), confirm, HttpUtility.UrlEncode(msg)));
            return url;
        }
        private string RegisterTagUrl(string text,
            string CmsHost,
            EmailQueueTo emailqueueto,
            Dictionary<string, OneTimeLink> list,
            string votelink,
            string id,
            bool showfamily = false)
        {
            var qs = "{0},{1},{2}".Fmt(id, emailqueueto.PeopleId, emailqueueto.Id);
            OneTimeLink ot;
            if (list.ContainsKey(qs))
                ot = list[qs];
            else
            {
                ot = new OneTimeLink
                {
                    Id = Guid.NewGuid(),
                    Querystring = qs
                };
                OneTimeLinks.InsertOnSubmit(ot);
                SubmitChanges();
                list.Add(qs, ot);
            }
            var url = Util.URLCombine(CmsHost, "/OnlineReg/RegisterLink/{0}".Fmt(ot.Id.ToCode()));
            if (showfamily)
                url += "?showfamily=true";
            return url;
        }
    }
}
