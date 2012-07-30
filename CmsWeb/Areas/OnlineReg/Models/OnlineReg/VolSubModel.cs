using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using CmsWeb.Areas.Main.Models;
using TaskAlias = System.Threading.Tasks.Task;
using System.Threading;
using Elmah;

namespace CmsWeb.Models
{
	public class VolSubModel
	{
		public int pid { get; set; }
		public int oid { get; set; }
		public long ticks { get; set; }
		public int sid { get; set; }
		public ICollection<int> pids { get; set; }
		public string subject { get; set; }
		public string message { get; set; }
		private CMSDataContext Db;
		public VolSubModel()
		{
			Db = DbUtil.Db;
		}
		public VolSubModel(int pid, int oid, long ticks)
			: this()
		{
			this.pid = pid;
			this.oid = oid;
			this.ticks = ticks;


		}
		public VolSubModel(string guid)
			: this()
		{
			var error = "";
			if (!guid.HasValue())
				error = "bad link";
			var g = guid.ToGuid();
			if (g == null)
				error = "invalid link";
			var ot = Db.OneTimeLinks.SingleOrDefault(oo => oo.Id == g.Value);
			if (ot == null)
				error = "invalid link";
			if (ot.Used)
				error = "link used";
			if (ot.Expires.HasValue && ot.Expires < DateTime.Now)
				error = "link expired";
			if (error.HasValue())
				throw new Exception(error);
			ot.Used = true;
			Db.SubmitChanges();
			var vs = JSONHelper.JsonDeserialize<VolSubModel>(ot.Querystring);
			pid = vs.pid;
			oid = vs.oid;
			ticks = vs.ticks;
			sid = vs.sid;
		}
		public DateTime timeslot { get { return new DateTime(ticks); } }
		private Person _Person;
		private Organization _Organization;
		private Meeting _meeting;
		public Meeting GetMeeting()
		{
			if (_meeting == null)
				_meeting = Db.Meetings.SingleOrDefault(mm => mm.OrganizationId == oid && mm.MeetingDate == timeslot);
			return _meeting;
		}
		private Attend _attendance;
		public Attend GetAttendance()
		{
			if (_attendance == null)
				_attendance = Db.Attends.SingleOrDefault(aa =>
					aa.OrganizationId == oid
					&& aa.PeopleId == pid
					&& aa.Registered == true
					&& aa.AttendanceFlag == false
					&& aa.MeetingDate == timeslot);
			return _attendance;
		}
		public void ComposeMessage()
		{
			var yeslink = @"<a href=""http://volsublink"" pid=""{0}"" oid=""{1}"" ticks=""{2}"" ans=""yes"">
Yes, I can sub for you.</a>".Fmt(pid, oid, ticks);
			var nolink = @"<a href=""http://volsublink"" pid=""{0}"" oid=""{1}"" ticks=""{2}"" ans=""no"">
Sorry, I cannot sub for you.</a>".Fmt(pid, oid, ticks);

			subject = "Volunteer substitute request for {0}".Fmt(org.OrganizationName);
			message = @"
<p>Hi {{first}},</p>
<p>I need a substitute for {0}<br>
on {1:dddd, MMM d} at {1:h:mm tt}</p>
<blockquote>
<p>{2}</p>
<p>{3}</p>
</blockquote>
<p>
Thank you for your consideration,<br />
{4}
</p>".Fmt(org.OrganizationName, timeslot, yeslink, nolink, person.Name);
		}
		public string DisplayMessage { get; set; }
		public string Error { get; set; }
		public Dictionary<int, string> FetchPotentialSubs()
		{
			var q = from om in Db.OrganizationMembers
					where om.OrganizationId == oid
					where om.MemberTypeId != CmsData.Codes.MemberTypeCode.InActive
					where om.Pending == false
					where om.PeopleId != pid
					let a = from a in Db.Attends
							where a.OrganizationId == oid
							where a.MeetingDate == timeslot
							select a.PeopleId
					where !a.Contains(om.PeopleId)
					orderby om.Person.Name2
					select om.Person;
			return q.ToDictionary(kk => kk.PeopleId, nn => nn.Name);
		}
		public Person person
		{
			get
			{
				if (_Person == null)
					_Person = Db.LoadPersonById(pid);
				return _Person;
			}
		}
		public Organization org
		{
			get
			{
				if (_Organization == null)
					_Organization = Db.LoadOrganizationById(oid);
				return _Organization;
			}
		}
		public void SendEmails()
		{
			var a = GetAttendance();
			var tag = Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, Db.NextTagId);
			Db.TagAll(pids, tag);
			var dt = DateTime.Now;

			var vrlist = (from vr in Db.SubRequests 
						  where vr.AttendId == a.AttendId 
						  where vr.RequestorId == pid 
						  select vr).ToDictionary(vv => vv.SubstituteId, vv => vv);

			foreach (var id in pids)
			{
				SubRequest vr;
				if (!vrlist.TryGetValue(id, out vr))
				{
					vr = new SubRequest
					{
						AttendId = a.AttendId,
						RequestorId = pid,
						SubstituteId = id,
					};
					a.SubRequests.Add(vr);
				}
				vr.Requested = dt;
			}

			var qb = Db.QueryBuilderScratchPad();
			qb.CleanSlate(Db);
			qb.AddNewClause(QueryType.HasMyTag, CompareType.Equal, "{0},temp".Fmt(tag.Id));
			Db.SubmitChanges();

			var reportlink = @"<a href=""{0}OnlineReg/VolSubReport/{1}/{2}/{3}"">Substitute Status Report</a>".Fmt(Db.CmsHost, pid, oid, ticks);
			var list = Db.PeopleFromPidString(org.NotifyIds).ToList();
			list.Insert(0, person);
			Db.Email(person.FromEmail, list,
				"Volunteer Substitute Commitment for " + org.OrganizationName, 
				@"
<p>{0} has requested a substitute on {1:MMM d} at {1:h:mm tt}.</p>
<blockquote>
{2}
</blockquote>".Fmt(person.Name, timeslot, reportlink));

			// Email subs
			var m = new MassEmailer(qb.QueryId, null);
			m.Subject = subject;
			m.Body = message;

			DbUtil.LogActivity("Emailing Vol Subs");
			m.FromName = person.Name;
			m.FromAddress = person.FromEmail;

			var eqid = m.CreateQueue(transactional: true);
			string host = Util.Host;

			TaskAlias.Factory.StartNew(() =>
			{
				Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
				try
				{
					var db = new CMSDataContext(Util.GetConnectionString(host));
					db.Host = host;
					db.SendPeopleEmail(eqid);
				}
				catch (Exception ex)
				{
					var ex2 = new Exception("Emailing error for queueid " + eqid, ex);
					ErrorLog errorLog = ErrorLog.GetDefault(null);
					errorLog.Log(new Error(ex2));

					var db = new CMSDataContext(Util.GetConnectionString(host));
					db.Host = host;
					var equeue = db.EmailQueues.Single(ee => ee.Id == eqid);
					equeue.Error = ex.Message.Truncate(200);
					db.SubmitChanges();
				}
			});
		}
		public void ProcessReply(string ans)
		{
			var a = GetAttendance();
			if (a == null)
			{
				DisplayMessage = "This substitute request has already been covered. Thank you so much for responding.";
				return;
			}
			var r = (from rr in Db.SubRequests
					 where rr.RequestorId == a.PeopleId
					 where rr.AttendId == a.AttendId
					 where rr.SubstituteId == sid
					 select rr).FirstOrDefault();
			r.Responded = DateTime.Now;
			if (ans != "yes")
			{
				DisplayMessage = "Thank you for responding";
				r.CanSub = false;
				Db.SubmitChanges();
				return;
			}
			r.CanSub = true;
			a.PeopleId = r.SubstituteId;
			Db.SubmitChanges();
			var body = @"
<p>{0},</p>
<p>Thank you so much.</p>
<p>You are now assigned to cover for {1}<br />
in the {2}<br />
on {3:MMM d, yyyy} at {3:H:mm tt}.
See you there!</p>".Fmt(r.Substitute.Name, r.Requestor.Name,
				org.OrganizationName, a.MeetingDate);

			// on screen message
			DisplayMessage = "<p>You have been sent the following email at {0}.</p>\n"
				.Fmt(Util.ObscureEmail(r.Substitute.EmailAddress)) + body;

			// email confirmation
			Db.Email(r.Requestor.FromEmail, r.Substitute,
				"Volunteer Substitute Committment for " + org.OrganizationName, body);

			// notify requestor and org notifyids
			var list = Db.PeopleFromPidString(org.NotifyIds).ToList();
			list.Insert(0, r.Requestor);
			Db.Email(r.Substitute.FromEmail, list,
				"Volunteer Substitute Committment for " + org.OrganizationName, 
				@"
<p>The following email was sent to {0}.</p>
<blockquote>
{1}
</blockquote>".Fmt(r.Substitute.Name, body));
		}
		public class SubStatusInfo
		{
			public string SubName { get; set; }
			public DateTime Requested { get; set; }
			public DateTime? Responded { get; set; }
			public bool? CanSub { get; set; }
			public HtmlString CanSubDisplay
			{
				get
				{
					switch (CanSub)
					{
						case true: return new HtmlString("<span class=\"red\">Can Substitute</span>"); break;
						case false: return new HtmlString("Cannot Substitute"); break;
					}
					return new HtmlString(""); 
				}
			}
		}
		public IEnumerable<SubStatusInfo> SubRequests()
		{
			var q = from r in Db.SubRequests
					where r.RequestorId == pid
					where r.Attend.MeetingDate == timeslot
					where r.Attend.OrganizationId == oid
					select new SubStatusInfo
					{
						SubName = r.Substitute.Name,
						Requested = r.Requested,
						Responded = r.Responded,
						CanSub = r.CanSub
					};
			return q;
		}
	}
}