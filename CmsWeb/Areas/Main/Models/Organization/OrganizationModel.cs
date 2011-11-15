using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
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
        public OrganizationModel(int? id, int[] groups)
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
            MemberModel = new MemberModel(id, groups, MemberModel.GroupSelect.Active, String.Empty);
        }
        public MemberModel MemberModel;

        private CodeValueController cv = new CodeValueController();

        //public void UpdateOrganization()
        //{
        //    org.SetTagString(DbUtil.Db, DivisionsList);
        //    if (org.DivisionId == 0)
        //        org.DivisionId = null;
        //    var divorg = org.DivOrgs.SingleOrDefault(d => d.DivId == org.DivisionId);
        //    if (divorg == null && org.DivisionId.HasValue)
        //        org.DivOrgs.Add(new DivOrg { DivId = org.DivisionId.Value });
        //    if (org.CampusId == 0)
        //        org.CampusId = null;
        //    DbUtil.Db.SubmitChanges();
        //    DbUtil.Db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, org);
        //}
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
        public IEnumerable<SelectListItem> Tags()
        {
            var cv = new CodeValueController();
            var tg = QueryModel.ConvertToSelect(cv.UserTags(Util.UserPeopleId), "Id").ToList();
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
                    return sc.SchedTime.Value.ToShortTimeString();
                return "8:00 AM";
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
        private RegSettings _RegSettings;
        public RegSettings RegSettings
        {
            get
            {
                if (_RegSettings == null)
                {
                    _RegSettings = new RegSettings(org.RegSetting, DbUtil.Db, org.OrganizationId);
                    _RegSettings.org = org;
                }
                return _RegSettings;
            }
        }
    }
}
