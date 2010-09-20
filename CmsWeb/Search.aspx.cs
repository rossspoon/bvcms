/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using UtilityExtensions;
using CMSPresenter;
using System.Diagnostics;
using System.Web.Configuration;
using CmsData;

namespace CmsWeb
{
    public partial class Search : System.Web.UI.Page
    {
        class SearchInfo
        {
            public string NameSearch { get; set; }
            public int Tag { get; set; }
            public int Membership { get; set; }
            public string Communication { get; set; }
            public string Address { get; set; }
            public int Campus { get; set; }
        }
        bool AutoNavigateOn1 = false;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                string name = this.QueryString<string>("name");
                if (name.HasValue())
                {
                    Parameters.Name = name;
                    PersonGrid1.Visible = true;
                    AutoNavigateOn1 = true;
                }
            }
        }
        private const string STR_PersonSearch = "PersonSearchInf0";
        protected void Page_Load(object sender, EventArgs e)
        {
            var site = (CmsWeb.Site)Page.Master;
            site.ScriptManager.EnablePageMethods = true;
            if (!Page.IsPostBack)
                if (!Parameters.Name.HasValue() && Session[STR_PersonSearch].IsNotNull())
                {
                    var ps = Session[STR_PersonSearch] as SearchInfo;
                    Parameters.Name = ps.NameSearch;
                    Parameters.Tag = ps.Tag;
                    Parameters.Member = ps.Membership;
                    Parameters.Comm = ps.Communication;
                    Parameters.Addr = ps.Address;
                    Parameters.Campus = ps.Campus;
                    DbUtil.Db.SetNoLock();
                    PersonGrid1.Visible = true;
                }
            Parameters.ClearButtonClicked += new EventHandler(Parameters_ClearButtonClicked);
            Parameters.SearchButtonClicked += new EventHandler(SearchButton_Click);
            PersonGrid1.DataBound += new EventHandler(PersonGrid1_DataBound);
        }

        void Parameters_ClearButtonClicked(object sender, EventArgs e)
        {
            PersonGrid1.Visible = false;
        }

        void PersonGrid1_DataBound(object sender, EventArgs e)
        {
            if (AutoNavigateOn1 && PersonGrid1.DataPager != null && PersonGrid1.DataPager.TotalRowCount == 1)
                Response.Redirect("~/Person/Index/" + PersonGrid1.TopPeopleId);
        }

        private void SaveToSession()
        {
            Session[STR_PersonSearch] = new SearchInfo
            {
                NameSearch = Parameters.Name,
                Tag = Parameters.Tag,
                Membership = Parameters.Member,
                Communication = Parameters.Comm,
                Address = Parameters.Addr,
                Campus = Parameters.Campus
            };
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            DbUtil.Db.SetNoLock();
            PersonGrid1.Visible = true;
            SaveToSession();
        }

        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            return MyTags.ToggleTag(PeopleId, controlid);
        }

        //protected void NewSearch_Click(object sender, EventArgs e)
        //{
        //    Session.Remove(STR_PersonSearch);
        //    Response.Redirect("~/Search.aspx");
        //}
    }
}
