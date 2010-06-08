/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;
using UtilityExtensions;
using CMSPresenter;
using CmsData;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections.Generic;
using System.Web;

namespace CMSWeb.Dialog
{
    public partial class AddTagShareds : System.Web.UI.Page
    {
        PersonSearchDialogController ctl = new PersonSearchDialogController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PersonSearchDialogController.ResetSearchTags();
                var t = DbUtil.Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_AddSelected);
                DbUtil.Db.TagPeople.DeleteAllOnSubmit(t.PersonTags);
                DbUtil.Db.SubmitChanges();
                var tag = DbUtil.Db.TagCurrent();
                foreach (var ts in tag.TagShares)
                    t.PersonTags.Add(new TagPerson { PeopleId = ts.PeopleId });
                DbUtil.Db.SubmitChanges();
                ListView1.DataSource = ctl.FetchSearchList(null, null, null, 0, 0, null, -1, 0, 0, true, 99);
                ListView1.DataBind();
            }
            if (Util.UserPeopleId == Util.CurrentTagOwnerId)
                Parameters.SearchButtonClicked += new EventHandler(Parameters_SearchButtonClicked);
            else
                AddSelectedUsers.Enabled = false;
        }

        void Parameters_SearchButtonClicked(object sender, EventArgs e)
        {
            ListView1.DataSource = ctl.FetchSearchList(Parameters, true);
            ListView1.DataBind();
        }

        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            var r = new ToggleTagReturn { ControlId = controlid };
            r.HasTag = Person.ToggleTag(PeopleId, Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_AddSelected);
            DbUtil.Db.SubmitChanges();
            var jss = new DataContractJsonSerializer(typeof(ToggleTagReturn));
            var ms = new MemoryStream();
            jss.WriteObject(ms, r);
            return Encoding.Default.GetString(ms.ToArray());
        }
        protected void AddSelectedUsers_Click(object sender, EventArgs e)
        {
            var tag = DbUtil.Db.TagCurrent();
            var selected_pids = SelectedPeople().Select(p => p.PeopleId).ToArray();
            var userDeletes = tag.TagShares.Where(ts => !selected_pids.Contains(ts.PeopleId));
            DbUtil.Db.TagShares.DeleteAllOnSubmit(userDeletes);
            var tag_pids = tag.TagShares.Select(ts => ts.PeopleId).ToArray();
            var userAdds = from pid in selected_pids
                           join tpid in tag_pids on pid equals tpid into j
                           from p in j.DefaultIfEmpty(-1)
                           where p == -1
                           select pid;
            foreach (var pid in userAdds)
                tag.TagShares.Add(new TagShare { PeopleId = pid });
            DbUtil.Db.SubmitChanges();
            this.Page.ClientScript.RegisterStartupScript(typeof(AddTagShareds), 
                "closeThickBox", "self.parent.ShareWith('{0}');".Fmt(tag.SharedWithCountString()), true);
        }
        public static IQueryable<Person> SelectedPeople()
        {
            var tag = DbUtil.Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_AddSelected);
            return DbUtil.Db.People.Where(p => p.Tags.Any(t => t.Id == tag.Id));
        }
    }
}
