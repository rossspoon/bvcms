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
    public partial class MovePerson : System.Web.UI.Page
    {
        public int PeopleId;

        protected void Page_Load(object sender, EventArgs e)
        {
            PeopleId = Request.QueryString["id"].ToInt();
            if (PeopleId == 0)
                throw new Exception("Cannot visit MovePersonDialog this way");
            if (!IsPostBack)
                PersonSearchDialogController.ResetSearchTags();
            Parameters.SearchButtonClicked += new EventHandler(SearchButton_Click);
            Parameters.ClearButtonClicked += new EventHandler(Parameters_ClearButtonClicked);
        }

        void Parameters_ClearButtonClicked(object sender, EventArgs e)
        {
            ListView1.Visible = false;
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ListView1.Visible = true;
            var ctl = new PersonSearchDialogController();
            ListView1.DataSource = ctl.FetchSearchList(Parameters, false);
            ListView1.DataBind();
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "select1")
            {
                var p = DbUtil.Db.People.Single(pp => pp.PeopleId == PeopleId);
                int otherid = e.CommandArgument.ToInt();
                try
                {
                    p.MovePersonStuff(otherid);
                    DbUtil.Db.SubmitChanges();
                    var s = "'{0}?id={1}&goback={2}'".Fmt(Page.ResolveUrl("~/Person.aspx"), otherid, PeopleId);
                    this.Page.ClientScript.RegisterStartupScript(typeof(MovePerson),
                        "closeThickBox", "self.parent.FinishMove({0});".Fmt(s), true);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
