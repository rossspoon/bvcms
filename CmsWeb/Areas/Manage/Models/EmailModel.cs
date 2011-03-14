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

namespace CmsWeb.Models
{
    public class EmailModel
    {
        public int id { get; set; }
        private EmailQueue _Queue;
        public EmailQueue queue
        {
            get
            {
                if (_Queue == null)
                    _Queue = DbUtil.Db.EmailQueues.SingleOrDefault(ee => ee.Id == id);
                return _Queue;
            }
        }
        public PagerModel2 Pager { get; set; }
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = GetEmailTos().Count();
            return _count.Value;
        }
        public EmailModel()
        {
            Pager = new PagerModel2(Count);
        }
        public IEnumerable<RecipientInfo> Recipients()
        {
            var q = GetEmailTos();
            var q2 = from e in q.OrderBy(ee => ee.Person.Name2)
                         .Skip(Pager.StartRow).Take(Pager.PageSize)
                     select new RecipientInfo
                     {
                         peopleid = e.PeopleId,
                         name = e.Person.Name,
                         address = e.Person.EmailAddress,
                         nopens = e.Person.EmailResponses.Count(er => er.EmailQueueId == e.Id)
                     };
            return q2;
        }
        private IQueryable<EmailQueueTo> GetEmailTos()
        {
            var q = from t in DbUtil.Db.EmailQueueTos
                    where t.Id == id
                    select t;
            return q;
        }
    }
    public class RecipientInfo
    {
        public string name { get; set; }
        public int peopleid { get; set; }
        public string address { get; set; }
        public int nopens { get; set; }
    }
}
