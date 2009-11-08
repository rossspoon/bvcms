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
using CmsData;
using System.Data.Linq;
using System.ComponentModel;
using UtilityExtensions;
using System.Web;

namespace CMSPresenter
{
    public class AttendenceController
    {
        public IEnumerable<OrganizationInfo> GetOrganizationInfo(int orgid)
        {
            return (new OrganizationController()).GetOrganizationInfo(orgid);
        }

        public IEnumerable<PastAttendeeInfo> PastAttendees(int orgid)
        {
            var Db = DbUtil.Db;
            // get list of people who have attended an event for this organization (including visitors)
            var pids = from a in Db.Attends
                       where a.Meeting.OrganizationId == orgid
                       where a.MeetingDate >= a.Organization.FirstMeetingDate
                       where a.AttendanceFlag == true
                       group a.PeopleId by a.PeopleId into g
                       select g.Key;

            var q = from p in Db.People
                    join pid in pids on p.PeopleId equals pid
                    let attendct = Db.Attends
                                    .Count(a => a.Meeting.OrganizationId == orgid && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                    let lastattend = Db.Attends
                                    .Where(a => a.Meeting.OrganizationId == orgid && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                                    .Max(a => a.MeetingDate)
                    let status = Db.OrganizationMembers
                                    .Count(m => m.PeopleId == p.PeopleId && m.OrganizationId == orgid) == 0 ?
                                    "visitor" : "member"
                    let attendpct = Db.OrganizationMembers
                                    .Where(ap => ap.PeopleId == p.PeopleId && ap.OrganizationId == orgid)
                                    .Max(ap => ap.AttendPct)
                    let attendstr = Db.OrganizationMembers
                                    .Where(astr => astr.PeopleId == p.PeopleId && astr.OrganizationId == orgid)
                                    .Max(astr => astr.AttendStr)
                    orderby status descending, lastattend descending
                    select new PastAttendeeInfo
                    {
                        PeopleId = p.PeopleId,
                        LastName = p.LastName,
                        FirstName = p.PreferredName,
                        Street = p.PrimaryAddress,
                        Birthday = UtilityExtensions.Util.FormatBirthday(p.BirthYear.Value, p.BirthMonth.Value, p.BirthDay.Value),
                        EmailHome = p.EmailAddress,
                        Phone = p.HomePhone.FmtFone(),
                        City = p.PrimaryCity,
                        State = p.PrimaryState,
                        Zip = p.PrimaryZip.FmtZip(),
                        Status = status,
                        AttendCt = attendct,
                        LastAttend = lastattend,
                        AttendPct = attendpct,
                        AttendStr = attendstr.FmtAttendStr()
                    };
            return q;
        }
    }
}
