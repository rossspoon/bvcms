/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using UtilityExtensions;
using CMSPresenter;
using CmsData;
using System.Threading;

namespace CMSWeb
{
    public partial class ExportToolBar : System.Web.UI.UserControl
    {
        public int queryId { get; set; }
        private QueryController QueryControl = new QueryController();
        public bool Single { get; set; }
        public bool OrganizationContext { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            EmailLink.NavigateUrl = GoTo("NavWindow", "EmailPeople.aspx?");
            LabelsLink.NavigateUrl = "/Report/LabelsRpt.aspx?id=" + queryId;
            VolunteerLink.NavigateUrl = "/Volunteers/Index/" + queryId;
            ExcelLink.NavigateUrl = "/ExportExcel.aspx?id=" + queryId;
            BulkMailLink.NavigateUrl = "/bulkmail.aspx?" + queryId;
            ProspectLink.NavigateUrl = GoTo("NewWindow", "Report/ProspectCardsRpt.aspx?");
            InreachLink.NavigateUrl = GoTo("NewWindow", "Report/InreachRpt.aspx?");
            ContactsLink.NavigateUrl = GoTo("NewWindow", "Report/ContactReport.aspx?");
            BarCodeLabels.NavigateUrl = GoTo("NewWindow", "Report/ChoirMeeting.aspx?");
            AveryLabels.NavigateUrl = GoTo("NewWindow", "Report/Avery.aspx?");
            AveryLabels3.NavigateUrl = GoTo("NewWindow", "Report/Avery3.aspx?");
            AveryAddressLabels.NavigateUrl = "/Report/AveryAddress.aspx?id=" + queryId;
            InvolvementLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Involvement&");
            AttendLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Attend&");
            ChildrenLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Children&");
            ChurchLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Church&");
            MemberLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Organization&");
            SmlLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=SML&");
            LRLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=LR&");
            PromoLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Promotion&");
            TagAddLabel.Text = Single ? "Add" : "Add All";
            TagRemoveLabel.Text = Single? "Remove" : "Remove All";
            RollsheetItem.Visible = OrganizationContext;
            MemberItem.Visible = OrganizationContext;
            SmlItem.Visible = OrganizationContext;
            LRItem.Visible = OrganizationContext;
            PromoItem.Visible = OrganizationContext;
        }
        private string GoTo(string function, string target)
        {
            return "javascript:TB" + function + "('" + Page.ResolveUrl("~/{0}id={1}')".Fmt(target, queryId));
        }
        public event EventHandler TaggedEvent;

        protected void TagAddAll_Click(object sender, EventArgs e)
        {
            QueryControl.TagAll(queryId);
            if (TaggedEvent != null)
                TaggedEvent(this, e);
        }

        protected void TagRemoveAll_Click(object sender, EventArgs e)
        {
            QueryControl.UnTagAll(queryId);
            if (TaggedEvent != null)
                TaggedEvent(this, e);
        }
        protected void AddContact_Click(object sender, EventArgs e)
        {
            var c = QueryControl.AddContact(queryId);
            if (c != null)
                Response.Redirect("~/Contact.aspx?id=" + c.ContactId);
        }
        protected void PurgeAll_Click(object sender, EventArgs e)
        {
            var t = new Thread(new ParameterizedThreadStart(StartPurge));
            t.Start(queryId);
            Thread.Sleep(1500);
            Response.Redirect("/PurgeProgress.aspx");
        }
        private void StartPurge(object id)
        {
            var Qb = DbUtil.Db.LoadQueryById((int)id);
            var q = DbUtil.Db.People.Where(Qb.Predicate()).Select(p => p.PeopleId);
            int n = 0;
            var st = DateTime.Now;
            Session.Remove("purgefinished");
            foreach (var pid in q)
            {
                DbUtil.Db.PurgePerson(pid);
                Session["purgecount"] = ++n;
                var ts = DateTime.Now.Subtract(st);
                var dt = new DateTime(ts.Ticks);
                Session["purgespeed"] = "{0:s.ff}".Fmt(new DateTime(Convert.ToInt64(ts.Ticks / n)));
                Session["purgetime"] = "{0:mm:ss}".Fmt(dt);
            }
            Session["purgefinished"] = "finished";
        }
    }
}
