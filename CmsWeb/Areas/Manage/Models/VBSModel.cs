/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using CmsData;
using System.Web.Mvc;

namespace CMSWeb.Models
{
    public interface IVBSBindable
    {
        string Sort { get; set; }
        string Dir { get; set; }
        int? Page { get; set; }
        int? PageSize { get; set; }
        string UserInfo { get; set; }
        string Grade { get; set; }
        bool NewAppsOnly { get; set; }
    }
    public class VBSModel : IVBSBindable
    {
        public string Sort { get; set; }
        public string Dir { get; set; }

        private int? _Page;
        public int? Page
        {
            get { return _Page ?? 1; }
            set { _Page = value; }
        }
        public int StartRow
        {
            get { return (Page.Value - 1) * PageSize.Value; }
        }
        public int? PageSize
        {
            get { return DbUtil.Db.UserPreference("PageSize", "10").ToInt(); }
            set
            {
                if (value.HasValue)
                    DbUtil.Db.SetUserPreference("PageSize", value);
            }
        }
        private bool? _NewAppsOnly;
        public bool NewAppsOnly
        {
            get
            {
                if (_NewAppsOnly.HasValue)
                    return _NewAppsOnly.Value;
                _NewAppsOnly = DbUtil.Db.UserPreference("VBSNewAppsOnly").ToBool2();
                return _NewAppsOnly.Value;
            }
            set
            {
                _NewAppsOnly = value;
                DbUtil.Db.SetUserPreference("VBSNewAppsOnly", value);
            }
        }
        private string _Grade;
        public string Grade
        {
            get
            {
                if (_Grade != null)
                    return _Grade;
                _Grade = DbUtil.Db.UserPreference("VBSGrade");
                return _Grade;
            }
            set
            {
                _Grade = value;
                DbUtil.Db.SetUserPreference("VBSGrade", value);
            }
        }
        private string _UserInfo;
        public string UserInfo
        {
            get
            {
                if (_UserInfo != null)
                    return _UserInfo;
                _UserInfo = DbUtil.Db.UserPreference("VBSUserInfo");
                return _UserInfo;
            }
            set
            {
                _UserInfo = value;
                DbUtil.Db.SetUserPreference("VBSUserInfo", value);
            }
        }

        public VBSInfo FetchVBSInfo(int? id)
        {
            var q = from v in DbUtil.Db.VBSApps
                    where id == v.Id
                    let org = DbUtil.Db.Organizations.SingleOrDefault(o => o.OrganizationId == v.OrgId)
                    select new VBSInfo
                    {
                        Id = v.Id,
                        MemberOurChurch = v.PeopleId == null ? false : v.Person.MemberStatusId == 10,
                        ActiveInAnotherChurch = v.ActiveInAnotherChurch ?? false,
                        PubPhoto = v.PubPhoto ?? false,
                        GradeCompleted = v.GradeCompleted,
                        Gender = v.PeopleId == null ? "" : (v.Person.GenderId == 1 ? "M" : (v.Person.GenderId == 2 ? "F" : "")),
                        Name = v.Person.Name,
                        Request = v.Request,
                        Uploaded = v.Uploaded,
                        OrgId = v.OrgId,
                        OrgName = org != null ? org.OrganizationName : null,
                        DivId = org != null ? (int?)org.DivisionId : null,
                        UserInfo = v.UserInfo
                    };
            return q.SingleOrDefault();
        }
        public void DeleteVBSApp(int id)
        {
            var v = DbUtil.Db.VBSApps.Single(vb => vb.Id == id);
            var img = ImageData.DbUtil.Db.Images.SingleOrDefault(i => i.Id == v.ImgId);
            DbUtil.Db.VBSApps.DeleteOnSubmit(v);
            DbUtil.Db.SubmitChanges();
            if (img != null)
            {
                ImageData.DbUtil.Db.Images.DeleteOnSubmit(img);
                ImageData.DbUtil.Db.SubmitChanges();
            }
        }
        public VBSInfo UpdateVBSApp(int id, int OrgId)
        {
            var q = from vb in DbUtil.Db.VBSApps
                    where vb.Id == id
                    select vb;
            var v = q.Single();
            if (v.OrgId != OrgId)
            {
                if (v.OrgId != null)
                {
                    var qm = from m in DbUtil.Db.OrganizationMembers
                             where m.OrganizationId == v.OrgId && m.PeopleId == v.PeopleId
                             select m;
                    var member = qm.SingleOrDefault();
                    if (member != null)
                        member.Drop();
                    DbUtil.Db.SubmitChanges();
                    OrganizationMember.UpdateMeetingsToUpdate();
                }
                OrganizationMember.InsertOrgMembers(OrgId,
                    v.PeopleId.Value,
                    (int)OrganizationMember.MemberTypeCode.Member,
                    Util.Now,
                    null, false);
                var qme = from m in DbUtil.Db.Meetings
                          where m.OrganizationId == OrgId
                          orderby m.MeetingDate descending
                          select m;
                var meeting = qme.FirstOrDefault();
                if (meeting != null && (Util.Now - meeting.MeetingDate.Value).TotalHours <= 24)
                    Attend.RecordAttendance(v.PeopleId.Value, meeting.MeetingId, true);
            }
            return FetchVBSInfo(id);
        }
        public IEnumerable<VBSInfo> FetchVBSInfo()
        {
            var q = from v in DbUtil.Db.ViewVBSInfos
                    where UserInfo == "0" || v.UserInfo == UserInfo || v.UserInfo == null
                    where Grade == "0" || v.GradeCompleted == Grade || (Grade == "99" && v.GradeCompleted == null)
                    where !NewAppsOnly || v.PeopleId == null
                    select v;
            if (Dir != "desc")
                switch (Sort)
                {
                    case "Name":
                        q = q.OrderBy(v => v.PeopleId == null ? "" : v.Name2);
                        break;
                    case "Member":
                        q = q.OrderBy(v => v.PeopleId == null ? false : v.MemberStatusId == 10);
                        break;
                    case "Grade":
                        q = from v in q
                            orderby v.GradeCompleted,
                                v.OrgName,
                                v.Name2
                            select v;
                        break;
                    case "Gender":
                        q = q.OrderBy(v => v.GenderId);
                        break;
                    case "Class":
                        q = q.OrderBy(v => v.OrgName);
                        break;
                    case "UserInfo":
                        q = q.OrderBy(v => v.OrgId == null ? "" : v.UserInfo);
                        break;
                    case "App Date":
                    default:
                        q = q.OrderBy(v => v.Uploaded);
                        break;
                }
            else // descending
                switch (Sort)
                {
                    case "Name":
                        q = q.OrderByDescending(v => v.Name2);
                        break;
                    case "Member":
                        q = q.OrderByDescending(v => v.PeopleId != null ? v.MemberStatusId == 10 : false);
                        break;
                    case "Grade":
                        q = q.OrderByDescending(v => v.GradeCompleted)
                            .ThenBy(v => v.OrgName)
                            .ThenBy(v => v.Name2);
                        break;
                    case "Gender":
                        q = q.OrderByDescending(v => v.GenderId);
                        break;
                    case "Class":
                        q = q.OrderByDescending(v => v.OrgName)
                            .ThenBy(v => v.Name2);
                        break;
                    case "UserInfo":
                        q = q.OrderByDescending(v => v.OrgId == null ? "" : v.UserInfo);
                        break;
                    case "App Date":
                        q = q.OrderByDescending(v => v.Uploaded);
                        break;
                }
            return from v in q
                   select new VBSInfo
                   {
                       Id = v.Id,
                       MemberOurChurch = v.PeopleId == null ? false : v.MemberStatusId == 10,
                       ActiveInAnotherChurch = v.ActiveInAnotherChurch ?? false,
                       PubPhoto = v.PubPhoto ?? false,
                       GradeCompleted = v.GradeCompleted,
                       Gender = v.PeopleId == null ? "" : (v.GenderId == 1 ? "M" : (v.GenderId == 2 ? "F" : "")),
                       Name = v.Name,
                       PeopleId = v.PeopleId,
                       Request = v.Request,
                       Uploaded = v.Uploaded,
                       DivId = v.DivId ?? 0,
                       OrgId = v.OrgId ?? 0,
                       OrgName = v.OrgName ?? "select class",
                       UserInfo = v.UserInfo
                   };
        }

        public IEnumerable<SelectListItem> FetchUserInfos()
        {
            var q = from v in DbUtil.Db.VBSApps
                    where v.UserInfo != null && v.UserInfo != ""
                    group v by v.UserInfo into g
                    orderby g.Key
                    select new SelectListItem
                    {
                        Text = g.Key.ToString()
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "0" });
            return list;
        }
        public IEnumerable<SelectListItem> FetchClasses()
        {
            var q = from v in DbUtil.Db.VBSApps
                    where v.GradeCompleted != null && v.GradeCompleted != ""
                    group v by v.GradeCompleted into g
                    orderby g.Key
                    select new SelectListItem
                    {
                        Text = g.Key.ToString()
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "0" });
            list.Add(new SelectListItem { Text = "Blank", Value = "99" });
            return list;
        }
        public IEnumerable<SelectListItem> FetchDivisions()
        {
            var q = from d in DbUtil.Db.Divisions
                    where d.Program.Name == "Vacation Bible School"
                    orderby d.Name
                    select new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    };
            return q;
        }
        public IEnumerable<SelectListItem> FetchOrganizations(int divid, int orgid)
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(d => d.DivId == divid)
                    orderby o.OrganizationName
                    select new SelectListItem
                    {
                        Value = o.OrganizationId.ToString(),
                        Text = o.OrganizationName,
                        Selected = o.OrganizationId == orgid
                    };
            return q;
        }
    }
    public class VBSInfo
    {
        public string Name { get; set; }
        public int? PeopleId { get; set; }
        public int Id { get; set; }
        public bool PubPhoto { get; set; }
        public bool MemberOurChurch { get; set; }
        public bool ActiveInAnotherChurch { get; set; }
        public string GradeCompleted { get; set; }
        public string Gender { get; set; }
        public string Request { get; set; }
        public DateTime? Uploaded { get; set; }
        public string OrgName { get; set; }
        public int? OrgId { get; set; }
        public int? DivId { get; set; }
        public string UserInfo { get; set; }
    }
}
