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
    public class GroupsModel
    {
        private int? _OrgId;
        public int? OrgId
        {
            get
            {
                if (_OrgId != null)
                    return _OrgId;
                _OrgId = DbUtil.Db.UserPreference("GroupsOrgId", "0").ToInt2();
                return _OrgId;
            }
            set
            {
                _OrgId = value;
                DbUtil.Db.SetUserPreference("GroupsOrgId", value);
            }
        }

        private CmsData.Organization _org;
        public CmsData.Organization Org
        {
            get
            {
                if (_org == null)
                {
                    _org = DbUtil.Db.Organizations.SingleOrDefault(p => p.OrganizationId == OrgId);
                }
                return _org;
            }
        }
        private int? _DivId;
        public int? DivId
        {
            get
            {
                if (_DivId != null)
                    return _DivId;
                _DivId = DbUtil.Db.UserPreference("GroupsDivId", "0").ToInt2();
                return _DivId;
            }
            set
            {
                _DivId = value;
                DbUtil.Db.SetUserPreference("GroupsDivId", value);
            }
        }

        public string TargetGroupName { get; set; }

        private bool? _FilterUnassigned;
        public bool FilterUnassigned
        {
            get
            {
                if (_FilterUnassigned != null)
                    return _FilterUnassigned.Value;
                _FilterUnassigned = DbUtil.Db.UserPreference("GroupsFilterUnassigned", "false").ToBool();
                return _FilterUnassigned.Value;
            }
            set
            {
                _FilterUnassigned = value;
                DbUtil.Db.SetUserPreference("GroupsFilterUnassigned", value);
            }
        }

        private bool? _NormalMembersOnly;
        public bool NormalMembersOnly
        {
            get
            {
                if (_NormalMembersOnly != null)
                    return _NormalMembersOnly.Value;
                _NormalMembersOnly = DbUtil.Db.UserPreference("GroupsNormalMembersOnly", "false").ToBool();
                return _NormalMembersOnly.Value;
            }
            set
            {
                _NormalMembersOnly = value;
                DbUtil.Db.SetUserPreference("GroupsNormalMembersOnly", value);
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
        public IEnumerable<MemberInfo> FetchMembers()
        {
            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.OrganizationId == OrgId
                    where !NormalMembersOnly || om.MemberTypeId == (int)OrganizationMember.MemberTypeCode.Member
                    let team = om.OrgMemMemTags.Count(mt => mt.MemberTag.Name.StartsWith(Prefix)) > 0
                    let recreg = om.Person.RecRegs.Single()
                    where !FilterUnassigned || team == false
                    select new MemberInfo
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
                        MemberStatus = "p=" + (om.Person.MemberStatusId == 10 ? "Yes" : "no") + ", hh=" + ((DbUtil.Db.OneHeadOfHouseholdIsMember(om.Person.FamilyId) ?? false) ? "Yes" : "no"),
                        TeamName = om.OrgMemMemTags.Where(mt => mt.MemberTag.Name.StartsWith(Prefix)).Select(mt => mt.MemberTag.Name).SingleOrDefault(),
                        FeePaid = om.Amount > 0,
                        Request = om.Request,
                        ShirtSize = recreg.ShirtSize,
                        Hash = om.Person.HashNum.Value,
                    };
            q = ApplySort(q);
            return q;
        }
        public IEnumerable<MemberInfo> FetchAll()
        {
            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.Organization.DivOrgs.Any(di => di.DivId == DivId)
                    let team = om.OrgMemMemTags.Count(mt => mt.MemberTag.Name.StartsWith(Prefix)) > 0
                    let recreg = om.Person.RecRegs.SingleOrDefault()
                    where recreg != null
                    select new MemberInfo
                    {
                        PeopleId = om.PeopleId,
                        OrgId = om.OrganizationId,
                        Name = om.Person.Name,
                        Name2 = om.Person.Name2,
                        BDay = om.Person.BirthDay,
                        BMon = om.Person.BirthMonth,
                        BYear = om.Person.BirthYear,
                        MemberType = om.MemberType.Description,
                        MemberStatus = "p=" + (om.Person.MemberStatusId == 10 ? "Yes" : "no") + ", hh=" + ((DbUtil.Db.OneHeadOfHouseholdIsMember(om.Person.FamilyId) ?? false) ? "Yes" : "no"),
                        TeamName = om.OrgMemMemTags.Where(mt => mt.MemberTag.Name.StartsWith(Prefix)).Select(mt => mt.MemberTag.Name).SingleOrDefault(),
                        FeePaid = om.Amount > 0,
                        Request = om.Request,
                        ShirtSize = recreg.ShirtSize,
                        Hash = om.Person.HashNum.Value,
                    };
            q = ApplySort(q);
            return q;
        }
        public IEnumerable<SelectListItem> ShirtSizes()
        {
            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.Organization.DivOrgs.Any(di => di.DivId == DivId)
                    let recreg = om.Person.RecRegs.SingleOrDefault()
                    where recreg != null
                    group om by recreg.ShirtSize into g
                    select new SelectListItem
                    {
                        Text = g.Key,
                        Value = g.Count().ToString()
                    };
            return q;
        }

        private IQueryable<MemberInfo> ApplySort(IQueryable<MemberInfo> q)
        {
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
                    case "Church":
                        q = q.OrderBy(i => i.MemberStatus);
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
                    case "Church":
                        q = q.OrderByDescending(i => i.MemberStatus);
                        break;
                }
            return q;
        }
        private string _Prefix;
        public string Prefix
        {
            get
            {
                return _Prefix ?? string.Empty;
            }
            set
            {
                _Prefix = value;
            }
        }
        public void AssignToGroup()
        {
            foreach (var pid in selected)
            {
                var team = (from mt in DbUtil.Db.OrgMemMemTags
                            where mt.PeopleId == pid && mt.OrganizationMember.OrganizationId == OrgId
                            where mt.MemberTag.Name.StartsWith(Prefix)
                            select mt).SingleOrDefault();
                if (team != null)
                {
                    DbUtil.Db.OrgMemMemTags.DeleteOnSubmit(team);
                    DbUtil.Db.SubmitChanges();
                }
                var jointeam = DbUtil.Db.MemberTags.SingleOrDefault(mt => mt.Name == TargetGroupName && mt.OrgId == OrgId);
                team = new OrgMemMemTag
                {
                    MemberTagId = jointeam.Id,
                    OrgId = OrgId.Value,
                    PeopleId = pid
                };
                DbUtil.Db.OrgMemMemTags.InsertOnSubmit(team);
                DbUtil.Db.SubmitChanges();
            }
        }
        public IEnumerable<SelectListItem> Divisions()
        {
            var q = from div in DbUtil.Db.Divisions
                    where div.Organizations.Count() > 0
                    orderby div.Name
                    select new SelectListItem
                    {
                        Text = div.Name,
                        Value = div.Id.ToString(),
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(Select division)", Value = "0", Selected = true });
            return list;
        }
        public IEnumerable<SelectListItem> Organizations()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.DivisionId == DivId
                    orderby o.OrganizationName
                    select new SelectListItem
                    {
                        Text = o.OrganizationName,
                        Value = o.OrganizationId.ToString(),
                    };
            var list = q.ToList();
            if (list.Count == 0)
                list.Insert(0, new SelectListItem { Text = "(Select division First)", Value = "0", Selected = true });
            else
                list.Insert(0, new SelectListItem { Text = "(Select organization)", Value = "0", Selected = true });
            return list;
        }
        public IEnumerable<SelectListItem> TargetGroups()
        {
            var q = from mt in DbUtil.Db.MemberTags
                    where mt.OrgId == OrgId
                    where mt.Name.StartsWith(Prefix)
                    orderby mt.Name
                    select new SelectListItem
                    {
                        Text = mt.Name,
                    };
            return q;
        }

        public class MemberInfo
        {
            public bool IsSelected { get; set; }
            public string Checked
            {
                get { return IsSelected ? "checked='checked'" : ""; }
            }
            public int? PeopleId { get; set; }
            public int? OrgId { get; set; }
            public string Name { get; set; }
            public string Name2 { get; set; }
            public string MemberType { get; set; }
            public string MemberStatus { get; set; }
            public string TeamName { get; set; }
            public bool? FeePaid { get; set; }
            public string Request { get; set; }
            public string ShirtSize { get; set; }
            public int? BDay { get; set; }
            public int? BMon { get; set; }
            public int? BYear { get; set; }
            public string Birthday
            {
                get { return Util.FormatBirthday(BYear, BMon, BDay); }
            }
            public int? Hash { get; set; }
            public DateTime? Uploaded { get; set; }
        }
    }
}
