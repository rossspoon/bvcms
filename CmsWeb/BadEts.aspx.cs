using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using CMSPresenter;
using CmsData;
using UtilityExtensions;

namespace CmsWeb
{
    public partial class BadEts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var site = (CmsWeb.Site)Page.Master;
            //site.ScriptManager.EnablePageMethods = true;
            var q = from et in DbUtil.Db.BadETs
                    group et by new { et.PeopleId, et.OrgId } into g
                    select g;
            NumberPeople.Text = q.Count().ToString("N0");
        }
        //[System.Web.Services.WebMethod]
        //public static string ChangeDate(int id, string dtstr)
        //{
        //    var t = DbUtil.Db.EnrollmentTransactions.Single(et => et.TransactionId == id);
        //    DateTime dt;
        //    if (!DateTime.TryParse(dtstr, out dt))
        //        return "BadDate";
        //    t.TransactionDate = dt;
        //    DbUtil.Db.SubmitChanges();
        //    UpdateLinks(t);
        //    return "OK";
        //}

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;
            var h = e.Row.FindControl("HyperLink1") as HyperLink;
            var d = e.Row.DataItem as BadEtsInfo;
            h.NavigateUrl = "javascript:EditDate({0},'{1}')".Fmt(d.TranId, d.TranDt);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var id = e.CommandArgument.ToInt();
            EnrollmentTransaction t = null;
            EnrollmentTransaction nt = null;
            switch (e.CommandName)
            {
                case "Time++":
                    t = DbUtil.Db.EnrollmentTransactions.Single(et => et.TransactionId == id);
                    t.TransactionDate = t.TransactionDate.AddSeconds(1.0);
                    //t.TransactionNotes = "fix";
                    DbUtil.Db.SubmitChanges();
                    UpdateLinks(t);
                    DbUtil.Db.FlagOddTransaction(t.PeopleId, t.OrganizationId);
                    GridView1.DataBind();
                    break;
                case "Disable":
                    t = DbUtil.Db.EnrollmentTransactions.Single(et => et.TransactionId == id);
                    t.TransactionStatus = true;
                    //t.TransactionNotes = "fix";
                    DbUtil.Db.SubmitChanges();
                    UpdateLinks(t);
                    DbUtil.Db.FlagOddTransaction(t.PeopleId, t.OrganizationId);
                    GridView1.DataBind();
                    break;
                case "Add5":
                    t = DbUtil.Db.EnrollmentTransactions.Single(et => et.TransactionId == id);
                    nt = Clone(t, 5);
                    nt.TransactionDate = t.TransactionDate.AddSeconds(1.0);
                    DbUtil.Db.SubmitChanges();
                    UpdateLinks(t);
                    DbUtil.Db.FlagOddTransaction(t.PeopleId, t.OrganizationId);
                    GridView1.DataBind();
                    break;
                case "Ins1":
                    t = DbUtil.Db.EnrollmentTransactions.Single(et => et.TransactionId == id);
                    nt = Clone(t, 1);
                    nt.TransactionDate = t.TransactionDate.AddSeconds(-1.0);
                    DbUtil.Db.SubmitChanges();
                    UpdateLinks(t);
                    DbUtil.Db.FlagOddTransaction(t.PeopleId, t.OrganizationId);
                    GridView1.DataBind();
                    break;
            }
        }
        private EnrollmentTransaction Clone(EnrollmentTransaction t, int typeid)
        {
            var nt = new EnrollmentTransaction();
            nt.TransactionTypeId = typeid;
            nt.OrganizationId = t.OrganizationId;
            nt.PeopleId = t.PeopleId;
            nt.OrganizationName = t.OrganizationName;
            nt.MemberTypeId = t.MemberTypeId;
            nt.CreatedDate = Util.Now;
            //nt.TransactionNotes = "fix";
            DbUtil.Db.EnrollmentTransactions.InsertOnSubmit(nt);
            return nt;
        }
        private static void UpdateLinks(EnrollmentTransaction t)
        {
            var q = from et in DbUtil.Db.EnrollmentTransactions
                    where et.PeopleId == t.PeopleId && et.OrganizationId == t.OrganizationId
                    orderby et.TransactionDate, et.TransactionId
                    select et;
            foreach (var et in q)
                DbUtil.Db.LinkEnrollmentTransaction(et.TransactionId, et.TransactionDate, et.TransactionTypeId, et.OrganizationId, et.PeopleId);
        }

        protected void RunAnalysis_Click(object sender, EventArgs e)
        {
            DbUtil.Db.ExecuteCommand("exec dbo.FlagOddTransactions");
            GridView1.DataBind();
        }

        protected void ChangeDate_Click(object sender, EventArgs e)
        {
            var id = TranId.Value.ToInt();
            var t = DbUtil.Db.EnrollmentTransactions.Single(et => et.TransactionId == id);
            DateTime dt;
            if (!DateTime.TryParse(NewDate.Value, out dt))
                return;
            t.TransactionDate = dt;
            DbUtil.Db.SubmitChanges();
            UpdateLinks(t);
            DbUtil.Db.FlagOddTransaction(t.PeopleId, t.OrganizationId);
            DataBind();
        }
    }


    public class BadEtsInfo
    {
        public int TranId { get; set; }
        public DateTime TranDt { get; set; }
        public int Flag { get; set; }
        public string Name { get; set; }
        public int PeopleId { get; set; }
        public int? OrgId { get; set; }
        public string OrgName { get; set; }
        public int TranType { get; set; }
        public bool Status { get; set; }
    }


    [DataObject]
    public class BadEtsController
    {
        int count;
        public int Count(int flag, int start, int max)
        {
            return count;
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<BadEtsInfo> FetchBadEts(int flag, int start, int max)
        {
            var q = from et in DbUtil.Db.BadEtsList(flag)
                    orderby et.OrganizationId, et.PeopleId, et.TransactionDate descending
                    select et;
            count = q.Count();
            var q2 = from et in q
                     select new BadEtsInfo
                     {
                         TranType = et.TransactionTypeId,
                         Name = et.Name2,
                         OrgId = et.OrganizationId,
                         TranDt = et.TransactionDate,
                         Flag = et.Flag ?? 0,
                         OrgName = et.OrganizationName,
                         PeopleId = et.PeopleId,
                         TranId = et.TransactionId,
                         Status = et.TransactionStatus,
                     };
            return q2.Skip(start).Take(max);
        }
    }
}
