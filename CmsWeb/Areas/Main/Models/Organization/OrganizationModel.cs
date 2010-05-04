using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;
using System.Text.RegularExpressions;

namespace CMSWeb.Models.OrganizationPage
{
    public class OrganizationModel
    {
        public CmsData.Organization org { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationModel(int id)
        {
            OrganizationId = id;
            org = DbUtil.Db.LoadOrganizationById(id);
            MemberModel = new MemberModel(id, 0, MemberModel.GroupSelect.Active);
        }
        public MemberModel MemberModel;
        
        private CodeValueController cv = new CodeValueController();

        public string DivisionsTitle()
        {
            return "";
        }
        public void UpdateOrganization()
        {
            org.TagString = DivisionsList;
            var divorg = org.DivOrgs.SingleOrDefault(d => d.DivId == org.DivisionId);
            if (divorg == null && org.DivisionId.HasValue)
                org.DivOrgs.Add(new DivOrg { DivId = org.DivisionId.Value });
            if (org.CampusId == 0)
                org.CampusId = null;
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, org);
        }
        public List<SelectListItem> DaysOfWeek()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Sunday", Value = "0" },
                new SelectListItem { Text = "Monday", Value = "1" },
                new SelectListItem { Text = "Tuesday", Value = "2" },
                new SelectListItem { Text = "Wednesday", Value = "3" },
                new SelectListItem { Text = "Thursday", Value = "4" },
                new SelectListItem { Text = "Friday", Value = "5" },
                new SelectListItem { Text = "Saturday", Value = "6" },
            };
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
                if (org.SchedTime != null)
                    return org.SchedTime.Value.ToShortTimeString();
                return "8:00 AM";
            }
        }
        public DateTime NewMeetingDate
        {
            get
            {
                if (org.SchedTime != null)
                {
                    var d = Util.Now.Date;
                    d = d.AddDays(-(int)d.DayOfWeek); // prev sunday
                    d = d.AddDays(org.SchedDay ?? 0);
                    if (d > Util.Now.Date)
                        d = d.AddDays(-7);
                    return d;
                }
                return Util.Now.Date;
            }
        }
    }
}
