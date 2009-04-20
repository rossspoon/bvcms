/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CMSPresenter;
using CmsData;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Linq;

namespace CMSWeb
{
    public partial class MyTags : System.Web.UI.Page
    {
        TagController ctl;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var qb = DbUtil.Db.QueryBuilderHasCurrentTag();
            ExportToolBar1.queryId = qb.QueryId;
            UserTags.SelectParameters[0].DefaultValue = Util.UserPeopleId.ToString();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var site = (CMSWeb.Site)Page.Master;
            site.ScriptManager.EnablePageMethods = true;
            ctl = new TagController();
            if (!IsPostBack)
            {
                DbUtil.LogActivity("Managing Tags");
                Tags.DataBind();
                SetSelectedItem();
                EnableDisableButtons();
            }
            ExportToolBar1.TaggedEvent += new EventHandler(ExportToolBar1_TaggedEvent);
        }

        void ExportToolBar1_TaggedEvent(object sender, EventArgs e)
        {
            PersonGrid1.DataBind();
        }

        protected void setNewTag_Click(object sender, EventArgs e)
        {
            if (!IsValid)
                return;
            ctl.NewTag(TagName.Text);
            Tags.DataBind();
            SetSelectedItem();
            TagName.Text = "";
        }
        protected void renameTag_Click(object sender, EventArgs e)
        {
            ctl.RenameTag(TagName.Text);
            Tags.DataBind();
            SetSelectedItem();
            TagName.Text = "";
        }

        protected void delTag_Click(object sender, EventArgs e)
        {
            ctl.DeleteTag();
            Util.CurrentTag = "UnNamed";
            Tags.DataBind();
            SetSelectedItem();
        }
        private void EnableDisableButtons()
        {
            //ShareLink.Enabled = Util.UserPeopleId == Util.CurrentTagOwnerId;
            delTag.Enabled = ShareLink.Enabled;
        }
        protected void Tags_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = Tags.SelectedValue.SplitStr(",", 2);
            Util.CurrentTag = a[1];
            ShareLink.Text = ctl.SharedCount();
            PersonGrid1.DataBind();
            EnableDisableButtons();
        }
        private void SetSelectedItem()
        {
            var t = DbUtil.Db.TagCurrent();
            var item = Tags.Items.FindByValue(t.Id + "," + Util.CurrentTag);
            if (item != null)
                item.Selected = true;
            PeopleData.SelectParameters.UpdateValues(HttpContext.Current, Tags);
            ShareLink.Text = ctl.SharedCount();
        }
        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            var r = new ToggleTagReturn { ControlId = controlid };
            r.HasTag = Person.ToggleTag(PeopleId, Util.CurrentTagName, Util.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();
            var jss = new DataContractJsonSerializer(typeof(ToggleTagReturn));
            var ms = new MemoryStream();
            jss.WriteObject(ms, r);
            return Encoding.Default.GetString(ms.ToArray());
        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            PersonGrid1.DataBind();
        }

    }
}
