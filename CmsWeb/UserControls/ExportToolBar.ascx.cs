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

namespace CmsWeb
{
    public partial class ExportToolBar : System.Web.UI.UserControl
    {
        public int queryId { get; set; }
        public bool Single { get; set; }
        public bool OrganizationContext { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            EmailLink.NavigateUrl = "/Email/Index/" + queryId;
            EmailLinkParents.NavigateUrl = "/Email/Index/" + queryId + "?parents=true";
            LabelsLink.NavigateUrl = "/Reports/RollLabels/" + queryId;
            VolunteerLink.NavigateUrl = "/Volunteers/Index/" + queryId;
            ExcelLink.NavigateUrl = "/ExportExcel.aspx?id=" + queryId;
            ExcelUpdateLink.NavigateUrl = "/Export/Update/" + queryId;
            ExcelFamLink.NavigateUrl = "/ExportExcel.aspx?format=AllFamily&id=" + queryId;
            ExcelPicLink.NavigateUrl = "/ExportExcel.aspx?format=IndividualPicture&id=" + queryId;
            BulkMailLink.NavigateUrl = "/bulkmail.aspx?id=" + queryId;
            ProspectLink.NavigateUrl = GoTo2("NewWindow", "Reports/Prospect/" + queryId);
            ContactsLink.NavigateUrl = GoTo2("NewWindow", "Reports/Contacts/" + queryId);
            BarCodeLabels.NavigateUrl = GoTo2("NewWindow", "Reports/BarCodeLabels/" + queryId);
            AveryLabels.NavigateUrl = GoTo2("NewWindow", "Reports/Avery/" + queryId);
            AveryLabels3.NavigateUrl = GoTo2("NewWindow", "Reports/Avery3/" + queryId);
            AveryAddressLabels.NavigateUrl = "/Reports/AveryAddress/" + queryId;
            RegistrationLink.NavigateUrl = "/Reports/Registration/" + queryId;
            InvolvementLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Involvement&");
            AttendLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Attend&");
            AttendanceLink.NavigateUrl = GoTo2("NewWindow", "/Reports/WeeklyAttendance/" + queryId);
            FamilyLink.NavigateUrl = GoTo2("NewWindow", "/Reports/Family/" + queryId);
            ChildrenLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Children&");
            ChurchLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Church&");
            MemberLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Organization&");
            SmlLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=SML&");
            PromoLink.NavigateUrl = GoTo("NewWindow", "ExportExcel.aspx?format=Promotion&");
            TagAddLabel.Text = Single ? "Add" : "Add All";
            TagRemoveLabel.Text = Single? "Remove" : "Remove All";
            RollsheetItem.Visible = OrganizationContext;
            MemberItem.Visible = OrganizationContext;
            SmlItem.Visible = OrganizationContext;
            PromoItem.Visible = OrganizationContext;
        }
        private string GoTo(string function, string target)
        {
            return "javascript:TB" + function + "('" + Page.ResolveUrl("~/{0}id={1}')".Fmt(target, queryId));
        }
        private string GoTo2(string function, string target)
        {
            return "javascript:TB" + function + "('" + target + "')";
        }
        public event EventHandler TaggedEvent;

        protected void TagAddAll_Click(object sender, EventArgs e)
        {
            var q = DbUtil.Db.PeopleQuery(queryId);
            DbUtil.Db.TagAll(q);
            if (TaggedEvent != null)
                TaggedEvent(this, e);
        }

        protected void TagRemoveAll_Click(object sender, EventArgs e)
        {
            var q = DbUtil.Db.PeopleQuery(queryId);
            DbUtil.Db.UnTagAll(q);
            if (TaggedEvent != null)
                TaggedEvent(this, e);
        }
        protected void AddContact_Click(object sender, EventArgs e)
        {
            var id = NewContact.AddContact(queryId);
            Response.Redirect("~/Contact.aspx?id=" + id);
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
            var q = DbUtil.Db.People.Where(Qb.Predicate(DbUtil.Db)).Select(p => p.PeopleId);
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
