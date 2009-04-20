/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web.UI.WebControls;
using CMSPresenter;
using System.Web.UI;
using UtilityExtensions;
using System.Web;

namespace CMSWeb
{
    public partial class PersonGrid : System.Web.UI.UserControl
    {
        public event EventHandler DataBound;
        //public event GridViewRowEventHandler RowDataBound;

        //protected void PersonGridView_DataBound(object sender, EventArgs e)
        //{
        //}
        public int pagesize { get { return Util.GetPageSizeCookie(); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            pager.PageSize = pagesize;
            pager2.PageSize = pagesize;
        }

        public override void DataBind()
        {
            ListView1.DataBind();
        }
        public string DataSourceID
        {
            set { ListView1.DataSourceID = value; }
        }
        public object DataSource
        {
            set { ListView1.DataSource = value; }
        }

        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            if (DataBound != null)
                DataBound(sender, e);
        }
        public DataPager DataPager
        {
            get { return pager; }
        }
        public int TopPeopleId
        {
            get { return (int)ListView1.DataKeys[0].Value; }
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var link = e.Item.FindControl("HyperLink1") as HyperLink;
                var r = e.Item as ListViewDataItem;
                var d = r.DataItem as PersonInfo;
                link.NavigateUrl = "javascript:PageMethods.ToggleTag({0},'{1}',ToggleTagCallback)".Fmt(d.PeopleId, link.ClientID);
            }
        }

    }
}
