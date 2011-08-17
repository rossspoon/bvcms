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

        public void UpdateOrganization()
        {
            org.SetTagString(DbUtil.Db, DivisionsList);
            if (org.DivisionId == 0)
                org.DivisionId = null;
            var divorg = org.DivOrgs.SingleOrDefault(d => d.DivId == org.DivisionId);
            if (divorg == null && org.DivisionId.HasValue)
                org.DivOrgs.Add(new DivOrg { DivId = org.DivisionId.Value });
            if (org.CampusId == 0)
                org.CampusId = null;
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, org);
        }
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
        public string DivisionsList { get; set; }
        public List<SelectListItem> DivisionPickList()
        {
            var q1 = from d in DbUtil.Db.DivOrgs
                     where d.OrgId == OrganizationId
                     orderby d.Division.Name
                     select d.Division.Name;
            var q2 = from p in DbUtil.Db.Programs
                     from d in p.Divisions
                     where !q1.Contains(d.Name)
                     orderby d.Name
                     select d.Name;
            var list = q1.Select(name => new SelectListItem { Text = name, Selected = true }).ToList();
            list.AddRange(q2.Select(name => new SelectListItem { Text = name }).ToList());
            return list;
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
        public void ValidateSettings(ModelStateDictionary ModelState)
        {
            //const string STR_NotFormedCorrectly = "not formed correctly";

            //if (org.ShirtSizes.HasValue())
            //    try
            //    {
            //        var q = from s in (org.ShirtSizes).Split(',')
            //                let a = s.Split('=')
            //                select new { Text = a[1].Trim(), Value = a[0].Trim() };
            //        var list = q.ToList();

            //    }
            //    catch (Exception)
            //    {
            //        ModelState.AddModelError("shirtsizes", STR_NotFormedCorrectly);
            //    }

            //if (org.AskOptions.HasValue())
            //    try
            //    {
            //        var q = from s in (org.AskOptions).Split(',')
            //                let a = s.Split('=')
            //                let amt = a.Length > 1 ? " ({0:C})".Fmt(decimal.Parse(a[1])) : ""
            //                select new { Text = a[0].Trim() + amt, Value = a[0].Trim() };
            //        var list = q.ToList();

            //    }
            //    catch (Exception)
            //    {
            //        ModelState.AddModelError("askoptions", STR_NotFormedCorrectly);
            //    }

            //if (org.ExtraOptions.HasValue())
            //    try
            //    {
            //        var q = from s in org.ExtraOptions.Split(',')
            //                where s.HasValue()
            //                let a = s.Split('=')
            //                select new { Text = a[1].Trim(), Value = a[0].ToInt().ToString() };
            //        var list = q.ToList();
            //    }
            //    catch (Exception)
            //    {
            //        ModelState.AddModelError("extraoptions", STR_NotFormedCorrectly);
            //    }

            //if (org.GradeOptions.HasValue())
            //    try
            //    {
            //        var q = from s in (org.GradeOptions).Split(',')
            //                where s.HasValue()
            //                let a = s.Split('=')
            //                select new { Text = a[1].Trim(), Value = int.Parse(a[0]) };
            //        var list = q.ToList();
            //    }
            //    catch (Exception)
            //    {
            //        ModelState.AddModelError("gradeoptions", STR_NotFormedCorrectly);
            //    }

            //if (org.AgeFee.HasValue())
            //    try
            //    {
            //        var q = from o in org.AgeFee.Split(',')
            //                let b = o.Split('=')
            //                let a = b[0].Split('-')
            //                select new { startage = int.Parse(a[0]), endage = int.Parse(a[1]), amt = decimal.Parse(b[1]) };
            //        var list = q.ToList();
            //    }
            //    catch (Exception)
            //    {
            //        ModelState.AddModelError("agefee", STR_NotFormedCorrectly);
            //    }

            //if (org.AgeGroups.HasValue())
            //    try
            //    {
            //        var q = from o in (org.AgeGroups ?? string.Empty).Split(',')
            //                where o.HasValue()
            //                let b = o.Split('=')
            //                let a = b[0].Split('-')
            //                select new
            //                {
            //                    StartAge = a[0].ToInt(),
            //                    EndAge = a[1].ToInt(),
            //                    Name = b[1]
            //                };
            //        var list = q.ToList();
            //    }
            //    catch (Exception)
            //    {
            //        ModelState.AddModelError("agegroups", STR_NotFormedCorrectly);
            //    }

            //if (org.YesNoQuestions.HasValue())
            //    try
            //    {
            //        var q = from s in (org.YesNoQuestions ?? string.Empty).Split(',')
            //                let a = s.Split('=')
            //                where s.HasValue()
            //                select new { name = a[0].Trim(), desc = a[1] };
            //        var list = q.ToList();
            //    }
            //    catch (Exception)
            //    {
            //        ModelState.AddModelError("yesnoquestions", STR_NotFormedCorrectly);
            //    }
            //if (org.NotifyIds.HasValue())
            //{
            //    var a = org.NotifyIds.SplitStr(",").Select(ss => ss.ToInt()).ToArray();
            //    var q = from p in DbUtil.Db.People
            //            where a.Contains(p.PeopleId)
            //            select p.PeopleId;
            //    var g = from i in a
            //            join p in q on i equals p into j
            //            from pid in j.DefaultIfEmpty()
            //            where pid == 0
            //            select i;
            //    if (g.Count() > 0)
            //        ModelState.AddModelError("NotifyIds", "Id: " + g.First() + " not found");
            //}
            //else if (org.RegistrationTypeId > RegistrationEnum.None)
            //    ModelState.AddModelError("NotifyIds", "Need a PeopleId in the NotifyIds field");

            //if (org.DonationFundId.HasValue)
            //{
            //    var f = DbUtil.Db.ContributionFunds.SingleOrDefault(ff => ff.FundId == org.DonationFundId);
            //    if (f == null)
            //        ModelState.AddModelError("DonationFundId", "fund not found");
            //}

        }
        public IEnumerable<SelectListItem> Divisions()
        {
            return QueryModel.ConvertToSelect(cv.AllOrgDivTags(), "Id");
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
        public IEnumerable<SelectListItem> AttendTrkLevelList()
        {
            return QueryModel.ConvertToSelect(cv.AttendanceTrackLevelCodes(), "Id");
        }
        public IEnumerable<SelectListItem> AttendClassificationList()
        {
            return QueryModel.ConvertToSelect(cv.AttendanceClassifications(), "Id");
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
        public IEnumerable<SelectListItem> RegistrationTypes()
        {
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
    }
}
