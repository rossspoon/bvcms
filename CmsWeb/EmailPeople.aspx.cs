/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Web;
using UtilityExtensions;
using CmsData;
using System.Threading;
using System.Net;
using System.Web.UI;

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
            CKEditPanel.Visible = IsHtml.Checked;
        }

        protected void SendEmail_Click(object sender, EventArgs e)
        {
            var Db = DbUtil.Db;
            var Qb = Db.LoadQueryById(this.QueryString<int>("id"));

            DbUtil.LogActivity("Emailing people");
            var q = Db.People.Where(Qb.Predicate());
            q = q.Where(p => p.EmailAddress != null && p.EmailAddress != "");
            var em = new Emailer(EmailFrom.SelectedItem.Value, EmailFrom.SelectedItem.Text);
            em.SendPeopleEmail(q, SubjectLine.Text, EmailBody.Text, FileUpload1, IsHtml.Checked);
            Label1.Visible = true;
            SendEmail.Enabled = false;
        }
        protected void TestSendEmail_Click(object sender, EventArgs e)
        {
            var Db = DbUtil.Db;
            DbUtil.LogActivity("Testing Email");
            var q = Db.People.Where(p => p.PeopleId == Util.UserPeopleId);
            var em = new Emailer(EmailFrom.SelectedItem.Value, EmailFrom.SelectedItem.Text);
            em.SendPeopleEmail(q, SubjectLine.Text, EmailBody.Text, FileUpload1, IsHtml.Checked);
        }

        protected void IsHtml_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
    public class MyAsyncTask
    {
        internal QueryBuilderClause Qb = null;
        private String _taskprogress;
        private AsyncTaskDelegate _dlgt;

        protected delegate void AsyncTaskDelegate();

        public String GetAsyncTaskProgress()
        {
            return _taskprogress;
        }
        public void DoTheAsyncTask()
        {
            var Db = DbUtil.Db;
            //DbUtil.LogActivity("Emailing people");
            //var q = Db.People.Where(Qb.Predicate());
            //q = q.Where(p => p.EmailAddress != null && p.EmailAddress != "");
            //var em = new Emailer(EmailFrom.SelectedItem.Value, EmailFrom.SelectedItem.Text);
            //em.SendPeopleEmail(q, SubjectLine.Text, EmailBody.Text, FileUpload1, IsHtml.Checked);
            //Label1.Visible = true;
            //SendEmail.Enabled = false;
        }

        public IAsyncResult OnBegin(object sender, EventArgs e, AsyncCallback cb, object extraData)
        {
            _taskprogress = "Beginning async task.";
            _dlgt = new AsyncTaskDelegate(DoTheAsyncTask);
            IAsyncResult result = _dlgt.BeginInvoke(cb, extraData);
            return result;
        }
        public void OnEnd(IAsyncResult ar)
        {
            _taskprogress = "Asynchronous task completed.";
            _dlgt.EndInvoke(ar);
        }
        public void OnTimeout(IAsyncResult ar)
        {
            _taskprogress = "Ansynchronous task failed to complete because it exceeded the AsyncTimeout parameter.";
        }
    }
}
