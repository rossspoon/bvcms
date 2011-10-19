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
using System.Net.Mail;

namespace CmsWeb.Areas.Main.Models
{
    [Serializable]
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
        public bool PublicViewable { get; set; }

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

            q = from p in q
                where p.EmailAddress != null
                where p.EmailAddress != ""
                where (p.SendEmailAddress1 ?? true) || (p.SendEmailAddress2 ?? false)
                select p;
            Count = q.Count();
        }

        public int CreateQueue()
        {
            var From = new MailAddress(FromAddress, FromName);
            return DbUtil.Db.CreateQueue(From, Subject, Body, Schedule, QBId, wantParents , PublicViewable).Id; 
        }

        public IEnumerable<SelectListItem> EmailFroms()
        {
            return new SelectList(new CodeValueController().UsersToEmailFrom(), "Code", "Value");
        }
    }
}