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
using System.Linq.Expressions;
using UtilityExtensions;
using System.Configuration;
using System.Reflection;
using System.Collections;
using System.Data.Linq.SqlClient;
using System.Text.RegularExpressions;
using System.Web;

namespace CmsData
{
    internal static class Expressions
    {
        internal static Expression MemberTypeIds(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int org,
            int sched,
            CompareType op,
            params int[] ids)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.OrganizationMembers.Any(m =>
                    ids.Contains(m.MemberTypeId)
                    && (m.OrganizationId == org || org == 0)
                    && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                    && (m.Organization.ScheduleId == sched || sched == 0)
                    );
            Expression expr = Expression.Invoke(pred, parm); // substitute parm for p
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression MembOfOrgWithSched(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int org,
            CompareType op,
            params int[] ids)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.OrganizationMembers.Any(m =>
                    ids.Contains(m.Organization.ScheduleId.Value)
                    && (m.OrganizationId == org || org == 0)
                    && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                    );
            Expression expr = Expression.Invoke(pred, parm); // substitute parm for p
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression WasMemberAsOf1(ParameterExpression parm,
            CMSDataContext Db,
            DateTime? from,
            DateTime? to,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            bool tf)
        {
            to = to.HasValue ? to.Value.AddDays(1) : from.Value.AddDays(1);
            Expression<Func<Person, bool>> pred = p =>
                Db.MembersAsOf(from, to, progid, divid, org).Select(m => m.PeopleId).Contains(p.PeopleId);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression WasMemberAsOf(
            ParameterExpression parm, CMSDataContext Db,
            DateTime? startdt,
            DateTime? enddt,
            int? progid,
            int? divid,
            int? orgid,
            CompareType op,
            bool tf)
        {
            enddt = enddt.HasValue ? enddt.Value.AddDays(1) : startdt.Value.AddDays(1);
            Expression<Func<Person, bool>> pred = p =>
                p.EnrollmentTransactions.Any(et =>
                    et.TransactionTypeId <= 3 // things that start a change
                    && et.TransactionStatus == false
                    && et.TransactionDate <= enddt // transaction starts <= looked for end
                    && (et.Pending ?? false) == false
                    && (et.NextTranChangeDate ?? Util.Now) >= startdt // transaction ends >= looked for start
                    && (et.OrganizationId == orgid || orgid == 0)
                    && (et.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (et.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                    );
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression MemberTypeAsOf(
            ParameterExpression parm,
            DateTime? from,
            DateTime? to,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            params int[] ids)
        {
            to = to.HasValue ? to.Value.AddDays(1) : from.Value.AddDays(1);

            Expression<Func<Person, bool>> pred = p =>
                p.EnrollmentTransactions.Any(et =>
                    et.TransactionTypeId <= 3 // things that start a change
                    && et.TransactionStatus == false
                    && from <= (et.NextTranChangeDate ?? Util.Now) // where it ends
                    && et.TransactionDate <= to // where it begins
                    && (et.Pending ?? false) == false
                    && ids.Contains(et.MemberTypeId)  // what it's type was during that time
                    && (et.OrganizationId == org || org == 0)
                    && (et.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (et.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                    );
            Expression expr = Expression.Invoke(pred, parm);
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression AttendanceTypeAsOf(
            ParameterExpression parm,
            DateTime? from,
            DateTime? to,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            params int[] ids)
        {
            to = to.HasValue ? to.Value.AddDays(1) : from.Value.AddDays(1);
            Expression<Func<Person, bool>> pred = p =>
                p.Attends.Any(a => a.MeetingDate >= from
                    && a.MeetingDate < to
                    && a.AttendanceFlag == true
                    && ids.Contains(a.AttendanceTypeId.Value)
                    && (a.Meeting.OrganizationId == org || org == 0)
                    && (a.Meeting.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (a.Meeting.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                    );
            Expression expr = Expression.Invoke(pred, parm);
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression RecentAttendType(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            int days,
            CompareType op,
            int[] ids)
        {
            var mindt = Util.Now.AddDays(-days).Date;
            Expression<Func<Person, bool>> pred = p =>
                p.Attends.Any(a => a.MeetingDate >= mindt
                    && a.AttendanceFlag == true
                    && ids.Contains(a.AttendanceTypeId.Value)
                    && (a.Meeting.OrganizationId == org || org == 0)
                    && (a.Meeting.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (a.Meeting.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                    );
            Expression expr = Expression.Invoke(pred, parm);
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression HasTaskWithName(
            ParameterExpression parm,
            CompareType op,
            string task)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.TasksAboutPerson.Any(t => t.Description.Contains(task)
                    && t.StatusId != (int)Task.StatusCode.Complete);
            Expression expr = Expression.Invoke(pred, parm);
            if (op == CompareType.NotEqual)
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression RecentContactType(
            ParameterExpression parm,
            int days,
            CompareType op,
            int[] ids)
        {
            var mindt = Util.Now.AddDays(-days).Date;
            Expression<Func<Person, bool>> pred = p =>
                p.contactsHad.Any(a => a.contact.ContactDate >= mindt
                    && ids.Contains(a.contact.ContactTypeId));
            Expression expr = Expression.Invoke(pred, parm);
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression RecentContactMinistry(
            ParameterExpression parm,
            int days,
            CompareType op,
            int[] ids)
        {
            var mindt = Util.Now.AddDays(-days).Date;
            Expression<Func<Person, bool>> pred = p =>
                p.contactsHad.Any(a => a.contact.ContactDate >= mindt
                    && ids.Contains(a.contact.MinistryId.Value));
            Expression expr = Expression.Invoke(pred, parm);
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression MeetingId(
            ParameterExpression parm,
            CompareType op,
            int id)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.Attends.Any(a =>
                    (a.AttendanceFlag == true || a.Registered == true)
                    && a.MeetingId == id
                    );
            Expression expr = Expression.Invoke(pred, parm);
            if (op == CompareType.NotEqual)
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression PeopleExtra(
            ParameterExpression parm,
            CompareType op,
            string value)
        {
            var a = value.Split(new char[] { ':' }, 2);
            Expression<Func<Person, bool>> pred = p =>
                p.PeopleExtras.Any(e =>
                    e.Field == a[0] && e.StrValue == a[1]);
            Expression expr = Expression.Invoke(pred, parm);
            if (op == CompareType.NotEqual)
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression PeopleExtraDate(
            ParameterExpression parm,
            CompareType op,
            string value)
        {
            var a = value.Split(new char[] { ':' }, 2);
            var dt = DateTime.Parse(a[1]);
            Expression<Func<Person, DateTime>> pred = p =>
                p.PeopleExtras.SingleOrDefault(e =>
                    e.Field == a[0]).DateValue.Value;
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(dt), left.Type);
            return Compare(left, op, right);
        }
        internal static Expression RecentAttendCount(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            int days,
            CompareType op,
            int cnt)
        {
            var mindt = Util.Now.AddDays(-days).Date;
            Expression<Func<Person, int>> pred = p =>
                p.Attends.Count(a => a.AttendanceFlag == true
                    && a.MeetingDate >= mindt
                    && (a.Meeting.OrganizationId == org || org == 0)
                    && (a.Meeting.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (a.Meeting.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                    );
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(cnt), left.Type);
            return Compare(left, op, right);
        }
        internal static Expression KidsRecentAttendCount(
            ParameterExpression parm,
            int days,
            CompareType op,
            int cnt)
        {
            var mindt = Util.Now.AddDays(-days).Date;
            Expression<Func<Person, int>> pred = p =>
                p.Family.People.Where(k => k.PositionInFamilyId == 30).Max(k =>
                    k.Attends.Count(a => a.AttendanceFlag == true && a.MeetingDate >= mindt));
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(cnt), left.Type);
            return Compare(left, op, right);
        }
        internal static Expression RecentContributionCount(
            ParameterExpression parm,
            int days,
            CompareType op,
            int cnt)
        {
            int[] ReturnedReversedTypes = new int[] 
            { 
                (int)Contribution.TypeCode.ReturnedCheck, 
                (int)Contribution.TypeCode.Reversed 
            };
            var mindt = Util.Now.AddDays(-days).Date;
            Expression<Func<Person, int>> pred = p =>
                p.Contributions.Count(c => c.ContributionDate >= mindt
                    && (p.ContributionOptionsId != (int)Person.EnvelopeOptionCode.Joint
                        || (p.Family.HeadOfHouseholdId == p.PeopleId 
                            && p.ContributionOptionsId == (int)Person.EnvelopeOptionCode.Joint))
                    && c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
                    && !ReturnedReversedTypes.Contains(c.ContributionTypeId)
                );
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(cnt), left.Type);
            if (HttpContext.Current.User.IsInRole("Finance"))
                return Compare(left, op, right);
            return Compare(right, CompareType.NotEqual, right);
        }
        internal static Expression RecentContributionAmount(
            ParameterExpression parm,
            int days,
            CompareType op,
            decimal amt)
        {
            int[] ReturnedReversedTypes = new int[] 
            { 
                (int)Contribution.TypeCode.ReturnedCheck, 
                (int)Contribution.TypeCode.Reversed 
            };
            var mindt = Util.Now.AddDays(-days).Date;
            Expression<Func<Person, decimal?>> pred = p =>
                p.Contributions.Where(c => c.ContributionDate >= mindt
                    && (p.ContributionOptionsId != (int)Person.EnvelopeOptionCode.Joint
                        || (p.Family.HeadOfHouseholdId == p.PeopleId
                            && p.ContributionOptionsId == (int)Person.EnvelopeOptionCode.Joint))
                    && c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
                    && !ReturnedReversedTypes.Contains(c.ContributionTypeId)
                ).Sum(c => c.ContributionAmount);
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(amt), left.Type);
            if (HttpContext.Current.User.IsInRole("Finance"))
                return Compare(left, op, right);
            return Compare(right, CompareType.NotEqual, right);
        }
        internal static Expression NumberOfFamilyMembers(
            ParameterExpression parm,
            CompareType op,
            int cnt)
        {
            Expression<Func<Person, int>> pred = p => p.Family.People.Count();
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(cnt), left.Type);
            return Compare(left, op, right);
        }
        internal static Expression NumberOfMemberships(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            int? sched,
            CompareType op,
            int cnt)
        {
            Expression<Func<Person, int>> pred = p =>
                p.OrganizationMembers.Count(m =>
                    (m.OrganizationId == org || org == 0)
                    && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                    && (m.Organization.ScheduleId == sched || sched == 0)
                    );
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(cnt), left.Type);
            return Compare(left, op, right);
        }
        internal static Expression VisitedCurrentOrg(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            var mindt = Util.Now.AddDays(-Util.VisitLookbackDays).Date;
            var ids = new int[] 
            { 
                (int)Attend.AttendTypeCode.NewVisitor, 
                (int)Attend.AttendTypeCode.RecentVisitor, 
                (int)Attend.AttendTypeCode.VisitingMember 
            };
            Expression<Func<Person, bool>> pred = p =>
                p.Attends.Any(a =>
                    a.AttendanceFlag == true
                    && a.MeetingDate >= mindt
                    && (a.MeetingDate >= a.Organization.FirstMeetingDate || a.Organization.FirstMeetingDate == null)
                    && ids.Contains(a.AttendanceTypeId.Value)
                    && a.Meeting.OrganizationId == Util.CurrentOrgId
                    )
                && !p.OrganizationMembers.Any(m => m.OrganizationId == Util.CurrentOrgId
                    && (m.Pending ?? false) == false);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression AttendMemberTypeAsOf(
            ParameterExpression parm, CMSDataContext Db,
            DateTime? from,
            DateTime? to,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            string ids)
        {
            to = to.HasValue ? to.Value.AddDays(1) : from.Value.AddDays(1);
            Expression<Func<Person, bool>> pred = p =>
                Db.AttendMemberTypeAsOf(from, to, progid, divid, org,
                    op == CompareType.NotEqual || op == CompareType.NotOneOf,
                    ids).Select(a => a.PeopleId).Contains(p.PeopleId);
            Expression expr = Expression.Invoke(pred, parm);
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression RecentAttendMemberType(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            int days,
            CompareType op,
            int[] ids)
        {
            var mindt = Util.Now.AddDays(-days).Date;
            Expression<Func<Person, bool>> pred = p =>
                p.Attends.Any(a => a.AttendanceFlag == true
                    && a.MeetingDate >= mindt
                    && ids.Contains(a.MemberTypeId)
                    && (a.Meeting.OrganizationId == org || org == 0)
                    && (a.Meeting.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (a.Meeting.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                    );
            Expression expr = Expression.Invoke(pred, parm);
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression OrgMemberCreatedDate(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            DateTime? date)
        {
            Expression<Func<Person, DateTime?>> pred = p =>
                p.OrganizationMembers.Where(m =>
                    (m.OrganizationId == org || org == 0)
                    && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0))
                .First().CreatedDate.Value.Date;
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Constant(date, typeof(DateTime?));
            return Compare(left, op, right);
        }
        internal static Expression CreatedBy(
            ParameterExpression parm, CMSDataContext Db,
            CompareType op,
            string name)
        {
            Expression<Func<Person, string>> pred = p =>
                Db.People.FirstOrDefault(u => u.Users.Any(u2 => u2.UserId == p.CreatedBy)).Name2;
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Constant(name, typeof(string));
            return Compare(left, op, right);
        }
        internal static Expression SmallGroup(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            string name)
        {
            Expression<Func<Person, bool>> pred1 = p =>
                    p.OrganizationMembers.Any(m =>
                        (m.OrganizationId == org || org == 0)
                        && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                        && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0));
            var expr1 = Expression.Convert(Expression.Invoke(pred1, parm), typeof(bool));
            if (name.HasValue())
            {
                Expression<Func<Person, bool>> pred = p =>
                        p.OrganizationMembers.Any(m =>
                            (m.OrganizationId == org || org == 0)
                            && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                            && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                            && m.OrgMemMemTags.Any(mt => mt.MemberTag.Name.Contains(name)));
                var expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
                if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                    expr = Expression.Not(expr);
                return Expression.And(expr1, expr);
            }
            else
            {
                Expression<Func<Person, bool>> pred = p =>
                        p.OrganizationMembers.Any(m =>
                            (m.OrganizationId == org || org == 0)
                            && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                            && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                            && m.OrgMemMemTags.Count() == 0);
                var expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
                if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                    expr = Expression.Not(expr);
                return Expression.And(expr1, expr);
            }
        }

        internal static Expression OrgJoinDateCompare(
           ParameterExpression parm,
           int? progid,
           int? divid,
           int? org,
           CompareType op,
           string prop2)
        {
            Expression<Func<Person, DateTime?>> pred = p =>
                p.OrganizationMembers.Where(m =>
                    (m.OrganizationId == org || org == 0)
                    && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0))
                .Min(m => m.EnrollmentDate);
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Property(parm, prop2);
            return Compare(left, op, right);
        }

        internal static Expression OrgJoinDate(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            DateTime? date)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.OrganizationMembers.Any(m =>
                    (m.OrganizationId == org || org == 0)
                    && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                    && m.EnrollmentDate.Value.Date == date);
            Expression expr = Expression.Invoke(pred, parm); // substitute parm for p
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression OrgJoinDateDaysAgo(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            int days)
        {
            Expression<Func<Person, int?>> pred = p =>
                SqlMethods.DateDiffDay(p.OrganizationMembers.Where(m =>
                    (m.OrganizationId == org || org == 0)
                    && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0))
                .Min(m => m.EnrollmentDate), Util.Now);
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Constant(days, typeof(int?));
            return Compare(left, op, right);
        }
        internal static Expression Birthday(
            ParameterExpression parm,
            CompareType op,
            string dob)
        {
            Expression<Func<Person, bool>> pred = p => false; // default
            DateTime dt;
            if (DateTime.TryParse(dob, out dt))
                if (Regex.IsMatch(dob, @"\d+/\d+/\d+"))
                    pred = p => p.BirthDay == dt.Day && p.BirthMonth == dt.Month && p.BirthYear == dt.Year;
                else
                    pred = p => p.BirthDay == dt.Day && p.BirthMonth == dt.Month;
            else
            {
                int y;
                if (int.TryParse(dob, out y))
                    if (y <= 12 && y > 0)
                        pred = p => p.BirthMonth == y;
                    else
                        pred = p => p.BirthYear == y;
            }
            Expression expr = Expression.Invoke(pred, parm); // substitute parm for p
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression WeddingDate(
            ParameterExpression parm,
            CompareType op,
            string wed)
        {
            Expression<Func<Person, bool>> pred = p => false; // default
            DateTime dt;
            if (DateTime.TryParse(wed, out dt))
                if (Regex.IsMatch(wed, @"\d+/\d+/\d+"))
                    pred = p => p.WeddingDate == dt;
                else
                    pred = p => p.WeddingDate.Value.Day == dt.Day && p.WeddingDate.Value.Month == dt.Month;
            else
            {
                int y;
                if (int.TryParse(wed, out y))
                    if (y <= 12 && y > 0)
                        pred = p => p.WeddingDate.Value.Month == y;
                    else
                        pred = p => p.WeddingDate.Value.Year == y;
            }
            Expression expr = Expression.Invoke(pred, parm); // substitute parm for p
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression OrgInactiveDate(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            DateTime? date)
        {
            Expression<Func<Person, DateTime?>> pred = p =>
                p.OrganizationMembers.Where(m =>
                    (m.OrganizationId == org || org == 0)
                    && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0))
                .First().InactiveDate;
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Constant(date, typeof(DateTime?));
            return Compare(left, op, right);
        }
        internal static Expression WidowedDate(
            ParameterExpression parm, CMSDataContext Db,
            CompareType op,
            DateTime? date)
        {
            Expression<Func<Person, DateTime?>> pred = p =>
                Db.WidowedDate(p.PeopleId);
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Constant(date, typeof(DateTime?));
            return Compare(left, op, right);
        }
        internal static Expression DaysTillBirthday(
            ParameterExpression parm, CMSDataContext Db,
            CompareType op,
            int days)
        {
            Expression<Func<Person, int?>> pred = p =>
                SqlMethods.DateDiffDay(Util.Now.Date, Db.NextBirthday(p.PeopleId));
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Constant(days, typeof(int?));
            return Compare(left, op, right);
        }
        internal static Expression DaysSinceContact(
            ParameterExpression parm,
            CompareType op,
            int days)
        {
            Expression<Func<Person, bool>> hadcontact = p => p.contactsHad.Count() > 0;
            var dt = Util.Now.Date;
            Expression<Func<Person, int?>> pred = p =>
                SqlMethods.DateDiffDay(p.LastContact, dt);
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Constant(days, typeof(int?));
            var expr1 = Expression.Invoke(hadcontact, parm);
            return Expression.And(expr1, Compare(left, op, right));
        }
        internal static Expression HasContacts(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p => p.contactsHad.Count() > 0;
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression AttendPct(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            decimal pct)
        {
            Expression<Func<Person, decimal>> pred = p =>
                p.OrganizationMembers.Where(om =>
                    (om.OrganizationId == org || org == 0)
                    && (om.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (om.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0))
                    .Average(om => om.AttendPct).Value;

            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(pct), left.Type);
            return Compare(left, op, right);
        }

        internal static Expression AttendCntHistory(
            ParameterExpression parm, CMSDataContext Db,
            int? progid,
            int? divid,
            int? org,
            DateTime? start,
            DateTime? end,
            CompareType op,
            int cnt)
        {
            var memb = WasMemberAsOf(parm, Db, start, end, progid, divid, org, CompareType.Equal, true);

            Expression<Func<Person, int>> pred = p =>

                p.Attends.Count(a =>
                    a.AttendanceFlag == true
                    && a.MeetingDate >= start && a.MeetingDate <= end
                    && (a.Meeting.OrganizationId == org || org == 0)
                    && (Db.Organizations.Single(o => o.OrganizationId == a.Meeting.OrganizationId)
                        .DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (Db.Organizations.Single(o => o.OrganizationId == a.Meeting.OrganizationId)
                        .DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0));

            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(cnt), left.Type);
            return Expression.And(memb, Compare(left, op, right));
        }
        internal static Expression AttendPctHistory(
           ParameterExpression parm, CMSDataContext Db,
           int? progid,
           int? divid,
           int? org,
           DateTime? start,
           DateTime? end,
           CompareType op,
           decimal pct)
        {
            var memb = WasMemberAsOf(parm, Db, start, end, progid, divid, org, CompareType.Equal, true);

            Expression<Func<Person, double>> pred = p =>

                p.Attends.Count(a =>
                    a.EffAttendFlag != null
                    && a.MeetingDate >= start && a.MeetingDate <= end
                    && (a.OrganizationId == org || org == 0)
                    && (Db.Organizations.Single(o => o.OrganizationId == a.OrganizationId)
                        .DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (Db.Organizations.Single(o => o.OrganizationId == a.OrganizationId)
                        .DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0))

                    == 0 ? 0 :

                p.Attends.Count(a =>
                    a.EffAttendFlag == true
                    && a.MeetingDate >= start && a.MeetingDate <= end
                    && (a.OrganizationId == org || org == 0)
                    && (Db.Organizations.Single(o => o.OrganizationId == a.OrganizationId)
                        .DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (Db.Organizations.Single(o => o.OrganizationId == a.OrganizationId)
                        .DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0))

                    * 100.0 /

                p.Attends.Count(a =>
                    a.EffAttendFlag != null
                    && a.MeetingDate >= start && a.MeetingDate <= end
                    && (a.OrganizationId == org || org == 0)
                    && (Db.Organizations.Single(o => o.OrganizationId == a.OrganizationId)
                        .DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (Db.Organizations.Single(o => o.OrganizationId == a.OrganizationId)
                        .DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0));

            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(pct), left.Type);
            return Expression.And(memb, Compare(left, op, right));
        }

        internal static Expression IsMemberOf(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.OrganizationMembers.Any(m =>
                    m.MemberTypeId != (int)OrganizationMember.MemberTypeCode.InActive
                    && (m.OrganizationId == org || org == 0)
                    && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0)
                    && (m.Pending ?? false) == false);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression IsInactiveMemberOf(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.OrganizationMembers.Any(m =>
                    m.MemberTypeId == (int)OrganizationMember.MemberTypeCode.InActive
                    && (m.OrganizationId == org || org == 0)
                    && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0));
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression IsPendingMemberOf(
            ParameterExpression parm,
            int? progid,
            int? divid,
            int? org,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.OrganizationMembers.Any(m =>
                    (m.Pending ?? false) == true
                    && (m.OrganizationId == org || org == 0)
                    && (m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0)
                    && (m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0));
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression InBFClass(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.OrganizationMembers.Any(m =>
                    m.Organization.DivOrgs.Any(t => t.Division.ProgId == DbUtil.BFClassOrgTagId)
                    && (m.Pending ?? false) == false);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression RecActiveOtherChurch(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> hasapp = p => p.RecRegs.Count() > 0;
            Expression<Func<Person, bool>> pred = p =>
                    p.RecRegs.Any(v => v.ActiveInAnotherChurch == true)
                    && p.RecRegs.Count() > 0;
            Expression expr1 = Expression.Convert(Expression.Invoke(hasapp, parm), typeof(bool));
            Expression expr2 = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr2 = Expression.Not(expr2);
            return Expression.And(expr1, expr2);
        }
        internal static Expression FamilyHasChildren(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.Family.People.Any(m => m.Age <= 12);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression FamilyHasChildrenAged(
            ParameterExpression parm,
            int age,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.Family.People.Any(m => m.Age <= age);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression HasRelatedFamily(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.Family.RelatedFamilies1.Count() > 0
                || p.Family.RelatedFamilies2.Count() > 0;
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression IsHeadOfHousehold(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.Family.HeadOfHouseholdId == p.PeopleId;
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression HasLowerName(CMSDataContext Db,
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    Db.StartsLower(p.FirstName).Value
                    || Db.StartsLower(p.LastName).Value;
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression IsUser(
           ParameterExpression parm,
           CompareType op,
           bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.Users.Count() > 0;
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression HasPicture(ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p => p.PictureId != null;
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression HasCurrentTag(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.Tags.Any(t => t.Tag.Name == Util.CurrentTagName && t.Tag.PeopleId == Util.CurrentTagOwnerId);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression InCurrentOrg(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.OrganizationMembers.Any(m =>
                        m.OrganizationId == Util.CurrentOrgId
                        && (m.OrgMemMemTags.Any(mt => mt.MemberTagId == Util.CurrentGroupId) || Util.CurrentGroupId <= 0)
                        && (m.OrgMemMemTags.Count() == 0 || Util.CurrentGroupId != -1)
                        && m.MemberTypeId != (int)OrganizationMember.MemberTypeCode.InActive
                        && (m.Pending ?? false) == false);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression InactiveCurrentOrg(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.OrganizationMembers.Any(m =>
                        m.OrganizationId == Util.CurrentOrgId
                        && m.MemberTypeId == (int)OrganizationMember.MemberTypeCode.InActive);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression PendingCurrentOrg(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.OrganizationMembers.Any(m =>
                        m.OrganizationId == Util.CurrentOrgId
                        && (m.Pending ?? false) == true);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression PreviousCurrentOrg(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.EnrollmentTransactions.Any(m =>
                        m.OrganizationId == Util.CurrentOrgId
                        && m.TransactionTypeId > 3
                        && m.TransactionStatus == false
                        && (m.Pending ?? false) == false)
                    && !p.OrganizationMembers.Any(m =>
                        m.OrganizationId == Util.CurrentOrgId
                        && (m.Pending ?? false) == false);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression IsCurrentPerson(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.PeopleId == Util.CurrentPeopleId;
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression HasMyTag(ParameterExpression parm,
            string tag,
            CompareType op,
            bool tf)
        {
            var a = tag.Split(';').Select(s => s.Split(',')[0].ToInt()).ToArray();
            Expression<Func<Person, bool>> pred = p =>
                p.Tags.Any(t => a.Contains(t.Id));
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression HasVolunteered(ParameterExpression parm,
            string View,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred;
            if (View == "ns")
                pred = p => p.VolInterestInterestCodes.Count() > 0;
            else
            {
                var orgkeys = Person.OrgKeys(View);
                pred = p =>
                      p.VolInterestInterestCodes.Any(vi => orgkeys.Contains(vi.VolInterestCode.Org));
            }
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression VolunteerApprovalCode(ParameterExpression parm,
            CompareType op,
            int[] ids)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.Volunteers.Any(v =>
                    (!v.Standard && !v.Children && !v.Leader && ids.Contains(0))
                    || (v.Standard && ids.Contains(10))
                    || (v.Children && ids.Contains(20))
                    || (v.Leader && ids.Contains(30)))
                || (p.Volunteers.Count() == 0 && ids.Contains(0));
            Expression expr = Expression.Invoke(pred, parm); // substitute parm for p
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression CampusId(ParameterExpression parm,
            CompareType op,
            int[] ids)
        {
            Expression<Func<Person, bool>> pred = p =>
                ids.Contains(p.CampusId.Value);
            Expression expr = Expression.Invoke(pred, parm); // substitute parm for p
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression VolAppStatusCode(ParameterExpression parm,
            CompareType op,
            int[] ids)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.Volunteers.Any(v => ids.Contains(v.StatusId.Value));
            Expression expr = Expression.Invoke(pred, parm); // substitute parm for p
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression BadET(ParameterExpression parm,
            CompareType op,
            int[] ids)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.BadETs.Any(b => ids.Contains(b.Flag));
            Expression expr = Expression.Invoke(pred, parm); // substitute parm for p
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression VolunteerProcessedDateMonthsAgo(ParameterExpression parm,
            CompareType op,
            int months)
        {
            Expression<Func<Person, int?>> pred = p =>
                SqlMethods.DateDiffMonth(p.Volunteers.Max(v => v.ProcessedDate), Util.Now);
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Constant(months, typeof(int?));
            return Compare(left, op, right);
        }
        internal static Expression HaveVolunteerApplications(ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                    p.Volunteers.Any(v => v.VolunteerForms.Count() > 0);
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            if (!(op == CompareType.Equal && tf))
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression IncludeDeceased(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = null;

            bool include = ((tf && op == CompareType.Equal) || (!tf && op == CompareType.NotEqual));
            if (include)
                pred = p => p.DeceasedDate == null || p.DeceasedDate != null;
            else
                pred = p => p.DeceasedDate == null;
            Expression expr = Expression.Convert(Expression.Invoke(pred, parm), typeof(bool));
            return expr;
        }
        internal static Expression UserRole(
            ParameterExpression parm,
            CompareType op,
            int[] ids)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.Users.Any(u => u.UserRoles.Any(ur => ids.Contains(ur.RoleId)));
            Expression expr = Expression.Invoke(pred, parm);
            if (op == CompareType.NotEqual || op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }
        internal static Expression FamHasPrimAdultChurchMemb(
            ParameterExpression parm,
            CompareType op,
            bool tf)
        {
            Expression<Func<Person, bool>> pred = p =>
                p.Family.People.Any(m =>
                    m.PositionInFamilyId == (int)Family.PositionInFamily.PrimaryAdult
                    && m.MemberStatusId == 10 // church member
                    //&& m.PeopleId != p.PeopleId // someone else in family
                    );
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(tf), left.Type);
            return Compare(left, op, right);
        }
        internal static Expression InOneOfMyOrgs(
            ParameterExpression parm, CMSDataContext Db,
            CompareType op,
            bool tf)
        {
            var uid = Util.UserPeopleId;
            Expression<Func<Person, bool>> pred = p =>
                p.OrganizationMembers.Any(m =>
                    Db.OrganizationMembers.Any(um =>
                        um.OrganizationId == m.OrganizationId && um.PeopleId == uid)
                );
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(tf), left.Type);
            return Compare(left, op, right);
        }
        internal static Expression SavedQuery(ParameterExpression parm,
            CMSDataContext Db,
            string QueryIdDesc,
            CompareType op,
            bool tf)
        {
            var a = QueryIdDesc.Split(':');
            var savedquery = Db.QueryBuilderClauses.SingleOrDefault(q =>
                q.SavedBy == a[0] && q.Description == a[1]);
            var pred = savedquery.Predicate();
            Expression left = Expression.Invoke(pred, parm);
            var right = Expression.Convert(Expression.Constant(tf), left.Type);
            return Compare(left, op, right);
        }
        private static MethodInfo EnumerableContains = null;
        private static Expression CompareContains(ParameterExpression parm,
            string prop,
            CompareType op,
            object a,
            Type arrayType,
            Type itemType)
        {
            var left = Expression.Constant(a, arrayType);
            var right = Expression.Convert(Expression.Property(parm, prop), itemType);
            if (EnumerableContains == null)
                EnumerableContains = typeof(Enumerable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == "Contains");
            var method = EnumerableContains.MakeGenericMethod(itemType);
            Expression expr = Expression.Call(method, new Expression[] { left, right });
            if (op == CompareType.NotOneOf)
                expr = Expression.Not(expr);
            return expr;
        }

        internal static Expression CompareDateConstant(
            ParameterExpression parm,
            string prop,
            CompareType op,
            DateTime? dt)
        {
            Expression left = Expression.Property(parm, prop);
            if (dt.HasValue && dt.Value.Date == dt) // 12:00:00 AM?
            {
                left = Expression.MakeMemberAccess(left, typeof(DateTime?).GetProperty("Value"));
                left = Expression.MakeMemberAccess(left, typeof(DateTime).GetProperty("Date"));
            }
            var right = Expression.Convert(Expression.Constant(dt), typeof(DateTime));
            return Compare(left, op, right);
        }
        internal static Expression CompareConstant(
            ParameterExpression parm,
            string prop,
            CompareType op,
            object value)
        {
            if (value != null)
                if (value.GetType() == typeof(int[])) // use isarray?
                    return CompareContains(parm, prop, op, value, typeof(int[]), typeof(int));
                else if (value.GetType() == typeof(string[]))
                    return CompareContains(parm, prop, op, value, typeof(string[]), typeof(string));
            var left = Expression.Property(parm, prop);
            var right = Expression.Convert(Expression.Constant(value), left.Type);
            return Compare(left, op, right);
        }

        internal static Expression CompareProperty(
            ParameterExpression parm,
            string prop,
            CompareType op,
            string prop2)
        {
            var left = Expression.Property(parm, prop);
            var right = Expression.Property(parm, prop2);
            return Compare(left, op, right);
        }

        private static Expression Compare(
            Expression left,
            CompareType op,
            Expression right)
        {
            Expression expr = null;
            switch (op)
            {
                case CompareType.NotEqual:
                case CompareType.IsNotNull:
                    expr = Expression.NotEqual(left, right);
                    break;
                case CompareType.Equal:
                case CompareType.IsNull:
                    expr = Expression.Equal(left, right);
                    break;
                case CompareType.Greater:
                    expr = Expression.GreaterThan(left, right);
                    break;
                case CompareType.GreaterEqual:
                    expr = Expression.GreaterThanOrEqual(left, right);
                    break;
                case CompareType.Less:
                    expr = Expression.LessThan(left, right);
                    break;
                case CompareType.LessEqual:
                    expr = Expression.LessThanOrEqual(left, right);
                    break;
                case CompareType.DoesNotStartWith:
                case CompareType.StartsWith:
                    expr = Expression.Call(left,
                        typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                        new[] { right });
                    break;
                case CompareType.DoesNotEndWith:
                case CompareType.EndsWith:
                    expr = Expression.Call(left,
                        typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                        new[] { right });
                    break;
                case CompareType.DoesNotContain:
                case CompareType.Contains:
                    expr = Expression.Call(left,
                        typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        new[] { right });
                    break;
                case CompareType.StrGreater:
                case CompareType.StrGreaterEqual:
                case CompareType.StrLess:
                case CompareType.StrLessEqual:
                    expr = Expression.Call(left,
                        typeof(string).GetMethod("CompareTo", new[] { typeof(string) }),
                        new[] { right });
                    break;
            }
            switch (op)
            {
                // now reverse the logic if necessary
                case CompareType.DoesNotEndWith:
                case CompareType.DoesNotContain:
                case CompareType.DoesNotStartWith:
                    expr = Expression.Not(expr);
                    break;
                case CompareType.StrGreater:
                    expr = Expression.GreaterThan(expr, Expression.Constant(0));
                    break;
                case CompareType.StrGreaterEqual:
                    expr = Expression.GreaterThanOrEqual(expr, Expression.Constant(0));
                    break;
                case CompareType.StrLess:
                    expr = Expression.LessThan(expr, Expression.Constant(0));
                    break;
                case CompareType.StrLessEqual:
                    expr = Expression.LessThanOrEqual(expr, Expression.Constant(0));
                    break;
            }
            return expr;
        }
    }
}
/*
-- ================================================
-- Template generated from Template Explorer using:
-- Create Inline Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION WasMemberAsOf 
(	
	@from DATE,
	@to DATE,
	@progid INT,
	@divid INT,
	@orgid INT,
	@tf BIT
)
RETURNS TABLE 
AS
RETURN 

	SELECT PeopleId FROM dbo.People
	WHERE
	EXISTS (
        SELECT NULL
        FROM dbo.EnrollmentTransaction et
        WHERE et.TransactionTypeId <= 3 
			AND @from <= COALESCE(et.NextTranChangeDate,GETDATE()) 
			AND et.TransactionDate <= @to 
			AND EXISTS (
				SELECT NULL
				FROM dbo.Organizations AS o, dbo.DivOrg AS do
				WHERE do.DivId = @divid 
					AND o.OrganizationId = et.OrganizationId
					AND do.OrgId = o.OrganizationId
				 AND (EXISTS(
				SELECT NULL AS [EMPTY]
				FROM dbo.[Organizations] AS [t4]
				CROSS JOIN dbo.[DivOrg] AS [t5]
				INNER JOIN dbo.[Division] AS [t6] ON [t6].[Id] = [t5].[DivId]
				WHERE ([t6].[ProgId] = @p5) AND ([t4].[OrganizationId] = et.[OrganizationId]) AND ([t5].[OrgId] = [t4].[OrganizationId])
				)) AND (et.[PeopleId] = [t0].[PeopleId])
        )
GO
*/