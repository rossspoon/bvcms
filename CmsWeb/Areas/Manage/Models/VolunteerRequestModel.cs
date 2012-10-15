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
using CmsData.Codes;

namespace CmsWeb.Models
{
	public class VolunteerRequestModel
	{
		public long ticks { get; set; }
		public int vid { get; set; }

		public ICollection<int> pids { get; set; }
		public string subject { get; set; }
		public string message { get; set; }

		private CMSDataContext Db;

		public Meeting meeting { get; set; }
		public Person person { get; set; } 
		public Organization org { get; set; }
		public int count { get; set; }
		public int limit { get; set; }

		private void FetchEntities(int mid, int pid)
		{
			var q = from m in Db.Meetings
					where m.MeetingId == mid
					let c = m.Attends.Count(aa => aa.Commitment == AttendCommitmentCode.Attending || aa.Commitment == AttendCommitmentCode.Substitute)
					let p = Db.People.Single(pp => pp.PeopleId == pid)
					select new
					{
						m,
						org = m.Organization,
						p,
						c,
					};
			var i = q.Single();
			org = i.org;
			meeting = i.m;
			person = i.p;
			count = i.c;
		}
		public VolunteerRequestModel()
		{
			Db = DbUtil.Db;
		}
		public VolunteerRequestModel(int mid, int pid, long ticks)
			: this(mid, pid)
		{
			this.ticks = ticks;
		}
		public VolunteerRequestModel(int mid, int pid)
			: this()
		{
			FetchEntities(mid, pid);
		}
		public VolunteerRequestModel(string guid)
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
			var a = ot.Querystring.Split(',');
			FetchEntities(a[0].ToInt(), a[1].ToInt());
			ticks = a[2].ToLong();
			vid = a[3].ToInt();
		}

		public void ComposeMessage()
		{
			var dt = DateTime.Now;
			ticks = dt.Ticks;
			var yeslink = @"<a href=""http://volreqlink"" mid=""{0}"" pid=""{1}"" ticks=""{2}"" ans=""yes"">
Yes, I will be there.</a>".Fmt(meeting.MeetingId, person.PeopleId, ticks);
			var nolink = @"<a href=""http://volreqlink"" mid=""{0}"" pid=""{1}"" ticks=""{2}"" ans=""no"">
Sorry, I cannot be there.</a>".Fmt(meeting.MeetingId, person.PeopleId, ticks);

			subject = "Volunteer request for {0}".Fmt(org.OrganizationName);
			message = @"
<p>Hi {{first}},</p>
<p>We need additional volunteers for {0}<br>
on {1:dddd, MMM d} at {1:h:mm tt}</p>
<blockquote>
<p>{2}</p>
<p>{3}</p>
</blockquote>
<p>
Thank you for your consideration,<br />
{4}
</p>".Fmt(org.OrganizationName, meeting.MeetingDate, yeslink, nolink, person.Name);
		}
		public string DisplayMessage { get; set; }
		public string Error { get; set; }


		public Dictionary<int, string> FetchPotentialVolunteers()
		{
			var q = from om in Db.OrganizationMembers
					where om.OrganizationId == org.OrganizationId
					where om.MemberTypeId != CmsData.Codes.MemberTypeCode.InActive
					where om.Pending == false
					where om.PeopleId != person.PeopleId
					where !Db.Attends.Any(aa => aa.MeetingId == meeting.MeetingId
						&& aa.Commitment != null && aa.PeopleId == om.PeopleId)
					orderby om.Person.Name2
					select om.Person;
			return q.ToDictionary(kk => kk.PeopleId, nn => nn.Name);
		}
		public void SendEmails(int? additional)
		{
			var tag = Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, Db.NextTagId);
			Db.ExecuteCommand("delete TagPerson where Id = {0}", tag.Id);
			Db.TagAll(pids, tag);
			var dt = new DateTime(ticks);

			foreach (var id in pids)
			{
				var vr = new VolRequest
				{
					MeetingId = meeting.MeetingId,
					RequestorId = person.PeopleId,
					Requested = dt,
					VolunteerId = id,
				};
				meeting.VolRequests.Add(vr);
			}

			var qb = Db.QueryBuilderScratchPad();
			qb.CleanSlate(Db);
			qb.AddNewClause(QueryType.HasMyTag, CompareType.Equal, "{0},temp".Fmt(tag.Id));
			if (additional + limit > limit)
				meeting.AddEditExtra(Db, "TotalVolunteersNeeded", (additional + limit).ToString());
			Db.SubmitChanges();

			var reportlink = @"<a href=""{0}Manage/RequestReport/{1}/{2}/{3}"">Volunteer Request Status Report</a>"
				.Fmt(Db.CmsHost, meeting.MeetingId, person.PeopleId, dt.Ticks);
			var list = Db.PeopleFromPidString(org.NotifyIds).ToList();
			//list.Insert(0, person);
			Db.Email(person.FromEmail, list,
				"Volunteer Commitment for " + org.OrganizationName,
				@"
<p>{0} has requested volunteers on {1:MMM d} for {1:h:mm tt}.</p>
<blockquote>
{2}
</blockquote>".Fmt(person.Name, meeting.MeetingDate, reportlink));

			// Email subs
			var m = new MassEmailer(qb.QueryId, null);
			m.Subject = subject;
			m.Body = message;

			DbUtil.LogActivity("Emailing Vol Subs");
			m.FromName = person.Name;
			m.FromAddress = person.FromEmail;

			var eqid = m.CreateQueue(transactional: true);
			string host = Util.Host;
			// save these from HttpContext to set again inside thread local storage
			var useremail = Util.UserEmail;
			var isinroleemailtest = HttpContext.Current.User.IsInRole("EmailTest");

			TaskAlias.Factory.StartNew(() =>
			{
				Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
				try
				{
					var db = new CMSDataContext(Util.GetConnectionString(host));
					db.Host = host;
					// set these again inside thread local storage
					Util.UserEmail = useremail;
					Util.IsInRoleEmailTest = isinroleemailtest;
					db.SendPeopleEmail(eqid);
				}
				catch (Exception ex)
				{
					var ex2 = new Exception("Emailing error for queueid " + eqid, ex);
					ErrorLog errorLog = ErrorLog.GetDefault(null);
					errorLog.Log(new Error(ex2));

					var db = new CMSDataContext(Util.GetConnectionString(host));
					db.Host = host;
					// set these again inside thread local storage
					Util.UserEmail = useremail;
					Util.IsInRoleEmailTest = isinroleemailtest;
					var equeue = db.EmailQueues.Single(ee => ee.Id == eqid);
					equeue.Error = ex.Message.Truncate(200);
					db.SubmitChanges();
				}
			});
		}
		public void ProcessReply(string ans)
		{
			var dt = new DateTime(ticks);
			var i = (from rr in Db.VolRequests
					 where rr.MeetingId == meeting.MeetingId
					 where rr.RequestorId == person.PeopleId
					 where rr.Requested == dt
					 where rr.VolunteerId == vid
					 let commits = (from a in rr.Meeting.Attends
									where a.Commitment == AttendCommitmentCode.Attending || a.Commitment == AttendCommitmentCode.FindSub
									select a).Count()
					 let needed = (from e in rr.Meeting.MeetingExtras
								   where e.Field == "TotalVolunteersNeeded"
								   select e.Data).Single()
					 select new
					 {
						 volunteer = rr.Volunteer,
						 r = rr,
						 commits,
						 needed
					 }).Single();
			if (i.commits >= i.needed.ToInt())
			{
				DisplayMessage = "This volunteer request has already been covered. Thank you so much for responding.";
				return;
			}
			i.r.Responded = DateTime.Now;
			if (ans != "yes")
			{
				DisplayMessage = "Thank you for responding";
				i.r.CanVol = false;
				Db.SubmitChanges();
				return;
			}
			i.r.CanVol = true;
			Attend.MarkRegistered(Db, i.r.VolunteerId, meeting.MeetingId, AttendCommitmentCode.Attending);
			Db.SubmitChanges();
			var body = @"
<p>{0},</p>
<p>Thank you so much.</p>
<p>You are now assigned to volunteer on {2:MMM d, yyyy} at {2:t}.
in {1}<br />
See you there!</p>".Fmt(i.volunteer.Name, org.OrganizationName, meeting.MeetingDate);

			// on screen message
			DisplayMessage = "<p>You have sent the following email from {0}.</p>\n"
				.Fmt(Util.ObscureEmail(i.volunteer.EmailAddress)) + body;

			// notify requestor and org notifyids
			var list = Db.PeopleFromPidString(org.NotifyIds).ToList();
			list.Insert(0, person);
			Db.Email(i.volunteer.FromEmail, list,
				"Volunteer Committment for " + org.OrganizationName,
				@"
<p>The following email was sent to {0}.</p>
<blockquote>
{1}
</blockquote>".Fmt(i.volunteer.Name, body));
		}
		public class VolStatusInfo
		{
			public string VolName { get; set; }
			public DateTime Requested { get; set; }
			public DateTime? Responded { get; set; }
			public bool? CanVol { get; set; }
			public HtmlString CanVolDisplay
			{
				get
				{
					switch (CanVol)
					{
						case true: return new HtmlString("<span class=\"red\">Can Volunteer</span>");
						case false: return new HtmlString("Cannot Volunteer");
					}
					return new HtmlString("");
				}
			}
		}
		public IEnumerable<VolStatusInfo> VolRequests()
		{
			var dt = new DateTime(ticks);
			var q = from r in Db.VolRequests
					where r.MeetingId == meeting.MeetingId
					where r.RequestorId == person.PeopleId
					where r.Requested == dt
					select new VolStatusInfo
					{
						VolName = r.Volunteer.Name,
						Requested = r.Requested,
						Responded = r.Responded,
						CanVol = r.CanVol
					};
			return q;
		}
	}
}