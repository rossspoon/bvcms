using System.Linq;
using System.Net.Mail;
using UtilityExtensions;
using IronPython.Hosting;
using System;

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

	    public void Email(string savedquery, string fromaddr, string fromname, string subject, string body)
	    {
            var from = new MailAddress(fromaddr, fromname);
			var qB = Db.QueryBuilderClauses.FirstOrDefault(c => c.Description == savedquery && c.IsPublic && c.SavedBy == "public");
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
	        var emailqueue = Db.CreateQueue(from, subject, body, null, tag.Id, false);
            Db.SendPeopleEmail(emailqueue.Id);
	    }
	    public void EmailNotices(string savedquery, string fromaddr, string fromname, string subject, string body)
	    {
            var from = new MailAddress(fromaddr, fromname);
			var qB = Db.QueryBuilderClauses.FirstOrDefault(c => c.Description == savedquery && c.IsPublic && c.SavedBy == "public");
	        if (qB == null)
	            return;
            var q = Db.PeopleQuery(qB.QueryId);
            q = from p in q
                where p.EmailAddress != null
                where p.EmailAddress != ""
                where (p.SendEmailAddress1 ?? true) || (p.SendEmailAddress2 ?? false)
                select p;
            var tag = Db.PopulateSpecialTag(q, DbUtil.TagTypeId_Emailer);
	        var emailqueue = Db.CreateQueue(from, subject, body, null, tag.Id, false);
	        emailqueue.Transactional = true;
            Db.SendPeopleEmail(emailqueue.Id);
	    }
	}
}
