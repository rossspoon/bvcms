using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CmsData;
using UtilityExtensions;
using System.Diagnostics;

namespace CMSWeb.Admin
{
    public partial class OneTimeETStudy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var start = Util.Now;
            DbUtil.Db.ExecuteCommand("truncate table dbo.BadET");
            var q = from t in DbUtil.Db.EnrollmentTransactions
                    group t by new { t.PeopleId, t.OrganizationId } into g
                    orderby g.Key.PeopleId
                    let om = DbUtil.Db.OrganizationMembers.SingleOrDefault(m =>
                        g.Key.OrganizationId == m.OrganizationId && g.Key.PeopleId == m.PeopleId)
                    let tlast = g.OrderByDescending(et => et.TransactionDate).First()
                    select new { memb = om, trans = g, tlast = tlast };
            foreach (var g in q)
            {
                var list = new List<BadET>();
                var q2 = from t in g.trans
                         orderby t.TransactionDate
                         select t;
                EnrollmentTransaction pt = null;
                foreach (var t in q2)
                {
                    if (pt != null && pt.TransactionDate == t.TransactionDate)
                    {
                        AddBadET(list, pt, 15);
                        AddBadET(list, t, 15);
                    }
                    if (pt != null)
                    {
                        if (t.TransactionTypeId < 3 && pt.TransactionTypeId < 3)
                            AddBadET(list, t, 11);
                        if (t.TransactionTypeId > 3 && pt.TransactionTypeId > 3)
                            AddBadET(list, t, 55);
                    }
                    pt = t;
                }
                if (g.memb == null && g.tlast != null && g.tlast.TransactionTypeId < 3)
                    AddBadET(list, g.tlast, 10);
                WriteBadEts(list);
            }
            var time = Util.Now.Subtract(start);
            Label1.Text = time.ToString();
        }
        void AddBadET(List<BadET> list, EnrollmentTransaction t, int flag)
        {
            var b = new BadET
            {
                TranId = t.TransactionId,
                Flag = flag,
                OrgId = t.OrganizationId,
                PeopleId = t.PeopleId,
            };
            if (list.Contains(b))
                return;
            list.Add(b);
        }
        void WriteBadEts(List<BadET> list)
        {
            foreach (var b in list)
            {
                DbUtil.Db.BadETs.InsertOnSubmit(b);
                Debug.WriteLine("pid:{0}, {1}".Fmt(b.PeopleId, b.Flag));
            }
            DbUtil.Db.SubmitChanges();
        }
    }
}
