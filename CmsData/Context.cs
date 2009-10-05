/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Data.Linq.SqlClient;
using UtilityExtensions;
using System.Configuration;
using System.Web;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace CmsData
{
    public partial class CMSDataContext
    {
        const string STR_System = "System";

        partial void OnCreated()
        {
            CommandTimeout = 600;
        }
        private string _LogFile;
        public string LogFile
        {
            get
            {
                if (_LogFile == null)
                    _LogFile = DbUtil.Settings("LinqLogFile", null);
                return _LogFile;
            }
        }
        public override void SubmitChanges(System.Data.Linq.ConflictMode failureMode)
        {
            //if (Util.Auditing)
            //{
            //    this.Audit<Person>(p => p.PeopleId);
            //    this.Audit<Organization>(o => o.OrganizationId);
            //    this.Audit<Family>(f => f.FamilyId);
            //    this.Audit<EnrollmentTransaction>(et => et.TransactionId);
            //    this.Audit<NewContact>(c => c.ContactId);

            AuditUtility.ProcessInserts(GetChangeSet().Inserts);
            AuditUtility.ProcessUpdates(GetChangeSet().Updates);
            //}

            //base.SubmitChanges(failureMode);

            int t = Thread.CurrentThread.ManagedThreadId;
            if (LogFile.HasValue())
            {
                this.Log = File.AppendText(LogFile);
                DateTime n = Util.Now;
                this.Log.WriteLine("-->> {0} at {1:d};{2:T}, by {3}", t, n, n, Util.UserName);
            }
            base.SubmitChanges(failureMode);
            if (LogFile.HasValue())
            {
                this.Log.WriteLine("--<< {0}", t);
                this.Log.Close();
                this.Log = null;
            }
        }
        public Person LoadPersonById(int id)
        {
            return this.People.FirstOrDefault(p => p.PeopleId == id);
        }

        public Organization LoadOrganizationById(int id)
        {
            return this.Organizations.FirstOrDefault(o => o.OrganizationId == id);
        }
        private QueryBuilderClause CheckBadQuery(QueryBuilderClause qb)
        {
            if (qb != null && qb.Field == null) // bad query
            {
                DeleteQueryBuilderClauseOnSubmit(qb);
                SubmitChanges();
                return null;
            }
            return qb;
        }
        public QueryBuilderClause LoadQueryById(int? queryid)
        {
            var qb = QueryBuilderClauses.SingleOrDefault(c => c.QueryId == queryid);
            return CheckBadQuery(qb);
        }
        public QueryBuilderClause QueryBuilderScratchPad()
        {
            var qb = LoadQueryById(Util.QueryBuilderScratchPadId);
            if (qb == null)
            {
                qb = QueryBuilderClauses.FirstOrDefault(c => c.SavedBy == Util.UserName
                    && c.Description == Util.ScratchPad);
                qb = CheckBadQuery(qb);
                if (qb == null)
                {
                    qb = QueryBuilderClause.NewGroupClause();
                    qb.Description = Util.ScratchPad;
                    qb.SavedBy = Util.UserName;
                    QueryBuilderClauses.InsertOnSubmit(qb);
                    SubmitChanges();
                }
            }
            Util.QueryBuilderScratchPadId = qb.QueryId;
            return qb;
        }
        public QueryBuilderClause QueryBuilderHasCurrentTag()
        {
            const string STR_HasCurrentTag = "HasCurrentTag";
            var qb = QueryBuilderClauses.FirstOrDefault(c => c.SavedBy == STR_System
                && c.Description == STR_HasCurrentTag);
            if (qb == null)
            {
                qb = QueryBuilderClause.NewGroupClause();
                qb.Description = STR_HasCurrentTag;
                qb.SavedBy = STR_System;
                qb.AddNewClause(QueryType.HasCurrentTag, CompareType.Equal, "1,T");
                QueryBuilderClauses.InsertOnSubmit(qb);
                SubmitChanges();
            }
            return qb;
        }
        public QueryBuilderClause QueryBuilderInCurrentOrg()
        {
            const string STR_InCurrentOrg = "InCurrentOrg";
            var qb = QueryBuilderClauses.FirstOrDefault(c => c.SavedBy == STR_System
                && c.Description == STR_InCurrentOrg);
            if (qb == null)
            {
                qb = QueryBuilderClause.NewGroupClause();
                qb.Description = STR_InCurrentOrg;
                qb.SavedBy = STR_System;
                qb.AddNewClause(QueryType.InCurrentOrg, CompareType.Equal, "1,T");
                QueryBuilderClauses.InsertOnSubmit(qb);
                SubmitChanges();
            }
            return qb;
        }
        public QueryBuilderClause QueryBuilderInactiveCurrentOrg()
        {
            const string STR_InactiveCurrentOrg = "InactiveCurrentOrg";
            var qb = QueryBuilderClauses.FirstOrDefault(c => c.SavedBy == STR_System
                && c.Description == STR_InactiveCurrentOrg);
            if (qb == null)
            {
                qb = QueryBuilderClause.NewGroupClause();
                qb.Description = STR_InactiveCurrentOrg;
                qb.SavedBy = STR_System;
                qb.AddNewClause(QueryType.InactiveCurrentOrg, CompareType.Equal, "1,T");
                QueryBuilderClauses.InsertOnSubmit(qb);
                SubmitChanges();
            }
            return qb;
        }
        public QueryBuilderClause QueryBuilderPendingCurrentOrg()
        {
            const string STR_PendingCurrentOrg = "PendingCurrentOrg";
            var qb = QueryBuilderClauses.FirstOrDefault(c => c.SavedBy == STR_System
                && c.Description == STR_PendingCurrentOrg);
            if (qb == null)
            {
                qb = QueryBuilderClause.NewGroupClause();
                qb.Description = STR_PendingCurrentOrg;
                qb.SavedBy = STR_System;
                qb.AddNewClause(QueryType.PendingCurrentOrg, CompareType.Equal, "1,T");
                QueryBuilderClauses.InsertOnSubmit(qb);
                SubmitChanges();
            }
            return qb;
        }
        public QueryBuilderClause QueryBuilderPreviousCurrentOrg()
        {
            const string STR_PreviousCurrentOrg = "PreviousCurrentOrg";
            var qb = QueryBuilderClauses.FirstOrDefault(c => c.SavedBy == STR_System
                && c.Description == STR_PreviousCurrentOrg);
            if (qb == null)
            {
                qb = QueryBuilderClause.NewGroupClause();
                qb.Description = STR_PreviousCurrentOrg;
                qb.SavedBy = STR_System;
                qb.AddNewClause(QueryType.PreviousCurrentOrg, CompareType.Equal, "1,T");
                QueryBuilderClauses.InsertOnSubmit(qb);
                SubmitChanges();
            }
            return qb;
        }
        public QueryBuilderClause QueryBuilderVisitedCurrentOrg()
        {
            const string STR_VisitedCurrentOrg = "VisitedCurrentOrg";
            var qb = QueryBuilderClauses.FirstOrDefault(c => c.SavedBy == STR_System
                && c.Description == STR_VisitedCurrentOrg);
            if (qb == null)
            {
                qb = QueryBuilderClause.NewGroupClause();
                qb.Description = STR_VisitedCurrentOrg;
                qb.SavedBy = STR_System;
                qb.AddNewClause(QueryType.VisitedCurrentOrg, CompareType.Equal, "1,T");
                QueryBuilderClauses.InsertOnSubmit(qb);
                SubmitChanges();
            }
            return qb;
        }
        public QueryBuilderClause QueryBuilderIsCurrentPerson()
        {
            const string STR_IsCurrentPerson = "IsCurrentPerson";
            var qb = QueryBuilderClauses.FirstOrDefault(c => c.SavedBy == STR_System
                && c.Description == STR_IsCurrentPerson);
            if (qb == null)
            {
                qb = QueryBuilderClause.NewGroupClause();
                qb.Description = STR_IsCurrentPerson;
                qb.SavedBy = STR_System;
                qb.AddNewClause(QueryType.IsCurrentPerson, CompareType.Equal, "1,T");
                QueryBuilderClauses.InsertOnSubmit(qb);
                SubmitChanges();
            }
            return qb;
        }
        public void DeleteQueryBuilderClauseOnSubmit(QueryBuilderClause qb)
        {
            foreach (var c in qb.Clauses)
                DeleteQueryBuilderClauseOnSubmit(c);
            this.QueryBuilderClauses.DeleteOnSubmit(qb);
        }
        public void TagAll(IQueryable<Person> list)
        {
            var tag = TagCurrent();
            TagAll(list, tag);
        }
        public void TagAll(IQueryable<Person> list, Tag tag)
        {
            var q = from p in list
                    where !p.Tags.Any(tp => tp.Id == tag.Id)
                    select p.PeopleId;
            foreach (var id in q)
                tag.PersonTags.Add(new TagPerson { PeopleId = id });
            SubmitChanges();
        }
        public void UnTagAll(IQueryable<Person> list)
        {
            var person = list.FirstOrDefault();
            var tag = TagCurrent();
            var q = from p in list
                    from t in p.Tags
                    where t.Id == tag.Id
                    select t;

            foreach (var t in q)
                TagPeople.DeleteOnSubmit(t);
            SubmitChanges();
        }
        public Tag PopulateSpecialTag(int QueryId, int TagTypeId)
        {
            var Qb = LoadQueryById(QueryId);
            var q = People.Where(Qb.Predicate());
            return PopulateSpecialTag(q, TagTypeId);
        }
        public Tag PopulateSpecialTag(IQueryable<Person> q, int TagTypeId)
        {
            var tag = FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, TagTypeId);
            TagPeople.DeleteAllOnSubmit(tag.PersonTags);
            SubmitChanges();
            TagAll(q, tag);
            return tag;
        }
        public void DePopulateSpecialTag(IQueryable<Person> q, int TagTypeId)
        {
            var tag = FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, TagTypeId);
            TagPeople.DeleteAllOnSubmit(tag.PersonTags);
            SubmitChanges();
        }
        public Tag TagById(int id)
        {
            return Tags.SingleOrDefault(t => t.Id == id);
        }
        public Tag TagCurrent()
        {
            return FetchOrCreateTag(Util.CurrentTagName, Util.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
        }
        public User CurrentUser
        {
            get { return Users.SingleOrDefault(u => u.UserId == Util.UserId); }
        }
        public Person NewPeopleManager
        {
            get { return People.SingleOrDefault(p => p.PeopleId == DbUtil.NewPeopleManagerId); }
        }
        public Tag OrgMembersOnlyTag
        {
            get { return FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_OrgMembersOnly); }
        }

        public Tag FetchOrCreateTag(string tagname, int? OwnerId, int tagtypeid)
        {
            var tag = Tags.FirstOrDefault(t =>
                t.Name == tagname && t.PeopleId == OwnerId && t.TypeId == tagtypeid);
            if (tag == null)
            {
                tag = new Tag { Name = tagname, PeopleId = OwnerId, TypeId = tagtypeid };
                Tags.InsertOnSubmit(tag);
                SubmitChanges();
            }
            return tag;
        }
        public void SetOrgMembersOnly()
        {
            var me = Util.UserPeopleId;

            // members of any of my orgs excluding unshared orgs
            var q = from p in People
                    where p.OrganizationMembers.Any(m =>
                        OrganizationMembers.Any(um =>
                            (um.Organization.SecurityTypeId != 3
                                || um.MemberTypeId == (int)OrganizationMember.MemberTypeCode.Teacher)
                             && um.OrganizationId == m.OrganizationId && um.PeopleId == me))
                    select p;
            var tag = PopulateSpecialTag(q, DbUtil.TagTypeId_OrgMembersOnly);

            // members of my family
            q = from p in People
                where p.FamilyId == CurrentUser.Person.FamilyId
                select p;
            TagAll(q, tag);

            // visitors in the last year to one of my orgs excluding unshared
            var attype = new int[] { 40, 50, 60 };
            var dt = Util.Now.AddYears(-1);
            q = from p in People
                where p.Attends.Any(a =>
                    OrganizationMembers.Any(um =>
                        um.Organization.SecurityTypeId != 3 &&
                        um.OrganizationId == a.Meeting.OrganizationId && um.PeopleId == me)
                    && attype.Contains(a.AttendanceTypeId.Value) && a.MeetingDate > dt)
                select p;
            TagAll(q, tag);

            // people assigned to me in one of my tasks
            q = from p in People
                where p.TasksAboutPerson.Any(at => at.OwnerId == me || at.CoOwnerId == me)
                select p;
            TagAll(q, tag);

            // people I have visited in a contact
            q = from p in People
                where p.contactsHad.Any(c => c.contact.contactsMakers.Any(cm => cm.PeopleId == me))
                select p;
            TagAll(q, tag);
        }
        [Function(Name = "dbo.AddAbsents")]
        public int AddAbsents([Parameter(DbType = "Int")] int? meetid, [Parameter(DbType = "Int")] int? userid)
        {
            var result = this.ExecuteMethodCall(this, (MethodInfo)(MethodInfo.GetCurrentMethod()), meetid, userid);
            return (int)(result.ReturnValue);
        }
        [Function(Name = "dbo.UpdateAttendStr")]
        public int UpdateAttendStr([Parameter(DbType = "Int")] int? orgid, [Parameter(DbType = "Int")] int? pid)
        {
            var result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), orgid, pid);
            return ((int)(result.ReturnValue));
        }
        public class AttendMeetingInfo1
        {
            public AttendMeetingInfo2 info;
            public Attend AttendanceOrg;
            public Attend Attendance;
            public Meeting Meeting;
            public List<Attend> VIPAttendance;
            public OrganizationMember BFCMember;
            public Attend BFCAttendance;
            public Meeting BFCMeeting;
            public int path;
        }
        public class AttendMeetingInfo2
        {
            public int? AttendedElsewhere { get; set; }
            public int? MemberTypeId { get; set; }
            //public bool? IsRegularHour { get; set; }
            //public int? ScheduleId { get; set; }
            //public bool? IsSameHour { get; set; }
            public bool? IsOffSite { get; set; }
            public bool? IsRecentVisitor { get; set; }
            public string Name { get; set; }
            public int? BFClassId { get; set; }
        }

        [Function(Name = "dbo.AttendMeetingInfo")]
        [ResultType(typeof(AttendMeetingInfo2))]
        [ResultType(typeof(Attend))] // Attendance
        [ResultType(typeof(Meeting))] // Meeting Attended
        [ResultType(typeof(Attend))] // VIP Attendance
        [ResultType(typeof(OrganizationMember))] // BFC membership
        [ResultType(typeof(Attend))] // BFC Attendance at same time
        [ResultType(typeof(Meeting))] // BFC Meeting Attended
        public IMultipleResults AttendMeetingInfo(int MeetingId, int PeopleId)
        {
            var result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), MeetingId, PeopleId);
            return (IMultipleResults)result.ReturnValue;
        }
        public AttendMeetingInfo1 AttendMeetingInfo0(int MeetingId, int PeopleId)
        {
            var r = AttendMeetingInfo(MeetingId, PeopleId);
            var o = new AttendMeetingInfo1();
            o.info = r.GetResult<CMSDataContext.AttendMeetingInfo2>().First();
            o.Attendance = r.GetResult<Attend>().FirstOrDefault();
            if (o.Attendance != null)
            {
                o.AttendanceOrg = new Attend
                {
                    AttendanceFlag = o.Attendance.AttendanceFlag,
                    AttendanceTypeId = o.Attendance.AttendanceTypeId,
                    AttendId = o.Attendance.AttendId,
                    CreatedBy = o.Attendance.CreatedBy,
                    CreatedDate = o.Attendance.CreatedDate,
                    EffAttendFlag = o.Attendance.EffAttendFlag,
                    MeetingDate = o.Attendance.MeetingDate,
                    MeetingId = o.Attendance.MeetingId,
                    MemberTypeId = o.Attendance.MemberTypeId,
                    OrganizationId = o.Attendance.OrganizationId,
                    OtherOrgId = o.Attendance.OtherOrgId,
                    PeopleId = o.Attendance.PeopleId,
                };
            }

            o.Meeting = r.GetResult<Meeting>().First();
            o.VIPAttendance = r.GetResult<Attend>().ToList();
            o.BFCMember = r.GetResult<OrganizationMember>().FirstOrDefault();
            o.BFCAttendance = r.GetResult<Attend>().FirstOrDefault();
            o.BFCMeeting = r.GetResult<Meeting>().FirstOrDefault();
            return o;
        }
        public string UserPreference(string pref)
        {
            return UserPreference(pref, string.Empty);
        }
        public string UserPreference(string pref, string def)
        {
            var cp = HttpContext.Current.Session["pref-" + pref];
            if (cp != null)
                return cp.ToString();
            var p = CurrentUser.Preferences.SingleOrDefault(up => up.PreferenceX == pref);
            return p != null ? p.ValueX : def;
        }
        public void SetUserPreference(string pref, object value)
        {
            var p = CurrentUser.Preferences.SingleOrDefault(up => up.PreferenceX == pref);
            if (p != null)
                p.ValueX = value.ToString();
            else
            {
                p = new Preference { UserId = Util.UserId1, PreferenceX = pref, ValueX = value.ToString() };
                DbUtil.Db.Preferences.InsertOnSubmit(p);
            }
            HttpContext.Current.Session["pref-" + pref] = p.ValueX;
            SubmitChanges();
        }
        [Function(Name = "dbo.LinkEnrollmentTransaction")]
        public int LinkEnrollmentTransaction([Parameter(DbType = "Int")] int? tid, [Parameter(DbType = "DateTime")] DateTime? trandt, [Parameter(DbType = "Int")] int? typeid, [Parameter(DbType = "Int")] int? orgid, [Parameter(DbType = "Int")] int? pid)
        {
            var result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), tid, trandt, typeid, orgid, pid);
            return ((int)(result.ReturnValue));
        }
        [Function(Name = "dbo.FlagOddTransaction")]
        public int FlagOddTransaction([Parameter(DbType = "Int")] int? pid, [Parameter(DbType = "Int")] int? orgid)
        {
            var result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), pid, orgid);
            return ((int)(result.ReturnValue));
        }
        [Function(Name = "dbo.PurgePerson")]
        public int PurgePerson([Parameter(DbType = "Int")] int? pid)
        {
            var result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), pid);
            return ((int)(result.ReturnValue));
        }
        [Function(Name = "dbo.PurgeOrganization")]
        public int PurgeOrganization([Parameter(DbType = "Int")] int? oid)
        {
            var result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), oid);
            return ((int)(result.ReturnValue));
        }
        [Function(Name = "dbo.UpdateMeetingCounters")]
        public int UpdateMeetingCounters([Parameter(DbType = "Int")] int? mid)
        {
            var result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), mid);
            return ((int)(result.ReturnValue));
        }
        [Function(Name = "dbo.DeleteSpecialTags")]
        public int DeleteSpecialTags([Parameter(DbType = "Int")] int? pid)
        {
            var result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), pid);
            return ((int)(result.ReturnValue));
        }
        [Function(Name = "dbo.UpdateSchoolGrade")]
        public int UpdateSchoolGrade([Parameter(DbType = "Int")] int? pid)
        {
            var result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), pid);
            return ((int)(result.ReturnValue));

        }
        [Function(Name = "dbo.OneHeadOfHouseholdIsMember", IsComposable = true)]
        public bool? OneHeadOfHouseholdIsMember([Parameter(DbType = "Int")] int? fid)
        {
            return ((bool?)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), fid).ReturnValue));
        }
    }
}
