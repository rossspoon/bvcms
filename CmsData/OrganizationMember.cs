/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;
using System.Web;

namespace CmsData
{
    public partial class OrganizationMember
    {
        private const string STR_MeetingsToUpdate = "MeetingsToUpdate";
        public enum MemberTypeCode
        {
            Teacher = 160,
            Member = 220,
            InActive = 230,
            VisitingMember = 300,
            Visitor = 310,
            InServiceMember = 500,
            VIP = 700,
            Drop = -1,
        }
        public MemberTypeCode MemberTypeEnum
        {
            get { return (MemberTypeCode)MemberTypeId; }
            set { MemberTypeId = (int)value; }
        }
        private CMSDataContext _Db;
        public CMSDataContext Db
        {
            get
            {
                if (_Db == null)
                    _Db = this.GetDataContext() as CMSDataContext;
                return _Db;
            }
        }
        public void Drop()
        {
            var q = from o in Db.Organizations
                    where o.OrganizationId == OrganizationId
                    let count = Db.Attends.Count(a => a.PeopleId == PeopleId 
                        && a.OrganizationId == OrganizationId 
                        && a.MeetingDate < DateTime.Today)
                    select new
                    {
                        FirstMeetingDt = o.FirstMeetingDate,
                        AttendCount = count,
                        TrackAttendance = o.AttendTrkLevelId == 20
                    };
            var i = q.Single();
            if ((i.FirstMeetingDt ?? DateTime.MinValue) > Util.Now 
                || (i.TrackAttendance && i.AttendCount == 0))
            {
                var qt = from et in Db.EnrollmentTransactions
                         where et.PeopleId == PeopleId && et.OrganizationId == OrganizationId
                         select et;
                Db.EnrollmentTransactions.DeleteAllOnSubmit(qt);
                var qa = from et in Db.Attends
                         where et.PeopleId == PeopleId && et.OrganizationId == OrganizationId
                         select et;
                var smids = HttpContext.Current.Items[STR_MeetingsToUpdate] as List<int>;
                var mids = qa.Select(a => a.MeetingId).ToList();
                if (smids != null)
                    smids.AddRange(mids);
                else
                    HttpContext.Current.Items[STR_MeetingsToUpdate] = mids;
                Db.Attends.DeleteAllOnSubmit(qa);
            }
            else
            {
                var et = new EnrollmentTransaction
                {
                    OrganizationId = OrganizationId,
                    PeopleId = PeopleId,
                    MemberTypeId = MemberTypeId,
                    OrganizationName = Organization.OrganizationName,
                    TransactionDate = Util.Now,
                    TransactionTypeId = 5, // drop
                    CreatedBy = Util.UserId1,
                    CreatedDate = Util.Now,
                    VipWeek1 = VipWeek1,
                    VipWeek2 = VipWeek2,
                    VipWeek3 = VipWeek3,
                    VipWeek4 = VipWeek4,
                    VipWeek5 = VipWeek5,
                    Pending = Pending
                };
                Db.EnrollmentTransactions.InsertOnSubmit(et);
            }
            Db.OrgMemMemTags.DeleteAllOnSubmit(this.OrgMemMemTags);
            Db.OrganizationMembers.DeleteOnSubmit(this);

        }
        public static void UpdateMeetingsToUpdate()
        {
            var mids = HttpContext.Current.Items[STR_MeetingsToUpdate] as List<int>;
            if (mids != null)
                foreach (var mid in mids)
                    DbUtil.Db.UpdateMeetingCounters(mid);
        }
        public bool ToggleGroup(int groupid)
        {
            var group = OrgMemMemTags.SingleOrDefault(g => 
                g.OrgId == OrganizationId && g.PeopleId == PeopleId && g.MemberTagId == groupid);
            if (group == null)
            {
                OrgMemMemTags.Add(new OrgMemMemTag { MemberTagId = groupid });
                return true;
            }
            OrgMemMemTags.Remove(group);
            Db.OrgMemMemTags.DeleteOnSubmit(group);
            return false;
        }
    }
}
