/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using UtilityExtensions;
using CMSPresenter;

namespace CMSWeb
{
    public partial class SavedQuery : System.Web.UI.Page
    {
        QueryController QueryControl = new QueryController();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ExportToolBar1.queryId = this.QueryString<int>("id");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var site = (CMSWeb.Site)Page.Master;
            site.ScriptManager.EnablePageMethods = true;
            PersonGrid1.DataBound += new EventHandler(PersonGrid1_DataBound);
            ExportToolBar1.TaggedEvent += new EventHandler(ExportToolBar1_TaggedEvent);
        }

        void ExportToolBar1_TaggedEvent(object sender, EventArgs e)
        {
            PersonGrid1.DataBind();
        }

        void PersonGrid1_DataBound(object sender, EventArgs e)
        {
            QueryDesc.Text = QueryControl.QueryDescription;
        }

        protected void ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = QueryControl;
        }
        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            return MyTags.ToggleTag(PeopleId, controlid);
        }
    }
}
