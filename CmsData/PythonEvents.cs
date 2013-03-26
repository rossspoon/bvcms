using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using CmsData.Codes;
using UtilityExtensions;
using IronPython.Hosting;
using System;
using System.Text;

namespace CmsData
{
	public class PythonEvents
	{
		private CMSDataContext Db;
		public dynamic instance { get; set; }

		public PythonEvents(CMSDataContext Db, string classname, string script)
		{
			this.Db = Db;
			var engine = Python.CreateEngine();
			var sc = engine.CreateScriptSourceFromString(script);

			var code = sc.Compile();
			var scope = engine.CreateScope();
            scope.SetVariable("model", this);
			code.Execute(scope);

			dynamic Event = scope.GetVariable(classname);
			instance = Event();
		}

        // List of api functions to call from Python

		public void CreateTask(int forPeopleId, Person p, string description)
		{
			DbUtil.LogActivity("Adding Task about: {0}".Fmt(p.Name));
			var t = p.AddTaskAbout(Db, forPeopleId, description);
			Db.SubmitChanges();
            Db.Email(DbUtil.SystemEmailAddress, DbUtil.Db.LoadPersonById(forPeopleId),
                "TASK: " + description,
                Task.TaskLink(Db, description, t.Id) + "<br/>" + p.Name);
		}
		public void JoinOrg(int orgId, Person p)
		{
		    OrganizationMember.InsertOrgMembers(Db, orgId, p.PeopleId, 220, DateTime.Now, null, false);
		}
		public void UpdateField(Person p, string field, object value)
		{
			p.UpdateValue(field, value);
		}
        public void EmailReminders(int orgId)
        {
            var org = Db.LoadOrganizationById(orgId);
            var m = new API.APIOrganization(Db);
            if (org.RegistrationTypeId == RegistrationTypeCode.ChooseVolunteerTimes)
                m.SendVolunteerReminders(orgId, false);
            else
                m.SendEventReminders(orgId);
        }

        public int DayOfWeek { get { return DateTime.Today.DayOfWeek.ToInt(); } }
	    public DateTime DateTime { get { return DateTime.Now; } }

	    public void Email(string savedquery, int queuedBy, string fromaddr, string fromname, string subject, string body, bool transactional = false)
	    {
            var from = new MailAddress(fromaddr, fromname);
			var qB = Db.QueryBuilderClauses.FirstOrDefault(c => c.Description == savedquery && c.SavedBy == "public");
	        if (qB == null)
	            return;
            var q = Db.PeopleQuery(qB.QueryId);
            if (qB.ParentsOf)
				q = Db.PersonQueryParents(q);

            q = from p in q
                where p.EmailAddress != null
                where p.EmailAddress != ""
                where (p.SendEmailAddress1 ?? true) || (p.SendEmailAddress2 ?? false)
                select p;
            var tag = Db.PopulateSpecialTag(q, DbUtil.TagTypeId_Emailer);
	        var emailqueue = Db.CreateQueue(queuedBy, from, subject, body, null, tag.Id, false);
            Db.SendPeopleEmail(emailqueue.Id);
	    }
	    public void EmailContent(string savedquery, int queuedBy, string fromaddr, string fromname, string subject, string content)
	    {
            var from = new MailAddress(fromaddr, fromname);
			var qB = Db.QueryBuilderClauses.FirstOrDefault(cc => cc.Description == savedquery && cc.SavedBy == "public");
	        if (qB == null)
	            return;
            var q = Db.PeopleQuery(qB.QueryId);
            if (qB.ParentsOf)
				q = Db.PersonQueryParents(q);

            q = from p in q
                where p.EmailAddress != null
                where p.EmailAddress != ""
                where (p.SendEmailAddress1 ?? true) || (p.SendEmailAddress2 ?? false)
                select p;
            var tag = Db.PopulateSpecialTag(q, DbUtil.TagTypeId_Emailer);
	        var c = Db.Content(content);
	        if (c == null)
	            return;
	        var emailqueue = Db.CreateQueue(queuedBy, from, subject, c.Body, null, tag.Id, false);
            Db.SendPeopleEmail(emailqueue.Id);
	    }
	}
}
