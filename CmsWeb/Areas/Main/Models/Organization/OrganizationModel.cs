using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;

namespace CMSWeb.Models.OrganizationPage
{
    public class OrganizationModel
    {
        public CmsData.Organization org;
        public OrganizationModel(int id)
        {
            org = DbUtil.Db.LoadOrganizationById(id);
            MemberModel = new OrganizationMemberModel(id, 0, OrganizationMemberModel.GroupSelect.Active);
        }
        public OrganizationMemberModel MemberModel;
        
        private CodeValueController cv = new CodeValueController();

        public string Campus { get { return cv.AllCampuses0().ItemValue(org.CampusId ?? 0); } }
        public string OrgStatus { get { return cv.OrganizationStatusCodes().ItemValue(org.OrganizationStatusId); } }
        public string LeaderType { get { return CodeValueController.MemberTypeCodes0().ItemValue(org.LeaderMemberTypeId ?? 0); } }
        public string EntryPoint { get { return cv.EntryPoints().ItemValue(org.EntryPointId ?? 0); } }
        public string Gender { get { return cv.GenderCodes().ItemValue(org.GenderId ?? 0); } }
        public string SchedDay { get { return DaysOfWeek().Single(d => d.Value.ToInt() == (org.SchedDay ?? 0)).Text; } }
        public string SchedTime { get { return org.SchedTime.Value.ToShortTimeString(); } }
        public string AttendanceOverlap { get { return org.AllowAttendOverlap == true ? "checked='checked'" : ""; } }
        public string ClassFilled { get { return org.ClassFilled == true ? "checked='checked'" : ""; } }
        public string CanSelfCheckin { get { return org.CanSelfCheckin == true ? "checked='checked'" : ""; } }
        public string AllowNonCampusCheckin { get { return org.AllowNonCampusCheckIn == true ? "checked='checked'" : ""; } }
        public string AttendTrkLevel { get { return cv.AttendanceTrackLevelCodes().ItemValue(org.AttendTrkLevelId); } }
        public string AttendClassification { get { return cv.AttendanceClassifications().ItemValue(org.AttendClassificationId); } }
        public string SecurityType { get { return cv.SecurityTypeCodes().ItemValue(org.SecurityTypeId); } }

        public string DivisionsTitle()
        {
            return "";
        }
        public void UpdateOrganization()
        {
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, org);
        }
        public static List<SelectListItem> DaysOfWeek()
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
        public static IEnumerable<SelectListItem> CampusList()
        { 
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.AllCampuses0(), "Id"); 
        }
        public static IEnumerable<SelectListItem> OrgStatusList()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.OrganizationStatusCodes(), "Id");
        }
        public static IEnumerable<SelectListItem> LeaderTypeList()
        {
            return QueryModel.ConvertToSelect(CodeValueController.MemberTypeCodes0(), "Id");
        }
        public static IEnumerable<SelectListItem> EntryPointList()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.EntryPoints(), "Id");
        }
        public static IEnumerable<SelectListItem> GenderList()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.GenderCodes(), "Id");
        }
        public static IEnumerable<SelectListItem> AttendTrkLevelList()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.AttendanceTrackLevelCodes(), "Id");
        }
        public static IEnumerable<SelectListItem> AttendClassificationList()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.AttendanceClassifications(), "Id");
        }
        public static IEnumerable<SelectListItem> SecurityTypeList()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.SecurityTypeCodes(), "Id");
        }
    }
}
