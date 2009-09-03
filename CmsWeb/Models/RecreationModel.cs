using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Web.Mvc;
using CMSPresenter;

namespace CMSWeb.Models
{
    public class RecreationModel
    {
        private int? _AgeDivId;
        public int? AgeDivId
        {
            get
            {
                if (_AgeDivId != null)
                    return _AgeDivId;
                _AgeDivId = DbUtil.Db.UserPreference("AgeDivId", "0").ToInt2();
                return _AgeDivId;
            }
            set
            {
                _AgeDivId = value;
                DbUtil.Db.SetUserPreference("AgeDivId", value);
            }
        }

        private CmsData.Organization _AgeDiv;
        public CmsData.Organization AgeDiv
        {
            get
            {
                if (_AgeDiv == null)
                {
                    _AgeDiv = DbUtil.Db.Organizations.SingleOrDefault(p => p.OrganizationId == AgeDivId);
                }
                return _AgeDiv;
            }
        }
        private int? _LeagueId;
        public int? LeagueId
        {
            get
            {
                if (_LeagueId != null)
                    return _LeagueId;
                _LeagueId = DbUtil.Db.UserPreference("LeagueId", "0").ToInt2();
                return _LeagueId;
            }
            set
            {
                _LeagueId = value;
                DbUtil.Db.SetUserPreference("LeagueId", value);
            }
        }

        public string TargetTeamName { get; set; }

        private bool? _FilterUnassigned;
        public bool FilterUnassigned
        {
            get
            {
                if (_FilterUnassigned != null)
                    return _FilterUnassigned.Value;
                _FilterUnassigned = DbUtil.Db.UserPreference("RecFilterUnassigned", "false").ToBool();
                return _FilterUnassigned.Value;
            }
            set
            {
                _FilterUnassigned = value;
                DbUtil.Db.SetUserPreference("RecFilterUnassigned", value);
            }
        }

        private bool? _NormalMembersOnly;
        public bool NormalMembersOnly
        {
            get
            {
                if (_NormalMembersOnly != null)
                    return _NormalMembersOnly.Value;
                _NormalMembersOnly = DbUtil.Db.UserPreference("RecNormalMembersOnly", "false").ToBool();
                return _NormalMembersOnly.Value;
            }
            set
            {
                _NormalMembersOnly = value;
                DbUtil.Db.SetUserPreference("RecNormalMembersOnly", value);
            }
        }

        private int[] _Selected;
        public int[] selected
        {
            get
            {
                if (_Selected == null)
                    _Selected = new int[0];
                return _Selected;
            }
            set
            {
                _Selected = value;
            }
        }

        private string _Sort = "Name";
        public string Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }
        private string _Dir = "asc";
        public string Dir
        {
            get { return _Dir; }
            set { _Dir = value; }
        }

        public IEnumerable<RecReg> FetchWannabeCoaches()
        {
            var q = from r in DbUtil.Db.RecRegs
                    where r.IsDocument.Value
                    select r;
            var list = q.ToList();
            var list2 = new List<RecReg>();
            foreach (var r in list)
            {
                var i = ImageData.DbUtil.Db.Images.Single(im => im.Id == r.ImgId);
                if (i.InterestedInCoaching())
                    list2.Add(r);
            }
            return list2;
        }
        public IEnumerable<ParticipantInfo> FetchParticipants()
        {
            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.OrganizationId == AgeDivId
                    where !NormalMembersOnly || om.MemberTypeId == (int)OrganizationMember.MemberTypeCode.Member
                    let team = om.OrgMemMemTags.Count(mt => mt.MemberTag.Name.StartsWith("TM:")) > 0
                    let recreg = om.Person.RecRegs.Where(r =>
                        r.OrgId == om.OrganizationId && (r.Expired ?? false) == false)
                        .OrderByDescending(r => r.Uploaded).FirstOrDefault()
                    where !FilterUnassigned || team == false
                    select new ParticipantInfo
                    {
                        IsSelected = selected.Contains(om.PeopleId),
                        PeopleId = om.PeopleId,
                        OrgId = om.OrganizationId,
                        Name = om.Person.Name,
                        Name2 = om.Person.Name2,
                        BDay = om.Person.BirthDay,
                        BMon = om.Person.BirthMonth,
                        BYear = om.Person.BirthYear,
                        MemberType = om.MemberType.Description,
                        TeamName = om.OrgMemMemTags.Where(mt => mt.MemberTag.Name.StartsWith("TM:")).Select(mt => mt.MemberTag.Name).SingleOrDefault(),
                        Id = recreg.Id,
                        FeePaid = recreg.FeePaid ?? false,
                        Request = recreg.Request,
                        ShirtSize = recreg.ShirtSize,
                        Hash = om.Person.HashNum.Value,
                        Uploaded = recreg.Uploaded.Value
                    };
            if (Dir == "asc")
                switch (Sort)
                {
                    case "Mixed":
                        q = q.OrderBy(i => i.Hash);
                        break;
                    case "Name":
                        q = q.OrderBy(i => i.Name2);
                        break;
                    case "Team":
                        q = q.OrderBy(i => i.TeamName);
                        break;
                    case "Birthday":
                        q = from i in q
                            orderby i.BYear, i.BMon, i.BDay
                            select i;
                        break;
                    case "FeePaid":
                        q = q.OrderBy(i => i.FeePaid);
                        break;
                    case "ShirtSize":
                        q = q.OrderBy(i => i.ShirtSize);
                        break;
                    case "Request":
                        q = q.OrderBy(i => i.Request);
                        break;
                    case "Uploaded":
                        q = q.OrderBy(i => i.Uploaded);
                        break;
                    case "Type":
                        q = q.OrderBy(i => i.MemberType);
                        break;
                }
            else
                switch (Sort)
                {
                    case "Mixed":
                        q = q.OrderByDescending(i => i.Hash);
                        break;
                    case "Name":
                        q = q.OrderByDescending(i => i.Name2);
                        break;
                    case "Team":
                        q = q.OrderByDescending(i => i.TeamName);
                        break;
                    case "Birthday":
                        q = from i in q
                            orderby i.BYear descending, i.BMon descending, i.BDay descending
                            select i;
                        break;
                    case "FeePaid":
                        q = q.OrderByDescending(i => i.FeePaid);
                        break;
                    case "ShirtSize":
                        q = q.OrderByDescending(i => i.ShirtSize);
                        break;
                    case "Request":
                        q = q.OrderByDescending(i => i.Request);
                        break;
                    case "Uploaded":
                        q = q.OrderByDescending(i => i.Uploaded);
                        break;
                    case "Type":
                        q = q.OrderByDescending(i => i.MemberType);
                        break;
                }
            return q;
        }
        public IEnumerable<ParticipantInfo> FetchParticipants0()
        {
            var q = from recreg in DbUtil.Db.RecRegs
                    where (recreg.DivId ?? 0) == 0
                    where (recreg.Expired ?? false) == false
                    select new ParticipantInfo
                    {
                        IsSelected = selected.Contains(recreg.PeopleId ?? 0),
                        PeopleId = recreg.PeopleId,
                        Id = recreg.Id,
                        Hash = 0,
                        Uploaded = recreg.Uploaded.Value,
                        Name = recreg.PeopleId.HasValue? recreg.Person.Name : "",
                        Request = recreg.Request,
                        ShirtSize = recreg.ShirtSize,
                         FeePaid = recreg.FeePaid ?? false,
                          
                        
                    };
            if (Dir == "asc")
                switch (Sort)
                {
                    case "Uploaded":
                        q = q.OrderBy(i => i.Uploaded);
                        break;
                }
            else
                switch (Sort)
                {
                    case "Uploaded":
                        q = q.OrderByDescending(i => i.Uploaded);
                        break;
                }
            return q;
        }

        public void AssignToTeam()
        {
            foreach (var pid in selected)
            {
                var team = (from mt in DbUtil.Db.OrgMemMemTags
                            where mt.PeopleId == pid && mt.OrganizationMember.OrganizationId == AgeDivId
                            where mt.MemberTag.Name.StartsWith("TM:")
                            select mt).SingleOrDefault();
                if (team != null)
                {
                    DbUtil.Db.OrgMemMemTags.DeleteOnSubmit(team);
                    DbUtil.Db.SubmitChanges();
                }
                var jointeam = DbUtil.Db.MemberTags.SingleOrDefault(mt => mt.Name == TargetTeamName && mt.OrgId == AgeDivId);
                team = new OrgMemMemTag
                {
                    MemberTagId = jointeam.Id,
                    OrgId = AgeDivId.Value,
                    PeopleId = pid
                };
                DbUtil.Db.OrgMemMemTags.InsertOnSubmit(team);
                DbUtil.Db.SubmitChanges();
            }
        }
        public IEnumerable<SelectListItem> Leagues()
        {
            var q = from d in DbUtil.Db.Divisions
                    where d.RecAgeDivisions.Count() > 0
                    orderby d.Name
                    select new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.Id.ToString(),
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(Select League)", Value = "0", Selected = true });
            return list;
        }
        public IEnumerable<SelectListItem> AgeDivisions()
        {
            var q = from o in DbUtil.Db.RecAgeDivisions
                    where o.DivId == LeagueId
                    orderby o.Organization.OrganizationName
                    select new SelectListItem
                    {
                        Text = o.Organization.OrganizationName,
                        Value = o.OrgId.ToString(),
                    };
            var list = q.ToList();
            if (list.Count == 0)
                list.Insert(0, new SelectListItem { Text = "(Select League First)", Value = "0", Selected = true });
            else
                list.Insert(0, new SelectListItem { Text = "(Select Age Division)", Value = "0", Selected = true });
            return list;
        }
        public IEnumerable<SelectListItem> TargetTeams()
        {
            var q = from mt in DbUtil.Db.MemberTags
                    where mt.OrgId == AgeDivId
                    where mt.Name.StartsWith("TM:")
                    orderby mt.Name
                    select new SelectListItem
                    {
                        Text = mt.Name,
                    };
            return q;
        }
        public void DeleteRecReg(int id)
        {
            var r = DbUtil.Db.RecRegs.Single(vb => vb.Id == id);
            var img = ImageData.DbUtil.Db.Images.SingleOrDefault(i => i.Id == r.ImgId);
            DbUtil.Db.RecRegs.DeleteOnSubmit(r);
            DbUtil.Db.SubmitChanges();
            if (img != null)
            {
                ImageData.DbUtil.Db.Images.DeleteOnSubmit(img);
                ImageData.DbUtil.Db.SubmitChanges();
            }
        }
    }
}
