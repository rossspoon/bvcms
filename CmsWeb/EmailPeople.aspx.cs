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
using System.Web.UI.WebControls;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace CMSWeb
{
    public partial class EmailPeople : System.Web.UI.Page
    {
        public delegate void AsyncTaskDelegate(object data);
        AsyncTaskDelegate runner = null;

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

        public void DoJob(object data2)
        {
            var args = data2 as EmailArguments;
            HttpContext.Current = args.current;
            var Db = DbUtil.Db;
            var Qb = Db.LoadQueryById(args.QBId);

            var q = Db.People.Where(Qb.Predicate());
            q = q.Where(p => p.EmailAddress != null && p.EmailAddress != "").OrderBy(p => p.PeopleId);
            var em = new Emailer(args.FromAddress, args.FromName);
            em.SendPeopleEmail(q, args.Subject, args.Body, args.FileUpload, args.IsHtml);
        }

        protected void SendEmail_Click(object sender, EventArgs e)
        {
            DbUtil.LogActivity("Emailing people");

            var task = new PageAsyncTask(
                OnBegin,
                OnEnd,
                null,
                new EmailArguments(
                    this.QueryString<int>("id"),
                    EmailFrom.SelectedItem.Value,
                    EmailFrom.SelectedItem.Text,
                    SubjectLine.Text,
                    EmailBody.Text,
                    IsHtml.Checked,
                    FileUpload1, 
                    HttpContext.Current));

            RegisterAsyncTask(task);
            SendEmail.Enabled = false;
        }
        IAsyncResult OnBegin(object sender, EventArgs e, AsyncCallback cb, object state)
        {
            runner = new AsyncTaskDelegate(this.DoJob);
            IAsyncResult result = runner.BeginInvoke(state, cb, state);
            return result;
        }
        void OnEnd(IAsyncResult ar)
        {
            runner.EndInvoke(ar);
            Label1.Visible = true;
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
    public class EmailArguments
    {
        public int QBId { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public FileUpload FileUpload { get; set; }
        public HttpContext current { get; set; }
        public EmailArguments(int qBId, string fromAddress, string fromName, string subject, string body, bool isHtml, FileUpload fileUpload, HttpContext current)
        {
            QBId = qBId;
            FromAddress = fromAddress;
            FromName = fromName;
            Subject = subject;
            Body = body;
            IsHtml = isHtml;
            FileUpload = fileUpload;
            this.current = current;
        }
    }
}
