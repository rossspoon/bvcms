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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CmsData;
using CMSPresenter;
using System.Collections.Generic;

namespace CmsWeb
{
    public partial class QueryConditions : System.Web.UI.UserControl
    {
        public delegate void ConditionSelectHandler(string field);
        public event ConditionSelectHandler Selected;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ConditionTabs.DataSource = FieldCategories();
                ConditionTabs.DataBind();
                Repeater1.DataSource = FieldCategories();
                Repeater1.DataBind();
            }
        }

        protected void ItemCommand_Click(object source, RepeaterCommandEventArgs e)
        {
            var field = e.CommandArgument.ToString();
            if (Selected != null)
                Selected(field);
        }

        protected void lbGroup_Click(object sender, EventArgs e)
        {
            if (Selected != null)
                Selected("Group");
        }
        public IEnumerable<CategoryClass> FieldCategories()
        {
            var q = from c in CategoryClass.Categories
                    where c.Title != "Grouping"
                    select c;
            return q;
        }
    }
}