using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data.Linq;
using CmsData;
using UtilityExtensions;
using System.Web.Mvc;
using System.Text;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

namespace CmsWeb.Models
{
    public class EmailsModel
    {
        public int? peopleid { get; set; }
        public string name
        {
            get
            {
                var nam = (from p in DbUtil.Db.People
                          where p.PeopleId == peopleid
                          select p.Name).SingleOrDefault();
                return nam;
            }
        }
        public int? senderid { get; set; }
        public string sender
        {
            get
            {
                var nam = (from p in DbUtil.Db.People
                          where p.PeopleId == senderid
                          select p.Name).SingleOrDefault();
                return nam;
            }
        }
        public string subject { get; set; }
        public string body { get; set; }
        public string from { get; set; }
        public DateTime? startdt { get; set; }
        public DateTime? enddt { get; set; }
        public bool transactional { get; set; }
        public PagerModel2 Pager { get; set; }
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = FetchEmails().Count();
            return _count.Value;
        }
        public EmailsModel()
        {
            Pager = new PagerModel2(Count);
            Pager.Sort = "Sent/Scheduled";
            Pager.Direction = "desc";
        }
        public IEnumerable<EmailQueueInfo> Emails()
        {
            var q = ApplySort();
            q = q.Skip(Pager.StartRow).Take(Pager.PageSize);
            var q2 = from e in q
                     select new EmailQueueInfo
                     {
                         queue = e,
                         count = e.EmailQueueTos.Count(),
                         nopens = e.EmailResponses.Count(),
                         nuopens = e.EmailResponses.Select(er => er.PeopleId).Distinct().Count()
                     };
            return q2;
        }

        private IQueryable<EmailQueue> _emails;
        private IQueryable<EmailQueue> FetchEmails()
        {
            if (_emails != null)
                return _emails;
            _emails
               = from t in DbUtil.Db.EmailQueues
                 where t.Sent >= startdt || startdt == null
                 where subject == null || t.Subject.Contains(subject)
                 where body == null || t.Body.Contains(body)
                 where @from == null || t.FromName.Contains(@from) || t.FromAddr.Contains(@from)
                 where peopleid == null || t.EmailQueueTos.Any(et => et.PeopleId == peopleid)
                 where senderid == null || t.QueuedBy == senderid
                 where (t.Transactional ?? false) == transactional
                 select t;
            var edt = enddt;
            if (!edt.HasValue && startdt.HasValue)
                 edt = startdt.Value.AddHours(24);
            if (edt.HasValue)
                _emails = _emails.Where(t => t.Sent < edt);
            if (!HttpContext.Current.User.IsInRole("Admin")
                && !HttpContext.Current.User.IsInRole("ManageEmails"))
            {
                var u = DbUtil.Db.LoadPersonById(Util.UserPeopleId.Value);
                _emails = from t in _emails
                          where t.FromAddr == u.EmailAddress
                          || t.EmailQueueTos.Any(et => et.PeopleId == u.PeopleId)
                          select t;
            }
            return _emails;
        }
        public IQueryable<EmailQueue> ApplySort()
        {
            var q = FetchEmails();
            if (Pager.Direction == "asc")
                switch (Pager.Sort)
                {
                    case "Sent/Scheduled":
                        q = from t in q
                            orderby (t.SendWhen ?? t.Sent) ?? t.Queued
                            select t;
                        break;
                    case "From":
                        q = from t in q
                            orderby t.FromAddr, t.Sent
                            select t;
                        break;
                    case "Name":
                        q = from t in q
                            orderby t.FromName, t.Sent
                            select t;
                        break;
                    case "Subject":
                        q = from t in q
                            orderby t.Subject, t.Sent
                            select t;
                        break;
                    case "Count":
                        q = from t in q
                            orderby t.EmailQueueTos.Count()
                            select t;
                        break;
                }
            else
                switch (Pager.Sort)
                {
                    case "Sent/Scheduled":
                        q = from t in q
                            orderby ((t.SendWhen ?? t.Sent) ?? t.Queued) descending
                            select t;
                        break;
                    case "From":
                        q = from t in q
                            orderby t.FromAddr descending, t.Sent descending
                            select t;
                        break;
                    case "Name":
                        q = from t in q
                            orderby t.FromName, t.Sent descending
                            select t;
                        break;
                    case "Subject":
                        q = from t in q
                            orderby t.Subject, t.Sent descending
                            select t;
                        break;
                    case "Count":
                        q = from t in q
                            orderby t.EmailQueueTos.Count() descending
                            select t;
                        break;
                }
            return q;
        }
    }
    public class EmailQueueInfo
    {
        public EmailQueue queue { get; set; }
        public int count { get; set; }
        public int nopens { get; set; }
        public int nuopens { get; set; }
    }
}
