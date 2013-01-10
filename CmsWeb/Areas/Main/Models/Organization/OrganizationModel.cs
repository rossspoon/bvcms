using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CmsData;
using System.Web.Mvc;
using CmsData.Registration;
using UtilityExtensions;
using CMSPresenter;
using System.Text.RegularExpressions;
using CmsData.Codes;

namespace CmsWeb.Models.OrganizationPage
{
	public class OrganizationModel
	{
		public CmsData.Organization org { get; set; }
		public int? OrganizationId { get; set; }
		public List<ScheduleInfo> schedules { get; set; }
		public string Schedule { get; set; }
		public bool IsVolunteerLeader { get; set; }
		public OrganizationModel(int? id)
		{
			OrganizationId = id;
			var q = from o in DbUtil.Db.Organizations
					let sc = o.OrgSchedules.FirstOrDefault() // SCHED
					where o.OrganizationId == id
					select new
					{
						o,
						sch = DbUtil.Db.GetScheduleDesc(sc.MeetingTime),
						sc = o.OrgSchedules
					};
			var i = q.SingleOrDefault();
			if (i == null)
				return;
			org = i.o;
			Schedule = i.sch;
			var u = from s in i.sc
					orderby s.Id
					select new ScheduleInfo(s);
			schedules = u.ToList();
			MemberModel = new MemberModel(id, MemberModel.GroupSelect.Active, String.Empty);

			IsVolunteerLeader = VolunteerLeaderInOrg(OrganizationId);
		}
		public static bool VolunteerLeaderInOrg(int? orgid)
		{
		    if (orgid == null)
		        return false;
		    var o = DbUtil.Db.LoadOrganizationById(orgid);
		    if (o == null || o.RegistrationTypeId != RegistrationTypeCode.ChooseVolunteerTimes)
		        return false;
			if (HttpContext.Current.User.IsInRole("Admin") ||
				HttpContext.Current.User.IsInRole("ManageVolunteers"))
				return true;
			var leaderorgs = DbUtil.Db.GetLeaderOrgIds(Util.UserPeopleId);
		    if (leaderorgs == null)
		        return false;
		    return leaderorgs.Contains(orgid.Value);
		}
		public MemberModel MemberModel;

		private CodeValueController cv = new CodeValueController();

		public IEnumerable<SelectListItem> Groups()
		{
			var q = from g in DbUtil.Db.MemberTags
					where g.OrgId == OrganizationId
					orderby g.Name
					select new SelectListItem
					{
						Text = g.Name,
						Value = g.Id.ToString()
					};
			return q;
		}
		public static IEnumerable<SelectListItem> Tags()
		{
			var cv = new CodeValueController();
			var tg = QueryModel.ConvertToSelect(cv.UserTags(Util.UserPeopleId), "Id").ToList();
			if (HttpContext.Current.User.IsInRole("Edit"))
				tg.Insert(0, new SelectListItem { Value = "-1", Text = "(last query)" });
			tg.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
			return tg;
		}
		public void UpdateSchedules()
		{
			DbUtil.Db.OrgSchedules.DeleteAllOnSubmit(org.OrgSchedules);
			org.OrgSchedules.Clear();
			DbUtil.Db.SubmitChanges();
			foreach (var s in schedules.OrderBy(ss => ss.Id))
				org.OrgSchedules.Add(new OrgSchedule
				{
					OrganizationId = OrganizationId.Value,
					Id = s.Id,
					SchedDay = s.SchedDay,
					SchedTime = s.Time.ToDate(),
					AttendCreditId = s.AttendCreditId
				});
			DbUtil.Db.SubmitChanges();
		}
		public SelectList Schedules()
		{
			var q = new SelectList(schedules.OrderBy(cc => cc.Id), "Value", "Display");
			return q;
		}
		public IEnumerable<Division> Divisions()
		{
			var q = from d in org.DivOrgs
					orderby d.Id ?? 99
					select d.Division;
			return q;
		}

		public IEnumerable<SelectListItem> CampusList()
		{
			return QueryModel.ConvertToSelect(cv.AllCampuses0(), "Id");
		}
		public IEnumerable<SelectListItem> OrgStatusList()
		{
			return QueryModel.ConvertToSelect(cv.OrganizationStatusCodes(), "Id");
		}
		public IEnumerable<SelectListItem> LeaderTypeList()
		{
			var items = CodeValueController.MemberTypeCodes0().Select(c => new CodeValueItem { Code = c.Code, Id = c.Id, Value = c.Value });
			return QueryModel.ConvertToSelect(items, "Id");
		}
		public IEnumerable<SelectListItem> EntryPointList()
		{
			return QueryModel.ConvertToSelect(cv.EntryPoints(), "Id");
		}
		public IEnumerable<SelectListItem> OrganizationTypes()
		{
			return QueryModel.ConvertToSelect(cv.OrganizationTypes0(), "Id");
		}
		public IEnumerable<SelectListItem> GenderList()
		{
			return QueryModel.ConvertToSelect(cv.GenderCodes(), "Id");
		}
		public IEnumerable<SelectListItem> AttendCreditList()
		{
			return QueryModel.ConvertToSelect(CodeValueController.AttendCredits(), "Id");
		}
		public IEnumerable<SelectListItem> SecurityTypeList()
		{
			return QueryModel.ConvertToSelect(cv.SecurityTypeCodes(), "Id");
		}
		public static string SpaceCamelCase(string s)
		{
			return Regex.Replace(s, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}
		public static IEnumerable<SelectListItem> RegistrationTypes()
		{
			var cv = new CodeValueController();
			return QueryModel.ConvertToSelect(cv.RegistrationTypes(), "Id");
		}
		public string NewMeetingTime
		{
			get
			{
				var sc = org.OrgSchedules.FirstOrDefault(); // SCHED
				if (sc != null && sc.SchedTime != null)
					return sc.SchedTime.ToString2("t");
				return "08:00 AM";
			}
		}
		public DateTime NewMeetingDate
		{
			get
			{
				var sc = org.OrgSchedules.FirstOrDefault(); // SCHED
				if (sc != null && sc.SchedTime != null && sc.SchedDay < 9)
				{
					var d = Util.Now.Date;
					d = d.AddDays(-(int)d.DayOfWeek); // prev sunday
					d = d.AddDays(sc.SchedDay ?? 0);
					if (d > Util.Now.Date)
						d = d.AddDays(-7);
					return d;
				}
				return Util.Now.Date;
			}
		}
		private Settings _RegSettings;
		public Settings RegSettings
		{
			get
			{
				if (_RegSettings == null)
				{
					_RegSettings = new Settings(org.RegSetting, DbUtil.Db, org.OrganizationId);
					_RegSettings.org = org;
				}
				return _RegSettings;
			}
		}
		public void SendVolunteerReminders(bool sendall, Controller controller)
		{
			var Db = DbUtil.Db;
			var setting = RegSettings;
			var currmembers = from om in org.OrganizationMembers
							  where (om.Pending ?? false) == false
							  where om.MemberTypeId != CmsData.Codes.MemberTypeCode.InActive
							  where org.Attends.Any(a => (a.MeetingDate <= DateTime.Today.AddDays(7) || sendall)
								  && a.MeetingDate >= DateTime.Today
								  && (a.Commitment == AttendCommitmentCode.Attending || a.Commitment == AttendCommitmentCode.Substitute)
								  && a.PeopleId == om.PeopleId)
							  select om;

			var subject = Util.PickFirst(setting.ReminderSubject, "no subject");
			var message = Util.PickFirst(setting.ReminderBody, "no body");
			if (subject == "no subject" || message == "no body")
				throw new Exception("no subject or body");
			var notify = Db.StaffPeopleForOrg(org.OrganizationId).FirstOrDefault();
			if (notify == null)
				throw new Exception("no notify person");

			foreach (var om in currmembers)
			{
				var q = from a in org.Attends
						where a.PeopleId == om.PeopleId
						where a.Commitment == AttendCommitmentCode.Attending || a.Commitment == AttendCommitmentCode.Substitute
						where a.MeetingDate >= DateTime.Today
						orderby a.MeetingDate
						select a.MeetingDate;
				if (!q.Any())
					continue;
				var details = CmsController.RenderPartialViewToString(controller, "VolunteerCommitmentsSummary", q);

				var OrganizationName = org.OrganizationName;

				subject = Util.PickFirst(setting.ReminderSubject, "no subject");
				message = Util.PickFirst(setting.ReminderBody, "no body");

				string Location = org.Location;
				message = OnlineRegModel.MessageReplacements(om.Person, null, OrganizationName, Location, message);

				message = message.Replace("{phone}", org.PhoneNumber.FmtFone7());
				message = message.Replace("{details}", details.ToString());

				Db.Email(notify.FromEmail, om.Person, subject, message);
			}
		}
		public void SendEventReminders()
		{
			var Db = DbUtil.Db;
			var setting = RegSettings;
			var currmembers = from om in org.OrganizationMembers
							  where (om.Pending ?? false) == false
							  where om.MemberTypeId != CmsData.Codes.MemberTypeCode.InActive
							  select om;

			string noSubject = "no subject";
			const string noBody = "no body";

			var subject = Util.PickFirst(setting.ReminderSubject, noSubject);
			var message = Util.PickFirst(setting.ReminderBody, noBody);
			if (subject == noSubject || message == noBody)
				throw new Exception("no subject or body");
			var notify = Db.StaffPeopleForOrg(org.OrganizationId).FirstOrDefault();
			if (notify == null)
				throw new Exception("no notify person");

			foreach (var om in currmembers)
			{
				var details = PrepareSummaryText2(Db, om, setting);
				var OrganizationName = org.OrganizationName;

				subject = Util.PickFirst(setting.ReminderSubject, noSubject);
				message = Util.PickFirst(setting.ReminderBody, noBody);

				string Location = org.Location;
				message = OnlineRegModel.MessageReplacements(om.Person, null, OrganizationName, Location, message);

				message = message.Replace("{phone}", org.PhoneNumber.FmtFone7());
				message = message.Replace("{details}", details);

				Db.Email(notify.FromEmail, om.Person, subject, message);
			}
		}
		private string PrepareSummaryText2(CMSDataContext Db, OrganizationMember om, Settings setting)
		{
			var org = om.Organization;
			var person = om.Person;
			var sb = new StringBuilder();
			sb.Append("<table>");
			sb.AppendFormat("<tr><td>Org:</td><td>{0}</td></tr>\n", org.OrganizationName);
			sb.AppendFormat("<tr><td>First:</td><td>{0}</td></tr>\n", person.PreferredName);
			sb.AppendFormat("<tr><td>Last:</td><td>{0}</td></tr>\n", person.LastName);

			foreach (var ask in setting.AskItems)
			{
				if (ask.Type == "AskDropdown")
				{
					var option = ((AskDropdown)ask).list.Where(mm => om.OrgMemMemTags.Any(mt => mt.MemberTag.Name == mm.SmallGroup)).ToList();
					if (option.Any())
						sb.AppendFormat("<tr><td>{1}:</td><td>{0}</td></tr>\n", option.First().Description,
										Util.PickFirst(((AskDropdown)ask).Label, "Options"));
				}
				else if (ask.Type == "AskCheckboxes")
				{
					var option = ((AskCheckboxes)ask).list.Where(mm => om.OrgMemMemTags.Any(mt => mt.MemberTag.Name == mm.SmallGroup)).ToList();
					if (option.Any())
					{
						var label = ((AskCheckboxes)ask).Label;
						foreach (var m in option)
						{
							var row = "<tr><td>{0}</td><td>{1}</td></tr>\n".Fmt(label, m.Description);
							sb.AppendFormat(row);
							label = string.Empty;
						}
					}
				}
			}
			sb.Append("</table>");
			return sb.ToString();
		}
	}
}
