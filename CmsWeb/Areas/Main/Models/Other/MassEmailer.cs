using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using CmsData;
using CMSPresenter;
using UtilityExtensions;
using System.IO;
using System.Web.Mvc;

namespace CmsWeb.Areas.Main.Models
{
    public class MassEmailer
    {
        public int Count { get; set; }

        public int QBId { get; set; }
        public bool wantParents { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? Schedule { get; set; }

        public int QueueId { get; set; }
        public string CmsHost { get; set; }
        public Session2 session2 { get; set; }

        public MassEmailer()
        {
            CmsHost = Util.CmsHost;
        }

        public MassEmailer(int QBId, bool? parents)
            : this()
        {
            this.QBId = QBId;
            this.wantParents = parents ?? false;
            var Qb = DbUtil.Db.LoadQueryById(QBId);
            var q = DbUtil.Db.PeopleQuery(QBId);
            if (Qb.ParentsOf || wantParents)
            {
                q = from p in q
                    from fm in DbUtil.Db.People.Where(ff => ff.FamilyId == p.FamilyId)
                    where (fm.PositionInFamilyId == 10 && p.PositionInFamilyId != 10)
                    || (fm.PeopleId == p.PeopleId && p.PositionInFamilyId == 10)
                    select fm;
                q = q.Distinct();
            }

            q = q.Where(p => p.EmailAddress != null && p.EmailAddress != "");
            Count = q.Count();
        }

        public int Queue()
        {
            DbUtil.Db.CopySession();
            session2 = DbUtil.Db.ExportSession();
            var emailqueue = new EmailQueue
            {
                Queued = DateTime.Now,
                FromAddr = FromAddress,
                FromName = FromName,
                Subject = Subject,
                Body = Body,
                SendWhen = Schedule,
            };
            DbUtil.Db.EmailQueues.InsertOnSubmit(emailqueue);
            DbUtil.Db.SubmitChanges();
            QueueId = emailqueue.Id;

            var t = new Thread(new ParameterizedThreadStart(QueueEmails));
            t.Start(this);
            Thread.Sleep(1000);
            return emailqueue.Id;
        }
        public void TestSend(int id)
        {
            var q = DbUtil.Db.People.Where(pp => pp.PeopleId == id);
            var em = new Emailer(FromAddress, FromName);
            em.SendPeopleEmail(DbUtil.Db, Util.CmsHost, q, Subject, Body);
        }
        public void Send(int id)
        {
            QueueId = id;
            DbUtil.Db.CopySession();
            session2 = DbUtil.Db.ExportSession();

            var t = new Thread(new ParameterizedThreadStart(SendEmails));
            t.Start(this);
            Thread.Sleep(1000);
        }

        public static void QueueEmails(object stateInfo)
        {
            var m = stateInfo as MassEmailer;
            var Db = new CMSDataContext(Util.GetConnectionString(m.session2.Host));
            Db.ImportSession(m.session2);
            Db.CommandTimeout = 1200;

            var Qb = Db.LoadQueryById(m.QBId);
            Db.SetNoLock();
            var q = Db.People.Where(Qb.Predicate());
            if (m.wantParents || Qb.ParentsOf)
                q = from p in q
                    from fm in DbUtil.Db.People.Where(ff => ff.FamilyId == p.FamilyId)
                    where (fm.PositionInFamilyId == 10 && p.PositionInFamilyId != 10)
                    || (fm.PeopleId == p.PeopleId && p.PositionInFamilyId == 10)
                    select fm;

            q = from p in q.Distinct()
                where p.EmailAddress != null && p.EmailAddress != ""
                where !p.EmailOptOuts.Any(oo => oo.FromEmail == m.FromAddress)
                orderby p.PeopleId
                select p;

            var emailqueue = Db.EmailQueues.Single(eq => eq.Id == m.QueueId);
            foreach (var p in q)
            {
                if (!Util.ValidEmail(p.EmailAddress))
                    continue;
                var to = new EmailQueueTo { Id = emailqueue.Id, PeopleId = p.PeopleId, OrgId = Db.CurrentOrgId };
                Db.EmailQueueTos.InsertOnSubmit(to);
            }
            Db.SubmitChanges();
            if (!emailqueue.SendWhen.HasValue)
                Emailer.SendPeopleEmail(Db, m.CmsHost, emailqueue);
        }
        public static void SendEmails(object stateInfo)
        {
            var m = stateInfo as MassEmailer;
            var Db = new CMSDataContext(Util.GetConnectionString(m.session2.Host));
            Db.ImportSession(m.session2);
            Db.CommandTimeout = 1200;

            var emailqueue = Db.EmailQueues.Single(eq => eq.Id == m.QueueId);
            Emailer.SendPeopleEmail(Db, m.CmsHost, emailqueue);
        }
        public IEnumerable<SelectListItem> EmailFroms()
        {
            return new SelectList(new CodeValueController().UsersToEmailFrom(), "Code", "Value");
        }
    }
}