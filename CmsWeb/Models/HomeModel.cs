using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Web.Mvc;
using System.Text;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data.Linq.SqlClient;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using CmsData.Codes;

namespace CmsWeb.Models
{
    public class HomeModel
    {
        public class BirthdayInfo
        {
            public DateTime Birthday { get; set; }
            public string Name { get; set; }
            public int PeopleId { get; set; }
        }
        public IEnumerable<BirthdayInfo> Birthdays()
        {
            var up = DbUtil.Db.CurrentUserPerson;
            if (up == null)
                return new List<BirthdayInfo>();

            var n = UtilityExtensions.Util.Now;
            var tag = DbUtil.Db.FetchOrCreateTag("TrackBirthdays", up.PeopleId, DbUtil.TagTypeId_Personal);
            var q = tag.People(DbUtil.Db);
            if (q.Count() == 0)
                q = from p in DbUtil.Db.People
                    where p.OrganizationMembers.Any(om => om.OrganizationId == up.BibleFellowshipClassId)
                    select p;

            var q2 = from p in q
                     let nextbd = DbUtil.Db.NextBirthday(p.PeopleId)
                     where SqlMethods.DateDiffDay(UtilityExtensions.Util.Now, nextbd) <= 15
                     where p.DeceasedDate == null
                     orderby nextbd
                     select new BirthdayInfo { Birthday = nextbd.Value, Name = p.Name, PeopleId = p.PeopleId };
            return q2;
        }
        public class MyInvolvementInfo
        {
            public string Name { get; set; }
            public string MemberType { get; set; }
            public int OrgId { get; set; }
        }
        public IEnumerable<MyInvolvementInfo> MyInvolvements()
        {
            var u = DbUtil.Db.CurrentUser;
            if (u == null)
                return new List<MyInvolvementInfo>();

            var pid = u.PeopleId;

            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.PeopleId == pid
                    where (om.Pending ?? false) == false
                    where !(om.Organization.SecurityTypeId == 3 && Util2.OrgMembersOnly)
                    orderby om.Organization.OrganizationName
                    select new MyInvolvementInfo
                    {
                        Name = om.Organization.OrganizationName,
                        MemberType = om.MemberType.Description,
                        OrgId = om.OrganizationId
                    };
            return q;
        }
        public class NewsInfo
        {
            public string Title {get; set;}
            public DateTime Published {get; set;}
            public string Url {get; set;}
        }
        public IEnumerable<NewsInfo> BVCMSNews()
        {
            var feedurl = "http://feeds.feedburner.com/Bvcms-Blog";

            //BlogLink.NavigateUrl = "http://blog.bvcms.com";

            var wr = new WebClient();
            var feed = DbUtil.Db.RssFeeds.FirstOrDefault(r => r.Url == feedurl);

            HttpWebRequest req = null;
            try
            {
                req = WebRequest.Create(feedurl) as HttpWebRequest;
            }
            catch
            {
            }

            if (feed != null)
            {
                if (feed.LastModified.HasValue)
                {
                    req.IfModifiedSince = feed.LastModified.Value;
                    req.Headers.Add("If-None-Match", feed.ETag);
                }
            }
            else
            {
                feed = new RssFeed();
                DbUtil.Db.RssFeeds.InsertOnSubmit(feed);
                feed.Url = feedurl;
            }

            if (req != null)
            {
                try
                {
                    var resp = req.GetResponse() as HttpWebResponse;
                    feed.LastModified = resp.LastModified;
                    feed.ETag = resp.Headers["ETag"];
                    var sr = new StreamReader(resp.GetResponseStream());
                    feed.Data = sr.ReadToEnd();
                    sr.Close();
                    DbUtil.Db.SubmitChanges();
                }
                catch (WebException)
                {
                }
                if (feed.Data != null)
                {
                    var reader = XmlReader.Create(new StringReader(feed.Data));
                    var f = SyndicationFeed.Load(reader);
                    var posts = from item in f.Items
                                select new NewsInfo
                                {
                                    Title = item.Title.Text,
                                    Published = item.PublishDate.DateTime,
                                    Url = item.Links.Single(i => i.RelationshipType == "alternate").GetAbsoluteUri().AbsoluteUri
                                };
                    return posts;
                }
            }
            return null;
        }
        public IEnumerable<NewsInfo> ChurchNews()
        {
            var feedurl = DbUtil.Db.Setting("ChurchFeedUrl", "");

            var feed = DbUtil.Db.RssFeeds.FirstOrDefault(r => r.Url == feedurl);


            HttpWebRequest req = null;
            try
            {
                req = WebRequest.Create(feedurl) as HttpWebRequest;
            }
            catch
            {
            }

            if (feed != null)
            {
                if (feed.LastModified.HasValue)
                {
                    req.IfModifiedSince = feed.LastModified.Value;
                    req.Headers.Add("If-None-Match", feed.ETag);
                }
            }
            else
            {
                feed = new RssFeed();
                DbUtil.Db.RssFeeds.InsertOnSubmit(feed);
                feed.Url = feedurl;
            }

            if (req != null)
            {
                try
                {

                    var resp = req.GetResponse() as HttpWebResponse;
                    feed.LastModified = resp.LastModified;
                    feed.ETag = resp.Headers["ETag"];
                    var sr = new StreamReader(resp.GetResponseStream());
                    feed.Data = sr.ReadToEnd();
                    sr.Close();
                    DbUtil.Db.SubmitChanges();
                }
                catch (WebException)
                {
                }
                if (feed.Data != null)
                {
                    var reader = XmlReader.Create(new StringReader(feed.Data));
                    var f = SyndicationFeed.Load(reader);
                    var posts = from item in f.Items
                                let au = item.Authors.First().Name
                                select new NewsInfo
                                {
                                    Title = item.Title.Text,
                                    Published = item.PublishDate.DateTime,
                                    Url = item.Links.Single(i => i.RelationshipType == "alternate").GetAbsoluteUri().AbsoluteUri
                                };
                    return posts;
                }
            }
            return new NewsInfo[] { };
        }

        public class MySavedQueryInfo
        {
            public string Name { get; set; }
            public int QueryId { get; set; }
        }
        public IEnumerable<MySavedQueryInfo> MyQueries()
        {
            var q = from c in DbUtil.Db.QueryBuilderClauses
                    where c.SavedBy == Util.UserName
                    where c.GroupId == null && c.Field == "Group" && c.Clauses.Count() > 0
                    where !c.Description.Contains("scratchpad")
                    orderby c.Description
                    select new MySavedQueryInfo
                    {
                        Name = c.Description,
                        QueryId = c.QueryId
                    };
            return q;
        }
        public class TaskInfo
        {
            public int TaskId { get; set; }
            public int PeopleId { get; set; }
            public string Who { get; set; }
            public string Description { get; set; }
        }
        public IEnumerable<TaskInfo> Tasks()
        {
            var completedcode = TaskStatusCode.Complete;
            var pid = DbUtil.Db.CurrentUser.PeopleId;
            var q = from t in DbUtil.Db.Tasks
                    where t.Archive == false // not archived
                    where t.OwnerId == pid || t.CoOwnerId == pid
                    where t.WhoId != null && t.StatusId != completedcode
                    where !(t.OwnerId == pid && t.CoOwnerId != null)
                    orderby t.CreatedOn
                    select new TaskInfo
                    {
                        TaskId = t.Id,
                        PeopleId = t.AboutWho.PeopleId,
                        Who = t.AboutWho.Name,
                        Description = t.Description,
                    };
            return q;
        }
        public IEnumerable<CMSPresenter.CodeValueItem> Tags()
        {
            var ctl = new CMSPresenter.CodeValueController();
            var pid = DbUtil.Db.CurrentUser.PeopleId;
            var list = ctl.UserTags(pid);
            return list;
        }
    }
}
