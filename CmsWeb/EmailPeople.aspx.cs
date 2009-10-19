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
using System.Text.RegularExpressions;
using CMSPresenter;
using CmsData;

namespace CMSWeb
{
    public partial class EmailPeople : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var Qb = DbUtil.Db.LoadQueryById(this.QueryString<int?>("id"));
            if (Qb == null)
                Response.EndShowMessage("query not found");
            var q = DbUtil.Db.People.Where(Qb.Predicate());
            q = q.Where(p => p.EmailAddress != null && p.EmailAddress != "");
            Count.Text = q.Count().ToString();
            if (!Page.IsPostBack)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetAllowResponseInBrowserHistory(false);
            }
        }

        protected void SendEmail_Click(object sender, EventArgs e)
        {
            var Db = DbUtil.Db;
            DbUtil.LogActivity("Emailing people");
            var Qb = Db.LoadQueryById(this.QueryString<int>("id"));
            var q = Db.People.Where(Qb.Predicate());
            q = q.Where(p => p.EmailAddress != null && p.EmailAddress != "");
            var em = new Emailer(EmailFrom.SelectedItem.Value, EmailFrom.SelectedItem.Text);
            em.SendPeopleEmail(q, SubjectLine.Text, EmailBody.Text, FileUpload1);
            Label1.Visible = true;
            SendEmail.Enabled = false;
        }
        protected void TestSendEmail_Click(object sender, EventArgs e)
        {
            var Db = DbUtil.Db;
            DbUtil.LogActivity("Testing Email");
            var q = Db.People.Where(p => p.PeopleId == Util.UserPeopleId);
            var em = new Emailer(EmailFrom.SelectedItem.Value, EmailFrom.SelectedItem.Text);
            em.SendPeopleEmail(q, SubjectLine.Text, EmailBody.Text, FileUpload1);
        }
    }
}
