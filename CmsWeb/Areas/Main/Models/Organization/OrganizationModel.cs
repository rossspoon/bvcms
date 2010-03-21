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
        public CmsData.Organization org { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationModel(int id)
        {
            OrganizationId = id;
            org = DbUtil.Db.LoadOrganizationById(id);
            MemberModel = new OrganizationMemberModel(id, 0, OrganizationMemberModel.GroupSelect.Active);
        }
        public OrganizationMemberModel MemberModel;
        
        private CodeValueController cv = new CodeValueController();

        public string DivisionsTitle()
        {
            return "";
        }
        public void UpdateOrganization()
        {
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
            var items = CodeValueController.MemberTypeCodes0().Cast<CodeValueItem>();
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
    }
}
