using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;
using System.Data.Linq.SqlClient;
using System.Data.Linq;

namespace CMSWeb.Models.PersonPage
{
    public class MemberInfo
    {
        private static CodeValueController cv = new CodeValueController();
        public int PeopleId { get; set; }

        public int? StatementOptionId { get; set; }
        public string StatementOption { get { return cv.EnvelopeOptions().ItemValue(StatementOptionId ?? 0); } }
        public int? EnvelopeOptionId { get; set; }
        public string EnvelopeOption { get { return cv.EnvelopeOptions().ItemValue(EnvelopeOptionId ?? 0); } }
        public int? DecisionTypeId { get; set; }
        public string DecisionType { get { return cv.DecisionCodes().ItemValue(DecisionTypeId ?? 0); } }
        public DateTime? DecisionDate { get; set; }
        public int JoinTypeId { get; set; }
        public string JoinType { get { return cv.JoinTypes().ItemValue(JoinTypeId); } }
        public DateTime? JoinDate { get; set; }
        public int? BaptismTypeId { get; set; }
        public string BaptismType { get { return cv.BaptismTypes().ItemValue(BaptismTypeId ?? 0); } }
        public int? BaptismStatusId { get; set; }
        public string BaptismStatus { get { return cv.BaptismStatuses().ItemValue(BaptismStatusId ?? 0); } }
        public DateTime? BaptismDate { get; set; }
        public DateTime? BaptismSchedDate { get; set; }
        public int DropTypeId { get; set; }
        public string DropType { get { return cv.DropTypes().ItemValue(DropTypeId); } }
        public DateTime? DropDate { get; set; }
        public string NewChurch { get; set; }
        public string PrevChurch { get; set; }
        public int? NewMemberClassStatusId { get; set; }
        public string NewMemberClassStatus { get { return cv.DiscoveryClassStatusCodes().ItemValue(NewMemberClassStatusId ?? 0); } }
        public DateTime? NewMemberClassDate { get; set; }
        public int MemberStatusId { get; set; }
        public string MemberStatus { get { return cv.MemberStatusCodes().ItemValue(MemberStatusId); } }

        public static MemberInfo GetMemberInfo(int? id)
        {
            var q = from p in DbUtil.Db.People
                    where p.PeopleId == id
                    select new MemberInfo
                    {
                        PeopleId = p.PeopleId,
                        BaptismSchedDate = p.BaptismSchedDate,
                        BaptismDate = p.BaptismDate,
                        DecisionDate = p.DecisionDate,
                        DropDate = p.DropDate,
                        DropTypeId = p.DropCodeId,
                        JoinTypeId = p.JoinCodeId,
                        NewChurch = p.OtherNewChurch,
                        PrevChurch = p.OtherPreviousChurch,
                        NewMemberClassDate = p.DiscoveryClassDate,
                        MemberStatusId = p.MemberStatusId,
                        JoinDate = p.JoinDate,
                        BaptismTypeId = p.BaptismTypeId ?? 0,
                        BaptismStatusId = p.BaptismStatusId ?? 0,
                        DecisionTypeId = p.DecisionTypeId ?? 0,
                        EnvelopeOptionId = p.EnvelopeOptionsId ?? 0,
                        StatementOptionId = p.ContributionOptionsId ?? 0,
                        NewMemberClassStatusId = p.DiscoveryClassStatusId ?? 0,
                    };
            return q.Single();
        }

        public void UpdateMember()
        {
            if (NewMemberClassStatusId == 0)
                NewMemberClassStatusId = null;
            if (StatementOptionId == 0)
                StatementOptionId = null;
            if (DecisionTypeId == 0)
                DecisionTypeId = null;
            if (BaptismStatusId == 0)
                BaptismStatusId = null;
            if (EnvelopeOptionId == 0)
                EnvelopeOptionId = null;
            if (BaptismTypeId == 0)
                BaptismTypeId = null;
            var p = DbUtil.Db.LoadPersonById(PeopleId);
            p.BaptismSchedDate = BaptismSchedDate;
            p.BaptismTypeId = BaptismTypeId;
            p.BaptismStatusId = BaptismStatusId;
            p.BaptismDate = BaptismDate;
            p.DecisionDate = DecisionDate;
            p.DecisionTypeId = DecisionTypeId;
            p.DropDate = DropDate;
            p.DropCodeId = DropTypeId;
            p.EnvelopeOptionsId = EnvelopeOptionId;
            p.ContributionOptionsId = StatementOptionId;
            p.JoinCodeId = JoinTypeId;
            p.OtherNewChurch = NewChurch;
            p.OtherPreviousChurch = PrevChurch;
            p.DiscoveryClassDate = NewMemberClassDate;
            p.DiscoveryClassStatusId = NewMemberClassStatusId;
            p.MemberStatusId = MemberStatusId;
            p.MemberProfileAutomation();
            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Updated Person: {0}".Fmt(p.Name), false);
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, p);
        }
        public static IEnumerable<SelectListItem> MemberStatuses()
        {
            return QueryModel.ConvertToSelect(cv.MemberStatusCodes(), "Id");
        }
        public static IEnumerable<SelectListItem> BaptismStatuses()
        {
            return QueryModel.ConvertToSelect(cv.BaptismStatuses(), "Id");
        }
        public static IEnumerable<SelectListItem> DecisionCodes()
        {
            return QueryModel.ConvertToSelect(cv.DecisionCodes(), "Id");
        }
        public static IEnumerable<SelectListItem> EnvelopeOptions()
        {
            return QueryModel.ConvertToSelect(cv.EnvelopeOptions(), "Id");
        }
        public static IEnumerable<SelectListItem> JoinTypes()
        {
            return QueryModel.ConvertToSelect(cv.JoinTypes(), "Id");
        }
        public static IEnumerable<SelectListItem> BaptismTypes()
        {
            return QueryModel.ConvertToSelect(cv.BaptismTypes(), "Id");
        }
        public static IEnumerable<SelectListItem> DropTypes()
        {
            return QueryModel.ConvertToSelect(cv.DropTypes(), "Id");
        }
        public static IEnumerable<SelectListItem> NewMemberClassStatuses()
        {
            return QueryModel.ConvertToSelect(cv.DiscoveryClassStatusCodes(), "Id");
        }

    }
}
