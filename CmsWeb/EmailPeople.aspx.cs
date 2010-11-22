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
using System.Configuration;

namespace CmsWeb
{
    public partial class EmailPeople : System.Web.UI.Page
    {
        public delegate void AsyncTaskDelegate(object data);
        AsyncTaskDelegate runner = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Util.SessionStarting)
                Response.EndShowMessage("session timed out", "/", "home");

            var Qb = DbUtil.Db.LoadQueryById(this.QueryString<int?>("id"));
            if (Qb == null)
                Response.EndShowMessage("query not found", "/", "home");
            var q = DbUtil.Db.People.Where(Qb.Predicate());

            if (this.QueryString<string>("parents") == "true" || Qb.ParentsOf)
            {
                q = from p in q
                         from m in p.Family.People
                         where (m.PositionInFamilyId == 10 && p.PositionInFamilyId != 10)
                         || (m.PeopleId == p.PeopleId && p.PositionInFamilyId == 10)
                         select m;
                q = q.Distinct();
            }

            q = q.Where(p => p.EmailAddress != null && p.EmailAddress != "");
            Count.Text = q.Count().ToString();
            if (!Page.IsPostBack)
            {
                Response.NoCache();
                Response.Cache.SetAllowResponseInBrowserHistory(false);

                var body = this.QueryString<string>("body");
                if (body.HasValue())
                    EmailBody.Text = Server.UrlDecode(body);
                var subj = this.QueryString<string>("subj");
                if (subj.HasValue())
                    SubjectLine.Text = Server.UrlDecode(subj);
                var ishtml = this.QueryString<bool?>("ishtml");
                IsHtml.Checked = ishtml ?? false;
            }
            CKEditPanel.Visible = IsHtml.Checked;
            SendEmail.Enabled = DbUtil.Db.Setting("emailer", "on") != "off";
        }

        public void DoJob(object data2)
        {
            var args = data2 as EmailArguments;
            HttpContext.Current = args.current;
            var Db = DbUtil.Db;
            var Qb = Db.LoadQueryById(args.QBId);

            Db.SetNoLock();
            var q = Db.People.Where(Qb.Predicate());
            if (args.wantParents || Qb.ParentsOf)
                q = from p in q
                         from m in p.Family.People
                         where (m.PositionInFamilyId == 10 && p.PositionInFamilyId != 10)
                         || (m.PeopleId == p.PeopleId && p.PositionInFamilyId == 10)
                         select m;

            q = from p in q.Distinct()
                where p.EmailAddress != null && p.EmailAddress != ""
                where !p.EmailOptOuts.Any(oo => oo.FromEmail == args.FromAddress)
                orderby p.PeopleId
                select p;
            var em = new Emailer(args.FromAddress, args.FromName);
#if DEBUG2
            em.SendPeopleEmail2(q, args.Subject, args.Body, args.IsHtml, null);
#else
            em.SendPeopleEmail(q, args.Subject, args.Body, args.IsHtml);
#endif
        }

        protected void SendEmail_Click(object sender, EventArgs e)
        {
            DbUtil.LogActivity("Emailing people");

            var task = new PageAsyncTask(
                OnBegin,
                OnEnd,
                null,
                new EmailArguments
                {
                    QBId = this.QueryString<int>("id"),
                    FromAddress = EmailFrom.SelectedItem.Value,
                    FromName = EmailFrom.SelectedItem.Text,
                    Subject = SubjectLine.Text,
                    Body = EmailBody.Text,
                    IsHtml = IsHtml.Checked,
                    current = HttpContext.Current,
                    wantParents = this.QueryString<string>("parents") == "true"
                });

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
#if DEBUG2
            em.SendPeopleEmail2(q, SubjectLine.Text, EmailBody.Text, IsHtml.Checked, null);
#else
            em.SendPeopleEmail(q, SubjectLine.Text, EmailBody.Text, IsHtml.Checked);
#endif
        }

        protected void IsHtml_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
    public class EmailArguments
    {
        public int QBId { get; set; }
        public bool wantParents { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public HttpContext current { get; set; }
    }
}
