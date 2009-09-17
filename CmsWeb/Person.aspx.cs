/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using UtilityExtensions;
using CMSPresenter;
using System.Linq;
using CmsData;
using System.Drawing;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Data.Linq;
//using System.Transactions;
using System.Web.Services;
using System.Data.Linq.SqlClient;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace CMSWeb
{
    public partial class PersonPage : System.Web.UI.Page
    {
        private int[] DiscClassStatus = new int[] 
        { 
            (int)Person.DiscoveryClassStatusCode.AdminApproval, 
            (int)Person.DiscoveryClassStatusCode.Attended, 
            (int)Person.DiscoveryClassStatusCode.ExemptedChild 
        };

        public Person person;
        private bool RebindEnrollments;
        private bool RebindFamily;
        public Volunteer vol;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var qb = DbUtil.Db.QueryBuilderIsCurrentPerson();
            ExportToolBar1.queryId = qb.QueryId;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var site = (CMSWeb.Site)Page.Master;
            site.ScriptManager.EnablePageMethods = true;

            int? id = this.QueryString<int?>("id");

            if (!id.HasValue)
                id = Session["ActivePersonId"].ToInt2();
            person = DbUtil.Db.People.SingleOrDefault(p => p.PeopleId == id);
            if (person == null)
                Response.EndShowMessage("no person", "/", "home");
            if (Util.OrgMembersOnly && !DbUtil.Db.OrgMembersOnlyTag.People().Any(p => p.PeopleId == id.Value))
            {
                DbUtil.LogActivity("Trying to view person: {0}".Fmt(person.Name));
                Response.EndShowMessage("You must be a member one of this person's organizations to have access to this page");
            }

            vol = person.Volunteers.FirstOrDefault();
            if (vol == null)
                vol = new Volunteer();

            DeletePerson.Visible = User.IsInRole("Admin");

            Picture.ImageUrl = "~/Image.ashx?portrait=1&id=" + (person.PictureId == null ? 0 : person.Picture.SmallId.Value);
            Picture.NavigateUrl = "~/UploadPicture.aspx?id=" + person.PeopleId;

            FamilyLink.NavigateUrl = "~/Family.aspx?id=" + person.FamilyId;

            PersonPrimaryAddr.person = person;
            PersonPrimaryAddr.PreferredAddressControl = PreferredAddressDropDown;
            PersonAltAddr.person = person;
            PersonAltAddr.PreferredAddressControl = PreferredAddressDropDown;
            FamilyPrimaryAddr.person = person;
            FamilyPrimaryAddr.PreferredAddressControl = PreferredAddressDropDown;
            FamilyAltAddr.person = person;
            FamilyAltAddr.PreferredAddressControl = PreferredAddressDropDown;

            Util.CurrentPeopleId = person.PeopleId;
            Session["ActivePerson"] = person.Name;

            EditUpdateButton1.DataBind();
            if (!IsPostBack)
            {
                DbUtil.LogActivity("Viewing Person: {0}".Fmt(person.Name));
                GridPager.SetPageSize(AttendGrid);
                GridPager.SetPageSize(EnrollGrid);
                GridPager.SetPageSize(PrevEnrollGrid);
            }

            ExportToolBar1.TaggedEvent += new EventHandler(ExportToolBar1_TaggedEvent);

            VolAppReview.Visible = User.IsInRole("ApplicationReview");
            VolAppReview.NavigateUrl = "~/AppReview/VolunteerApp.aspx?id=" + person.PeopleId;

            AddContactLink.ToolTip = "Add contact where {0} was the recipient of the ministry".Fmt(person.Name);
            AddContactMadeLink.ToolTip = "Add contact where {0} was the minister".Fmt(person.Name);

            ContributionsLink.NavigateUrl = "~/Contributions/Years.aspx?id={0}".Fmt(person.PeopleId);
            ContributionsLink.Visible = User.IsInRole("Finance");

            var recreg = person.RecRegs.OrderByDescending(v => v.Uploaded).FirstOrDefault();
            if (recreg != null)
            {
                RecFormLink.NavigateUrl = "/Recreation/Detail/{0}".Fmt(recreg.Id);
                RecFormLink.Visible = User.IsInRole("Attendance");
            }

            if (((CMSWeb.Site)Page.Master).ScriptManager.IsInAsyncPostBack)
                if (Page.Request.Params["__EVENTTARGET"] == UpdatePanel1.ClientID)
                    if (Page.Request.Params["__EVENTARGUMENT"] == "RebindMemberGrids")
                        RebindMemberGrids();

            movestuff.NavigateUrl = "~/MovePersonDialog.aspx?id={0}&TB_iframe=true&height=450&width=600"
                .Fmt(person.PeopleId);
            movestuff.Visible = User.IsInRole("Admin");
            var otherid = this.QueryString<int?>("goback");
            goback.Visible = User.IsInRole("Admin") && otherid.HasValue && otherid.Value > 0;
            if (goback.Visible)
                goback.NavigateUrl = "~/Person.aspx?id=" + otherid.Value;
        }

        void ExportToolBar1_TaggedEvent(object sender, EventArgs e)
        {
            Response.Redirect("~/Person.aspx?id=" + person.PeopleId.ToString());
        }

        private void GridOnTabClick(TabContainer tabs, string find, GridView grid, string onclick)
        {
            var tab = tabs.FindControl(find) as TabPanel;
            if (!grid.Visible)
                tab.OnClientClick = onclick;
        }
        public string addrtab { get; set; }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            switch (person.AddressTypeId)
            {
                case (int)Address.AddressTypes.Family:
                    addrtab = "family1-tab";
                    break;
                case (int)Address.AddressTypes.Personal:
                    addrtab = "personal1-tab";
                    break;
                case (int)Address.AddressTypes.PersonalAlternate:
                    addrtab = "personal2-tab";
                    break;
                case (int)Address.AddressTypes.FamilyAlternate:
                    addrtab = "family2-tab";
                    break;
            }
            PersonPrimaryAddr.PreferredAddressControl = PreferredAddressDropDown;
            PersonAltAddr.PreferredAddressControl = PreferredAddressDropDown;
            FamilyPrimaryAddr.PreferredAddressControl = PreferredAddressDropDown;
            FamilyAltAddr.PreferredAddressControl = PreferredAddressDropDown;

            trPrimaryAddress2.Visible = person.PrimaryAddress2.HasValue();
            trSchool.Visible = person.SchoolOther.HasValue() || SchoolOther.Editing == true;
            trEmployer.Visible = person.EmployerOther != null || EmployerOther.Editing == true;
            trOccupation.Visible = person.OccupationOther != null || OccupationOther.Editing == true;

            CopyToClipboard.OnClientClick = "copyclip(" + HiddenAddress.ClientID + ");return false;";
            HiddenAddress.Text = person.Name + "\n" + person.PrimaryAddress + "\n";
            if (person.PrimaryAddress2.HasValue())
                HiddenAddress.Text += person.PrimaryAddress2 + "\n";
            HiddenAddress.Text += person.CityStateZip;

            deceased.Visible = person.DeceasedDate.HasValue;
        }

        protected void EditUpdateButton1_Click(object sender, EventArgs e)
        {
            if (EditUpdateButton1.Updating)
                {
                    MemberProfileAutomation();
                    DbUtil.Db.SubmitChanges();
                    OrganizationMember.UpdateMeetingsToUpdate();
                    DbUtil.LogActivity("Updated Person: {0}".Fmt(person.Name));
                    DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, person);
                    EditUpdateButton1.DataBind();
                    if (RebindEnrollments)
                    {
                        EnrollGrid.DataBind();
                        PrevEnrollGrid.DataBind();
                        UpdatePanel1.Update();
                        UpdatePanel2.Update();
                    }
                    if (RebindFamily)
                        FamilyGrid.DataBind();
                }
        }

        protected void HiddenButton1_Click(object sender, EventArgs e)
        {
            DbUtil.LogActivity("Viewing enrollment for: {0}".Fmt(person.Name));
            EnrollGrid.Visible = true;
        }
        protected void HiddenButton2_Click(object sender, EventArgs e)
        {
            DbUtil.LogActivity("Viewing previous enrollment for: {0}".Fmt(person.Name));
            PrevEnrollGrid.Visible = true;
        }
        protected void HiddenButton3_Click(object sender, EventArgs e)
        {
            DbUtil.LogActivity("Viewing pending enrollment for: {0}".Fmt(person.Name));
            PendingEnrollGrid.Visible = true;
        }
        protected void HiddenButton4_Click(object sender, EventArgs e)
        {
            DbUtil.LogActivity("Viewing attendance for: {0}".Fmt(person.Name));
            AttendGrid.Visible = true;
        }

        protected void AddContact_Click(object sender, EventArgs e)
        {
            DbUtil.LogActivity("Adding contact to: {0}".Fmt(person.Name));
            var c = new NewContact
            {
                CreatedDate = DateTime.Now,
                CreatedBy = Util.UserId1,
                ContactDate = Util.Now.Date,
                ContactTypeId = 99,
                ContactReasonId = 99,
            };

            DbUtil.Db.NewContacts.InsertOnSubmit(c);
            DbUtil.Db.SubmitChanges();

            var pc = new Contactee
            {
                PeopleId = person.PeopleId,
                ContactId = c.ContactId
            };

            DbUtil.Db.Contactees.InsertOnSubmit(pc);
            DbUtil.Db.SubmitChanges();

            Response.Redirect("~/Contact.aspx?id=" + c.ContactId);
        }
        protected void AddDelegatedTask_Click(object sender, EventArgs e)
        {
            var pid = Util.UserPeopleId.Value;
            var active = (int)Task.StatusCode.Active;
            var t = new Task
            {
                OwnerId = pid,
                Description = "NewTask",
                ListId = Models.TaskModel.InBoxId(pid),
                CoListId = Models.TaskModel.InBoxId(person.PeopleId),
                StatusId = active,
            };
            person.TasksCoOwned.Add(t);
            DbUtil.Db.SubmitChanges();
            Response.Redirect("~/Task/List/{0}".Fmt(t.Id));
        }

        protected void AddContactMade_Click(object sender, EventArgs e)
        {
            DbUtil.LogActivity("Adding contact from: {0}".Fmt(person.Name));
            var c = new NewContact
            {
                CreatedDate = DateTime.Now,
                CreatedBy = Util.UserId1,
                ContactDate = Util.Now.Date,
                ContactTypeId = 99,
                ContactReasonId = 99,
            };

            DbUtil.Db.NewContacts.InsertOnSubmit(c);
            DbUtil.Db.SubmitChanges();

            var cp = new Contactor
            {
                PeopleId = person.PeopleId,
                ContactId = c.ContactId
            };

            DbUtil.Db.Contactors.InsertOnSubmit(cp);
            DbUtil.Db.SubmitChanges();

            Response.Redirect("~/Contact.aspx?id=" + c.ContactId);
        }
        protected void AddAboutTask_Click(object sender, EventArgs e)
        {
            var pid = Util.UserPeopleId.Value;
            var active = (int)Task.StatusCode.Active;
            var t = new Task
            {
                OwnerId = pid,
                Description = "NewTask",
                ListId = Models.TaskModel.InBoxId(pid),
                StatusId = active,
            };
            person.TasksAboutPerson.Add(t);
            DbUtil.Db.SubmitChanges();
            Response.Redirect("~/Task/List/{0}".Fmt(t.Id));
        }

        protected void FamilyGrid_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var h = e.Row.FindControl("namelink") as HyperLink;
                var d = e.Row.DataItem as FamilyMember;
                var pid = Session["ActivePersonId"].ToInt2();
                var ppl = DbUtil.Db.People.Single(p => p.PeopleId == pid);
                if (d.Deceased)
                    h.ForeColor = Color.Red;
                if (d.Id == ppl.SpouseId)
                    h.Text = h.Text + "*";
            }
        }
        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            return MyTags.ToggleTag(PeopleId, controlid);
        }
        void RebindMemberGrids()
        {
            EnrollGrid.DataBind();
            PrevEnrollGrid.DataBind();
            UpdatePanel1.Update();
            UpdatePanel2.Update();
        }

        protected void DeletePerson_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Util.Auditing = false;
            if (!person.PurgePerson())
            {
                ValidateDelete.IsValid = false;
                return;
            }
            Util.CurrentPeopleId = 0;
            Session.Remove("ActivePerson");
            Response.EndShowMessage("Person Deleted", "/", "click here");
        }

        protected void Enroll_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var d = e.Row.DataItem as PersonController.OrganizationView;
            var mlink = e.Row.FindControl("MemberLink") as HyperLink;
            if (mlink != null)
                mlink.NavigateUrl = "~/EditMemberDialog.aspx?oid={0}&pid={1}&from={2}&TB_iframe=true&height=450&width=600"
                    .Fmt(d.Id, person.PeopleId, UpdatePanel1.ClientID);
        }
        protected void PrevEnroll_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var d = e.Row.DataItem as PersonController.OrganizationView;
            var mlink = e.Row.FindControl("MemberLink") as HyperLink;
            if (mlink != null)
            {
                mlink.NavigateUrl = "~/AttendStrDetail.aspx?id={0}&oid={1}"
                    .Fmt(person.PeopleId, d.Id);
            }
        }
        [WebMethod]
        public static string[] GetPrevChurchCompletionList(string prefixText, int count)
        {
            var q = from p in DbUtil.Db.People
                    where p.OtherPreviousChurch.Contains(prefixText)
                    group p by p.OtherPreviousChurch into g
                    orderby g.Key
                    select g.Key;
            return q.Take(count).ToArray();
        }
        [WebMethod]
        public static string[] GetNewChurchCompletionList(string prefixText, int count)
        {
            var q = from p in DbUtil.Db.People
                    where p.OtherNewChurch.Contains(prefixText)
                    group p by p.OtherNewChurch into g
                    orderby g.Key
                    select g.Key;
            return q.Take(count).ToArray();
        }
        [WebMethod]
        public static string[] GetSchoolCompletionList(string prefixText, int count)
        {
            var q = from p in DbUtil.Db.People
                    where p.SchoolOther.StartsWith(prefixText)
                    group p by p.SchoolOther into g
                    orderby g.Key
                    select g.Key;
            return q.Take(count).ToArray();
        }
        [WebMethod]
        public static string[] GetEmployerCompletionList(string prefixText, int count)
        {
            var q = from p in DbUtil.Db.People
                    where p.EmployerOther.StartsWith(prefixText)
                    group p by p.EmployerOther into g
                    orderby g.Key
                    select g.Key;
            return q.Take(count).ToArray();
        }

        private void MemberProfileAutomation()
        {
            if (DecisionTypeId.HadBeenChanged)
                switch (person.DecisionTypeId ?? 0)
                {
                    case (int)Person.DecisionCode.ProfessionForMembership:
                        person.MemberStatusId = (int)Person.MemberStatusCode.Pending;
                        if (person.DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            person.DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.Pending;
                        if (person.Age <= 12 && person.Family.People.Any(p =>
                                p.PositionInFamilyId == (int)Person.PositionInFamilyCode.Primary
                                && p.MemberStatusId == (int)Person.MemberStatusCode.Member
                                && SqlMethods.DateDiffMonth(p.JoinDate, Util.Now) >= 12))
                            person.BaptismTypeId = (int)Person.BaptismTypeCode.Biological;
                        else
                            person.BaptismTypeId = (int)Person.BaptismTypeCode.Original;
                        person.BaptismStatusId = (int)Person.BaptismStatusCode.NotScheduled;
                        break;
                    case (int)Person.DecisionCode.ProfessionNotForMembership:
                        person.MemberStatusId = (int)Person.MemberStatusCode.NotMember;
                        if (person.DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            person.DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.NotSpecified;
                        if (person.BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                        {
                            person.BaptismTypeId = (int)Person.BaptismTypeCode.NonMember;
                            person.BaptismStatusId = (int)Person.BaptismStatusCode.NotScheduled;
                        }
                        break;
                    case (int)Person.DecisionCode.Letter:
                        person.MemberStatusId = (int)Person.MemberStatusCode.Pending;
                        if (person.DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            person.DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.Pending;
                        if (person.BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                        {
                            person.BaptismTypeId = (int)Person.BaptismTypeCode.NotSpecified;
                            person.BaptismStatusId = (int)Person.BaptismStatusCode.NotSpecified;
                        }
                        break;
                    case (int)Person.DecisionCode.Statement:
                        person.MemberStatusId = (int)Person.MemberStatusCode.Pending;
                        if (person.DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            person.DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.Pending;
                        if (person.BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                        {
                            person.BaptismTypeId = (int)Person.BaptismTypeCode.NotSpecified;
                            person.BaptismStatusId = (int)Person.BaptismStatusCode.NotSpecified;
                        }
                        break;
                    case (int)Person.DecisionCode.StatementReqBaptism:
                        person.MemberStatusId = (int)Person.MemberStatusCode.Pending;
                        if (person.DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            person.DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.Pending;
                        if (person.BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                        {
                            person.BaptismTypeId = (int)Person.BaptismTypeCode.Required;
                            person.BaptismStatusId = (int)Person.BaptismStatusCode.NotScheduled;
                        }
                        break;
                    case (int)Person.DecisionCode.Cancelled:
                        person.MemberStatusId = (int)Person.MemberStatusCode.NotMember;
                        if (person.DiscoveryClassStatusId != (int)Person.DiscoveryClassStatusCode.Attended)
                            person.DiscoveryClassStatusId = (int)Person.DiscoveryClassStatusCode.NotSpecified;
                        if (person.BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                            if (person.BaptismStatusId != (int)Person.BaptismStatusCode.Completed)
                            {
                                person.BaptismTypeId = (int)Person.BaptismTypeCode.NotSpecified;
                                person.BaptismStatusId = (int)Person.BaptismStatusCode.Canceled;
                            }
                        person.EnvelopeOptionsId = (int)Person.EnvelopeOptionCode.None;
                        break;
                }
            // This section sets join codes
            if (DiscoveryClassStatusID.HadBeenChanged || BaptismStatusId.HadBeenChanged)
                switch (person.DecisionTypeId ?? 0)
                {
                    case (int)Person.DecisionCode.ProfessionForMembership:
                        if (person.DiscoveryClassStatusId.HasValue 
                            && DiscClassStatus.Contains(person.DiscoveryClassStatusId.Value)
                            && person.BaptismStatusId == (int)Person.BaptismStatusCode.Completed)
                        {
                            person.MemberStatusId = (int)Person.MemberStatusCode.Member;
                            if (person.BaptismTypeId == (int)Person.BaptismTypeCode.Biological)
                                person.JoinCodeId = (int)Person.JoinTypeCode.BaptismBIO;
                            else
                                person.JoinCodeId = (int)Person.JoinTypeCode.BaptismPOF;
                            person.JoinDate = (person.DiscoveryClassDate.HasValue && person.DiscoveryClassDate.Value > person.BaptismDate.Value) ?
                                person.DiscoveryClassDate.Value : person.BaptismDate.Value;
                        }
                        break;
                    case (int)Person.DecisionCode.Letter:
                        if (DiscoveryClassStatusID.HadBeenChanged)
                            if ((person.DiscoveryClassStatusId.HasValue
                                    && DiscClassStatus.Contains(person.DiscoveryClassStatusId.Value))
                                || person.DiscoveryClassStatusId == (int)Person.DiscoveryClassStatusCode.AdminApproval)
                            {
                                person.MemberStatusId = (int)Person.MemberStatusCode.Member;
                                person.JoinCodeId = (int)Person.JoinTypeCode.Letter;
                                person.JoinDate = person.DiscoveryClassDate.HasValue? person.DiscoveryClassDate : person.DecisionDate;
                            }
                        break;
                    case (int)Person.DecisionCode.Statement:
                        if (DiscoveryClassStatusID.HadBeenChanged)
                            if (person.DiscoveryClassStatusId.HasValue
                                    && DiscClassStatus.Contains(person.DiscoveryClassStatusId.Value))
                            {
                                person.MemberStatusId = (int)Person.MemberStatusCode.Member;
                                person.JoinCodeId = (int)Person.JoinTypeCode.Statement;
                                person.JoinDate = person.DiscoveryClassDate.HasValue ? person.DiscoveryClassDate : person.DecisionDate;
                            }
                        break;
                    case (int)Person.DecisionCode.StatementReqBaptism:
                        if ((person.DiscoveryClassStatusId.HasValue
                                    && DiscClassStatus.Contains(person.DiscoveryClassStatusId.Value))
                            && person.BaptismStatusId == (int)Person.BaptismStatusCode.Completed)
                        {
                            person.MemberStatusId = (int)Person.MemberStatusCode.Member;
                            person.JoinCodeId = (int)Person.JoinTypeCode.BaptismSRB;
                            if (person.DiscoveryClassDate.HasValue)
                                person.JoinDate = person.DiscoveryClassDate.Value > person.BaptismDate.Value ?
                                    person.DiscoveryClassDate.Value : person.BaptismDate.Value;
                            else
                                person.JoinDate = person.BaptismDate;
                        }
                        break;
                }
            if (DeceasedDate3.HadBeenChanged)
            {
                if (person.DeceasedDate.HasValue)
                    DeceasePerson();
                RebindFamily = true;
            }
            else if (DropCodeId.HadBeenChanged)
            {
                switch (person.DropCodeId)
                {
                    case (int)Person.DropTypeCode.Administrative:
                        DropMembership();
                        break;
                    case (int)Person.DropTypeCode.AnotherDenomination:
                        DropMembership();
                        break;
                    case (int)Person.DropTypeCode.Duplicate:
                        DropMembership();
                        person.MemberStatusId = (int)Person.MemberStatusCode.NotMember;
                        break;
                    case (int)Person.DropTypeCode.LetteredOut:
                        DropMembership();
                        break;
                    case (int)Person.DropTypeCode.Other:
                        DropMembership();
                        break;
                    case (int)Person.DropTypeCode.Requested:
                        DropMembership();
                        break;
                }
            }
            if (DiscoveryClassStatusID.HadBeenChanged 
                && person.DiscoveryClassStatusId == (int)Person.DiscoveryClassStatusCode.Attended)
            {
                var q = from om in DbUtil.Db.OrganizationMembers
                        where om.PeopleId == person.PeopleId
                        where om.Organization.OrganizationName == "Step 1"
                        select om;
                foreach (var om in q)
                    om.Drop();
            }
        }
        private void DropMembership()
        {
            dropMembership(false);
        }
        private void DeceasePerson()
        {
            dropMembership(true);
        }
        private void dropMembership(bool Deceased)
        {
            if (person.MemberStatusId == (int)Person.MemberStatusCode.Member)
            {
                if (Deceased)
                    person.DropCodeId = (int)Person.DropTypeCode.Deceased;
                person.MemberStatusId = (int)Person.MemberStatusCode.Previous;
                person.DropDate = Util.Now.Date;
            }
            if (Deceased)
            {
                person.EmailAddress = null;
                person.DoNotCallFlag = true;
                person.DoNotMailFlag = true;
                person.DoNotVisitFlag = true;
            }
            if (person.SpouseId.HasValue)
            {
                var spouse = DbUtil.Db.LoadPersonById(person.SpouseId.Value);
                if (Deceased)
                {
                    spouse.MaritalStatusId = (int)Person.MaritalStatusCode.Widowed;
                    if (spouse.EnvelopeOptionsId != (int)Person.EnvelopeOptionCode.None)
                        spouse.EnvelopeOptionsId = (int)Person.EnvelopeOptionCode.Individual;
                    spouse.ContributionOptionsId = (int)Person.EnvelopeOptionCode.Individual;
                }

                if (spouse.MemberStatusId == (int)Person.MemberStatusCode.Member)
                    if (spouse.EnvelopeOptionsId == (int)Person.EnvelopeOptionCode.Joint)
                        spouse.EnvelopeOptionsId = (int)Person.EnvelopeOptionCode.Individual;
            }
            person.EnvelopeOptionsId = (int)Person.EnvelopeOptionCode.None;
            foreach (var om in person.OrganizationMembers)
                om.Drop();
            RebindEnrollments = true;
        }
        [WebMethod]
        public static string VerifyAddress(string clientid, string line1, string line2, string city, string st, string zip)
        {
            var r = PersonController.LookupAddress(line1, line2, city, st, zip);
            r.selector = "#" + clientid.Substring(0, clientid.Length - 5);
            var jss = new DataContractJsonSerializer(typeof(PersonController.AddressResult));
            var ms = new MemoryStream();
            jss.WriteObject(ms, r);
            return Encoding.Default.GetString(ms.ToArray());
        }
    }
}
