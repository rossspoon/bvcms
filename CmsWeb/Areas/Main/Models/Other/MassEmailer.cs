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

        public string CmsHost { get; set; }
        public string Host { get; set; }
        public Session2 session2 { get; set; }

        public MassEmailer()
        {
        }
        public MassEmailer(int QBId, bool? parents)
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

        public EmailQueue CreateQueue()
        {
            var emailqueue = new EmailQueue
            {
                Queued = DateTime.Now,
                FromAddr = FromAddress,
                FromName = FromName,
                Subject = Subject,
                Body = Body,
                SendWhen = Schedule,
                QueuedBy = Util.UserPeopleId,
            };
            DbUtil.Db.EmailQueues.InsertOnSubmit(emailqueue);
            DbUtil.Db.SubmitChanges();

            var Qb = DbUtil.Db.LoadQueryById(QBId);
            var q = DbUtil.Db.People.Where(Qb.Predicate());
            if (wantParents || Qb.ParentsOf)
                q = from p in q
                    from fm in DbUtil.Db.People.Where(ff => ff.FamilyId == p.FamilyId)
                    where (fm.PositionInFamilyId == 10 && p.PositionInFamilyId != 10)
                    || (fm.PeopleId == p.PeopleId && p.PositionInFamilyId == 10)
                    select fm;

            q = from p in q.Distinct()
                where p.EmailAddress != null && p.EmailAddress != ""
                where !p.EmailOptOuts.Any(oo => oo.FromEmail == emailqueue.FromAddr)
                orderby p.PeopleId
                select p;

            foreach (var p in q)
            {
                if (!Util.ValidEmail(p.EmailAddress))
                    continue;
                var to = new EmailQueueTo { Id = emailqueue.Id, PeopleId = p.PeopleId, OrgId = DbUtil.Db.CurrentOrgId };
                DbUtil.Db.EmailQueueTos.InsertOnSubmit(to);
            }
            DbUtil.Db.SubmitChanges();
            return emailqueue;
        }

        public static void SendEmails(int id, string Host, string CmsHost)
        {
            var Db = new CMSDataContext(Util.GetConnectionString(Host));
            var emailqueue = Db.EmailQueues.Single(eq => eq.Id == id);
            Emailer.SendPeopleEmail(Db, CmsHost, emailqueue);
        }
        public IEnumerable<SelectListItem> EmailFroms()
        {
            return new SelectList(new CodeValueController().UsersToEmailFrom(), "Code", "Value");
        }
    }
}